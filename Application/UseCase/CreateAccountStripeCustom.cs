using Stripe;

namespace app.Application.UseCase
{
    public class CreateAccountStripeCustom
    {
        public CreateAccountStripeCustom(){}

        public async Task<string> execute(string email)
        {
            try
            {
                var options = new AccountCreateOptions
                {
                    Type = "custom",
                    Email = email,
                    BusinessType = "individual", // Ou "company" se for uma empresa
                    Capabilities = new AccountCapabilitiesOptions
                    {
                        CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                        Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
                    },
                    Country = "BR", // Altere para o país do usuário
                };

                var service = new AccountService();
                var account = await service.CreateAsync(options);
                return account.Id;
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"Erro ao criar a conta: {ex.Message}");
                throw;
            }
        }
    }
}
