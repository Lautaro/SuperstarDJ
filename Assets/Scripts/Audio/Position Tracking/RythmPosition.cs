using SuperstarDJ.Audio.PositionTracking;
using System;

namespace SuperstarDJ.Audio
{
    public struct RythmPosition
    {
        public readonly Tick Tick;
        public readonly Double RawPosition;
        public readonly Double PaddedPosition;

        public RythmPosition( Tick tick, double rawPosition, double paddedPosition )
        {
            Tick = tick;
            RawPosition = rawPosition;
            PaddedPosition = paddedPosition;
        }


        public RythmPosition( Tick tick, double rawPosition )
        {
            Tick = tick;
            RawPosition = rawPosition;
            PaddedPosition = rawPosition;
        }
        public override string ToString()
        {
            return $"({Tick.Id.ToString ( "D2" )})  [{Tick.Measure}][{Tick.Beat}][{Tick.Index}]";
        }
    }
}
