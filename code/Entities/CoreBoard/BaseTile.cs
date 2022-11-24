using System;
using System.Linq;
using Sandbox;

namespace BonitoBlitz.Entities.CoreBoard;

/// <summary>
/// Singular map tile entity; used to denote tile position
/// Requires name, all position and transforms etc. done in-engine
/// </summary>
public abstract class BaseTile : Entity
{
	/// <summary>
	/// Find BaseTile from tile name
	/// </summary>
	/// <param name="tileName">Tile name</param>
	/// <returns>BaseTile or null</returns>
	public static BaseTile FromName( string tileName ) => All.OfType<BaseTile>()
		.FirstOrDefault( ent => string.Equals( ent.Name, tileName, StringComparison.CurrentCultureIgnoreCase ) );
}
