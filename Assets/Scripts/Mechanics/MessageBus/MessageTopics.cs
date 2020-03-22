using System;

namespace SuperstarDJ.MessageSystem
{
    [Flags]
    public enum MessageTopics
    {
        DisplayUI_FX_string = 1 << 1,
        NewRythmPosition = 1 << 20,
        SongStarted_string = 1 << 30,
        TrackStarted_Track = 1 << 40,
        TrackStopped_Track = 1 << 50,
        StepHit_Step = 1 << 60,
        HitMissed_Step = 1 << 70,
        ResetRythmLoop = 1 << 80,
        HitRangePassed_Step = 1<<90

    }
}
