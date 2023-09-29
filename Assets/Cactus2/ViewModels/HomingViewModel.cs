#nullable enable
using UnityEngine;
using static Utils;

public class HomingViewModel : ViewModel<IHoming>
{
    [SerializeField]
    Transform _targetTransform;

    private void Start()
    {
        // TODO: Laycast hit.
    }

    private void Update()
    {
        Assert(Model is not null);

        Model.TargetCoordinate = _targetTransform.position;

        transform.SetPositionAndRotation(Model.Position, Model.Rotation);
    }
}