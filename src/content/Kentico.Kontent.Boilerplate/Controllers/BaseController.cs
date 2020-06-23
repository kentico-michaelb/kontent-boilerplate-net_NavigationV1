using Kentico.Kontent.Delivery.Abstractions;
using Kentico.Kontent.Boilerplate.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Kentico.Kontent.Boilerplate.Controllers
{
    [TypeFilter(typeof(CultureValidatorActionFilter))]
    public class BaseController<T> : Controller
    {
        protected ILogger<T> Logger { get; }

        protected IDeliveryClient DeliveryClient { get; }

        protected string Language => CultureInfo.CurrentCulture.Name;

        public BaseController(IDeliveryClient deliveryClient, ILogger<T> logger)
        {
            DeliveryClient = deliveryClient;
            Logger = logger;
        }
    }
}
