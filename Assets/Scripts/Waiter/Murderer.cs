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
    }

    protected override void GetNextSeat()
    {
        if (findNextSeatCoroutine == null)
        {
            currentSequenceIndex++;
            currentSequenceIndex %= sequence.Count;
            findNextSeatCoroutine = StartCoroutine(GetNextSeatCoroutine());
        }
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
            yield return new WaitForSeconds(0.1f);
        }
        findNextSeatCoroutine = null;
    }


}
