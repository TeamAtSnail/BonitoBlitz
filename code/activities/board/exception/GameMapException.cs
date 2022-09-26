/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace BonitoBlitz.Board.Error;
using System;

public class GameMapException : Exception
{
	public GameMapException()
	{
	}

	public GameMapException( string message )
		: base( message )
	{
	}

	public GameMapException( string message, Exception inner )
		: base( message, inner )
	{
	}
}