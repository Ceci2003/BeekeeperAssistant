namespace BeekeeperAssistant.Web.Routes
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;

    public static class BeekeeperAssistantRoutesConfig
    {
        public static void ConfigureBeekeeperAssistantRoutes(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute("apiaryRoute", "Apiary/{apiaryNumber:apiaryNumber}", new { controller = "Apiary", action = "ByNumber" });
            endpoints.MapControllerRoute("beehiveRoute", "Beehive/{apiaryNumber:apiaryNumber}/{beehiveId}", new { controller = "Beehive", action = "ById" });
            endpoints.MapControllerRoute("beehiveTreatmentRoute", "Beehive/{apiaryNumber:apiaryNumber}/{beehiveId}#tabPage={tabPage}", new { controller = "Beehive", action = "ById", tabPage = string.Empty });
        }
    }
}
