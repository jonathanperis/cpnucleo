using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly Context _context;

        public WorkflowRepository(Context context) => _context = context;

        public async Task IncluirAsync(WorkflowItem workflow)
        {
            workflow.DataInclusao = DateTime.Now;

            _context.Workflows.Add(workflow);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarAsync(WorkflowItem workflow)
        {
            var workflowItem = await ConsultarAsync(workflow.IdWorkflow);

            workflowItem.Nome = workflow.Nome;
            workflowItem.Ordem = workflow.Ordem;

            workflowItem.DataAlteracao = DateTime.Now;

            _context.Workflows.Update(workflowItem);
            await _context.SaveChangesAsync();
        }

        public async Task<WorkflowItem> ConsultarAsync(int idWorkflow)
        {
            return await _context.Workflows
                .SingleOrDefaultAsync(x => x.IdWorkflow == idWorkflow);
        }

        public async Task<IEnumerable<WorkflowItem>> ListarAsync()
        {
            return await _context.Workflows
                .AsNoTracking()
                .OrderBy(x => x.DataInclusao)
                .ToListAsync();
        }

        public async Task RemoverAsync(WorkflowItem workflow)
        {    
            var workflowItem = await ConsultarAsync(workflow.IdWorkflow);            

            _context.Workflows.Remove(workflowItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkflowItem>> ListarTarefasWorkflowAsync()
        {
            var listaWorkflow =  await _context.Workflows
                .AsNoTracking()
                .Include(x => x.ListaTarefas)
                    .ThenInclude(x => x.Recurso)                
                .Include(x => x.ListaTarefas)
                    .ThenInclude(x => x.ListaApontamentos)
                .Include(x => x.ListaTarefas)
                    .ThenInclude(x => x.ListaImpedimentos)                    
                .OrderBy(x => x.Ordem)
                .Take(4) //@@JONATHAN - 22/02/2017 - TRAVA TEMPORÁRIA.
                .ToListAsync();

            foreach(var item in listaWorkflow)
            {
                foreach(var tarefa in item.ListaTarefas)
                {
                    tarefa.HorasConsumidas = tarefa.ListaApontamentos.Sum(x=> x.QtdHoras);
                    tarefa.HorasRestantes = tarefa.QtdHoras - tarefa.HorasConsumidas;    
                }
            }
            
            return listaWorkflow;    
        }
    }
}