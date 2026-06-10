using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    [Authorize(Roles = "Admin")] // Apenas o administrador tem acesso à gestão de tecnólogos
    public class TecnologosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MongoDbContext _context;

        public TecnologosController(UserManager<ApplicationUser> userManager, MongoDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Listagem de Tecnólogos
        public async Task<IActionResult> Index()
        {
            var tecnologos = await _context.Tecnologos.Find(u => true).ToListAsync();
            return View(tecnologos);
        }

        // GET: Tecnologos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tecnologos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TecnologoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var novoTecnologo = new Tecnologo
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NomeCompleto = model.NomeCompleto,
                    Cpf = model.Cpf,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(novoTecnologo, model.Senha);
                if (result.Succeeded)
                {
                    // Atribui a Role de Tecnólogo
                    await _userManager.AddToRoleAsync(novoTecnologo, "Tecnologo");
                    TempData["MensagemSucesso"] = "Tecnólogo cadastrado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // GET: Tecnologo/Details/5
        [Authorize(Roles = "Admin,Tecnologo")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Busca o usuário pelo ID
            var tecnologo = await _userManager.FindByIdAsync(id);
            if (tecnologo == null)
            {
                return NotFound();
            }

            // Garante que o usuário encontrado realmente pertence à Role Tecnologo
            if (!await _userManager.IsInRoleAsync(tecnologo, "Tecnologo"))
            {
                return Unauthorized();
            }

            return View(tecnologo);
        }

        // GET: Tecnologo/Edit/5
        [Authorize(Roles = "Admin,Tecnologo")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tecnologo = await _userManager.FindByIdAsync(id);
            if (tecnologo == null)
            {
                return NotFound();
            }

            if (!await _userManager.IsInRoleAsync(tecnologo, "Tecnologo"))
            {
                return Unauthorized();
            }

            return View(tecnologo);
        }

        // POST: Tecnologo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Tecnologo")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Email,PhoneNumber")] ApplicationUser model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tecnologo = await _userManager.FindByIdAsync(id);
                    if (tecnologo == null)
                    {
                        return NotFound();
                    }

                    if (!await _userManager.IsInRoleAsync(tecnologo, "Tecnologo"))
                    {
                        return Unauthorized();
                    }

                    // Atualiza os campos básicos de ApplicationUser
                    tecnologo.UserName = model.UserName;
                    tecnologo.Email = model.Email;
                    tecnologo.PhoneNumber = model.PhoneNumber;

                    var result = await _userManager.UpdateAsync(tecnologo);
                    if (result.Succeeded)
                    {
                        TempData["MensagemSucesso"] = "Dados do tecnólogo atualizados com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception)
                {
                    TempData["MensagemErro"] = "Ocorreu um erro ao atualizar as informações do tecnólogo.";
                }
            }
            return View(model);
        }

        // GET: Tecnologo/Delete/5
        [Authorize(Roles = "Admin")] // Normalmente restrito apenas ao Admin do sistema
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tecnologo = await _userManager.FindByIdAsync(id);
            if (tecnologo == null)
            {
                return NotFound();
            }

            if (!await _userManager.IsInRoleAsync(tecnologo, "Tecnologo"))
            {
                return Unauthorized();
            }

            return View(tecnologo);
        }

        // POST: Tecnologo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tecnologo = await _userManager.FindByIdAsync(id);
            if (tecnologo == null)
            {
                return NotFound();
            }

            if (!await _userManager.IsInRoleAsync(tecnologo, "Tecnologo"))
            {
                return Unauthorized();
            }

            var result = await _userManager.DeleteAsync(tecnologo);
            if (result == null)
            {
                return NotFound();
            }

            TempData["MensagemSucesso"] = "Tecnólogo removido com sucesso.";
            return RedirectToAction(nameof(Index));
        }
    }
}