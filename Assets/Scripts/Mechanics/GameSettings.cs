using Sirenix.OdinInspector;
using SuperstarDJ.Audio;
using System;
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


        public bool MuteAudio;

        public bool DoThis;
        public bool DoThat;




    }
}