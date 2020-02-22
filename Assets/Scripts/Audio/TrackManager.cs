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
        public string TrackName { get; set; }
        List<DynamicTrack> Tracks;
        public bool IsPlaying { get; }
        double paddingTime = 0.01;
        bool isScheduledToStart;
        internal double ScheduledToStartAt { get; private set; }
        internal double EndsPlayingDspTime { get; set; }
        public TrackManager( List<DynamicTrack> tracks ) => Tracks = tracks; //CTOR
        void StartAllTracks( List<TrackNames> startsWithVolume )
        {
            var dspTime = CurrentDspTime () + paddingTime;
            ScheduledToStartAt = dspTime; ;
            isScheduledToStart = true;

            foreach ( var track in Tracks )
            {
                EndsPlayingDspTime = ScheduledToStartAt + track.Duration;
                track.Source.PlayScheduled ( dspTime );

                // Should start muted or enabled? 
                track.Source.volume = startsWithVolume.Contains ( track.TrackName ) ? 1f : 0f;
            }
        }
        public TrackNames[] TracksPlaying()
        {
            return Tracks.Where ( t => t.Source.isPlaying && t.Source.volume > 0f ).Select ( t => t.TrackName ).ToArray ();

        }
        public bool IsTrackPlaying( string trackName )
        {
            return Tracks.Any ( t => t.ClipName == trackName && t.Source.isPlaying && t.Source.volume > 0f );

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
        public Dictionary<string, float> GetPlayingTracks()
        {

            var dic = new Dictionary<string, float> ();
            foreach ( var track in Tracks )
            {
                if ( track.Source.volume > 0.1 )
                {
                    var volume = track.Source.volume;
                    dic.Add ( track.ClipName, volume );
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
            GetTrackByName ( name ).Source.volume = 0;
        }

        public void UnMuteTrack( TrackNames name )
        {
            GetTrackByName ( name ).Source.volume = 1f;
        }

        public DynamicTrack GetTrackByName( TrackNames name )
        {
            return Tracks.First ( t => t.TrackName == name );
        }
    }
}