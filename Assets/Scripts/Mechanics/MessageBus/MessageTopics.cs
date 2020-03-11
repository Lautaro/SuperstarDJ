using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperstarDJ.MessageSystem
{
    public enum MessageTopics
    {
        DisplayUI_FX_string, 
        NewRythmPosition,
        SongStarted_string,
        TrackStarted_Track,
        TrackStopped_Track,
        TickHit_Tick,
        TrackStartsFromZero_string
    }
}
