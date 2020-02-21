﻿using System;
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
        public double Duration { get { return ( double )source.clip.samples / source.clip.frequency; } }
        public string ClipName { get { return source.clip.name; } }


        public DynamicSongTrack(AudioSource _source, AudioClip clip, string clipName)
        {
            source = _source;
            source.clip = clip;
        }
    }
}

