using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    /// <summary>
    /// IEX uri EndPoint
    /// uri : /stock/{stock_symbol_here}/chart
    /// uri: /stock/{stock_symbol_here}/chart/date/20180129
    /// uri: /stock/{stock_symbol_here}/chart/1d
    /// uri: /stock/{stock_symbol_here}/chart/5y
    /// </summary>
    class Get_StockChartResponse_IEX
    {
        public IEXStockInfoForDate[] chart { get; set; }
    }
}
