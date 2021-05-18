using System.Collections.Generic;
using System.Security.Claims;
using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.MVC.Models
{
    public class TarefaViewModel
    {
        public Tarefa Tarefa { get; set; }

        public IEnumerable<Tarefa> Lista { get; set; }

        public SelectList SelectProjetos { get; set; }

        public SelectList SelectSistemas { get; set; }

        public SelectList SelectWorkflows { get; set; }

        public SelectList SelectTipoTarefas { get; set; }

        public ClaimsPrincipal User { get; set; }
    }
}