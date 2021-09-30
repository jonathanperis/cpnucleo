namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;

[DataContract]
public class GetProjetoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public ProjetoViewModel Projeto { get; set; }
}