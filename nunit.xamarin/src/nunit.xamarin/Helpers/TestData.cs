using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace nunit.xamarin.Helpers
{
    public class TestData
    {
        [JsonProperty("device")]
        public string Device { get; set; }

        [JsonProperty("platform")]
        public string DevicePlatform { get; set; }

        [JsonProperty("os_version")]
        public string DeviceOS { get; set; }

        [JsonProperty("framework")]
        public string Framework { get; set; }

        [JsonProperty("framework_version")]
        public string FrameworkVersion { get; set; }

        [JsonProperty("build_number")]
        public string BuildNumber { get; set; }

        [JsonProperty("results")]
        public Dictionary<string, int> Results { get; set; }
    }
}
