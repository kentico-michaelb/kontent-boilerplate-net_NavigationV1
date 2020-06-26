using Kentico.Kontent.Boilerplate.Infrastructure;
using Kentico.Kontent.Delivery;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kentico.Kontent.Boilerplate.Views.Shared.Components.Navigation
{
    public class NavigationViewComponent: ViewComponent
    {

        private readonly NavigationProvider _navigation;

        public NavigationViewComponent(NavigationProvider navigation)
        {
            _navigation = navigation;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Navigation", await _navigation.GetCachedNavigation("root_navigation_item",5));
        }
        
    }
}
