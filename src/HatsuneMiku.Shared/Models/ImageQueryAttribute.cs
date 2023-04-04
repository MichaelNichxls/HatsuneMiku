using GScraper;
using HatsuneMiku.Shared.GScraper;
using System;

namespace HatsuneMiku.Shared.Models;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ImageQueryAttribute : Attribute, IImageQuery
{
    public string Query { get; }
    public SafeSearchLevel SafeSearch { get; }

    public ImageQueryAttribute(string query, SafeSearchLevel safeSearch = SafeSearchLevel.Moderate)
    {
        Query       = query;
        SafeSearch  = safeSearch;
    }

    public void Deconstruct(out string query, out SafeSearchLevel safeSearch)
    {
        query       = Query;
        safeSearch  = SafeSearch;
    }
}