using Sandbox;

namespace BonitoBlitz.Activities;

public partial class BoardPawn : AnimatedEntity
{
	[Net] public libblitz.GameMember GameMember { get; private set; }

	private readonly Vector3 _mins = new Vector3( -16f, -16f, 0f );
	private readonly Vector3 _maxs = new Vector3( 16f, 16f, 72f );
	public BBox CollisionBox => new(_mins, _maxs);

	public BoardPawn( libblitz.GameMember gameMember )
	{
		GameMember = gameMember;

		if ( gameMember.Client == null )
		{
			return;
		}

		// Load player clothing
		ClothingContainer clothing = new();
		clothing.LoadFromClient( gameMember.Client );

		// Dress player
		clothing.DressEntity( this );

		// Go to tile position
		var tile = GameMember.CurrentTile;
		if ( tile != null )
		{
			Position = GameMember.CurrentTile.Position;
		}
	}

	public BoardPawn() => Host.AssertClient();

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/citizen/citizen.vmdl" );

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		Transmit = TransmitType.Always;
	}

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		EyeLocalPosition = Vector3.Up * (CollisionBox.Maxs.z - 8);
	}
}
