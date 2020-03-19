using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MrRobot
{
    public static class Analytics
    {
        static WebClient webClient = new WebClient();
         static Analytics()
        {
            webClient.Headers.Add("UserAgent", "test");
        }
        public static void Log(Guid id,string eventCategory,string eventAction,string eventLabel,int eventValue)
        {
          //  Console.WriteLine(eventCategory + "," + eventAction + "," + eventLabel);
          //  return;
            try
            {
                string UA = "1";
                webClient.UploadStringAsync(new Uri("https://www.google-analytics.com/collect"),
    $"v=1&tid=UA-{UA}-1&cid={id}&t=event&ec={eventCategory}&ea={eventAction}&el={eventLabel}&ev={eventValue}");
            }
            catch { }
        }
    }
}
