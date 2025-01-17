﻿using Newtonsoft.Json;
using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Converters;
using System;

namespace SharpCryptoExchange.Okx.Models.Trading
{
    [JsonConverter(typeof(ArrayConverter))]
    public class OkxPutCallRatio
    {
        [ArrayProperty(0), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [ArrayProperty(1)]
        public decimal OpenInterestRatio { get; set; }

        [ArrayProperty(2)]
        public decimal VolumeRatio { get; set; }
    }
}
