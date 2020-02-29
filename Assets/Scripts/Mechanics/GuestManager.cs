using SuperstarDJ.Audio;
using SuperstarDJ.DynamicMusic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GuestManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Guest> Guests;
   
    SpawnGuest guestSpawner;
    GameObject guestContainer;
    int plannedNewGuests = 0;

    #region variables
    float guestSpawnCooldownSpanInSeconds = 1f;
    DateTime guestSpawnCooldownTimer;
    int minAmountOfGuests = 1;
    int maxAmountOfGuests = 3;

    float danceFlorSize = 6f;

    float SatisfactionBoost = 0.05f;
    float SatisfactionDamage = 0.05f;
    #endregion

    void Start()
    {
        guestContainer = transform.root.Find("/Game/Guests").gameObject;
        guestSpawner = new SpawnGuest();

        InvokeRepeating("SlowUpdate", 0, 0.3f);

        ResetGuestCooldownTimer();
    }

    void SlowUpdate()
    {   
        Guests = GetAllGuests();
        HandleGuestAmount();
    }

    private void HandleGuestAmount()
    {
        var count = Guests.Count();

        if (maxAmountOfGuests > (count + plannedNewGuests))
        {
            if (DateTime.Now > guestSpawnCooldownTimer)
             {
                Invoke("AddGuest", new System.Random().Next(2, 6));
                plannedNewGuests++;
                ResetGuestCooldownTimer();
            }
        }
    }

    void ResetGuestCooldownTimer()
    {
        guestSpawnCooldownTimer = DateTime.Now.AddSeconds(guestSpawnCooldownSpanInSeconds);
    }

    List<Guest> GetAllGuests()
    {
        return guestContainer.GetComponentsInChildren<Guest>().ToList();
    }

    void AddGuest()
    {
        var randomSpawnPoint = UnityTools.GetRandomWithinBounds(new Bounds(guestContainer.transform.position,UnityTools.GetSymmetricalVector( danceFlorSize)));
        guestSpawner.SpawnGuests(1, guestContainer.transform, randomSpawnPoint);
        plannedNewGuests--;
    }


    // Update is called once per frame
    void Update()
    {
        foreach (var guest in Guests)
        {
            var favouriteTracks = guest.FavouriteTracks;
            var worstTracks = guest.DislikedTracks;
            var satisfactionMod = 0f;

            var activeFavTracks = new List<string>();
            var activeWorstTracks = new List<string>();

            // Save all active fav track and add satisfaction
            foreach (var favTrack in favouriteTracks)
            {
                if ( MusicManager.IsTrackPlaying ( favTrack ))
                {
                    satisfactionMod += SatisfactionBoost;
                    activeFavTracks.Add(favTrack);
                }
            }

            // Save all active worst track and reduce satisfaction
            foreach (var worstTrack in worstTracks)
            {
                if (MusicManager.IsTrackPlaying(worstTrack))
                {
                    satisfactionMod -= SatisfactionDamage;
                    activeWorstTracks.Add(worstTrack);
                }
            }

            // IF NOTHING IS PLAYING MOD SHOULD BE NEGATIVE
            if (MusicManager.TracksPlaying().Length == 0)
            {
                satisfactionMod = -SatisfactionDamage;
            }

            // APPLY SATISFACTION MOD
            guest.Satisfaction += satisfactionMod;

            // UPDATE GUEST UI WITH CURRENT ACTIVE TRACKS AND SATISFACTION
            guest.favTracksUI.text = activeFavTracks.Aggregate("", (current, next) => current + "\n " + next);
            guest.dislikedTracksUI.text = activeWorstTracks.Aggregate("", (current, next) => current + "\n " + next);
            guest.satisfactionUI.text = guest.Satisfaction.ToString("N0");

            // SET SATISFACTION UI COLOR BY SATISFACTION MOD
            if (satisfactionMod >= 0)
            {
                guest.satisfactionUI.color = Color.green;
            }
            else
            {
                guest.satisfactionUI.color = Color.red;
            }

            // CHECK IF FAIL OR SUCCESS
            if (guest.Satisfaction >= 100)
            {
                guest.OnSatisfiedSuccess();
            }
            if (guest.Satisfaction <= 0)
            {
                guest.OnSatisfiedFail();
            }
        }
    }
}
