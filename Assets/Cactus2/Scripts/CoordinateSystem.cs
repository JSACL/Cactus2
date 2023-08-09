using UnityEngine;

public class CoordinateSystem : MonoBehaviour
{
    [SerializeField]
    CoordinateSystemType _type;

    public CoordinateSystemType Type
    {
        get => _type;
        set => _type = value;
    }

    private void Update()
    {
        switch (_type)
        {
        case CoordinateSystemType.Global:
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            return;
        }
    }
}

public enum CoordinateSystemType
{
    Global,
}