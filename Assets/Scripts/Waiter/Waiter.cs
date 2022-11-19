using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Waiter : MonoBehaviour
{

    [ShowInInspector]
    public static float maxSecondsToDelay;
    [ShowInInspector]
    protected Seat targetSeat;
    [ShowInInspector]
    protected Coroutine findNextSeatCoroutine = null;

    protected void Start()
    {
        float secondsToDelay = Random.Range(0, maxSecondsToDelay);
        StartCoroutine(StartDelay(secondsToDelay));
    }

    private IEnumerator StartDelay(float secondsToDelay)
    {
        yield return new WaitForSeconds(secondsToDelay);
        DelayedStart();
    }

    private void DelayedStart()
    {
        GetNextSeat();
    }

    protected virtual void GetNextSeat()
    {
        if (findNextSeatCoroutine == null)
        {
            findNextSeatCoroutine = StartCoroutine(GetNextSeatCoroutine());
        }
    }

    protected virtual IEnumerator GetNextSeatCoroutine()
    {
        while (targetSeat == null)
        {
            targetSeat = MasqueradeManager.Instance.GetEmptySeatForWaiter();
            yield return new WaitForSeconds(0.1f);
        }
        findNextSeatCoroutine = null;
    }

}
