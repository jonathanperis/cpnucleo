using Cpnucleo.Shared.Queries.GetSistema;
using Cpnucleo.Shared.Queries.ListSistema;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class SistemaController : BaseController
{
    private readonly ISistemaGrpcService _sistemaGrpcService;

    private SistemaViewModel _viewModel;

    public SistemaController(IConfiguration configuration)
        : base(configuration)
    {
        _sistemaGrpcService = MagicOnionClient.Create<ISistemaGrpcService>(CreateAuthenticatedChannel());
    }

    public SistemaViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new SistemaViewModel();
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
            ListSistemaViewModel result = await _sistemaGrpcService.ListSistema(new ListSistemaQuery());

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            ViewModel.Lista = result.Sistemas;

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
    public async Task<IActionResult> Incluir(SistemaViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            OperationResult result = await _sistemaGrpcService.CreateSistema(new CreateSistemaCommand(obj.Sistema.Nome, obj.Sistema.Descricao));

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
    public async Task<IActionResult> Alterar(SistemaViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Sistema.Id);

                return View(ViewModel);
            }

            OperationResult result = await _sistemaGrpcService.UpdateSistema(new UpdateSistemaCommand(obj.Sistema.Id, obj.Sistema.Nome, obj.Sistema.Descricao));

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
    public async Task<IActionResult> Remover(SistemaViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Sistema.Id);

                return View(ViewModel);
            }

            OperationResult result = await _sistemaGrpcService.RemoveSistema(new RemoveSistemaCommand(obj.Sistema.Id));

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
        GetSistemaViewModel result = await _sistemaGrpcService.GetSistema(new GetSistemaQuery(id));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.Sistema = result.Sistema;
    }
}
