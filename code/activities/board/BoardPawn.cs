/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace BonitoBlitz.Board;
using Sandbox;

public partial class BoardPawn : AnimatedEntity
{
	[Net] public libblitz.Player Player { get; private set; }

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

	public BoardPawn( libblitz.Player player )
	{
		Player = player;

		// Prepare events
		MovementAnimationCompleteEvent += HandleMovementComplete;

		// Load tile
		if ( player.SavedTileName != null )
		{
			InternalTile = BaseTile.FromTileName( player.SavedTileName );
			StartMoving( InternalTile ); // TODO: figure out why just setting Position won't work
		}
	}

	public override void OnClientActive( Client cl )
	{
		base.OnClientActive( cl );

		// Load citizen model
		Model = Model.Load( "models/citizen/citizen.vmdl" );

		// Load player clothing
		ClothingContainer clothing = new();
		clothing.LoadFromClient( cl );

		// Dress player
		//clothing.DressEntity( this );
	}

	public BoardPawn() : base( null )
	{
		// Prepare events
		MovementAnimationCompleteEvent += HandleMovementComplete;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		MoveSimulate( cl );
		TurnSimulate( cl );
	}

	public override void FrameSimulate( Client cl )
	{
		base.FrameSimulate( cl );

		MoveFrameSimulate( cl );
	}
}