using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Waiter : MonoBehaviour
{
    [ShowInInspector]
    public static float maxSecondsToDelay;
    protected Seat targetSeat
    {
        get { return _targetSeat; }
        set
        {
            _targetSeat = value;
            agent.SetDestination(targetSeat.transform.position);
        }
    }
    private Seat _targetSeat;
    [ShowInInspector]
    protected Coroutine findNextSeatCoroutine = null;
    protected NavMeshAgent agent;

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
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
