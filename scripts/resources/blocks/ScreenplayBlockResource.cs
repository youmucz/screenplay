using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screenplay.Utils;
using Screenplay.Blocks;
using Screenplay.Factory;

namespace Screenplay.Resources;

[Tool, BlockType(Elements.Screenplay)]
public partial class ScreenplayBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Screenplay.ToString();
    
    public ScreenplayBlockResource() {}
    
    public ScreenplayBlockResource(Dictionary data) : base(data)
    {
    
    }
}