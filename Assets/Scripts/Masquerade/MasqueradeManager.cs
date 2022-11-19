using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MasqueradeManager : Singleton<MasqueradeManager>
{

    [SerializeField]
    public List<Station> stations;

    public Seat GetEmptySeatForWaiter()
    {
        if (!VacancyForWaiter()) return null;

        while (true)
        {
            Station randomStation = stations[Random.Range(0, stations.Count)];
            Seat randomSeat = randomStation.GetEmptySeatForWaiter();
            if (randomSeat != null) return randomSeat;
        }
    }
    public Seat GetEmptySeatForMurderer()
    {
        if (!VacancyForMurderer()) return null;

        while (true)
        {
            Station randomStation = stations[Random.Range(0, stations.Count)];
            Seat randomSeat = randomStation.GetEmptySeatForMurderer();
            if (randomSeat != null) return randomSeat;
        }
    }
    public Seat GetEmptySeatForMurderer(StationName stationToOccupy)
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

    private bool Vacancy(int minNumberOfSeats = 1)
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
    public bool VacancyForWaiter()
    {
        return Vacancy(2);
    }
    public bool VacancyForMurderer()
    {
        return Vacancy();
    }
}
