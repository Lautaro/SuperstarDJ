using MessageSystem;
using Sirenix.Utilities;
using SuperstarDJ.Audio.PositionTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.Audio.PatternDetection
{
    public class PatternDetector
    {
        List<Pattern> patterns = new List<Pattern> ();
        Dictionary<string, SuccesState[]> successTrackers = new Dictionary<string, SuccesState[]> ();
        Tick latestCompletedTick;
        public PatternDetector( List<Pattern> patterns_ )
        {
            patterns = patterns_;

            //  Debug.Log ( $"Loaded {patterns.Count} patterns : {string.Join ( " * ", patterns.Select ( p => p.PatternName ).ToArray () ) }" );

            foreach ( var pattern in patterns )
            {
                successTrackers.Add ( pattern.PatternName, new SuccesState[64] );
            }

            MessageHub.Subscribe ( MessageTopics.NewRythmPosition, EvaluateSuccessForSilentTick );
            MessageHub.Subscribe ( MessageTopics.TickHit_Tick, CheckHitTickForSuccess );
            MessageHub.Subscribe ( MessageTopics.TrackStartsFromZero_string, ResetPatterns );
        }

        void ResetPatterns( Message message )
        {
            foreach ( var tracker in successTrackers )
            {
                for ( int i = 0; i < tracker.Value.Length; i++ )
                {
                    tracker.Value[i] = SuccesState.Waiting;
                }
            }
        }

        void EvaluateSuccessForSilentTick( Message rythmPositionMessage )
        {
            var newPosition = rythmPositionMessage.Open<RythmPosition> ();
            var index = newPosition.Tick.Id - 1; ;
            foreach ( var pattern in patterns )
            {
                var successStates = successTrackers[pattern.PatternName];
                if ( successStates[index] != SuccesState.Waiting )
                {
                    return;// This tick has already been checked. Exit.
                }
                var patternAction = pattern.PatternStates[index];
                if ( patternAction != PatternAction.None )
                {
                    successTrackers[pattern.PatternName][index] = SuccesState.Failed;
                 //   Debug.Log ( $"Pattern '{pattern.PatternName}' has failed at tick {tick.Id} because *{patternAction.ToString ()}* was expected but tick was silent" );
                }
                else
                {
                    successTrackers[pattern.PatternName][index] = SuccesState.Sucess;
                }
            }
        }

        public void CheckHitTickForSuccess( Message rythmPositionOfHitTick )
        {
            var rythmPosition = rythmPositionOfHitTick.Open<RythmPosition> ();
            var tickWasHit = rythmPosition.Tick;
            foreach ( var pattern in patterns )
            {
                if ( successTrackers[pattern.PatternName][tickWasHit.Id] != SuccesState.Waiting )
                {
                    // This tick has already been set. Exit.
                    return;
                }
                var patternAction = pattern.PatternStates[tickWasHit.Id];
                var result = SuccesState.Failed;
                switch ( patternAction )
                {
                    case PatternAction.None:
                        Debug.Log ( $"Pattern has failed at tick {tickWasHit.Id} because *{patternAction.ToString ()}* was expected but tick *HIT*" );
                        result = SuccesState.Failed;
                        break;
                    case PatternAction.Hit:
                        result = SuccesState.Sucess; // Not yet implemented
                        break;
                    case PatternAction.Hold:
                        result = SuccesState.Failed; // Not yet implemented
                        break;
                    default:
                        break;
                }
                successTrackers[pattern.PatternName][tickWasHit.Id] = result;
            }
        }

        void SetSuccessForStep( string patternName, int index, SuccesState success )
        {
            successTrackers[patternName][index] = success;
        }
        public void ClearAllSuccessTrackers()
        {
            foreach ( var dic in successTrackers )
            {
                dic.Value.ForEach ( ( ss ) => ss = SuccesState.Waiting );
            }
        }

        public bool IsPatternSuccessfull( string patternName )
        {
            return successTrackers[patternName].Any ( ss => ss != SuccesState.Sucess );

        }
    }
}
