using System;
using System.Collections.Generic;

/// <summary>
/// Contains GameObject entities in a Euclidian space
/// </summary>
public class World
{

	public List<GameObject> gameObjects { get; private set; } = new List<GameObject>();

	public World()
	{
	}
}
