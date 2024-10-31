using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Raknet
{
    public enum FrameReliability: byte
    {
        Unreliable,
        UnreliableSequenced,
        Reliable,
        ReliableOrdered,
        ReliableSequenced,
        UnreliableWithAckReceipt,
        ReliableWithAckReceipt,
        ReliableOrderedWithAckReceipt
    }
}
