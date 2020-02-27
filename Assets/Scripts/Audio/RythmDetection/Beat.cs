using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.Audio.RythmDetection
{
    public class Beat
    {
        public int positionInMeasure;
        public readonly Measure parentMeasure;
        public List<Tick> ticks;
        double startsAt;
        double endsAt;
        double hitAreaEndsAt;
        public bool isInHitArea(double samplePosition)
        {
            return samplePosition >= startsAt && samplePosition <= hitAreaEndsAt;
        }
        public Beat(int _index, Measure parent, int amountOfTicks, double beatDuration, double measureStartsAt )
        {
            positionInMeasure = _index;
            parentMeasure = parent;
            ticks = new List<Tick> ();

            startsAt = beatDuration * ( positionInMeasure - 1 ) + measureStartsAt;
            endsAt = ( beatDuration * positionInMeasure ) + measureStartsAt;
            var ticksDuration = beatDuration / amountOfTicks;

            for ( int i = 0; i < amountOfTicks; i++ )
            {
                ticks.Add ( new Tick ( i+1, this, ticksDuration, startsAt ) );
            }

            hitAreaEndsAt = ticks[0].endsAt;
        }

        public void DebugTicks()
        {
            Debug.Log ( $"         { string.Join(", ", ticks.Select(t => t.positionInBeat))}" );
        }

        internal string TicksToString()
        {
            return  string.Join ( ", ", ticks.Select ( t => t.positionInBeat ) ) ;
        }
    }
}
