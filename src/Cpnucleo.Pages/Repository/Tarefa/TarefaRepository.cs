using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository.Tarefa
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly Context _context;

        public TarefaRepository(Context context) => _context = context;

        public async Task Incluir(TarefaItem tarefa)
        {
            tarefa.DataInclusao = DateTime.Now;

            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(TarefaItem tarefa)
        {
            var tarefaItem = _context.Tarefas.Find(tarefa.IdTarefa);
            tarefaItem.Nome = tarefa.Nome;
            tarefaItem.IdProjeto = tarefa.IdProjeto;
            tarefaItem.DataInicio = tarefa.DataInicio;
            tarefaItem.DataTermino = tarefa.DataTermino;
            tarefaItem.QtdHoras = tarefa.QtdHoras;
            tarefaItem.Detalhe = tarefa.Detalhe;
            tarefaItem.IdWorkflow = tarefa.IdWorkflow;
            tarefaItem.PercentualConcluido = tarefa.PercentualConcluido;
            tarefaItem.IdRecurso = tarefa.IdRecurso;
            tarefaItem.IdTipoTarefa = tarefa.IdTipoTarefa;

            tarefaItem.DataAlteracao = tarefa.DataAlteracao;

            _context.Tarefas.Update(tarefaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<TarefaItem> Consultar(int idTarefa)
        {
            return await _context.Tarefas
                .AsNoTracking()
                .Include(x => x.Projeto)
                .Include(x => x.Projeto.Sistema)
                .Include(x => x.Workflow)
                .Include(x => x.Recurso)
                .Include(x => x.TipoTarefa)
                .SingleOrDefaultAsync(x => x.IdTarefa == idTarefa);
        }

        public async Task<IEnumerable<TarefaItem>> Listar()
        {
            return await _context.Tarefas
                .AsNoTracking()
                .Include(x => x.Projeto)
                .Include(x => x.Projeto.Sistema)
                .OrderBy(x => x.DataInclusao)
                .ToListAsync();
        }     

        public async Task Remover(TarefaItem tarefa)
        {    
            var tarefaItem = _context.Tarefas.Find(tarefa.IdTarefa);            

            _context.Tarefas.Remove(tarefaItem);
            await _context.SaveChangesAsync();
        }

        public void AlterarPorFluxoTrabalho(int idTarefa, int idWorkflow)
        {
            lock (this)
            {
                var tarefaItem = _context.Tarefas
                    .SingleOrDefault(x => x.IdTarefa == idTarefa);

                if (tarefaItem != null)
                {
                    tarefaItem.IdWorkflow = idWorkflow;

                    _context.Tarefas.Update(tarefaItem);
                }

                _context.SaveChanges();                
            }
        }   
    }
}