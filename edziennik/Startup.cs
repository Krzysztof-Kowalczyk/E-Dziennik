using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(edziennik.Startup))]
namespace edziennik
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
