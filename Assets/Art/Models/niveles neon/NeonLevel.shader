// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "NeonLevel"
{
	Properties
	{
		_Brillo("Brillo", Float) = 0.5
		_gillacoloresmask("gilla colores mask", 2D) = "white" {}
		_Float3("Float 3", Float) = 0
		_Speed("Speed", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _gillacoloresmask;
		uniform float4 _gillacoloresmask_ST;
		uniform float _Speed;
		uniform float _Brillo;
		uniform float _Float3;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_gillacoloresmask = i.uv_texcoord * _gillacoloresmask_ST.xy + _gillacoloresmask_ST.zw;
			float mulTime8 = _Time.y * _Speed;
			float mulTime22 = _Time.y * _Float3;
			o.Emission = ( ( ( tex2D( _gillacoloresmask, uv_gillacoloresmask ) * saturate( ( abs( sin( mulTime8 ) ) + 0.88 ) ) ) * _Brillo ) * (0.2 + (sin( mulTime22 ) - -1.0) * (2.0 - 0.2) / (1.0 - -1.0)) ).rgb;
			o.Metallic = 1.0;
			o.Smoothness = 0.8;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
674;112;685;556;1237.269;80.00597;1.3;False;False
Node;AmplifyShaderEditor.RangedFloatNode;9;-2135.822,105.6569;Float;False;Property;_Speed;Speed;4;0;Create;True;0;0;False;0;0;1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-1949.817,112.5111;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;7;-1722.089,133.9561;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;12;-1511.352,91.24954;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1470.455,210.131;Float;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;False;0;0.88;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-1233.247,345.3889;Float;False;Property;_Float3;Float 3;3;0;Create;True;0;0;False;0;0;1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-1354.117,93.79315;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;14;-1212.755,17.4463;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1533.575,-182.8405;Float;True;Property;_gillacoloresmask;gilla colores mask;2;0;Create;True;0;0;False;0;a840c6df8e801b3408379d965ba3edf9;a840c6df8e801b3408379d965ba3edf9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;22;-1049.841,348.343;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-983.0209,-93.09128;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;23;-853.5131,317.4881;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-999.1182,134.7109;Float;False;Property;_Brillo;Brillo;1;0;Create;True;0;0;False;0;0.5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-722.8061,-26.9771;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;25;-671.3041,257.526;Float;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0.2;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-227.4326,119.2187;Float;False;Constant;_Float1;Float 1;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-663.1821,-276.5805;Float;True;Property;_gillacolores;gilla colores;0;0;Create;True;0;0;False;0;None;8222b223e7fe1ea4cad90cf4e25ea22b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-224.9561,230.6557;Float;False;Constant;_Float2;Float 2;4;0;Create;True;0;0;False;0;0.8;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-430.9705,75.09412;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;NeonLevel;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;9;0
WireConnection;7;0;8;0
WireConnection;12;0;7;0
WireConnection;17;0;12;0
WireConnection;17;1;18;0
WireConnection;14;0;17;0
WireConnection;22;0;21;0
WireConnection;10;0;6;0
WireConnection;10;1;14;0
WireConnection;23;0;22;0
WireConnection;5;0;10;0
WireConnection;5;1;3;0
WireConnection;25;0;23;0
WireConnection;24;0;5;0
WireConnection;24;1;25;0
WireConnection;0;2;24;0
WireConnection;0;3;19;0
WireConnection;0;4;20;0
ASEEND*/
//CHKSM=979740C157BEA2114E7CDE47D66C9608FDB1B323