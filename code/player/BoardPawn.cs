/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class BoardPawn : libgm0.Pawn
{
	public CameraMode Camera
	{
		get => Components.Get<CameraMode>();
		set
		{
			if ( Camera == value ) return;
			Components.RemoveAny<CameraMode>();
			Components.Add( value );
		}
	}

	public BoardPawn( libgm0.PlayerData data ) : base( data )
	{
		// Prepare events
		MovementAnimationCompleteEvent += HandleMovementComplete;

		// Load tile
		if ( data.Extensions.ContainsKey( "TileName" ) )
		{
			InternalTile = BaseTile.FromTileName( data.Extensions["TileName"] );
			StartMoving( InternalTile ); // TODO: figure out why just setting Position won't work
		}
	}

	public override void OnClientActive( Client client )
	{
		base.OnClientActive( client );

		// Load citizen model
		Model = Model.Load( "models/citizen/citizen.vmdl" );

		// Load player clothing
		ClothingContainer clothing = new();
		clothing.LoadFromClient( Client );

		// Dress player
		//clothing.DressEntity( this );
	}

	public BoardPawn() : base( null )
	{
		// Prepare events
		MovementAnimationCompleteEvent += HandleMovementComplete;
	}

	public override void Simulate( Client client )
	{
		base.Simulate( client );

		MoveSimulate( client );
		TurnSimulate( client );
	}

	public override void FrameSimulate( Client client )
	{
		base.FrameSimulate( client );

		MoveFrameSimulate( client );
	}
}