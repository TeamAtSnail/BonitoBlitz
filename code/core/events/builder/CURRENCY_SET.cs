/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0.Events;

public partial class Builder
{
	public partial class Currency
	{
		public static GameEvent SetCurrencyX( uint playerIndex, uint currency, uint value )
			=> new( ActionCode.CURRENCY_SET, playerIndex, currency, value );
		public static GameEvent SetCurrency0( uint playerIndex, uint value )
			=> new( ActionCode.CURRENCY_SET, playerIndex, 0, value );
		public static GameEvent SetCurrency1( uint playerIndex, uint value )
			=> new( ActionCode.CURRENCY_SET, playerIndex, 1, value );
	}
}