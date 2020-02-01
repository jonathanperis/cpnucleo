﻿using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Workflow
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IWorkflowApiService _workflowApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    IWorkflowApiService workflowApiService)
            : base(claimsManager)
        {
            _workflowApiService = workflowApiService;
        }

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Workflow = _workflowApiService.Consultar(Token, id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _workflowApiService.Remover(Token, Workflow.Id);

            return RedirectToPage("Listar");
        }
    }
}