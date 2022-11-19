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
        currentSequenceIndex = 0;
        base.Start();
    }

    protected void CreateSequence()
    {
        StationName[] allStationNames = (StationName[])Enum.GetValues(typeof(StationName));
        System.Random rnd = new System.Random();
        sequence = allStationNames.OrderBy(x => rnd.Next()).ToList();
        foreach (StationName station in sequence)
        {
            Debug.Log(station);
        }
    }

    protected override void GetNextSeat()
    {
        if (findNextSeatCoroutine == null)
        {
            Debug.Log("Getting next seat!");
            currentSequenceIndex++;
            currentSequenceIndex %= sequence.Count;
            findNextSeatCoroutine = StartCoroutine(GetNextSeatCoroutine());
        }
    }

    protected override IEnumerator GetNextSeatCoroutine()
    {
        while (targetSeat == null)
        {
            Debug.Log("Looking for empty seat");
            targetSeat = MasqueradeManager.Instance.GetEmptySeatForMurderer((StationName)currentSequenceIndex);
            yield return new WaitForSeconds(0.1f);
        }
        findNextSeatCoroutine = null;
    }


}
