using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhysicianLocator.Startup))]
namespace PhysicianLocator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
