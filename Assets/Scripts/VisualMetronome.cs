using SuperstarDJ.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VisualMetronome : MonoBehaviour
{
    List<Transform> blips;
    Vector3 blipDefaultSize;
    RythmPosition rythmPosition;
    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI> ();
        blips = GetComponentsInChildren<Transform> ().Where ( t => t.name.Length == 1 ).ToList ();
        blipDefaultSize = blips[0].localScale;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "";
   
            blips.ForEach ( t => t.localScale = blipDefaultSize );
            rythmPosition = RythmManager.RythmPosition;
            text.text = $"[{rythmPosition.Tick.Measure}] - {rythmPosition.Tick.Beat}";
            var index = rythmPosition.Tick.Beat;
            blips[index].localScale *= 2;
    }
}
