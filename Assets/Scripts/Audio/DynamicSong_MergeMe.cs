﻿using System;
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
            Loop.ShouldLoop = true;
        }

        private void Update()
        {
            if (TrackingObject != null)
            {
                foreach (var clip in GetAllClipsInSongy())
                {
                    var distance = PercentageOfMinimalDistanceToClip(clip) / 100;
                    clip.source.volume = 1f - distance;
                }
            }
        }

        public void SetTrackingObjectOnClip(string clipName, Transform transform)
        {
            var clip = GetTrackByName(clipName);

            if (clip == null)
            {
                throw new KeyNotFoundException($"Clip named {clipName} does not exist in SongySong");
            }

            clip.TrackingObject = transform;
        }

        float PercentageOfMinimalDistanceToClip(DynamicSongTrack clip)
        {
            var clipPosition = clip.TrackingObject.position;
            var soundPosition = TrackingObject.position;

            var diff = Mathf.Abs(Vector2.Distance(clipPosition, soundPosition));

            return diff / MinimalDistance * 100;
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
            foreach (var clip in GetAllClipsInSongy())
            {
                clip.source.volume = MainVolume;
            }
        }

        /// <summary>
        /// Returns true if sound is muted
        /// </summary>
        public bool ToggleTrack(string clipName)
        {
            var allClips = GetTrackByName(clipName);

            foreach (var songyClip in GetAllClipsInSongy())
            {
                if (songyClip.source.volume > 0)
                {
                    songyClip.source.volume = 0f;
                    return true;
                }
                else
                {
                    songyClip.source.volume = 1f;
                    return false;
                }
            }

            throw new Exception("Could not find clip named : " + clipName);
        }

        List<DynamicSongTrack> GetAllClipsInSongy()
        {
            var allClipsInSongy = new List<DynamicSongTrack>();
            void AddIfNotNullAndNotDupes(Dictionary<string, DynamicSongTrack> clips)
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

        private DynamicSongTrack GetTrackByName(string trackName)
        {
            var allClips = GetAllClipsInSongy();
            var returnClip = allClips.FirstOrDefault(c => c.ClipName == trackName);
            return returnClip;
        }

        public void SetIntro(params string[] clipNames)
        {
            AddClips(clipNames, Intro);
        }

        public void SetLoop(params string[] clipNames)
        {
            AddClips(clipNames, Loop);
        }

        public void SetOutro(params string[] clipNames)
        {
            AddClips(clipNames, Outro);
        }

        void AddClips(string[] clipNames, DynamicSong songyPart)
        {
            foreach (var name in clipNames)
            {
                if (GetTrackByName(name) != null)
                {
                    Debug.LogWarning($"You are trying to add the SongyClip named {name} twice. Dont do that plz. ");
                    continue;
                }
                songyPart.AddClip(FolderPath, name);
            }
        }

        public Dictionary<string, float> GetPlayingTracks()
        {
            var clips = GetAllClipsInSongy();
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
            var clips = GetAllClipsInSongy();
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
                Intro.PlayClipNow();
                Loop.PlayClipAfter(Intro);
                IsPlaying = true;
            }
            else if (Loop.HasClip())
            {
                Loop.PlayClipNow();
                IsPlaying = true;
            }
            else
            {
                IsPlaying = false;
            }
        }

        public bool KillSong()
        {
            if (Loop.IsPlaying && Loop.HasClip())
            {
                Loop.StopAndPlayNext(Outro);
                IsPlaying = false;
                return true;
            }

            return false;
        }

    }
}