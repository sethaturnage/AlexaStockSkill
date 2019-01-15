using System;
using System.Collections.Generic;
using System.Text;

namespace StockTickerLambda
{
    class IEXNewsStory
    {
            public string datetime { get; set; }
            public string headline { get; set; }
            public string source { get; set; }
            public string url { get; set; }
            public string summary { get; set; }
            public string related { get; set; }
            public string image { get; set; }
    }
}
