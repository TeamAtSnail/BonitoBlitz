/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox;
using SandboxEditor;

namespace BonitoBlitz.CoreActivities.Board;

[Library( "bb_board_camera" ), HammerEntity]
[Title( "Board Camera" ), Category( "Gameplay" ), Icon( "place" )]
[Description( "Board camera spot" )]
[EditorModel( "models/editor/camera.vmdl" )]
public class BoardCamera : Entity { }