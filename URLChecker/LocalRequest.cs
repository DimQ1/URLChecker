using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    public class LocalRequest
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static async Task BrutForceAsync(string webUrl, string[] arStr)
        {
            await Task.Run(async () =>
            {
                bool flag = false;
                for (Int32 i=0; i<arStr.Length; i++)
                {
                    if (arStr[i] == webUrl) { flag = true; break; }
                }

                if (flag)
                {
                    logger.Info($"ok| {webUrl}");
                }
                else
                {
                    logger.Error("Ошибка - " + webUrl);
                }

                /*
                WebRequest webRequest = WebRequest.Create(webUrl);
                try
                {
                    webRequest.Method = "HEAD";

                    HttpWebResponse webresponse = (await webRequest.GetResponseAsync()) as HttpWebResponse;

                    logger.Info($"ok| {webresponse.StatusCode:D}|{webUrl}");

                    webresponse.Close();
                }
                catch (Exception e)
                {
                    logger.Error(e, $"{e.Message}|{webUrl}");
                }
                */




            });
        }
    }
}
