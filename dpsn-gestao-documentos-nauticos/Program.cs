using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
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

var app = builder.Build();

// Bloco de teste de conexão opcional (pinga o banco pra ver se deu certo)
using (var scope = app.Services.CreateScope())
{
    var client = scope.ServiceProvider.GetRequiredService<IMongoClient>();
    try
    {
        client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
        Console.WriteLine("Conexão com MongoDB Atlas estabelecida com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao conectar no MongoDB: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
