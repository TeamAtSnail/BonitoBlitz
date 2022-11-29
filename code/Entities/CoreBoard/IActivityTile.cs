using BonitoBlitz.Activities.CoreBoard;

namespace BonitoBlitz.Entities.CoreBoard;

/// <summary>
/// Blocking tile; ends batch of tile animations and starts its own activity
/// </summary>
public interface IActivityTile
{
	public string ActivityName { get; set; }

}
