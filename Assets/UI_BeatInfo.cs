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
        if ( RythmManager.RythmPosition.Measure != null  && RythmManager.RythmPosition.IsInHitArea())
        {
            
            var measure = RythmManager.RythmPosition.Measure.index;
            var beat = RythmManager.RythmPosition.Beat.index;
            text.text = $"{measure}:{beat}";
        }
        else
        {
            text.text = $"";
        }
    }
}
