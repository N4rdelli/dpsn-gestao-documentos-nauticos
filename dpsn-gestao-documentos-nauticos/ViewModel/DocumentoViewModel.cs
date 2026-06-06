using dpsn_gestao_documentos_nauticos.Models;
using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.ViewModel
{
    public class DocumentoViewModel
    {
        // Listas para preencher os Dropdowns na View
        public List<Estaleiro>? Estaleiros { get; set; }
        public List<Embarcacao>? Embarcacoes { get; set; }

        // IDs selecionados no formulário
        [Required(ErrorMessage = "O estaleiro é obrigatório.")]
        public string EstaleiroId { get; set; }

        [Required(ErrorMessage = "A embarcação é obrigatória.")]
        public string EmbarcacaoId { get; set; }

        // Dados do Cliente (Armador)
        [Required(ErrorMessage = "O ano de construção é obrigatório.")]
        public string Ano_contrucao { get; set; }
        [Required(ErrorMessage = "O número do chassi é obrigatório.")]
        public string NumeroChassi { get; set; }
        [Required(ErrorMessage = "O nome do armador é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O CPF ou CNPJ é obrigatório.")]
        public string Cpf_cnpj { get; set; }

        // Endereço do Cliente
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [MaxLength(8, ErrorMessage = "CEP inválido.")]
        public string Cep { get; set; }
        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        public string Logradouro { get; set; }
        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string Estado { get; set; }

        public string NumeroInscricao { get; set; } = "A ser inscrita";
    }
}