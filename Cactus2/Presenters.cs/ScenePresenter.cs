//#nullable enable
//using System.ComponentModel;
//using Nonno.Assets;
//using Nonno.Assets.Presentation;

//public class ScenePresenter : IPresenter<IScene>
//{
//    readonly IndexedDynamicDispatcher _addDispatcher;
//    readonly IndexedDynamicDispatcher _removeDispatcher;
//    IScene? _model;

//    public ScenePresenter(ISceneViewModel sceneViewModel)
//    {
//        _viewModel = sceneViewModel;
//        _addDispatcher = new();
//        _removeDispatcher = new();

//        _addDispatcher.Overload<IEntity>(m => _viewModel.);
//        _removeDispatcher.Overload<IEntity>(m =>
//        {

//        });
//    }

//    public IScene? Model 
//    {
//        set 
//        {
//            if (_model is not null) _model.FamilyChanged -= SceneChanged;
//            _model = value;
//            if (_model is not null) _model.FamilyChanged += SceneChanged;
//        }
//    }

//    private void SceneChanged(object? sender, FamilyChangeEventArgs e)
//    {
//        switch (e.Action)
//        {
//        case SceneChangeAction.Add:
//            _addDispatcher.Dispatch(e.Object);
//            break;
//        case SceneChangeAction.Remove:
//            _removeDispatcher.Dispatch(e.Object);
//            break;
//        }
//    }
//}