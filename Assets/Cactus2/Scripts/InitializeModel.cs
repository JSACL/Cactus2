using System;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class InitializeModel : MonoBehaviour
{
    public string modelTypeName;
    //public LocalVisitor visitor;
    new public string tag;

    private void Start()
    {
        var modelT = Type.GetType(modelTypeName, throwOnError: true);
        if (!typeof(IEntity).IsAssignableFrom(modelT)) throw new ArgumentException("èäíËå^ÇÕé¿ç›ï®Ç≈ÇÕÇ†ÇËÇ‹ÇπÇÒÅB");
        var model = (IEntity)Activator.CreateInstance(modelT);
        //model.Visitor = visitor;
        model.TrySetTag(Tag.GetOrCreate(tag));
    }
}