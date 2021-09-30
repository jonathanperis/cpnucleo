namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;

[DataContract]
public class CreateProjetoCommand
{
    [DataMember(Order = 1)]
    public ProjetoViewModel Projeto { get; set; }
}