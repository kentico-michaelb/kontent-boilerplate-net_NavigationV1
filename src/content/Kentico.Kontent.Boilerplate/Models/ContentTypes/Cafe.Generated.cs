// This code was generated by a kontent-generators-net tool 
// (see https://github.com/Kentico/kontent-generators-net).
// 
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated. 
// For further modifications of the class, create a separate file with the partial class.

using System;
using System.Collections.Generic;
using Kentico.Kontent.Delivery.Abstractions;

namespace KenticoKontentModels
{
    public partial class Cafe
    {
        public const string Codename = "cafe";
        public const string AddressCodename = "address";
        public const string DescriptionCodename = "description";
        public const string ImageCodename = "image";
        public const string NameCodename = "name";
        public const string UrlCodename = "url";

        public string Address { get; set; }
        public string Description { get; set; }
        public IEnumerable<Asset> Image { get; set; }
        public string Name { get; set; }
        public ContentItemSystemAttributes System { get; set; }
        public string Url { get; set; }
    }
}