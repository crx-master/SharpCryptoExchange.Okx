﻿using Newtonsoft.Json;
using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Converters;
using System;

namespace SharpCryptoExchange.Okx.Models.Trading
{
    [JsonConverter(typeof(ArrayConverter))]
    public class OkxTakerFlow
    {
        [ArrayProperty(0), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [ArrayProperty(1)]
        public string CallOptionBuyVolume { get; set; }

        [ArrayProperty(2)]
        public string CallOptionSellVolume { get; set; }

        [ArrayProperty(3)]
        public string PutOptionBuyVolume { get; set; }

        [ArrayProperty(4)]
        public string PutOptionSellVolume { get; set; }

        [ArrayProperty(5)]
        public decimal CallBrickVolume { get; set; }

        [ArrayProperty(6)]
        public decimal PutBrickVolume { get; set; }
    }
}
