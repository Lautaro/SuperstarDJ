using SuperstarDJ.Audio.DynamicTracks;
using TMPro;
using UnityEngine;

namespace SuperstarDJ.Audio
{
    public class Record : MonoBehaviour
    {
        Track track;
        public Track Track
        {
            get
            {
                return track;
            }

            set
            {
                track = value;
                GetComponentInChildren<TextMeshPro> ().text = track.Abreviation;
            }
        }

    }
}
