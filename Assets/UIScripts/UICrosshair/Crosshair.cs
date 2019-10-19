using UnityEngine;
using System.Collections;
using UnityEditor;

public class Crosshair : MonoBehaviour
{
    public static Crosshair Instance { get; private set; }
    public float height = 10f;
    public float width = 2f;
    public float defaultSpread = 10f;
    public Color color = Color.grey;
    public bool resizeable = true;
    public float resizedSpread = 20f;
    public float resizeSpeed = 3f;
    public float distantConst = 20f;
    [Header("Animation Settings")]
    public AnimationClip fadeInClip;
    public AnimationClip fadeOutClip;
    public Animation Anim;

    float spread;
    bool resizing = true;

    void Awake()
    {
        Instance = this;
        //set spread
        spread = defaultSpread;
        resizedSpread = defaultSpread;
    }

    //public void Pause()
    //{
    //    enableDrawing = false;
    //}

    //public void Resume()
    //{
    //    enableDrawing = true;
    //}
    bool isEnabled = false;
    private readonly object stateMutex = new object();
    /// <summary>
    /// 设定准心显示状态。
    /// </summary>
    /// <param name="flag">True，显示；False，隐藏</param>
    public static void SetState(bool flag)
    {
        lock (Instance.stateMutex)
        {
            if (flag && !Instance.isEnabled)
            {
                // enable
                Instance.Anim.clip = Instance.fadeInClip;
                Instance.color.a = 0.7f;
                Instance.Anim.Play();
                Instance.isEnabled = true;
            }
            else if(!flag && Instance.isEnabled)
            {
                // disable
                Instance.Anim.clip = Instance.fadeOutClip;
                Instance.color.a = 0f;
                Instance.Anim.Play();
                Instance.isEnabled = false;
            }
        }
    }

    public void SetDefaultAccuracy(float accuracy)
    {
        float angle = 100 - accuracy;
        float spread = distantConst * Mathf.Tan(angle * Mathf.Deg2Rad);
        defaultSpread = spread;
    }

    public void SetAccuracy(float accuracy)
    {
        float angle = 100 - accuracy;
        float spread = distantConst * Mathf.Tan(angle * Mathf.Deg2Rad) + defaultSpread;
        resizedSpread = spread;
    }

    void Update()
    {
        //for demonstration purposes
        //if (Input.GetMouseButton(0)) { resizing = true; } else { resizing = false; }
        
        if (resizeable)
        {
            if (resizing)
            {   
                //increase spread 
                spread = Mathf.Lerp(spread, resizedSpread, resizeSpeed * Time.deltaTime);
            }
            else
            {
                //decrease spread
                spread = Mathf.Lerp(spread, defaultSpread, resizeSpeed * Time.deltaTime);
            }

            //clamp spread
            //spread = Mathf.Clamp(spread, defaultSpread, resizedSpread);
        }
    }

    void OnGUI()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        //up rect
        GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - height / 2) + spread / 2, width, height), texture);

        //down rect
        GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - height / 2) - spread / 2, width, height), texture);

        //left rect
        GUI.DrawTexture(new Rect((Screen.width / 2 - height / 2) + spread / 2, Screen.height / 2 - width / 2, height, width), texture);

        //right rect
        GUI.DrawTexture(new Rect((Screen.width / 2 - height / 2) - spread / 2, Screen.height / 2 - width / 2, height, width), texture);
    }

    public void SetRisizing(bool state)
    {
        resizing = state;
    }
}
