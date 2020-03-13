using Sirenix.OdinInspector;
using SuperstarDJ.MessageSystem;
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
        public static GameSettings LoadGameSettings()
        {
            instance = Resources.Load<GameSettings> ( "Settings/GameSettings" );
            return instance;
        }


        [Title ( "MuteAudio" )]
        [HideLabel]
        [PropertySpace ( SpaceAfter = 10 )]
        [PropertyOrder ( 0)]
        public bool MuteAudio;

        [Button( ButtonSizes.Large,Name = "Clear" ), GUIColor(1f, 0.6f, 0.6f)]
        [PropertyOrder ( 1 )]
        [PropertySpace ( 0, 10 )]
        [BoxGroup ( "MESSAGE LOGGING" )]
        public void ClearMuted()
        {
            MutedTopics = 0;
        }

        [Title ( "MESSAGE LOGGING" )]
        [BoxGroup ( "MESSAGE LOGGING", ShowLabel = false )]
        [HideLabel]
        [EnumToggleButtons]
        [PropertyOrder ( 1)]
        [Title("Muted Topics",horizontalLine:false)]
        public MessageTopics MutedTopics;



        [EnumToggleButtons]
        [HideLabel]
        [PropertyOrder ( 2 )]
        [BoxGroup ( "MESSAGE LOGGING" )]
        [InfoBox ( "If any topic is on this list then all topics not on list will be muted. Clear list to return to normal configuration." )]
        [Title ( "Muted Topics", horizontalLine: false )]
        public MessageTopics SoloTopics;

        [PropertySpace ( 0, 10 )]
        [PropertyOrder(2)]
        [BoxGroup ( "MESSAGE LOGGING" )]
        [Button ( ButtonSizes.Large, Name = "Clear" ), GUIColor ( 1f, 0.6f, 0.6f )]
        public void ClearSolo()
        {
            SoloTopics = 0;
        }










    }
}