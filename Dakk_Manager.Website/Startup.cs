using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dakk_Manager.Website.Startup))]
namespace Dakk_Manager.Website
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
