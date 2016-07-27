using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HanzoProxyClient
{

    public class Client
    {
        private Config _config;
        private bool _initialized = false;
        private string _subid;
        private string _authtoken;
        private static Client _clientInstance;


        private Client()
        {
            // Private Constructor
        }

        private HttpResponseMessage ExecuteGetRequest(string requrl)
        {
            string apisig = Crypt.EncryptString(
                               string.Format("Sub-ID={0}&Auth-Token={1}", _subid, _authtoken),
                               _config.ApiKey
                            );
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-key", _config.ApiKey);
                client.DefaultRequestHeaders.Add("Api-Sig", apisig);
                return client.GetAsync(requrl).Result;
            }
        }

        private HttpResponseMessage ExecutePostRequest(string requrl, string body)
        {
            string apisig = Crypt.EncryptString(
                               string.Format("Sub-ID={0}&Auth-Token={1}", _subid, _authtoken),
                               _config.ApiKey
                            );
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Api-key", _config.ApiKey);
                client.DefaultRequestHeaders.Add("Api-Sig", apisig);
                return client.PostAsync(
                                    requrl,
                                    new StringContent(body, Encoding.UTF8, "application/json")
                                ).Result;
            }
        }

        public static Client GetInstance()
        {
            if (_clientInstance == null)
            {
                _clientInstance = new Client();
            }
            return _clientInstance;
        }

        public void Initialize(string subid, string authtoken, Config config)
        {
            _subid = subid;
            _authtoken = authtoken;
            _config = config;
        }

        public bool IsInitialized()
        {
            return _initialized;
        }

        public JObject GetVmList()
        {
            string apiUrl = string.Empty;
            string u = _config.ProxyServerUrl;
            if (!u.EndsWith("/"))
            {
                u += "/";
            }
            apiUrl = string.Format("{0}api/vm/list?api_version={1}", u, _config.ApiVersion);
            HttpResponseMessage r = ExecuteGetRequest(apiUrl);
            if (r.StatusCode != HttpStatusCode.OK)
            {
                // error
                //Console.WriteLine("api/vm/list failure: {0} {1} \r\n",
                //   (int)r.StatusCode, r.Content.ReadAsStringAsync().Result.ToString());
                throw new Exception(
                            string.Format("api/vm/list failure: {0} {1}",
                                            (int)r.StatusCode,
                                            r.Content.ReadAsStringAsync().Result.ToString())
                    );
            }
            return JObject.Parse(r.Content.ReadAsStringAsync().Result);
        }

        public JObject GetVmListByReource(string resourceGroup)
        {
            string apiUrl = string.Empty;
            string u = _config.ProxyServerUrl;
            if (!u.EndsWith("/"))
            {
                u += "/";
            }
            apiUrl = string.Format("{0}api/vm/list/resource/{1}?api_version={2}", u, resourceGroup, _config.ApiVersion);

            HttpResponseMessage r = ExecuteGetRequest(apiUrl);
            if (r.StatusCode != HttpStatusCode.OK)
            {
                // error
                //Console.WriteLine("api/vm/list/resource failure: {0} {1} \r\n",
                //                (int)r.StatusCode, r.Content.ReadAsStringAsync().Result.ToString());
                throw new Exception(
                         string.Format("api/vm/list/resource failure: {0} {1}",
                                         (int)r.StatusCode,
                                         r.Content.ReadAsStringAsync().Result.ToString())
                             );

            Console.WriteLine("start GetVMList~~~~~~~~~~~~~~~~");
            }
            return JObject.Parse(r.Content.ReadAsStringAsync().Result);
        }


        public JObject ManageVm(string vmname, string resourcegroup, string action)
        {
            List<Dictionary<string, string>> req_entities = new List<Dictionary<string, string>>();
            Dictionary<string, string> e = new Dictionary<string, string>();
            e.Add("name", vmname);
            e.Add("resourcegroup", resourcegroup);
            e.Add("action", action);
            req_entities.Add(e);
            return ManageVms(req_entities);
        }

        public JObject ManageVms(List<Dictionary<string, string>> req_entities)
        {
            string u = _config.ProxyServerUrl;
            if (!u.EndsWith("/"))
            {
                u += "/";
            }
            string apiUrl = string.Format("{0}api/vm/manage?api_version={1}", u, _config.ApiVersion);
            // create body
            string reqbody = JsonConvert.SerializeObject(
                                 new
                                 {
                                     entities = req_entities
                                 }
                     );

            HttpResponseMessage r = ExecutePostRequest(apiUrl, reqbody);
            if (r.StatusCode != HttpStatusCode.OK)
            {
                // error
                //Console.WriteLine("api/vm/manage failure: {0} {1} \r\n",
                //                (int)r.StatusCode, r.Content.ReadAsStringAsync().Result.ToString());
                throw new Exception(
                     string.Format("api/vm/manage failure: {0} {1}",
                                     (int)r.StatusCode,
                                     r.Content.ReadAsStringAsync().Result.ToString())
                         );
            }
            return JObject.Parse(r.Content.ReadAsStringAsync().Result);
        }

        public JObject ScheduleVm(string vmname, string resourcegroup, string action, DateTime triggertime )
        {
            TimeSpan ts = triggertime - new DateTime(1970, 1, 1);
            int sec_since_epoch = (int)ts.TotalSeconds;
            List<Dictionary<string, string>> req_schedules = new List<Dictionary<string, string>>();
            Dictionary<string, string> s = new Dictionary<string, string>();
            s.Add("name", vmname);
            s.Add("resourcegroup", resourcegroup);
            s.Add("action", action);
            s.Add("triggertime", sec_since_epoch.ToString());
            req_schedules.Add(s);
            return ScheduleVms(req_schedules);

        }

        public JObject ScheduleVms(List<Dictionary<string, string>> req_schedules)
        {
            string u = _config.ProxyServerUrl;
            if (!u.EndsWith("/"))
            {
                u += "/";
            }
            string apiUrl = string.Format("{0}api/vm/schedule?api_version={1}", u, _config.ApiVersion);
            string reqbody = JsonConvert.SerializeObject(
                           new
                           {
                               schedules = req_schedules
                           }
                );

            HttpResponseMessage r = ExecutePostRequest(apiUrl, reqbody);
            if (r.StatusCode != HttpStatusCode.OK)
            {
                // error
                //Console.WriteLine("api/vm/schedule failure: {0} {1} \r\n",
                //                (int)r.StatusCode, r.Content.ReadAsStringAsync().Result.ToString());
                throw new Exception(
                     string.Format("api/vm/manage failure: {0} {1}",
                                     (int)r.StatusCode,
                                     r.Content.ReadAsStringAsync().Result.ToString())
                         );
            }
            return JObject.Parse(r.Content.ReadAsStringAsync().Result);
        }

    }
}
