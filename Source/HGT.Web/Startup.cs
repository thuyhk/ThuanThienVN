using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HGT.Web.Startup))]
namespace HGT.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
