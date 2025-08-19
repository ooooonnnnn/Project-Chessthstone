using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Behaviors;

namespace MonoGameProject1;

/// <summary>
/// A sprite renderer that scales like a 9-sliced: the corners don't scale, the edges scale on one axis, the center scales fully. Requires a Transform <br/>
/// Doesn't support tiling <br/>
/// Doesn't support corner shrinking for small scales
/// </summary>
public class NineSliced : SpriteRenderer
{
	//corner scale
	public float cornerScale = 1;
	
	//base rectangles from which to take the texture
	private Rectangle topLeft, topCtr, topRght, ctrLeft, ctrCtr, ctrRght, btmLeft, btmCtr, btmRght;
	//scale of each part of the sprite
	private Vector2 tlScale = Vector2.One, tcScale = Vector2.One, trScale = Vector2.One,
		clScale = Vector2.One, ccScale = Vector2.One, crScale = Vector2.One,
		blScale = Vector2.One, bcScale = Vector2.One, brScale = Vector2.One;
	private int topHeight, btmHeight, leftWidth, rghtWidth;
	private float invCtrWidth, invCtrHeight;
	
	/// <summary>
	/// Use this constructor when the source texture isn't contiguous, i.e. when it's split within the atlas.
	/// </summary>
	/// <param name="texture">The Source texture</param>
	/// <param name="topLeft">Rectangle of the top left corner</param>
	/// <param name="topCtr">Rectangle of the top edge</param>
	/// <param name="topRght">Rectangle of the top right corner</param>
	/// <param name="ctrLeft">Rectangle of the left edge</param>
	/// <param name="ctrCtr">Rectangle of the center part</param>
	/// <param name="ctrRght">Rectangle of the right edge</param>
	/// <param name="btmLeft">Rectangle of the bottom left corner</param>
	/// <param name="btmCtr">Rectangle of the bottom edge</param>
	/// <param name="btmRght">Rectangle of the bottom right corner</param>
	public NineSliced(Texture2D texture, Rectangle topLeft, Rectangle topCtr, Rectangle topRght, Rectangle ctrLeft, Rectangle ctrCtr, Rectangle ctrRght, Rectangle btmLeft, Rectangle btmCtr, Rectangle btmRght) : base(texture)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Constructs a 9-sliced renderer with the texture taken from the sourceRectangle with regions defined by the margins
	/// </summary>
	/// <param name="texture"></param>
	/// <param name="sourceRectangle"></param>
	/// <param name="leftMargin">distance of the left margin from the origin</param>
	/// <param name="rightMargin">distance of the right margin from the origin</param>
	/// <param name="topMargin">distance of the top margin from the origin</param>
	/// <param name="bottomMargin">distance of the bottom margin from the origin</param>
	/// <param name="cornerScale">Scales the corners</param>
	public NineSliced(
		Texture2D texture, Rectangle sourceRectangle, int leftMargin, int rightMargin, int topMargin, int bottomMargin, float cornerScale = 1)
		: base(texture, sourceRectangle)
	{
		ValidateInput(leftMargin, rightMargin, topMargin, bottomMargin);
		this.cornerScale = cornerScale;
		SetRectsFromContiguousTexture(leftMargin, rightMargin, topMargin, bottomMargin);
	}
	
	/// <summary>
	/// Assumes full texture is used
	/// </summary>
	public NineSliced(Texture2D texture, int leftMargin, int rightMargin, int topMargin, int bottomMargin, float cornerScale = 1)
                                                                                                       : base(texture)
	{
		ValidateInput(leftMargin, rightMargin, topMargin, bottomMargin);
		this.cornerScale = cornerScale;
		SetRectsFromContiguousTexture(leftMargin, rightMargin, topMargin, bottomMargin);
	}

	private void ValidateInput(int leftMargin, int rightMargin, int topMargin, int bottomMargin)
	{
		//check sourceRectangle is big enough
		if (sourceWidth < 3 || sourceHeight < 3)
			throw new ArgumentException("Source texture isn't big enough to be split into 9 regions. Must be at least 3x3");
		
		//check margins are valid
		if (leftMargin < 0 || rightMargin < 0 || topMargin < 0 || bottomMargin < 0)
			throw new ArgumentException("Margins must be non-negative");
		
		//check margins ordered correctly
		if (leftMargin >= rightMargin || topMargin >= bottomMargin)
			throw new ArgumentException("Right and bottom margins must be greater than left and bottom margins respectively");
		
		//check margins don't go outside of sourceRectangle
		if (leftMargin > sourceWidth - 1 || rightMargin > sourceWidth - 1 || topMargin > sourceHeight - 1 || bottomMargin > sourceHeight - 1)
			throw new ArgumentException("Margins must be less than the width and height of the source texture");
	}
	
