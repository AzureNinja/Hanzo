using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanzoProxyClient
{
    public class ClientBuilder
    {
        private const string DEFAULT_APIKEY = "AD6ED4A72D0726D71EB1958168DB0E1DFD0C30005C968DAE4209D785EAF65FD9";
        private const string DEFAULT_APIVERSION = "2016-07-25-Preview";
        private const string DEFAULT_PROXYSERVERURL = "http://azureninjaproxy.azurewebsites.net";

        private ClientBuilder()
        {
            // Private Constructor
        }

        public static Client GetClientInstance(string subid, string authtoken)
        {
            return GetClientInstance(
                        subid,
                        authtoken,
                        new Config(
                            DEFAULT_APIKEY,
                            DEFAULT_APIVERSION,
                            DEFAULT_PROXYSERVERURL)
                        );
        }

        public static Client GetClientInstance(string subid, string authtoken, Config config)
        {
            Client.GetInstance().Initialize(
                        subid,
                        authtoken,
                        config);

            return Client.GetInstance();
        }

    }
}
