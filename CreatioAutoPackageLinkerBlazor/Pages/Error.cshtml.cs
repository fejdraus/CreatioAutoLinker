using System.Diagnostics;
using CreatioAutoPackageLinkerBlazor.Data;
using CreatioAutoPackageLinkerBlazor.Data.Rest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CreatioAutoPackageLinkerBlazor.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;
    
    private readonly ICreatioService _creatioService;

    public ErrorModel(ILogger<ErrorModel> logger, ICreatioService creatioService)
    {
        _logger = logger;
        _creatioService = creatioService;
    }

    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}