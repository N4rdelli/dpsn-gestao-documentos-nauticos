using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Numerics;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    [Authorize(Roles = "Admin,Tecnologo")]
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
            // Como o controller está travado, aqui só entram Admins ou Tecnologos.
            // Portanto, listamos sempre todos os estaleiros.
            var estaleiros = await _context.Estaleiros.Find(u => true).ToListAsync();
            return View(estaleiros);
        }

        // GET: Estaleiro/Create
        public async Task<IActionResult> Create()
        {
            var model = new EstaleiroViewModel();

            // Se quem está logado for Admin, precisamos listar os tecnólogos para o Dropdown
            if (User.IsInRole("Admin"))
            {
                var tecnologos = await _context.Tecnologos.Find(u => true).ToListAsync();
                ViewBag.Tecnologos = tecnologos;
            }

            return View(model);
        }

        // POST: Estaleiro/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstaleiroViewModel model)
        {
            var usuarioLogado = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Tecnologo"))
            {
                // Força o ID do tecnólogo logado automaticamente e remove o campo do estado de validação
                model.TecnologoId = usuarioLogado.Id;
                ModelState.Remove("TecnologoId");
            }

            if (ModelState.IsValid)
            {
                var novoEstaleiro = new Estaleiro
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NomeFantasia = model.NomeFantasia,
                    RazaoSocial = model.RazaoSocial,
                    Cnpj = model.Cnpj,
                    Telefone = model.Telefone,
                    Senha = model.Senha,
                    Endereco = new Endereco
                    {
                        Cep = model.Cep,
                        Logradouro = model.Logradouro,
                        Numero = model.Numero,
                        Complemento = model.Complemento,
                        Bairro = model.Bairro,
                        Cidade = model.Cidade,
                        Estado = model.Estado
                    },
                    TecnologoId = model.TecnologoId, // Salva o Id atrelado!
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(novoEstaleiro, model.Senha);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(novoEstaleiro, "Estaleiro");
                    TempData["MensagemSucesso"] = "Estaleiro cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Se falhar e for admin, recarrega a ViewBag
            if (User.IsInRole("Admin"))
            {
                ViewBag.Tecnologos = await _context.Tecnologos.Find(u => true).ToListAsync();
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
