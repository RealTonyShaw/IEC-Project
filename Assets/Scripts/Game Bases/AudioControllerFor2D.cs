using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControllerFor2D : MonoBehaviour
{
    public static AudioControllerFor2D Instance { get; private set; }

    private readonly object listMutex = new object();
    public GameObject EmptyAudioSource;
    public AudioSource[] defaultSources;
    private int srcCnt = 0;
    private Cell[] cells = new Cell[GameDB.MAX_2D_AUDIO_SOURCES_NUM];
    private class Cell
    {
        public Cell(AudioSource src)
        {
            this.src = src;
        }
        public bool isOccupied = false;
        public readonly AudioSource src;
        public readonly object mutex = new object();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        lock (listMutex)
        {
            for (int i = 0; i < GameDB.MAX_2D_AUDIO_SOURCES_NUM; i++)
            {
                if (i < defaultSources.Length)
                {
                    srcCnt++;
                    cells[i] = new Cell(defaultSources[i]);
                }
                else
                {
                    cells[i] = null;
                }
            }
        }
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    public void Play2DAudio(AudioClip clip)
    {
        Play2DAudio(clip, 1f, 1f);
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    /// <param name="vol">音量</param>
    public void Play2DAudio(AudioClip clip, float vol)
    {
        Play2DAudio(clip, vol, 1f);
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    /// <param name="vol">音量</param>
    /// <param name="pitch">播放速度，取值(0,2]</param>
    public void Play2DAudio(AudioClip clip, float vol, float pitch)
    {
        for (int i = 0; i < GameDB.MAX_2D_AUDIO_SOURCES_NUM; i++)
        {
            if (cells[i] == null)
            {
                cells[i] = new Cell(Instantiate(EmptyAudioSource, transform).GetComponent<AudioSource>());
            }
            lock (cells[i].mutex)
            {
                if (!cells[i].isOccupied)
                {
                    cells[i].isOccupied = true;
                    AudioSource src = cells[i].src;
                    src.clip = clip;
                    src.volume = vol;
                    src.pitch = pitch;
                    src.Play();
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    /// <param name="vol">音量</param>
    /// <param name="pitch">播放速度，取值(0,2]</param>
    /// <param name="beginTime">开始播放的时刻(相对声音剪辑)</param>
    /// <param name="endTime">停止播放的时刻(相对声音剪辑)</param>
    public void Play2DAudio(AudioClip clip, float vol, float pitch, float beginTime, float endTime)
    {
        for (int i = 0; i < GameDB.MAX_2D_AUDIO_SOURCES_NUM; i++)
        {
            if (cells[i] == null)
            {
                cells[i] = new Cell(Instantiate(EmptyAudioSource, transform).GetComponent<AudioSource>());
            }
            lock (cells[i].mutex)
            {
                if (!cells[i].isOccupied)
                {
                    cells[i].isOccupied = true;
                    AudioSource src = cells[i].src;
                    src.clip = clip;
                    src.volume = vol;
                    src.pitch = pitch;
                    src.time = beginTime;
                    src.Play();
                    StartCoroutine(AutoStop(i, (endTime - beginTime) / pitch));
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    /// <param name="vol">音量</param>
    /// <param name="pitch">播放速度，取值(0,2]</param>
    /// <param name="beginTime">开始播放的时刻(相对声音剪辑)</param>
    /// <param name="endTime">停止播放的时刻(相对声音剪辑)</param>
    /// <param name="startDelay">播放延时</param>
    public void Play2DAudio(AudioClip clip, float vol, float pitch, float beginTime, float endTime, float startDelay)
    {
        for (int i = 0; i < GameDB.MAX_2D_AUDIO_SOURCES_NUM; i++)
        {
            if (cells[i] == null)
            {
                cells[i] = new Cell(Instantiate(EmptyAudioSource, transform).GetComponent<AudioSource>());
            }
            lock (cells[i].mutex)
            {
                if (!cells[i].isOccupied)
                {
                    cells[i].isOccupied = true;
                    AudioSource src = cells[i].src;
                    src.clip = clip;
                    src.volume = vol;
                    src.pitch = pitch;
                    src.time = beginTime;
                    src.PlayDelayed(startDelay);
                    StartCoroutine(AutoStop(i, (endTime - beginTime + startDelay) / pitch));
                    break;
                }
            }
        }
    }

    IEnumerator AutoStop(int srcIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        lock (cells[srcIndex].mutex)
        {
            cells[srcIndex].isOccupied = false;
            cells[srcIndex].src.Stop();
        }
    }
}

public partial class GameDB
{
    public const int MAX_2D_AUDIO_SOURCES_NUM = 10;

}

public partial class Gamef
{
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    public static void Play2DAudio(AudioClip clip)
    {
        AudioControllerFor2D.Instance.Play2DAudio(clip, 1f, 1f);
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    /// <param name="vol">音量</param>
    public static void Play2DAudio(AudioClip clip, float vol)
    {
        AudioControllerFor2D.Instance.Play2DAudio(clip, vol, 1f);
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    /// <param name="vol">音量</param>
    /// <param name="pitch">播放速度，取值(0,2]</param>
    public static void Play2DAudio(AudioClip clip, float vol, float pitch)
    {
        AudioControllerFor2D.Instance.Play2DAudio(clip, vol, pitch);
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    /// <param name="vol">音量</param>
    /// <param name="pitch">播放速度，取值(0,2]</param>
    /// <param name="beginTime">开始播放的时刻(相对声音剪辑)</param>
    /// <param name="endTime">停止播放的时刻(相对声音剪辑)</param>
    public static void Play2DAudio(AudioClip clip, float vol, float pitch, float beginTime, float endTime)
    {
        AudioControllerFor2D.Instance.Play2DAudio(clip, vol, pitch, beginTime, endTime);
    }
    /// <summary>
    /// 播放2D声效。
    /// </summary>
    /// <param name="clip">声音剪辑</param>
    /// <param name="vol">音量</param>
    /// <param name="pitch">播放速度，取值(0,2]</param>
    /// <param name="beginTime">开始播放的时刻(相对声音剪辑)</param>
    /// <param name="endTime">停止播放的时刻(相对声音剪辑)</param>
    /// <param name="startDelay">播放延时</param>
    public static void Play2DAudio(AudioClip clip, float vol, float pitch, float beginTime, float endTime, float startDelay)
    {
        AudioControllerFor2D.Instance.Play2DAudio(clip, vol, pitch, beginTime, endTime, startDelay);
    }
}
