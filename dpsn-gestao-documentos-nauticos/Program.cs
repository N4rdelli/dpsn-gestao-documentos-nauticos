using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.Seeds;
using dpsn_gestao_documentos_nauticos.Services;
using dpsn_gestao_documentos_nauticos.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configurações do Banco
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Registro do MongoClient customizado
builder.Services.AddSingleton<IMongoClient>(sp => {
    var mongoSettings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;

    // Converte a string para o objeto de configurações
    var settings = MongoClientSettings.FromConnectionString(mongoSettings.ConnectionString);

    // Define a Stable API para garantir compatibilidade futura
    settings.ServerApi = new ServerApi(ServerApiVersion.V1);

    return new MongoClient(settings);
});

builder.Services.AddScoped<MongoDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuração do Identity
var mongoSettings = builder.Configuration.GetSection("MongoDbSettings")
    .Get<MongoDbSettings>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>
    (options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
    .AddMongoDbStores<ApplicationUser, ApplicationRole, string>(
        mongoSettings.ConnectionString, mongoSettings.DatabaseName)
        .AddDefaultTokenProviders();
builder.Services.AddRazorPages();

// Configuração de cookies para manter o usuario logado
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Login";
    // Usuario vai ficar logado por 30 dias
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    // Se o usuário acessar o site faltando menos da metade do tempo para expirar,
    // o ASP.NET renova o cookie por mais 30 dias automaticamente.
    options.SlidingExpiration = true;
});

//configuração envio de email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<EmailService>();

var app = builder.Build();

// Inicialização e seeds
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // 1. Teste de conexão (Ping)
    try
    {
        var client = services.GetRequiredService<IMongoClient>();
        client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
        Console.WriteLine("Conexão com MongoDB Atlas estabelecida com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao conectar no MongoDB: {ex.Message}");
    }

    // 2. Execução das Sementes do Sistema
    try
    {
        string senhaPadrao = "Admin@123";

        // Cria as Roles e o Admin inicial
        await IdentitySeeds.SeedRolesAndUser(services, senhaPadrao);

        // NOVA LINHA: Carrega o Tecnólogo Helcio, os Estaleiros, Embarcações e Documentos
        await IdentitySeeds.SeedAppData(services, senhaPadrao);

        Console.WriteLine("Seeds executadas com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao executar as Seeds: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

//acrescentar app.UseAuthentication() antes do app.UseAuthorization();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();