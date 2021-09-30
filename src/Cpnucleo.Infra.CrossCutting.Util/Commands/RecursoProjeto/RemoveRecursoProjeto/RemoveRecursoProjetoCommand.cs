namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;

[DataContract]
public class RemoveRecursoProjetoCommand
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}