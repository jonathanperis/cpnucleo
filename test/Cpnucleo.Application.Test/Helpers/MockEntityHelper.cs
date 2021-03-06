﻿using Cpnucleo.Domain.Entities;
using System;

namespace Cpnucleo.Application.Test.Helpers
{
    public class MockEntityHelper
    {
        public static Apontamento GetNewApontamento(Guid tarefaId, Guid recursoId, Guid apontamentoId = default)
        {
            return new Apontamento
            {
                Id = apontamentoId == Guid.Empty ? Guid.NewGuid() : apontamentoId,
                Descricao = "Descrição do Apontamento de teste",
                DataApontamento = DateTime.Now,
                QtdHoras = 8,
                IdTarefa = tarefaId,
                IdRecurso = recursoId,
            };
        }

        public static Impedimento GetNewImpedimento(Guid impedimentoId = default)
        {
            return new Impedimento
            {
                Id = impedimentoId == Guid.Empty ? Guid.NewGuid() : impedimentoId,
                Nome = "Impedimento de teste",
            };
        }

        public static ImpedimentoTarefa GetNewImpedimentoTarefa(Guid tarefaId, Guid impedimentoId, Guid impedimentoTarefaId = default)
        {
            return new ImpedimentoTarefa
            {
                Id = impedimentoTarefaId == Guid.Empty ? Guid.NewGuid() : impedimentoTarefaId,
                Descricao = "Descrição do Impedimento Tarefa de teste",
                IdTarefa = tarefaId,
                IdImpedimento = impedimentoId
            };
        }

        public static Projeto GetNewProjeto(Guid sistemaId, Guid projetoId = default)
        {
            return new Projeto
            {
                Id = projetoId == Guid.Empty ? Guid.NewGuid() : projetoId,
                Nome = "Projeto de teste",
                IdSistema = sistemaId
            };
        }

        public static Recurso GetNewRecurso(Guid recursoId = default)
        {
            return new Recurso
            {
                Id = recursoId == Guid.Empty ? Guid.NewGuid() : recursoId,
                Nome = "Recurso de teste",
                Login = "usuario.teste",
                Senha = "12345678",
            };
        }

        public static RecursoProjeto GetNewRecursoProjeto(Guid projetoId, Guid recursoId, Guid recursoProjetoId = default)
        {
            return new RecursoProjeto
            {
                Id = recursoProjetoId == Guid.Empty ? Guid.NewGuid() : recursoProjetoId,
                IdProjeto = projetoId,
                IdRecurso = recursoId
            };
        }

        public static RecursoTarefa GetNewRecursoTarefa(Guid tarefaId, Guid recursoId, Guid recursoTarefaId = default)
        {
            return new RecursoTarefa
            {
                Id = recursoTarefaId == Guid.Empty ? Guid.NewGuid() : recursoTarefaId,
                PercentualTarefa = 25,
                IdTarefa = tarefaId,
                IdRecurso = recursoId,
            };
        }

        public static Sistema GetNewSistema(Guid sistemaId = default)
        {
            return new Sistema
            {
                Id = sistemaId == Guid.Empty ? Guid.NewGuid() : sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
            };
        }

        public static Tarefa GetNewTarefa(Guid projetoId, Guid workflowId, Guid recursoId, Guid tipoTarefaId, Guid tarefaId = default)
        {
            return new Tarefa
            {
                Id = tarefaId == Guid.Empty ? Guid.NewGuid() : tarefaId,
                Nome = "Sistema de teste",
                DataInicio = DateTime.Now,
                DataTermino = DateTime.Now.AddDays(5),
                QtdHoras = 40,
                IdProjeto = projetoId,
                IdWorkflow = workflowId,
                IdRecurso = recursoId,
                IdTipoTarefa = tipoTarefaId,
            };
        }

        public static TipoTarefa GetNewTipoTarefa(Guid tipoTarefaId = default)
        {
            return new TipoTarefa
            {
                Id = tipoTarefaId == Guid.Empty ? Guid.NewGuid() : tipoTarefaId,
                Nome = "TipoTarefa de teste",
                Element = "success-element",
                Image = "success.png",
            };
        }

        public static Workflow GetNewWorkflow(Guid workflowId = default)
        {
            return new Workflow
            {
                Id = workflowId == Guid.Empty ? Guid.NewGuid() : workflowId,
                Nome = "Workflow de teste",
                Ordem = 1,
            };
        }
    }
}
