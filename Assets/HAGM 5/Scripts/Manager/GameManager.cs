using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[Title("References")]
	public CurrencyConfigs currencyConfigs;

	#region StateMachine
	public static Action<GameState> OnGameStateEnter;
	public static Action<GameState> OnGameStateExit;

	private GameState m_gameState = GameState.None;
	public GameState GameState
	{
		get => m_gameState;
		set
		{
			if (m_gameState == value) return;
			GameState oldState = m_gameState;
			ExitState(m_gameState, value);
			m_gameState = value;
			EnterState(m_gameState, oldState);
		}
	}

	private void ExitState ( GameState _gameState, GameState _newState )
	{
		switch (_gameState)
		{
			case GameState.None:
				break;
			case GameState.MainScreen:
				UIManager.ClosePanel(Panel.Type.Main, true);
				break;
			case GameState.InGame:
				UIManager.ClosePanel(Panel.Type.Game, true);
				break;
			case GameState.EndScreen:
				UIManager.ClosePanel(Panel.Type.End, true);
				break;
		}

		OnGameStateExit?.Invoke(_gameState);
	}

	private void EnterState ( GameState _gameState, GameState _oldState )
	{
		switch (_gameState)
		{
			case GameState.None:
				break;
			case GameState.MainScreen:
				AudioManager.PlayMain("MainTheme");
				UIManager.OpenPanel(Panel.Type.Main, false);
				break;
			case GameState.InGame:
				UIManager.OpenPanel(Panel.Type.Game, true);
				CurrencyManager.Init();
				break;
			case GameState.EndScreen:
				if (isPaused) TogglePause();
				UIManager.OpenPanel(Panel.Type.End, true);
				break;
		}

		OnGameStateEnter?.Invoke(_gameState);
	}
	#endregion

	#region Pause
	[HideInInspector] public bool isPaused = false;
	public void TogglePause ()
	{
		AudioManager.PlaySfx("Punch");

		isPaused = !isPaused;

		if (isPaused)
			UIManager.OpenPanel(Panel.Type.Pause, true);
		else
			UIManager.ClosePanel(Panel.Type.Pause, true);
	}
	#endregion

	private void Start ()
	{
		GameState = GameState.MainScreen;
	}

	private void Update ()
	{
		if (GameState == GameState.MainScreen && Input.GetMouseButtonDown(0)) GameState = GameState.InGame;
		if (GameState == GameState.InGame && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))) TogglePause();
		if (GameState == GameState.InGame && Input.GetKeyDown(KeyCode.Return)) GameState = GameState.EndScreen;
		if (GameState == GameState.EndScreen && Input.GetKeyDown(KeyCode.R)) Restart();
	}

	private void Restart ()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}

public enum GameState
{
	None,
	MainScreen,
	InGame,
	EndScreen,
}
