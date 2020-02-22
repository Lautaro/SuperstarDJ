using SuperstarDJ.Audio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SuperstarDJ.DynamicMusic
{
 
    public class DynamicTrack
    {
       public  AudioSource Source { get; set; }
        public double Duration { get { return ( double )Source.clip.samples / Source.clip.frequency; } }
        public string ClipName { get { return Source.clip.name; } }

        public TrackNames TrackName { get; set; }


        public DynamicTrack( AudioSource _source, AudioClip clip)
        {
            Source = _source;
            Source.clip = clip;
        }
    }
}

