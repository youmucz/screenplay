using System;
using System.Reflection;
using Godot;
using Godot.Collections;
using Screenplay.Blocks;

namespace Screenplay.Factory;

[Tool]
public class BlockMetaAttribute : Attribute
{
    // 要通过GetCustomAttributes去获取属性的特性标记,需要给属性加上{get;set;}
}


[Tool]
public partial class BlockFactory : IFactory
{
    private readonly System.Collections.Generic.Dictionary<StringName, Type> _blockScenes = new ();

    public override void _Ready()
    {
        base._Ready();
        
        Initialize();
    }

    public override void Initialize()
    {
        // 获取程序集
        var assembly = Assembly.GetExecutingAssembly();
        // 获取程序集中的所有类型
        var types = assembly.GetTypes();
		
        // 遍历所有类
        foreach (var type in types)
        {
            if (!type.IsSubclassOf(typeof(BlockResource))) continue;
            
            StringName blockType = null;
            
            // 获取类中已经初始化的参数属性
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.Name == "Type") blockType = (StringName)property.GetValue(Activator.CreateInstance(type));
            }
			
            if (blockType == null) continue;
			
            // 存储block类型
            _blockScenes.TryAdd(blockType, type);
        }
    }
        
    public BlockResource CreateBlock(StringName blockType, ScreenplayResource resource, Dictionary data)
    {
        if (_blockScenes.TryGetValue(blockType, out var type))
        {
            var instance = (BlockResource)Activator.CreateInstance(type, new object[] { resource, data });
            return instance;
        }
		
        return null;
    }
}
