using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Bot.Connector;

namespace BingSearchBot
{
    [Serializable]
    public   class BingSearch//:IDialog<object>
    {
      
        public static  HttpClient GetHttpClientConnection ()
        {
            var client = HttpClientFactory.Create();           
            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ServiceUrls.bingKey.ToString());
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return  client;
        }
        
        public async Task<IMessageActivity> Search(IMessageActivity replyMsg ,string query)
        {
            try
            {
                // Request parameters
                var client = BingSearch.GetHttpClientConnection();
                replyMsg.Attachments = new List<Attachment>();
                var queryString = ServiceUrls.BingSearchUrl + "?q=" + query + "&count=10" + "&mkt=en-us";
                int i = 0;
                string r = await client.GetStringAsync(queryString);
                var jsonResult = JsonConvert.DeserializeObject<RootObject>(r);
                var result = new StringBuilder();
                /// web result 

                foreach (var jresult in jsonResult.webPages.value.ToList())
                {
                    replyMsg.Attachments.Add(new Attachment
                    {
                       
                        Name = jresult.name,
                        Content = jresult.snippet,
                        ContentUrl = jresult.url

                    }
                    );
                   

                }
                return replyMsg;
            }
            catch (Exception t)
            {
                throw t;
            }
        }

        public  async Task<IMessageActivity> SearchImages(IMessageActivity replyMsg, string query)
        {
            try
            {
                var client = BingSearch.GetHttpClientConnection();
                var imagelist = new List<Attachment>();
                var queryString = ServiceUrls.BingImageSearchUrl + "q=" + query + "&count=10" + "&mkt=en-us";
                replyMsg.Attachments = new List<Attachment>();
                string r = await client.GetStringAsync(queryString);
                var jsonResult = JsonConvert.DeserializeObject<BingImageSearchResponse.RootObject>(r);
                foreach (var rjson in jsonResult.value.ToList())
                {
                    replyMsg.Attachments.Add(

                        new Attachment
                        {
                            Name = rjson.name,
                            ThumbnailUrl = rjson.thumbnailUrl,
                            ContentType = "image / png",
                            ContentUrl = rjson.contentUrl
                        }


                        );
                }
                return replyMsg;

            }
            catch (Exception t)
            {
                throw t;
            }

        }

        

    }
}