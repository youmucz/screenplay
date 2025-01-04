using Godot;
using Godot.Collections;

namespace Screenplay.Resources;


[Tool]
public partial class ScreenplayResource : Resource
{
    [Export] public string FileDir;
    [Export] public string Filename;
    [Export] public string Filepath;
    [Export] public string EngineVersion = Engine.GetVersionInfo()["string"].ToString();
    
    [Export] public Array<Dictionary> PageData = new (){
        new Dictionary
        {
            {"BlockGuid", ""}, 
            {"BlockType", Elements.Text.ToString()}, 
            {"BlockParent", ""},
            {"Content", new Array()},
            {"Properties", new Dictionary()},
        }
    };
}
