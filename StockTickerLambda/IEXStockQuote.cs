using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    public class IEXStockQuote
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string primaryExchange { get; set; }
        public string sector { get; set; }
        public string calculationPrice { get; set; }
        public double open { get; set; }
        public long openTime { get; set; }
        public double close { get; set; }
        public long closeTime { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double latestPrice { get; set; }
        public string latestSource { get; set; }
        public string latestTime { get; set; }
        public long latestUpdate { get; set; }
        public long latestVolume { get; set; }
        public double iexRealtimePrice { get; set; }
        public double iexRealtimeSize { get; set; }
        public long iexLastUpdated { get; set; }
        public double delayedPrice { get; set; }
        public long delayedPriceTime { get; set; }
        public double extendedPrice { get; set; }
        public double extendedChange { get; set; }
        public double extendedChangePercent { get; set; }
        public long extendedPriceTime { get; set; }
        public double previousClose { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
        public double iexMarketPercent { get; set; }
        public long iexVolume { get; set; }
        public long avgTotalvolume { get; set; }
        public double iexBidPrice { get; set; }
        public double iexBidSize { get; set; }
        public double iexAskPrice { get; set; }
        public double iexAskSize { get; set; }
        public long marketCap { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double peRatio { get; set; }
        public double week52High { get; set; }
        public double week52Low { get; set; }
        public double ytdChange { get; set; }
    }
}
