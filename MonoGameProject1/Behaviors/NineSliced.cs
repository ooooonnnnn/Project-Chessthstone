using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IUpdateable = MonoGameProject1.Content.IUpdateable;

namespace MonoGameProject1;

/// <summary>
/// A sprite renderer that scales like a 9-sliced: the corners don't scale, the edges scale on one axis, the center scales fully. Requires a Transform <br/>
/// Doesn't support tiling <br/>
/// Doesn't support corner shrinking for small scales
/// </summary>
public class NineSliced : SpriteRenderer
{
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
	/// <param name="baseScale">Scales the entire sprite</param>
	public NineSliced(
		Texture2D texture, Rectangle sourceRectangle, int leftMargin, int rightMargin, int topMargin, int bottomMargin)//, Vector2 baseScale)
		: base(texture, sourceRectangle)
	{
		ValidateInput(leftMargin, rightMargin, topMargin, bottomMargin);
		SetRectsFromContiguousTexture(leftMargin, rightMargin, topMargin, bottomMargin);
	}
	
	/// <summary>
	/// Assumes full texture is used
	/// </summary>
	public NineSliced(Texture2D texture, int leftMargin, int rightMargin, int topMargin, int bottomMargin)//, Vector2 baseScale)
                                                                                                       : base(texture)
	{
		ValidateInput(leftMargin, rightMargin, topMargin, bottomMargin);
		SetRectsFromContiguousTexture(leftMargin, rightMargin, topMargin, bottomMargin);
	}

	private void ValidateInput(int leftMargin, int rightMargin, int topMargin, int bottomMargin)
	{
		//check sourceRectangle is big enough
		if (width < 3 || height < 3)
			throw new ArgumentException("Source texture isn't big enough to be split into 9 regions. Must be at least 3x3");
		
		//check margins are valid
		if (leftMargin < 0 || rightMargin < 0 || topMargin < 0 || bottomMargin < 0)
			throw new ArgumentException("Margins must be non-negative");
		
		//check margins ordered correctly
		if (leftMargin >= rightMargin || topMargin >= bottomMargin)
			throw new ArgumentException("Right and bottom margins must be greater than left and bottom margins respectively");
		
		//check margins don't go outside of sourceRectangle
		if (leftMargin > width - 1 || rightMargin > width - 1 || topMargin > height - 1 || bottomMargin > height - 1)
			throw new ArgumentException("Margins must be less than the width and height of the source texture");
	}
	
	private void SetRectsFromContiguousTexture(int leftMargin, int rightMargin, int topMargin, int bottomMargin)
	{
		Point baseOrigin = new Point(_sourceRectangle.X, _sourceRectangle.Y);
		int ctrOrgX = baseOrigin.X + leftMargin + 1;
		int rghtOrgX = baseOrigin.X + rightMargin + 1;
		int ctrOrgY = baseOrigin.Y + topMargin + 1;
		int btmOrgY = baseOrigin.Y + bottomMargin + 1;

		leftWidth = leftMargin;
		int ctrWidth = rightMargin - leftMargin;
		invCtrWidth = 1f / ctrWidth;
		rghtWidth = width - rightMargin - 1;
		
		topHeight = topMargin;
		int ctrHeight = bottomMargin - topMargin;
		invCtrHeight = 1f / ctrHeight;
		btmHeight = height - bottomMargin - 1;
		
		
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
		DrawRegion(spriteBatch, topLeft, tlScale, _transform.ToWorldSpace(Vector2.Zero));
		DrawRegion(spriteBatch, btmLeft, blScale, _transform.ToWorldSpace(height* Vector2.UnitY),
			Vector2.UnitY * btmLeft.Height);
		DrawRegion(spriteBatch, topRght, trScale, _transform.ToWorldSpace(width * Vector2.UnitX),
			Vector2.UnitX * topRght.Width);
		DrawRegion(spriteBatch, btmRght, brScale, _transform.ToWorldSpace(new Vector2(width, height)),
			new Vector2(btmRght.Width, btmRght.Height));
		
		//Draw edges
		DrawRegion(spriteBatch, topCtr, tcScale, _transform.ToWorldSpace((width*0.5f)*Vector2.UnitX),
			new Vector2(topCtr.Width*0.5f,0));
		DrawRegion(spriteBatch, ctrLeft, clScale, _transform.ToWorldSpace(height*0.5f*Vector2.UnitY),
			new Vector2(0,ctrLeft.Height*0.5f));
		DrawRegion(spriteBatch, btmCtr, bcScale, _transform.ToWorldSpace(new Vector2(width*0.5f, height)), 
			new Vector2(btmCtr.Width*0.5f, btmCtr.Height));
		DrawRegion(spriteBatch, ctrRght, crScale, _transform.ToWorldSpace(new Vector2(width, height*0.5f)),
			new Vector2(ctrRght.Width, ctrRght.Height*0.5f));
		
		//Draw center
		DrawRegion(spriteBatch, ctrCtr, ccScale, _transform.ToWorldSpace(new Vector2(width*0.5f, height*0.5f)),
			new Vector2(ctrCtr.Width*0.5f, ctrCtr.Height*0.5f));
	}

	private void DrawRegion(SpriteBatch spriteBatch, Rectangle region, Vector2 scale, Vector2 position)
	{
		DrawRegion(spriteBatch, region, scale, position, Vector2.Zero);
	}
	
	private void DrawRegion(SpriteBatch spriteBatch, Rectangle region, Vector2 scale, Vector2 position, Vector2 origin)
	{
		spriteBatch.Draw(_texture, position, region, color, _transform.rotation, origin, scale, effects, layerDepth);
	}

	private void UpdateScales()
	{
		float centerRowHeight = invCtrHeight * (height * _transform.scale.Y - topHeight - btmHeight);
		float centerColumnWidth = invCtrWidth * (width * _transform.scale.X - leftWidth - rghtWidth); 
		clScale.Y = centerRowHeight;
		tcScale.X = centerColumnWidth;
		crScale.Y = centerRowHeight;
		bcScale.X = centerColumnWidth;
		ccScale.X = centerColumnWidth;
		ccScale.Y = centerRowHeight;
	}
}