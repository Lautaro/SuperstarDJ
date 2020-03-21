using SuperstarDJ.Audio.PositionTracking;
using System;

namespace SuperstarDJ.Audio
{
    public struct RythmPosition
    {
        public readonly Step Step;
        public readonly Double RawPosition;
        public readonly Double PaddedPosition;

        /// <summary>
        /// RythmPosition is an exact position within the loop. It has no information om whether it represents a successfull DjAct hit.  
        /// If the position is a succesfull hit the actual hit Step might be the next one. 
        /// </summary>
        /// <param name="step">The Step the position is within</param>
        /// <param name="rawPosition">The actual position</param>
        /// <param name="paddedPosition">The posiiton used for calculation. Rawposition modified for lag.</param>
        public RythmPosition( Step step, double rawPosition, double paddedPosition)
        {
            Step = step;
            RawPosition = rawPosition;
            PaddedPosition = paddedPosition;
        }

        public RythmPosition( Step step, double rawPosition)
        {
            Step = step;
            RawPosition = rawPosition;
            PaddedPosition = rawPosition;
        }
        public override string ToString()
        {
            return $"({Step.Id.ToString ( "D2" )})  [{Step.Measure}][{Step.Beat}][{Step.Index}]";
        }
    }
}
