using Microsoft.Bot.Connector;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BingSearchBot
{
    public class EmotionSearch
    {
        public async Task<Activity> UploadAndDetectEmotions(string uri)
        {
            EmotionServiceClient emotionServiceClient = new EmotionServiceClient(ServiceUrls.faceMotionKey);

            //replyMsg.Attachments = new List<Attachment>();
            try
            {
                Emotion[] emotionResult;
                byte[] byteData = Encoding.UTF8.GetBytes("{ 'url': '" + uri + "' }");
                var Activity = new Activity();
                Activity.Attachments = new List<Attachment>();

                //using (Stream imageFileStream = new MemoryStream(byteData))
                //{
                //    //
                //    // Detect the emotions in the URL
                //    //
                //    emotionResult = await emotionServiceClient.RecognizeAsync(imageFileStream);
                 
                //    ListEmotionResult(Activity, emotionResult);
                //    return Activity;
                //}

                var url = "https://api.projectoxford.ai/emotion/v1.0/recognize";
                HttpResponseMessage response = null;
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ServiceUrls.faceMotionKey);
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(url, content).ConfigureAwait(false);
                }

                string responseString = await response.Content.ReadAsStringAsync();

                Emotion[] faces = JsonConvert.DeserializeObject<Emotion[]>(responseString);
                ListEmotionResult(Activity, faces);
                return Activity;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void ListEmotionResult(Activity msg,  Emotion[] emotionResult)
        {
            List<Attachment> emotionList = new List<Attachment>();
            if (emotionResult != null)
            {
                EmotionResultDisplay[] resultDisplay = new EmotionResultDisplay[8];
                
                for (int i = 0; i < emotionResult.Length; i++)
                {
                    Emotion emotion = emotionResult[i];
                    resultDisplay[0] = new EmotionResultDisplay { EmotionString = "Anger", Score = emotion.Scores.Anger };
                    resultDisplay[1] = new EmotionResultDisplay { EmotionString = "Contempt", Score = emotion.Scores.Contempt };
                    resultDisplay[2] = new EmotionResultDisplay { EmotionString = "Disgust", Score = emotion.Scores.Disgust };
                    resultDisplay[3] = new EmotionResultDisplay { EmotionString = "Fear", Score = emotion.Scores.Fear };
                    resultDisplay[4] = new EmotionResultDisplay { EmotionString = "Happiness", Score = emotion.Scores.Happiness };
                    resultDisplay[5] = new EmotionResultDisplay { EmotionString = "Neutral", Score = emotion.Scores.Neutral };
                    resultDisplay[6] = new EmotionResultDisplay { EmotionString = "Sadness", Score = emotion.Scores.Sadness };
                    resultDisplay[7] = new EmotionResultDisplay { EmotionString = "Surprise", Score = emotion.Scores.Surprise };

                    Array.Sort(resultDisplay, CompareDisplayResults);

                    String[] emotionStrings = new String[3];
                    for (int j = 0; j < 3; j++)
                    {
                        emotionList.Add(new Attachment { Name = resultDisplay[j].EmotionString + ":" + resultDisplay[j].Score.ToString("0.000000") });
                       
                        //emotionStrings[j] = resultDisplay[j].EmotionString + ":" + resultDisplay[j].Score.ToString("0.000000"); ;
                    }

                    msg.Attachments = emotionList;
                }
            }
         
        }

        private int CompareDisplayResults(EmotionResultDisplay result1, EmotionResultDisplay result2)
        {
            return ((result1.Score == result2.Score) ? 0 : ((result1.Score < result2.Score) ? 1 : -1));
        }

        internal class EmotionResultDisplay
        {
            public string EmotionString
            {
                get;
                set;
            }
            public float Score
            {
                get;
                set;
            }

            public int OriginalIndex
            {
                get;
                set;
            }
        }
    }
}