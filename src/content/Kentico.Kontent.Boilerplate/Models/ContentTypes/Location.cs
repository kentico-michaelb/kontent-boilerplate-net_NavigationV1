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
    public partial class Location
    {
        public const string Codename = "location";
        public const string CafesCodename = "cafes";
        public const string NameCodename = "name";

        public IEnumerable<object> Cafes { get; set; }
        public string Name { get; set; }
        public ContentItemSystemAttributes System { get; set; }
    }
}