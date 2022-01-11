using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class RecursoProjetoController : BaseController
{
    private readonly IRecursoProjetoGrpcService _recursoProjetoGrpcService;
    private readonly IRecursoGrpcService _recursoGrpcService;
    private readonly IProjetoGrpcService _projetoGrpcService;

    private RecursoProjetoView _recursoProjetoView;

    public RecursoProjetoController(IConfiguration configuration)
    {
        _recursoProjetoGrpcService = MagicOnionClient.Create<IRecursoProjetoGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcRecursoProjeto"]));
        _recursoGrpcService = MagicOnionClient.Create<IRecursoGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcRecurso"]));
        _projetoGrpcService = MagicOnionClient.Create<IProjetoGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcProjeto"]));
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
            RecursoProjetoView.Lista = await _recursoProjetoGrpcService.GetByProjetoAsync(new GetByProjetoQuery { IdProjeto = idProjeto });

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
            RecursoProjetoView.RecursoProjeto = await _recursoProjetoGrpcService.GetAsync(new GetRecursoProjetoQuery { Id = id });

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
                RecursoProjetoView.RecursoProjeto = await _recursoProjetoGrpcService.GetAsync(new GetRecursoProjetoQuery { Id = obj.RecursoProjeto.Id });

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
            RecursoProjetoView.RecursoProjeto = await _recursoProjetoGrpcService.GetAsync(new GetRecursoProjetoQuery { Id = id });

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
                RecursoProjetoView.RecursoProjeto = await _recursoProjetoGrpcService.GetAsync(new GetRecursoProjetoQuery { Id = obj.RecursoProjeto.Id });

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
        RecursoProjetoView.Projeto = await _projetoGrpcService.GetAsync(new GetProjetoQuery { Id = idProjeto });
    }

    private async Task CarregarSelectRecursos()
    {
        IEnumerable<RecursoViewModel> response = await _recursoGrpcService.AllAsync(new ListRecursoQuery { });
        RecursoProjetoView.SelectRecursos = new SelectList(response, "Id", "Nome");
    }
}
