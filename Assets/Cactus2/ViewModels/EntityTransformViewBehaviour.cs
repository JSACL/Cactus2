using UnityEngine;
using static Utils;

public class EntityTransformViewBehaviour : MonoBehaviour
{
    [SerializeField]
    bool _isAggressive = false;

    public Entity? Model { get; set; }

    private void Update()
    {
        Assert(Model is not null);

        if (_isAggressive)
        {
            Model.Position = transform.position;
            Model.Rotation = transform.rotation;
            Model.Velocity = Vector3.zero; 
            Model.AngularVelocity = Vector3.zero;
        }
        else
        {
            transform.position = Model.Position;
            transform.rotation = Model.Rotation;
        }
    }
}