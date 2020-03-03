using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperstarDJ.Audio.RythmDetection
{
    public class RythmPattern : SerializedScriptableObject
    {
        public string PatternName { get; set; }

        public RythmPatternStates[] Pattern { get; set; } = new RythmPatternStates[64];

    }
}