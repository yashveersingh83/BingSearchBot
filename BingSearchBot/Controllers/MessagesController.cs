using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Builder.Dialogs;
using System;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Http.Description;

namespace BingSearchBot
{
    [BotAuthentication]
    public class ActivitysController : ApiController
    {
        /// <summary>
        /// POST: api/Activitys
        /// Receive a Activity from a user and reply to it
        /// </summary>
        /// 

        internal static IDialog<SearchOrder> MakeRootDialog()
        {
            return Chain.From(() => FormDialog.FromForm(SearchOrder.BuildForm));
            
        }

        [ResponseType(typeof(void))]
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            try
            {
                if (activity != null)
                {
                    // one of these will have an interface and process it
                    switch (activity.GetActivityType())
                    {
                        case ActivityTypes.Message:
                            if (activity.Attachments.Count == 0)
                                await Conversation.SendAsync(activity, MakeRootDialog);
                            else
                            {
                                await new EmotionSearch().UploadAndDetectEmotions(activity.Attachments[0].ContentUrl);
                            }
                            break;

                        case ActivityTypes.ConversationUpdate:
                        case ActivityTypes.ContactRelationUpdate:
                        case ActivityTypes.Typing:
                        case ActivityTypes.DeleteUserData:
                        default:
                            Trace.TraceError($"Unknown activity type ignored: {activity.GetActivityType()}");
                            break;
                    }
                }


                if (activity.Attachments.Count == 0)
                    await Conversation.SendAsync(activity, MakeRootDialog);
                else
                {
                    await new EmotionSearch().UploadAndDetectEmotions(activity.Attachments[0].ContentUrl);
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
            }
            catch (Exception t)
            {
                throw t;
            }
              
        }

        private Activity HandleSystemActivity(Activity Activity)
        {
            if (Activity.Type == "Ping")
            {
                Activity reply = Activity.CreateReply();
                reply.Type = "Ping";
                return reply;
            }
            else if (Activity.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real Activity
            }
            else if (Activity.Type == "BotAddedToConversation")
            {
            }
            else if (Activity.Type == "BotRemovedFromConversation")
            {
            }
            else if (Activity.Type == "UserAddedToConversation")
            {
            }
            else if (Activity.Type == "UserRemovedFromConversation")
            {
            }
            else if (Activity.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}