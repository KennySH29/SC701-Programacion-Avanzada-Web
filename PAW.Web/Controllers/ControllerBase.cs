using Microsoft.AspNetCore.Mvc;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public abstract class ControllerBase : Controller
{
    private readonly IExchangeRateService _exchangeRateService;

    protected ControllerBase(
        IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    protected Task<TipoCambioResponse> GetTipoCambioAsync()
    {
        return _exchangeRateService.GetExchangeRateAsync();
    }
}
