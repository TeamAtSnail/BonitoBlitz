/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class BoardPlayer : libgm0.BasePlayer
{
	public BoardPlayer( libgm0.PlayerData data ) : base( data )
	{
		// Prepare events
		MovementAnimationCompleteEvent += HandleMovementComplete;
	}

	public BoardPlayer() : base( null )
	{
		// Prepare events
		MovementAnimationCompleteEvent += HandleMovementComplete;
	}

	public override void Initialize()
	{
		// Load citizen model
		Model = Model.Load( "models/citizen/citizen.vmdl" );

		// Load player clothing
		ClothingContainer clothing = new();
		clothing.LoadFromClient( Client );

		// Dress player
		clothing.DressEntity( this );
	}

	public override void LibSimulate( Client client )
	{
		base.LibSimulate( client );

		MoveSimulate( client );
		TurnSimulate( client );
	}

	public override void LibFrameSimulate( Client client )
	{
		base.LibFrameSimulate( client );

		MoveFrameSimulate( client );
	}
}