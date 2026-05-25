using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.ViewModel
{
    public class EditPasswordViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "A senha atual é obrigatória.")]
        public string senhaAtual { get; set; }
        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [DataType(DataType.Password)]
        public string novaSenha { get; set; }
        [Required(ErrorMessage = "A confirmação da nova senha é obrigatória.")]
        // Compara se as senhas são iguais
        [Compare("novaSenha", ErrorMessage = "As senhas estão diferentes")]
        public string novaSenhaConfirm { get; set; }
    }
}
