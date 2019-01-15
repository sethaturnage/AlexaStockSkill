using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    class IEXStockTrade
    {
        public double price { get; set; }
        public int size { get; set; }
        public int tradeId { get; set; }
        public bool isISO { get; set; }
        public bool isOddLot { get; set; }
        public bool isOutsideRegularHours { get; set; }
        public bool isSinglePriceCross { get; set; }
        public bool isTradeThroughExempt { get; set; }
        public int timestamp { get; set; }
    }
}
