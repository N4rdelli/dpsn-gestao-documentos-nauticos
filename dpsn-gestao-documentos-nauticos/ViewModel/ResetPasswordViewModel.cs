using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.ViewModel
{
    public class ResetPasswordViewModel
    {
        public string? UserId { get; set; }
        public string? Token { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A senha deve conter no minimo 6 caracteres")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        // Compara se as senhas são iguais
        [Compare("NewPassword", ErrorMessage = "As senhas estão diferentes")]
        public string? ConfirmPassword { get; set; }
    }
}
