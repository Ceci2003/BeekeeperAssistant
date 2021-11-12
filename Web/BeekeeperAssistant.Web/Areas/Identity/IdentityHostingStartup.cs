using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BeekeeperAssistant.Web.Areas.Identity.IdentityHostingStartup))]

namespace BeekeeperAssistant.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
