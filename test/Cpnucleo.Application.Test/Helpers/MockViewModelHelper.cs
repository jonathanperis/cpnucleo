using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;

namespace Cpnucleo.Application.Test.Helpers
{
    internal class MockViewModelHelper
    {
        public static SistemaViewModel GetNewSistema(Guid sistemaId = default, DateTime dataInclusao = default)
        {
            return new SistemaViewModel
            {
                Id = sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
                DataInclusao = dataInclusao
            };
        }

        public static ProjetoViewModel GetNewProjeto(Guid sistemaId, Guid projetoId = default, DateTime dataInclusao = default)
        {
            return new ProjetoViewModel
            {
                Id = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId,
                Nome = "Projeto de teste",
                IdSistema = sistemaId,
                DataInclusao = dataInclusao
            };
        }

        public static WorkflowViewModel GetNewWorkflow(Guid workflowId = default, DateTime dataInclusao = default)
        {
            return new WorkflowViewModel
            {
                Id = workflowId == Guid.Empty ? Guid.NewGuid() : workflowId,
                Nome = "Workflow de teste",
                Ordem = 1,
                DataInclusao = dataInclusao
            };
        }

        public static TipoTarefaViewModel GetNewTipoTarefa(Guid tipoTarefaId = default, DateTime dataInclusao = default)
        {
            return new TipoTarefaViewModel
            {
                Id = tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId,
                Nome = "TipoTarefa de teste",
                Element = "success-element",
                Image = "success.png",
                DataInclusao = dataInclusao
            };
        }

        public static RecursoViewModel GetNewRecurso(Guid recursoId = default, DateTime dataInclusao = default)
        {
            return new RecursoViewModel
            {
                Id = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId,
                Nome = "Recurso de teste",
                Login = "usuario.teste",
                Senha = "12345678",
                DataInclusao = dataInclusao
            };
        }

        public static ImpedimentoViewModel GetNewImpedimento(Guid impedimentoId = default, DateTime dataInclusao = default)
        {
            return new ImpedimentoViewModel
            {
                Id = impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId,
                Nome = "Impedimento de teste",
                DataInclusao = dataInclusao
            };
        }

        public static ApontamentoViewModel GetNewApontamento(Guid tarefaId, Guid recursoId, Guid apontamentoId = default, DateTime dataInclusao = default)
        {
            return new ApontamentoViewModel
            {
                Id = apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId,
                Descricao = "Descrição do Apontamento de teste",
                DataApontamento = DateTime.Now,
                QtdHoras = 8,
                IdTarefa = tarefaId,
                IdRecurso = recursoId,
                DataInclusao = dataInclusao
            };
        }
    }
}
