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

            MessageHub.Subscribe ( MessageTopics.HitRangePassed_Step, OnHitRangePassed );
            MessageHub.Subscribe ( MessageTopics.DjActHit_DjAct, CheckDjAct );
            //       MessageHub.Subscribe ( MessageTopics.ResetLoop, OnResetLoop );
            MessageHub.Subscribe ( MessageTopics.ResetPatternStatuses, EvaluatePatternSuccessAtEndOfLoop );
            MessageHub.Subscribe ( MessageTopics.SongStarted_string, OnSongStarted );
        }
        void ResetPatterns()
        {
            foreach ( var pattern in patterns )
            {
                pattern.Reset ();
            }
        }

        void OnEndOfLoop()
        {
            if ( patterns.Any ( p => p.StepStatuses.Any ( ss => ss == PatternStepStatus.Waiting ) ) )
            {
                var ids = new List<int> ();
                foreach ( var pattern in patterns )
                {
                    for ( int i = 0; i < pattern.StepStatuses.Length; i++ )
                    {
                        var ss = pattern.StepStatuses[i];
                        if ( ss == PatternStepStatus.Waiting )
                        {
                            ids.Add ( i );
                        }
                    }
                    Debug.LogError ( $"Pattern {pattern.PatternName} has StepStatus still set to Waiting at end of loop. Steps = [{string.Join ( ",", ids )}]" );
                    ids.Clear ();
                }
            }

            ResetPatterns ();
        }

        void OnSongStarted( Message message )
        {
            ResetPatterns ();
            EvaluateSuccessOfPassedStep ( new Step () ); // Make sure first step is evaluated as well
        }

        private void EvaluatePatternSuccessAtEndOfLoop( Message lastHitRangeOfLoopPassed )
        {
            foreach ( var pattern in patterns )
            {
                var successStates = pattern.Steps;

                var builder = new StringBuilder ();

                var wasSuccess = pattern.StepStatuses.Any ( ss => ss != PatternStepStatus.Sucess ) == true ? "Failed" : "SUCCESS!";
                builder.AppendLine ( $"   *** PATTERN REPORT ({wasSuccess}) " );
                builder.AppendLine ( $"   {pattern.PatternName} " );


                for ( int i = 0; i < successStates.Length; i++ )
                {
                    var step = successStates[i];
                    if ( step.Status != PatternStepStatus.Sucess )
                    {
                        builder.AppendLine ( $"[{i}] State: {step.Status.ToString ()}" );
                    }
                }
                Debug.Log ( builder.ToString () );
                builder.AppendLine ( $" " );
            }

            OnEndOfLoop ();
        }

        void OnHitRangePassed( Message stepMessage )
        {
            var newStep = stepMessage.Open<Step> ();
            EvaluateSuccessOfPassedStep ( newStep );
        }

        void EvaluateSuccessOfPassedStep( Step newStep )
        {
            var newId = newStep.Id;
            var index = newId;
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
        public void CheckDjAct( Message djActMessage )
        {
            var djAct = djActMessage.Open<DjAct> ();
            var rythmPosition = djAct.ActualPosition;
            var stepThatWasHit = djAct.StepThatWastHit.Value;

            foreach ( var pattern in patterns )
            {
                pattern.SetHitStepIndex ( stepThatWasHit.Id, rythmPosition );

                if ( pattern.StepStatuses[stepThatWasHit.Id] != PatternStepStatus.Waiting && stepThatWasHit.Id != 0 )
                {
                    Debug.LogWarning ( "WARNING! This step has already been set." );
                    // This step has already been set. Exit.
                    return;
                }
                var patternAction = pattern.Steps[stepThatWasHit.Id].Action;
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
                pattern.SetStatusOfStep ( stepThatWasHit.Id, result );
            }
        }
    }
}
