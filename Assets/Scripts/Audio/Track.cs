using UnityEngine;

namespace SuperstarDJ.Audio.DynamicTracks
{

    public class Track : MonoBehaviour
    {
        [HideInInspector]
        AudioSource source { get; set; }

        //(double)AudioClip.samples / AudioClip.frequency;
        public double Duration { get { return ( double )source.clip.samples; } }// / source.clip.frequency; } }
        string clipName { get { return source.clip.name; } }
        public string Abreviation;
        public float VolumeModification;

        public string TrackName
        {
            get
            {
                return clipName;
            }
        }

        public AudioSource Source()
        {
            return source; ;
        }

        void Awake()
        {
            source = gameObject.AddComponent<AudioSource> ();
        }
        public bool IsPlaying
        {
            get
            {
                return source.isPlaying == true && source.volume > 0f;
            }
        }
    }
}

