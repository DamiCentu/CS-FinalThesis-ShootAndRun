using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

#if AMPLIFY_SHADER_EDITOR
namespace AmplifyShaderEditor
{
    [System.Serializable]
    [NodeAttributes("Node Name", "Node Category", "Node Description")]
    public class RectangleNode : ParentNode
    {
        protected override void CommonInit(int uniqueId)
        {
            base.CommonInit(uniqueId);
            AddInputPort(WirePortDataType.FLOAT2, false, "UV");
            AddInputPort(WirePortDataType.FLOAT, false, "width");
            AddInputPort(WirePortDataType.FLOAT, false, "height");
            AddOutputPort(WirePortDataType.FLOAT, "Out");
        }
    }
}
#endif
