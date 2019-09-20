using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace URLChecker
{
    public class LowLevelHttpRequest
    {
        public delegate void HttpStateHandler(string message);
        // Событие, возникающее при выводе денег
        public static event HttpStateHandler SuccessUrl;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static async Task/*<bool>*/ BrutForceAsync(string webUrl, CancellationToken cancellationToken)
        {
            await Task.Run(async () => // это полезно для выполнения логирования в фоновом процессе.
            {
                WebRequest webRequest = WebRequest.Create(webUrl);
                try
                {
                    webRequest.Method = "HEAD";

                    HttpWebResponse webresponse = (await webRequest.GetResponseAsync().WithCancellation(cancellationToken, webRequest.Abort)) as HttpWebResponse;

                    logger.Info($"ok| {webresponse.StatusCode:D}|{webUrl}"); // это синхронный код и без Task.Run будет блокировать форму

                    SuccessUrl?.Invoke(webUrl); // это синхронный код и без Task.Run будет блокировать форму

                    webresponse.Dispose();
                }
                catch (Exception e)
                {
                    webRequest = null;
                    logger.Error(e, $"{e.Message}|{webUrl}"); // это синхронный код и без Task.Run будет блокировать форму

                }
            });
        }
    }
}
