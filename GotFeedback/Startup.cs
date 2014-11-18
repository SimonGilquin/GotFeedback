using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GotFeedback.Startup))]
namespace GotFeedback
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}