using System.Diagnostics;
using ConMaster.Deepslate.Worlds;

namespace ConMaster.Deepslate.Service
{
    public partial class Game
    {
        private long WaitTicksTimeSpan = 1_000_0000L / 20;
        private ulong _tick = 0;

        public event Action<Game, ulong>? OnTick;
        public ulong CurrentTick => _tick;
        public double CurrentTPS { get; private set; }
        public double CurrentEffectiveTPSRatio { get; private set; }
        public int TicksPerSecond { get => (int)(1_000_0000L / WaitTicksTimeSpan); set => WaitTicksTimeSpan = 1_000_0000 / value; }
        public virtual async Task Run(CancellationToken token)
        {
            Stopwatch sw = Stopwatch.StartNew();
            do
            {
                World._RunTick(World, _tick);
                OnTick?.Invoke(this, _tick);
                System.Tick(_tick);
                double RunTimeTPS = (sw.ElapsedTicks / (double)WaitTicksTimeSpan);


                //End Of Jobs and wait until there is right time
                long ticks = WaitTicksTimeSpan - sw.ElapsedTicks;
                while (ticks > 20_0000)
                {
                    await Task.Delay(1);
                    ticks = WaitTicksTimeSpan - sw.ElapsedTicks;
                }


                // 1000 ticks == 0.1ms -> just in case the logic at the end of the tick doesn't consume performance
                while (ticks > 1000) ticks = WaitTicksTimeSpan - sw.ElapsedTicks;
                
                //Pre Restart
                Interlocked.Increment(ref _tick);
                CurrentTPS = 1_000_0000d / sw.ElapsedTicks;
                CurrentEffectiveTPSRatio = RunTimeTPS;
                sw.Restart();
            }
            while (!token.IsCancellationRequested);
        }
    }
}