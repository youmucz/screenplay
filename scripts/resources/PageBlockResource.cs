using Godot;
using Screenplay.Factory;

namespace Screenplay.Resources;

[Tool]
public partial class PageBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Page.ToString();
}