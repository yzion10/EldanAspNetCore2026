namespace ApiLesson3.Services
{
    public class DevelopmentEmailService : IEmailService
    {
        private string _to;
        private string _from;
        private Guid _id;


        public DevelopmentEmailService()
        {
            _to = "xxDev@email.com";
            _from = "yyDev@email.com";

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
