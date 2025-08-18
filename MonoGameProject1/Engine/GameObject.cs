using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProject1.Engine;
using IDrawable = MonoGameProject1.IDrawable;

namespace MonoGameProject1;

/// <summary>
/// Any dynamic object in the game. It has a list of behaviors that do stuff.
/// </summary>
public class GameObject : IUpdatable, IDrawable, IDisposable, IStart, IActivatable
{
    public List<Behavior> behaviors { get; protected set; } = new();
    public string name { get; init; }
    public Scene parentScene;
    private bool _isActive = false;
    public bool isActive => _isActive;

	/// <summary>
	/// Some behaviors are hierarchical, which means they have children of the same type.
	/// SceneManager needs to know about those children 
	/// </summary>
	private List<IHierarchy> _hierarchicalBehaviors = new();
	public IReadOnlyList<IHierarchy> hierarchicalBehaviors => _hierarchicalBehaviors;
	
	private List<IUpdatable> _updatables = new();
	private List<IDrawable> _drawables = new();

    public GameObject(string name, List<Behavior> behaviors = null)
    {
        this.name = name;

        if (behaviors == null)
            this.behaviors = new List<Behavior>();
        else
        {
            this.behaviors = behaviors;
            InitializeBehaviors(behaviors);
        }
    }

    /// <summary>
    /// Initializes the newly added behaviors
    /// </summary>
    public void InitializeBehaviors(List<Behavior> newBehaviors = null)
    {
        if (newBehaviors == null) newBehaviors = behaviors;
        foreach (var behavior in newBehaviors)
        {
            behavior.gameObject = this;
            behavior.Initialize();
            ClassifyBehavior(behavior);
        }
    }

    /// <summary>
    /// Adds a reference of the behavior to the proper lists, for functionality, i.e. IUpdatables need to be in _updatables
    /// </summary>
    protected virtual void ClassifyBehavior(Behavior behavior)
    {
        if (behavior is IUpdatable updateable)
        {
            if (!_updatables.Contains(updateable))
                _updatables.Add(updateable);
        }

        if (behavior is IDrawable drawable)
        {
            if (!_drawables.Contains(drawable))
                _drawables.Add(drawable);
        }

        if (behavior is IHierarchy hierarchy)
        {
            if (!_hierarchicalBehaviors.Contains(hierarchy))
                _hierarchicalBehaviors.Add(hierarchy);
        }
    }

    /// <summary>
    /// Sets the active state of all behaviors of this GameObject.
    /// IUpdatables and IDrawables get added or removed from their lsits
    /// </summary>
    public virtual void SetActive(bool active)
    {
        foreach (Behavior behavior in behaviors)
        {
            (behavior as IActivatable)?.SetActive(active);

            if (behavior is IUpdatable updatable)
            {
                if (active && !_updatables.Contains(updatable))
                    _updatables.Add(updatable);
                else if (!active)
                    _updatables.Remove(updatable);
            }

            if (behavior is IDrawable drawable)
            {
                if (active && !_drawables.Contains(drawable))
                    _drawables.Add(drawable);
                else if (!active)
                    _drawables.Remove(drawable);
            }
        }

        _isActive = active;
        Console.WriteLine($"{name} active: {active}");

        foreach (var hierarchy in _hierarchicalBehaviors)
        {
            foreach (GameObject child in hierarchy.children)
            {
                child.SetActive(active);
            }
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
            behavior.Draw(spriteBatch);
        }
    }

    public void AddBehaviors(List<Behavior> newBehaviors)
    {
        List<Behavior> behaviorsToInitialize = new(newBehaviors);

        foreach (var behavior in newBehaviors)
        {
            if (behaviors.Contains(behavior))
            {
                behaviorsToInitialize.Remove(behavior);
            }

            behaviors.Add(behavior);
        }

        InitializeBehaviors(behaviorsToInitialize);
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

    /// <summary>
    /// Throws an error if the GamgeObject has a behavior of type T which isn't the requester
    /// </summary>
    /// <param name="requester"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public void DontAllowBehaviorBesidesThis<T>(Behavior requester) where T : Behavior
    {
        if (behaviors.Any(x => x is T && x != requester))
        {
            throw new ArgumentException($"{name} has a {typeof(T)} behavior while a {requester.GetType()} forbids it");
        }
    }

	/// <summary>
	/// Disposes disposable behaviors and the parentScene reference
	/// </summary>
	public virtual void Dispose()
	{
		foreach (var behavior in behaviors)
		{
			if (behavior is IDisposable disposable) disposable.Dispose();
		}
		parentScene = null;
	}

    /// <summary>
    /// Calls Start on IStart behaviors
    /// </summary>
    public virtual void Start()
    {
        foreach (var behavior in behaviors)
        {
            if (behavior is IStart start) start.Start();
        }
    }
}