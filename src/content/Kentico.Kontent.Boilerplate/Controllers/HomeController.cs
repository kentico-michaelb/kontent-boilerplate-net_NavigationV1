using System.Threading.Tasks;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using KenticoKontentModels;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Kentico.Kontent.Boilerplate.Configuration;

namespace Kentico.Kontent.Boilerplate.Controllers
{
    [LocalizedRoute(CultureConstants.EnglishCulture, "Home")]
    [LocalizedRoute(CultureConstants.SpanishCulture, "Inicio")]
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(IDeliveryClient deliveryClient, ILogger<HomeController> logger) :  base(deliveryClient, logger)
        {
            
        }
        [LocalizedRoute(CultureConstants.EnglishCulture, "")]
        [LocalizedRoute(CultureConstants.SpanishCulture, "")]
        public async Task<ViewResult> Index()
        {
            var response = await DeliveryClient.GetItemsAsync<Location>(
                new EqualsFilter("system.type", "location"),
                new EqualsFilter("system.language", Language),
                new LanguageParameter(Language),              
                new DepthParameter(0)
            );

            return View(response.Items);
        }
    }
}
