using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screenplay.Utils;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Blocks;

[Tool, BlockType(Elements.Action)]
public partial class ActionBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Text.ToString();
    
    public ActionBlockResource() {}
    
    public ActionBlockResource(Dictionary data) : base(data)
    {
    
    }
}