/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 * semi based on the Player class for sbox-pool 
 *   -> (https://github.com/Facepunch/sbox-pool/blob/master/code/player/Player.cs)
 */
namespace gm0;
using Sandbox;

/// <summary>
/// Player class for the gm0 gamemode
/// </summary>
class Player : Entity
{
	/// <summary>
	/// Player camera attribute
	/// Uses EventCamera which acts based on server requests
	/// </summary>
	public EventCamera Camera
	{
		get => Components.Get<EventCamera>();
		set
		{
			var current = Camera;
			if ( current == value ) return;

			Components.RemoveAny<EventCamera>();
			Components.Add( value );
		}
	}

	public Player()
	{
		Camera = new EventCamera();
	}
}