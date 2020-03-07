using MessageSystem;
using SuperstarDJ.Audio.RythmDetection;
using SuperstarDJ.UnityTools.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.Audio.RythmDetection
{
    class RythmPositionTracker
    {
        //Measure[] measures;
        //Beat[] beats;
        Tick[] ticks;
        Dictionary<Vector2, int> HitRanges = new Dictionary<Vector2, int> (); // KEY: HitRange (x = start, y = end)  VALUE: tickIndex
                                                                              //    public const int beatTrackingPadding = 2000;
        static public bool DebugEnabled = false;
        readonly double trackDuration;
        readonly int paddingInPercentage = 15; //15%

        public RythmPositionTracker( int measuresPerLoop, int beatsPerMeasure, int ticksPerBeat, double _trackDuration )
        {
            var amountOfTicks = measuresPerLoop * beatsPerMeasure * ticksPerBeat;
            trackDuration = _trackDuration;
            var tickDuration = _trackDuration / amountOfTicks;
            double multiplier =(double) paddingInPercentage / 100 ;
            var paddingDuration = tickDuration * multiplier;
            ticks = new Tick[amountOfTicks];

            var tickCounter = 0;

            for ( int mi = 0; mi < measuresPerLoop; mi++ )
            {
                for ( int bi = 0; bi < beatsPerMeasure; bi++ )
                {
                    for ( int ti = 0; ti < ticksPerBeat; ti++ )
                    {

                        var tickStartPosition = tickCounter * tickDuration;
                        var newTick = new Tick ( tickCounter, tickStartPosition, bi, mi, ti );

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

            //measures = new Measure[measuresPerLoop];

            //for ( int i = 0; i < measuresPerLoop; i++ )
            //{
            //    measures[i] = new Measure ( i + 1, beatsPerMeasure, ticksPerBeat, measureDuration );
            //}
            //beats = measures.SelectMany ( m => m.beats ).ToArray ();
            //ticks = beats.SelectMany ( b => b.ticks ).ToArray ();
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


        //public void DebugMeasures()
        //{
        //    foreach ( var measure in measures )
        //    {
        //        Debug.Log ( $"{measure.index}M" );
        //        measure.DebugBeats ();
        //        Debug.Log ( "" );
        //    }
        //}
        //internal RythmPosition GetPositionInRythm( double currentPositionInClip )
        //{
        //    var paddedPosition = currentPositionInClip + beatTrackingPadding;
        //    if ( paddedPosition > trackDuration )
        //    {
        //        return new RythmPosition ( ticks[0], beats[0], measures[0], 0 );
        //    }

        //    var tick = ticks.FirstOrDefault ( t => t.IsWithinHitRange ( paddedPosition ) );
        //    if ( tick.parentBeat == null ) { Debug.LogWarning ( "RythmPosition update found no matching tick at position :" + currentPositionInClip ); };

        //    return new RythmPosition ( tick, tick.parentBeat, tick.parentBeat.parentMeasure, currentPositionInClip );
        //}

        internal RythmPosition GetPositionInRythm( double position )
        {
            // Get index of hit tick. Vector X is hit range start and Y is hit range end
            var isWithinHitRange = HitRanges.Any ( kvp => kvp.Key.x <= position && kvp.Key.y >= position );

            var currentTick = ticks.First ( t => t.TickStartsAt <= position );
            if ( isWithinHitRange )
            {
                var hitRangeTickIndex = HitRanges.First ( kvp => kvp.Key.x <= position && kvp.Key.y >= position ).Value;

                if ( hitRangeTickIndex != currentTick.Id )
                {
                    throw new Exception ( "Indexes of current and hitRange tick should be the same. Something is not right!" );
                }
            }


            //    if ( tick.parentBeat == null ) { Debug.LogWarning ( "RythmPosition update found no matching tick at position :" + position ); };

            return new RythmPosition ( currentTick, position, isWithinHitRange );
        }

        internal RythmPosition UpdateCurrentRythmPosition( RythmPosition rythmPosition, int currentPositionInClip )
        {
            var previousRp = rythmPosition;
            rythmPosition = GetPositionInRythm ( currentPositionInClip );

            if ( previousRp.Tick.Id != rythmPosition.Tick.Id )
            {
                MessageHub.PublishNews<RythmPosition> ( MessageTopics.NewRythmPosition, rythmPosition );
            }

            return rythmPosition;
        }
    }
}
