using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sprite : MonoBehaviour
{
    public Texture2D TextureAtlas;

    public Vector2 TextureOffset;
    public Vector2 TextureFrameSize;

    public bool InvertHorizontalUV = false;
    public bool InvertVerticalUV = false;

    public Vector2 ObjectSize;

    public int CurrentFrame = 0;

    public int TotalFrames = 0;

    public string PlayOnStart = "";

    
    [HideInInspector]
    public Material ImageMaterial;

    private int framesPerWidth = 0;

    private Mesh mesh;
    private Vector2[] uv = new Vector2[4];
    private Vector2 textureOffset = new Vector2();
    private int lastFrame = 0;

    private Dictionary<string, Animation> animations = new Dictionary<string,Animation>();
    private Animation currentAnimation; 

    // Use this for initialization
	void Awake()
    {
        CreateMesh(false);

        Animation[] anims = this.GetComponents<Animation>();

        foreach (Animation a in anims)
        {
            if (animations.ContainsKey(a.Name))
            {
                Debug.LogError("Animation name " + a.Name + " already exists!!");
            }
            else
            {
                animations[a.Name] = a;
            }
            a.enabled = false;
        }

        if (PlayOnStart != "")
        {
            PlayAnimation(PlayOnStart);
        }
        else
            SetFrame(CurrentFrame);
	}

    public void CreateMesh(bool editor)
    {
        MeshFilter meshFilter = this.gameObject.GetComponent<MeshFilter>();

        if (meshFilter == null)
            meshFilter = (MeshFilter)(this.gameObject.AddComponent(typeof(MeshFilter)));

        if (meshFilter.sharedMesh == null)
            meshFilter.sharedMesh = new Mesh();

        this.mesh = meshFilter.sharedMesh;

        MeshRenderer meshRenderer = this.gameObject.GetComponent<MeshRenderer>();

        if (meshRenderer == null)
            meshRenderer = (MeshRenderer)this.gameObject.AddComponent(typeof(MeshRenderer));

        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];

        ImageMaterial = new Material(Shader.Find("GUI/AlphaSelfIllum"));
        meshRenderer.material = ImageMaterial;

        if (TextureAtlas != null)
        {
            ImageMaterial.mainTexture = TextureAtlas;
            uv = GetUV(TextureOffset, TextureFrameSize);

            framesPerWidth = (int)(TextureAtlas.width / TextureFrameSize.x);

            if (TotalFrames == 0)
            {
                TotalFrames = framesPerWidth;
                TotalFrames *= (int)(TextureAtlas.height / TextureFrameSize.y);
            }
        }

        if (editor)
        {
            if (ObjectSize.x == 0.0f && ObjectSize.y == 0.0f)
            {
                ObjectSize = new Vector2(TextureFrameSize.x, TextureFrameSize.y);

                Debug.Log("Setting automatic scale to: " + TextureFrameSize.x + " - " + TextureFrameSize.y);
            }
        }

        vertices[0] = new Vector3(-ObjectSize.x, ObjectSize.y, 0);
        vertices[1] = new Vector3(ObjectSize.x, ObjectSize.y, 0);
        vertices[2] = new Vector3(-ObjectSize.x, -ObjectSize.y, 0);
        vertices[3] = new Vector3(ObjectSize.x, -ObjectSize.y, 0);

        int[] triangles = new int[6];

        triangles[2] = 2;
        triangles[1] = 1;
        triangles[0] = 0;
        triangles[5] = 3;
        triangles[4] = 1;
        triangles[3] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }


    protected Vector2[] GetUV(Vector2 textureOffset, Vector2 textureAtlasSize)
    {
        if (TextureAtlas != null)
        {
            if (textureAtlasSize.x == 0)
                textureAtlasSize.x = TextureAtlas.width;

            if (textureAtlasSize.y == 0)
                textureAtlasSize.y = TextureAtlas.height;

            if (InvertHorizontalUV)
            {
                uv[0].x = (textureOffset.x + textureAtlasSize.x) / TextureAtlas.width;
                uv[1].x = textureOffset.x / TextureAtlas.width;
                uv[2].x = (textureOffset.x + textureAtlasSize.x) / TextureAtlas.width;
                uv[3].x = textureOffset.x / TextureAtlas.width;
            }
            else 
            {
                uv[0].x = textureOffset.x / TextureAtlas.width;
                uv[1].x = (textureOffset.x + textureAtlasSize.x) / TextureAtlas.width;
                uv[2].x = textureOffset.x / TextureAtlas.width;
                uv[3].x = (textureOffset.x + textureAtlasSize.x) / TextureAtlas.width;
            }

            if (InvertVerticalUV)
            {
                uv[0].y = (TextureAtlas.height - (textureOffset.y + textureAtlasSize.y)) / TextureAtlas.height;
                uv[1].y = (TextureAtlas.height - (textureOffset.y + textureAtlasSize.y)) / TextureAtlas.height;
                uv[2].y = (TextureAtlas.height - textureOffset.y) / TextureAtlas.height;
                uv[3].y = (TextureAtlas.height - textureOffset.y) / TextureAtlas.height;
            }
            else
            {
                uv[0].y = (TextureAtlas.height - textureOffset.y) / TextureAtlas.height;
                uv[1].y = (TextureAtlas.height - textureOffset.y) / TextureAtlas.height;
                uv[2].y = (TextureAtlas.height - (textureOffset.y + textureAtlasSize.y)) / TextureAtlas.height;
                uv[3].y = (TextureAtlas.height - (textureOffset.y + textureAtlasSize.y)) / TextureAtlas.height;
            }
        }

        return uv;
    }

    public void SetFrame(int frame)
    {
        if (mesh != null && TextureAtlas != null && frame < TotalFrames)
        {
            CurrentFrame = frame % TotalFrames;

            float x = (float)(CurrentFrame % framesPerWidth);
            float y = (float)((int)(CurrentFrame / framesPerWidth)); 

            textureOffset.x = TextureOffset.x + TextureFrameSize.x * x;
            textureOffset.y = TextureOffset.y + TextureFrameSize.y * y;

            uv = GetUV(textureOffset, TextureFrameSize);

            mesh.uv = uv;
        }
    }

    public void PlayAnimation(string name)
    {
        if (animations.ContainsKey(name))
        {
            if (currentAnimation != null)
                currentAnimation.enabled = false;

            currentAnimation = animations[name];

            currentAnimation.enabled = true;
        }
    }

    public void StopCurrentAnimation()
    {
        if (currentAnimation != null)
            currentAnimation.enabled = false;

        currentAnimation = null;
    }

    public bool IsPlayingAnimation()
    {
        if (currentAnimation == null)
            return false;

        return (currentAnimation.enabled);
    }

    void OnDrawGizmos()
    {
        if (CurrentFrame != lastFrame)
        {
            SetFrame(CurrentFrame);

            lastFrame = CurrentFrame;
        }
    }
}
