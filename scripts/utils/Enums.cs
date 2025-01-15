using Godot;
using System;
namespace Screenplay.Utils;


public enum Elements
{
    Text,
    Page,
    Document,
    
    // screenplay
    Screenplay,
    Scene,
    Action,
    Option,
    Dialogue,
    Character,
    Transition,
    Parenthesis,
}


public struct PlaceholderText
{
    public const string Text = "Write something, or press '/' for commands...";
}

