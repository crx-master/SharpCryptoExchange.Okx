﻿using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class MarginTransferModeConverter : BaseConverter<OkxMarginTransferMode>
    {
        public MarginTransferModeConverter() : this(true) { }
        public MarginTransferModeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxMarginTransferMode, string>> Mapping => new List<KeyValuePair<OkxMarginTransferMode, string>>
        {
            new KeyValuePair<OkxMarginTransferMode, string>(OkxMarginTransferMode.AutoTransfer, "automatic"),
            new KeyValuePair<OkxMarginTransferMode, string>(OkxMarginTransferMode.ManualTransfer, "autonomy"),
        };
    }
}