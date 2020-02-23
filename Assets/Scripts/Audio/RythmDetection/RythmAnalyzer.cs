using SuperstarDJ.Audio.RhythmDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.Audio.RythmDetection
{
    class RythmAnalyzer
    {
        public static RythmAnalyzer Instance;
        List<Measure> measures = new List<Measure> ();

        float trackDuration; 
        float currentPosition;

        public RythmAnalyzer(int measuresPerLoop, int beatsPerMeasure, int ticksPerBeat, double trackDuration)
        {
            if ( Instance != null ) {Debug.LogError ( "A second RythmAnalyser is instanciating. This is a no no." );}

            var measureDuration = trackDuration / measuresPerLoop;

            for ( int i = 0; i < measuresPerLoop; i++ )
            {
                measures.Add ( new Measure ( i + 1, beatsPerMeasure, ticksPerBeat, measureDuration ) );
            }
            DebugMeasures ();
        }
        public void DebugMeasures()
        {
            foreach ( var measure in measures )
            {
                Debug.Log ( $"{measure.index}M");
                measure.DebugBeats ();
                Debug.Log ( "" );
            }
        }
    }
}
