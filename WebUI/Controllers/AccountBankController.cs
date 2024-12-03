using app.Application.UseCase;
using Microsoft.AspNetCore.Mvc;
using Stripe;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountBankController : ControllerBase
    {
        [HttpPost("createLink")]
        public IActionResult CreateAccountLink([FromBody] AccountLinkRequest request)
        {
            var options = new AccountLinkCreateOptions
            {
                Account = request.AccountId, // ID da conta conectada
                RefreshUrl = "https://sua-plataforma.com/refresh", // URL para recarregar em caso de erro
                ReturnUrl = "https://sua-plataforma.com/success", // URL após configuração
                Type = "account_onboarding",
            };

            var service = new AccountLinkService();
            var accountLink = service.Create(options);

            return Ok(new { url = accountLink.Url });
        }

        [HttpPost("createAccount")]
        public async Task<ActionResult> CreateConnectedAccount([FromBody] ConnectedAccountRequest request)
        {
            CreateAccountStripeCustom usecase = new CreateAccountStripeCustom();
            string idaccount = await usecase.execute(request.Email);

            return Ok(new { accountId = idaccount });
        }
    }

    public class AccountLinkRequest
    {
        public string AccountId { get; set; }
    }
    public class ConnectedAccountRequest
    {
        public string Email { get; set; }
    }
}
