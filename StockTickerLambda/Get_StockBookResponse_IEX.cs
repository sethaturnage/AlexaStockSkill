using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    /// <summary>
    /// IEX uri EndPoint
    /// uri: ---------
    /// </summary>
    class Get_StockBookResponse_IEX
    {
        public IEXStockQuote quote { get; set; }
        public IEXStockBid[] bids { get; set; }
        public IEXStockAsk[] asks { get; set; }
        public IEXStockTrade[] trades { get; set; }
        public IEXSystemEvent systemEvent { get; set; }
    }
}
