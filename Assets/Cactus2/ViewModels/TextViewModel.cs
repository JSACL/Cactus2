using TMPro;
using UnityEngine;

public class TextViewModel : ViewModel<object>
{
    public TextMeshPro text;

    protected void Update()
    {
        text.text = Model.ToString();
    }
}
