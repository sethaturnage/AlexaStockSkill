namespace StockTickerLambda
{
    class IEXQUERIED_Stock
    {
        /*handle the fact that every stock is named something different*/
        public IEXStockInfoForDate[] DynamicName { get; set; }
    }

}