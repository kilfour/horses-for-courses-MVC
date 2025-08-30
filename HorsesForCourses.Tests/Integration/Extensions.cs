namespace HorsesForCourses.Tests.Integration;

public static class TaskExtensions
{
    public static Task<TOut> Attach<TIn, TOut>(this Task<TOut> task, Needler<TIn, TOut> needler, string key, TIn record, int sleepy = 0)
    {
        task.ContinueWith(a =>
        {
            if (sleepy != 0)
                Thread.Sleep(sleepy); // <= introduce some lag 
            needler.Register(key, record, a.Result);
        }, TaskContinuationOptions.OnlyOnRanToCompletion);
        return task;
    }
}