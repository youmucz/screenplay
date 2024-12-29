using Godot;
using Screenplay.Factory;

namespace Screenplay.Blocks;

[Tool]
public partial class DialogueBlockResource : BlockResource
{
    [BlockMeta] public override StringName Type { get; set; } = Elements.Dialogue.ToString();
}
