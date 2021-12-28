namespace BeekeeperAssistant.Web.ViewModels.Apiaries
{
    // {area}{action}{controller}{FieldName?}ViewModel
    public class AllApiariesViewModel
    {
        // AllApiaryUserApiariesViewModel
        public AllUserApiariesViewModel UserApiaries { get; set; }

        public AllHelperApiariesViewModel UserHelperApiaries { get; set; }
    }
}
