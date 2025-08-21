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
	private Button[] pickButtons = new Button[6];
	
	private const int selectorSpacing = 280;
	private const float selectorsHeight = 200;

	public TeamSelectionScene()
	{
		List<GameObject> objectsToAdd = [];
		//Instantiate and load selectors
		for (int i = 0; i < selectors.Length; i++)
		{
			selectors[i] = new Selector($"Selector {i + 1}",
				ChessPieceFactory.GetAllPieces(true, (PieceType)i));
			objectsToAdd.Add(selectors[i]);
			selectors[i].transform.parentSpacePos = new Vector2(
				GameManager.Graphics.Viewport.Width / 2f + -selectorSpacing * 2.5f + selectorSpacing * i, 
				GameManager.Graphics.Viewport.Height / 2f - selectorsHeight);
			
			descriptions[i] = new ToolTip($"{selectors[i].name} description", "");
			descriptions[i].transform.parent = selectors[i].transform;
			int maxWidth = 200;
			descriptions[i].textRenderer.MaxWidth = maxWidth;
			descriptions[i].transform.origin = Vector2.UnitX * maxWidth * 0.5f; 
			descriptions[i].transform.parentSpacePos = new Vector2(0, 130);
			objectsToAdd.Add(descriptions[i]);

			var i1 = i;
			void UpdateDescription(Sprite sprite)
			{
				if (sprite is not ChessPiece chessPiece)
					return;
				descriptions[i1].text = string.Concat(
					chessPiece.ability?.ToString() ?? "No special ability",
					"\n",
					$"{chessPiece.baseHealth} HP | {chessPiece.BaseDamage} DMG");
			};
			selectors[i].OnSpriteChanged += UpdateDescription;
			UpdateDescription(selectors[i].currentSprite);
			
			pickButtons[i] = new Button($"{selectors[i].name} pick", "Pick");
			objectsToAdd.Add(pickButtons[i]);
			pickButtons[i].transform.parent = selectors[i].transform;
			pickButtons[i].transform.origin = pickButtons[i].spriteRenderer.sizePx.ToVector2() * 0.5f;
			pickButtons[i].transform.parentSpacePos = new Vector2(0, -100);
			pickButtons[i].ChangeBackgroundScale(new Vector2(0.8f, 0.4f));
		}
		
		nextOrStartButton = new ("Next or start", "Ready");
		objectsToAdd.Add(nextOrStartButton);
		nextOrStartButton.ChangeBackgroundScale(new Vector2(1, 0.7f));
		nextOrStartButton.transform.origin = nextOrStartButton.spriteRenderer.sizePx.ToVector2() * 0.5f;
		nextOrStartButton.AddListener(HandleNextButtonPress);

		nextOrStartButton.transform.parentSpacePos = new Vector2(
			GameManager.Graphics.Viewport.Width / 2f + selectorSpacing * 2.5f, 
			selectors[0].nextButton.transform.worldSpacePos.Y + 500);

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