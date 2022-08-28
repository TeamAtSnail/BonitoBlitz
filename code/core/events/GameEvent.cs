/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0.Events;

/// <summary>
/// Data for a single in-game event. Can be used as a network packet
/// var1-3 are multi-purpose variables based on the value of action
/// </summary>
public struct GameEvent
{
	public Message16 data = new();

	public ActionCode Action => (ActionCode)data.Uint1;

	public GameEvent( ActionCode action, uint var1 = 0, uint var2 = 0, uint var3 = 0 ) => data.SetByUint( (uint)action, var1, var2, var3 );
	public GameEvent( ActionCode action, int var1 = 0, int var2 = 0, int var3 = 0 ) => data.SetByUint( (uint)action, (uint)var1, (uint)var2, (uint)var3 );
	public GameEvent( ActionCode action, float var1 = 0, float var2 = 0, float var3 = 0 )
	{
		data.Uint1 = (uint)action;
		data.Float2 = var1;
		data.Float3 = var2;
		data.Float4 = var3;
	}
	public GameEvent( ActionCode action, uint var1, ulong var2 )
	{
		data.Uint1 = (uint)action;
		data.Uint2 = var1;
		data.Ulong2 = var2;
	}
	public GameEvent( ActionCode action, uint var1, long var2 )
	{
		data.Uint1 = (uint)action;
		data.Uint2 = var1;
		data.Long2 = var2;
	}

	public uint Var1
	{
		get => data.Uint2;
		set => data.Uint2 = value;
	}

	public uint Var2
	{
		get => data.Uint3;
		set => data.Uint3 = value;
	}

	public uint Var3
	{
		get => data.Uint4;
		set => data.Uint4 = value;
	}

	public int Var1i
	{
		get => data.Int2;
		set => data.Int2 = value;
	}

	public int Var2i
	{
		get => data.Int3;
		set => data.Int3 = value;
	}

	public int Var3i
	{
		get => data.Int4;
		set => data.Int4 = value;
	}

	public float Var1f
	{
		get => data.Float2;
		set => data.Float2 = value;
	}

	public float Var2f
	{
		get => data.Float3;
		set => data.Float3 = value;
	}

	public float Var3f
	{
		get => data.Float4;
		set => data.Float4 = value;
	}
}
