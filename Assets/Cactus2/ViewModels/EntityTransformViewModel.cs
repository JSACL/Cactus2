using UnityEngine;
using static Utils;

public class EntityTransformViewModel : ViewModel<IEntity>
{
    public bool match;
    public bool stop;

    private void Update()
    {
        if (match)
        {
            if (stop && Model is Entity e)
            {
                e.Velocity = Vector3.zero;
                e.AngularVelocity = Vector3.zero;
            }
            else
            {
                transform.position += Time.deltaTime * Model.Velocity;
                transform.Rotate(Time.deltaTime * Model.AngularVelocity);
            }
            Model.Position = transform.position;
            Model.Rotation = transform.rotation;
        }
        else
        {
            transform.position = Model.Position;
            transform.rotation = Model.Rotation;
        }
    }
}