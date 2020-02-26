using SuperstarDJ.Audio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SuperstarDJ.DynamicMusic
{
    public static class UnityTools
    {
        public static Vector3 GetRandomWithinBounds( Bounds bounds )
        {
            return bounds.center + new Vector3 (
                ( Random.value - 0.5f ) * bounds.size.x,
                ( Random.value - 0.5f ) * bounds.size.y,
                ( Random.value - 0.5f ) * bounds.size.z
             );
        }

        public static Vector2 GetRandomPlaceWithinScreen(float limitToCenter = 1f)
        {
            float randomY = Random.Range
                 (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
            float spawnX = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

            spawnX *= limitToCenter;
            randomY *= limitToCenter;
            return new Vector2(spawnX, randomY);
        }

        public static Vector3 GetSymmetricalVector( float size )
        {
            return new Vector3 ( size, size, size );
        }

        public static TrackNames TrackNameFromString( string trackName )
        {
            TrackNames name;
            if ( Enum.TryParse ( trackName, out name ) )
            {
                return name;
            }
            else
            {   
                throw new System.InvalidOperationException ( "You are trying to parse a string to a TrackName enum that doesnt exist. Bad boy! Not found: "+ trackName );
            };
        }
    }
}