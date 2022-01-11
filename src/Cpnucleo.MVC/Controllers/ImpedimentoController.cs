using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ImpedimentoController : BaseController
{
    private readonly IImpedimentoGrpcService _impedimentoGrpcService;

    private ImpedimentoView _impedimentoView;

    public ImpedimentoController(IConfiguration configuration)
    {
        _impedimentoGrpcService = MagicOnionClient.Create<IImpedimentoGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcImpedimento"]));
    }

    public ImpedimentoView ImpedimentoView
    {
        get
        {
            if (_impedimentoView == null)
            {
                _impedimentoView = new ImpedimentoView();
            }

            return _impedimentoView;
        }
        set => _impedimentoView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            ImpedimentoView.Lista = await _impedimentoGrpcService.AllAsync(new ListImpedimentoQuery { });

            return View(ImpedimentoView);
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
    public async Task<IActionResult> Incluir(ImpedimentoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _impedimentoGrpcService.AddAsync(new CreateImpedimentoCommand { Impedimento = obj.Impedimento });

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
            ImpedimentoView.Impedimento = await _impedimentoGrpcService.GetAsync(new GetImpedimentoQuery { Id = id });

            return View(ImpedimentoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(ImpedimentoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ImpedimentoView.Impedimento = await _impedimentoGrpcService.GetAsync(new GetImpedimentoQuery { Id = obj.Impedimento.Id });

                return View(ImpedimentoView);
            }

            await _impedimentoGrpcService.UpdateAsync(new UpdateImpedimentoCommand { Impedimento = obj.Impedimento });

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
            ImpedimentoView.Impedimento = await _impedimentoGrpcService.GetAsync(new GetImpedimentoQuery { Id = id });

            return View(ImpedimentoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(ImpedimentoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ImpedimentoView.Impedimento = await _impedimentoGrpcService.GetAsync(new GetImpedimentoQuery { Id = obj.Impedimento.Id });

                return View(ImpedimentoView);
            }

            await _impedimentoGrpcService.RemoveAsync(new RemoveImpedimentoCommand { Id = obj.Impedimento.Id });

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }
}
