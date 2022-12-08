using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReadingList.Core;

/// <summary>
/// Order parameter for sorting book notes
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum OrderParameter
{
    None = 0,
    Title = 1,
    Author = 2,
    Category = 3,
    Priority = 4,
    Status = 5,
}