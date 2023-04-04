using GScraper;

namespace HatsuneMiku.Shared.GScraper;

public interface IImageQuery
{
    string Query { get; }
    SafeSearchLevel SafeSearch { get; }
}