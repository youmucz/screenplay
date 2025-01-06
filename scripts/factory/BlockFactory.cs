using System;
using System.Reflection;
using Godot;
using Godot.Collections;
using Screenplay.Blocks;
using Screenplay.Resources;

namespace Screenplay.Factory;

[Tool]
public class BlockMetaAttribute : Attribute
{
    // 要通过GetCustomAttributes去获取属性的特性标记,需要给属性加上{get;set;}
}


[Tool]
public partial class BlockFactory : IFactory
{
    /// <summary>
    /// 全局单例
    /// </summary>
    public static BlockFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (BlockFactory)Engine.GetSingleton("BlockFactorySingleton");
            }
            return _instance;
        }
    }
    
    private static BlockFactory _instance;
    
    private readonly System.Collections.Generic.Dictionary<StringName, Type> _blockResources = new ();
    private readonly System.Collections.Generic.Dictionary<StringName, Type> _blockScenes = new ();

    public override void _Ready()
    {
        base._Ready();
        
        Engine.RegisterSingleton("BlockFactorySingleton", this);
        
        Initialize();
    }
    
    /// <summary>
    /// initialize class type
    /// </summary>
    public override void Initialize()
    {
        // 获取程序集
        var assembly = Assembly.GetExecutingAssembly();
        // 获取程序集中的所有类型
        var types = assembly.GetTypes();
		
        // 遍历所有类
        foreach (var type in types)
        {
            // var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            // 获取类上的自定义属性
            var attributes = type.GetCustomAttributes(typeof(BlockTypeAttribute), false);
            
            if (attributes.Length > 0)
            {
                // 获取第一个属性并打印其描述
                var attribute = (BlockTypeAttribute)attributes[0];
                
                if (type.IsSubclassOf(typeof(BlockScene))) _blockScenes.TryAdd(attribute.BlockType.ToString(), type);
                if (type.IsSubclassOf(typeof(BlockResource))) _blockResources.TryAdd(attribute.BlockType.ToString(), type);
            }
        }
    }
    
    /// <summary>
    /// Using reflection to call generic methods to generate block resource.
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public BlockResource AddBlockResource(StringName blockType, Dictionary data)
    {
        BlockResource blockResource = null;
        
        if (_blockResources.TryGetValue(blockType, out var type))
        {
            blockResource = (BlockResource)Activator.CreateInstance(type, new object[] { data });
        }
		
        return blockResource;
    }
    
    /// <summary>
    /// Using reflection to call generic methods to generate block scenes.
    /// </summary>
    /// <param name="blockType"></param>
    /// <param name="blockScenes"></param>
    /// <returns></returns>
    public BlockScene AddBlockScene(Elements blockType, Dictionary<Elements, PackedScene> blockScenes)
    {
        BlockScene block = null;
        
        if (_blockScenes.TryGetValue(blockType.ToString(), out var type))
        {
            var blockScene = blockScenes[blockType];
            block = (BlockScene)typeof(PackedScene).GetMethod("InstantiateOrNull")?.MakeGenericMethod(type)
                .Invoke(blockScene, [PackedScene.GenEditState.Disabled]);
        }

        var resource = AddBlockResource(blockType.ToString(), null);
        
        if (block != null) block.BlockResource = resource;
		
        return block;
    }
}
