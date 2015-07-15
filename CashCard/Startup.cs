using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CashCard.Startup))]
namespace CashCard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
