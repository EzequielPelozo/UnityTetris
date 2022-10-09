using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Sprite))]
public class Animation : MonoBehaviour
{
    public enum LoopType
    {
        None,
        Loop, 
        PingPong
    }

    public int FromFrame = 0;
    public int ToFrame = 0;

    public int FPS = 30;

    public string Name = "Animation";

    public LoopType Loop = LoopType.None;

    public int[] Frames;

    private Sprite sprite;
    private float deltaT;
    private float lastTime;
    private int currentId = 0;
    private int factor = 1;

    void Awake()
    {
        sprite = this.GetComponent<Sprite>();

        if (FromFrame != 0 || ToFrame != 0)
        {
            Frames = new int[Mathf.Abs(ToFrame - FromFrame) + 1];
            
            int id = 0;

            if (FromFrame < ToFrame)
            {
                for (int i = FromFrame; i <= ToFrame; i++)
                {
                    Frames[id++] = i;
                }
            }
            else
            {
                for (int i = FromFrame; i >= ToFrame; i--)
                {
                    Frames[id++] = i;
                }
            }
        }

        if (FromFrame > ToFrame)
            factor = -1;

        deltaT = 1.0f / FPS;
    }

    void OnEnable()
    {
        currentId = 0;

        if (FromFrame > ToFrame)
            factor = -1;
        else
            factor = 1;
    }

    void FixedUpdate()
    {
        if (Time.time - lastTime >= deltaT)
        {
            lastTime = Time.time;

            if (currentId < 0 || currentId >= Frames.Length)
            {
                if (Loop == LoopType.Loop)
                {
                    currentId %= Frames.Length;
                }
                else if (Loop == LoopType.PingPong)
                {
                    factor *= -1;
                    if (factor < 0 && Frames.Length > 1)
                    {
                        currentId = Frames.Length - 2;
                    }
                    else if (factor > 0 && Frames.Length - 1 >= 1)
                    {
                        currentId = 1;
                    }
                    else
                    {
                        currentId = 0;
                    }
                }
                else
                {
                    this.enabled = false;
                    return;
                }
            }

            sprite.SetFrame(Frames[currentId]);

            currentId += factor;
        }
    }

    public bool IsPlaying()
    {
        return this.enabled;
    }
}