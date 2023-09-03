using System;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class InitializeModel : MonoBehaviour
{
    public string modelTypeName;
    public LocalVisitor visitor;
    public string teamName;

    private void Start()
    {
        var vm = GetComponent<ViewModel>();
        var modelT = Type.GetType(modelTypeName, throwOnError: true);
        //var modelIT = vm.ModelType;
        //if (modelT is null) throw new ArgumentException($"値不正。 {modelTypeName} 何。");
        //if (!modelIT.IsAssignableFrom(modelT)) throw new ArgumentException($"値不正。 {modelT} 非 {modelIT} 也。");
        var model = Activator.CreateInstance(modelT);
        
        vm.Model = model;
        visitor.AddStatic(model, vm);
        if (model is IVisible visible) visible.Visitor = visitor; // TODO: 要修正。ここでifが必要なのは初期設定によるモデルは抑々どこの客にも会わないことが原因。
        Referee.Current.Join(model, teamName);
    }
}