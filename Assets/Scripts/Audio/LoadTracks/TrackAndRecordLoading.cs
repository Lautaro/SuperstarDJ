using Newtonsoft.Json.Linq;
using SuperstarDJ.Audio.Enums;
using SuperstarDJ.DynamicMusic;
using SuperstarDJ.Enums;
using SuperstarDJ.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuperstarDJ.Audio.InitialiseAudio
{
public static class TrackAndRecordLoading
    {
        static List<Dictionary<string, string>> trackMetadata;
        public static List<Track> Load( string path,string musicSettingsFile, Func<Track> TrackFactory )
        {
            // load json
            var textAsset = Resources.Load ( path + musicSettingsFile) as TextAsset;
            if ( textAsset == null ) Debug.LogError ( "Could not load music settings file. Expected path : " + path );

            var tracks = new List<Track> ();

            var settingsJson = JObject.Parse ( textAsset.ToString () );
            var metaData = ParseTrackMetadata ( settingsJson );

            foreach ( var trackInfo  in metaData )
            {
                var name = trackInfo["TrackName"];
                var clip = Resources.Load<AudioClip> ( $"{path}{name}" );
                var track = TrackFactory ();
                track.VolumeModification = float.Parse(trackInfo["VolumeModifier"]);
                track.Abreviation = trackInfo["Abreviation"];
                track.Source().clip = clip;
                track.Source ().volume = 0f;
                track.Source ().loop = true;
                tracks.Add ( track );
            }
       
            return tracks;
        }

        public static List<GameObject> GetRecordPrefabs( List<Track> tracks, Transform parent )
        {
            var recordPrefabs = new List<GameObject> ();
            foreach ( var track in tracks )
            {
                var prefab = SpawnPrefab.Instance.Spawn ( Prefabs.DynamicRecord );
                var record = prefab.GetComponent<Record> ();
                record.Track = track;
                prefab.name = track.TrackName.ToString() + " (Record)";
                prefab.transform.parent = parent;
                recordPrefabs.Add ( prefab );
            }
            return recordPrefabs;
        }

        #region Internal private methods
        private static List<Dictionary<string, string>> ParseTrackMetadata( JObject settings )
        {
            var allMetadata = settings["Tracks"].Children ().Select ( md => ( string )md ).ToList ();
            var metaDataDics = new List<Dictionary<string, string>> ();

            foreach ( var metaData in allMetadata )
            {
                var dic = new Dictionary<string, string> ();

                var split = metaData.Split ( ' ' );
                dic.Add ( "TrackName", split[0] );
                dic.Add ( "VolumeModifier", split[1] );
                dic.Add("Abreviation", split[2]);

                metaDataDics.Add ( dic );
            }

            var enums = Enum.GetValues ( typeof ( TrackNames ) ).Length;
            if ( metaDataDics.Count () != enums )
            {
                Debug.LogError ( $"The amount of tracks in the TrackName enum is not the same as the amount of tracks specidfied in the settings file. Enums={enums} - Tracks in settings = {metaDataDics.Count () } " );

            }
            return metaDataDics;
        }
        #endregion

    }
}
