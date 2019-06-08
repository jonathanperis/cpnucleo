using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cpnucleo.Pages.Repository.Recurso
{
    [Table("CPN_TB_RECURSO")]
    public class RecursoItem
    {
        public RecursoItem()
        {
            Ativo = true;
        }

        [Key]
        [Display(Name = "Código Recurso")]      
        [Column("REC_ID", TypeName = "int")]
        public int IdRecurso { get; set; }

        [Display(Name = "Nome")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Recurso.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("REC_NOME", TypeName = "varchar(80)")]  
        public string Nome { get; set; }

        [Display(Name = "Ativo")]      
        [Column("REC_ATIVO", TypeName = "bit")]
        public bool Ativo { get; set; }         

        [Display(Name = "Login")]      
        [Required(ErrorMessage = "Necessário informar o {0} do Recurso.")]
        [MaxLength(50, ErrorMessage = "{0} pode conter no máximo {1} caractéres.")]
        [Column("REC_LOGIN", TypeName = "varchar(50)")]  
        public string Login { get; set; }        

        [NotMapped]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]        
        [Required(ErrorMessage = "Necessário informar a {0} do Recurso.")]
        [StringLength(50, ErrorMessage = "A {0} deve conter ao menos {2} e o máximo {1} caractéres.", MinimumLength = 8)]
        public string Senha { get; set; }

        [NotMapped]
        [Display(Name = "Confirmação de Senha")]        
        [DataType(DataType.Password)]        
        [Required(ErrorMessage = "Necessário informar a {0} do Recurso.")]
        [Compare("Senha", ErrorMessage = "A {1} e a {0} não correspondem.")]
        public string ConfirmarSenha { get; set; }

        [Column("REC_SENHA", TypeName = "varchar(64)")]
        public string SenhaCriptografada { get; set; }

        [Column("REC_SENHA_SALT", TypeName = "varchar(64)")]
        public string Salt { get; set; }        

        [Display(Name = "Data de Inclusão")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("REC_DATA_INCLUSAO", TypeName = "datetime")]
        public DateTime? DataInclusao { get; set; }

        [Display(Name = "Data de Alteração")]      
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Column("REC_DATA_ALTERACAO", TypeName = "datetime")]
        public DateTime? DataAlteracao { get; set; }                
    }
}