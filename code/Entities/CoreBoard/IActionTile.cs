using libblitz;

namespace BonitoBlitz.Entities.CoreBoard;

/// <summary>
/// Tile with pass / stand actions (don't use this to push/pop activities)
/// </summary>
public interface IActionTile
{
	public void OnPass( GameMember member );
	public void OnStand( GameMember member );
}
