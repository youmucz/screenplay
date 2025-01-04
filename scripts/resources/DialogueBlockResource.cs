using Godot;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Blocks;

[Tool]
public partial class DialogueBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Dialogue.ToString();
}
