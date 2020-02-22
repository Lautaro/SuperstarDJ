using Newtonsoft.Json.Linq;
using SuperstarDJ.DynamicMusic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuperstarDJ.Audio.InitialiseAudio
{
public static class LoadTracks
    {
        static List<Dictionary<string, string>> trackMetadata;
        public static List<DynamicTrack> Load( string path,string musicSettingsFile, Func<AudioSource> AudioSourceFactory )
        {
            // load json
            var textAsset = Resources.Load ( path + musicSettingsFile) as TextAsset;
            if ( textAsset == null ) Debug.LogError ( "Could not load music settings file. Expected path : " + path );

            var tracks = new List<DynamicTrack> ();

            var settingsJson = JObject.Parse ( textAsset.ToString () );
            var metaData = ParseTrackMetadata ( settingsJson );

            foreach ( var trackInfo  in metaData )
            {
                var name = trackInfo["TrackName"];
                var clip = Resources.Load<AudioClip> ( $"{path}{name}" );
                var track = new DynamicTrack (AudioSourceFactory() ,clip  );

                tracks.Add ( track );
            }
       
            return null;
        }

        private static void  LoadAudio( List<DynamicTrack> tracks,  string path )
        {
      
        }

        private static List<Dictionary<string, string>> ParseTrackMetadata( JObject settings )
        {
            var allMetadata = settings["Tracks"].Children ().Select ( md => ( string )md ).ToList ();
            var metaDataDics = new List<Dictionary<string, string>> ();

            foreach ( var metaData in allMetadata )
            {
                var dic = new Dictionary<string, string> ();

                var split = metaData.Split ( ' ' );
                dic.Add ( "TrackName", split[0] );
                dic.Add ( "VolumeModifier", split[0] );

                metaDataDics.Add ( dic );
            }
            return metaDataDics;
        }
    }
}
