using System.Linq;
using System.Threading.Tasks;
using Kentico.AspNetCore.LocalizedRouting.Attributes;
using Kentico.Kontent.Boilerplate.Configuration;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using KenticoKontentModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kentico.Kontent.Boilerplate.Controllers
{
    [LocalizedRoute(CultureConstants.EnglishCulture, "Cafes")]
    [LocalizedRoute(CultureConstants.SpanishCulture, "Cafeterias")]
    public class CafesController : BaseController<CafesController>
    {
        public CafesController(IDeliveryClient deliveryClient, ILogger<CafesController> logger) : base(deliveryClient, logger)
        {

        }

        [LocalizedRoute(CultureConstants.EnglishCulture, "Index")]
        [LocalizedRoute(CultureConstants.SpanishCulture, "Indice")]
        public async Task<ViewResult> Index(string location)
        {
            if (location == null)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

            var response = await DeliveryClient.GetItemsAsync<Location>(
                    new EqualsFilter("system.type", "location"),
                    new EqualsFilter("elements.url", location),
                    new LanguageParameter(Language),
                    new EqualsFilter("system.language", Language),
                    new DepthParameter(2)
                    );

                return View(response.Items[0]);
        }
    }
}