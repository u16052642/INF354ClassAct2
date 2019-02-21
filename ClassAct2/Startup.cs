using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClassAct2.Startup))]
namespace ClassAct2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
