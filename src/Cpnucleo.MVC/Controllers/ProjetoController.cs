namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ProjetoController : BaseController
{
    private readonly IProjetoGrpcService _projetoGrpcService;
    private readonly ISistemaGrpcService _sistemaGrpcService;

    private ProjetoViewModel _viewModel;

    public ProjetoController(IConfiguration configuration)
        : base(configuration)
    {
        _projetoGrpcService = MagicOnionClient.Create<IProjetoGrpcService>(CreateAuthenticatedChannel());
        _sistemaGrpcService = MagicOnionClient.Create<ISistemaGrpcService>(CreateAuthenticatedChannel());
    }

    public ProjetoViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new ProjetoViewModel();
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
            ListProjetoViewModel result = await _projetoGrpcService.ListProjeto(new ListProjetoQuery { GetDependencies = true });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            ViewModel.Lista = result.Projetos;

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Incluir()
    {
        await CarregarDados();

        return View(ViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(ProjetoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados();

                return View(ViewModel);
            }

            OperationResult result = await _projetoGrpcService.CreateProjeto(new CreateProjetoCommand(Guid.Empty, obj.Projeto.Nome, obj.Projeto.IdSistema));

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
    public async Task<IActionResult> Alterar(ProjetoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Projeto.Id);

                return View(ViewModel);
            }

            OperationResult result = await _projetoGrpcService.UpdateProjeto(new UpdateProjetoCommand(obj.Projeto.Id, obj.Projeto.Nome, obj.Projeto.IdSistema));

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
    public async Task<IActionResult> Remover(ProjetoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Projeto.Id);

                return View(ViewModel);
            }

            OperationResult result = await _projetoGrpcService.RemoveProjeto(new RemoveProjetoCommand(obj.Projeto.Id));

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

    private async Task CarregarDados(Guid? id = default)
    {
        if (id is not null)
        {
            GetProjetoViewModel result = await _projetoGrpcService.GetProjeto(new GetProjetoQuery { Id = id.Value });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return;
            }

            ViewModel.Projeto = result.Projeto;
        }

        ListSistemaViewModel result2 = await _sistemaGrpcService.ListSistema(new ListSistemaQuery { });

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.SelectSistemas = new SelectList(result2.Sistemas, "Id", "Nome");
    }
}
