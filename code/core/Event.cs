/// <summary>
/// part of the gm0 (w.i.p name) gamemode
/// - lotuspar, 2022 (github.com/lotuspar)
/// </summary>
namespace gm0;

/// <summary>
/// Data for a single in-game event. Can be used as a network packet
/// var1 & var2 are multi-purpose variables based on the value of action
/// </summary>
public struct GameEvent {
    public GameEvent(uint uid, uint action, uint var1 = 0, uint var2 = 0, uint var3 = 0) {
        this.Uid = uid;
        this.Action = action;
        this.Var1 = var1;
        this.Var2 = var2;
        this.Var3 = var3;
    }

    public readonly uint Uid;
    public readonly uint Action;
    public uint Var1;
    public uint Var2;
    public uint Var3;
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
/// Class to quickly create event for specific action
/// </summary>
public partial class GameEventCreator {
    /// <summary>
    /// Acknowledge processing of a received event
    /// </summary>
    /// <param name="uid">New event UID</param>
    /// <param name="acknowledgedEventUid">Received event UID</param>
    /// <param name="status">Status code to provide back (0 always OK)</param>
    /// <returns>New event</returns>
	public static GameEvent Acknowledgement(uint uid, uint acknowledgedEventUid = 0, uint status = 0) {
        return new GameEvent(uid, (uint) GameEventAction.ACK, acknowledgedEventUid, status);
    }
}