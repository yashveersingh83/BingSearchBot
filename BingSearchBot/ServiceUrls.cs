using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BingSearchBot
{
    public static class ServiceUrls
    {
        public static string BingSearchUrl = "https://api.cognitive.microsoft.com/bing/v5.0/search/";
        public static string bingKey = "578cf22df494422c899bfbe5efddaa96";
        public static string BingImageSearchUrl = "https://api.cognitive.microsoft.com/bing/v5.0/images/search?";
        public static string faceMotionKey = "8d71a0af48024aeaac315a0f5c6a43b5";
        public static string FaceEmotionUrl = "https://api.projectoxford.ai/emotion/v1.0";
    }
}