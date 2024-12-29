using Godot;
using Screenplay.Factory;

namespace Screenplay.Blocks;

[Tool]
public partial class PageBlockResource : BlockResource
{
    [BlockMeta] public override StringName Type { get; set; } = Elements.Page.ToString();
}