using Sirenix.OdinInspector;
using SuperstarDJ.Audio;
using SuperstarDJ.CustomEditors.CustomAttributes;
using SuperstarDJ.MessageSystem;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SuperstarDJ
{
    [CreateAssetMenu ( menuName = "SUPERSTAR_DJ/GameSettings" )]
    public class GameSettings : SerializedScriptableObject
    {
        #region internal UI Settings
       const float d = 30f; // Distance between regions
        #endregion

        public static GameSettings instance;

        private void OnEnable()
        {
            //instance =  Resources.Load<GameSettings> ( "Settings/GameSettings" );
            instance = this;
        }

        #region Audio Settings

        [Title ( "Audio Settings" ), PropertySpace ( d, 0 )]
        [PropertyOrder ( 0 )]
        public bool MuteAudio;
        #endregion

        #region PatternDetection
        [PropertySpace ( d, 0 )]
        [Title ( "Pattern Detection" )]

        [Range ( 0, 50000 ), LabelText ( "Input Padding" ), PropertyOrder ( 1 )]
        public double PatternDetectionInputLagPadding;

        [Button, PropertyOrder ( 1 )]
        private void DebugLogStepHitranges()
        {
            RythmManager.CreateHitRangeTable ();
        }

        [Range ( 0, 49 ),LabelText ( "Hit Range Padding" ), PropertyOrder ( 1 )]
        public int HitRangePaddingInPercentage = 20; //15 = 15%  Amount of padding to make it more forgiving to hit a steps HitArea


        [Button, PropertyOrder ( 5 )]
        private void UpdatePositionTrackingInfo()
        {
            var positionTrackingInfo = RythmManager.GetPositionTrackingInfo ();
            if ( positionTrackingInfo != null )
            {
                PositionTrackingInfo.Clear ();
                foreach ( var item in positionTrackingInfo )
                {
                    PositionTrackingInfo.Add ( $"{item.Key} : {item.Value}" );
                }
            }
        }


        [Title ( "Position Tracker Info" ), PropertySpace ( d, 0 ), PropertyOrder ( 5 ), DisplayAsString]
        [ListDrawerSettings ( Expanded = true, IsReadOnly = true )]
        List<string> PositionTrackingInfo = new List<string> ();

 
        #endregion

        #region MESSAGE LOGGING
        [PropertySpace ( d, 0 )]
        [Title ( "MESSAGE LOGGING" ), ]
        
        [Title ( "Muted Topics", horizontalLine: false ), EnumToggleButtons, HideLabel, PropertyOrder ( 10 )]
         public MessageTopics MutedTopics;

        [Button ( ButtonSizes.Large, Name = "Clear" ), GUIColor ( 1f, 0.6f, 0.6f ), PropertyOrder(10)]
        public void ClearMuted()
        {
            MutedTopics = 0;
        }

        [InfoBox ( "If any topic is on this list then all topics not on list will be muted. Clear list to return to normal configuration." )]
        [Title ( "Solo Topics", horizontalLine: false ), EnumToggleButtons, HideLabel, PropertyOrder ( 20 )]
        public MessageTopics SoloTopics;

        [PropertyOrder ( 20 )]
        [Button ( ButtonSizes.Large, Name = "Clear" ), GUIColor ( 1f, 0.6f, 0.6f )]
        public void ClearSolo()
        {
            SoloTopics = 0;
        }
        #endregion

        #region Rythm Settings

        [IncludeMyAttributes, LabelWidth ( 200 ), DisplayAsString, PropertyOrder ( 50 ), ShowInInspector]
         class RythmSettingsAttributesAttribute : Attribute { }

        [Title ( "Rythm Settings" ), PropertySpace (d ,0 ), RythmSettingsAttributes]
        public readonly int StepsInPattern = 64;
        [RythmSettingsAttributes]
        public double PatternLengthInSamples;
        [RythmSettingsAttributes]
        public readonly int MEASURES_PER_LOOP = 4;
        [RythmSettingsAttributes]
        public readonly int BEATS_PER_MEASURE = 4;
        [RythmSettingsAttributes]
        public readonly int TICKS_PER_BEATS = 4;

        [RythmSettingsAttributes, LabelText("Position in %")]
        public string PositionInPercentage
        {
            get {
                if ( RythmManager.instance == null ) return "N/A";
                return $"{ ( ( float )( RythmManager.instance.PositionInPercentage () * 100 ) ).ToString ( "N0" )}"; 
            }
            set { return;  }
        }
        #endregion
    }
}