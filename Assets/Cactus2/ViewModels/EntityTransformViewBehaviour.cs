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
        }
        else
        {
            transform.position = Model.Position;
            transform.rotation = Model.Rotation;
        }
    }
}