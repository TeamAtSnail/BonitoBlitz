/*
 * part of the gm0 (w.i.p name) gamemode
 * - last updated indev:2
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;
using Sandbox;

public partial class BoardPlayer : BasePlayer
{
	public override void Initialize()
	{
		// Load citizen model
		Model = Model.Load( "models/citizen/citizen.vmdl" );

		// Load player clothing
		ClothingContainer clothing = new();
		clothing.LoadFromClient( Client );

		// Dress player
		clothing.DressEntity( this );
	}
}