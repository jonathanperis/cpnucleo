using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public class RecursoTarefaRepository : IRecursoTarefaRepository
    {
        private readonly Context _context;

        private readonly IApontamentoRepository _apontamentoRepository;

        public RecursoTarefaRepository(Context context,
                                       IApontamentoRepository apontamentoRepository)
        {
            _context = context;
            _apontamentoRepository = apontamentoRepository;
        }        

        public async Task IncluirAsync(RecursoTarefaItem recursoTarefa)
        {
            recursoTarefa.DataInclusao = DateTime.Now;
            
            _context.RecursoTarefas.Add(recursoTarefa);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarAsync(RecursoTarefaItem recursoTarefa)
        {
            var recursoTarefaItem = await ConsultarAsync(recursoTarefa.IdRecursoTarefa);

            recursoTarefaItem.IdRecurso = recursoTarefa.IdRecurso;
            recursoTarefaItem.PercentualTarefa = recursoTarefa.PercentualTarefa;

            recursoTarefaItem.DataAlteracao = DateTime.Now;

            _context.RecursoTarefas.Update(recursoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<RecursoTarefaItem> ConsultarAsync(int idRecursoTarefa)
        {
            return await _context.RecursoTarefas
                .Include(x => x.Tarefa)
                .Include(x => x.Recurso)
                .SingleOrDefaultAsync(x => x.IdRecursoTarefa == idRecursoTarefa);
        }

        public async Task<IEnumerable<RecursoTarefaItem>> ListarPoridTarefaAsync(int idTarefa)
        {
            return await _context.RecursoTarefas
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .Include(x => x.Recurso)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdTarefa == idTarefa)                                
                .ToListAsync();
        }

        public async Task RemoverAsync(RecursoTarefaItem recursoTarefa)
        {
            var recursoTarefaItem = await ConsultarAsync(recursoTarefa.IdRecursoTarefa);

            _context.RecursoTarefas.Remove(recursoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<RecursoTarefaItem>> ListarAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RecursoTarefaItem>> ListarPoridRecursoAsync(int idRecurso)
        {
            var listaRecursoTarefa = await _context.RecursoTarefas
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .Include(x => x.Tarefa.Workflow)
                .Include(x => x.Tarefa.Recurso)
                .Include(x => x.Tarefa.ListaImpedimentos)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdRecurso == idRecurso)
                .ToListAsync();

            foreach (var item in listaRecursoTarefa)
            {
                item.HorasUtilizadas = await _apontamentoRepository.ObterTotalHorasPoridRecursoAsync(item.IdRecurso, item.IdTarefa);

                if (item.PercentualTarefa != null)
                {
                    double horasFracionadas = ((item.Tarefa.QtdHoras / 100.0) * item.PercentualTarefa.Value);               
                    item.HorasDisponiveis = (int)(horasFracionadas - item.HorasUtilizadas);
                }
            }     

            return listaRecursoTarefa;           
        }        
    }
}