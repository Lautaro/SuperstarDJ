using SuperstarDJ.Audio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.DynamicMusic
{

    public class Track : MonoBehaviour
    {
        [HideInInspector]
        AudioSource source { get; set; }

        //(double)AudioClip.samples / AudioClip.frequency;
        public double Duration { get { return ( double )source.clip.samples / source.clip.frequency; } }
        string clipName { get { return source.clip.name; } }
        float VolumeModification;

        public TrackNames TrackName
        {
            get
            {
                return UnityTools.TrackNameFromString ( clipName );
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

