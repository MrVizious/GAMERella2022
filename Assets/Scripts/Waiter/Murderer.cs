using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Murderer : Waiter
{
    private List<StationNames> sequence;


    private void CreateSequence()
    {
        StationNames[] allStationNames = (StationNames[])Enum.GetValues(typeof(StationNames));
        System.Random rnd = new System.Random();
        sequence = allStationNames.OrderBy(x => rnd.Next()).ToList();
    }

    private void GetNextSeat()
    {

    }
}
