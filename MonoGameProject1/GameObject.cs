using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IUpdateable = MonoGameProject1.Content.IUpdateable;
using IDrawable = MonoGameProject1.Content.IDrawable;

namespace MonoGameProject1;

public class GameObject : IUpdateable, IDrawable
{
	//has a set of behaviors.

	public List<Behavior> behaviors { get; protected set; } = new();
	public string name { get; init; }
	
	private List<IUpdateable> _updatables = new();
	private List<IDrawable> _drawables = new();

	public GameObject(string name)
	{
		this.name = name;
	}

	public GameObject(string name, List<Behavior> behaviors) : this(name)
	{
		this.behaviors = behaviors;
		InitializeBehaviors();
	}

	private void InitializeBehaviors()
	{
		if (behaviors.Count == 0) return;

		foreach (var behavior in behaviors)
		{
			behavior.gameObject = this;
			behavior.Initialize();
			if (behavior is IUpdateable updateable) _updatables.Add(updateable);
			if (behavior is IDrawable drawable) _drawables.Add(drawable);
		}
	}

	/// <summary>
	/// Add/removes the behavior to the update list
	/// </summary>
	/// <param name="active"></param>
	/// <param name="behavior">behavior to add</param>
	public void ActivateBehavior(bool active, Behavior behavior)
	{
		if (behavior is not IUpdateable)
		{
			throw new ArgumentException("Behavior must implement IUpdateable");
		}
		
		IUpdateable updateable = (IUpdateable)behavior;
		if (active)
		{
			if (!_updatables.Contains(updateable)) _updatables.Add(updateable);
		}
		else
		{
			_updatables.Remove(updateable);
		}
	}

	public virtual void Update(GameTime gameTime)
	{
		foreach (var behavior in _updatables)
		{
			behavior.Update(gameTime);
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		foreach (var behavior in _drawables)
		{
			(behavior as IDrawable).Draw(spriteBatch);
		}
	}

	public void AddBehaviors(List<Behavior> newBehaviors)
	{
		foreach (var behavior in newBehaviors)
		{
			behaviors.Add(behavior);
		}
		
		InitializeBehaviors();
	}
	/// <summary>
	///given a behavior type T, assigns the first corresponding behavior in gameObject to behavior.<br/>
	///throws an ArgumentNullException if it's not found
	/// </summary>
	public T TryGetBehavior<T>() where T : Behavior
	{
		try
		{
			return this.behaviors.Find(x => x is T) as T;
		}
		catch (ArgumentNullException e)
		{
			Console.WriteLine($"{this.name} has no {typeof(T)} behavior.");
			throw;
		}
	}
}