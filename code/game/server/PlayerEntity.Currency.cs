/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class PlayerEntity
{
	private uint currency0 = 0;
	private uint currency1 = 0;

	public uint Currency0
	{
		get => currency0;
		set
		{
			Host.AssertServer( "Currency0" );
			Gamemode0.Session.BroadcastEvent( Events.Builder.Currency.SetCurrency0( SessionClientIndex, value ) );
		}
	}

	public uint Currency1
	{
		get => currency1;
		set
		{
			Host.AssertServer( "Currency1" );
			Gamemode0.Session.BroadcastEvent( Events.Builder.Currency.SetCurrency1( SessionClientIndex, value ) );
		}
	}

	/// <summary>
	/// Event handler for CURRENCY_SET
	/// </summary>
	/// <param name="message">Event message</param>
	/// <returns>Status code</returns>
	private uint HandleSetCurrency( SessionIncomingMessage message )
	{
		uint playerIdx = message.Event.Var1;
		uint currency = message.Event.Var2;
		uint value = message.Event.Var3;

		if ( playerIdx != SessionClientIndex )
			return (uint)Events.StatusCode.UNEXPECTED_PLAYER;

		if ( currency == 0 )
			currency0 = value;

		if ( currency == 1 )
			currency1 = value;

		return 0;
	}
}