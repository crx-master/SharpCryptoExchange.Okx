﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCryptoExchange.Okx.Objects.Core
{
    public class OkxApiAddresses
    {
        public string UnifiedAddress { get; set; }

        public static OkxApiAddresses Default = new OkxApiAddresses
        {
            UnifiedAddress = "https://www.okx.com",
        };
    }
}