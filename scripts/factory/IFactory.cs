using Godot;
using System;

namespace Screenplay.Factory;

public abstract partial class IFactory : Node
{
    public abstract void Initialize();
}
