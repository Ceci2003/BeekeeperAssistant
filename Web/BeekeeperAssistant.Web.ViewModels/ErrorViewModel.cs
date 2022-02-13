namespace BeekeeperAssistant.Web.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

        public string Title { get; set; }

        public string Message { get; set; }
    }
}
