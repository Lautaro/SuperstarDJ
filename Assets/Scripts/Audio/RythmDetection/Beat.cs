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
        public int index;
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
            index = _index;
            parentMeasure = parent;
            ticks = new List<Tick> ();

            startsAt = beatDuration * ( index - 1 ) + measureStartsAt;
            endsAt = ( beatDuration * index ) + measureStartsAt;
            var ticksDuration = beatDuration / amountOfTicks;

            for ( int i = 0; i < amountOfTicks; i++ )
            {
                ticks.Add ( new Tick ( i+1, this, ticksDuration, startsAt ) );
            }

            hitAreaEndsAt = ticks[0].endsAt;
        }

        public void DebugTicks()
        {
            Debug.Log ( $"         { string.Join(", ", ticks.Select(t => t.index))}" );
        }

        internal string TicksToString()
        {
            return  string.Join ( ", ", ticks.Select ( t => t.index ) ) ;
        }
    }
}
