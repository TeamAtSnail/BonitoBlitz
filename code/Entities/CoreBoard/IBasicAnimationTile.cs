using libblitz;
using Sandbox;

namespace BonitoBlitz.Entities.CoreBoard;

/// <summary>
/// Tile with basic 3 function animation (start, update, end)
/// </summary>
public interface IBasicAnimationTile
{
	public float AnimationLength { get; }

	public void StartAnimation( GameMember member, Entity to );
	public void UpdateAnimation( GameMember member, Entity to, float progress );
	public void EndAnimation( GameMember member, Entity to );
}
