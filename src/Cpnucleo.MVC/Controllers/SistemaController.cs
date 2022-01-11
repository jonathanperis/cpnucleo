using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class SistemaController : BaseController
{
    private readonly ISistemaGrpcService _sistemaGrpcService;

    private SistemaView _sistemaView;

    public SistemaController(IConfiguration configuration)
    {
        _sistemaGrpcService = MagicOnionClient.Create<ISistemaGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcSistema"]));
    }

    public SistemaView SistemaView
    {
        get
        {
            if (_sistemaView == null)
            {
                _sistemaView = new SistemaView();
            }

            return _sistemaView;
        }
        set => _sistemaView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            SistemaView.Lista = await _sistemaGrpcService.AllAsync(new ListSistemaQuery { });

            return View(SistemaView);
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
    public async Task<IActionResult> Incluir(SistemaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _sistemaGrpcService.AddAsync(new CreateSistemaCommand { Sistema = obj.Sistema });

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
            SistemaView.Sistema = await _sistemaGrpcService.GetAsync(new GetSistemaQuery { Id = id });

            return View(SistemaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(SistemaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                SistemaView.Sistema = await _sistemaGrpcService.GetAsync(new GetSistemaQuery { Id = obj.Sistema.Id });

                return View(SistemaView);
            }

            await _sistemaGrpcService.UpdateAsync(new UpdateSistemaCommand { Sistema = obj.Sistema });

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
            SistemaView.Sistema = await _sistemaGrpcService.GetAsync(new GetSistemaQuery { Id = id });

            return View(SistemaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(SistemaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                SistemaView.Sistema = await _sistemaGrpcService.GetAsync(new GetSistemaQuery { Id = obj.Sistema.Id });

                return View(SistemaView);
            }

            await _sistemaGrpcService.RemoveAsync(new RemoveSistemaCommand { Id = obj.Sistema.Id });

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }
}
