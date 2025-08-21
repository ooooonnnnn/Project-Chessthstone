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
	private ToolTip[] descriptions = new ToolTip[6];

	public TeamSelectionScene()
	{
		nextOrStartButton = new ("Next or start", "Ready");
		nextOrStartButton.ChangeBackgroundScale(new Vector2(1, 0.7f));
		nextOrStartButton.transform.origin = nextOrStartButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
		nextOrStartButton.AddListener(HandleNextButtonPress);
		List<GameObject> objectsToAdd = [nextOrStartButton];

		//Instantiate and load selectors
		for (int i = 0; i < selectors.Length; i++)
		{
			selectors[i] = new Selector($"Selector {i + 1}",
				ChessPieceFactory.GetAllPieces(true, (PieceType)i));
			selectors[i].transform.parentSpacePos = new Vector2(
				GameManager.Graphics.Viewport.Width / 2f + -800 + 256 * i, 
				GameManager.Graphics.Viewport.Height / 2f - 100);
			selectors[i].previousButton.transform.parentSpacePos += new Vector2(0, 200);
			objectsToAdd.Add(selectors[i]);
			
			descriptions[i] = new ToolTip($"{selectors[i].name} description", "");
			descriptions[i].transform.parentSpacePos = selectors[i].transform.parentSpacePos + new Vector2(-50, 130);
			descriptions[i].textRenderer.MaxWidth = 200;
			objectsToAdd.Add(descriptions[i]);

			var i1 = i;
			void UpdateDescription(Sprite sprite)
			{
				if (sprite is not ChessPiece chessPiece)
					return;
				descriptions[i1].Text = string.Concat(
					chessPiece.ability?.ToString() ?? "No special ability",
					"\n",
					$"{chessPiece.baseHealth} HP | {chessPiece.BaseDamage} DMG");
			};
			selectors[i].OnSpriteChanged += UpdateDescription;
			UpdateDescription(selectors[i].currentSprite);
		}
		
		nextOrStartButton.transform.parentSpacePos = new Vector2(
			GameManager.Graphics.Viewport.Width / 2f + -800 + 256 * 6, 
			selectors[0].nextButton.transform.worldSpacePos.Y);

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
			SceneManager.ChangeScene(new GameScene(whiteTeam, blackTeam));
		}
	}

	public override void Initialize()
	{
		// Console.WriteLine($"{this} isn't initializing anything");
	}
}