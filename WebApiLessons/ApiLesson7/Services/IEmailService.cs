namespace ApiLesson7.Services
{
    public interface IEmailService
    {
        void Send(string subject, string message);
    }
}
