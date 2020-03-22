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
        DjActHit_DjAct = 1 << 60,
        DjActMissed_DjAct = 1 << 70,
        LastHitRangeOfLoopPassed = 1 << 79,
        ResetLoop = 1 << 80,
        HitRangePassed_Step = 1<<90

    }
}
