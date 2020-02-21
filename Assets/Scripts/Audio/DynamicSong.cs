using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace SuperstarDJ.DynamicMusic
{

    /// <summary>
    /// Clas Nested in SongySong class
    /// </summary>
    class DynamicSong : MonoBehaviour
    {
        public string TrackName { get; set; }
        Dictionary<string, DynamicSongTrack> clips;
        
        bool isPlaying;
        public bool IsPlaying { get { return isPlaying; } }
        double paddingTime = 0.01;
        bool isScheduledToStart;
        internal double ScheduledToStartAt { get; private set; }
        internal double EndsPlayingDspTime { get; set; }

        public bool HasClip()
        {
            return clips != null && clips.Count() > 1;
        }
        public Dictionary<string, DynamicSongTrack> GetClipsDic()
        {
            return clips;
        }

        public AudioSource GetSongyClipSource(string songyclipName)
        {
            return clips.FirstOrDefault(kvp => kvp.Key == songyclipName).Value.source;
        }

        public DynamicSong AddClip(string folderPath, string clipName)
        {
            if (clips == null)
            {
                clips = new Dictionary<string, DynamicSongTrack>();
            }

            if (clips.ContainsKey(clipName))
            {
                throw new Exception("SongyPart already has a clip called " + clipName);
            }

            var path = $"{folderPath}\\{clipName}";
            var clip = Resources.Load<AudioClip>(path);

            if (clip == null)
            {
                throw new Exception($"AudioClip does not exist at given path: {path}");
            }

            var songyClip = new DynamicSongTrack(gameObject.AddComponent<AudioSource>(), clip, clipName);
            clips.Add(clipName, songyClip);

            return this;
        }

        DynamicSongTrack FirstClip()
        {
            return clips.ElementAt(0).Value;
        }

        public void PlayClipNow()
        {
            SchedulePlay(CurrentDspTime() + paddingTime);
        }

        void SchedulePlay(double dspTime)
        {
            var firstClip = FirstClip();
            var firstSource = firstClip.source;

            ScheduledToStartAt = dspTime; ;
            isScheduledToStart = true;
            EndsPlayingDspTime = ScheduledToStartAt + firstClip.Duration;

            foreach (var source in clips.Values.Select(c => c.source))
            {   
                source.PlayScheduled(dspTime);
            }
        }

        public void PlayClipAfter(DynamicSong songPart)
        {
            var dspTime = songPart.EndsPlayingDspTime;
            SchedulePlay(dspTime);
        }

        public void Update()
        {
            if (CurrentDspTime() >= EndsPlayingDspTime && IsPlaying == true)
            {
                isPlaying = false;
                isScheduledToStart = false;
                print(TrackName + " STOPPED PLAYING  at " + CurrentDspTime());
                print("TimeSamples when stopped  = " + FirstClip().source.timeSamples);

            }

            if (CurrentDspTime() >= ScheduledToStartAt && isScheduledToStart == true && IsPlaying == false)
            {
                print(TrackName + " STARTS PLAYING  at " + CurrentDspTime());
                isPlaying = true;
            }
        }

        public void StopAndPlayNext(DynamicSong nextSongPart)
        {
            Stop();
            nextSongPart.SchedulePlay(EndsPlayingDspTime);
        }

        public void Stop()
        {
            EndsPlayingDspTime = CurrentDspTime() + paddingTime;

            foreach (var source in clips.Values.Select(c => c.source))
            {
                source.SetScheduledEndTime(EndsPlayingDspTime);
            }
        }

        private double CurrentDspTime()
        {
            return AudioSettings.dspTime;
        }
    }
}