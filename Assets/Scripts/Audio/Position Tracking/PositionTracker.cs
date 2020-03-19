using SuperstarDJ.MessageSystem;
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

        internal List<double> AllMissedHits = new List<double> ();
        public bool WasHitButMissedThisFrame;
        Step[] steps;
        Dictionary<Vector2, int> HitRanges = new Dictionary<Vector2, int> (); // KEY: HitRange (x = start, y = end)  VALUE: stepIndex
        static public bool DebugEnabled = false;
        internal readonly double trackDuration;
        internal double stepDuration;
        internal double paddingMultiplier;
        internal double paddingDuration;
        int paddingInPercentage { get {
                if ( RythmManager.Settings != null )
                {
                    return RythmManager.Settings.HitRangePaddingInPercentage;
                }
                return 0;
            } }
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
            paddingDuration = (stepDuration * paddingMultiplier);
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
                    builder.AppendLine ( $"Step[{step.Id.ToString("D2")}]  @({step.StepStartsAt.ToString("N0")})  --- " +
                        $"Hit Range:  { hitRange.x.ToString ( "N0" )} - {hitRange.y.ToString ( "N0" )} " );
                }
            }

            this.DebugLog ( builder.ToString () );
        }
        internal DjAct CheckIfStepWasHit( double position )
        {
            var newDjaAct = new DjAct ( position );
            var inputLagPadding = RythmManager.Settings.PatternDetectionInputLagPadding;
            var matchPosition = position - inputLagPadding;

            // Get index of hit step. Vector X is hit range start and Y is hit range end
            List<int> positionWithinRange = new List<int> ();
            foreach ( var hitrange in HitRanges )
            {
                var start = hitrange.Key.x;
                var end= hitrange.Key.y;
                if ( start <= matchPosition & end >= matchPosition )
                {
                    positionWithinRange.Add (hitrange.Value);
                }

                // Sanity check!
                if ( positionWithinRange.Distinct ().Count () > 1 )
                {
                    Debug.LogError ("There are more than one step marked as hit. Something is wrong!");
                }
            }
            //      var isWithinHitRange = HitRanges.Where ( kvp => kvp.Key.x <= position && kvp.Key.y >= position );

            if ( positionWithinRange.Count () > 0 )
            {
                //     var hitRangeStepIndex = HitRanges.First ( kvp => kvp.Key.x <= position && kvp.Key.y >= position ).Value;
                var hitStep = steps[positionWithinRange[0]];
                return new RythmPosition ( hitStep, position,matchPosition,false  );
            }
            else
            {
                AllMissedHits.Add ( position );
                WasHitButMissedThisFrame = true;
                return new RythmPosition ( new Step ( -1, -1, -1,-1, -1, -1 ), position,true ); // No hit
            }
        }
        internal void UpdateCurrentRythmPosition( double currentPositionInClip )
        {
  
            var step = steps.First ( t => t.StepStartsAt <= currentPositionInClip && t.StepEndsAt >= currentPositionInClip );
            var positionWasHit = AllMissedHits.Contains ( currentPositionInClip );
            currentPosition = new RythmPosition ( step, currentPositionInClip, positionWasHit );

            MessageHub.PublishNews<RythmPosition> ( MessageTopics.NewRythmPosition, currentPosition );
            //     Debug.Log ( $"[{currentPosition.Step.Measure}][{currentPosition.Step.Beat}][{currentPosition.Step.Index}]" );

            //            if ( currentPosition.Step.Id == steps.Length && step.Id == 0 )
            if ( step.Id == 0 )
            {
                // new loop. Reset.
                MessageHub.PublishNews<string> ( MessageTopics.TrackStartsFromZero_string, "Track start from zero" );
                AllMissedHits.Clear ();
            }
            WasHitButMissedThisFrame = false;
        }
    }
}
