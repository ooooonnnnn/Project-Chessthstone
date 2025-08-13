using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

/// <summary>
/// The scene where the players choose their team composition
/// </summary>
public class TeamSelectionScene : Scene
{
	private List<ChessPiece> whiteTeam = [];
	private List<ChessPiece> blackTeam = [];
	
	private Button nextOrStartButton;
	private Selector[] selectors = new Selector[6];

	public TeamSelectionScene()
	{
		nextOrStartButton = new ("Next or start", "Ready");
		nextOrStartButton.AddListener(() =>
		{
			SceneManager.ChangeScene(new TestGameScene());
		});


		List<GameObject> objectsToAdd = [nextOrStartButton];
		for (int i = 0; i < selectors.Length; i++)
		{
			selectors[i] = new Selector("", []);
			selectors[i].transform.parentSpacePos = new Vector2(GameManager.Graphics.Viewport.Width / 2f + 100 * i, 
				GameManager.Graphics.Viewport.Height / 2f);
			objectsToAdd.Add(selectors[i]);
		}
		AddGameObjects(objectsToAdd);
	}
}