/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

public partial class GameEventCreator
{
	public partial class Currency
	{
		public static GameEvent SetCurrencyArbitrary( uint playerIndex, uint currency, uint value )
			=> new( (uint)GameEventAction.CURRENCY_SET, playerIndex, currency, value );
		public static GameEvent SetCurrency0( uint playerIndex, uint value )
			=> new( (uint)GameEventAction.CURRENCY_SET, playerIndex, 0, value );
		public static GameEvent SetCurrency1( uint playerIndex, uint value )
			=> new( (uint)GameEventAction.CURRENCY_SET, playerIndex, 1, value );
	}
}