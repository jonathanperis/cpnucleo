using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using dotnet_cpnucleo_pages.Repository.Apontamento;

namespace dotnet_cpnucleo_pages.Repository.RecursoTarefa
{
    public class RecursoTarefaRepository : IRecursoTarefaRepository
    {
        private readonly RecursoTarefaContext _context;

        private readonly IApontamentoRepository _apontamentoRepository;

        public RecursoTarefaRepository(RecursoTarefaContext context,
                                       IApontamentoRepository apontamentoRepository)
        {
            _context = context;
            _apontamentoRepository = apontamentoRepository;
        }        

        public async Task Incluir(RecursoTarefaItem recursoTarefa)
        {
            recursoTarefa.DataInclusao = DateTime.Now;
            
            _context.RecursoTarefas.Add(recursoTarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(RecursoTarefaItem recursoTarefa)
        {
            var recursoTarefaItem = _context.RecursoTarefas.Find(recursoTarefa.IdRecursoTarefa);
            recursoTarefaItem.IdRecurso = recursoTarefa.IdRecurso;
            recursoTarefaItem.PercentualTarefa = recursoTarefa.PercentualTarefa;

            recursoTarefaItem.DataAlteracao = DateTime.Now;

            _context.RecursoTarefas.Update(recursoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<RecursoTarefaItem> Consultar(int idRecursoTarefa)
        {
            return await _context.RecursoTarefas
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .Include(x => x.Recurso)
                .SingleOrDefaultAsync(x => x.IdRecursoTarefa == idRecursoTarefa);
        }

        public async Task<IList<RecursoTarefaItem>> ListarPoridTarefa(int idTarefa)
        {
            return await _context.RecursoTarefas
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .Include(x => x.Recurso)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdTarefa == idTarefa)                                
                .ToListAsync();
        }

        public async Task Remover(RecursoTarefaItem recursoTarefa)
        {
            var recursoTarefaItem = _context.RecursoTarefas.Find(recursoTarefa.IdRecursoTarefa);

            _context.RecursoTarefas.Remove(recursoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public Task<IList<RecursoTarefaItem>> Listar()
        {
            throw new NotImplementedException();
        }

        public async Task<IList<RecursoTarefaItem>> ListarPoridRecurso(int idRecurso)
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
                item.HorasUtilizadas = await _apontamentoRepository.ObterTotalHorasPoridRecurso(item.IdRecurso, item.IdTarefa);

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