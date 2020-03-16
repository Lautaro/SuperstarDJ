using Sirenix.OdinInspector;
using SuperstarDJ.Audio;
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
        static GameSettings instance;
        public static GameSettings Instance
        {
            get
            {
                if ( instance == null ) LoadGameSettings ();
                return instance;
            }
        }

        [PropertySpace ( 10, 0 )]
        [Title ( "Mute Audio" )]
        [HideLabel]
        [PropertyOrder ( 0 )]
        public bool MuteAudio;

        [PropertySpace ( 30, 0 )]
        [ShowInInspector]
        [Range ( 0, 50000 )]
        [Title ( "Pattern Detection" )]
        [LabelText ( "Input Padding" )]
        [PropertyOrder ( 1 )]
        public double PatternDetectionInputLagPadding;

        [Button]
        [PropertyOrder ( 1 )]
        private void DebugLogTickHitranges()
        {
            RythmManager.CreateHitRangeTable ();
        }

        [ShowInInspector]
        [Range ( 0, 49 )]
        [LabelText ( "Hit Range Padding" )]
        [PropertyOrder ( 1 )]
        public int HitRangePaddingInPercentage = 20; //15 = 15%  Amount of padding to make it more forgiving to hit a ticks HitArea


        [Button]
        [PropertyOrder ( 5 )]
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


        [Title( "Position Tracker Info" )]
        [PropertySpace ( 30, 0 )]
        [PropertyOrder ( 5 )]
        [ListDrawerSettings ( Expanded = true, IsReadOnly = true )]
        [DisplayAsString]
        [ShowInInspector]
        List<string> PositionTrackingInfo = new List<string> ();

        public static GameSettings LoadGameSettings()
        {
            instance = Resources.Load<GameSettings> ( "Settings/GameSettings" );
            return instance;
        }

        [PropertySpace ( 30, 0 )]
        [Title ( "MESSAGE LOGGING" )]
        [HideLabel]
        [EnumToggleButtons]
        [Title ( "Muted Topics", horizontalLine: false )]
        [PropertyOrder ( 10 )]
        public MessageTopics MutedTopics;

        [Button ( ButtonSizes.Large, Name = "Clear" ), GUIColor ( 1f, 0.6f, 0.6f )]
        [PropertyOrder ( 10 )]
        public void ClearMuted()
        {
            MutedTopics = 0;
        }

        [EnumToggleButtons]
        [HideLabel]
        [InfoBox ( "If any topic is on this list then all topics not on list will be muted. Clear list to return to normal configuration." )]
        [Title ( "Solo  Topics", horizontalLine: false )]
        [PropertyOrder ( 20 )]
        public MessageTopics SoloTopics;

        [PropertyOrder ( 20 )]
        [Button ( ButtonSizes.Large, Name = "Clear" ), GUIColor ( 1f, 0.6f, 0.6f )]
        public void ClearSolo()
        {
            SoloTopics = 0;
        }
    }
}