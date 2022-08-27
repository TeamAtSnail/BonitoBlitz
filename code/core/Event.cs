/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System;

/// <summary>
/// Data for a single in-game event. Can be used as a network packet
/// var1-3 are multi-purpose variables based on the value of action
/// </summary>
public struct GameEvent
{
	public GameEvent( uint action, uint var1 = 0, uint var2 = 0, uint var3 = 0 )
	{
		this.action = action;
		Var1 = var1;
		Var2 = var2;
		Var3 = var3;
	}

	public GameEvent( uint action, int var1 = 0, int var2 = 0, int var3 = 0 )
	{
		this.action = action;
		Var1 = 0;
		Var2 = 0;
		Var3 = 0;
		Var1i = var1;
		Var2i = var2;
		Var3i = var3;
	}

	public GameEvent( uint action, float var1 = 0, float var2 = 0, float var3 = 0 )
	{
		this.action = action;
		Var1 = 0;
		Var2 = 0;
		Var3 = 0;
		Var1f = var1;
		Var2f = var2;
		Var3f = var3;
	}

	private readonly uint action;
	public GameEventAction Action => (GameEventAction)action;

	public uint Var1;
	public uint Var2;
	public uint Var3;

	public int Var1i
	{
		get => (int)Var1;
		set => Var1 = (uint)value;
	}

	public int Var2i
	{
		get => (int)Var2;
		set => Var2 = (uint)value;
	}

	public int Var3i
	{
		get => (int)Var3;
		set => Var3 = (uint)value;
	}

	public float Var1f
	{
		get => BitConverter.ToSingle( BitConverter.GetBytes( Var1 ), 0 );
		set => Var1 = BitConverter.ToUInt32( BitConverter.GetBytes( value ), 0 );
	}

	public float Var2f
	{
		get => BitConverter.ToSingle( BitConverter.GetBytes( Var2 ), 0 );
		set => Var2 = BitConverter.ToUInt32( BitConverter.GetBytes( value ), 0 );
	}

	public float Var3f
	{
		get => BitConverter.ToSingle( BitConverter.GetBytes( Var3 ), 0 );
		set => Var3 = BitConverter.ToUInt32( BitConverter.GetBytes( value ), 0 );
	}
}

/// <summary>
/// Registry of all known events
/// </summary>
public enum GameEventAction : uint
{
	INVALID = 0,
	ACK,
	UPDATE,

	/* Game actions */
	/* Camera */
	CAMERA_SET_ANG, // (pitch, yaw, roll)
	CAMERA_SET_POS, // (x, y, z)
	CAMERA_SET_FOV, // (fov,,)

	/* Currencies */
	CURRENCY_SET // (player, currency, value)
}

/// <summary>
/// Class to quickly create events for specific actions
/// </summary>
public partial class GameEventCreator { }