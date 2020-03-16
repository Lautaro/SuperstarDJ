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


        Tick[] ticks;
        Dictionary<Vector2, int> HitRanges = new Dictionary<Vector2, int> (); // KEY: HitRange (x = start, y = end)  VALUE: tickIndex
        static public bool DebugEnabled = false;
        internal readonly double trackDuration;
        internal double tickDuration;
        internal double paddingMultiplier;
        internal double paddingDuration;
        int paddingInPercentage { get {
                return GameSettings.Instance.HitRangePaddingInPercentage;
            } }

        RythmPosition currentPosition;
        public RythmPosition CurrentPosition
        {
            get
            {
                return currentPosition;
            }
        }

        public PositionTracker( int measuresPerLoop, int beatsPerMeasure, int ticksPerBeat, double _trackDuration )
        {
            var amountOfTicks = measuresPerLoop * beatsPerMeasure * ticksPerBeat;
            trackDuration = _trackDuration;
            tickDuration = _trackDuration / amountOfTicks;
             paddingMultiplier = ( double )paddingInPercentage / 100;
            paddingDuration = (tickDuration * paddingMultiplier);
            ticks = new Tick[amountOfTicks];

            var tickCounter = 0;

            for ( int mi = 0; mi < measuresPerLoop; mi++ )
            {
                for ( int bi = 0; bi < beatsPerMeasure; bi++ )
                {
                    for ( int ti = 0; ti < ticksPerBeat; ti++ )
                    {
                        var tickStartPosition = tickCounter * tickDuration;
                        var newTick = new Tick ( tickCounter, tickStartPosition, tickStartPosition + tickDuration, bi, mi, ti );

                        if ( tickCounter == 0 )
                        {
                            /*
                             The first tick has a special hit area divided into two ranges.  
                              1.First  starts with the padding at the  end of the track and ends with the end of the track
                              2. Second  starts at the beginning of the track and ends att the range of padding
                             Both ranges maps to tick index 0 (the first tick)
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
                            // all Ticks except first will only have one hit range
                            double hitAreaRangeStart, hitAreaRangeEnd;
                            hitAreaRangeStart = tickStartPosition - paddingDuration;
                            hitAreaRangeEnd = tickStartPosition + paddingDuration;
                            var hitAreaRange = new Vector2 ( ( float )hitAreaRangeStart, ( float )hitAreaRangeEnd );

                            HitRanges.Add ( hitAreaRange, tickCounter );
                        }
                        ticks[tickCounter] = newTick;
                        tickCounter++;
                    }
                }
            }
            CreateHitRangeTable ();




        }
        private void DebugLogTicks()
        {
            var builder = new StringBuilder ();
            foreach ( var tick in ticks )
            {
                builder.AppendLine ( $"{tick.ToString ()}" );
            }

            this.DebugLog ( builder.ToString () );
        }

  
        internal void CreateHitRangeTable()
        {
            var builder = new StringBuilder ();
            builder.AppendLine ( $"[HIT RANGES] [Table created: {DateTime.Now.ToShortDateString ()} -{DateTime.Now.ToShortTimeString ()}]" );
            builder.AppendLine ( $"[Padding percentage : {paddingInPercentage}% ] [Padding in duration  : {paddingInPercentage}% ] " );
            foreach ( var tick in ticks )
            {
                var hitRanges = HitRanges.Where ( kvp => kvp.Value == tick.Id ).Select ( kvp => kvp.Key );

                foreach ( var hitRange in hitRanges )
                {
                    builder.AppendLine ( $"Tick[{tick.Id.ToString("D2")}]  @({tick.TickStartsAt.ToString("N0")})  --- " +
                        $"Hit Range:  { hitRange.x.ToString ( "N0" )} - {hitRange.y.ToString ( "N0" )} " );
                }
            }

            this.DebugLog ( builder.ToString () );
        }
        internal RythmPosition CheckIfTickWasHit( double position )
        {
            var inputLagPadding = GameSettings.Instance.PatternDetectionInputLagPadding;
            var matchPosition = position - inputLagPadding;

            // Get index of hit tick. Vector X is hit range start and Y is hit range end
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
                    Debug.LogError ("There are more than one tick marked as hit. Something is wrong!");
                }
            }
            //      var isWithinHitRange = HitRanges.Where ( kvp => kvp.Key.x <= position && kvp.Key.y >= position );

            if ( positionWithinRange.Count () > 0 )
            {
                //     var hitRangeTickIndex = HitRanges.First ( kvp => kvp.Key.x <= position && kvp.Key.y >= position ).Value;
                var hitTick = ticks[positionWithinRange[0]];
                return new RythmPosition ( hitTick, position,matchPosition );
            }
            else
            {
                return new RythmPosition ( new Tick ( -1, -1, -1,-1, -1, -1 ), position ); // No hit
            }
        }
        internal void UpdateCurrentRythmPosition( double currentPositionInClip )
        {
            var tick = ticks.First ( t => t.TickStartsAt <= currentPositionInClip && t.TickEndsAt >= currentPositionInClip );

            if ( tick.Id == currentPosition.Tick.Id )
                return;

            currentPosition = new RythmPosition ( tick, currentPositionInClip );

            MessageHub.PublishNews<RythmPosition> ( MessageTopics.NewRythmPosition, currentPosition );
            //     Debug.Log ( $"[{currentPosition.Tick.Measure}][{currentPosition.Tick.Beat}][{currentPosition.Tick.Index}]" );

            //            if ( currentPosition.Tick.Id == ticks.Length && tick.Id == 0 )
            if ( tick.Id == 0 )
            {
                // new loop. Reset.
                MessageHub.PublishNews<string> ( MessageTopics.TrackStartsFromZero_string, "Track start from zero" );
            }

        }
    }
}
