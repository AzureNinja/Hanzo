using System;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json.Linq;

using HanzoProxyClient;

namespace Hanzo
{
    class HanzoCmd
    {
        private static string _apiVersion = ConfigurationManager.AppSettings["apiVersion"] ?? "";
        private static string _apiKey = ConfigurationManager.AppSettings["apiKey"] ?? "";
        private static string _proxyServerUrl = ConfigurationManager.AppSettings["proxyServerUrl"] ?? "";

        public static string GetVmListCommand(Client client)
        {
            string message = string.Empty;
            try
            {
                JObject resobj = client.GetVmList();

                string stat = resobj["stat"].ToString();
                if (stat == "ok")
                {
                    Console.WriteLine("GetVmList succeeded!");
                    var result_items = resobj["result"];
                    foreach (var result_item in result_items)
                    {
                        message += string.Format("VirtualMachine name: {0} resourcegroup: {1} status: {2}", 
                                            result_item["name"].ToString(),
                                            result_item["resourcegroup"].ToString(),
                                            result_item["status"].ToString());
                        /*
                        Console.WriteLine("VirtualMachine name: {0} resourcegroup: {1} status: {2}",
                                            result_item["name"].ToString(),
                                            result_item["resourcegroup"].ToString(),
                                            result_item["status"].ToString()
                                        );
                                        */
                    }
                }
                else
                {
                    Console.WriteLine("GetVmList failure! \n {0} \n", resobj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception caught:");
                while (e != null)
                {
                    Console.WriteLine("\t{0}", e.Message);
                    e = e.InnerException;
                }
            }

            return message;
        }

        static void GetVmListByResourceCommand(Client client, string resourceGroup)
        {
            try
            {
                JObject resobj = client.GetVmListByReource(resourceGroup);

                string stat = resobj["stat"].ToString();
                if (stat == "ok")
                {
                    Console.WriteLine("GetVmListByResource succeeded!");
                    var result_items = resobj["result"];
                    foreach (var result_item in result_items)
                    {
                        Console.WriteLine("VirtualMachine name: {0} resourcegroup: {1} status: {2}",
                                            result_item["name"].ToString(),
                                            result_item["resourcegroup"].ToString(),
                                            result_item["status"].ToString()
                                        );
                    }
                }
                else
                {
                    Console.WriteLine("GetVmListByResource failure! \n {0} \n", resobj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception caught:");
                while (e != null)
                {
                    Console.WriteLine("\t{0}", e.Message);
                    e = e.InnerException;
                }
            }
        }

        static void ManageVmCommand(Client client, string vmname, string resourcegroup, string action)
        {
            try
            {
                JObject resobj = client.ManageVm(vmname, resourcegroup, action);

                string stat = resobj["stat"].ToString();
                if (stat == "ok")
                {
                    Console.WriteLine("ManageVm succeeded!");
                    var result_items = resobj["result"];
                    foreach (var result_item in result_items)
                    {
                        Console.WriteLine("VM Managed name: {0} resourcegroup: {1} action: {2} status: {2} message: {3}",
                                            result_item["name"].ToString(),
                                            result_item["resourcegroup"].ToString(),
                                            result_item["action"].ToString(),
                                            result_item["status"].ToString(),
                                            result_item["message"].ToString()
                                        );
                    }
                }
                else
                {
                    Console.WriteLine("ManageVm failure! \n {0} \n", resobj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception caught:");
                while (e != null)
                {
                    Console.WriteLine("\t{0}", e.Message);
                    e = e.InnerException;
                }
            }
        }

        static void ManageVmsCommand(Client client, List<Dictionary<string, string>> req_entities)
        {
            try
            {
                JObject resobj = client.ManageVms(req_entities);

                string stat = resobj["stat"].ToString();
                if (stat == "ok")
                {
                    Console.WriteLine("ManageVms succeeded!");
                    var result_items = resobj["result"];
                    foreach (var result_item in result_items)
                    {
                        Console.WriteLine("VM Managed name: {0} resourcegroup: {1} action: {2} status: {2} message: {3}",
                                            result_item["name"].ToString(),
                                            result_item["resourcegroup"].ToString(),
                                            result_item["action"].ToString(),
                                            result_item["status"].ToString(),
                                            result_item["message"].ToString()
                                        );
                    }
                }
                else
                {
                    Console.WriteLine("ManageVms failure! \n {0} \n", resobj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception caught:");
                while (e != null)
                {
                    Console.WriteLine("\t{0}", e.Message);
                    e = e.InnerException;
                }
            }
        }

        static void ScheduleVmCommand(Client client, string vmname, string resourcegroup, string action, DateTime triggerTime)
        {
            try
            {
                JObject resobj = client.ScheduleVm(vmname, resourcegroup, action, triggerTime);

                string stat = resobj["stat"].ToString();
                if (stat == "ok")
                {
                    Console.WriteLine("ScheduleVm succeeded!");
                    var result_items = resobj["result"];
                    foreach (var result_item in result_items)
                    {
                        Console.WriteLine("VM Scheduled name: {0} resourcegroup: {1} action: {2} status: {2} scheduleid: {3}",
                                            result_item["name"].ToString(),
                                            result_item["resourcegroup"].ToString(),
                                            result_item["action"].ToString(),
                                            result_item["status"].ToString(),
                                            result_item["scheduleid"].ToString()
                                        );
                    }
                }
                else
                {
                    Console.WriteLine("ScheduleVm failure! \n {0} \n", resobj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception caught:");
                while (e != null)
                {
                    Console.WriteLine("\t{0}", e.Message);
                    e = e.InnerException;
                }
            }
        }

        static void ScheduleVmsCommand(Client client, List<Dictionary<string, string>> req_schedules)
        {
            try
            {
                JObject resobj = client.ScheduleVms(req_schedules);

                string stat = resobj["stat"].ToString();
                if (stat == "ok")
                {
                    Console.WriteLine("ScheduleVms succeeded!");
                    var result_items = resobj["result"];
                    foreach (var result_item in result_items)
                    {
                        Console.WriteLine("VM Scheduled name: {0} resourcegroup: {1} action: {2} status: {2} scheduleid: {3}",
                                            result_item["name"].ToString(),
                                            result_item["resourcegroup"].ToString(),
                                            result_item["action"].ToString(),
                                            result_item["status"].ToString(),
                                            result_item["scheduleid"].ToString()
                                        );
                    }
                }
                else
                {
                    Console.WriteLine("ScheduleVms failure! \n {0} \n", resobj);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unhandled exception caught:");
                while (e != null)
                {
                    Console.WriteLine("\t{0}", e.Message);
                    e = e.InnerException;
                }
            }
        }

        static void Main(string[] args)
        {

            string subID = "<your subscrptionID>";
            string authToken = "<your authToken>";
            string resGroup = "RG-YOICHIKA-DEV";
            string vmname1 = "yoichika-dev2";
            string vmname2 = "yoichika-dev3";
            string action1 = "stop";
            string action2 = "start";

            Console.WriteLine("apiKey=" + _apiKey);
            Console.WriteLine("apiVersion=" + _apiVersion);
            Console.WriteLine("proxyServerUrl=" + _proxyServerUrl);


            Client client = ClientBuilder.GetClientInstance(subID, authToken,
                                new Config(
                                        _apiKey,
                                        _apiVersion,
                                        _proxyServerUrl)
                                );

            Console.WriteLine("\r\n\r\nExecute GetVmList ********* \r\n");
            GetVmListCommand(client);

            Console.WriteLine("\r\n\r\nExecute GetVmListByResource ********* \r\n");
            GetVmListByResourceCommand(client, resGroup);

            Console.WriteLine("\r\n\r\nExecute ManageVm ******** \r\n");
            ManageVmCommand(client, vmname1, resGroup, action1);

            Console.WriteLine("\r\n\r\nExecute ManageVms ******** \r\n");

            List<Dictionary<string, string>> req_entities = new List<Dictionary<string, string>>();
            Dictionary<string, string> e1 = new Dictionary<string, string>();
            e1.Add("name", vmname1);
            e1.Add("resourcegroup", resGroup);
            e1.Add("action", action1);
            req_entities.Add(e1);
            Dictionary<string, string> e2 = new Dictionary<string, string>();
            e2.Add("name", vmname2);
            e2.Add("resourcegroup", resGroup);
            e2.Add("action", action2);
            req_entities.Add(e2);
            ManageVmsCommand(client, req_entities);

            Console.WriteLine("\r\n\r\nExecute ScheduleVm ******** \r\n");
            ScheduleVmCommand(client, vmname1, resGroup, action1, new DateTime(2016, 12, 30));

            Console.WriteLine("\r\n\r\nExecute ScheduleVms ******** \r\n");
            TimeSpan t1 = new DateTime(2016, 12, 30) - new DateTime(1970, 1, 1);
            int sec_since_epoch1 = (int)t1.TotalSeconds;
            TimeSpan t2 = new DateTime(2016, 12, 30) - new DateTime(1970, 1, 1);
            int sec_since_epoch2 = (int)t2.TotalSeconds;
            List<Dictionary<string, string>> req_schedules = new List<Dictionary<string, string>>();
            Dictionary<string, string> s1 = new Dictionary<string, string>();
            s1.Add("name", vmname1);
            s1.Add("resourcegroup", resGroup);
            s1.Add("action", action1);
            s1.Add("triggertime", sec_since_epoch1.ToString());
            req_schedules.Add(s1);
            Dictionary<string, string> s2 = new Dictionary<string, string>();
            s2.Add("name", vmname2);
            s2.Add("resourcegroup", resGroup);
            s2.Add("action", action2);
            s2.Add("triggertime", sec_since_epoch2.ToString());
            req_schedules.Add(s2);
            ScheduleVmsCommand(client, req_schedules);

            Console.ReadKey();
        }
    }
}
