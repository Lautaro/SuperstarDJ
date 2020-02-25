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
        RythmPositionTracker rythmPositionTracker;
        RythmPosition rythmPosition;
        public bool MuteAudio;
        // Start is called before the first frame update
        void Start()
        {
            if ( instance == null )
            {
                instance = this;
                LoadTracks ();
                InitializeRythmPositionTracker ();
            }
            else
            {
                Debug.LogError ( "There can only be one MusicManager. A second one has been instantiated! " );
            }
        }

        void InitializeRythmPositionTracker()
        {
            var trackDuration = trackManager.Duration;
            rythmPositionTracker = new RythmPositionTracker ( MEASURES_PER_LOOP, BEATS_PER_MEASURE, TICKS_PER_BEATS, trackDuration );
        }

        void Beat()
        {
        //    BeatMark.DORewindAndPlayNext ();
          
            Debug.Log (rythmPosition.ToString() + $"  ({rythmPosition.Position})");
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
                UpdateRythmPosition ();
                AudioListener.volume = MuteAudio == true ? 0f : 1f;
                VisualMetronome.rythmPosition = rythmPosition;
            }
        }

        void UpdateRythmPosition()
        {
            var currentPositionInClip = trackManager.GetCurrentPosition ();
            rythmPosition = rythmPositionTracker.GetPositionInRythm ( currentPositionInClip );
        }
        #endregion
    }
}