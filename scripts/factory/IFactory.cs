using Godot;
using System;

namespace Screenplay.Factory;

[Tool]
public abstract partial class IFactory : Node
{
    public abstract void Initialize();
}
