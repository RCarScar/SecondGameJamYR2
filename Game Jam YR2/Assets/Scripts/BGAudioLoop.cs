using Sounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGAudioLoop : MonoBehaviour
{
    [SerializeField] private SoundPlayer player;
    [SerializeField] private AudioClip[] backgroundMusic;
    [SerializeField] private float timer = 0, randomInterval = 0;
    [SerializeField] private AudioClip stinger, heartbeat;
    [SerializeField] private Transform playerGO;
    //Audio source, Sound Effect Source.
    [SerializeField] private AudioSource aus, ses;
    void Start()
    {
        randomInterval = 15;
        if(backgroundMusic.Length == 0)
        {
            Debug.Log("ADD More Background Audio or Remove Script");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= randomInterval)
        {
            ses.clip = stinger;
            timer = 0;
            randomInterval = Random.Range(1, 40);
        }
        PlayAudio(Mathf.RoundToInt(Random.Range(0, backgroundMusic.Length)));
    }
    private void PlayAudio(int num)
    {
        if(aus.isPlaying)
        {
            return;
        }
        else
        {
            Debug.Log(num);
            aus.clip = backgroundMusic[num];
            aus.Play();
        }
    }
}
