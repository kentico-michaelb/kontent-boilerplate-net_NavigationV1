using System;
using System.Collections.Generic;
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
    [LocalizedRoute(CultureConstants.EnglishCulture, "Cafe")]
    [LocalizedRoute(CultureConstants.SpanishCulture, "Cafeteria")]
    public class CafeController : BaseController<CafeController>
    {
        public CafeController(IDeliveryClient deliveryClient, ILogger<CafeController> logger) : base(deliveryClient, logger)
        {

        }

        public async Task<ViewResult> Index(string id)
        {

            var response = await DeliveryClient.GetItemsAsync<Cafe>(
                new EqualsFilter("system.type", "cafe"),
                new EqualsFilter("elements.url", id),
                new LanguageParameter(Language),
                new EqualsFilter("system.language", Language)
                );

            return View(response.Items[0]);
        }
    }
}