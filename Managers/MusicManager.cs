using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioClip[] music;

    [SerializeField] AudioSource source;

    List<AudioClip> playlist = new List<AudioClip>(), cliplist = new List<AudioClip>();

    private static readonly System.Random rng = new System.Random();

    [SerializeField] bool playing, fadeOut;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < music.Length; i++)
        {
            playlist.Add(music[i]);
        }

        MakePlaylist();
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying && playing)
        {
            NewSong();
        }

        if (fadeOut)
        {
            source.volume = Mathf.Clamp(Mathf.Lerp(source.volume, source.volume - (Time.deltaTime * 100 / source.clip.length), Time.deltaTime), 0.5f, 1.0f);
        }
        else
        {
            source.volume = 1;
        }
    }

    void MakePlaylist()
    {
        int n = playlist.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            AudioClip clip = playlist[k];
            playlist[k] = playlist[n];
            playlist[n] = clip;
        }

        foreach(AudioClip clip in playlist)
        {
            int repeats = UnityEngine.Random.Range(0.0f, 1.0f) > 0.4f ? 2 : 4;

            for(int i = 0; i < repeats; i++)
            {
                cliplist.Add(clip);
            }
        }
    }

    public void NewSong()
    {
        if(cliplist.Count == 0)
        {
            MakePlaylist();
        }
        
        source.clip = cliplist[0];
        source.volume = 1;
        source.Play();
        playing = true;

        fadeOut = cliplist[0] != cliplist[1];

        cliplist.RemoveAt(0);
    }

    public void StopPlaying()
    {
        source.Stop();
        playing = false;
    }
}
