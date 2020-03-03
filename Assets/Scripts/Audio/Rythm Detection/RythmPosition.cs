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
        public readonly Beat Beat;
        public readonly Measure Measure;
        public readonly Double Position;
        
        public RythmPosition(Tick tick, Beat beat, Measure measure, double position)
        {
            Tick = tick;
            Beat = beat;
            Measure = measure;
            Position = position;
        }

        public bool IsInHitArea()
        {
            return Tick.index == 1  || Tick.index == 3;
        }

        public override string ToString()
        {
            return $"[{Measure.index}]-[{Beat.index}]  ({Tick.index})";
        }
    }
}
