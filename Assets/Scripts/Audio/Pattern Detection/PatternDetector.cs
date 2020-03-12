using SuperstarDJ.MessageSystem;
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
        //    Dictionary<string, SuccesState[]> successTrackers = new Dictionary<string, SuccesState[]> ();
        Tick latestCompletedTick;
        public PatternDetector( List<Pattern> patterns_ )
        {
            patterns = patterns_;

            //  Debug.Log ( $"Loaded {patterns.Count} patterns : {string.Join ( " * ", patterns.Select ( p => p.PatternName ).ToArray () ) }" );

            MessageHub.Subscribe ( MessageTopics.NewRythmPosition, EvaluatePreviousStepForSilentTick );
            MessageHub.Subscribe ( MessageTopics.TickHit_Tick, CheckHitTickForSuccess );
            MessageHub.Subscribe ( MessageTopics.TrackStartsFromZero_string, ResetPatterns );
            MessageHub.Subscribe ( MessageTopics.SongStarted_string, ResetPatterns );
        }
        void ResetPatterns( Message message )
        {
            foreach ( var pattern in patterns )
            {
                if ( pattern.StepStatuses.Any ( ss => ss == PatternStepStatus.Waiting ) )
                    Debug.LogError ( "Patterns cant reset while there is still a step that has not been evaluated" );
                pattern.ResetStepStatus ();
            }

            DebugPatternSuccessAtEndOfLoop ();
        }
        private void DebugPatternSuccessAtEndOfLoop()
        {
            foreach ( var pattern in patterns )
            {
                var successStates = pattern.Steps;

                var builder = new StringBuilder ();
                builder.AppendLine ( $"   *** PATTERN REPORT " );
                builder.AppendLine ( $"   {pattern.PatternName} " );


                for ( int i = 0; i < successStates.Length; i++ )
                {
                    var step = successStates[i];
                    builder.AppendLine ( $"[{i}] State: {step.Status.ToString ()}" );
                }
                Debug.Log ( builder.ToString () );
                builder.AppendLine ( $" " );
            }
        }
        void EvaluatePreviousStepForSilentTick( Message rythmPositionMessage )
        {
            var newPosition = rythmPositionMessage.Open<RythmPosition> ();
            var newId = newPosition.Tick.Id;
            var index = newId > 0 ? newId - 1 : 63;
            patterns.ForEach ( p => p.SetCurrentStepIndex ( newId ) ); 

            foreach ( var pattern in patterns )
            {
                var successStates = pattern.StepStatuses;

                if ( successStates[index] != PatternStepStatus.Waiting )
                {
                    return;// This tick has already been checked. Exit.
                }
                var patternAction = pattern.Steps[index].Action;
                if ( patternAction != PatternStepAction.None )
                {
                    pattern.SetStatusOfStep ( index, PatternStepStatus.Failed );
                    //   Debug.Log ( $"Pattern '{pattern.PatternName}' has failed at tick {tick.Id} because *{patternAction.ToString ()}* was expected but tick was silent" );
                }
                else
                {
                    pattern.SetStatusOfStep ( index, PatternStepStatus.Sucess );
                }
            }
        }
        public void CheckHitTickForSuccess( Message rythmPositionOfHitTick )
        {
            var rythmPosition = rythmPositionOfHitTick.Open<RythmPosition> ();
            var tickWasHit = rythmPosition.Tick;

            foreach ( var pattern in patterns )
            {
                pattern.SetHitStepIndex ( tickWasHit.Id );

                if ( pattern.StepStatuses[tickWasHit.Id] != PatternStepStatus.Waiting && tickWasHit.Id != 0)
                {
                    // This tick has already been set. Exit.
                    return;
                }
                var patternAction = pattern.Steps[tickWasHit.Id].Action;
                var result = PatternStepStatus.Failed;
                switch ( patternAction )
                {
                    case PatternStepAction.None:
                   //     Debug.Log ( $"Pattern has failed at tick {tickWasHit.Id} because *{patternAction.ToString ()}* was expected but tick *HIT*" );
                        result = PatternStepStatus.Failed;
                        break;
                    case PatternStepAction.Hit:
                        result = PatternStepStatus.Sucess;
                 
                        break;
                    case PatternStepAction.Hold:
                        result = PatternStepStatus.Failed; // Not yet implemented
                        break;
                    default:
                        break;
                }
                pattern.SetStatusOfStep(tickWasHit.Id, result);
            }
        }
    }
}
