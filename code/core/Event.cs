/*
 * part of the gm0 (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
namespace gm0;

/// <summary>
/// Data for a single in-game event. Can be used as a network packet
/// var1-3 are multi-purpose variables based on the value of action
/// </summary>
public struct GameEvent {
    public GameEvent(uint action, uint var1 = 0, uint var2 = 0, uint var3 = 0) {
		Action = action;
        Var1 = var1;
        Var2 = var2;
        Var3 = var3;
    }
    
    public readonly uint Action;
    public uint Var1;
    public uint Var2;
    public uint Var3;
}

/// <summary>
/// Server-side GameEvent with index / history number
/// </summary>
public struct RegisteredGameEvent {
	public RegisteredGameEvent(uint index, GameEvent evt) {
		Index = index;
		Event = evt;
	}
	public readonly uint Index;
	public readonly GameEvent Event;
}

/// <summary>
/// Registry of all known events
/// </summary>
public enum GameEventAction : uint {
    INVALID = 0,
    ACK,

    /* Game actions */
    /* Camera */
    CAMERA_SET_ANGLE,
    CAMERA_SET_POS,
    CAMERA_SET_FOV
}

/// <summary>
/// Class to quickly create events for specific actions
/// </summary>
public partial class GameEventCreator {}