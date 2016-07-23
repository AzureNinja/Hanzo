using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureNinjaAuthWebApp.Startup))]
namespace AzureNinjaAuthWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
