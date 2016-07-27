using System;
using HanzoProxyAPI.Models;
using HanzoProxyAPI.Core;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq; // JObject
using Newtonsoft.Json;
using Microsoft.Rest;
using Microsoft.Rest.Azure;

namespace HanzoProxyAPI.Controllers
{

    public class VirtualMachineController : ApiController
    {

        [HttpGet]
        public object DoTest(string api_version)
        {
            //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            /*
                        var vm1 = new VirtualMachineObject { name = "yoichika-dev1", resourcegroup = "RG-YOICHIKA-DEV" ,status = "VM Running" };
                        var vm2 = new VirtualMachineObject { name = "yoichika-dev2", resourcegroup = "RG-YOICHIKA-DEV", status = "VM Running" };
                        var vm3 = new VirtualMachineObject { name = "yoichika-dev3", resourcegroup = "RG-YOICHIKA-DEV", status = "VM Deallocated" };
                        var vm4 = new VirtualMachineObject { name = "yoichika-dev4", resourcegroup = "RG-YOICHIKA-DEV", status = "VM Deallocated" };
                        List<VirtualMachineObject> vmolist = new List<VirtualMachineObject>();
                        vmolist.Add(vm1);
                        vmolist.Add(vm2);
                        vmolist.Add(vm3);
                        vmolist.Add(vm4);
            */
            Dictionary<string, string> entity_result = new Dictionary<string, string>();
            entity_result.Add("name", "yoichika-dev1");
            entity_result.Add("resourcegroup", "RG-YOICHIKA-DEV");
            entity_result.Add("status", Constants.ApiStatOK);
            entity_result.Add("message", "done!");
            List<Dictionary<string, string>> diclist = new List<Dictionary<string, string>>();
            diclist.Add(entity_result);

            return new
            {
                //result = vmolist
                result = diclist
            };

        }

        [HttpGet]
        public object GetListAll(string api_version)
        {
            HttpRequestAuth reqauth = new HttpRequestAuth(Request);
            if (!reqauth.IsHeaderVerfied())
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeInvalidParameter,
                    message = "Invalid Parameters. Wrong headers"
                };
            }
            if (!reqauth.IsTokenAuthenticated())
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnauthorized,
                    message = "No Token"
                };
            }

            try
            {
                var credentials = new TokenCredentials(reqauth.GetTokenCache());
                var vmm = new VirtualMachineManager(credentials, reqauth.GetSubID());
                List<VirtualMachineObject> vmos = vmm.GetAllVirtualMachines();
                return new
                {
                    stat = Constants.ApiStatOK,
                    result = vmos
                };
            }
            catch (CloudException ce)
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnauthorized,
                    message = ce.Message
                };
            }
            catch (Exception e)
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnknownExecutionError,
                    message = e.Message
                };
            }
        }

        [HttpGet]
        public object GetListByResource(string api_version, string resource)
        {
            HttpRequestAuth reqauth = new HttpRequestAuth(Request);
            if (!reqauth.IsHeaderVerfied())
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeInvalidParameter,
                    message = "Invalid Parameters. Wrong headers"
                };
            }
            if (!reqauth.IsTokenAuthenticated())
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnauthorized,
                    message = "No Token"
                };
            }

            try
            {
                var credentials = new TokenCredentials(reqauth.GetTokenCache());
                var vmm = new VirtualMachineManager(credentials, reqauth.GetSubID());
                List<VirtualMachineObject> vmos = vmm.GetVirtualMachinesByResource(resource);
                return new
                {
                    stat = Constants.ApiStatOK,
                    result = vmos
                };
            }
            catch (CloudException ce)
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnauthorized,
                    message = ce.Message
                };
            }
            catch (Exception e)
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnknownExecutionError,
                    message = e.Message
                };
            }
        }

        [HttpPost]

        public object PostManage(string api_version, [FromBody]JObject data)
        {
            HttpRequestAuth reqauth = new HttpRequestAuth(Request);
            if (!reqauth.IsHeaderVerfied())
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeInvalidParameter,
                    message = "Invalid Parameters. Wrong headers"
                };
            }
            if (!reqauth.IsTokenAuthenticated())
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnauthorized,
                    message = "No Token"
                };
            }

            var entities = data["entities"];
            VirtualMachineManager vmm;
            try
            {
                var credentials = new TokenCredentials(reqauth.GetTokenCache());
                vmm = new VirtualMachineManager(credentials, reqauth.GetSubID());
            }
            catch (CloudException ce)
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnauthorized,
                    message = ce.Message
                };
            }
            catch (Exception e)
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnknownExecutionError,
                    message = e.Message
                };
            }

            List<Dictionary<string, string>> result_entities = new List<Dictionary<string, string>>();

            foreach (var entity in entities)
            {
                string action = entity["action"].ToString();
                string name = entity["name"].ToString();
                string resourcegroup = entity["resourcegroup"].ToString();
                string status = string.Empty;
                string message = string.Empty;

                try
                {
                    switch (action)
                    {
                        case "start":
                            vmm.StartVirtualMachine(resourcegroup, name);
                            break;
                        case "stop":
                            vmm.StopVirtualMachine(resourcegroup, name);
                            break;
                        case "restart":
                            vmm.RestartVirtualMachine(resourcegroup, name);
                            break;
                        default:
                            status = Constants.ApiStatFailure;
                            message = "Invalid action param! Need to be either start, stop, or restart";
                            break;
                    }
                    status = Constants.ApiStatOK;
                    message = "OK";

                }
                catch (Exception e)
                {
                    status = Constants.ApiStatFailure;
                    message = e.Message;
                }
                Dictionary<string, string> result_entity = new Dictionary<string, string>();
                result_entity.Add("name", name);
                result_entity.Add("resourcegroup", resourcegroup);
                result_entity.Add("action", action);
                result_entity.Add("status", status);
                result_entity.Add("message", message);
                result_entities.Add(result_entity);
            }

            return new
            {
                stat = Constants.ApiStatOK,
                result = result_entities
            };
        }

        [HttpPost]
        public object PostSchedule(string api_version, [FromBody]JObject data)
        {
            HttpRequestAuth reqauth = new HttpRequestAuth(Request);
            if (!reqauth.IsHeaderVerfied())
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeInvalidParameter,
                    message = "Invalid Parameters. Wrong headers"
                };
            }
            if (!reqauth.IsTokenAuthenticated())
            {
                return new
                {
                    stat = Constants.ApiStatFailure,
                    code = Constants.ApiErrorCodeUnauthorized,
                    message = "No Token"
                };
            }

            var schedules = data["schedules"];
            List<Dictionary<string, string>> result_entities = new List<Dictionary<string, string>>();

            int c = 10000;
            foreach (var schedule in schedules)
            {
                string action = schedule["action"].ToString();
                string name = schedule["name"].ToString();
                string resourcegroup = schedule["resourcegroup"].ToString();
                string triggertime = schedule["triggertime"].ToString();
                string status = string.Empty;
                string message = string.Empty;

                // do schedule 
                status = Constants.ApiStatOK;

                Dictionary<string, string> result_entity = new Dictionary<string, string>();
                result_entity.Add("name", name);
                result_entity.Add("resourcegroup", resourcegroup);
                result_entity.Add("action", action);
                result_entity.Add("status", status);
                result_entity.Add("message", message);
                result_entity.Add("scheduleid", c.ToString());
                result_entities.Add(result_entity);
                c++;
            }

            return new
            {
                stat = Constants.ApiStatOK,
                result = result_entities
            };
        }

    }
}
