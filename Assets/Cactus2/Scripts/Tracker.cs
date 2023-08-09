using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField]
    GameObject _target;

    private void Update()
    {
        transform.position = _target.transform.position - _target.transform.forward + new Vector3(0, 01, 0);
        transform.forward = _target.transform.forward;
    }
}