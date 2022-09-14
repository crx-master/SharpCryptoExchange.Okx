﻿using SharpCryptoExchange;
using SharpCryptoExchange.Authentication;
using SharpCryptoExchange.Interfaces;
using SharpCryptoExchange.Logging;
using SharpCryptoExchange.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpCryptoExchange.Okx.Objects.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCryptoExchange.Okx
{
    public partial class OkxClient : BaseRestClient
    {
        #region Internal Fields
        internal OkxClientOptions Options { get; }
        internal OkxClientUnifiedApi UnifiedApi { get; }
        internal const string BodyParameterKey = "<BODY>";
        #endregion

        #region Rest Api Endpoints

        #region Trade Endpoints
        protected const string Endpoints_V5_Trade_Order = "api/v5/trade/order";
        protected const string Endpoints_V5_Trade_BatchOrders = "api/v5/trade/batch-orders";
        protected const string Endpoints_V5_Trade_CancelOrder = "api/v5/trade/cancel-order";
        protected const string Endpoints_V5_Trade_CancelBatchOrders = "api/v5/trade/cancel-batch-orders";
        protected const string Endpoints_V5_Trade_AmendOrder = "api/v5/trade/amend-order";
        protected const string Endpoints_V5_Trade_AmendBatchOrders = "api/v5/trade/amend-batch-orders";
        protected const string Endpoints_V5_Trade_ClosePosition = "api/v5/trade/close-position";
        protected const string Endpoints_V5_Trade_OrdersPending = "api/v5/trade/orders-pending";
        protected const string Endpoints_V5_Trade_OrdersHistory = "api/v5/trade/orders-history";
        protected const string Endpoints_V5_Trade_OrdersHistoryArchive = "api/v5/trade/orders-history-archive";
        protected const string Endpoints_V5_Trade_Fills = "api/v5/trade/fills";
        protected const string Endpoints_V5_Trade_FillsHistory = "api/v5/trade/fills-history";
        protected const string Endpoints_V5_Trade_OrderAlgo = "api/v5/trade/order-algo";
        protected const string Endpoints_V5_Trade_CancelAlgos = "api/v5/trade/cancel-algos";
        protected const string Endpoints_V5_Trade_CancelAdvanceAlgos = "api/v5/trade/cancel-advance-algos";
        protected const string Endpoints_V5_Trade_OrdersAlgoPending = "api/v5/trade/orders-algo-pending";
        protected const string Endpoints_V5_Trade_OrdersAlgoHistory = "api/v5/trade/orders-algo-history";
        #endregion

        #region Funding Endpoints
        protected const string Endpoints_V5_Asset_Currencies = "api/v5/asset/currencies";
        protected const string Endpoints_V5_Asset_Balances = "api/v5/asset/balances";
        protected const string Endpoints_V5_Asset_Transfer = "api/v5/asset/transfer";
        protected const string Endpoints_V5_Asset_Bills = "api/v5/asset/bills";
        protected const string Endpoints_V5_Asset_DepositLightning = "api/v5/asset/deposit-lightning";
        protected const string Endpoints_V5_Asset_DepositAddress = "api/v5/asset/deposit-address";
        protected const string Endpoints_V5_Asset_DepositHistory = "api/v5/asset/deposit-history";
        protected const string Endpoints_V5_Asset_Withdrawal = "api/v5/asset/withdrawal";
        protected const string Endpoints_V5_Asset_WithdrawalLightning = "api/v5/asset/withdrawal-lightning";
        protected const string Endpoints_V5_Asset_WithdrawalCancel = "api/v5/asset/cancel-withdrawal";
        protected const string Endpoints_V5_Asset_WithdrawalHistory = "api/v5/asset/withdrawal-history";
        protected const string Endpoints_V5_Asset_SavingBalance = "api/v5/asset/saving-balance";
        protected const string Endpoints_V5_Asset_SavingPurchaseRedempt = "api/v5/asset/purchase_redempt";
        #endregion

        #region Account Endpoints
        protected const string Endpoints_V5_Account_Balance = "api/v5/account/balance";
        protected const string Endpoints_V5_Account_Positions = "api/v5/account/positions";
        protected const string Endpoints_V5_Account_PositionRisk = "api/v5/account/account-position-risk";
        protected const string Endpoints_V5_Account_Bills = "api/v5/account/bills";
        protected const string Endpoints_V5_Account_BillsArchive = "api/v5/account/bills-archive";
        protected const string Endpoints_V5_Account_Config = "api/v5/account/config";
        protected const string Endpoints_V5_Account_SetPositionMode = "api/v5/account/set-position-mode";
        protected const string Endpoints_V5_Account_SetLeverage = "api/v5/account/set-leverage";
        protected const string Endpoints_V5_Account_MaxSize = "api/v5/account/max-size";
        protected const string Endpoints_V5_Account_MaxAvailSize = "api/v5/account/max-avail-size";
        protected const string Endpoints_V5_Account_PositionMarginBalance = "api/v5/account/position/margin-balance";
        protected const string Endpoints_V5_Account_LeverageInfo = "api/v5/account/leverage-info";
        protected const string Endpoints_V5_Account_MaxLoan = "api/v5/account/max-loan";
        protected const string Endpoints_V5_Account_TradeFee = "api/v5/account/trade-fee";
        protected const string Endpoints_V5_Account_InterestAccrued = "api/v5/account/interest-accrued";
        protected const string Endpoints_V5_Account_InterestRate = "api/v5/account/interest-rate";
        protected const string Endpoints_V5_Account_SetGreeks = "api/v5/account/set-greeks";
        protected const string Endpoints_V5_Account_MaxWithdrawal = "api/v5/account/max-withdrawal";
        #endregion

        #region Sub-Account Endpoints
        protected const string Endpoints_V5_SubAccount_List = "api/v5/users/subaccount/list";
        protected const string Endpoints_V5_SubAccount_ResetApiKey = "api/v5/users/subaccount/modify-apikey";
        protected const string Endpoints_V5_SubAccount_TradingBalances = "api/v5/account/subaccount/balances";
        protected const string Endpoints_V5_SubAccount_FundingBalances = "api/v5/asset/subaccount/balances";
        protected const string Endpoints_V5_SubAccount_Bills = "api/v5/asset/subaccount/bills";
        protected const string Endpoints_V5_SubAccount_Transfer = "api/v5/asset/subaccount/transfer";
        #endregion

        #region Market Data
        protected const string Endpoints_V5_Market_Tickers = "api/v5/market/tickers";
        protected const string Endpoints_V5_Market_Ticker = "api/v5/market/ticker";
        protected const string Endpoints_V5_Market_IndexTickers = "api/v5/market/index-tickers";
        protected const string Endpoints_V5_Market_Books = "api/v5/market/books";
        protected const string Endpoints_V5_Market_Candles = "api/v5/market/candles";
        protected const string Endpoints_V5_Market_HistoryCandles = "api/v5/market/history-candles";
        protected const string Endpoints_V5_Market_IndexCandles = "api/v5/market/index-candles";
        protected const string Endpoints_V5_Market_MarkPriceCandles = "api/v5/market/mark-price-candles";
        protected const string Endpoints_V5_Market_Trades = "api/v5/market/trades";
        protected const string Endpoints_V5_Market_TradesHistory = "api/v5/market/history-trades";
        protected const string Endpoints_V5_Market_Platform24Volume = "api/v5/market/platform-24-volume";
        protected const string Endpoints_V5_Market_OpenOracle = "api/v5/market/open-oracle";
        protected const string Endpoints_V5_Market_IndexComponents = "api/v5/market/index-components";
        protected const string Endpoints_V5_Market_BrickTickers = "api/v5/market/brick-tickers";
        protected const string Endpoints_V5_Market_BrickTicker = "api/v5/market/brick-ticker";
        protected const string Endpoints_V5_Market_BrickTrades = "api/v5/market/brick-trades";
        #endregion

        #region Public Data
        protected const string Endpoints_V5_Public_Instruments = "api/v5/public/instruments";
        protected const string Endpoints_V5_Public_DeliveryExerciseHistory = "api/v5/public/delivery-exercise-history";
        protected const string Endpoints_V5_Public_OpenInterest = "api/v5/public/open-interest";
        protected const string Endpoints_V5_Public_FundingRate = "api/v5/public/funding-rate";
        protected const string Endpoints_V5_Public_FundingRateHistory = "api/v5/public/funding-rate-history";
        protected const string Endpoints_V5_Public_PriceLimit = "api/v5/public/price-limit";
        protected const string Endpoints_V5_Public_OptionSummary = "api/v5/public/opt-summary";
        protected const string Endpoints_V5_Public_EstimatedPrice = "api/v5/public/estimated-price";
        protected const string Endpoints_V5_Public_DiscountRateInterestFreeQuota = "api/v5/public/discount-rate-interest-free-quota";
        protected const string Endpoints_V5_Public_Time = "api/v5/public/time";
        protected const string Endpoints_V5_Public_LiquidationOrders = "api/v5/public/liquidation-orders";
        protected const string Endpoints_V5_Public_MarkPrice = "api/v5/public/mark-price";
        protected const string Endpoints_V5_Public_PositionTiers = "api/v5/public/position-tiers";
        protected const string Endpoints_V5_Public_InterestRateLoanQuota = "api/v5/public/interest-rate-loan-quota";
        protected const string Endpoints_V5_Public_VIPInterestRateLoanQuota = "api/v5/public/vip-interest-rate-loan-quota";
        protected const string Endpoints_V5_Public_Underlying = "api/v5/public/underlying";
        protected const string Endpoints_V5_Public_InsuranceFund = "api/v5/public/insurance-fund";
        protected const string Endpoints_V5_Public_ConvertContractCoin = "api/v5/public/convert-contract-coin";
        #endregion

        #region Trading Data
        protected const string Endpoints_V5_RubikStat_TradingDataSupportCoin = "api/v5/rubik/stat/trading-data/support-coin";
        protected const string Endpoints_V5_RubikStat_TakerVolume = "api/v5/rubik/stat/taker-volume";
        protected const string Endpoints_V5_RubikStat_MarginLoanRatio = "api/v5/rubik/stat/margin/loan-ratio";
        protected const string Endpoints_V5_RubikStat_ContractsLongShortAccountRatio = "api/v5/rubik/stat/contracts/long-short-account-ratio";
        protected const string Endpoints_V5_RubikStat_ContractsOpenInterestVolume = "api/v5/rubik/stat/contracts/open-interest-volume";
        protected const string Endpoints_V5_RubikStat_OptionOpenInterestVolume = "api/v5/rubik/stat/option/open-interest-volume";
        protected const string Endpoints_V5_RubikStat_OptionOpenInterestVolumeRatio = "api/v5/rubik/stat/option/open-interest-volume-ratio";
        protected const string Endpoints_V5_RubikStat_OptionOpenInterestVolumeExpiry = "api/v5/rubik/stat/option/open-interest-volume-expiry";
        protected const string Endpoints_V5_RubikStat_OptionOpenInterestVolumeStrike = "api/v5/rubik/stat/option/open-interest-volume-strike";
        protected const string Endpoints_V5_RubikStat_OptionTakerBrickVolume = "api/v5/rubik/stat/option/taker-brick-volume";
        #endregion

        #region System
        protected const string Endpoints_V5_System_Status = "api/v5/system/status";
        #endregion

        #endregion

        #region Constructor/Destructor
        public OkxClient() : this(OkxClientOptions.Default)
        {
        }

        public OkxClient(OkxClientOptions options) : base("OKX REST API", options)
        {
            Options = options;
            UnifiedApi = AddApiClient(new OkxClientUnifiedApi(log, this, options));
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(OkxClientOptions options)
        {
            OkxClientOptions.Default = options;
        }

        /// <summary>
        /// Sets the API Credentials
        /// </summary>
        /// <param name="credentials">API Credentials Object</param>
        public void SetApiCredentials(OkxApiCredentials credentials)
        {
            ((OkxClientUnifiedApi)UnifiedApi).SetApiCredentials(credentials);
        }

        /// <summary>
        /// Sets the API Credentials
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        /// <param name="passPhrase">The passphrase you specified when creating the API key</param>
        public virtual void SetApiCredentials(string apiKey, string apiSecret, string passPhrase)
        {
            ((OkxClientUnifiedApi)UnifiedApi).SetApiCredentials(new OkxApiCredentials(apiKey, apiSecret, passPhrase));
        }
        #endregion

        #region Core Methods
        protected override void WriteParamBody(BaseApiClient apiClient, IRequest request, SortedDictionary<string, object> parameters, string contentType)
        {
            OkxWriteParamBody(apiClient, request, parameters, contentType);
        }

        protected virtual void OkxWriteParamBody(BaseApiClient apiClient, IRequest request, SortedDictionary<string, object> parameters, string contentType)
        {
            if (apiClient.requestBodyFormat == RequestBodyFormat.Json)
            {
                if (parameters.Count == 1 && parameters.Keys.First() == BodyParameterKey)
                {
                    // Write the parameters as json in the body
                    var stringData = JsonConvert.SerializeObject(parameters[BodyParameterKey]);
                    request.SetContent(stringData, contentType);
                }
                else
                {
                    // Write the parameters as json in the body
                    var stringData = JsonConvert.SerializeObject(parameters);
                    request.SetContent(stringData, contentType);
                }
            }
            else if (apiClient.requestBodyFormat == RequestBodyFormat.FormData)
            {
                // Write the parameters as form data in the body
                var stringData = parameters.ToFormData();
                request.SetContent(stringData, contentType);
            }
        }

        protected override Error ParseErrorResponse(JToken error)
        {
            return OkxParseErrorResponse(error);
        }

        protected virtual Error OkxParseErrorResponse(JToken error)
        {
            if (error["code"] == null || error["msg"] == null)
                return new ServerError(error.ToString());

            return new ServerError((int)error["code"]!, (string)error["msg"]!);
        }

        internal async Task<WebCallResult> ExecuteAsync(RestApiClient apiClient, Uri uri, HttpMethod method, CancellationToken ct, Dictionary<string, object> parameters = null, bool signed = false, HttpMethodParameterPosition? parameterPosition = null)
        {
            var result = await SendRequestAsync<object>(apiClient, uri, method, ct, parameters, signed, parameterPosition).ConfigureAwait(false);
            if (!result) return result.AsDatalessError(result.Error!);

            return result.AsDataless();
        }

        internal async Task<WebCallResult<T>> ExecuteAsync<T>(RestApiClient apiClient, Uri uri, HttpMethod method, CancellationToken ct, Dictionary<string, object> parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false, HttpMethodParameterPosition? parameterPosition = null) where T : class
        {
            var result = await SendRequestAsync<T>(apiClient, uri, method, ct, parameters, signed, parameterPosition, requestWeight: weight, ignoreRatelimit: ignoreRatelimit).ConfigureAwait(false);
            if (!result) return result.AsError<T>(result.Error!);

            return result.As(result.Data);
        }
        #endregion

    }

    public class OkxClientUnifiedApi : RestApiClient
    {
        #region Internal Fields
        internal readonly Log _log;
        internal readonly OkxClient _baseClient;
        internal readonly OkxClientOptions _options;
        internal static TimeSyncState TimeSyncState = new TimeSyncState("Unified Api");
        #endregion

        internal OkxClientUnifiedApi(Log log, OkxClient baseClient, OkxClientOptions options) : base(options, options.UnifiedApiOptions)
        {
            _baseClient = baseClient;
            _options = options;
            _log = log;
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new OkxAuthenticationProvider((OkxApiCredentials)credentials);

        internal Task<WebCallResult> ExecuteAsync(Uri uri, HttpMethod method, CancellationToken ct, Dictionary<string, object> parameters = null, bool signed = false, HttpMethodParameterPosition? parameterPosition = null)
         => _baseClient.ExecuteAsync(this, uri, method, ct, parameters, signed, parameterPosition: parameterPosition);

        internal Task<WebCallResult<T>> ExecuteAsync<T>(Uri uri, HttpMethod method, CancellationToken ct, Dictionary<string, object> parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false, HttpMethodParameterPosition? parameterPosition = null) where T : class
         => _baseClient.ExecuteAsync<T>(this, uri, method, ct, parameters, signed, weight, ignoreRatelimit: ignoreRatelimit, parameterPosition: parameterPosition);

        internal Uri GetUri(string endpoint, string param = "")
        {
            var x = endpoint.IndexOf('<');
            var y = endpoint.IndexOf('>');
            if (x > -1 && y > -1) endpoint = endpoint.Replace(endpoint.Substring(x, y - x + 1), param);

            return new Uri($"{BaseAddress.TrimEnd('/')}/{endpoint}");
        }

        protected override Task<WebCallResult<DateTimeOffset>> GetServerTimestampAsync()
             => _baseClient.GetSystemTimeAsync();

        public override TimeSyncInfo GetTimeSyncInfo()
            => new TimeSyncInfo(_log, _options.UnifiedApiOptions.AutoTimestamp, _options.UnifiedApiOptions.TimestampRecalculationInterval, TimeSyncState);

        public override TimeSpan GetTimeOffset()
            => TimeSyncState.TimeOffset;

    }

}