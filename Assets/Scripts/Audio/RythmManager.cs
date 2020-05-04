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
            instance.EvaluateDjAct ();

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
        List<DjAct> allDjActs = new List<DjAct> ();
        static GameSettings gameSettings;
        public static GameSettings Settings
        {
            get
            {
                if ( GameSettings.Instance == null )
                {
                    GameSettings.Instance = Resources.Load<GameSettings> ( "Settings/GameSettings" );
                }
                return GameSettings.Instance;
            }
        }
        static public RythmPosition CurrentPosition
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

        //static public List<DjAct> DjActs
        //{
        //    get
        //    {
        //        if ( instance != null )
        //        {
        //            return instance.rythmPositionTracker.CurrentPosition;
        //        }
        //        return new RythmPosition ();
        //    }
        //}


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

        void Start()
        {
            MessageHub.Subscribe ( MessageTopics.ResetPatternStatuses,OnResetLoop  );
        }
        private void OnResetLoop(Message message)
        {
            allDjActs.Clear ();
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

        public float CurrentPositionInPercentage() => ( float )( CurrentPosition.RawPosition / trackManager.Duration );
        public float CalculatePositionInPercentage(double position) => (float)(position / trackManager.Duration);

        void EvaluateDjAct()
        {
            var currentPosition = trackManager.GetCurrentSamplePosition ();
            var newDjAct = rythmPositionTracker.CheckDjActResult ( currentPosition );

            if ( newDjAct.StepThatWastHit != null )
            {
                Debug.Log ( $"(!!! {newDjAct.StepThatWastHit?.Id})Was hit:  {newDjAct.ActualPosition.RawPosition}  " );
                MessageHub.PublishNews<string> ( MessageTopics.DisplayUI_FX_string, UI_FXs.FX_Star );
                MessageHub.PublishNews<DjAct> ( MessageTopics.DjActHit_DjAct, newDjAct );
            }
            else
            {
                MessageHub.PublishNews<DjAct> ( MessageTopics.DjActMissed_DjAct, newDjAct );
                Debug.Log ( $"No steps hitrange was hit Position:  {newDjAct.ActualPosition.RawPosition}" );
            }

            allDjActs.Add (newDjAct);
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