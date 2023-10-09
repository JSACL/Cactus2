#nullable enable
using System.Collections.Generic;

namespace Cactus2.Presenter;
public class ItemListPresenter<TModel> : Presenter<TModel>, IFamilyPresenter where TModel : SysGC::ICollection<IItem>
{
    readonly List<ItemPresenter<IItem>> _children;

    public event FamilyChangeEventHandler? FamilyChanged;

    public ItemListPresenter()
    {
        _children = new();
    }

    protected override void Enable()
    {
        base.Enable();

        foreach (var item in Model)
        {
            var pter = new ItemPresenter<IItem>() { Model = item };
            _children.Add(pter);
            FamilyChanged?.Invoke(this, new(SceneChangeAction.Add, Typed.Get(pter)));
        }
    }

    protected override void Disable()
    {
        foreach (var item in _children)
        {
            FamilyChanged?.Invoke(this, new(SceneChangeAction.Remove, Typed.Get(item)));
        }
        _children.RemoveRange(0, _children.Count);

        base.Disable();
    }
}