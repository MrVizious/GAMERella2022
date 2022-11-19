using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Station : MonoBehaviour
{
    public StationName stationName;
    public List<Seat> seats;
    public int numberOfFreeSeats
    {
        get
        {
            int count = 0;
            foreach (Seat seat in seats)
            {
                if (seat.isFree) count++;
            }
            return count;
        }
    }

    public Seat GetEmptySeatForWaiter()
    {
        if (numberOfFreeSeats >= 2)
        {
            return GetEmptySeat();
        }
        return null;
    }

    public Seat GetEmptySeatForMurderer()
    {
        return GetEmptySeat();
    }

    private Seat GetEmptySeat()
    {
        if (numberOfFreeSeats > 0)
        {
            Debug.Log("There are free seats here!", this);
            while (true)
            {
                Seat randomSeat = seats[Random.Range(0, seats.Count)];
                if (randomSeat.isFree)
                {
                    Debug.Log("Found seat!");
                    return randomSeat;
                }
            }
        }
        return null;
    }
}

public enum StationName
{
    Vino,
    Platos,
    Ponche,
    Abrigos
}
