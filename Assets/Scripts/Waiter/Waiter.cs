using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Waiter : MonoBehaviour
{
    [SerializeField]
    private float maxSecondsToDelay;
    //[SerializeField]
    //private SpriteRenderer mask, jewels, feather;
    protected Seat targetSeat
    {
        get { return _targetSeat; }
        set
        {
            if (hasArrivedToSeatCoroutine == null)
            {
                _targetSeat = value;
                agent.SetDestination(targetSeat.transform.position);
                hasArrivedToSeatCoroutine = StartCoroutine(HasArrivedToSeatCoroutine());
            }
        }
    }
    private Seat _targetSeat;
    protected Coroutine findNextSeatCoroutine = null;
    protected Coroutine hasArrivedToSeatCoroutine = null;
    protected NavMeshAgent agent;

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        float secondsToDelay = Random.Range(0, maxSecondsToDelay);
        StartCoroutine(StartDelay(secondsToDelay));
    }

    private void GenerateColors()
    {
        TriadicPaletteSO newColorPallete = ScriptableObject.CreateInstance(typeof(TriadicPaletteSO)) as TriadicPaletteSO;

        //TODO
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
            Seat newSeat = MasqueradeManager.Instance.GetEmptySeatForWaiter();
            if (newSeat != null)
            {
                newSeat.Reserve(this);
                targetSeat = newSeat;
            }
            yield return new WaitForSeconds(0.1f);
        }
        findNextSeatCoroutine = null;
    }

    private IEnumerator HasArrivedToSeatCoroutine()
    {
        while (!HasArrivedToSeat())
        {
            yield return new WaitForSeconds(0.05f);
        }
        targetSeat.Occupy(this);
        hasArrivedToSeatCoroutine = null;
    }

    private bool HasArrivedToSeat()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Has arrived!");
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDestroy()
    {
        if (targetSeat != null)
        {
            targetSeat.Free(this);
        }
    }

}
