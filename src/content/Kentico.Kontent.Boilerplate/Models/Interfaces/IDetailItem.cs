﻿
namespace KenticoKontentModels
{
    public interface IDetailItem
    {
        string Type { get; }
        string Id { get; }
        string UrlPattern { get; }
    }
}
