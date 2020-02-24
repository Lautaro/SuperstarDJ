using SuperstarDJ.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualMetronome : MonoBehaviour
{
    List<Transform> blips;
    public static RythmPosition rythmPosition;
    // Start is called before the first frame update
    void Start()
    {
        blips = GetComponentsInChildren<Transform> ().Where ( t => t.name.Length == 1 ).ToList ();
        print ( $"Found {blips.Count} transforms" );
    }

    // Update is called once per frame
    void Update()
    {
        if ( rythmPosition.Beat != null )
        {
            var index = rythmPosition.Beat.index;
            blips.ForEach ( t => t.localScale *= 1 );
            blips[index].localScale *= 2;
        }
    }
}
