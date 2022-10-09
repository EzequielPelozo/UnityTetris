using UnityEngine;
using UnityEditor;
using System.Collections;

public class SpriteMenu : EditorWindow
{
    protected static SpriteMenu instance = null;

    private Sprite currentSprite;

    [MenuItem("Tools/Sprite/Create Sprite..")]
    public static void AddSprite()
    {
        GetReference().CreateSprite();
    }

    [MenuItem("Tools/Sprite/Add Animation..")]
    public static void AddAnimation()
    {
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.GetComponent<Sprite>() != null)
                GetReference().AddAnimation(Selection.activeGameObject);
        }
    }

    [MenuItem("Tools/Sprite/Add Animation...", true)]
    public static bool ValidateAddAnimation()
    {
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.GetComponent<Sprite>() != null)
                return true;
        }
        return false;
    }

    private static SpriteMenu GetReference()
    {
        if (instance == null)
        {
            instance = ScriptableObject.CreateInstance(typeof(SpriteMenu)) as SpriteMenu;
            instance.name = "SpriteMenu";
            instance.position = new Rect(10, 10, 500, 500);
        }
        return instance;
    }

    private void CreateSprite()
    {
        GameObject go = new GameObject();
        go.name = "Sprite";
        go.AddComponent<Sprite>();
    }

    private void AddAnimation(GameObject go)
    {
        go.AddComponent<Animation>();
    }

}
