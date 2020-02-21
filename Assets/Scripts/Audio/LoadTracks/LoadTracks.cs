using SuperstarDJ.DynamicMusic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.Audio.LoadTracks
{
    public static class LoadTracks
    {
        public static List<DynamicTrack> Load( string path )
        {
            // load json
            var textAsset = Resources.Load ( path ) as TextAsset;
            if ( textAsset == null ) Debug.LogError ( "Could not load music settings file. Expected path : " + path );

            //var settingsJson = JObject.Parse ( textAsset.ToString () );
            //var allSongs = SongParser.Parse ( settingsJson );

            //var settings = new MusicSettings ();
            //settings.AllSongs = allSongs;

            //settings.GameSongGroups = GameSongGroupParsing.Parse ( settingsJson, allSongs );
            //settings.TileSetSongGroups = TilesetSongGroupParsing.Parse ( settingsJson, allSongs );

            return null;
        }
    }
}
