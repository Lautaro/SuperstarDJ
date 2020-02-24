using SuperstarDJ.Audio.RythmDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.Audio.RythmDetection
{
    class RythmPositionTracker
    {
        public static RythmPositionTracker Instance;
        Measure[] measures;
        Beat[] beats;
        Tick[] ticks;

        float trackDuration;
        float currentPosition;

        public RythmPositionTracker( int measuresPerLoop, int beatsPerMeasure, int ticksPerBeat, double trackDuration )
        {
            if ( Instance != null ) { Debug.LogError ( "A second RythmAnalyser is instanciating. This is a no no." ); }

            var measureDuration = trackDuration / measuresPerLoop;
            measures = new Measure[measuresPerLoop ];

            for ( int i = 0; i < measuresPerLoop; i++ )
            {
                measures[i] = new Measure ( i + 1, beatsPerMeasure, ticksPerBeat, measureDuration) ;
            }
            beats = measures.SelectMany ( m => m.beats ).ToArray ();
            ticks = beats.SelectMany ( b => b.ticks ).ToArray ();
        }
        public void DebugMeasures()
        {
            foreach ( var measure in measures )
            {
                Debug.Log ( $"{measure.index}M" );
                measure.DebugBeats ();
                Debug.Log ( "" );
            }
        }

        internal RythmPosition GetPositionInRythm( double currentPositionInClip )
        {
            var tick = ticks.First ( t => t.HasPosition ( currentPositionInClip ) );
            return new RythmPosition ( tick, tick.parentBeat, tick.parentBeat.parentMeasure, currentPosition );
        }
    }
}
