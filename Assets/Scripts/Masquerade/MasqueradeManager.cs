using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasqueradeManager : MonoBehaviour
{
    public static List<Station> stations;

    public static Seat GetEmptySeatForWaiter()
    {
        if (!VacancyForWaiter()) return null;

        while (true)
        {
            Station randomStation = stations[Random.Range(0, stations.Count)];
            Seat randomSeat = randomStation.GetEmptySeatForWaiter();
            if (randomSeat != null) return randomSeat;
        }
    }
    public static Seat GetEmptySeatForMurderer()
    {
        if (!VacancyForMurderer()) return null;

        while (true)
        {
            Station randomStation = stations[Random.Range(0, stations.Count)];
            Seat randomSeat = randomStation.GetEmptySeatForMurderer();
            if (randomSeat != null) return randomSeat;
        }
    }
    public static Seat GetEmptySeatForMurderer(StationNames stationToOccupy)
    {
        foreach (Station station in stations)
        {
            if (station.stationName == stationToOccupy)
            {
                return station.GetEmptySeatForMurderer();
            }
        }
        return null;
    }

    private static bool Vacancy(int minNumberOfSeats = 1)
    {

        bool vacancy = false;
        foreach (Station station in stations)
        {
            if (station.numberOfFreeSeats >= minNumberOfSeats)
            {
                vacancy = true;
                break;
            }
        }
        return vacancy;
    }
    public static bool VacancyForWaiter()
    {
        return Vacancy(2);
    }
    public static bool VacancyForMurderer()
    {
        return Vacancy();
    }
}
