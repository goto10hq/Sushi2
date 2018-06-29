using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sushi2
{
    public static class AsyncTools
    {
        static readonly Lazy<TaskFactory> _taskFactory = new Lazy<TaskFactory>(() => new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default));
        static TaskFactory MyTaskFactory => _taskFactory.Value;

        /// <summary>
        /// Run sync.
        /// </summary>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return MyTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Run sync.
        /// </summary>
        public static void RunSync(Func<Task> func)
        {
            MyTaskFactory.StartNew(func).Unwrap().GetAwaiter().GetResult();
        }
    }
}