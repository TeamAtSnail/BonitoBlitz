/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class PlayerEntity : AnimatedEntity
{
	public readonly SessionClient SessionClient;

	[Net]
	public uint SessionClientIndex { get; }

	public PlayerEntity( SessionClient sessionClient )
	{
		SessionClient = sessionClient;
		SessionClientIndex = sessionClient.Index;

		AddEventHandlers();

		// Load entity model
		Model = Model.Load( "models/citizen/citizen.vmdl" );

		// Dress entity
		ClothingContainer clothing = new();
		clothing.LoadFromClient( sessionClient.ActiveClient );
		clothing.DressEntity( this );
	}

	public PlayerEntity()
	{
		Host.AssertClient( "PlayerEntity" );
		AddEventHandlers();
	}

	public void AddEventHandlers()
	{
		/* CURRENCY_SET */
		Gamemode0.Session.AddForeverHandler( new(
			Events.ActionCode.CURRENCY_SET,
			"_PlayerEntity_CURRENCY_SET_" + SessionClientIndex,
			HandleSetCurrency
		) );
	}
}