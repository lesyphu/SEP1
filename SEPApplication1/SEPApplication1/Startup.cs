using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SEPApplication1.Startup))]
namespace SEPApplication1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
