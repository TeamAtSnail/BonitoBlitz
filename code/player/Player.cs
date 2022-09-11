/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System;
using System.Text.Json.Serialization;
using Sandbox;

public partial class Player : AnimatedEntity
{
	[Net]
	[JsonInclude]
	public Guid Uid { get; set; }
	[Net]
	[JsonInclude]
	public int Coins { get; set; }
	[Net]
	[JsonInclude]
	public int Stars { get; set; }
	[Net]
	[JsonInclude]
	public new Client Client { get; set; }

	/// <summary>
	/// Initial constructor for Player
	/// </summary>
	/// <param name="client">Initial active client</param>
	public Player( Client client )
	{
		Uid = Guid.NewGuid();
		SetNewClient( client );
	}

	/// <summary>
	/// Set active client for player
	/// </summary>
	/// <param name="client">New active client</param>
	public void SetNewClient( Client client )
	{
		Host.AssertServer( "SetNewClient" );

		Client = client;

		// Load entity model
		Model = Model.Load( "models/citizen/citizen.vmdl" );

		// Dress entity
		ClothingContainer clothing = new();
		clothing.LoadFromClient( client );
		clothing.DressEntity( this );
	}
}