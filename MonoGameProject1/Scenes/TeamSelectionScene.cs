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
	private Frame[] frames = new Frame[6];
	private Dictionary<int, ChessPiece> _chosenPieces = new();
	
	private const int selectorSpacing = 280;
	private const float selectorsHeight = 200;

 public TeamSelectionScene()
	{
		List<GameObject> objectsToAdd = [];
		// Initialize chosen pieces dictionary with 0..5 => null
		for (int i = 0; i < 6; i++)
		{
			_chosenPieces[i] = null;
		}
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
			// Wire pick button to PickPiece for this slot
			int capturedIndex = i;
			pickButtons[i].AddListener(() =>
			{
				var piece = selectors[capturedIndex].currentSprite as ChessPiece;
				PickPiece(capturedIndex, piece);
			});
		}

		// Create six frames at the bottom, evenly spaced
		const int frameSize = 160;
		float screenWidth = GameManager.Graphics.Viewport.Width;
		float screenHeight = GameManager.Graphics.Viewport.Height;
		float spacing = screenWidth / (frames.Length + 1f);
		float bottomMargin = 40f;
		for (int i = 0; i < frames.Length; i++)
		{
			frames[i] = new Frame($"Team Slot {i + 1}");
			objectsToAdd.Add(frames[i]);
			frames[i].spriteRenderer.layerDepth = LayerDepthManager.UiDepth;
			frames[i].spriteRenderer.sizePx = new Microsoft.Xna.Framework.Point(frameSize, frameSize);

			float centerX = spacing * (i + 1);
			// Position frames so they are evenly spaced and sit near the bottom
			var sizePx = frames[i].spriteRenderer.sizePx;
			frames[i].transform.parentSpacePos = new Vector2(
				centerX - sizePx.X * 0.5f,
				screenHeight - bottomMargin - sizePx.Y);
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
	
	public void PickPiece(int i, ChessPiece piece)
	{
		if (i < 0 || i >= frames.Length)
			return;
		// Detach previous piece if any
		if (_chosenPieces.TryGetValue(i, out var prev) && prev != null)
		{
			prev.transform.parent = null;
		}
		_chosenPieces[i] = piece;
		if (piece == null)
			return;
		// Parent and center the piece within the frame
		piece.transform.parent = frames[i].transform;
		// Center using the same approach as Button: use source sizes, scale is applied by parent
		var fw = frames[i].spriteRenderer.sourceWidth;
		var fh = frames[i].spriteRenderer.sourceHeight;
		// Ensure the piece origin is centered for nicer alignment
		piece.transform.origin = new Vector2(
			piece.spriteRenderer.sourceWidth * 0.5f,
			piece.spriteRenderer.sourceHeight * 0.5f);
		piece.transform.parentSpacePos = new Vector2(fw * 0.5f, fh * 0.5f);
		// Optional: elevate piece slightly above the frame in render order if needed
		piece.spriteRenderer.layerDepth = LayerDepthManager.UiDepth - 0.01f;
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