using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1;

/// <summary>
/// Phases of the game
/// </summary>
public enum GamePhase
{
	/// <summary>
	/// Game hasn't started
	/// </summary>
	None = -1,
	/// <summary>
	/// Setup phase: players place their pieces
	/// </summary>
	Setup = 0,
	/// <summary>
	/// Normal gameplay
	/// </summary>
	Gameplay = 1,
	/// <summary>
	/// Game ended, white won
	/// </summary>
	WhiteWon = 2,
	/// <summary>
	/// Game ended, black won
	/// </summary>
	BlackWon = 3
}

/// <summary>
/// Keeps track of the phases of the game (i.e. setup phase etc.)
/// </summary>
public class GamePhaseManager : SingletonGameObject<GamePhaseManager>
{
	public GamePhase phase
	{
		get => _phase;
		set
		{
			Console.WriteLine($"Setting phase to {value}");
			if (_phase == value) return;
			GamePhase prevPhase = _phase;
			_phase = value;
			OnPhaseChanged?.Invoke(prevPhase, _phase);
		}
	}
	private GamePhase _phase = GamePhase.None;

	/// <summary>
	/// Keeps track of the phases of the game (i.e. setup phase etc.)
	/// </summary>
	private GamePhaseManager(string name, List<Behavior> behaviors = null) : base(name, behaviors)
	{
	}

	/// <summary>
	/// Called whenever the phase changes. First arg is the previous phase, second arg is the new phase 
	/// </summary>
	public event Action<GamePhase, GamePhase> OnPhaseChanged;

	public static void Instantiate(string name)
	{
		if (instance == null)
			instance = new GamePhaseManager(name);
	}

	public override void Dispose()
	{
		base.Dispose();
		OnPhaseChanged = null;
	}
}