using Kentico.Kontent.Delivery;
using Kentico.Kontent.Delivery.Abstractions;
using KenticoKontentModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Kentico.Kontent.Boilerplate.Infrastructure
{
    public class NavigationProvider
    {
        private IDeliveryClient client;
        private IMemoryCache _cache;

        protected string Language => CultureInfo.CurrentCulture.Name;

        public NavigationProvider(IDeliveryClient deliveryClient, IMemoryCache memoryCache)
        {
            client = deliveryClient;
            _cache = memoryCache;
        }

        // inspect this in debug for cache entries.
        public async Task<List<NavigationItem>> GetCachedNavigation(int depth=1)
        {
            string cacheKey = "NavigationCache";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                var navigation = await GetNavigation(depth);

                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);

                return navigation;
            });

        }

        public async Task<List<NavigationItem>> GetNavigation(int depth)
        {
            NavigationItem rootItem = await client.GetItemAsync<NavigationItem>("root_navigation_item",
                    new DepthParameter(depth),
                    new EqualsFilter("system.language", Language),
                    new LanguageParameter(Language)
                    );

            var items = new List<NavigationItem>();

            foreach (NavigationItem navItem in rootItem.Subitems)
            {
                // add depth 1 items
                items.Add(navItem);

                var parents = new List<NavigationItem>();

                if (navItem.Subitems.Count() > 0)
                {
                    parents.Add(navItem);
                    buildRelationship(navItem.Subitems, parents, items);
                }

                buildUrls(items);

            }

            return items;
        }

        public List<NavigationItem> buildRelationship(IEnumerable<NavigationItem> children, List<NavigationItem> parents, List<NavigationItem> items)
        {
            foreach (NavigationItem child in children)
            {
                parents = checkParentRelationship(parents, child);

                foreach (NavigationItem parent in parents)
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
            if (!parents.Last().Subitems.Contains(child))
            {
                parents.Remove(parents.Last());
                checkParentRelationship(parents, child);

                return parents;
            }

            return parents;
        }

        public void buildUrls(List<NavigationItem> items)
        {
            foreach (NavigationItem item in items)
            {
                string path = "";

                foreach (NavigationItem parent in item.Parents)
                {
                    path += "/" + parent.Url;
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

