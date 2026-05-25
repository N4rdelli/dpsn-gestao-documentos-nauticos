using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.Services;
using dpsn_gestao_documentos_nauticos.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    public class AccountsController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private EmailService _emailService;
        public AccountsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
        [ValidateAntiForgeryToken]
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
            }
            else
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

        //get forgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }
        // Post forgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Informe o e-mail");
                return View();
            }
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirm");
            }
            //preparar o link para o envio do e-mail
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var callbackUrl = Url.Action("ResetPassword", "Accounts",
                new { userId = user.Id, token = encodedToken }, Request.Scheme);
            //preparar os dados do email
            var assunto = "Redefinição de Senha";
            var corpo = $"Clique no Link para redefinir sua senha:" +
                $"<a href='{callbackUrl}'>Redefinir Senha</a>";
            //enviar o email
            await _emailService.SendEmailAsync(email, assunto, corpo);
            return RedirectToAction("ForgotPasswordConfirm");

        }
        public IActionResult ForgotPasswordConfirm()
        {
            return View();
        }

        public IActionResult ResetPassword(string token, string userId)
        {
            if (token == null || token == "")
            {
                ModelState.AddModelError("", "Token invalido");
            }
            var model = new ResetPasswordViewModel
            {
                Token = token,
                UserId = userId
            };
            return View();
        }
        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirm");
            }
            var decodeToken = HttpUtility.UrlDecode(model.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodeToken, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirm");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // Get EditPassword
        public async Task<IActionResult> EditPassword(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var model = new EditPasswordViewModel
            {
                Id = id
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPassword(EditPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Busca o documento original no MongoDB
                var estaleiro = (Estaleiro)await _userManager.FindByIdAsync(model.Id);
                if (estaleiro == null)
                {
                    return NotFound();
                
                }
                if (!await _userManager.CheckPasswordAsync(estaleiro, model.senhaAtual))
                {
                    TempData["MensagemErro"] = "Senha atual incorreta.";
                    return View(model);
                }
                // Gerar um token de redefinição de senha
                var token = await _userManager.GeneratePasswordResetTokenAsync(estaleiro);
                // Redefinir a senha usando o token
                var result = await _userManager.ResetPasswordAsync(estaleiro, token, model.novaSenha);
                if (result.Succeeded)
                {
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["MensagemErro"] = "Ocorreu um erro ao alterar a senha.";
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
