using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : Panel
{
	[Title("Buttons")]
	[SerializeField] private Button m_pauseBtn;

	[Title("Sliders")]
	[SerializeField] private Slider m_musicSlider;
	[SerializeField] private Slider m_sfxSlider;

	private void Awake ()
	{
		m_musicSlider.onValueChanged.AddListener(MusicVolumeChanged);
		m_sfxSlider.onValueChanged.AddListener(SFXVolumeChanged);
		m_pauseBtn.onClick.AddListener(OnClickPause);
	}

	protected override void OnDestroy ()
	{
		base.OnDestroy();
		m_musicSlider.onValueChanged.RemoveListener(MusicVolumeChanged);
		m_sfxSlider.onValueChanged.RemoveListener(SFXVolumeChanged);
		m_pauseBtn.onClick.RemoveListener(OnClickPause);
	}

	private void OnClickPause ()
	{
		GameManager.Instance.TogglePause();
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
