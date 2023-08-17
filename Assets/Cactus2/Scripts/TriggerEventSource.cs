using UnityEngine;

public class TriggerEventSource : MonoBehaviour
{
    public event ColliderEventHandler Enter;
    public event ColliderEventHandler Stay;
    public event ColliderEventHandler Exit;

    private void OnTriggerEnter(Collider other)
    {
        Enter?.Invoke(this, new(ColliderFlug.Enter, other, 0));
    }

    private void OnTriggerStay(Collider other)
    {
        Stay?.Invoke(this, new(ColliderFlug.Stay, other, 0));
    }

    private void OnTriggerExit(Collider other)
    {
        Exit?.Invoke(this, new(ColliderFlug.Exit, other, 0));
    }
}