using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup soundEffectsMixerGroup;
    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private List<Sound> sounds;

    [HideInInspector] public Sound trackPlaying;
    private bool isTrackPlaying;

    private Queue<IEnumerator> coroutineQueue = new Queue<IEnumerator>();
    private bool coroutineRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            //DontDestroyOnLoad(gameObject);
            Instance = this;
            Instance.trackPlaying = null;
            isTrackPlaying = false;


            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.audioClip;
                s.source.loop = s.isLoop;
                s.source.volume = s.volume;

                switch (s.audioType)
                {
                    case Sound.AudioTypes.soundEffect:
                        s.source.outputAudioMixerGroup = soundEffectsMixerGroup;
                        break;

                    case Sound.AudioTypes.music:
                        s.source.outputAudioMixerGroup = musicMixerGroup;
                        break;
                }

                if (s.playOnAwake)
                {
                    s.source.Play();
                }
            }
            //if (Instance.trackPlaying == null)
            //    Debug.Log("TrackPlaying set to null");
        }
        else if (Instance != this)
        {
            //Destroy(gameObject);
        }
    }

    public void Play(string soundName)
    {
        Sound s = AudioManager.Instance.sounds.Find(dummySound => dummySound.clipName == soundName);
        if(s == null)
        {
            Debug.LogError("Sound: " + soundName + " does NOT exist!");
            return;
        }
        //Debug.Log("Playing " + soundName);
        s.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound s = AudioManager.Instance.sounds.Find(dummySound => dummySound.clipName == soundName);
        if (s == null)
        {
            Debug.LogError("Sound: " + soundName + " does NOT exist!");
            return;
        }
        s.source.Stop();
    }

    public void swapTracks(string newTrack, float fadeTime)
    {
        if(!coroutineRunning)
        {
            Instance.StartCoroutine(fadeTrack(newTrack, fadeTime));
        } else
        {
            coroutineQueue.Enqueue(fadeTrack(newTrack, fadeTime));
        }
        //Instance.StopAllCoroutines();
        //Instance.StartCoroutine(fadeTrack(newTrack, fadeTime));
    }

    private IEnumerator fadeTrack(string trackName, float fadeTime)
    {
        coroutineRunning = true;
        Debug.Log("Swaping to track " + trackName);

        float timeToFade = fadeTime;
        float timeElapsed = 0;

        Sound newTrack = AudioManager.Instance.sounds.Find(dummySound => dummySound.clipName == trackName);
        if(newTrack != null)
        {
            if (!Instance.isTrackPlaying || (Instance.isTrackPlaying && !(Instance.trackPlaying.clipName == trackName && Instance.trackPlaying.source.isPlaying)))
            {
                Sound prevTrack = Instance.trackPlaying;
                bool wasTrackPlaying = Instance.isTrackPlaying;
                //if (wasTrackPlaying) wasTrackPlaying = Instance.trackPlaying.source.isPlaying;
                //if (prevTrack == null) Debug.Log("Prev track is null");
                

                newTrack.source.Play();
                Instance.trackPlaying = newTrack;
                Instance.isTrackPlaying = true;
                if (wasTrackPlaying)
                {
                    while (timeElapsed < timeToFade)
                    {
                        newTrack.source.volume = Mathf.Lerp(0, newTrack.volume, timeElapsed / timeToFade);
                        prevTrack.source.volume = Mathf.Lerp(prevTrack.volume, 0, timeElapsed / timeToFade);

                        timeElapsed += Time.deltaTime;
                        yield return null;
                    }
                }
                if (wasTrackPlaying) prevTrack.source.Stop();

            }
        }

        if(coroutineQueue.Count > 0)
        {
            StartCoroutine(coroutineQueue.Dequeue());
        }
        else
            coroutineRunning = false;
    }

    public void onMusicSliderValueChange(float value)
    {
        GameDataController.controller.changeMusicVolume(value);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void onSFXSliderValueChange(float value)
    {
        GameDataController.controller.changeSFXVolume(value);
        soundEffectsMixerGroup.audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(value) * 20);
    }

    public void muteAudioOnCheck(bool value)
    {
        GameDataController.controller.setMuteAudio(value);
        masterMixerGroup.audioMixer.SetFloat("MasterVolume", value ? -80 : 0);
    }
}
