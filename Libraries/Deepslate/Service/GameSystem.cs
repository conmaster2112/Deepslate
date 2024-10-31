using System.Collections.Concurrent;

namespace ConMaster.Deepslate.Service
{
    public class GameSystem(Game game)
    {
        public Game Game { get; init; } = game;
        private readonly SystemTaskManager _Tasks = new();

        public Task WaitTicks(int ticks)
        {
            TaskCompletionSource task = new();
            if (ticks < 0) task.SetException(new Exception("Invalid wait ticks input: " + ticks));
            else _Tasks.AddTask(task.SetResult, (uint)ticks);
            return task.Task;
        }
        public void SetTimeout(Action action, int ticks)
        {
            if (ticks < 0) throw new Exception("Invalid wait ticks input: " + ticks);
            _Tasks.AddTask(action, (uint)ticks);
        }
        public void Tick(ulong tick)
        {
            _Tasks.Tick(tick);
        }
        protected class SystemTaskManager
        {
            private ulong _tick = ulong.MaxValue;
            private readonly ConcurrentDictionary<ulong, ConcurrentQueue<Action>> _Dictionary = new();
            private readonly ConcurrentQueue<Action> _RealQuickQueue = new();
            public void AddTask(Action task, uint WaitTicks)
            {
                if (WaitTicks == 0) _RealQuickQueue.Enqueue(task);
                else
                {
                    ulong tickId = _tick + WaitTicks;
                    if (!_Dictionary.TryGetValue(tickId, out ConcurrentQueue<Action>? bag))
                    {
                        bag = [];
                        _Dictionary[tickId] = bag;
                    }
                    bag.Enqueue(task);
                }
            }
            private static void SafeInvoke(Action action)
            {
                try
                {
                    action();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public void Tick(ulong tick)
            {
                _tick = tick;
                if (_Dictionary.TryRemove(tick, out ConcurrentQueue<Action>? actions))
                {
                    while (actions.TryDequeue(out Action? action)) SafeInvoke(action);
                }
                while (_RealQuickQueue.TryDequeue(out Action? action)) SafeInvoke(action);
            }
        }
    }
}
