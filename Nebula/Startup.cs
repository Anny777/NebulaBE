using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectOrderFood.Startup))]
namespace ProjectOrderFood
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
