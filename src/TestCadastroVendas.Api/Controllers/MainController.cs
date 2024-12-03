using Microsoft.AspNetCore.Mvc;

namespace TestCadastroVendas.Api.Controllers;

[Route("[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
public class MainController : ControllerBase
{
    [Route("/")]
    [Route("/docs")]
    [Route("/swagger")]
    public IActionResult Index() =>
        new RedirectResult("~/swagger");
}

