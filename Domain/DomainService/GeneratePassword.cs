namespace app.Domain.DomainService
{
    public class GeneratePassword
    {
        public string generate()
        {
            int tamanho = 8;
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            Random random = new Random();
            char[] senha = new char[tamanho];

            for (int i = 0; i < tamanho; i++)
            {
                senha[i] = caracteres[random.Next(caracteres.Length)];
            }

            return new string(senha);
        }
    }
}
