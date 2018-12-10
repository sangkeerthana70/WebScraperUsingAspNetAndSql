using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinanceWebScraperUsingAsp.NetAndSQL.Startup))]
namespace FinanceWebScraperUsingAsp.NetAndSQL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