	private void SetRectsFromContiguousTexture(int leftMargin, int rightMargin, int topMargin, int bottomMargin)
	{
		Point baseOrigin = new Point(sourceRectangle.X, sourceRectangle.Y);
		int ctrOrgX = baseOrigin.X + leftMargin + 1;
		int rghtOrgX = baseOrigin.X + rightMargin + 1;
		int ctrOrgY = baseOrigin.Y + topMargin + 1;
		int btmOrgY = baseOrigin.Y + bottomMargin + 1;

		leftWidth = leftMargin;
		int ctrWidth = rightMargin - leftMargin;
		invCtrWidth = 1f / ctrWidth;
		rghtWidth = sourceWidth - rightMargin - 1;
		
		topHeight = topMargin;
		int ctrHeight = bottomMargin - topMargin;
		invCtrHeight = 1f / ctrHeight;
		btmHeight = sourceHeight - bottomMargin - 1;
		
		
		topLeft = new Rectangle(baseOrigin.X, baseOrigin.Y, leftWidth, topHeight);
		topCtr = new Rectangle(ctrOrgX, baseOrigin.Y, ctrWidth, topHeight);
		topRght = new Rectangle(rghtOrgX, baseOrigin.Y, rghtWidth, topHeight);
		ctrLeft = new Rectangle(baseOrigin.X, ctrOrgY, leftWidth, ctrHeight);
		ctrCtr = new Rectangle(ctrOrgX, ctrOrgY, ctrWidth, ctrHeight);
		ctrRght = new Rectangle(rghtOrgX, ctrOrgY, rghtWidth, ctrHeight);
		btmLeft = new Rectangle(baseOrigin.X, btmOrgY, leftWidth, btmHeight);
		btmCtr = new Rectangle(ctrOrgX, btmOrgY, ctrWidth, btmHeight);
		btmRght = new Rectangle(rghtOrgX, btmOrgY, rghtWidth, btmHeight);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		UpdateScales();
		//Draw corners
		DrawRegion(spriteBatch, topLeft, tlScale, transform.ToWorldSpace(Vector2.Zero));
		DrawRegion(spriteBatch, btmLeft, blScale, transform.ToWorldSpace(sourceHeight* Vector2.UnitY),
			Vector2.UnitY * btmLeft.Height);
		DrawRegion(spriteBatch, topRght, trScale, transform.ToWorldSpace(sourceWidth * Vector2.UnitX),
			Vector2.UnitX * topRght.Width);
		DrawRegion(spriteBatch, btmRght, brScale, transform.ToWorldSpace(new Vector2(sourceWidth, sourceHeight)),
			new Vector2(btmRght.Width, btmRght.Height));
		
		//Draw edges
		DrawRegion(spriteBatch, topCtr, tcScale, transform.ToWorldSpace((sourceWidth*0.5f)*Vector2.UnitX),
			new Vector2(topCtr.Width*0.5f,0));
		DrawRegion(spriteBatch, ctrLeft, clScale, transform.ToWorldSpace(sourceHeight*0.5f*Vector2.UnitY),
			new Vector2(0,ctrLeft.Height*0.5f));
		DrawRegion(spriteBatch, btmCtr, bcScale, transform.ToWorldSpace(new Vector2(sourceWidth*0.5f, sourceHeight)), 
			new Vector2(btmCtr.Width*0.5f, btmCtr.Height));
		DrawRegion(spriteBatch, ctrRght, crScale, transform.ToWorldSpace(new Vector2(sourceWidth, sourceHeight*0.5f)),
			new Vector2(ctrRght.Width, ctrRght.Height*0.5f));
		
		//Draw center
		DrawRegion(spriteBatch, ctrCtr, ccScale, transform.ToWorldSpace(new Vector2(sourceWidth*0.5f, sourceHeight*0.5f)),
			new Vector2(ctrCtr.Width*0.5f, ctrCtr.Height*0.5f));
	}

	private void DrawRegion(SpriteBatch spriteBatch, Rectangle region, Vector2 scale, Vector2 position)
	{
		DrawRegion(spriteBatch, region, scale, position, Vector2.Zero);
	}
	
	private void DrawRegion(SpriteBatch spriteBatch, Rectangle region, Vector2 scale, Vector2 position, Vector2 origin)
	{
		spriteBatch.Draw(texture, position, region, color, transform.rotation, origin, scale, effects, layerDepth);
	}

	private void UpdateScales()
	{
		float centerColumnWidth = invCtrWidth * (sourceWidth * transform.worldSpaceScale.X - (leftWidth + rghtWidth)*cornerScale); 
		float centerRowHeight = invCtrHeight * (sourceHeight * transform.worldSpaceScale.Y - (topHeight + btmHeight)*cornerScale);
		//edge scaling
		clScale.Y = centerRowHeight;
		clScale.X = cornerScale;
		tcScale.X = centerColumnWidth;
		tcScale.Y = cornerScale;
		crScale.Y = centerRowHeight;
		crScale.X = cornerScale;
		bcScale.X = centerColumnWidth;
		bcScale.Y = cornerScale;
		//center scaling
		ccScale.X = centerColumnWidth;
		ccScale.Y = centerRowHeight;
		//corner scaling
		tlScale = Vector2.One * cornerScale;
		trScale = Vector2.One * cornerScale;
		blScale = Vector2.One * cornerScale;
		brScale = Vector2.One * cornerScale;
	}
}