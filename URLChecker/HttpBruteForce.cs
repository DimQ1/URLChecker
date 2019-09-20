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
    public class HttpBruteForce
    {
        private readonly int _parralelCount;
        private Stack<string> _urls;
        private string _baseUrl;
        private const int MaxConnectionsLimit = 10000;

        public HttpBruteForce(int parralelCount = 10, string baseUrl = null)
        {
            _parralelCount = parralelCount;
            _baseUrl = baseUrl;

            ServicePointManager.DefaultConnectionLimit = parralelCount > MaxConnectionsLimit ? parralelCount: MaxConnectionsLimit;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.ReusePort = true;
            ServicePointManager.EnableDnsRoundRobin = true;
        }

        public async Task StartBruteForce(Stack<string> urls, CancellationToken cancellationToken)
        {
            _urls = urls;
            Task[] tasks = new Task[_parralelCount > urls.Count ? urls.Count : _parralelCount];

            while (_urls.Count > 0)
            {
                AddTasks(tasks, cancellationToken);

                await Task.WhenAny(tasks.ToArray());

            }

            if (tasks.Length > 0)
            {
                await Task.WhenAll(tasks.Where(task => !task.IsCompleted && !task.IsFaulted && task.IsCanceled).ToArray());
            }

        }

        private void AddTasks(Task[] tasks, CancellationToken cancellationToken)
        {
            for (var i = 0; i < tasks.Length; i++)
            {
                var currentTask = tasks[i];
                if (currentTask == null || currentTask.IsCompleted || currentTask.IsFaulted || currentTask.IsCanceled)
                {
                    tasks[i] = LowLevelHttpRequest.BrutForceAsync(_urls.Pop(), cancellationToken);
                }
            }
        }
    }
}
