using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Blocks;

[Tool, BlockType(Elements.Text)]
public partial class TextBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Text.ToString();
    
    public TextBlockResource() {}
    
    public TextBlockResource(Dictionary data) : base(data)
    {
    
    }
}