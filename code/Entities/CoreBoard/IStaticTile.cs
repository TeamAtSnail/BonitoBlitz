namespace BonitoBlitz.Entities.CoreBoard;

/// <summary>
/// Tile with 1 static next tile option
/// </summary>
public interface IStaticTile
{
	public string NextName { get; }

	public BaseTile NextTile => BaseTile.FromName( NextName );
}
