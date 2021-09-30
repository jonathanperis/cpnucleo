namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;

[DataContract]
public class ListProjetoResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<ProjetoViewModel> Projetos { get; set; }
}