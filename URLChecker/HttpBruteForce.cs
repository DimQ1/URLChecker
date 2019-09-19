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

        public HttpBruteForce(int parralelCount = 10)
        {
            _parralelCount = parralelCount;
            
            ServicePointManager.DefaultConnectionLimit = parralelCount;
        }

        public async Task StartBruteForce(Stack<string> urls, CancellationToken cancellationToken)
        {
            _urls = urls;
            List<Task> tasks = new List<Task>();

            while (_urls.Count > 0)
            {
                tasks.Add(LowLevelHttpRequest.BrutForceAsync(_urls.Pop(), cancellationToken));
                //Console.WriteLine(_urls.Count());             //непонятно зачем

                if (tasks.Count > _parralelCount)
                {
                    await Task.WhenAny(tasks.ToArray());
                    tasks = CleanFinishTasks(tasks);
                }
            }

            if (tasks.Count > 0)
            {
                tasks = CleanFinishTasks(tasks);
                /*Task ok_optimization = */await Task.WhenAll(tasks.ToArray());

                //return true;       //!!!!! пробуем возврат значений
            }

            //return false;       //!!!!! пробуем возврат значений
        }

        private List<Task> CleanFinishTasks(List<Task> tasks)
        {
            List<Task> cleanTaskList = new List<Task>();

            foreach (var task in tasks)
            {
                if (!task.IsCompleted && !task.IsFaulted)
                {
                    cleanTaskList.Add(task);
                }
            }

            return cleanTaskList;
        }
    }
}
