﻿using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class LiquidationStateConverter : BaseConverter<OkxLiquidationState>
    {
        public LiquidationStateConverter() : this(true) { }
        public LiquidationStateConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxLiquidationState, string>> Mapping => new List<KeyValuePair<OkxLiquidationState, string>>
        {
            new KeyValuePair<OkxLiquidationState, string>(OkxLiquidationState.Unfilled, "unfilled"),
            new KeyValuePair<OkxLiquidationState, string>(OkxLiquidationState.Filled, "filled"),
        };
    }
}