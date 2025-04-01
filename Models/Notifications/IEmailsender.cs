namespace Systemize.Models.Notifications
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
