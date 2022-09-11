/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using System;
using System.Text.Json.Serialization;
using Sandbox;

public abstract partial class Player : AnimatedEntity
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
	public long PlayerId { get; set; }

	/// <summary>
	/// Initial constructor for Player
	/// </summary>
	public Player() => Uid = Guid.NewGuid();

	/// <summary>
	/// Set active client for player
	/// </summary>
	/// <param name="client">New active client</param>
	public void SetClient( Client client ) => PlayerId = client.PlayerId;

	public abstract void Initialize();
}