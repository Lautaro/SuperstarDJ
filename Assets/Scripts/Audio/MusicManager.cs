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
using SuperstarDJ.Mechanics;
using SuperstarDJ.Enums;
using UnityEngine.SceneManagement;
using MessageSystem;
using Assets.Scripts.Enums;

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

        public static void PlayTrack( TrackNames track )
        {
            instance.trackManager.PlayTrack ( track );
        }

        public static void StopTrack( TrackNames track )
        {
            instance.trackManager.StopTrack ( track );
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

        SongManager trackManager;
        public string PathToAudio;
        public string SettingsFile;
        public DOTweenAnimation BeatMark;
        RythmPositionTracker rythmPositionTracker;
        RythmPosition rythmPosition;

        static public RythmPosition RythmPosition { get {
                return instance.rythmPosition;
            }}
        public bool MuteAudio;
        // Start is called before the first frame update
        void Awake()
        {
            if ( instance == null )
            {
                instance = this;
                LoadTracksAndSpawnRecords ();
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
            //rythmPosition = rythmPositionTracker.GetPositionInRythm ();
        }

        void Beat()
        {
            //    BeatMark.DORewindAndPlayNext ();
            if ( rythmPosition.Measure != null && rythmPosition.IsInHitArea())
            {
                Debug.Log ( rythmPosition.ToString ()  );
                MessageHub.PublishNews<string> ( MessageTopics.DisplayUI_FX_string, UI_FXs.FX_Star );
            }
        }
        private void LoadTracksAndSpawnRecords()
        {
            var tracks = TrackAndRecordLoading.Load ( PathToAudio, SettingsFile, () => gameObject.AddComponent<Track> () );
            var records = TrackAndRecordLoading.GetRecordPrefabs ( tracks, GameObject.Find ( "Dynamic Records" ).transform );
            trackManager = new SongManager ( tracks );
        }

        void Update()
        {
            if ( trackManager.GetPlayingTracks ().Count > 0 )
            {
                rythmPosition = rythmPositionTracker.UpdateCurrentRythmPosition ( rythmPosition, trackManager.GetCurrentSamplePositionOfSong());
                AudioListener.volume = MuteAudio == true ? 0f : 1f;
            }
        }

        #endregion
    }
}