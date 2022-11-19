using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour
{

    public static float maxSecondsToDelay;
    private Seat targetSeat;

    private void Start()
    {
        float secondsToDelay = Random.Range(0, maxSecondsToDelay);
        StartCoroutine(StartDelay(secondsToDelay));
    }

    IEnumerator StartDelay(float secondsToDelay)
    {
        yield return new WaitForSeconds(secondsToDelay);
        DelayedStart();
    }
    private void DelayedStart()
    {

    }

    private void GetNextSeat()
    {
        MasqueradeManager.GetEmptySeatForWaiter();
    }

}
