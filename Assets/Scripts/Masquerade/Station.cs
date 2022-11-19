using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public StationNames stationName { get; private set; }
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
            while (true)
            {
                Seat randomSeat = seats[Random.Range(0, seats.Count)];
                if (randomSeat.isFree) return randomSeat;
            }
        }
        return null;
    }
}

public enum StationNames
{
    Vino,
    Platos,
    Ponche,
    Abrigos
}
