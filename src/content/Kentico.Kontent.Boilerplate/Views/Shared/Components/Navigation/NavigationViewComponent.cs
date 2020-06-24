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
            NavigationItem rootItem = await client.GetItemAsync<NavigationItem>("root_navigation_item",
                    new DepthParameter(5)
                    );

            var items = new List<NavigationItem>();

            foreach (NavigationItem navItem in rootItem.Subitems) 
            {
                var parents = new List<NavigationItem>();
                
                if (navItem.Subitems.Count() > 0)
                {
                    parents.Add(navItem);
                    buildRelationship(navItem.Subitems, parents, items);
                }

                buildUrls(items);

            }          

            return View("Navigation", items);
        }

        //
        public List<NavigationItem> buildRelationship(IEnumerable<NavigationItem> children, List<NavigationItem> parents, List<NavigationItem> items)
        {
            foreach (NavigationItem child in children)
            {
                parents = checkParentRelationship(parents, child);
            
                    foreach(NavigationItem parent in parents)
                    { 
                        child.Parents.Add(parent);
                    }

                    if (child.Subitems.Count() > 0)
                    {
                        parents.Add(child);

                        buildRelationship(child.Subitems, parents, items);
                    }
                    items.Add(child);              
            }
            
            return items;
        }

        public List<NavigationItem> checkParentRelationship(List<NavigationItem> parents, NavigationItem child)
        {
            if(!parents.Last().Subitems.Contains(child)) 
            {
                parents.Remove(parents.Last());
                checkParentRelationship(parents, child);

                return parents;
            }

            return parents;
        }

        public void buildUrls(List<NavigationItem> items)
        {
            foreach(NavigationItem item in items)
            {
                string path = "";

                foreach (NavigationItem parent in item.Parents)
                {
                    path +=  "/" + parent.Url;
                }
                
                path += "/" + item.Url;

                if (string.IsNullOrEmpty(item.customPath))
                {
                    item.customPath = path;
                }
            }
        }
    }
}
