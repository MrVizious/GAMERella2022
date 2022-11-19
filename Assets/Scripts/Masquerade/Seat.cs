using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Seat : MonoBehaviour
{
    public bool isFree
    {
        get
        {
            return !(isReserved || isBeingUsed);
        }
    }
    public string correctAnimationName, wrongAnimationName;
    private bool isReserved = false, isBeingUsed = false;
    private Waiter owner;

    [SerializeField]
    private List<GameObject> itemsToDisappear;

    public void Reserve(Waiter newWaiter)
    {
        if (!isFree)
        {
            Debug.LogError("Can't reserve because it is not free");
            return;
        }
        isReserved = true;
        owner = newWaiter;
        Debug.Log("Reserved");
    }

    public void Use(Waiter newWaiter)
    {
        if (newWaiter != owner)
        {
            Debug.LogError("A waiter that is not the owner is trying to use this seat!\nOwner: " + owner + "\nNew waiter: " + newWaiter, this);
            return;
        }
        DisappearItems();
        isReserved = false;
        isBeingUsed = true;
    }
    public void Free(Waiter newWaiter)
    {
        if (newWaiter != owner)
        {
            Debug.LogError("A waiter that is not the owner is trying to use this seat!\nOwner: " + owner + "\nNew waiter: " + newWaiter, this);
            return;
        }
        ReappearItems();
        isBeingUsed = false;
        isReserved = false;
    }

    private void DisappearItems()
    {
        foreach (GameObject item in itemsToDisappear)
        {
            item.SetActive(false);
        }
    }

    private void ReappearItems()
    {
        foreach (GameObject item in itemsToDisappear)
        {
            item.SetActive(true);
        }
    }

}
