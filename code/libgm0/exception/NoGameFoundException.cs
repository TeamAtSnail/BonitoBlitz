/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using System;

public class NoGameFoundException : Exception
{
	public NoGameFoundException()
	{
	}

	public NoGameFoundException( string message )
		: base( message )
	{
	}

	public NoGameFoundException( string message, Exception inner )
		: base( message, inner )
	{
	}
}