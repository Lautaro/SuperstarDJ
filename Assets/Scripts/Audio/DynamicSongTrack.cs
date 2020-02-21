using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.DynamicMusic
{
 
    class DynamicSongTrack
    {
        public AudioSource source { get; private set; }
        public double Duration { get; private set; }
        public Transform TrackingObject { get; set; }
        public string ClipName { get; private set; }


        public DynamicSongTrack(AudioSource _source, AudioClip clip, string clipName)
        {
            source = _source;
            source.clip = clip;
            Duration = (double)clip.samples / clip.frequency;
            ClipName = clipName;

        }
    }
}

