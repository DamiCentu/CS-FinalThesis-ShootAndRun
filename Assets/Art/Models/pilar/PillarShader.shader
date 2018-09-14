// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PillarShader"
{
	Properties
	{
		_pilar_pilar_AlbedoTransparency("pilar_pilar_AlbedoTransparency", 2D) = "white" {}
		_pilar_pilar_AO("pilar_pilar_AO", 2D) = "white" {}
		_pilar_pilar_Emission("pilar_pilar_Emission", 2D) = "white" {}
		_pilar_pilar_MetallicSmoothness("pilar_pilar_MetallicSmoothness", 2D) = "white" {}
		_pilar_pilar_Normal("pilar_pilar_Normal", 2D) = "white" {}
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

		uniform sampler2D _pilar_pilar_Normal;
		uniform float4 _pilar_pilar_Normal_ST;
		uniform sampler2D _pilar_pilar_AlbedoTransparency;
		uniform float4 _pilar_pilar_AlbedoTransparency_ST;
		uniform sampler2D _pilar_pilar_Emission;
		uniform float4 _pilar_pilar_Emission_ST;
		uniform sampler2D _pilar_pilar_MetallicSmoothness;
		uniform float4 _pilar_pilar_MetallicSmoothness_ST;
		uniform sampler2D _pilar_pilar_AO;
		uniform float4 _pilar_pilar_AO_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_pilar_pilar_Normal = i.uv_texcoord * _pilar_pilar_Normal_ST.xy + _pilar_pilar_Normal_ST.zw;
			o.Normal = tex2D( _pilar_pilar_Normal, uv_pilar_pilar_Normal ).rgb;
			float2 uv_pilar_pilar_AlbedoTransparency = i.uv_texcoord * _pilar_pilar_AlbedoTransparency_ST.xy + _pilar_pilar_AlbedoTransparency_ST.zw;
			o.Albedo = ( 0.83 * tex2D( _pilar_pilar_AlbedoTransparency, uv_pilar_pilar_AlbedoTransparency ) ).rgb;
			float2 uv_pilar_pilar_Emission = i.uv_texcoord * _pilar_pilar_Emission_ST.xy + _pilar_pilar_Emission_ST.zw;
			o.Emission = ( 6.0 * ( abs( sin( _Time.y ) ) * tex2D( _pilar_pilar_Emission, uv_pilar_pilar_Emission ) ) ).rgb;
			float2 uv_pilar_pilar_MetallicSmoothness = i.uv_texcoord * _pilar_pilar_MetallicSmoothness_ST.xy + _pilar_pilar_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _pilar_pilar_MetallicSmoothness, uv_pilar_pilar_MetallicSmoothness ).r;
			float2 uv_pilar_pilar_AO = i.uv_texcoord * _pilar_pilar_AO_ST.xy + _pilar_pilar_AO_ST.zw;
			o.Occlusion = tex2D( _pilar_pilar_AO, uv_pilar_pilar_AO ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
590;104;689;571;762.4387;546.7962;1.358337;False;False
Node;AmplifyShaderEditor.SimpleTimeNode;9;-1020.732,-29.16347;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SinOpNode;8;-815.4319,8.587743;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;10;-622.4326,66.46624;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-1039.166,163.5473;Float;True;Property;_pilar_pilar_Emission;pilar_pilar_Emission;2;0;Assets/Art/Models/pilar/pilar_pilar_Emission.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;-436.2303,109.6968;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;7;-601.7604,-92.8472;Float;False;Constant;_Float0;Float 0;5;0;6;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;1;-721.5131,-305.1329;Float;True;Property;_pilar_pilar_AlbedoTransparency;pilar_pilar_AlbedoTransparency;0;0;Assets/Art/Models/pilar/pilar_pilar_AlbedoTransparency.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;14;-497.8434,-378.5955;Float;False;Constant;_Float2;Float 2;5;0;0.83;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-308.6101,-224.6232;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;4;-188.723,-373.7151;Float;True;Property;_pilar_pilar_MetallicSmoothness;pilar_pilar_MetallicSmoothness;3;0;Assets/Art/Models/pilar/pilar_pilar_MetallicSmoothness.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-404.0059,666.3918;Float;True;Property;_pilar_pilar_AO;pilar_pilar_AO;1;0;Assets/Art/Models/pilar/pilar_pilar_AO.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;5;-455.958,470.2346;Float;True;Property;_pilar_pilar_Normal;pilar_pilar_Normal;4;0;Assets/Art/Models/pilar/pilar_pilar_Normal.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-285.393,43.15569;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;PillarShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;9;0
WireConnection;10;0;8;0
WireConnection;11;0;10;0
WireConnection;11;1;3;0
WireConnection;15;0;14;0
WireConnection;15;1;1;0
WireConnection;6;0;7;0
WireConnection;6;1;11;0
WireConnection;0;0;15;0
WireConnection;0;1;5;0
WireConnection;0;2;6;0
WireConnection;0;3;4;0
WireConnection;0;5;2;0
ASEEND*/
//CHKSM=BA23D6EBC6DAD696D66A28A38057A3C328BDB829