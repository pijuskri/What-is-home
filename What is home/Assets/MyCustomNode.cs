using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.ShaderGraph;
using System.Reflection;
[Title("Custom","My Custom Node")]
public class MyCustomNode : CodeFunctionNode
{
    public MyCustomNode()
    {
        name = "My Custom Node";
    }

    protected override MethodInfo GetFunctionToConvert()
    {
        //throw new System.NotImplementedException();
        return GetType().GetMethod("MyCustomFunction",
            BindingFlags.Static | BindingFlags.NonPublic);
    }
    static string MyCustomFunction(
        [Slot(0, Binding.None)] Vector1 IOR,
        [Slot(1, Binding.None)] Vector3 ViewDirection,
        [Slot(2, Binding.None)] Vector3 NormalDirection,
        [Slot(3, Binding.None)] out Vector3 Out)
    {
        Out = Vector3.zero;
        return
            @"
    {
       Out = refract(normalize(ViewDirection),normalize(NormalDirection),IOR);
    }";
    }
}
