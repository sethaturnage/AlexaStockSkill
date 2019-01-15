using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    /// <summary>
    /// IEX uri Endpoint
    /// uri: /stock/aapl/batch? types = quote, news, chart&range=1m&last=1
    /// uri: /stock/market/batch? symbols = aapl, fb, tsla&types=quote,news,chart&range=1m&last=5
    /// </summary>
    /// 
    class Get_StockBatchResponse_IEX
    {
        public IEXStock_QUERY_QUOTE_NEWS_CHART[] batch { get; set; }
    }
}
