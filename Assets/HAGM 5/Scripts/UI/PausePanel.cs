using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : Panel
{
	[Title("Sliders")]
	[SerializeField] private Slider m_musicSlider;
	[SerializeField] private Slider m_sfxSlider;

	private void Awake ()
	{
		m_musicSlider.onValueChanged.AddListener(MusicVolumeChanged);
		m_sfxSlider.onValueChanged.AddListener(SFXVolumeChanged);
	}

	private void MusicVolumeChanged ( float _value )
	{
		AudioManager.audioMixer.SetFloat("MusicVolume", -80f + _value * 80f);
	}

	private void SFXVolumeChanged ( float _value )
	{
		AudioManager.audioMixer.SetFloat("SFXVolume", -80f + _value * 80f);
	}
}
