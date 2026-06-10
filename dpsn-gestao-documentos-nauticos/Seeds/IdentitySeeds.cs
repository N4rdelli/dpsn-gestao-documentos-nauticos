using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static dpsn_gestao_documentos_nauticos.Models.Documento;

namespace dpsn_gestao_documentos_nauticos.Seeds
{
    public class IdentitySeeds
    {
        public static async Task SeedRolesAndUser(IServiceProvider serviceProvider, string defaultPassword)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            string[] roleNames = { "Admin", "Estaleiro", "Tecnologo" };
            foreach (var roleName in roleNames)
            {
                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByEmailAsync("jpderussi@gmail.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "jpderussi@gmail.com",
                    Email = "jpderussi@gmail.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, defaultPassword);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // MÉTODO ATUALIZADO: Carga de Dados conforme especificações do DPSN
        public static async Task SeedAppData(IServiceProvider serviceProvider, string defaultPassword)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<MongoDbContext>();

            // 1. SEED DO TECNÓLOGO PADRÃO (Helcio Marcelo de Russi)
            string tecnologoEmail = "helcio.russi@dpsn.com";
            ApplicationUser tecnologoBaseUser = await userManager.FindByEmailAsync(tecnologoEmail);
            Tecnologo tecnologoUser = tecnologoBaseUser as Tecnologo;

            if (tecnologoBaseUser == null)
            {
                var novoTecnologo = new Tecnologo
                {
                    UserName = tecnologoEmail,
                    Email = tecnologoEmail,
                    EmailConfirmed = true,
                    NomeCompleto = "Helcio Marcelo de Russi",
                    Cpf = "11122233344",
                    DataCadastro = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(novoTecnologo, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(novoTecnologo, "Tecnologo");
                    tecnologoBaseUser = novoTecnologo;
                }
            }

            // Se já existem estaleiros salvos no banco, interrompe para evitar dados duplicados
            long qtdEstaleiros = await context.Estaleiros.CountDocumentsAsync(u => true);
            if (qtdEstaleiros > 0) return;

            // Endereço padrão para os testes
            var enderecoPadrao = new Endereco
            {
                Cep = "17200000",
                Logradouro = "Rua das Naus, KM 10",
                Numero = "100",
                Bairro = "Distrito Naval",
                Cidade = "Jaú",
                Estado = "SP"
            };

            // 2. SEED DOS 3 ESTALEIROS RELACIONADOS AO TECNÓLOGO
            var estaleirosFicticios = new List<Estaleiro>
            {
                new Estaleiro { NomeFantasia = "Estaleiro Atlântico Sul", RazaoSocial = "Atlântico Sul Engenharia Naval LTDA", Cnpj = "11111111000111", Telefone = "14999990001", TecnologoId = tecnologoBaseUser.Id, Endereco = enderecoPadrao, DataCadastro = DateTime.UtcNow },
                new Estaleiro { NomeFantasia = "Estaleiro Mar de Proa", RazaoSocial = "Mar de Proa Construções Náuticas SA", Cnpj = "22222222000122", Telefone = "14999990002", TecnologoId = tecnologoBaseUser.Id, Endereco = enderecoPadrao, DataCadastro = DateTime.UtcNow },
                new Estaleiro { NomeFantasia = "Estaleiro Ventania", RazaoSocial = "Ventania Custom Boats LTDA", Cnpj = "33333333000133", Telefone = "14999990003", TecnologoId = tecnologoBaseUser.Id, Endereco = enderecoPadrao, DataCadastro = DateTime.UtcNow }
            };

            var listaEstaleirosSalvos = new List<Estaleiro>();

            foreach (var estaleiro in estaleirosFicticios)
            {
                string emailEstaleiro = $"{estaleiro.NomeFantasia.Replace(" ", "").ToLower()}@teste.com";
                estaleiro.UserName = emailEstaleiro;
                estaleiro.Email = emailEstaleiro;
                estaleiro.EmailConfirmed = true;
                estaleiro.Senha = "CriptografadaNoIdentity";

                var result = await userManager.CreateAsync(estaleiro, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(estaleiro, "Estaleiro");
                    listaEstaleirosSalvos.Add(estaleiro);
                }
            }

            // Cliente padrão fictício para preenchimento nos documentos
            var clienteFicticio = new Cliente
            {
                Nome = "Navegações Comandante Silva",
                Cpf_cnpj = "98765432100",
                Ano_contrucao = "2026",
                NumeroChassi = "BR-DPSN998877",
                Endereco = enderecoPadrao
            };

            // 3. SEED DE EMBARCAÇÕES E DOCUMENTAÇÕES
            // Cada estaleiro com 3 embarcações e 2 documentações (uma em RevisaoPendente e outra em EmRevisao)
            string[] sufixosNavio = { "Alpha", "Beta", "Odyssey" };

            foreach (var estaleiro in listaEstaleirosSalvos)
            {
                var embarcacoesDoEstaleiro = new List<Embarcacao>();

                // Geração das 3 embarcações para o estaleiro corrente
                for (int i = 0; i < 3; i++)
                {
                    var embarcacao = new Embarcacao
                    {
                        Nome = $"Navio {estaleiro.NomeFantasia.Replace("Estaleiro ", "")} {sufixosNavio[i]}",
                        EstaleiroId = estaleiro.Id,
                        TipoEmbarcacao = "Esporte e Recreio",
                        ComprimentoTotal = 14.25m + i,
                        BocaMoldada = 4.10m,
                        PontalMoldado = 2.10m,
                        CaladoMaximo = 1.20m,
                        CaladoLeve = 0.80m,
                        ArqueacaoBruta = 28.00m,
                        ArqueacaoLiquida = 18.50m,
                        Tpb = 10.00m,
                        Contorno = 6.50m,
                        AreaNavegacaoTipoServico = "Mar Aberto / Interior",
                        MaterialCasco = "Fibra de Vidro",
                        MotorizacaoMax = 600,
                        Tripulantes = 2,
                        Passageiros = 10,
                        Data = DateTime.UtcNow
                    };

                    await context.Embarcacoes.InsertOneAsync(embarcacao);
                    embarcacoesDoEstaleiro.Add(embarcacao);
                }

                // Criação do Documento 1: Status RevisaoPendente (Vinculado à primeira embarcação do estaleiro)
                var docPendente = new Documento
                {
                    Estaleiro = estaleiro,
                    Embarcacao = embarcacoesDoEstaleiro[0],
                    Cliente = clienteFicticio,
                    NumeroInscricao = "A ser inscrita",
                    DataCriacaoDocumento = DateTime.UtcNow.AddDays(-2),
                    Status = StatusDocumento.RevisaoPendente,
                    CaminhoPdfAssinado = null
                };
                await context.Documentos.InsertOneAsync(docPendente);

                // Criação do Documento 2: Status EmRevisao (Vinculado à segunda embarcação do estaleiro)
                var docEmRevisao = new Documento
                {
                    Estaleiro = estaleiro,
                    Embarcacao = embarcacoesDoEstaleiro[1],
                    Cliente = clienteFicticio,
                    NumeroInscricao = "A ser inscrita",
                    DataCriacaoDocumento = DateTime.UtcNow.AddDays(-1),
                    Status = StatusDocumento.EmRevisao,
                    CaminhoPdfAssinado = null
                };
                await context.Documentos.InsertOneAsync(docEmRevisao);
            }
        }
    }
}