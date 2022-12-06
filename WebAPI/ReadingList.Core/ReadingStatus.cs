//using System.Text.Json.Serialization;
//using Newtonsoft.Json.Converters;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ReadingList.Core;

[JsonConverter(typeof(StringEnumConverter))]
public enum ReadingStatus
{
    Scheduled = 0,
    InProgress = 1,
    Completed = 2,
    Skipped = 3,
}