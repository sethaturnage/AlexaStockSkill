using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    /// <summary>
    /// Historical Stock Data from IEX 
    /// 
    /// 
    /// </summary>
    class Get_HistoricalDataResponse_IEX
    {
        public IEXStockInfoForDate[] charts { get; set; }
    }
}
