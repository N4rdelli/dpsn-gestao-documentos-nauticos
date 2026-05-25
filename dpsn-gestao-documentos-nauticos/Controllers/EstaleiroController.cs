using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Numerics;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    public class EstaleiroController : Controller
    {
        // Criando o context do Banco de Dados
        private readonly MongoDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EstaleiroController(MongoDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            // Criar uma lista de usuarios
            IEnumerable<Estaleiro> estaleiros;
            if (User.IsInRole("Admin") || User.IsInRole("Tecnologo"))
            {
                // Se for admin ou tecnologo, retorna a lista com todos os usuarios
                estaleiros = await _context.Estaleiros.Find(u => true).ToListAsync();
                return View(estaleiros);
            }
            // Se for estaleiro , retorna apenas os dados do proprio estaleiro logado
            var estaleiro = await _context.Estaleiros.Find(u => u.UserName == User.Identity.Name).FirstOrDefaultAsync();
            if (estaleiro == null)
            {
                return NotFound();
            }
            estaleiros = new List<Estaleiro> { estaleiro };
            return View(estaleiros);
        }
        // GET: Estaleiros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estaleiros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // O método Create recebe um objeto do tipo CreateEstaleiro, que é um ViewModel com as informções do estaleiro e de endereço.
        public async Task<IActionResult> Create(EstaleiroViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Instancia o objeto Endereco 
                    var endereco = new Endereco
                    {
                        Cep = model.Cep,
                        Logradouro = model.Logradouro,
                        Numero = model.Numero,
                        Complemento = model.Complemento,
                        Bairro = model.Bairro,
                        Cidade = model.Cidade,
                        Estado = model.Estado
                    };

                    // Instancia o objeto Estaleiro preenchendo as propriedades herdadas do Identity
                    var estaleiro = new Estaleiro
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        EmailConfirmed = true,

                        // Propiredades do estaleiro
                        NomeFantasia = model.NomeFantasia,
                        RazaoSocial = model.RazaoSocial,
                        Cnpj = model.Cnpj,
                        Telefone = model.Telefone,
                        Endereco = endereco

                    };

                    // Cria o usuário no banco. O CreateAsync faz o hash automático da model.Senha
                    var resultadoCriacao = await _userManager.CreateAsync(estaleiro, model.Senha ?? "");
                    if (resultadoCriacao.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(estaleiro, "Estaleiro");
                        Console.WriteLine($"Estaleiro criado com sucesso: {estaleiro.NomeFantasia}");
                        TempData["MensagemSucesso"] = "Estaleiro criado com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    // Se o Identity recusar a criação 
                    // repassa os erros do Identity para o ModelState aparecer na tela para o usuário
                    foreach (var erro in resultadoCriacao.Errors)
                    {
                        ModelState.AddModelError(string.Empty, erro.Description);
                    }

                    TempData["MensagemErro"] = "Não foi possível criar o estaleiro. Verifique os alertas abaixo.";
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = "Ocorreu um erro inesperado ao criar o estaleiro.";
                    Console.WriteLine(ex.Message);
                }
            }
            return View(model);
        }

        // GET: Estaleiro/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca o documento original no MongoDB
            var estaleiro = (Estaleiro)await _userManager.FindByIdAsync(id);
            if (estaleiro == null)
            {
                return NotFound();
            }
            // Mapeia os dados do banco para a ViewModel
            var viewModel = new EstaleiroViewModel
            {
                Id = estaleiro.Id.ToString(), // Converte para string para a View 
                NomeFantasia = estaleiro.NomeFantasia,
                RazaoSocial = estaleiro.RazaoSocial,
                Cnpj = estaleiro.Cnpj,
                Email = estaleiro.Email,
                Telefone = estaleiro.Telefone,

            };


            // Se o estaleiro já tiver um endereço salvo, joga os dados dele para a ViewModel também
            if (estaleiro.Endereco != null)
            {
                viewModel.Cep = estaleiro.Endereco.Cep;
                viewModel.Logradouro = estaleiro.Endereco.Logradouro;
                viewModel.Numero = estaleiro.Endereco.Numero;
                viewModel.Complemento = estaleiro.Endereco.Complemento;
                viewModel.Bairro = estaleiro.Endereco.Bairro;
                viewModel.Cidade = estaleiro.Endereco.Cidade;
                viewModel.Estado = estaleiro.Endereco.Estado;
            }

            // Envia a ViewModel preenchida para a tela
            return View(viewModel);
        }

        // POST: Estaleiro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EstaleiroViewModel model)
        {
            // Remove a validação da senha pois ela não é editada nesse método.
            ModelState.Remove(nameof(model.Senha));
            if (ModelState.IsValid)
            {
                try
                {
                    // Busca o estaleiro original no banco
                    var estaleiroNoBanco = (Estaleiro)await _userManager.FindByIdAsync(id);

                    if (estaleiroNoBanco == null)
                    {
                        return NotFound();
                    }
                    // Atualiza as propriedades normais dele com o que veio da Model
                    estaleiroNoBanco.NomeFantasia = model.NomeFantasia;
                    estaleiroNoBanco.RazaoSocial = model.RazaoSocial;
                    estaleiroNoBanco.Cnpj = model.Cnpj;
                    estaleiroNoBanco.Telefone = model.Telefone;

                    // Atualiza dados de login se você permitir que ele mude o e-mail
                    estaleiroNoBanco.Email = model.Email;
                    estaleiroNoBanco.UserName = model.Email; // O Identity exige que fiquem iguais
                    // Instanciando o objeto endereco
                    estaleiroNoBanco.Endereco = new Endereco
                    {
                        Cep = model.Cep,
                        Logradouro = model.Logradouro,
                        Numero = model.Numero,
                        Complemento = model.Complemento,
                        Bairro = model.Bairro,
                        Cidade = model.Cidade,
                        Estado = model.Estado
                    };
                    var resultado = await _userManager.UpdateAsync(estaleiroNoBanco);
                    if (resultado.Succeeded)
                    {
                        TempData["MensagemSucesso"] = "Informações atualizadas com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    // Se o Identity rejeitar a atualização (ex: e-mail duplicado)
                    foreach (var erro in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, erro.Description);
                    }
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = "Ocorreu um erro ao atualizar as informações do estaleiro.";
                }
            }
            return View(model);
        }
        // GET: Estaleiro/Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estaleiro = await _context.Estaleiros.Find(m => m.Id == id).FirstOrDefaultAsync();
            if (estaleiro == null)
            {
                return NotFound();
            }

            return View(estaleiro);
        }

        // POST: Estaleiro/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _context.Estaleiros.DeleteOneAsync(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var estaleiro = await _context.Estaleiros.Find(m => m.Id == id).FirstOrDefaultAsync();
            if (estaleiro == null)
            {
                return NotFound();
            }
            return View(estaleiro);
        }
    }
}
