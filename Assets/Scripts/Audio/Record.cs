using SuperstarDJ.DynamicMusic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace SuperstarDJ.Audio
{
    public class Record : MonoBehaviour
    {
        Track track;
        public Track Track { get {
                return track;
            }
            
            set {
                track = value;
                GetComponentInChildren<TextMeshPro>().text = track.Abreviation;
            } }

    }
}
