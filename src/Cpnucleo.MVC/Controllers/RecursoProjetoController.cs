using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class RecursoProjetoController : BaseController
{
    private readonly IRecursoProjetoGrpcService _recursoProjetoGrpcService;
    private readonly IRecursoGrpcService _recursoGrpcService;
    private readonly IProjetoGrpcService _projetoGrpcService;

    private RecursoProjetoView _recursoProjetoView;

    public RecursoProjetoController(IConfiguration configuration)
        : base(configuration)
    {
        _recursoProjetoGrpcService = MagicOnionClient.Create<IRecursoProjetoGrpcService>(CreateAuthenticatedChannel());
        _recursoGrpcService = MagicOnionClient.Create<IRecursoGrpcService>(CreateAuthenticatedChannel());
        _projetoGrpcService = MagicOnionClient.Create<IProjetoGrpcService>(CreateAuthenticatedChannel());
    }

    public RecursoProjetoView RecursoProjetoView
    {
        get
        {
            if (_recursoProjetoView == null)
            {
                _recursoProjetoView = new RecursoProjetoView();
            }

            return _recursoProjetoView;
        }
        set => _recursoProjetoView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(Guid idProjeto)
    {
        try
        {
            GetByProjetoResponse response = await _recursoProjetoGrpcService.GetByProjetoAsync(new GetByProjetoQuery { IdProjeto = idProjeto });
            RecursoProjetoView.Lista = response.RecursoProjetos;

            ViewData["idProjeto"] = idProjeto;

            return View(RecursoProjetoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Incluir(Guid idProjeto)
    {
        await ObterProjeto(idProjeto);
        await CarregarSelectRecursos();

        return View(RecursoProjetoView);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(RecursoProjetoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await ObterProjeto(obj.Projeto.Id);
                await CarregarSelectRecursos();

                return View(RecursoProjetoView);
            }

            await _recursoProjetoGrpcService.AddAsync(new CreateRecursoProjetoCommand { RecursoProjeto = obj.RecursoProjeto });

            return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Alterar(Guid id)
    {
        try
        {
            GetRecursoProjetoResponse response = await _recursoProjetoGrpcService.GetAsync(new GetRecursoProjetoQuery { Id = id });
            RecursoProjetoView.RecursoProjeto = response.RecursoProjeto;

            await CarregarSelectRecursos();

            return View(RecursoProjetoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(RecursoProjetoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                GetRecursoProjetoResponse response = await _recursoProjetoGrpcService.GetAsync(new GetRecursoProjetoQuery { Id = obj.RecursoProjeto.Id });
                RecursoProjetoView.RecursoProjeto = response.RecursoProjeto;

                await CarregarSelectRecursos();

                return View(RecursoProjetoView);
            }

            await _recursoProjetoGrpcService.UpdateAsync(new UpdateRecursoProjetoCommand { RecursoProjeto = obj.RecursoProjeto });

            return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Remover(Guid id)
    {
        try
        {
            GetRecursoProjetoResponse response = await _recursoProjetoGrpcService.GetAsync(new GetRecursoProjetoQuery { Id = id });
            RecursoProjetoView.RecursoProjeto = response.RecursoProjeto;

            return View(RecursoProjetoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(RecursoProjetoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                GetRecursoProjetoResponse response = await _recursoProjetoGrpcService.GetAsync(new GetRecursoProjetoQuery { Id = obj.RecursoProjeto.Id });
                RecursoProjetoView.RecursoProjeto = response.RecursoProjeto;

                return View(RecursoProjetoView);
            }

            await _recursoProjetoGrpcService.RemoveAsync(new RemoveRecursoProjetoCommand { Id = obj.RecursoProjeto.Id });

            return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task ObterProjeto(Guid idProjeto)
    {
        GetProjetoResponse response = await _projetoGrpcService.GetAsync(new GetProjetoQuery { Id = idProjeto });
        RecursoProjetoView.Projeto = response.Projeto;
    }

    private async Task CarregarSelectRecursos()
    {
        ListRecursoResponse response = await _recursoGrpcService.AllAsync(new ListRecursoQuery { });
        RecursoProjetoView.SelectRecursos = new SelectList(response.Recursos, "Id", "Nome");
    }
}
