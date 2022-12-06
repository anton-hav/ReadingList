using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReadingList.Core;

[JsonConverter(typeof(StringEnumConverter))]
public enum ReadingPriority
{
    Never = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Urgent = 4,
    RightNow = 5,
}