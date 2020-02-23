using SuperstarDJ;
using SuperstarDJ.Audio;
using SuperstarDJ.DynamicMusic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanHoldItem : MonoBehaviour
{
    public  CanBeCarried HeldItem;
    public float RotationSpeed;

    public CanBeCarried Place(CanBeCarried objectToPlace)
    {
        var returnObject = HeldItem;
        HeldItem = objectToPlace;
        HeldItem.transform.position = transform.position;
        ResetRotationOfPlacedObject();

        var trackName = UnityTools.TrackNameFromString ( objectToPlace.GetComponent<CanBeCarried> ().CarriableName );

        if (objectToPlace != null)
        {   
            MusicManager.UnMuteTrack ( trackName );
        }

        if (returnObject != null)
        {
            MusicManager.MuteTrack( trackName );
        }
        return returnObject;
    }

    public bool HasItem()
    {
        return HeldItem != null;
    }

    public CanBeCarried Take()
    {
        var returnObject = HeldItem;
        ResetRotationOfPlacedObject();
        HeldItem = null;
        if (returnObject != null)
        {
            var trackName = UnityTools.TrackNameFromString( returnObject.CarriableName);
            MusicManager.MuteTrack( trackName );
        }
        return returnObject;
    }

    private void ResetRotationOfPlacedObject()
    {
        HeldItem.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void Update()
    {
        if (HeldItem)
        {
            HeldItem.transform.eulerAngles += new Vector3(0, 0, RotationSpeed);
            
        }
    }

}
