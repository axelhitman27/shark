using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SharkWebApp.Startup))]
namespace SharkWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
