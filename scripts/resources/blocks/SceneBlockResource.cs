using Godot;
using Godot.Collections;
using Screenplay.Utils;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Blocks;

[Tool, BlockType(Elements.Scene)]
public partial class SceneBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Text.ToString();
    
    public SceneBlockResource() {}
    
    public SceneBlockResource(Dictionary data) : base(data)
    {
    
    }
}