using SuperstarDJ;
using SuperstarDJ.Audio;
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
        if (objectToPlace != null)
        {
            var recordName = objectToPlace.GetComponent<CanBeCarried>().CarriableName;
            MusicManager.Instance.DynamicSong.SetClipVolume(1 ,recordName);
        }

        if (returnObject != null)
        {

            var record = returnObject.CarriableName;
            MusicManager.Instance.DynamicSong.SetClipVolume(0, record);
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
            var record = returnObject.CarriableName;
            MusicManager.Instance.DynamicSong.SetClipVolume(0, record);
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
