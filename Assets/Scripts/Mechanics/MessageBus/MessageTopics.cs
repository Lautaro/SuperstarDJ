using System;

namespace SuperstarDJ.MessageSystem
{
    [Flags]
    public enum MessageTopics
    {
        DisplayUI_FX_string = 1 << 1,
        NewRythmPosition = 1 << 2,
        SongStarted_string = 1 << 3,
        TrackStarted_Track = 1 << 4,
        TrackStopped_Track = 1 << 5,
        TickHit_Tick = 1 << 6,
        TrackStartsFromZero_string = 1 << 7
    }
}
