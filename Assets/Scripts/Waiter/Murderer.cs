using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Murderer : Waiter
{
    private List<StationName> sequence;
    private int currentSequenceIndex = 0;

    protected new void Start()
    {
        CreateSequence();
        currentSequenceIndex = -1;
        base.Start();
    }

    protected void CreateSequence()
    {
        //StationName[] allStationNames = (StationName[])Enum.GetValues(typeof(StationName));
        HashSet<StationName> stationNames = new HashSet<StationName>();
        foreach (Station station in MasqueradeManager.Instance.stations)
        {
            stationNames.Add(station.stationName);
        }
        System.Random rnd = new System.Random();
        sequence = stationNames.OrderBy(x => rnd.Next()).ToList();
        foreach (StationName stationName in sequence)
        {
            Debug.Log(stationName);
        }
    }

    public override void GetNextSeat()
    {
        if (findNextSeatCoroutine == null)
        {
            FreeSeat();
            currentSequenceIndex++;
            currentSequenceIndex %= sequence.Count;
            findNextSeatCoroutine = StartCoroutine(GetNextSeatCoroutine());
        }
    }

    protected override void OccupySeat()
    {
        targetSeat.Occupy(this);
        animator.Play(targetSeat.wrongAnimationName);
    }

    protected override IEnumerator GetNextSeatCoroutine()
    {
        while (targetSeat == null)
        {
            Seat newSeat = MasqueradeManager.Instance.GetEmptySeatForMurderer(sequence[currentSequenceIndex]);
            if (newSeat != null)
            {
                newSeat.Reserve(this);
                targetSeat = newSeat;
            }
            yield return null;
        }
        findNextSeatCoroutine = null;
    }


}
