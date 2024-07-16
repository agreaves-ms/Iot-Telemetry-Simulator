namespace IotTelemetrySimulator
{
    using Newtonsoft.Json;

    public class TelemetryCsvVariable
    {
        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("contents")]
        public string Contents { get; set; }

        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        [JsonProperty("fieldIndex")]
        public int? FieldIndex { get; set; }

        [JsonProperty("headers")]
        public bool Headers { get; set; }

        [JsonProperty("int")]
        public bool Int { get; set; }

        [JsonProperty("boolean")]
        public bool Boolean { get; set; }

        [JsonProperty("double")]
        public bool Double { get; set; }

        [JsonProperty("string")]
        public bool String { get; set; }
    }
}
