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
        //if (  RythmManager.RythmPosition.WasHit)
        //{
        //    var measure = RythmManager.RythmPosition.Step.Measure;
        //    var beat = RythmManager.RythmPosition.Step.Beat;
        //    text.text = $"{measure}:{beat}";
        //}
        //else
        //{
        //    text.text = $"";
        //}
    }
}
