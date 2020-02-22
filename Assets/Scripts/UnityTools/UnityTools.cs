using SuperstarDJ.Audio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public static Vector3 GetSymmetricalVector( float size )
    {
        return new Vector3 ( size, size, size );
    }

    public static TrackNames TrackNameFromString( string trackName )
    {
        TrackNames name;
        if (Enum.TryParse<TrackNames> ( trackName, out name ))
        {
            return name;
        }
        else
        {
            Debug.LogError ( "You are trying to parse a string to a TrackName enum that doesnt exist. Bad boy!" );
            throw new System.InvalidOperationException ( "You are trying to parse a string to a TrackName enum that doesnt exist. Bad boy!" );
        };
    }
}
