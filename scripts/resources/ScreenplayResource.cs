using System;
using Godot;
using Godot.Collections;
using Screenplay.Utils;

namespace Screenplay.Resources;


[Tool]
public partial class ScreenplayResource : Resource
{
    [Export] public string FileDir;
    [Export] public string Filename;
    [Export] public string Filepath;
    [Export] public string EngineVersion = Engine.GetVersionInfo()["string"].ToString();
    
    [Export] public Array<Dictionary> PageData =
    [
        new()
        {
            { "BlockGuid", Guid.NewGuid().ToString() },
            { "BlockType", Elements.Page.ToString() },
            { "BlockParent", "" },
            { "Content", new Godot.Collections.Array() },
            { "Properties", new Dictionary() },
        }
    ];
}
