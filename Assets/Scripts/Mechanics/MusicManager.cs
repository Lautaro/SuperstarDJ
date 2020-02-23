using SuperstarDJ.DynamicMusic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperstarDJ.UnityTools.Extensions;
using SuperstarDJ.Audio.Enums;
using System;
using SuperstarDJ.Audio.InitialiseAudio;

namespace SuperstarDJ.Audio
{
    public class MusicManager : MonoBehaviour
    {
        #region Static Methods
        static MusicManager instance;

        public static void UnMuteTrack( TrackNames track )
        {
            instance.trackManager.UnMuteTrack ( track );
        }

        public static void MuteTrack( TrackNames track )
        {
            instance.trackManager.MuteTrack( track );
        }
        public static bool IsTrackPlaying( string trackName )
        {
            return instance.trackManager.IsTrackPlaying ( trackName );
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
        // Start is called before the first frame update
        void Start()
        {
            if ( instance == null )
            {
                instance = this;
                Initialize ();
            }
            else
            {
                Debug.LogError ( "There can only be one MusicManager. A second one has been instantiated! " );
            }
        }

        private void Initialize()
        {
            var tracks = LoadTracks.Load ( PathToAudio, SettingsFile, () => gameObject.AddComponent<Track> () );
            trackManager = new TrackManager ( tracks );
        }
       #endregion
    }
}