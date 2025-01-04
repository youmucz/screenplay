using Godot;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Blocks;

[Tool]
public partial class TextBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Text.ToString();
}