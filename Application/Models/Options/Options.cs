using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Models.Options
{
    internal class Options
    {
        [JsonPropertyName("Options")]
        public IEnumerable<Option> Option { get; set; }
    }
}
