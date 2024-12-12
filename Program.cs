using app.Application.GatewayService.WppAPI;
using app.Application.IAuth;
using app.Application.IRepository;
using app.Infra.Auth;
using app.Infra.GatewayServices.WppAPI;
using app.Infra.Repository;
using app.Infra.Repository.FactoryContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:3000")
                        .WithExposedHeaders("Token")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add interfaces and classes in the DI.
builder.Services.AddScoped<IFactoryDbContext, FactoryDbContext>();
builder.Services.AddScoped<IRepositoryUserTutor, RepositoryUserTutor>();
builder.Services.AddScoped<IRepositoryUserClinic, RepositoryUserClinic>();
builder.Services.AddScoped<IRepositoryUserCollaborator,RepositoryUserCollaborator>();
builder.Services.AddScoped<IRepositoryPet, RepositoryPet>();
builder.Services.AddScoped<IRepositoryInvitation, RepositoryInvitation>();
builder.Services.AddScoped<IRepositoryNetWork, RepositoryNetWork>();
builder.Services.AddScoped<IRepositoryService, RepositoryService>();
builder.Services.AddScoped<IRepositoryOrderService, RepositoryOrderService>();
builder.Services.AddScoped<IRepositoryAttendance, RepositoryAttendance>();
builder.Services.AddScoped<IRepositoryProntuario, RepositoryProntuario>();
builder.Services.AddScoped<IRepositoryFile, RepositoryFile>();
builder.Services.AddScoped<IRepositoryForm, RepositoryForm>();
builder.Services.AddScoped<IRepositoryItem,RepositoryItem>();
builder.Services.AddScoped<IRepositoryStock, RepositoryStock>();
builder.Services.AddScoped<IRepositoryPortfolioClient, RepositoryPortfolioClient>();
builder.Services.AddScoped<ITokenService, TokenServiceSignin>();
builder.Services.AddScoped<IMessage,Message>();
builder.Services.AddScoped<AccountLinkService>(); // Para links de conta conectada
builder.Services.AddScoped<PaymentIntentService>(); // Para criar Payment Intents
builder.Services.AddScoped<ChargeService>(); // Para processar cobranças diretas





var key = Encoding.ASCII.GetBytes("ksksjffdfd74645834745hfhdhdfhdf8899889");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        Console.WriteLine($"Request path: {context.Request.Path}");
        if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
        {
            Console.WriteLine("CORS headers not set");
        }
        return Task.CompletedTask;
    });
    await next.Invoke();
});

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
