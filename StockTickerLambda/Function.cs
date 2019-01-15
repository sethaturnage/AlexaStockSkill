using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace StockTickerLambda
{
    public class Function
    {
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static HttpClient _httpClient;
        public const string INVOCATION_NAME = "market brief";

        /// <summary>
        /// All endpoints are prefixed with: https://api.iextrading.com/1.0
        /// IEX supports JSONP for all endpoints.
        /// </summary>
        private readonly Uri IEX_REST_ENDPOINT = new Uri("https://api.iextrading.com/1.0/");



        public Function()
        {
            _httpClient = new HttpClient();
        }

        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse AlexaSkillResponse = new SkillResponse();
            AlexaSkillResponse.Response = new ResponseBody();
            AlexaSkillResponse.Response.ShouldEndSession = false;
            IOutputSpeech AlexaSpeechOutput = null;
            var debugLog = context.Logger;

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                IEXStockQuote quote = await GetStockQuote("DIA", context);
                string WelcomeText = "Welcome to " + INVOCATION_NAME + " !";
                if (quote.changePercent > 0)
                {
                    WelcomeText += " The DOW is up "+(10000*quote.changePercent)+" basis points.";
                }
                else if (quote.changePercent < 0)
                {
                    WelcomeText += " The DOW is down " + (-10000 * quote.changePercent) + " basis points.";
                }
                else
                {
                    WelcomeText += " The DOW is holding strong at a $" + (10000 * quote.marketCap) + " marketCap.";
                }

                AlexaSpeechOutput = new PlainTextOutputSpeech();
                (AlexaSpeechOutput as PlainTextOutputSpeech).Text = WelcomeText;
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;
                var IntentName = intentRequest.Intent.Name;
                var Intent = intentRequest.Intent;

                /// Recognize and Respond to Intents
                switch (IntentName)
                {
                        /// NavigateHomeIntent
                        /// Sample Uttereance:
                        ///     home
                        /// Slots: none
                    case "AMAZON.NavigateHomeIntent":
                        debugLog.LogLine($"AMAZON.NavigateHomeIntent");
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Going home and exiting skill. Thank you for using market brief.";
                        AlexaSkillResponse.Response.ShouldEndSession = true;
                        break;
                        /// FallbackIntent
                        /// Sample Utterance:
                        ///     fall back
                        ///     back
                        /// Slots: none
                    case "AMAZON.FallbackIntent":
                        debugLog.LogLine($"AMAZON.FallbackIntent");
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Sorry, I didn't understand you. Say, 'tell me about Amazon stock.' ";

                        break;
                        /// CancelIntent
                        /// Sample Utterance:
                        ///     cancel
                        /// Slots: none
                    case "AMAZON.CancelIntent":
                        debugLog.LogLine($"AMAZON.CancelIntent: send stopMessage");
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Going home and exiting skill. Thank you for using market brief.";
                        AlexaSkillResponse.Response.ShouldEndSession = true;

                        break;
                        /// StopIntent
                        /// Sample Utterances:
                        ///     stop
                        /// Slots: none
                    case "AMAZON.StopIntent":
                        debugLog.LogLine($"AMAZON.StopIntent: send stopMessage");
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Going home and exiting skill. Thank you for using market brief.";
                        AlexaSkillResponse.Response.ShouldEndSession = true;

                        break;
                        /// HelpIntent
                        /// Sample Utterances:
                        ///     help
                        /// Slots: none
                    case "AMAZON.HelpIntent":
                        debugLog.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Just say: show me Amazon or show me Google. You can also specify the classes of stock in question.";

                        break;
                        /// MarketIntent
                        /// Sample Utterances:
                        ///     how is the stock market doing?
                        ///     give me a flash briefing on the stock market?
                        ///     check the stock market?
                        ///     update me on the market
                        /// Slots: none
                    case "MarketIntent":
                        string responseText = "";
                        IEXStockQuote NASDAQ = await GetStockQuote("ONEQ", context);
                        if (NASDAQ.changePercent > 0)
                        {
                            responseText += " The NASDAQ is up " + (100 * NASDAQ.changePercent) + " basis points.";
                        }
                        else if (NASDAQ.changePercent < 0)
                        {
                            responseText += " The NASDAQ is down " + (-100 * NASDAQ.changePercent) + " basis points.";
                        }
                        else
                        {
                            responseText += " The NASDAQ is holding strong at a " + (100 * NASDAQ.marketCap) + " marketCap.";
                        }
                        IEXStockQuote Dow_Jones_Index = await GetStockQuote("DIA", context);
                        if (Dow_Jones_Index.changePercent > 0)
                        {
                            responseText += " The DOW is up " + (100 * Dow_Jones_Index.changePercent) + " basis points.";
                        }
                        else if (Dow_Jones_Index.changePercent < 0)
                        {
                            responseText += " The DOW is down " + (-100 * Dow_Jones_Index.changePercent) + " basis points.";
                        }
                        else
                        {
                            responseText += " The DOW is holding strong at a " + (100 * Dow_Jones_Index.marketCap) + " marketCap.";
                        }
                        IEXStockQuote S_and_P_500 = await GetStockQuote("SPY", context);
                        if (S_and_P_500.changePercent > 0)
                        {
                            responseText += " The S. and P. 500 is up " + (100 * S_and_P_500.changePercent) + " basis points.";
                        }
                        else if (S_and_P_500.changePercent < 0)
                        {
                            responseText += " The S. and P. 500 is down " + (-100 * S_and_P_500.changePercent) + " basis points.";
                        }
                        else
                        {
                            responseText += " The S. and P. 500 is holding strong at a " + (100 * S_and_P_500.marketCap) + " marketCap.";
                        }
                        //small cap S&P
                        IEXStockQuote SPDR_Small_Cap_index = await GetStockQuote("SPSM", context);
                        if (SPDR_Small_Cap_index.changePercent > 0)
                        {
                            responseText += " The S.P.D.R. Small Cap index is up " + (100 * SPDR_Small_Cap_index.changePercent) + " basis points.";
                        }
                        else if (SPDR_Small_Cap_index.changePercent < 0)
                        {
                            responseText += " The S.P.D.R. Small Cap index is down " + (-100 * SPDR_Small_Cap_index.changePercent) + " basis points.";
                        }
                        else
                        {
                            responseText += " The S.P.D.R. Small Cap index is holding strong at a " + (100 * S_and_P_500.marketCap) + " marketCap.";
                        }
                        //S&P 1000 mid and small cap index
                        IEXStockQuote S_and_P_1000 = await GetStockQuote("SPMD", context);
                        if (S_and_P_1000.changePercent > 0)
                        {
                            responseText += " The S. and P. 1000 small and mid cap fund is up " + (100 * S_and_P_1000.changePercent) + " basis points.";
                        }
                        else if (S_and_P_1000.changePercent < 0)
                        {
                            responseText += " The S. and P. 1000 small and mid cap fund is down " + (-100 * S_and_P_1000.changePercent) + " basis points.";
                        }
                        else
                        {
                            responseText += " The S. and P. 1000 small and mid cap fund is holding strong at a " + (100 * S_and_P_1000.marketCap) + " marketCap.";
                        }

                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = responseText;
                        /*
                        * NASDAQ
                        * DOW Jones Index 
                        * S & P 500
                        * some Bond index
                        * Euro market
                        * South-East Asia market
                        * 
                        */
                    break;
                        /// SectorIntent
                        /// Sample Utterances:
                        ///     what is going on in {sector}
                        ///     was is my flash briefing on {sector}
                        ///     give me a flash briefing on {sector}
                        ///     what is the status of {sector}
                        ///     check on {sector}
                        ///     Slots:
                        ///     {
                        ///         name: "sector"
                        ///         value: cryptocurrency, Utilities, Energy, Consumer Discretionary, Consumer Staples, Real Estate, 
                        ///         ... Communication Services, Financials, Industrials, Basic Materials, Technology, Healthcare
                        ///     }
                    case "SectorIntent":
                        string responseText2 = "User wants to know how a given sector is doing.";

                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = responseText2;
                            /*
                            * Sector Intent
                            * if crypto: find quote of crypto intent based on average of cryptocurrencies
                            * performance percent
                            */
                   break;
                        /// CheckPortfolioIntent
                        /// Sample Utterances: 
                        ///     portfolio status
                        ///     check portfolio
                        ///     give me a flash briefing on my portfolio
                        ///     portfolio
                        ///     how is my portfolio doing
                        ///     Slots: none
                    case "CheckPortfolioIntent":
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Portfolio to be implemented. Thanks for using market brief and stay tuned for updates.";

                        break;
                        /// AddStockToPortfolioIntent
                        /// Sample Utterances:
                        ///     portfolio status
                        ///     check portfolio
                        ///     give me a flash briefing on my portfolio
                        ///     portfolio
                        ///     how is my portfolio doing
                        ///     Slots: none
                    case "AddStockToPortfolioIntent":
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Portfolio to be implemented. Thanks for using market brief and stay tuned for updates.";

                        break;
                        /// StockPerformanceTodayIntent
                        /// Sample Utterances:
                        ///     give me a flash briefing on {stockSlot}
                        ///     how is {stockSlot} doing
                        ///     check on {stockSlot} for me
                        ///     how is {stockSlot} stock doing today
                        ///     how is {stockSlot} doing today
                        ///     Slots: 
                        ///         name: "stockSlot"
                        ///         value: "AMAZON.SearchQuery"
                    case "StockPerformanceIntent":
                        List<string> stockNames = new List<string>();
                        List<string> stockSymbols = new List<string>();
                        
                        ResolutionAuthority[] stockResponses = Intent.Slots["stockSlot"].Resolution.Authorities;
                        foreach (ResolutionValueContainer RequestSlotValue in stockResponses[0].Values) {
                            stockNames.Add(RequestSlotValue.Value.Name);
                            stockSymbols.Add(RequestSlotValue.Value.Id);
                        }
                        responseText = "";

                        if (stockSymbols.Count > 0)
                        {
                            IEXStockQuote quote = await GetStockQuote(stockSymbols[0], context);

                            if (quote.changePercent > 0)
                            {
                                responseText = quote.companyName + " stock is up with a " + quote.changePercent + " percentage increase at a price of $" + quote.latestPrice + "dollars.";
                            }
                            else if (quote.changePercent < 0)
                            {
                                responseText = quote.companyName + " stock is down with a " + quote.changePercent + " percentage decrease at a price of $" + quote.latestPrice + "dollars.";
                            }
                            else
                            {
                                responseText = quote.companyName + " stock is holding steady at " + quote.changePercent + " at a price of $" + quote.latestPrice + "dollars.";
                            }
                        } else
                        {
                            responseText = "No stocks were found for that pronunciation. Try saying the stock in question more clearly, or in a different way. " +
                                "For example, instead of Apple inc. , say Apple stock or something. Market Brief uses a fuzzy algorithm in order to determine which " +
                                "stock you are talking about. At the moment, this is the best Alexa has to offer. If you are having problems with requesting a specific stock, " +
                                "leave a review and I will personally edit the slot to try and eliminate the error. ";
                        }

                         AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = responseText;

                        break;
                        /// NewsAboutPortfolioIntent
                        /// Sample Utterances:
                        ///     portfolio news
                        ///     news about portfolio
                        ///     tell me news about my portfolio
                        ///     give me a flash briefing about my portfolio
                        ///     flash briefing about my portfolio
                        ///     Slots: none
                    case "NewsAboutPortfolioIntent":
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Portfolio to be implemented. Thanks for using market brief and stay tuned for updates.";

                        break;
                        /// NewsAboutStockIntent
                        /// Sample Utterances:
                        ///     flash briefing about {company}
                        ///     give me a flash briefing about {company}
                        ///     news about {company}
                        ///     give me news about {company}
                        ///     Slots: 
                        ///         name: "company"
                        ///         value: "AMAZON.Corporation"
                    case "NewsAboutStockIntent":
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Portfolio to be implemented. Thanks for using market brief and stay tuned for updates.";

                        break;

                    case "AddCompanyStockToPortfolioIntent":
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Portfolio to be implemented. Thanks for using market brief and stay tuned for updates.";

                        break;
                        /// Unrecognized Intent
                        /// Slots: none
                    default:
                        debugLog.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                        AlexaSpeechOutput = new PlainTextOutputSpeech();
                        (AlexaSpeechOutput as PlainTextOutputSpeech).Text = "Sorry, Alexa didn't understand that.If you are having problems with requesting a specific stock, " +
                            "leave a review and I will personally edit the slot to try and eliminate the error. "; 

                        break;
                }
            }

            AlexaSkillResponse.Response.OutputSpeech = AlexaSpeechOutput;
            AlexaSkillResponse.Version = "1.0";
            debugLog.LogLine($"Skill Response Object...");
            debugLog.LogLine(JsonConvert.SerializeObject(AlexaSkillResponse));

            return AlexaSkillResponse;
        }


        private async Task<IEXStockQuote> GetStockQuote(string stockSymbol, ILambdaContext context)
        {
            IEXStockQuote StockDataList = new IEXStockQuote();
            var IEXTrading_API_PATH = $"https://api.iextrading.com/1.0/stock/{stockSymbol}/quote/1d";
            IEXTrading_API_PATH = string.Format(IEXTrading_API_PATH, stockSymbol);

            var uri = new Uri(IEXTrading_API_PATH);
            context.Logger.LogLine($"Attempting to fetch data from {uri.AbsoluteUri}");
            try
            {
                var response = await _httpClient.GetStringAsync(uri);

                StockDataList = JsonConvert.DeserializeObject<IEXStockQuote>(response);

                context.Logger.LogLine($"Response from URL:\n{response}");
                // TODO: (PMO) Handle bad requests

            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"\nException: {ex.Message}");
                context.Logger.LogLine($"\nStack Trace: {ex.StackTrace}");
            }
            return StockDataList;
        }


        ///
        /// 
        /// END OF LAMBDA FUNCTION
    }
    ///
    /// END OF NAMESPACE
}