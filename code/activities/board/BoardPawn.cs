/*
 * BoardPawn.cs
 * Core pawn for the game board experience
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;

namespace BonitoBlitz.CoreActivities.Board;

public partial class BoardPawn : AnimatedEntity
{
	[Net] public libblitz.Player Player { get; private set; }

	public BoardCameraMode Camera
	{
		get => Components.Get<BoardCameraMode>();
		set
		{
			if ( Camera == value ) return;
			Components.RemoveAny<BoardCameraMode>();
			Components.Add( value );
		}
	}

	public BoardPawn( libblitz.Player player ) : base()
	{
		Player = player;

		Camera = new BoardCameraMode();

		if ( Player.Client != null )
		{
			// Load player clothing
			ClothingContainer clothing = new();
			clothing.LoadFromClient( Player.Client );

			// Dress player
			clothing.DressEntity( this );
		}
	}

	public BoardPawn() : base() { }

	public override void Spawn()
	{
		// Load citizen model
		SetModel( "models/citizen/citizen.vmdl" );
	}
}