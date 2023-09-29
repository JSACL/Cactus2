#nullable enable
using static Utils;
using UnityEngine;

public class WeaponViewModel : ViewModel<IFirer>
{
    void Update()
    {
        Assert(Model is not null);

        Model.AddTime(Time.deltaTime);
    }
}