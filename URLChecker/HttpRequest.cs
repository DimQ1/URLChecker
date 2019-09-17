using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    public class LowLevelHttpRequest
    {

        public delegate void HttpStateHandler(string message);
        // Событие, возникающее при выводе денег
        public static event HttpStateHandler SuccessUrl;



        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static async Task/*<bool>*/ BrutForceAsync(string webUrl)
        {
            await Task.Run(async () =>
            {
                WebRequest webRequest = WebRequest.Create(webUrl);
                try
                {
                    webRequest.Method = "HEAD";

                    HttpWebResponse webresponse = (await webRequest.GetResponseAsync()) as HttpWebResponse;

                    logger.Info($"ok| {webresponse.StatusCode:D}|{webUrl}");

                    SuccessUrl?.Invoke(webUrl);

                    //webresponse.Close();
                    webresponse.Dispose();

                }
                catch (Exception e)
                {
                    webRequest = null;
                    logger.Error(e, $"{e.Message}|{webUrl}");
                    
                }
            });
        }
    }
}
