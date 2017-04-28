using UnityEngine;
using UnityEngine.Events;

public class NotifySender : MonoBehaviour
{ 
    [SerializeField]
    private UnityEvent _notifyEvent;

    public void Notify()
    {
        _notifyEvent.Invoke();
    }
}
