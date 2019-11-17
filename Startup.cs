using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NAPASTUDENT.Startup))]
namespace NAPASTUDENT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
