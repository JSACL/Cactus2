#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(LocalVisitor))]
public class LocalVisitorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}

#endif