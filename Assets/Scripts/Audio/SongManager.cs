using DG.Tweening;
using SuperstarDJ.Audio.Enums;
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
    class SongManager
    {

        float pitchChangeSpeed = 0.5f;

        List<Track> Tracks;
        public bool IsPlaying
        {
            get
            {
                return Tracks.Any(t => t.IsPlaying);
            }
        }
        static double paddingTime = 0.1;
        public double Duration
        {
            get
            {
                return Tracks[0].Duration;
            }
        }
        internal double ScheduledToStartAt { get; private set; }
        //  internal double EndsPlayingDspTime { get; set; }
        public SongManager(List<Track> tracks) => Tracks = tracks; //CTOR
        void StartSong(List<TrackNames> playsAtStart)
        {
            var dspTime = GetDspTime() + paddingTime;
            ScheduledToStartAt = dspTime;

            foreach (var track in Tracks)
            {
                track.Source().Stop();

                if (playsAtStart.Contains(track.TrackName))
                {
                    StartTrack(track);
                }
            }
        }
        private static double GetDspTime(bool withPadding = false)
        {
            var padding = withPadding == true ? paddingTime : 0;
            return AudioSettings.dspTime + padding;
        }
        private static void StartTrack(Track track, int startFromSamplePosition = 0)
        {
            track.Source().timeSamples = startFromSamplePosition;
            track.Source().volume = track.VolumeModification;
            track.Source().PlayScheduled(GetDspTime(true));
        }

        void StopSong()
        {
            foreach (var track in Tracks)
            {
                track.Source().Stop();
            }
        }
        public TrackNames[] TracksPlaying()
        {
            return Tracks.Where(t => t.Source().isPlaying && t.Source().volume > 0f).Select(t => t.TrackName).ToArray();

        }

        public int GetCurrentSamplePositionOfSong()
        {
            var referenceTrack = Tracks.First(t => t.IsPlaying == true);

            if (referenceTrack == null) { return 0; }
            return referenceTrack.Source().timeSamples;
        }

        internal void SlowPitch()
        {
            if (Tracks[0].Source().pitch > -2f)
            {
                Tracks.ForEach(t => t.Source().pitch -= pitchChangeSpeed);
            }
            else
            {
                FasterPitch();
            }
        }

        internal void FasterPitch()
        {
            if (Tracks[0].Source().pitch < 4f)
            {
                Tracks.ForEach(t => t.Source().pitch += pitchChangeSpeed);
            }
            else
            {
                SlowPitch();
            }
        }


        internal void ScratchPitch()
        {
            var duration = 0.25f;
            var pitch = Tracks[0].Source().pitch;
            DOTween.To(() => pitch, x => pitch = x, -2f, duration / 2).SetEase(Ease.OutSine)
                .OnUpdate(() => {

                    Debug.Log(pitch);
                    Tracks.ForEach(t => t.Source().pitch = pitch);
                })

                .OnComplete(() =>

                                 DOTween.To(() => pitch, x => pitch = x, 1f, duration / 2).SetEase(Ease.InSine)
                                .OnUpdate(() => {

                                    Debug.Log(pitch);
                                    Tracks.ForEach(t => t.Source().pitch =  pitch);
                                })

                );


        }

        internal void UpdatePitch()
        {

            if (Tracks[0].Source().pitch == 1)
            {
                return;
            }

            if (Tracks[0].Source().pitch < 1)
            {
                Tracks.ForEach(t => t.Source().pitch += pitchChangeSpeed * 2);
                if (Tracks[0].Source().pitch > 1)
                {
                    Tracks.ForEach(t => t.Source().pitch = 1f);
                }
            }

            if (Tracks[0].Source().pitch > 1)
            {
                Tracks.ForEach(t => t.Source().pitch -= pitchChangeSpeed * 2);
                if (Tracks[0].Source().pitch < 1)
                {
                    Tracks.ForEach(t => t.Source().pitch = 1f);
                }
            }

        }

        //public double GetCurrentSamplePositionOfSong()
        //{
        //    var referenceTrack = Tracks.First ( t => t.IsPlaying == true );

        //    if ( referenceTrack == null ) { return 0; }

        //    var timeSamples = ( double )referenceTrack.Source ().timeSamples;
        //    var frequency = referenceTrack.Source ().clip.frequency;
        //    var currentPosition = timeSamples / frequency;

        //    return currentPosition;
        //}
        public bool IsTrackPlaying(string trackName)
        {
            var _trackName = UnityTools.TrackNameFromString(trackName);
            return Tracks.Any(t => t.TrackName == _trackName && t.IsPlaying);

        }
        public void DebugSound(TrackNames trackName, int playAtSample = 0, float volume = 1f)
        {
            var track = GetTrackByName(trackName);
            track.Source().timeSamples = 100000;
            track.Source().volume = track.VolumeModification;
            track.Source().PlayScheduled(GetDspTime() + paddingTime);

        }
        public List<Track> GetPlayingTracks()
        {
            return Tracks.Where(t => t.IsPlaying == true).ToList();
        }

        public void StopTrack(TrackNames name)
        {
            GetTrackByName(name).Source().Stop();
        }
        public void PlayTrack(TrackNames name)
        {
            if (GetPlayingTracks().Count() < 1)
            {
                StartSong(new List<TrackNames>() { name });
            }

            var track = GetTrackByName(name);
            StartTrack(track, GetCurrentSamplePositionOfSong());
        }
        public Track GetTrackByName(TrackNames name)
        {
            return Tracks.First(t => t.TrackName == name);
        }
    }
}