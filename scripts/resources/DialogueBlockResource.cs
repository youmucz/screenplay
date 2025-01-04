using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screenplay.Factory;
using Screenplay.Resources;

namespace Screenplay.Blocks;

[Tool, BlockType(Elements.Dialogue)]
public partial class DialogueBlockResource : BlockResource
{
    [BlockMeta] public override string BlockType { get; set; } = Elements.Dialogue.ToString();
    
    public DialogueBlockResource() {}
    
    public DialogueBlockResource(Dictionary data) : base(data)
    {
    
    }
}
