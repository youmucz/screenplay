﻿using Godot;
using Screenplay.Factory;

namespace Screenplay.Blocks;

[Tool]
public partial class DialogueBlockResource : BlockResource
{
    [BlockMeta] public override StringName BlockType { get; set; } = Elements.Dialogue.ToString();
}
