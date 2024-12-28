using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Screenplay.Factory;

namespace Screenplay.Blocks;


[Tool]
public partial class BlockResource : Resource
{
    [BlockMeta] public virtual StringName Type { get; set; }
    
    /// <summary>需要存储到Resource本地文件里的参数和参数值。</summary>
    private readonly List<PropertyInfo> _metaPropertyInfo;

    #region 构造函数

    public BlockResource() {}
    
    /// <summary>
    /// 编辑器模式创建节点 @base
    /// </summary>
    /// <param name="screenplayResource"></param>
    /// <param name="blockScene"></param>
    /// <param name="data"></param>
    public BlockResource(ScreenplayResource screenplayResource, BlockScene blockScene, Dictionary data)
    {
        // DialogueGraphResource = screenplayResource;
    
        Deserialize(data);
    
        // 加载用作序列化的meta数据
        _metaPropertyInfo = new List<PropertyInfo>();
        _metaPropertyInfo = GetType().GetProperties()
            .Where(t => t.GetCustomAttributes(typeof(BlockMetaAttribute), true).Any())
            .ToList();
    }

    #endregion

    #region 序列化/反序列化

    /// <summary>
    /// 序列化meta数据,用来存储到resource本地文件上
    /// </summary>
    /// <returns></returns>
    public Dictionary Serialize()
    {
        var data = new Dictionary();
    
        foreach (var property in _metaPropertyInfo)
        {
            data.Add(property.Name, Get(property.Name));
        }

        return data;
    }

    /// <summary>
    /// 反序列化数据
    /// </summary>
    /// <param name="data"></param>
    public void Deserialize(Dictionary data)
    {
        if (data == null) return;
    
        foreach (var kvp in data)
        {
            Set((StringName)kvp.Key, kvp.Value);
        }
    }

    #endregion

}
