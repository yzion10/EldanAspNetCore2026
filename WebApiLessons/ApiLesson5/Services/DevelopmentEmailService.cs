namespace ApiLesson5.Services
{
    public class DevelopmentEmailService : IEmailService
    {
        private string _to;
        private string _from;
        private Guid _id;
        IConfiguration _config;

        public DevelopmentEmailService(IConfiguration config)
        {
            _config = config;

            _to = _config["mail:to"];
            _from = config["mail:from"];

            _id = Guid.NewGuid();
        }

        public void Send(string subject, string message)
        {
            // Code to send email in development environment
            Console.WriteLine($"[Development Email Service] Sending email from {_from}" +
                $" to {_to}" +
                $" with subject '{subject}'" +
                $" and message '{message}'" +
                $" and id '{_id}'");
        }
    }
}
