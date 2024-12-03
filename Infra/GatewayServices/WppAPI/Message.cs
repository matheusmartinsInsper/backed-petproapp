using app.Application.GatewayService.WppAPI;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace app.Infra.GatewayServices.WppAPI
{
    public class Message: IMessage
    {
        private readonly string _id;
        private readonly string _token;
        private readonly HttpClient _httpClient;

        public Message()
        {
            _id = "c6b12d90-7f3c-4008-a45d-0a839a7e90f3";
            _token = "4A77505C0C20-4C2A-A2D3-CD911EC62633";
            _httpClient = new HttpClient();
        }

        public async Task sendMessage(string message, string numberToSend)
        {
            string url = $"https://api.evoapicloud.com/message/sendText/{_id}";
            var requestBody = new
            {
                number = numberToSend,
                text = message
            };
            string jsonBody = JsonSerializer.Serialize(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("apikey", _token);

            try
            {
                HttpResponseMessage response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Mensagem enviada com sucesso: {responseContent}");
                }
                else
                {
                    Console.WriteLine($"Erro ao enviar mensagem: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem: {ex.Message}");
            }
        }
    }
}
