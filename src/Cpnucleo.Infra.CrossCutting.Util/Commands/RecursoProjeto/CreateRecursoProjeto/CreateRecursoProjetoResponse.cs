namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto
{
    [DataContract]
    public class CreateRecursoProjetoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }
    }
}
