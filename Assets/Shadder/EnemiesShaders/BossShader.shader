// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BossShader"
{
	Properties
	{
		_boss_03Default_AlbedoTransparency("boss_03 - Default_AlbedoTransparency", 2D) = "white" {}
		_boss_03Default_AO("boss_03 - Default_AO", 2D) = "white" {}
		_SegundaFase("SegundaFase", Range( 0 , 1)) = 0
		_maskBossShield("mask Boss Shield", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Shield("Shield", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _boss_03Default_AlbedoTransparency;
		uniform float4 _boss_03Default_AlbedoTransparency_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _SegundaFase;
		uniform sampler2D _maskBossShield;
		uniform float4 _maskBossShield_ST;
		uniform float _Shield;
		uniform sampler2D _boss_03Default_AO;
		uniform float4 _boss_03Default_AO_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_boss_03Default_AlbedoTransparency = i.uv_texcoord * _boss_03Default_AlbedoTransparency_ST.xy + _boss_03Default_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _boss_03Default_AlbedoTransparency, uv_boss_03Default_AlbedoTransparency ).rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float4 tex2DNode44 = tex2D( _TextureSample0, uv_TextureSample0 );
			float4 lerpResult25 = lerp( tex2DNode44 , ( tex2DNode44 * float4(1,0,0,0) ) , _SegundaFase);
			float2 uv_maskBossShield = i.uv_texcoord * _maskBossShield_ST.xy + _maskBossShield_ST.zw;
			float4 lerpResult36 = lerp( lerpResult25 , ( lerpResult25 + ( tex2D( _maskBossShield, uv_maskBossShield ) * 4.0 ) ) , _Shield);
			o.Emission = lerpResult36.rgb;
			float temp_output_21_0 = 1.0;
			o.Metallic = temp_output_21_0;
			o.Smoothness = temp_output_21_0;
			float2 uv_boss_03Default_AO = i.uv_texcoord * _boss_03Default_AO_ST.xy + _boss_03Default_AO_ST.zw;
			o.Occlusion = tex2D( _boss_03Default_AO, uv_boss_03Default_AO ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
501;122;602;571;2451.478;1666.555;5.391563;False;False
Node;AmplifyShaderEditor.ColorNode;14;-2057.017,-47.60217;Float;False;Constant;_Color0;Color 0;5;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;44;-2226.257,-371.668;Float;True;Property;_TextureSample0;Texture Sample 0;4;0;Create;True;0;0;False;0;None;1554e9d28391ea343b9d8c0f6f383b43;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1724.238,-66.89265;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1827.363,157.1292;Float;False;Property;_SegundaFase;SegundaFase;2;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1673.206,510.8411;Float;False;Constant;_Float3;Float 3;8;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;34;-1926.651,287.101;Float;True;Property;_maskBossShield;mask Boss Shield;3;0;Create;True;0;0;False;0;e5786154f0e886145b7301a2d5f4943d;e5786154f0e886145b7301a2d5f4943d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;25;-1419.165,-49.15578;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1446.269,280.7378;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-1159.858,69.45193;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-794.6477,204.546;Float;False;Property;_Shield;Shield;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-673.966,787.2915;Float;True;Property;_boss_03Default_AO;boss_03 - Default_AO;1;0;Create;True;0;0;False;0;d5a2621fd75a57c4c8ebadea0a583905;eba8491110e50be4c892fdd2b250f502;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-517.1669,-440.8604;Float;True;Property;_boss_03Default_AlbedoTransparency;boss_03 - Default_AlbedoTransparency;0;0;Create;True;0;0;False;0;1aa8aeccd8aa8f34ea74037f615b0f7f;1aa8aeccd8aa8f34ea74037f615b0f7f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-294.9938,151.07;Float;False;Constant;_metalic;metalic;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;36;-463.0743,2.270448;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;BossShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;44;0
WireConnection;45;1;14;0
WireConnection;25;0;44;0
WireConnection;25;1;45;0
WireConnection;25;2;26;0
WireConnection;38;0;34;0
WireConnection;38;1;39;0
WireConnection;42;0;25;0
WireConnection;42;1;38;0
WireConnection;36;0;25;0
WireConnection;36;1;42;0
WireConnection;36;2;37;0
WireConnection;0;0;1;0
WireConnection;0;2;36;0
WireConnection;0;3;21;0
WireConnection;0;4;21;0
WireConnection;0;5;2;0
ASEEND*/
//CHKSM=DA4F17EFDC921BAB7AC7AC85C7C1B8F5988AF2AE