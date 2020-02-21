using SuperstarDJ.DynamicMusic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperstarDJ.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public DynamicSong_MergeMe DynamicSong;
        public Transform Player;
        public Transform BasicBeat;
        public Transform Bass;
        public Transform Chords;
        public Transform Pads;
        public Transform Arrpegio;
        public Transform ExtraBeat;
        public static MusicManager Instance;
        // Start is called before the first frame update
        void Start()
        {
            DynamicSong = GetComponent<DynamicSong_MergeMe>();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new System.Exception("Only one music manager allowed");
            }

            DynamicSong.SetLoop("BasicBeat", "Bass", "Chords", "Pads", "Arrpegio", "ExtraBeat");

            //songySong.TrackingObject = Player;
            //songySong.SetTrackingObjectOnClip("BasicBeat", BasicBeat);
            //songySong.SetTrackingObjectOnClip("Bass", Bass);
            //songySong.SetTrackingObjectOnClip("Chords", Chords);
            //songySong.SetTrackingObjectOnClip("Pads", Pads);
            //songySong.SetTrackingObjectOnClip("Arrpegio", Arrpegio);
            //songySong.SetTrackingObjectOnClip("ExtraBeat", ExtraBeat);
            DynamicSong.SetMainVolume(0);
            DynamicSong.PlayDynamicSong();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}