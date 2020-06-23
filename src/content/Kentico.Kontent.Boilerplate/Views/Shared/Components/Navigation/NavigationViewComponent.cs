using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using KenticoKontentModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kentico.Kontent.Boilerplate.Views.Shared.Components.Navigation
{
    public class NavigationViewComponent: ViewComponent
    { 
        private IDeliveryClient client;

        public NavigationViewComponent(IDeliveryClient deliveryClient)
        {
            client = deliveryClient;
        }




        public async Task<IViewComponentResult> InvokeAsync()
        {

            var routes = new Dictionary<string, List<string>>();
            

            NavigationItem rootItem = await client.GetItemAsync<NavigationItem>("root_navigation_item",
                    new DepthParameter(10)
                    );

            foreach(NavigationItem navItem in rootItem.Subitems) 
            {
                var urls = new List<string>();
                var parents = new List<string>();

                if (navItem.Subitems.Count() > 0)
                {
                    parents.Add(navItem.Url);
                    urls = getChildren(urls, navItem.Subitems, parents);                  
                }
                routes.Add(navItem.Title, urls);
            }
            

            return View("Navigation", routes);
        }

        public List<string> getChildren(List<string> urls, IEnumerable<NavigationItem> children, List<string> parents)
        {
            foreach(NavigationItem child in children)
            {
                var tempUrl = "";
                if(parents.Count > 0)
                { 
                    foreach(string parent in parents)
                    {
                        tempUrl += parent+"/";
                    }
                    urls.Add(tempUrl + child.Url);
                }
                if (child.Subitems.Count() > 0)
                {
                    parents.Add(child.Url);
                    urls = getChildren(urls, child.Subitems, parents);
                }
                else
                {
                    parents.Clear();
                }
            }
            
            return urls;
        }
    }
}
