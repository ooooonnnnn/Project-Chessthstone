using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGameProject1.Scenes;

/// <summary>
/// The scene where the players choose their team composition
/// </summary>
public class TeamSelectionScene : Scene
{
	private ChessPiece[] whiteTeam = new ChessPiece[6];
	private ChessPiece[] blackTeam = new ChessPiece[6];
	private bool currentPlayerIsWhite = true;
	
	private Button nextOrStartButton;
	private Selector[] selectors = new Selector[6];

	public TeamSelectionScene()
	{
		nextOrStartButton = new ("Next or start", "Ready");
		nextOrStartButton.AddListener(HandleNextButtonPress);

		List<GameObject> objectsToAdd = [nextOrStartButton];

		//Instantiate and load selectors
		for (int i = 0; i < selectors.Length; i++)
		{
			selectors[i] = new Selector($"Selector {i + 1}",
				ChessPieceFactory.GetAllPieces(true, (PieceType)i));
		}

		for (int i = 0; i < selectors.Length; i++)
		{
			selectors[i].transform.parentSpacePos = new Vector2(
				GameManager.Graphics.Viewport.Width / 2f + -800 + 300 * i, 
				GameManager.Graphics.Viewport.Height / 2f);
			objectsToAdd.Add(selectors[i]);
		}
		AddGameObjects(objectsToAdd);
	}

	/// <summary>
	/// Loads the selectors with the apropriate pieces 
	/// </summary>
	private void LoadSelectors(bool isWhite)
	{
		for (int i = 0; i < selectors.Length; i++)
		{
			selectors[i].ChangeSprites(ChessPieceFactory.GetAllPieces(isWhite, (PieceType)i), false);
		}
	}

	private void HandleNextButtonPress()
	{
		if (currentPlayerIsWhite)
		{
			for (int i = 0; i < selectors.Length; i++)
			{
				ChessPiece whitePiece = selectors[i].currentSprite as ChessPiece;
				whitePiece.transform.parent = null;
				whiteTeam[i] = whitePiece;
			}
			LoadSelectors(false);
			currentPlayerIsWhite = false;
		}
		else
		{
			for (int i = 0; i < selectors.Length; i++)
			{
				ChessPiece blackPiece = selectors[i].currentSprite as ChessPiece;
				blackPiece.transform.parent = null;
				blackTeam[i] = blackPiece;
			}
			SceneManager.ChangeScene(new TestGameScene(whiteTeam, blackTeam));
		}
	}

	public override void Initialize()
	{
		Console.WriteLine($"{this} isn't initializing anything");
	}
}