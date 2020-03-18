using SuperstarDJ.Audio.PositionTracking;
using SuperstarDJ.MessageSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SuperstarDJ.Audio.PatternDetection
{
    public class PatternDetector
    {
        List<Pattern> patterns = new List<Pattern> ();
        //    Dictionary<string, SuccesState[]> successTrackers = new Dictionary<string, SuccesState[]> ();
   
        public PatternDetector( List<Pattern> patterns_ )
        {
            patterns = patterns_;

            //  Debug.Log ( $"Loaded {patterns.Count} patterns : {string.Join ( " * ", patterns.Select ( p => p.PatternName ).ToArray () ) }" );

            MessageHub.Subscribe ( MessageTopics.NewRythmPosition, EvaluatePreviousStepForSilentStep );
            MessageHub.Subscribe ( MessageTopics.StepHit_Step, CheckHitStepForSuccess );
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
        void EvaluatePreviousStepForSilentStep( Message rythmPositionMessage )
        {
            var newPosition = rythmPositionMessage.Open<RythmPosition> ();
            var newId = newPosition.Step.Id;
            var index = newId > 0 ? newId - 1 : 63;
            patterns.ForEach ( p => p.SetCurrentStepIndex ( newId ) );

            foreach ( var pattern in patterns )
            {
                var successStates = pattern.StepStatuses;

                if ( successStates[index] != PatternStepStatus.Waiting )
                {
                    return;// This step has already been checked. Exit.
                }
                var patternAction = pattern.Steps[index].Action;
                if ( patternAction != PatternStepAction.None )
                {
                    pattern.SetStatusOfStep ( index, PatternStepStatus.Failed );
                    //   Debug.Log ( $"Pattern '{pattern.PatternName}' has failed at step {step.Id} because *{patternAction.ToString ()}* was expected but step was silent" );
                }
                else
                {
                    pattern.SetStatusOfStep ( index, PatternStepStatus.Sucess );
                }
            }
        }
        public void CheckHitStepForSuccess( Message rythmPositionOfHitStep )
        {
            var rythmPosition = rythmPositionOfHitStep.Open<RythmPosition> ();
            var stepWasHit = rythmPosition.Step;

            foreach ( var pattern in patterns )
            {
                pattern.SetHitStepIndex ( stepWasHit.Id, rythmPosition );

                if ( pattern.StepStatuses[stepWasHit.Id] != PatternStepStatus.Waiting && stepWasHit.Id != 0 )
                {
                    // This step has already been set. Exit.
                    return;
                }
                var patternAction = pattern.Steps[stepWasHit.Id].Action;
                var result = PatternStepStatus.Failed;
                switch ( patternAction )
                {
                    case PatternStepAction.None:
                        //     Debug.Log ( $"Pattern has failed at step {stepWasHit.Id} because *{patternAction.ToString ()}* was expected but step *HIT*" );
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
                pattern.SetStatusOfStep ( stepWasHit.Id, result );
            }
        }
    }
}
