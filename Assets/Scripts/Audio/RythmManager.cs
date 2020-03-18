using Assets.Scripts.Enums;
using DG.Tweening;
using Sirenix.OdinInspector;
using SuperstarDJ.Audio.DynamicTracks;
using SuperstarDJ.Audio.InitialiseAudio;
using SuperstarDJ.Audio.PatternDetection;
using SuperstarDJ.Audio.PositionTracking;
using SuperstarDJ.MessageSystem;
using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;

namespace SuperstarDJ.Audio
{
    public class RythmManager : MonoBehaviour
    {



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
        public static bool IsAllTracksStopped()
        {
            return !instance.trackManager.IsAnyTrackPlaying ();
        }
        public static void BeatNow()
        {
            instance.CheckForBeatHit ();

        }
        public static string[] TracksPlaying()
        {
            return instance.trackManager.TracksPlaying ();
        }

        #endregion

        #region Instance
        TrackManager trackManager;
        public string PathToAudio;
        public string PathToPatterns;
        public string SettingsFile;
        public DOTweenAnimation BeatMark;
        PositionTracker rythmPositionTracker;
        PatternDetector patternDetector;

        public static List<double> GetAllMissedHits()
        {
            var allMissed = instance?.rythmPositionTracker?.AllMissedHits;
            if ( allMissed == null )
            {
                allMissed = new List<double> ();
            }
            return allMissed;
        }

        static GameSettings gameSettings;
        public static GameSettings Settings
        {
            get
            {
                if ( gameSettings == null )
                {
                    gameSettings = GameSettings.instance;
                }
                return gameSettings;
            }
        }
        public static bool WasHitButMissedThisFrame()
        {
            var  wasHit = instance?.rythmPositionTracker?.WasHitButMissedThisFrame;
            return wasHit.HasValue == true ? wasHit.Value :  false;
        }
        static public RythmPosition RythmPosition
        {
            get
            {
                if ( instance != null )
                {
                    return instance.rythmPositionTracker.CurrentPosition;
                }
                return new RythmPosition ();
            }
        }

        [ShowInInspector]
        public bool MuteAudio { get => RythmManager.Settings.MuteAudio; set => RythmManager.Settings.MuteAudio = value; }

        // Start is called before the first frame update
        void Awake()
        {
            if ( instance == null )
            {
                instance = this;
                LoadTracksAndSpawnRecords ();
                InitializeRythmPositionTracker ();
                InitializePatternDetector ();
            }
            else
            {
                Debug.LogError ( "There can only be one RythmManager. A second one has been instantiated! " );
            }
        }

        private void InitializePatternDetector()
        {
            var patterns = AudioLoading.LoadAllPatterns ( PathToPatterns );
            patternDetector = new PatternDetector ( patterns );
        }

        void InitializeRythmPositionTracker()
        {
            var trackDuration = trackManager.Duration;
            var settings = RythmManager.Settings;
            rythmPositionTracker = new PositionTracker ( settings.MEASURES_PER_LOOP, settings.BEATS_PER_MEASURE, settings.TICKS_PER_BEATS, trackDuration );
        }

        public float PositionInPercentage() => ( float )( RythmPosition.RawPosition / trackManager.Duration );

        void CheckForBeatHit()
        {
            var currentPosition = trackManager.GetCurrentSamplePosition ();
            var hitStep = rythmPositionTracker.CheckIfStepWasHit ( currentPosition );

            if ( hitStep.RawPosition >= 0 )
            {
                Debug.Log ( $"(!!! {hitStep.Step.Id})Was hit:  {hitStep.RawPosition}  " );
                MessageHub.PublishNews<string> ( MessageTopics.DisplayUI_FX_string, UI_FXs.FX_Star );
                MessageHub.PublishNews<RythmPosition> ( MessageTopics.StepHit_Step, hitStep );
            }
            else
            {
                MessageHub.PublishNews<RythmPosition> ( MessageTopics.HitMissed_Step, hitStep );
                Debug.LogWarning ( $"No steps hitrange was hit Position:  {hitStep.RawPosition}" );
            }
        }
        private void LoadTracksAndSpawnRecords()
        {
            var tracks = AudioLoading.LoadAllTracks ( PathToAudio, SettingsFile, () => gameObject.AddComponent<Track> () );
            var records = AudioLoading.GetRecordPrefabs ( tracks, GameObject.Find ( "Dynamic Records" ).transform );
            trackManager = new TrackManager ( tracks );
        }

        void Update()
        {
            if ( trackManager.IsAnyTrackPlaying () )
            {
                AudioListener.volume = MuteAudio == true ? 0f : 1f;

                rythmPositionTracker.UpdateCurrentRythmPosition ( trackManager.GetCurrentSamplePosition () );

            }
        }


        #region static help methods
        public static void CreateHitRangeTable()
        {
            if ( instance == null )
            {
                Debug.LogWarning ( "You cant create a hit range table atm. RythmPositionTracker is not awake. Run game and try again. " );
                return;
            }
            instance.rythmPositionTracker.CreateHitRangeTable ();
        }

        public static Dictionary<string, string> GetPositionTrackingInfo()
        {
            if ( instance == null || instance.rythmPositionTracker == null )
            {
                Debug.LogWarning ( "RythmPositionTracker is not awake. Run game and try again. " );
                return null;
            }
            var tracker = instance.rythmPositionTracker;

            var info = new Dictionary<string, string> ();
            info.Add ( "Padding multiplier", tracker.paddingMultiplier.ToString () );
            info.Add ( "Track duration", tracker.trackDuration.ToString ( "N0" ) );
            info.Add ( "Step  duration", tracker.stepDuration.ToString ( "N0" ) );
            info.Add ( "Padding duration", tracker.paddingDuration.ToString ( "N0" ) );

            return info;
        }

        #endregion
        #endregion
    }
}