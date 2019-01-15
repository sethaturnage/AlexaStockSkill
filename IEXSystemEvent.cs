using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    class IEXSystemEvent
    {
        public string systemEvent { get; set; }
        public int timeStamp { get; set; }
    }
}
