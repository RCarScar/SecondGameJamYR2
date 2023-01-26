
using System.Collections.Generic;
using UnityEngine;
using GGUtil;
using UnityEditor;

namespace Sounds
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public List<AudioClip> clips;
        public RangeF volume;
        public RangeF pitch;
        public bool Loop = false;

        [Space]

        public Transform Parent = null; //parent the audio source to this transform?

        public AudioClip GetRandomClip()
        {
            return clips[Random.Range(0, clips.Count)];
        }
    }

    public class SoundPlayer : MonoBehaviour
    {
        public List<Sound> sounds;

        /// <summary>
        /// Plays a sound at position
        /// </summary>
        public AudioSource PlaySound(string name, Vector3 position, float volumeMod = 1, float pitchMod = 1)
        {
            Sound sound = sounds.Find(s => s.name == name);
            return PlaySound(sound, position, volumeMod, pitchMod);
        }

        /// <summary>
        /// Plays a sound at position
        /// </summary>
        public AudioSource PlaySound(Sound sound, Vector3 position, float volumeMod = 1, float pitchMod = 1)
        {
            AudioClip clip = sound.GetRandomClip();
            GameObject obj = new();
            AudioSource source = obj.AddComponent<AudioSource>();

            obj.name = clip.name;

            source.clip = clip;
            source.pitch = Random.Range(sound.pitch.Min, sound.pitch.Max) * pitchMod;
            source.volume = Random.Range(sound.volume.Min, sound.volume.Max) * volumeMod;

            source.loop = sound.Loop;
            source.playOnAwake = false;

            if (!sound.Loop)
            {
                source.PlayOneShot(clip);
                Destroy(obj, clip.length);
            }
            else
            {
                source.Play();
            }

            source.transform.parent = sound.Parent;

            return source;
        }
    
    }

}

