using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PersonalLibraryManager.Startup))]
namespace PersonalLibraryManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
