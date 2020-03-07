using SuperstarDJ.Audio.RythmDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarDJ.Audio
{
    public struct RythmPosition
    {
        public readonly Tick Tick;
        public readonly Double Position;
        public readonly bool WasHit;

        public RythmPosition(Tick tick, double position, bool wasHit)
        {   
            Tick = tick;
            Position = position;
            WasHit = wasHit;
        }
    }
}
