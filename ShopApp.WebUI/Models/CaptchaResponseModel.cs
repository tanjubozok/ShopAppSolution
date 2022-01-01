using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ShopApp.WebUI.Models
{
    public class CaptchaResponseModel
    {
        public bool Success { get; set; }

        [JsonProperty(propertyName: "error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }

        [JsonProperty(propertyName: "challenge_ts")]
        public DateTime ChallengeTime { get; set; }

        public string HostName { get; set; }
        public double Score { get; set; }
        public string Action { get; set; }
    }
}
