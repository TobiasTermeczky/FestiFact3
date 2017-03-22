using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FestiFact3.Startup))]
namespace FestiFact3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
