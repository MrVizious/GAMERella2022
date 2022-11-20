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
    protected Animator animator;
    [ShowInInspector]
    protected Seat targetSeat
    {
        get { return _targetSeat; }
        set
        {
            if (hasArrivedToSeatCoroutine == null)
            {
                _targetSeat = value;
                if (_targetSeat != null)
                {
                    agent.SetDestination(targetSeat.transform.position);
                    hasArrivedToSeatCoroutine = StartCoroutine(HasArrivedToSeatCoroutine());
                }
                animator.Play("Walk");
            }
        }
    }
    private Seat _targetSeat;
    [ShowInInspector]
    protected Coroutine findNextSeatCoroutine = null;
    [ShowInInspector]
    protected Coroutine hasArrivedToSeatCoroutine = null;
    protected NavMeshAgent agent;
    private TriadicPaletteSO colorPalette;
    public Material featherMaterial;
    public Material maskMaterial;
    public Material jewelMaterial;

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        float secondsToDelay = Random.Range(0, maxSecondsToDelay);
        GenerateColors();
        StartCoroutine(StartDelay(secondsToDelay));
    }

    private void Update()
    {
        ApplyColors();
        if (Time.frameCount % 15 == 0 && Time.timeSinceLevelLoad >= maxSecondsToDelay)
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            string nameOfClip = clipInfo[0].clip.name;
            if (nameOfClip == "Walk" && hasArrivedToSeatCoroutine == null && findNextSeatCoroutine == null)
            {
                GetNextSeat();
            }
        }
    }

    private void ApplyColors()
    {
        feather.material = featherMaterial;
        mask.material = maskMaterial;
        jewels.material = jewelMaterial;
        mask.material.SetColor("_BaseColor", colorPalette.color1);
        feather.material.SetColor("_BaseColor", colorPalette.color2);
        jewels.material.SetColor("_BaseColor", colorPalette.color3);
    }

    private void GenerateColors()
    {
        colorPalette = ScriptableObject.CreateInstance(typeof(TriadicPaletteSO)) as TriadicPaletteSO;
        colorPalette.RandomizePallete();
        ApplyColors();
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

    public virtual void GetNextSeat()
    {
        if (findNextSeatCoroutine == null)
        {
            FreeSeat();
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
            yield return null;
        }
        findNextSeatCoroutine = null;
    }

    private IEnumerator HasArrivedToSeatCoroutine()
    {
        while (!HasArrivedToSeat())
        {
            yield return null;
        }
        OccupySeat();
        hasArrivedToSeatCoroutine = null;
    }

    protected virtual void OccupySeat()
    {
        targetSeat.Occupy(this);
        animator.Play(targetSeat.correctAnimationName);
    }
    protected void FreeSeat()
    {
        if (targetSeat == null) return;
        targetSeat.Free(this);
        targetSeat = null;
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
        if (agent.stoppingDistance >= Vector2.Distance(transform.position, targetSeat.transform.position))
        {
            return true;
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
