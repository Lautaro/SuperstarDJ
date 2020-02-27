using SuperstarDJ.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualMetronome : MonoBehaviour
{
    List<Transform> blips;
    Vector3 blipDefaultSize;
     RythmPosition rythmPosition;
    // Start is called before the first frame update
    void Start()
    {
        blips = GetComponentsInChildren<Transform> ().Where ( t => t.name.Length == 1 ).ToList ();
        blipDefaultSize = blips[0].localScale;
        print ( $"Found {blips.Count} transforms" );
        rythmPosition = MusicManager.RythmPosition ;
    }

    // Update is called once per frame
    void Update()
    {
        if ( rythmPosition.Beat != null )
        {
            var index = rythmPosition.Beat.index -1;
            blips.ForEach ( t => t.localScale = blipDefaultSize);
            blips[index].localScale *= 2;
        }
    }
}
