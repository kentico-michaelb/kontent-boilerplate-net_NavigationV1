using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using Kentico.AspNetCore.LocalizedRouting;
using KenticoKontentModels;
using Microsoft.Extensions.Logging;

namespace Kentico.Kontent.Boilerplate.Controllers
{
    public class LanguagesController : BaseController<LanguagesController>
    {
        private readonly ILocalizedRoutingProvider _localizedRoutingProvider;

        public LanguagesController(IDeliveryClient deliveryClient, ILogger<LanguagesController> logger,
            ILocalizedRoutingProvider localizedRoutingProvider) : base(deliveryClient, logger)
        {
            _localizedRoutingProvider = localizedRoutingProvider;
        }

        public async Task<ActionResult> Index([FromQuery]Guid itemId, [FromQuery]string originalAction, [FromQuery]string itemType, [FromQuery]string originalController, [FromQuery]string language)
        {
            var translatedController = await _localizedRoutingProvider.ProvideRouteAsync(language, originalController, originalController, ProvideRouteType.OriginalToTranslated);
            var tranclatedAction = await _localizedRoutingProvider.ProvideRouteAsync(language, originalAction, originalController, ProvideRouteType.OriginalToTranslated);

            if (tranclatedAction == null || translatedController == null)
            {
                return NotFound();
            }

            // Specific item is not selected, url will not be changed after redirect
            if (itemId == Guid.Empty || string.IsNullOrEmpty(itemType))
            {
                return RedirectToAction(tranclatedAction, translatedController, new { culture = language });
            }

            var item = (await DeliveryClient.GetItemsAsync<object>(
                new SystemTypeEqualsFilter(itemType),
                new EqualsFilter("system.id", itemId.ToString()),
                new LanguageParameter(language),
                new ElementsParameter("url_pattern"))).Items.FirstOrDefault();

            if (!(item is IDetailItem detaiItem))
            {
                return NotFound();
            }


            return RedirectToAction(tranclatedAction, translatedController, new { culture = language, urlSlug = detaiItem.UrlPattern });
        }
    }
}