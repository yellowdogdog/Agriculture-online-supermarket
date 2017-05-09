using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Agriculture_online_supermarket.Startup))]
namespace Agriculture_online_supermarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
