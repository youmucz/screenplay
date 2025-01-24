using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screenplay.Utils;
using Screenplay.Blocks;
using Screenplay.Factory;

namespace Screenplay.Resources;

[Tool, BlockType(Elements.Page)]
public partial class PageBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Page.ToString();
    
    public PageBlockResource() {}
    
    public PageBlockResource(Dictionary data) : base(data)
    {
    
    }
}