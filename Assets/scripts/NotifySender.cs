using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NotifySender : MonoBehaviour
{ 
    [SerializeField]
    private UnityEvent notifyEvent;

    public void Notify()
    {
        notifyEvent.Invoke();
    }
}
