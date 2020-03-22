﻿using SuperstarDJ.MessageSystem;
using SuperstarDJ.UnityTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SuperstarDJ.Audio.PositionTracking
{
    class PositionTracker
    {
        Step[] steps;
        Dictionary<Vector2, int> HitRanges = new Dictionary<Vector2, int> (); // KEY: HitRange (x = start, y = end)  VALUE: stepIndex
        static public bool DebugEnabled = false;
        internal readonly double trackDuration;
        internal double stepDuration;
        internal double paddingMultiplier;
        internal double paddingDuration;
        bool thisLoopHasReset = true;
        int indexOfLastStepEvaluated;
        int paddingInPercentage
        {
            get
            {
                if ( RythmManager.Settings != null )
                {
                    return RythmManager.Settings.HitRangePaddingInPercentage;
                }
                return 0;
            }
        }
        RythmPosition currentPosition;
        public RythmPosition CurrentPosition
        {
            get
            {
                return currentPosition;
            }
        }
        public float PositionInPercentage()
        {
            return ( float )( CurrentPosition.RawPosition / trackDuration ) * 100;
        }
        public PositionTracker( int measuresPerLoop, int beatsPerMeasure, int stepsPerBeat, double _trackDuration )
        {
            var amountOfSteps = measuresPerLoop * beatsPerMeasure * stepsPerBeat;
            trackDuration = _trackDuration;
            stepDuration = _trackDuration / amountOfSteps;
            paddingMultiplier = ( double )paddingInPercentage / 100;
            paddingDuration = ( stepDuration * paddingMultiplier );
            steps = new Step[amountOfSteps];

            var stepCounter = 0;

            for ( int mi = 0; mi < measuresPerLoop; mi++ )
            {
                for ( int bi = 0; bi < beatsPerMeasure; bi++ )
                {
                    for ( int ti = 0; ti < stepsPerBeat; ti++ )
                    {
                        var stepStartPosition = stepCounter * stepDuration;
                        var newStep = new Step ( stepCounter, stepStartPosition, stepStartPosition + stepDuration, bi, mi, ti );

                        if ( stepCounter == 0 )
                        {
                            /*
                             The first step has a special hit area divided into two ranges.  
                              1.First  starts with the padding at the  end of the track and ends with the end of the track
                              2. Second  starts at the beginning of the track and ends att the range of padding
                             Both ranges maps to step index 0 (the first step)
                             */

                            double preRangeStart, preRangeEnd, postRangeStart, postRangeEnd;
                            preRangeStart = trackDuration - paddingDuration;
                            preRangeEnd = trackDuration;
                            postRangeStart = 0;
                            postRangeEnd = paddingDuration;

                            var preRange = new Vector2 ( ( float )preRangeStart, ( float )preRangeEnd );
                            var postRange = new Vector2 ( ( float )postRangeStart, ( float )postRangeEnd );

                            HitRanges.Add ( preRange, 0 );
                            HitRanges.Add ( postRange, 0 );

                        }
                        else
                        {
                            // all Steps except first will only have one hit range
                            double hitAreaRangeStart, hitAreaRangeEnd;
                            hitAreaRangeStart = stepStartPosition - paddingDuration;
                            hitAreaRangeEnd = stepStartPosition + paddingDuration;
                            var hitAreaRange = new Vector2 ( ( float )hitAreaRangeStart, ( float )hitAreaRangeEnd );

                            HitRanges.Add ( hitAreaRange, stepCounter );
                        }
                        steps[stepCounter] = newStep;
                        stepCounter++;
                    }
                }
            }
            CreateHitRangeTable ();




        }

        internal void CreateHitRangeTable()
        {
            var builder = new StringBuilder ();
            builder.AppendLine ( $"[HIT RANGES] [Table created: {DateTime.Now.ToShortDateString ()} -{DateTime.Now.ToShortTimeString ()}]" );
            builder.AppendLine ( $"[Padding percentage : {paddingInPercentage}% ] [Padding in duration  : {paddingInPercentage}% ] " );
            foreach ( var step in steps )
            {
                var hitRanges = HitRanges.Where ( kvp => kvp.Value == step.Id ).Select ( kvp => kvp.Key );

                foreach ( var hitRange in hitRanges )
                {
                    builder.AppendLine ( $"Step[{step.Id.ToString ( "D2" )}]  @({step.StepStartsAt.ToString ( "N0" )})  --- " +
                        $"Hit Range:  { hitRange.x.ToString ( "N0" )} - {hitRange.y.ToString ( "N0" )} " );
                }
            }

            this.DebugLog ( builder.ToString () );
        }
        internal DjAct CheckDjActResult( double position )
        {
            var inputLagPadding = RythmManager.Settings.PatternDetectionInputLagPadding;
            var matchPosition = position + inputLagPadding;

            int stepThatWashit = -1;

            foreach ( var hitrange in HitRanges )
            {
                var start = hitrange.Key.x;
                var end = hitrange.Key.y;
                if ( start <= matchPosition & end >= matchPosition )
                {
                    stepThatWashit = hitrange.Value;
                }

                if ( stepThatWashit >= 0 )
                {
                    return new DjAct ()
                    {
                        ActualPosition = new RythmPosition ( currentPosition.Step, position, matchPosition ),
                        StepThatWastHit = steps[stepThatWashit]
                    };
                }
            }
            return new DjAct ()
            {
                ActualPosition = new RythmPosition ( currentPosition.Step, position, matchPosition ),
                StepThatWastHit = null
            };
        }

        internal void UpdateCurrentRythmPosition( double currentPositionInClip )
        {
            if ( currentPositionInClip == 0 )
            {
                Debug.Log ("CLIP STARTED");
            }
            var currentStep = steps.First ( t => t.StepStartsAt <= currentPositionInClip && t.StepEndsAt >= currentPositionInClip );
            currentPosition = new RythmPosition ( currentStep, currentPositionInClip );

            if ( currentStep.Id == 0 && thisLoopHasReset )
            {
                thisLoopHasReset = false;
            }

            MessageHub.PublishNews<RythmPosition> ( MessageTopics.NewRythmPosition, currentPosition );

            var inAHitRange = HitRanges.Where ( hr => hr.Key.x <= currentPositionInClip && hr.Key.y >= currentPositionInClip ).ToArray();
      
            if ( inAHitRange.Count () == 0 || inAHitRange.ToArray ()[0].Value != indexOfLastStepEvaluated )
            {
                if ( indexOfLastStepEvaluated != currentStep.Id )
                {
                    MessageHub.PublishNews<Step> ( MessageTopics.HitRangePassed_Step, currentStep );
                    indexOfLastStepEvaluated = currentStep.Id;
                }
            }

            if ( currentStep.Id == steps.Length - 1  && thisLoopHasReset == false)
            {
                // new loop. Reset.
                MessageHub.PublishNews<string> ( MessageTopics.ResetLoop, "Reset loop" );
                indexOfLastStepEvaluated = steps.Length - 1;
                thisLoopHasReset = true;
            }
        }
    }
}
