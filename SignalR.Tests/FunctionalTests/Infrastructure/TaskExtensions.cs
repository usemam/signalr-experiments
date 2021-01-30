using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionalTests.Infrastructure
{
    public static class TaskExtensions
    {
        private const int DefaultTimeout = 10 * 1000;
        
        public static Task<T> OrTimeout<T>(
            this Task<T> task,
            int milliseconds = DefaultTimeout,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int? lineNumber = null)
        {
            return OrTimeout(task, TimeSpan.FromMilliseconds(milliseconds), memberName, filePath, lineNumber);
        }

        public static async Task<T> OrTimeout<T>(
            this Task<T> task,
            TimeSpan timeout,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int? lineNumber = null)
        {
            if (task.IsCompleted)
            {
                return await task;
            }

            var cts = new CancellationTokenSource();
            var completed = await Task.WhenAny(
                task, Task.Delay(Debugger.IsAttached ? Timeout.InfiniteTimeSpan : timeout, cts.Token));
            if (completed != task)
            {
                throw new TimeoutException(GetMessage(memberName, filePath, lineNumber));
            }
            cts.Cancel();

            return await task;
        }
        
        private static string GetMessage(string memberName, string filePath, int? lineNumber)
        {
            if (!string.IsNullOrEmpty(memberName))
            {
                return $"Operation in {memberName} timed out at {filePath}:{lineNumber}";
            }
            else
            {
                return "Operation timed out";
            }
        }
        
    }
}