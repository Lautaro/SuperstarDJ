using SuperstarDJ;
using SuperstarDJ.Audio;
using SuperstarDJ.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player_Interactions : MonoBehaviour
{
    public List<GameObject> RecordsInReach;
    public GameObject Carrying;
    public List<CanHoldItem> SurfaceInReach;
    GameObject Hand;
    public bool BeatMixingIsActive;

    // Start is called before the first frame update 
    void Start()
    {
        SurfaceInReach = new List<CanHoldItem> ();
        RecordsInReach = new List<GameObject> ();
    }

    void OnTriggerEnter2D( Collider2D col )
    {
        var carriable = col.gameObject.GetComponent<CanBeCarried> ();
        var surfaceInReach = col.gameObject.GetComponent<CanHoldItem> ();
        if ( carriable != null )
        {
            RecordsInReach.Add ( col.gameObject );
        }

        if ( surfaceInReach != null )
        {
            SurfaceInReach.Add ( surfaceInReach );
        }

    }
    void OnTriggerExit2D( Collider2D col )
    {
        var carriable = col.gameObject.GetComponent<CanBeCarried> ();
        var surfaceInReach = col.gameObject.GetComponent<CanHoldItem> ();
        if ( carriable != null )
        {
            RecordsInReach.Remove ( col.gameObject );
        }

        if ( surfaceInReach != null )
        {
            SurfaceInReach.Remove ( surfaceInReach );
        }
    }

    public void OnPlayerMoving( object facesLeft )
    {
        if ( Carrying != null )
        {
            var direction = ( bool )facesLeft == true ? -0.5f : 0.5f;
            Carrying.transform.position = transform.position + new Vector3 ( direction, 0 );
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown ( KeyCode.U ) )
        {
            if ( Carrying != null )
            {
                if ( SurfaceInReach.Count > 0 )
                {
                    var surface = SurfaceInReach[0];
                    var carriable = Carrying.GetComponent<CanBeCarried> ();
                    var returnObject = surface.Place ( carriable );
                    if ( returnObject != null )
                    {
                        Carrying = returnObject.gameObject;
                    }
                    else
                    {
                        Carrying = null;
                    }
                }
                else
                {
                    Carrying = null;
                }

            }
            else if ( RecordsInReach.Count > 0 || SurfaceInReach.Count > 0 )
            {
                if ( SurfaceInReach.Count > 0 && SurfaceInReach[0].HeldItem != null )
                {
                    var surface = SurfaceInReach[0];
                    Carrying = surface.Take ().gameObject;

                }
                else if ( RecordsInReach.Count > 0 )
                {
                    Carrying = RecordsInReach[0];
                }
            }
        }

        #region BeatMixing

        if ( Input.GetKeyDown ( KeyCode.Space ) )
        {
            BeatMixingIsActive = true;
            ToggleBeatMixing ( true );
        }

        if ( Input.GetKeyUp ( KeyCode.Space ) )
        {
            BeatMixingIsActive = false;
            ToggleBeatMixing ( false );
        }
        #endregion
    }

    private void ToggleBeatMixing( bool play )
    {
        var turnTableWithRecord = SurfaceInReach.FirstOrDefault (
            surf => surf.HasItem () &&
            surf.HeldItem.CarriableType == CarriableItemType.Record );

        // IF RECORD EXISTS 
        if ( turnTableWithRecord != null )
        {
            var track = UnityTools.TrackNameFromString ( turnTableWithRecord.HeldItem.name );
            if ( play == true )
            {
                MusicManager.UnMuteTrack ( track );
            }
            else
            {
                MusicManager.MuteTrack ( track );
            };
        }
    }
}
