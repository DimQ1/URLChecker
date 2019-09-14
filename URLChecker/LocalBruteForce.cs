using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLChecker
{
    public class LocalBruteForce
    {
        private readonly int _parralelCount;
        private Stack<string> _urls;

        public LocalBruteForce(int parralelCount = 10)
        {
            _parralelCount = parralelCount;

            //ServicePointManager.DefaultConnectionLimit = parralelCount;
        }

        public async Task StartBruteForce(Stack<string> urls, string[] arStr)
        {
            _urls = urls;
            List<Task> tasks = new List<Task>();

            while (_urls.Count > 0)
            {
                tasks.Add(LocalRequest.BrutForceAsync(_urls.Pop(), arStr));
                //Console.WriteLine(_urls.Count());

                if (tasks.Count > _parralelCount)
                {
                    await Task.WhenAny(tasks.ToArray());
                    tasks = CleanFinishTasks(tasks);
                }
            }

            if (tasks.Count > 0)
            {
                tasks = CleanFinishTasks(tasks);
                await Task.WhenAll(tasks.ToArray());
            }

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
