using Godot;
using System;

[Tool]
public partial class BlockScene : MarginContainer
{
    public virtual bool CanDestroy() { return true; }
}
