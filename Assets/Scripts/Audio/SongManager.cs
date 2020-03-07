using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MessageSystem;
using SuperstarDJ.UnityTools.Extensions;


namespace SuperstarDJ.DynamicMusic
{
    class SongManager
    {

        List<Track> Tracks;
        public bool IsPlaying
        {
            get
            {
                return Tracks.Any ( t => t.IsPlaying );
            }
        }
        static double paddingTime = 0.01;
        public double Duration
        {
            get
            {
                return Tracks[0].Duration;
            }
        }
        internal double ScheduledToStartAt { get; private set; }
        //  internal double EndsPlayingDspTime { get; set; }
        public SongManager( List<Track> tracks ) => Tracks = tracks; //CTOR
        void StartSong( List<string> playsAtStart )
        {
            var dspTime = GetDspTime () + paddingTime;
            ScheduledToStartAt = dspTime;
            
            foreach ( var track in Tracks )
            {
                track.Source ().Stop ();

                if ( playsAtStart.Contains ( track.TrackName ) )
                {
                    StartTrack ( track );
                }
            }
        }
        private static double GetDspTime( bool withPadding = false )
        {
            var padding = withPadding == true ? paddingTime : 0;
            return AudioSettings.dspTime + padding;
        }
        private static void StartTrack( Track track, int startFromSamplePosition = 0 )
        {
            track.Source ().timeSamples = startFromSamplePosition;
            track.Source ().volume = track.VolumeModification;
            track.Source ().PlayScheduled ( GetDspTime ( true ) );

            MessageHub.PublishNews<Track> ( MessageTopics.TrackStarted_Track, track );
            
        }

        void StopSong()
        {
            foreach ( var track in Tracks )
            {
                track.Source ().Stop ();
            }
        }
        public string[] TracksPlaying()
        {
            return Tracks.Where ( t => t.Source ().isPlaying && t.Source ().volume > 0f ).Select ( t => t.TrackName ).ToArray ();

        }

        public int GetCurrentSamplePositionOfSong()
        {
            var referenceTrack = Tracks.First ( t => t.IsPlaying == true );

            if ( referenceTrack == null ) { return 0; }
            return referenceTrack.Source ().timeSamples;
        }
        //public double GetCurrentSamplePositionOfSong()
        //{
        //    var referenceTrack = Tracks.First ( t => t.IsPlaying == true );

        //    if ( referenceTrack == null ) { return 0; }

        //    var timeSamples = ( double )referenceTrack.Source ().timeSamples;
        //    var frequency = referenceTrack.Source ().clip.frequency;
        //    var currentPosition = timeSamples / frequency;

        //    return currentPosition;
        //}
        public bool IsTrackPlaying( string trackName )
        {
            return Tracks.Any ( t => t.TrackName == trackName && t.IsPlaying );
        }
        public void DebugSound( string trackName, int playAtSample = 0, float volume = 1f )
        {
            var track = GetTrackByName ( trackName );
            track.Source ().timeSamples = 100000;
            track.Source ().volume = track.VolumeModification;
            track.Source ().PlayScheduled ( GetDspTime () + paddingTime );

        }
        public List<Track> GetPlayingTracks()
        {
            return Tracks.Where ( t => t.IsPlaying == true ).ToList ();
        }

        public void StopTrack( string trackName )
        {
            var track = GetTrackByName ( trackName );
            track.Source ().Stop ();
            MessageHub.PublishNews<Track> ( MessageTopics.TrackStopped_Track, track );
        }
        public void PlayTrack( string trackName )
        {
            if ( GetPlayingTracks ().Count () < 1 )
            {
                StartSong ( new List<string> () { trackName } );
            }

            var track = GetTrackByName ( trackName );
            StartTrack ( track, GetCurrentSamplePositionOfSong () );
        }
        public Track GetTrackByName( string trackName )
        {
            return Tracks.First ( t => t.TrackName == trackName );
        }
    }
}