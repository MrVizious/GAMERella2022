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
    [SerializeField]
    private MeshRenderer mask, jewels, feather;
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
        GenerateColors();
        StartCoroutine(StartDelay(secondsToDelay));
    }

    private void GenerateColors()
    {
        TriadicPaletteSO newColorPallete = ScriptableObject.CreateInstance(typeof(TriadicPaletteSO)) as TriadicPaletteSO;
        newColorPallete.RandomizePallete();
        mask.material.color = newColorPallete.color1;
        feather.material.color = newColorPallete.color2;
        jewels.material.color = newColorPallete.color3;

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
