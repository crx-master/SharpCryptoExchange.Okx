﻿using Newtonsoft.Json;
using SharpCryptoExchange.Authentication;
using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Okx.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace SharpCryptoExchange.Okx.Models.Core
{
    public class OkxAuthenticationProvider : AuthenticationProvider, IDisposable
    {
        private readonly HMACSHA256 _encryptor;
        private bool _disposedValue;

        public OkxAuthenticationProvider(OkxApiCredentials credentials) : base(credentials)
        {
            if (credentials == null || credentials.Secret == null)
                throw new ArgumentException("No valid API credentials provided. Key/Secret needed.");

            _encryptor = new HMACSHA256(Encoding.ASCII.GetBytes(credentials.Secret.GetString()));
        }

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth, ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition, out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
        {
            uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            headers = new Dictionary<string, string>();

            // Get Clients
            var baseClient = ((OkxClientUnifiedApi)apiClient)._baseClient;

            // Check Point
            if (!(auth || baseClient.Options.SignPublicRequests))
                return;

            // Check Point
            if (Credentials == null || Credentials.Key == null || Credentials.Secret == null || ((OkxApiCredentials)Credentials).PassPhrase == null)
                throw new ArgumentException("No valid API credentials provided. Key/Secret/PassPhrase needed.");

            // Set Parameters
            uri = uri.SetParameters(uriParameters, arraySerialization);

            // Signature Body
            // var time = (DateTime.UtcNow.ToUnixTimeMilliSeconds() / 1000.0m).ToString(CultureInfo.InvariantCulture);
            var time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.sssZ", OkxGlobals.OkxCultureInfo);
            var signtext = time + method.Method.ToUpper(OkxGlobals.OkxCultureInfo) + uri.PathAndQuery.Trim('?');

            if (method == HttpMethod.Post)
            {
                if (bodyParameters.Count == 1 && bodyParameters.Keys.First() == OkxClient.BodyParameterKey)
                    signtext += JsonConvert.SerializeObject(bodyParameters[OkxClient.BodyParameterKey]);
                else
                    // signtext += JsonConvert.SerializeObject(bodyParameters.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value));
                    signtext += JsonConvert.SerializeObject(bodyParameters);
            }

            // Compute Signature
            var signature = Base64Encode(_encryptor.ComputeHash(Encoding.UTF8.GetBytes(signtext)));

            // Headers
            headers.Add("OK-ACCESS-KEY", Credentials.Key.GetString());
            headers.Add("OK-ACCESS-SIGN", signature);
            headers.Add("OK-ACCESS-TIMESTAMP", time);
            headers.Add("OK-ACCESS-PASSPHRASE", ((OkxApiCredentials)Credentials).PassPhrase.GetString());

            // Demo Trading Flag
            if (baseClient.Options.DemoTradingService)
                headers.Add("x-simulated-trading", "1");
        }

        public static string Base64Encode(byte[] plainBytes)
        {
            return Convert.ToBase64String(plainBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _encryptor.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~OkxAuthenticationProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
