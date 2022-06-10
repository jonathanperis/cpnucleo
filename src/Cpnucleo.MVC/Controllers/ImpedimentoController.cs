namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ImpedimentoController : BaseController
{
    private readonly IImpedimentoGrpcService _impedimentoGrpcService;

    private ImpedimentoViewModel _viewModel;

    public ImpedimentoController(IConfiguration configuration)
        : base(configuration)
    {
        _impedimentoGrpcService = MagicOnionClient.Create<IImpedimentoGrpcService>(CreateAuthenticatedChannel());
    }

    public ImpedimentoViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new ImpedimentoViewModel();
            }

            return _viewModel;
        }
        set => _viewModel = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            ListImpedimentoViewModel result = await _impedimentoGrpcService.ListImpedimento(new ListImpedimentoQuery { });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            ViewModel.Lista = result.Impedimentos;

            return View(ViewModel);
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
    public async Task<IActionResult> Incluir(ImpedimentoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            OperationResult result = await _impedimentoGrpcService.CreateImpedimento(new CreateImpedimentoCommand { Nome = obj.Impedimento.Nome });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

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
            await CarregarDados(id);

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(ImpedimentoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Impedimento.Id);

                return View(ViewModel);
            }

            OperationResult result = await _impedimentoGrpcService.UpdateImpedimento(new UpdateImpedimentoCommand { Id = obj.Impedimento.Id, Nome = obj.Impedimento.Nome });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

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
            await CarregarDados(id);

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(ImpedimentoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Impedimento.Id);

                return View(ViewModel);
            }

            OperationResult result = await _impedimentoGrpcService.RemoveImpedimento(new RemoveImpedimentoCommand { Id = obj.Impedimento.Id });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task CarregarDados(Guid id)
    {
        GetImpedimentoViewModel result = await _impedimentoGrpcService.GetImpedimento(new GetImpedimentoQuery { Id = id });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.Impedimento = result.Impedimento;
    }
}
