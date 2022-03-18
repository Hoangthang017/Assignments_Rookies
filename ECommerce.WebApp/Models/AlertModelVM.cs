namespace ECommerce.WebApp.Models
{
    public class AlertModelVM
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string Type { get; set; }

        public string RedirectToController { get; set; }

        public string RedirecToAcction { get; set; }

        public Dictionary<string, string> RouteValues { get; set; }
    }
}