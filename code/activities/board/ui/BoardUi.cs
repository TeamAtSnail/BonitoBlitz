/*
 * part of the BonitoBlitz (w.i.p name) gamemode
 * - lotuspar, 2022 (github.com/lotuspar)
 */
using Sandbox.UI;

namespace BonitoBlitz.Board;

[UseTemplate]
public partial class BoardUiPanel : Panel
{
    public BoardActivity Activity => libblitz.Game.Current.Activity as BoardActivity;

    
}