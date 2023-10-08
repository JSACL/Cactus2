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
                e.Velocity = default;
            }
            else
            {
                transform.position += Time.deltaTime * Model.Velocity.Linear.ToUnityVector3();
                transform.Rotate(Time.deltaTime * Model.Velocity.Angular.ToUnityVector3());
            }
            Model.Transform = transform.ToTransform();
        }
        else
        {
            transform.Set(Model.Transform);
        }
    }
}