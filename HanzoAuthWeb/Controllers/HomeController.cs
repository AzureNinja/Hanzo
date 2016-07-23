using AzureNinjaAuthWebApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzureNinjaAuthWebApp.Controllers
{
    public class HomeController : AsyncController
    {
        const string AAD_AUTH_STRING = @"https://login.windows.net/{0}/oauth2/authorize?response_type=code&client_id={1}&client_secret={2}&redirect_uri={3}&resource={4}";
        const string AAD_AUTHTOKEN_STRING = @"https://login.windows.net/common/oauth2/token";
        public ActionResult Index(string tenantId, string clientId, string clientSecret, string redirectUri, string resource)
        {
            if (string.IsNullOrEmpty(tenantId) == false && string.IsNullOrEmpty(clientId) == false &&
                string.IsNullOrEmpty(clientSecret) == false && string.IsNullOrEmpty(redirectUri) == false &&
                string.IsNullOrEmpty(resource) == false)
            {
                var url = string.Format(AAD_AUTH_STRING, tenantId, clientId, clientSecret, redirectUri, resource);
                Session["redirect_uri"] = redirectUri;
                Session["client_id"] = clientId;
                Session["client_secret"] = clientSecret;
                Session["resource"] = resource;
                ViewBag.Url = url;
            }
            return View();
        }

        public async Task<ActionResult> Authorized(string code)
        {
            AADResponse model = null;
            if (string.IsNullOrEmpty(code) == false)
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        { "grant_type", "authorization_code" },
                        { "redirect_uri", Session["redirect_uri"].ToString() },
                        { "client_id", Session["client_id"].ToString()},
                        { "client_secret", Session["client_secret"].ToString()},
                        { "code", code }
                    };
                    var response = await client.PostAsync(AAD_AUTHTOKEN_STRING, new FormUrlEncodedContent(values));
                    var nativeMessage = await response.Content.ReadAsStringAsync();

                    // TODO: This AADResponse is my defined model. If you want to store AccessToken and others, please use this class.
                    model = JsonConvert.DeserializeObject<AADResponse>(nativeMessage);
                    ViewBag.RawMessage = nativeMessage;
                }
                ViewBag.Code = code;
            }
            else
            {
                TempData["message"] = "This request doesn't contain 'code' parameter.";
            }
            return View(model);
        }
    }
}