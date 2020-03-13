using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SpawnGuest
{
    GameObject guestPrefab;
    string prefabFolderPath = "Prefabs";
    int minFavTracks = 2;
    int maxFavTracks = 3;
    int minDislikedTracks = 1;
    int maxDislikedTracks = 4;

    public SpawnGuest()
    {
        guestPrefab = Resources.Load<GameObject> ( prefabFolderPath + "\\Guest" );
    }

    public List<Guest> SpawnGuests( int amount, Transform parent, Vector3 position )
    {
        var guests = new List<Guest> ();

        for ( int i = 0; i < amount; i++ )
        {
            var guestGo = GameObject.Instantiate ( guestPrefab, new Vector3 ( 0, 0, 0 ), Quaternion.identity, parent );
            guestGo.transform.position = position;
            var guest = guestGo.GetComponent<Guest> ();

            RandomFavTracks ( guest );
            RandomDislikedTracks ( guest );

            guests.Add ( guest );
        }

        return guests;
    }

    private void RandomFavTracks( Guest guest )
    {
        var amount = new Random ().Next ( minFavTracks, maxFavTracks );
        var returnList = new List<string> ();

        for ( int i = 0; i < amount; i++ )
        {
            Tracks track = GetRandomTrack ();

            while ( returnList.Contains ( track.ToString () ) )
            {
                track = GetRandomTrack ();
            }

            returnList.Add ( track.ToString () );
        }

        guest.FavouriteTracks = returnList;
    }

    private void RandomDislikedTracks( Guest guest )
    {
        var amount = new Random ().Next ( minDislikedTracks, maxDislikedTracks );
        var tracks = new List<string> ();

        for ( int i = 0; i < amount; i++ )
        {
            Tracks track = GetRandomTrack ();

            while ( tracks.Contains ( track.ToString () ) || guest.FavouriteTracks.Contains ( track.ToString () ) )
            {
                track = GetRandomTrack ();
            }

            tracks.Add ( track.ToString () );
        }

        guest.DislikedTracks = tracks;
    }

    Tracks GetRandomTrack()
    {
        var values = Enum.GetValues ( typeof ( Tracks ) );
        return ( Tracks )values.GetValue ( new Random ().Next ( values.Length ) );
    }
}
