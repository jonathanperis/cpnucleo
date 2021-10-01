using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class RecursoController : BaseController
{
    private readonly IRecursoGrpcService _recursoGrpcService;

    private RecursoView _recursoView;

    public RecursoController(IConfiguration configuration)
        : base(configuration)
    {
        _recursoGrpcService = MagicOnionClient.Create<IRecursoGrpcService>(CreateAuthenticatedChannel());
    }

    public RecursoView RecursoView
    {
        get
        {
            if (_recursoView == null)
            {
                _recursoView = new RecursoView();
            }

            return _recursoView;
        }
        set => _recursoView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            ListRecursoResponse response = await _recursoGrpcService.AllAsync(new ListRecursoQuery { });
            RecursoView.Lista = response.Recursos;

            return View(RecursoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public IActionResult Incluir()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(RecursoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _recursoGrpcService.AddAsync(new CreateRecursoCommand { Recurso = obj.Recurso });

            return RedirectToAction("Listar");
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
            GetRecursoResponse response = await _recursoGrpcService.GetAsync(new GetRecursoQuery { Id = id });
            RecursoView.Recurso = response.Recurso;

            return View(RecursoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(RecursoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                GetRecursoResponse response = await _recursoGrpcService.GetAsync(new GetRecursoQuery { Id = obj.Recurso.Id });
                RecursoView.Recurso = response.Recurso;

                return View(RecursoView);
            }

            await _recursoGrpcService.UpdateAsync(new UpdateRecursoCommand { Recurso = obj.Recurso });

            return RedirectToAction("Listar");
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
            GetRecursoResponse response = await _recursoGrpcService.GetAsync(new GetRecursoQuery { Id = id });
            RecursoView.Recurso = response.Recurso;

            return View(RecursoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(RecursoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                GetRecursoResponse response = await _recursoGrpcService.GetAsync(new GetRecursoQuery { Id = obj.Recurso.Id });
                RecursoView.Recurso = response.Recurso;

                return View(RecursoView);
            }

            await _recursoGrpcService.RemoveAsync(new RemoveRecursoCommand { Id = obj.Recurso.Id });

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }
}
