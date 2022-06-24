using Cpnucleo.Shared.Common.DTOs;
using System.Security.Claims;

namespace Cpnucleo.MVC.Models;

public class TarefaViewModel
{
    public TarefaDTO Tarefa { get; set; }

    public IEnumerable<TarefaDTO> Lista { get; set; }

    public SelectList SelectProjetos { get; set; }

    public SelectList SelectSistemas { get; set; }

    public SelectList SelectWorkflows { get; set; }

    public SelectList SelectTipoTarefas { get; set; }

    public ClaimsPrincipal User { get; set; }
}
