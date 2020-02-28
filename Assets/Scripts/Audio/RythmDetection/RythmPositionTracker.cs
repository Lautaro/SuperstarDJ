using MessageSystem;
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
        Measure[] measures;
        Beat[] beats;
        Tick[] ticks;
        public const int beatTrackingPadding = 2000;
        static public bool DebugEnabled = false;
        readonly double trackDuration;

        public RythmPositionTracker( int measuresPerLoop, int beatsPerMeasure, int ticksPerBeat, double _trackDuration )
        {
            trackDuration = _trackDuration;
            var measureDuration = _trackDuration / measuresPerLoop;
            measures = new Measure[measuresPerLoop];

            for ( int i = 0; i < measuresPerLoop; i++ )
            {
                measures[i] = new Measure ( i + 1, beatsPerMeasure, ticksPerBeat, measureDuration );
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
            var paddedPosition = currentPositionInClip + beatTrackingPadding;
            if ( paddedPosition > trackDuration )
            {
                return new RythmPosition ( ticks[0], beats[0], measures[0] ,0) ;
            }

            var tick = ticks.FirstOrDefault ( t => t.HasPosition ( paddedPosition ) );
            if ( tick.parentBeat == null ) { Debug.LogWarning ( "RythmPosition update found no matching tick at position :" + currentPositionInClip );   };

            return new RythmPosition ( tick, tick.parentBeat, tick.parentBeat.parentMeasure, currentPositionInClip);
        }

        internal RythmPosition UpdateCurrentRythmPosition( RythmPosition rythmPosition, int currentPositionInClip )
        {
            var previousRp = rythmPosition;
            rythmPosition = GetPositionInRythm ( currentPositionInClip );

            if ( previousRp.Measure == null ) return rythmPosition;

            if ( previousRp.Measure.index != rythmPosition.Measure.index )
            {
                MessageHub.PublishNews<RythmPosition> ( MessageTopics.NextMeasure_string, rythmPosition );
                if ( DebugEnabled ) Debug.Log ( $"------ {rythmPosition.Measure.index} ------ ({currentPositionInClip})" );
            }

            if ( previousRp.Beat.index != rythmPosition.Beat.index )
            {
                MessageHub.PublishNews<RythmPosition> ( MessageTopics.NextBeat_string, rythmPosition );
             if( DebugEnabled )   Debug.Log ( "NewBeat: " + rythmPosition.Beat.index  + $"({ currentPositionInClip})");
            }
            return rythmPosition;
        }
    }
}
