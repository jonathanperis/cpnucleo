namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto
{
    [DataContract]
    public class UpdateProjetoCommand
    {
        [DataMember(Order = 1)]
        public ProjetoViewModel Projeto { get; set; }
    }
}
