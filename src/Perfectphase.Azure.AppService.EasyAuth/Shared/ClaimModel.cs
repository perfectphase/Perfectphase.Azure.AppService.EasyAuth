using Newtonsoft.Json;

namespace Perfectphase.Azure.AppService.EasyAuth.Shared
{
    public class ClaimModel
    {
        [JsonProperty("typ")]
        public string Type { get; set; }

        [JsonProperty("val")]
        public string Value { get; set; }
    }
}
