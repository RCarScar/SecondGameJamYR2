/* By Ryan Chen */
using Sounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGAudioLoop : MonoBehaviour
{
    [SerializeField] private SoundPlayer player;
    [SerializeField] private AudioClip[] backgroundMusic;
    [SerializeField] private float timer = 0, timer2 = 0, randomInterval = 0, footstepsFrequency = 1;
    [SerializeField] private AudioClip stinger, heartbeat, footsteps;
    [SerializeField] private Transform playerGO;
    [SerializeField] private Rigidbody2D playerRB;
    //Audio source, Sound Effect Source.
    [SerializeField] private AudioSource aus, ses, fss;
    void Start()
    {
        playerRB = PlayerController.Instance.GetComponent<Rigidbody2D>();
        randomInterval = 15;
        if(backgroundMusic.Length == 0)
        {
            Debug.Log("ADD More Background Audio or Remove Script");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        timer2 += Time.deltaTime;
        if(Mathf.Abs(playerRB.velocity.x) > 0 && timer2 > footstepsFrequency && Mathf.Abs(playerRB.velocity.y) < 0.01f)
        {
            timer2 = 0;
            fss.clip = footsteps;
            fss.pitch = Mathf.Abs(playerRB.velocity.x/10);
            fss.Play();
        }

        if(Mathf.Abs(playerRB.velocity.y) > 0.01f)
        {
            fss.Stop();
        }

        if(timer >= randomInterval)
        {
            ses.clip = stinger;
            ses.Play();
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
