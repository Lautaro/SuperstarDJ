using SuperstarDJ.Audio.PositionTracking;
using System;

namespace SuperstarDJ.Audio
{
    public struct RythmPosition
    {
        public readonly Step Step;
        public readonly Double RawPosition;
        public readonly Double PaddedPosition;
        public readonly bool WasHitButMissed;

        public RythmPosition( Step step, double rawPosition, double paddedPosition, bool wasHitButMissed )
        {
            Step = step;
            RawPosition = rawPosition;
            PaddedPosition = paddedPosition;
            WasHitButMissed = wasHitButMissed;
        }


        public RythmPosition( Step step, double rawPosition,  bool wasHit )
        {
            Step = step;
            RawPosition = rawPosition;
            PaddedPosition = rawPosition;
            WasHitButMissed = wasHit;
        }
        public override string ToString()
        {
            return $"({Step.Id.ToString ( "D2" )})  [{Step.Measure}][{Step.Beat}][{Step.Index}]";
        }
    }
}
