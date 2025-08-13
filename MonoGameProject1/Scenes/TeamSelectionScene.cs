using System;
using System.Collections.Generic;

namespace MonoGameProject1.Scenes;

/// <summary>
/// The scene where the players choose their team composition
/// </summary>
public class TeamSelectionScene : Scene
{
	private List<ChessPiece> whiteTeam = [];
	private List<ChessPiece> blackTeam = [];
	
	private Button nextOrStartButton;

	public TeamSelectionScene()
	{
		nextOrStartButton = new ("Next or start", "Ready");
		nextOrStartButton.AddListener(() =>
		{
			SceneManager.ChangeScene(new TestGameScene());
		});
		
		AddGameObjects([nextOrStartButton]);
	}
}