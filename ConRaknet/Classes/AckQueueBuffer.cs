
namespace ConMaster.Raknet
{
    public class AckQueueBuffer(AckRecord[] rangeBuffer)
    {
        public AckRecord this[int index]
        {
            get
            {
                return RangeBuffer[index];
            }
        }
        private readonly AckRecord[] RangeBuffer = rangeBuffer;
        private volatile int _length;
        public int Length => _length + 1;
        public bool IsEmpty => _length == -1;
        public void Add(int item)
        {
            lock (this)
            {
                if (IsEmpty) _length++;
                ref AckRecord range = ref RangeBuffer[_length];
                if (item == range.High + 1)
                {
                    range.High++;
                    //Console.WriteLine("Continue: " + range.High);
                }
                else
                {
                    RangeBuffer[++_length] = range;
                    range = ref RangeBuffer[_length];
                    range.Low = item;
                    range.High = item;
                }
            }
        }
        public void Clear() {
            lock (this)
            {
                RangeBuffer[0] = RangeBuffer[_length];
                _length = -1;
            }
        }
        public ReadOnlySpan<AckRecord> GetRecords() => RangeBuffer.AsSpan(0, Length);
    }
}
