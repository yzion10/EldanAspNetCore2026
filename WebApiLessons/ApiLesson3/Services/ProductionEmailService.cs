namespace ApiLesson3.Services
{
    public class ProductionEmailService : IEmailService
    {
        private string _to;
        private string _from;

        public ProductionEmailService()
        {
            _to = "xxProd@email.com";
            _from = "yyProd@email.com";
        }

        public void Send(string subject, string message)
        {
            // Code to send email in production environment
            Console.WriteLine($"Sending email from {_from} to {_to} with subject '{subject}' and message '{message}'");
        }
    }
}
