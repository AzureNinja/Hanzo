using System;

namespace HanzoProxyClient
{
    public class Config
    {
        private string _apiVersion;
        private string _apiKey;
        private string _proxyServerUrl;
        public Config() {
        }
        public Config(string apiKey, string apiVersion, string proxyServerUrl) {
            this._apiKey = apiKey;
            this._apiVersion = apiVersion;
            this._proxyServerUrl = proxyServerUrl;
        }

         public string ApiVersion
        {
            set { this._apiVersion= value; }
            get { return this._apiVersion; }
        }

          public string ApiKey
        {
            set { this._apiKey= value; }
            get { return this._apiKey; }
        }

          public string ProxyServerUrl
        {
            set { this._proxyServerUrl= value; }
            get { return this._proxyServerUrl; }
        }
         
    }
}
