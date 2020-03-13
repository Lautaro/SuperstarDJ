using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Guest : MonoBehaviour
{
    public TextMeshPro satisfactionUI;
    public TextMeshPro favTracksUI;
    public TextMeshPro dislikedTracksUI;
    public float Satisfaction = 70;
    public List<string> FavouriteTracks;
    public List<string> DislikedTracks;
    // Start is called before the first frame update

    void Start()
    {
        satisfactionUI = transform.Find ( "SatisfactionUI" ).GetComponent<TextMeshPro> ();
        favTracksUI = transform.Find ( "FavTracksUI" ).GetComponent<TextMeshPro> ();
        dislikedTracksUI = transform.Find ( "DislikedTracksUI" ).GetComponent<TextMeshPro> ();

        favTracksUI.text = string.Join ( " \n", FavouriteTracks.ToArray () );
        dislikedTracksUI.text = string.Join ( " \n", DislikedTracks.ToArray () );
    }

    public void OnSatisfiedSuccess()
    {
        this.gameObject.SetActive ( false );
    }

    public void OnSatisfiedFail()
    {
        this.gameObject.SetActive ( false );

    }
}
