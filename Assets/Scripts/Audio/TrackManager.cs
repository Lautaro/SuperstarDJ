﻿using SuperstarDJ.MessageSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace SuperstarDJ.Audio.DynamicTracks
{
    class TrackManager
    {

        List<Track> Tracks;

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
        public TrackManager(List<Track> tracks) { 
            Tracks = tracks;
         //   MessageHub.Subscribe(MessageTopics.StartLoop, ResetSong);
        }
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

            MessageHub.PublishNews ( MessageTopics.SongStarted_string, "" );
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
        }
        void StopSong()
        {
            foreach ( var track in Tracks )
            {
                track.Source ().Stop ();
            }
        }

        void ResetSong(Message message)
        {
            Debug.Log("HOLLAAAAA!!!!");
            foreach (var track in TrackNamesPlaying())
            {
                StartTrack(track);
            }
        }
        public List<Track> TrackNamesPlaying()
        {
            return Tracks.Where(t => t.Source().isPlaying && t.Source().volume > 0f).ToList();

        }
        public int GetCurrentSamplePosition()
        {
            var referenceTrack = Tracks.FirstOrDefault ( t => t.IsPlaying == true );

            if ( referenceTrack == null ) { return 0; }
            return referenceTrack.Source ().timeSamples;
        }

        public bool IsTrackPlaying( string trackName )
        {
            return Tracks.Any ( t => t.TrackName == trackName && t.IsPlaying );
        }
        //public void DebugSound( string trackName, int playAtSample = 0, float volume = 1f )
        //{
        //    var track = GetTrackByName ( trackName );
        //    track.Source ().timeSamples = 100000;
        //    track.Source ().volume = track.VolumeModification;
        //    track.Source ().PlayScheduled ( GetDspTime () + paddingTime );

        //}
        public List<Track> GetPlayingTracks()
        {
            return Tracks.Where ( t => t.IsPlaying == true ).ToList ();
        }

        public bool IsAnyTrackPlaying()
        {
            return Tracks.Any ( t => t.IsPlaying == true );
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
            else
            {
                var track = GetTrackByName ( trackName );
                StartTrack ( track, GetCurrentSamplePosition () );
            }


        }
        public Track GetTrackByName( string trackName )
        {
            return Tracks.First ( t => t.TrackName == trackName );
        }
    }
}