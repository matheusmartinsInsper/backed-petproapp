namespace app.Application.GatewayService.WppAPI
{
    public interface IMessage
    {
        Task sendMessage(string message, string numberToSend);
    }
}
