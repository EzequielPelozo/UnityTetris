using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Sprite))]
public class SpriteEditor : Editor
{
    private Sprite sprite;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        sprite = ((Sprite)target);

        GUILayout.BeginVertical("Button");
        {
            GUILayout.Label("Actions");

            if (GUILayout.Button("Create"))
            {
                sprite.CreateMesh(true);
            }
        }

        GUILayout.EndVertical();
    }
}
