using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.Audio.RythmDetection
{
    //public class Measure
    //{
    //    public int index;
    //    public List<Beat> beats;
    //    double startsAt;

    //    public Measure( int _index,  int amountOfBeats, int amountOfTicks, double measureDuration )
    //    {
    //        index = _index;
    //        beats = new List<Beat> ();
    //        startsAt = measureDuration * (index - 1);
            
    //        var beatsDuration = measureDuration / amountOfBeats;
    //        for ( int i = 0; i < amountOfBeats; i++ )
    //        {
    //            beats.Add ( new Beat ( i + 1, this, amountOfTicks, beatsDuration, startsAt ) );
    //        }
    //    }

    //    public void DebugBeats()
    //    {
    //        foreach ( var beat in beats )
    //        {
    //            Debug.Log ( $"  {beat.index}B - {beat.TicksToString()}" );
                
    //        }
    //    }
    //}
}
