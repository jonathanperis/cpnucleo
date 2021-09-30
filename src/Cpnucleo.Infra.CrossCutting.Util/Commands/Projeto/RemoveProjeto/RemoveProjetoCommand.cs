namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto
{
    [DataContract]
    public class RemoveProjetoCommand
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
