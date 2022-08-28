/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0.Events;
using System;

public struct Message16
{
	private ulong ul1;
	private ulong ul2;

	/* ulong */
	public ulong Ulong1
	{
		get => ul1;
		set => ul1 = value;
	}

	public ulong Ulong2
	{
		get => ul2;
		set => ul2 = value;
	}

	/* uint */
	public uint Uint1
	{
		get => (uint)(ul1 & 0xffffffff);
		set => ul1 = ((ulong)value) | (((ulong)Uint2) << 32);
	}

	public uint Uint2
	{
		get => (uint)((ul1 >> 32) & 0xffffffff);
		set => ul1 = ((ulong)Uint1) | (((ulong)value) << 32);
	}

	public uint Uint3
	{
		get => (uint)(ul2 & 0xffffffff);
		set => ul2 = ((ulong)value) | (((ulong)Uint4) << 32);
	}

	public uint Uint4
	{
		get => (uint)((ul2 >> 32) & 0xffffffff);
		set => ul2 = ((ulong)Uint3) | (((ulong)value) << 32);
	}

	public short Ushort1 => (short)(ul1 & 0xffffffff);
	public short Ushort2 => (short)((ul1 >> 16) & 0xffffffff);
	public short Ushort3 => (short)((ul1 >> 32) & 0xffffffff);
	public short Ushort4 => (short)((ul1 >> 48) & 0xffffffff);
	public short Ushort5 => (short)(ul2 & 0xffffffff);
	public short Ushort6 => (short)((ul2 >> 16) & 0xffffffff);
	public short Ushort7 => (short)((ul2 >> 32) & 0xffffffff);
	public short Ushort8 => (short)((ul2 >> 48) & 0xffffffff);

	/* long */
	public long Long1
	{
		get => (long)Ulong1;
		set => Ulong1 = (ulong)value;
	}

	public long Long2
	{
		get => (long)Ulong2;
		set => Ulong2 = (ulong)value;
	}

	/* int */
	public int Int1
	{
		get => (int)(ul1 & 0xffffffff);
		set => Uint1 = (uint)value;
	}
	public int Int2
	{
		get => (int)((ul1 >> 32) & 0xffffffff);
		set => Uint2 = (uint)value;
	}
	public int Int3
	{
		get => (int)(ul2 & 0xffffffff);
		set => Uint3 = (uint)value;
	}
	public int Int4
	{
		get => (int)((ul2 >> 32) & 0xffffffff);
		set => Uint4 = (uint)value;
	}

	/* float */
	/* : ( */
	public float Float1
	{
		get => BitConverter.ToSingle( BitConverter.GetBytes( Uint1 ), 0 );
		set => Uint1 = BitConverter.ToUInt32( BitConverter.GetBytes( value ), 0 );
	}
	public float Float2
	{
		get => BitConverter.ToSingle( BitConverter.GetBytes( Uint2 ), 0 );
		set => Uint2 = BitConverter.ToUInt32( BitConverter.GetBytes( value ), 0 );
	}
	public float Float3
	{
		get => BitConverter.ToSingle( BitConverter.GetBytes( Uint3 ), 0 );
		set => Uint3 = BitConverter.ToUInt32( BitConverter.GetBytes( value ), 0 );
	}
	public float Float4
	{
		get => BitConverter.ToSingle( BitConverter.GetBytes( Uint4 ), 0 );
		set => Uint4 = BitConverter.ToUInt32( BitConverter.GetBytes( value ), 0 );
	}

	public void SetByUint( uint v1, uint v2 = 0, uint v3 = 0, uint v4 = 0 )
	{
		if ( v1 == 0 && v2 == 0 )
		{
			ul1 = 0;
		}
		else
		{
			ul1 = ((ulong)v1) | (((ulong)v2) << 32);
		}

		if ( v3 == 0 && v4 == 0 )
		{
			ul2 = 0;
		}
		else
		{
			ul2 = ((ulong)v3) | (((ulong)v4) << 32);
		}
	}
}