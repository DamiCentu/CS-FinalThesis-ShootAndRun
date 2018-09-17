// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/Water"
{
	Properties
	{
		[HideInInspector] _DummyTex( "", 2D ) = "white" {}
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_NormalSpeed("Normal Speed", Vector) = (2,1,0,0)
		_Normal("Normal", 2D) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Background+0" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_DummyTex;
		};

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		uniform sampler2D _Normal;
		uniform float2 _NormalSpeed;
		uniform sampler2D _DummyTex;
		uniform float _EdgeLength;

		float4 tessFunction( appdata v0, appdata v1, appdata v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 texCoordDummy22 = i.uv_DummyTex*float2( 1,1 ) + float2( 0,0 );
			float2 panner20 = ( texCoordDummy22 + 1.0 * _Time.y * _NormalSpeed);
			float3 tex2DNode6 = UnpackNormal( tex2D( _Normal, panner20 ) );
			o.Normal = tex2DNode6;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
574;150;689;571;-564.8601;-223.7229;2.709765;False;False
Node;AmplifyShaderEditor.CommentaryNode;50;-2693.047,-692.0967;Float;False;1236.024;551.9349;;7;22;21;20;6;28;27;39;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2643.047,-607.8959;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector2Node;21;-2585.152,-484.6025;Float;False;Property;_NormalSpeed;Normal Speed;8;0;2,1;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;52;-1721.42,956.0231;Float;False;1137.144;684.9301;;6;43;45;34;46;47;48;Fluid Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;56;-213.5648,-41.70351;Float;False;788.2414;530.3614;;2;1;35;Albedo Blending;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;55;-222.6778,611.8326;Float;False;731.1217;393.5186;;2;38;41;Normal Blending;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;20;-2350.372,-512.216;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.CommentaryNode;53;-1178.194,-199.9153;Float;False;780.0874;446.8341;;3;3;25;2;Flow Map Blending;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;51;-3597.949,-51.79786;Float;False;2176.409;613.9392;;10;16;19;17;18;32;23;31;5;15;14;Movement;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-1280.963,1345.374;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector2Node;3;-934.2155,69.83441;Float;False;Property;_Speed;Speed;7;0;2,1;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-1128.194,-149.9153;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT2;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;34;-1394.589,1006.024;Float;True;Property;_BricksBlending;BricksBlending;13;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;2;-669.1074,-6.081247;Float;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SaturateNode;48;-759.2761,1248.592;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;38;243.4437,745.5905;Float;True;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;35;309.6765,186.1572;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;1;-162.4688,8.296492;Float;True;Property;_Texture;Texture;6;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;47;-1031.142,1250.567;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;-1700.023,-617.1355;Float;False;myNormal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.GetLocalVarNode;41;-116.2838,661.8326;Float;False;39;0;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;45;-1625.254,1525.954;Float;False;Property;_FluidLevel;FluidLevel;15;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TFHCRemapNode;17;-2864.826,-0.1041946;Float;True;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;2.0;False;3;FLOAT;0.5;False;4;FLOAT;0.7;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-2573.882,273.2214;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.DynamicAppendNode;23;-2295.696,279.416;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;19;-2792.882,304.1413;Float;True;Property;_SinOffset;Sin Offset;10;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;15;-3547.948,22.39311;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;14;-3288.839,29.77133;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;16;-3055.626,-1.797855;Float;False;2;2;0;FLOAT;1.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1681.539,237.6474;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1702.342,-393.1618;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;43;-1671.42,1274.921;Float;True;Property;_NoiseCloud;Noise Cloud;14;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;6;-2105.545,-642.0967;Float;True;Property;_Normal;Normal;9;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector2Node;32;-2142.658,159.8855;Float;False;Property;_Tiling;Tiling;12;0;0,0;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;28;-1948.41,-271.1778;Float;False;Property;_NormalMultiply;Normal Multiply;11;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;31;-1918.554,248.514;Float;False;2;2;0;FLOAT2;0.0,0;False;1;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2042.186,751.7358;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;MyShaders/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;False;Transparent;Background;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;True;2;2;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;22;0
WireConnection;20;2;21;0
WireConnection;46;0;43;1
WireConnection;46;1;45;0
WireConnection;25;0;27;0
WireConnection;25;1;5;0
WireConnection;2;0;25;0
WireConnection;2;2;3;0
WireConnection;48;0;47;0
WireConnection;38;0;41;0
WireConnection;38;2;48;0
WireConnection;35;0;1;0
WireConnection;35;2;48;0
WireConnection;1;1;2;0
WireConnection;47;0;34;1
WireConnection;47;1;46;0
WireConnection;39;0;6;0
WireConnection;17;0;16;0
WireConnection;18;0;17;0
WireConnection;18;1;19;0
WireConnection;23;0;18;0
WireConnection;23;1;18;0
WireConnection;14;0;15;0
WireConnection;16;1;14;0
WireConnection;5;0;31;0
WireConnection;27;0;6;0
WireConnection;27;1;28;0
WireConnection;6;1;20;0
WireConnection;31;0;32;0
WireConnection;31;1;23;0
WireConnection;0;1;6;0
ASEEND*/
//CHKSM=A55C04868C9A49D816A5D7F8471E21C6F5226B97