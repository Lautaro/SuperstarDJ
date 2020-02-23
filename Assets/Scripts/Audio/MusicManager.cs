using SuperstarDJ.DynamicMusic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperstarDJ.UnityTools.Extensions;
using SuperstarDJ.Audio.Enums;
using System;
using SuperstarDJ.Audio.InitialiseAudio;
using DG.Tweening;
using SuperstarDJ.Audio.RythmDetection;

namespace SuperstarDJ.Audio
{
    public class MusicManager : MonoBehaviour
    {

        #region Rythm Settings
        const int MEASURES_PER_LOOP = 4;
        const int BEATS_PER_MEASURE = 4;
        const int TICKS_PER_BEATS = 4;

        #endregion


        #region Static Methods
        static MusicManager instance;

        public static void UnMuteTrack( TrackNames track )
        {
            instance.trackManager.UnMuteTrack ( track );
        }

        public static void MuteTrack( TrackNames track )
        {
            instance.trackManager.MuteTrack ( track );
        }
        public static bool IsTrackPlaying( string trackName )
        {
            return instance.trackManager.IsTrackPlaying ( trackName );
        }
        public static void BeatNow()
        {
            instance.Beat ();
        }

        public static TrackNames[] TracksPlaying()
        {
            return instance.trackManager.TracksPlaying ();
        }
        #endregion

        #region Instance

        TrackManager trackManager;
        public string PathToAudio;
        public string SettingsFile;
        public DOTweenAnimation BeatMark;
        RythmAnalyzer rythmAnalyzer;
        // Start is called before the first frame update
        void Start()
        {
            if ( instance == null )
            {
                instance = this;
                LoadTracks ();
                InitializeRythmAnalyzer ();
            }
            else
            {
                Debug.LogError ( "There can only be one MusicManager. A second one has been instantiated! " );
            }
        }

        void InitializeRythmAnalyzer()
        {
            var trackDuration = trackManager.Duration / MEASURES_PER_LOOP;
            rythmAnalyzer = new RythmAnalyzer ( MEASURES_PER_LOOP, BEATS_PER_MEASURE, TICKS_PER_BEATS, trackDuration );
        }

        void Beat()
        {
            BeatMark.DORewindAndPlayNext ();

        }
        private void LoadTracks()
        {
            var tracks = InitialiseAudio.LoadTracks.Load ( PathToAudio, SettingsFile, () => gameObject.AddComponent<Track> () );
            trackManager = new TrackManager ( tracks );
        }

        void Update()
        {
            if ( trackManager.GetPlayingTracks ().Count > 0 )
            {
                int currentMeasure;
                int currentBeat;
                int currentTick;
                trackManager.GetCurrentPosition ();
                    }
        }
        #endregion
    }
}