using Godot;
using Screenplay.Factory;

namespace Screenplay.Blocks;

[Tool]
public partial class TextBlockResource : BlockResource
{
    [BlockMeta] public override StringName Type { get; set; } = Elements.Text.ToString();
}