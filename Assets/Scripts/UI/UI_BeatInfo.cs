using SuperstarDJ.Audio;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BeatInfo : MonoBehaviour
{   
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI> ();
        text.text = $"";
    }

    // Update is called once per frame
    void Update()
    {
        if ( MusicManager.RythmPosition.Measure != null  && MusicManager.RythmPosition.IsInHitArea == true)
        {   
            var measure = MusicManager.RythmPosition.Measure.index;
            var beat = MusicManager.RythmPosition.Beat.positionInMeasure;
            text.text = $"{measure}:{beat}";
        }
        else
        {
            text.text = $"";
        }
    }
}
