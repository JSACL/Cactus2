using Cactus2;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ColliderComponent : MonoBehaviour
{
    public event ColliderEventHandler Enter;
    public event ColliderEventHandler Stay;
    public event ColliderEventHandler Exit;

    private void OnCollisionEnter(Collision collision)
    {
        Enter?.Invoke(this, new(ColliderFlug.Enter, collision, 0));
    }

    private void OnCollisionStay(Collision collision)
    {
        Stay?.Invoke(this, new(ColliderFlug.Stay, collision, 0));
    }

    private void OnCollisionExit(Collision collision)
    {
        Exit?.Invoke(this, new(ColliderFlug.Exit, collision, 0));
    }
}