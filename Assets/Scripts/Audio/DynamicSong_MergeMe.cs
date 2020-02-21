using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SuperstarDJ.DynamicMusic
{

    public class DynamicSong_MergeMe : MonoBehaviour
    {
        DynamicSong Intro;
        DynamicSong Loop;
        DynamicSong Outro;
        public bool IsPlaying;
        public string FolderPath;
        float MainVolume = 1f;
        public float MinimalDistance;
        public Transform TrackingObject { get; set; }

        private void Awake()
        {
            Intro = gameObject.AddComponent<DynamicSong>();
            Loop = gameObject.AddComponent<DynamicSong>();
            Outro = gameObject.AddComponent<DynamicSong>();
        }

        public void SetClipVolume(float volume, string clipName)
        {
            var clip = GetTrackByName(clipName);
            clip.source.volume = volume;
        }

        public float GetClipVolume(string clipName)
        {
            var clip = GetTrackByName(clipName);
            return clip.source.volume;
        }

        public void SetMainVolume(float volume)
        {
            MainVolume = volume;
            foreach (var clip in GetAllClipsInSong())
            {
                clip.source.volume = MainVolume;
            }
        }

        /// <summary>
        /// Returns true if sound is muted
        /// </summary>
        //public bool ToggleTrack(string clipName)
        //{
        //    foreach (var songyClip in GetAllClipsInSong())
        //    {
        //        if (songyClip.source.volume > 0)
        //        {
        //            songyClip.source.volume = 0f;
        //            return true;
        //        }
        //        else
        //        {
        //            songyClip.source.volume = 1f;
        //            return false;
        //        }
        //    }

        //    throw new Exception("Could not find clip named : " + clipName);
        //}

        List<DynamicTrack> GetAllClipsInSong()
        {
            var allClipsInSongy = new List<DynamicTrack>();
            void AddIfNotNullAndNotDupes(Dictionary<string, DynamicTrack> clips)
            {
                if (clips != null)
                {
                    foreach (var clip in clips)
                    {
                        if (!allClipsInSongy.Contains(clip.Value))
                        {
                            allClipsInSongy.Add(clip.Value);
                        }
                    }
                }
            }
            AddIfNotNullAndNotDupes(Intro.GetClipsDic());
            AddIfNotNullAndNotDupes(Loop.GetClipsDic());
            AddIfNotNullAndNotDupes(Outro.GetClipsDic());

            return allClipsInSongy;
        }

        internal void ToggleTrackExplicit(string trackName, bool play)
        {
            if (play)
            {
                SetClipVolume( 1f, trackName);
            }
            else
            {
                SetClipVolume(0f, trackName);
            }
        }

        private DynamicTrack GetTrackByName(string trackName)
        {
            var allClips = GetAllClipsInSong();
            var returnClip = allClips.FirstOrDefault(c => c.ClipName == trackName);
            return returnClip;
        }
        public void SetLoop(params string[] clipNames)
        {
            AddClips(clipNames, Loop);
        }

        void AddClips(string[] clipNames, DynamicSong dynamicSong)
        {
            foreach (var name in clipNames)
            {
                if (GetTrackByName(name) != null)
                {
                    Debug.LogWarning($"You are trying to add the SongyClip named {name} twice. Dont do that plz. ");
                    continue;
                }
                dynamicSong.AddClip(FolderPath, name);
            }
        }

        public Dictionary<string, float> GetPlayingTracks()
        {
            var clips = GetAllClipsInSong();
            var dic = new Dictionary<string, float>();
            foreach (var clip in clips)
            {
                if (clip.source.volume > 0.1)
                {
                    var volume = clip.source.volume;
                    dic.Add(clip.ClipName, volume);
                }
            }

            return dic;
        }

        public Dictionary<string, float> GetAllClipVolume()
        {
            var clips = GetAllClipsInSong();
            var dic = new Dictionary<string, float>();
            foreach (var clip in clips)
            {
                var volume = clip.source.volume;
                dic.Add(clip.ClipName, volume);
            }

            return dic;
        }
        public void PlayDynamicSong()
        {
            if (Intro.HasClip())
            {
                Intro.PlayClipAsap();
                Loop.PlayClipAfter(Intro);
                IsPlaying = true;
            }
            else if (Loop.HasClip())
            {
                Loop.PlayClipAsap();
                IsPlaying = true;
            }
            else
            {
                IsPlaying = false;
            }
        }
    }
}
