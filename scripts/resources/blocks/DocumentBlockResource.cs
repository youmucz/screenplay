using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screenplay.Utils;
using Screenplay.Blocks;
using Screenplay.Factory;

namespace Screenplay.Resources;

[Tool, BlockType(Elements.Document)]
public partial class DocumentBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Document.ToString();
    
    public DocumentBlockResource() {}
    
    public DocumentBlockResource(Dictionary data) : base(data)
    {
    
    }
}