using TMPro;
using UnityEngine;

public class SceneViewModel : ViewModel<IScene>
{
    [SerializeField]
    TextMeshPro _text;

    private void Update()
    {
        _text.text = 
            $"Time: {Model.Time}" +
            $"That's all.";
    }
}