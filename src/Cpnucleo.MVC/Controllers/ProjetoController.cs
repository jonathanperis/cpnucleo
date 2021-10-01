using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ProjetoController : BaseController
{
    private readonly IProjetoGrpcService _projetoGrpcService;
    private readonly ISistemaGrpcService _sistemaGrpcService;

    private ProjetoView _projetoView;

    public ProjetoController(IConfiguration configuration)
        : base(configuration)
    {
        _projetoGrpcService = MagicOnionClient.Create<IProjetoGrpcService>(CreateAuthenticatedChannel());
        _sistemaGrpcService = MagicOnionClient.Create<ISistemaGrpcService>(CreateAuthenticatedChannel());
    }

    public ProjetoView ProjetoView
    {
        get
        {
            if (_projetoView == null)
            {
                _projetoView = new ProjetoView();
            }

            return _projetoView;
        }
        set => _projetoView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            ListProjetoResponse response = await _projetoGrpcService.AllAsync(new ListProjetoQuery { GetDependencies = true });
            ProjetoView.Lista = response.Projetos;

            return View(ProjetoView);
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
        await CarregarSelectSistemas();

        return View(ProjetoView);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(ProjetoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarSelectSistemas();

                return View(ProjetoView);
            }

            await _projetoGrpcService.AddAsync(new CreateProjetoCommand { Projeto = obj.Projeto });

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
            GetProjetoResponse response = await _projetoGrpcService.GetAsync(new GetProjetoQuery { Id = id });
            ProjetoView.Projeto = response.Projeto;

            await CarregarSelectSistemas();

            return View(ProjetoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(ProjetoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                GetProjetoResponse response = await _projetoGrpcService.GetAsync(new GetProjetoQuery { Id = obj.Projeto.Id });
                ProjetoView.Projeto = response.Projeto;

                await CarregarSelectSistemas();

                return View(ProjetoView);
            }

            await _projetoGrpcService.UpdateAsync(new UpdateProjetoCommand { Projeto = obj.Projeto });

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
            GetProjetoResponse response = await _projetoGrpcService.GetAsync(new GetProjetoQuery { Id = id });
            ProjetoView.Projeto = response.Projeto;

            return View(ProjetoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(ProjetoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                GetProjetoResponse response = await _projetoGrpcService.GetAsync(new GetProjetoQuery { Id = obj.Projeto.Id });
                ProjetoView.Projeto = response.Projeto;

                return View(ProjetoView);
            }

            await _projetoGrpcService.RemoveAsync(new RemoveProjetoCommand { Id = obj.Projeto.Id });

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task CarregarSelectSistemas()
    {
        ListSistemaResponse response = await _sistemaGrpcService.AllAsync(new ListSistemaQuery { });
        ProjetoView.SelectSistemas = new SelectList(response.Sistemas, "Id", "Nome");
    }
}
