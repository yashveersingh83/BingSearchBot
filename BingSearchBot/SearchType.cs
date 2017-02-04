using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BingSearchBot
{

public enum SearchType {
        [Describe("Search only web result")]        
        WebSearch ,
        [Describe("Search only images")]
        ImageSearch,
            [Describe("Detect face emotions")]
        Emotion

    }
    [Serializable]
    public class SearchOrder
    {
        [Prompt("What size of search do you want? {||}")]
        public SearchType? Search;
        [Prompt("Please enter search term ? {||}")]
        public string SearchQuery { get; set; }

        public List<Attachment> Attachments { get; set; }
       
        public static IForm<SearchOrder> BuildForm()
        {

            OnCompletionAsyncDelegate<SearchOrder> search = async (context, state) =>
            {
                //context.ConversationData.GetValue("A");


                var reply = context.MakeMessage();
                switch(state.Search)
                {
                    case SearchType.WebSearch:  reply = await new BingSearch().Search(reply, state.SearchQuery);
                        break;
                    case SearchType.ImageSearch:
                        reply = await new BingSearch().SearchImages(reply, state.SearchQuery);
                        break;

                    case SearchType.Emotion:
                        reply = await new EmotionSearch().UploadAndDetectEmotions(state.SearchQuery);
                        break;
                    default : reply = await new BingSearch().Search(reply, state.SearchQuery);break;

                }
                await context.PostAsync(reply);
            };
            return new FormBuilder<SearchOrder>()
                    .Message("Welcome to the simple search bot!")
                    .Field(nameof(Search))              
                    
                    .OnCompletion(search)
                    .AddRemainingFields()
                    .Build();


           
    }
    }

    
}