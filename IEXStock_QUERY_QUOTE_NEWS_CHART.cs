using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    class IEXStock_QUERY_QUOTE_NEWS_CHART
    {
        public IEXStockQuote quote { get; set; }
        public IEXStockNews news { get; set; }
        public IEXStockInfoForDate[] chart { get; set; }
    }
}
