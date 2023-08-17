using UnityEngine;
using static Utils;

public class EntityTransformViewBehaviour : MonoBehaviour
{
    [SerializeField]
    bool _isAggressive = false;

    public IEntity? Model { get; set; }

    private void Update()
    {
        Assert(Model is not null);

        if (_isAggressive)
        {
            Model.Position = transform.position;
            Model.Rotation = transform.rotation;
            if (Model is Entity e) 
            { 
                e.Velocity = Vector3.zero; 
                e.AngularVelocity = Vector3.zero;
            }
        }
        else
        {
            transform.position = Model.Position;
            transform.rotation = Model.Rotation;
        }
    }
}