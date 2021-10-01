using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ImpedimentoController : BaseController
{
    private readonly IImpedimentoGrpcService _impedimentoGrpcService;

    private ImpedimentoView _impedimentoView;

    public ImpedimentoController(IConfiguration configuration)
        : base(configuration)
    {
        _impedimentoGrpcService = MagicOnionClient.Create<IImpedimentoGrpcService>(CreateAuthenticatedChannel());
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
            ListImpedimentoResponse response = await _impedimentoGrpcService.AllAsync(new ListImpedimentoQuery { });
            ImpedimentoView.Lista = response.Impedimentos;

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
            GetImpedimentoResponse response = await _impedimentoGrpcService.GetAsync(new GetImpedimentoQuery { Id = id });
            ImpedimentoView.Impedimento = response.Impedimento;

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
                GetImpedimentoResponse response = await _impedimentoGrpcService.GetAsync(new GetImpedimentoQuery { Id = obj.Impedimento.Id });
                ImpedimentoView.Impedimento = response.Impedimento;

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
            GetImpedimentoResponse response = await _impedimentoGrpcService.GetAsync(new GetImpedimentoQuery { Id = id });
            ImpedimentoView.Impedimento = response.Impedimento;

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
                GetImpedimentoResponse response = await _impedimentoGrpcService.GetAsync(new GetImpedimentoQuery { Id = obj.Impedimento.Id });
                ImpedimentoView.Impedimento = response.Impedimento;

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
