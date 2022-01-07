using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private AudioMixer m_audioMixer;
	public static AudioMixer audioMixer => Instance.m_audioMixer;

	[Title("Audio Sources")]
	[SerializeField] private AudioSource m_mainSource;
	[SerializeField] private AudioSource m_ambientSource;
	[SerializeField] private AudioSource m_soundAudioSource;

	[Title("Audio Clips")]
	[SerializeField] private List<Audio> m_audioList;
	[SerializeField] private List<Audio> m_sfxList;

	private Dictionary<string, AudioClip> m_audioDictionary;
	private Dictionary<string, AudioClip> m_sfxDictionary;

	public override void Awake ()
	{
		base.Awake();

		m_audioDictionary = new Dictionary<string, AudioClip>();

		foreach (Audio audio in m_audioList)
			if (!m_audioDictionary.ContainsKey(audio.name))
				m_audioDictionary.Add(audio.name, audio.audioClip);

		m_sfxDictionary = new Dictionary<string, AudioClip>();

		foreach (Audio audio in m_sfxList)
			if (!m_sfxDictionary.ContainsKey(audio.name))
				m_sfxDictionary.Add(audio.name, audio.audioClip);
	}

	public static void PlayMain ( string soundName )
	{
		if (Instance.m_audioDictionary.ContainsKey(soundName))
		{
			if (Instance.m_mainSource.isPlaying)
				Instance.m_mainSource.Stop();

			Instance.m_mainSource.clip = Instance.m_audioDictionary[soundName];
			Instance.m_mainSource.Play();
		}
		else
			Debug.Log("AudioManager : Main / KeyNotFound : " + soundName);
	}

	public static void PlayAmbient ( string soundName )
	{
		if (Instance.m_audioDictionary.ContainsKey(soundName))
		{
			if (Instance.m_ambientSource.isPlaying)
				Instance.m_ambientSource.Stop();

			Instance.m_ambientSource.clip = Instance.m_audioDictionary[soundName];
			Instance.m_ambientSource.Play();
		}
		else
			Debug.Log("AudioManager : Main / KeyNotFound : " + soundName);
	}

	public static void PlaySfx ( string soundName, float pitch = 1f)
	{
		if (Instance.m_sfxDictionary.ContainsKey(soundName))
		{
			Instance.m_soundAudioSource.pitch = pitch;
			Instance.m_soundAudioSource.PlayOneShot(Instance.m_sfxDictionary[soundName]);
		}
		else
			Debug.Log("AudioManager : SFX / KeyNotFound : " + soundName);
	}
}

[System.Serializable]
public class Audio
{
	public string name;
	public AudioClip audioClip;
}
