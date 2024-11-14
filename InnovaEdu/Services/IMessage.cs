namespace ColegioWeb.Services
{
    public interface IMessage
    {

        void SendEmail(string asunto, string body, string to);
    }
}
