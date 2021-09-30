namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;

[DataContract]
public class UpdateRecursoProjetoCommand
{
    [DataMember(Order = 1)]
    public RecursoProjetoViewModel RecursoProjeto { get; set; }
}