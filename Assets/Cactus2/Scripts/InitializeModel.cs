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
        //if (modelT is null) throw new ArgumentException($"�l�s���B {modelTypeName} ���B");
        //if (!modelIT.IsAssignableFrom(modelT)) throw new ArgumentException($"�l�s���B {modelT} �� {modelIT} ��B");
        var model = Activator.CreateInstance(modelT);
        
        vm.Model = model;
        visitor.AddStatic(model, vm);
        if (model is IVisible visible) visible.Visitor = visitor; // TODO: �v�C���B������if���K�v�Ȃ̂͏����ݒ�ɂ�郂�f���͗}�X�ǂ��̋q�ɂ����Ȃ����Ƃ������B
        Referee.Current.Join(model, teamName);
    }
}