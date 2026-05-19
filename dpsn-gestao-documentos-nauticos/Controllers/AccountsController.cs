using dpsn_gestao_documentos_nauticos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    public class AccountsController:Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Get login
        // Permite o usuario acessar o método sem estar logado
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // Post Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [Required][EmailAddress] string email,
            [Required] string password, bool manterConectado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Busaca o usuário pelo email
                    ApplicationUser appuser = await _userManager.FindByEmailAsync(email);
                    if (appuser != null)
                    {
                        // Verifica a senha e realiza o login
                        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appuser, password, manterConectado, false);
                        if (result.Succeeded)
                        {
                            TempData["MensagemSucesso"] = $"Login realizado com sucesso! Bem vindo.";
                            return RedirectToAction("Index", "Home");
                        }
                        ModelState.AddModelError(nameof(email), "Verifique as credenciais");
                    }
                    ModelState.AddModelError(nameof(email), "Usuario não foi encontrado");

                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = "Ocorreu um erro ao realizar o login. Verifique as credencias";
                    Console.WriteLine(ex.Message);
                }
            } else
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao realizar o login. Verifique as credencias";
            }
            return View();
        }//login

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Accounts");
        }
    }
}
