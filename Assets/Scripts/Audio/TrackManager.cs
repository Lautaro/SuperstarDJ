using SuperstarDJ.Audio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace SuperstarDJ.DynamicMusic
{

    /// <summary>
    /// Clas Nested in SongySong class
    /// </summary>
    class TrackManager
    {

        List<Track> Tracks;
        public bool IsPlaying
        {
            get
            {
                return Tracks.Any ( t => t.IsPlaying );
            }
        }
        double paddingTime = 0.01;
        public double Duration
        {
            get
            {
                return Tracks[0].Duration;
            }
        }
        internal double ScheduledToStartAt { get; private set; }
        //  internal double EndsPlayingDspTime { get; set; }
        public TrackManager( List<Track> tracks ) => Tracks = tracks; //CTOR
        void StartAllTracks( List<TrackNames> playsAtStart )
        {
            var dspTime = CurrentDspTime () + paddingTime;
            ScheduledToStartAt = dspTime;

            foreach ( var track in Tracks )
            {
                //EndsPlayingDspTime = ScheduledToStartAt + track.Duration;
                track.Source ().PlayScheduled ( dspTime );

                // Should start muted or enabled? 
                track.Source ().volume = playsAtStart.Contains ( track.TrackName ) ? 1f : 0f;
            }
        }

        void StopAllTracks()
        {
            foreach ( var track in Tracks )
            {
                track.Source ().Stop ();
            }
        }
        public TrackNames[] TracksPlaying()
        {
            return Tracks.Where ( t => t.Source ().isPlaying && t.Source ().volume > 0f ).Select ( t => t.TrackName ).ToArray ();

        }
        public bool IsTrackPlaying( string trackName )
        {
            var _trackName = UnityTools.TrackNameFromString ( trackName );
            return Tracks.Any ( t => t.TrackName == _trackName && t.IsPlaying );

        }
        public void Update()
        {
            //if (CurrentDspTime() >= EndsPlayingDspTime && IsPlaying == true)
            //{
            //    isPlaying = false;
            //    isScheduledToStart = false;
            //    print(TrackName + " STOPPED PLAYING  at " + CurrentDspTime());
            //}

            //if (CurrentDspTime() >= ScheduledToStartAt && isScheduledToStart == true && IsPlaying == false)
            //{
            //    print(TrackName + " STARTS PLAYING  at " + CurrentDspTime());
            //    isPlaying = true;
            //}
        }
        public Dictionary<TrackNames, float> GetPlayingTracks()
        {
            var dic = new Dictionary<TrackNames, float> ();
            foreach ( var track in Tracks )
            {
                if ( track.Source ().volume > 0.1 )
                {
                    var volume = track.Source ().volume;
                    dic.Add ( track.TrackName, volume );
                }
            }

            return dic;
        }
        private double CurrentDspTime()
        {
            return AudioSettings.dspTime;
        }
        void print( string log )
        {

        }
        public void MuteTrack( TrackNames name )
        {
            GetTrackByName ( name ).Source ().volume = 0;
            if ( GetPlayingTracks ().Count () <= 0 )
            {

            }
        }

        public void UnMuteTrack( TrackNames name )
        {
            if ( GetPlayingTracks ().Count () < 1 )
            {
                StartAllTracks ( new List<TrackNames> () { name } );
            }

            GetTrackByName ( name ).Source ().volume = 1f;
        }

        public double GetCurrentPosition()
        {

            var track = Tracks[0];
            Debug.Log ( track.Source ().timeSamples );


            return 0;
        }
        public Track GetTrackByName( TrackNames name )
        {
            return Tracks.First ( t => t.TrackName == name );
        }
    }
}