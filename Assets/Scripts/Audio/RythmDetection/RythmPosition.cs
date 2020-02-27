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
        public readonly bool IsInHitArea;
        public readonly Measure Measure;
        public readonly Double Position; 

        public RythmPosition(Tick tick, Beat beat, Measure measure, double position, bool isInHitArea)
        {
            Tick = tick;
            Beat = beat;
            Measure = measure;
            Position = position;
            IsInHitArea = isInHitArea;
        }

        public override string ToString()
        {
            return $"[{Measure.index}]-[{Beat.positionInMeasure}]  ({Tick.positionInBeat})";
        }
    }
}
