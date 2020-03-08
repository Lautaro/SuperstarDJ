using SuperstarDJ.DynamicMusic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperstarDJ.UnityTools.Extensions;
using System;
using SuperstarDJ.Audio.InitialiseAudio;
using DG.Tweening;
using SuperstarDJ.Audio.RythmDetection;
using SuperstarDJ.Mechanics;
using SuperstarDJ.Enums;
using UnityEngine.SceneManagement;
using MessageSystem;
using Assets.Scripts.Enums;
using System.Text;

namespace SuperstarDJ.Audio
{
    public class RythmManager : MonoBehaviour
    {

        #region Rythm Settings
        const int MEASURES_PER_LOOP = 4;
        const int BEATS_PER_MEASURE = 4;
        const int TICKS_PER_BEATS = 4;
        #endregion

        #region Static Methods
        public static RythmManager instance;

        public static void PlayTrack( string track )
        {
            instance.trackManager.PlayTrack ( track );
        }

        public static void StopTrack( string track )
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

        public static string[] TracksPlaying()
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
                Debug.LogError ( "There can only be one RythmManager. A second one has been instantiated! " );
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
            var hitTick = rythmPositionTracker.CheckIfHit ( trackManager.GetCurrentSamplePosition () );

            if ( hitTick.Position >= 0 )
            {
                Debug.Log ($"(!!! {hitTick.Tick.Id})Was hit:  {hitTick.Position}  " );
                MessageHub.PublishNews<string> ( MessageTopics.DisplayUI_FX_string, UI_FXs.FX_Star );
            }
            else
            {
                Debug.LogWarning ( $"({hitTick.Tick.Id})FAIL! Position:  {hitTick.Position}" );
            }
        }
        private void LoadTracksAndSpawnRecords()
        {
            var tracks = AudioLoading.Load ( PathToAudio, SettingsFile, () => gameObject.AddComponent<Track> () );
            var records = AudioLoading.GetRecordPrefabs ( tracks, GameObject.Find ( "Dynamic Records" ).transform );
            trackManager = new SongManager ( tracks );
        }

        void Update()
        {
            if ( trackManager.GetPlayingTracks ().Count > 0 )
            {
                rythmPosition = rythmPositionTracker.UpdateCurrentRythmPosition ( rythmPosition, trackManager.GetCurrentSamplePosition());
                AudioListener.volume = MuteAudio == true ? 0f : 1f;
            }
        }

        #endregion
    }
}