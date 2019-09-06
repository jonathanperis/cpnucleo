using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Projeto
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRepository<ProjetoModel> _projetoRepository;

        public RemoverModel(IRepository<ProjetoModel> projetoRepository) => _projetoRepository = projetoRepository;

        [BindProperty]
        public ProjetoModel Projeto { get; set; }

        public async Task<IActionResult> OnGetAsync(int idProjeto)
        {
            Projeto = await _projetoRepository.ConsultarAsync(idProjeto);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _projetoRepository.RemoverAsync(Projeto);

            return RedirectToPage("Listar");
        }
    }
}