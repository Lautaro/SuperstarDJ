using Newtonsoft.Json.Linq;
using SuperstarDJ.Audio.DynamicTracks;
using SuperstarDJ.Audio.PatternDetection;
using SuperstarDJ.Enums;
using SuperstarDJ.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuperstarDJ.Audio.InitialiseAudio
{
    public static class AudioLoading
    {
        static List<Dictionary<string, string>> trackMetadata;
        public static List<Track> LoadAllTracks( string path, string musicSettingsFile, Func<Track> TrackFactory )
        {
            // load json
            var textAsset = Resources.Load ( path + musicSettingsFile ) as TextAsset;
            if ( textAsset == null ) Debug.LogError ( "Could not load music settings file. Expected path : " + path );

            var tracks = new List<Track> ();

            var settingsJson = JObject.Parse ( textAsset.ToString () );
            var metaData = ParseTrackMetadata ( settingsJson );

            foreach ( var trackInfo in metaData )
            {
                var name = trackInfo["TrackName"];
                var clip = Resources.Load<AudioClip> ( $"{path}{name}" );
                var track = TrackFactory ();
                track.VolumeModification = float.Parse ( trackInfo["VolumeModifier"] );
                track.Abreviation = trackInfo["Abreviation"];
                track.Source ().clip = clip;
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
                prefab.name = track.TrackName.ToString () + " (Record)";
                prefab.transform.parent = parent;
                prefab.transform.position = ProjectTools.GetRandomPlaceWithinScreen ();
                recordPrefabs.Add ( prefab );


            }
            return recordPrefabs;
        }

        public static List<Pattern> LoadAllPatterns( string path )
        {
            return Resources.LoadAll<Pattern> ( path ).ToList ();
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
                dic.Add ( "Abreviation", split[2] );

                metaDataDics.Add ( dic );
            }

            return metaDataDics;
        }
        #endregion

    }
}
