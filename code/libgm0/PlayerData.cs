/*
 * part of the gm0 (w.i.p name) gamemode
 * library used across the board gamemode & minigames
 * - last updated indev:3
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace libgm0;
using System;
using System.Collections.Generic;
using Sandbox;

public partial class PlayerData : BaseNetworkable
{
	public PlayerData()
	{
		Uid = Guid.NewGuid();
		PlayedBy = new();
		Extensions = new Dictionary<string, string>();
	}

	/// <summary>
	/// IDs of clients that have used this player
	/// </summary>
	public List<long> PlayedBy { get; set; }

	[Net]
	public Guid Uid { get; set; }
	[Net]
	public int Coins { get; set; }
	[Net]
	public int Stars { get; set; }
	[Net]
	public long PlayerId { get; set; }
	[Net]
	public string DisplayName { get; set; }
	[Net]
	public Dictionary<string, string> Extensions { get; set; }
}