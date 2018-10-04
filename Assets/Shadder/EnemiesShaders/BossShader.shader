// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BossShader"
{
	Properties
	{
		_boss_03Default_AlbedoTransparency("boss_03 - Default_AlbedoTransparency", 2D) = "white" {}
		_boss_03Default_AO("boss_03 - Default_AO", 2D) = "white" {}
		_boss_03Default_Normal("boss_03 - Default_Normal", 2D) = "white" {}
		_texturaforcefield("textura forcefield", 2D) = "white" {}
		_maskBosseye("mask Boss eye", 2D) = "white" {}
		_SegundaFase("SegundaFase", Range( 0 , 1)) = 0
		_maskBossShield("mask Boss Shield", 2D) = "white" {}
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

		uniform sampler2D _boss_03Default_Normal;
		uniform float4 _boss_03Default_Normal_ST;
		uniform sampler2D _boss_03Default_AlbedoTransparency;
		uniform float4 _boss_03Default_AlbedoTransparency_ST;
		uniform sampler2D _maskBosseye;
		uniform float4 _maskBosseye_ST;
		uniform float _SegundaFase;
		uniform sampler2D _texturaforcefield;
		uniform float4 _texturaforcefield_ST;
		uniform sampler2D _maskBossShield;
		uniform float4 _maskBossShield_ST;
		uniform float _Shield;
		uniform sampler2D _boss_03Default_AO;
		uniform float4 _boss_03Default_AO_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_boss_03Default_Normal = i.uv_texcoord * _boss_03Default_Normal_ST.xy + _boss_03Default_Normal_ST.zw;
			o.Normal = tex2D( _boss_03Default_Normal, uv_boss_03Default_Normal ).rgb;
			float2 uv_boss_03Default_AlbedoTransparency = i.uv_texcoord * _boss_03Default_AlbedoTransparency_ST.xy + _boss_03Default_AlbedoTransparency_ST.zw;
			float2 uv_maskBosseye = i.uv_texcoord * _maskBosseye_ST.xy + _maskBosseye_ST.zw;
			float4 _Color0 = float4(1,0,0,0);
			float4 temp_output_15_0 = ( tex2D( _maskBosseye, uv_maskBosseye ) * _Color0 );
			float4 temp_output_20_0 = ( tex2D( _boss_03Default_AlbedoTransparency, uv_boss_03Default_AlbedoTransparency ) + temp_output_15_0 );
			float4 lerpResult31 = lerp( temp_output_20_0 , ( 0.18 * temp_output_20_0 ) , _SegundaFase);
			o.Albedo = lerpResult31.rgb;
			float4 temp_output_18_0 = ( 30.36 * temp_output_15_0 );
			float2 uv_texturaforcefield = i.uv_texcoord * _texturaforcefield_ST.xy + _texturaforcefield_ST.zw;
			float4 lerpResult25 = lerp( temp_output_18_0 , ( temp_output_18_0 + ( _Color0 * tex2D( _texturaforcefield, uv_texturaforcefield ).r ) ) , _SegundaFase);
			float2 uv_maskBossShield = i.uv_texcoord * _maskBossShield_ST.xy + _maskBossShield_ST.zw;
			float4 lerpResult36 = lerp( lerpResult25 , ( lerpResult25 + ( tex2D( _maskBossShield, uv_maskBossShield ) * 4.0 ) ) , _Shield);
			o.Emission = lerpResult36.rgb;
			o.Metallic = 0.58;
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
Version=13701
352;129;668;571;1061.807;250.8939;1.647612;False;False
Node;AmplifyShaderEditor.SamplerNode;16;-1684.764,-396.4546;Float;True;Property;_maskBosseye;mask Boss eye;4;0;Assets/Art/texturas enemigos/Boss/mask Boss eye.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;14;-1614.064,-113.43;Float;False;Constant;_Color0;Color 0;5;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1246.305,-153.0557;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;19;-1001.536,-141.2593;Float;False;Constant;_Float0;Float 0;5;0;30.36;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;22;-1357.442,171.0205;Float;True;Property;_texturaforcefield;textura forcefield;3;0;Assets/Shadder/textura forcefield.jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-970.1625,124.4182;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-764.9108,-107.3173;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-754.4683,85.01437;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;1;-1271.174,-444.2722;Float;True;Property;_boss_03Default_AlbedoTransparency;boss_03 - Default_AlbedoTransparency;0;0;Assets/Art/texturas enemigos/Boss/boss_03 - Default_AlbedoTransparency.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;39;-788.9123,517.3101;Float;False;Constant;_Float3;Float 3;8;0;4;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-924.8619,228.2987;Float;False;Property;_SegundaFase;SegundaFase;5;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;34;-901.1841,320.0408;Float;True;Property;_maskBossShield;mask Boss Shield;6;0;Assets/Art/texturas enemigos/Boss/mask Boss Shield.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-822.7554,-373.1478;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;32;-697.7822,-513.6471;Float;False;Constant;_Float1;Float 1;6;0;0.18;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-549.52,336.5664;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;25;-585.134,18.53874;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;37;-572.4396,524.6777;Float;False;Property;_Shield;Shield;7;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-542.5515,-437.8033;Float;True;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-465.3711,81.9238;Float;True;2;2;0;COLOR;0.0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;21;-253.565,688.5524;Float;False;Constant;_metalic;metalic;4;0;0.58;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-673.966,787.2915;Float;True;Property;_boss_03Default_AO;boss_03 - Default_AO;1;0;Assets/Art/texturas enemigos/Boss/boss_03 - Default_AO.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;36;-258.892,36.8387;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;31;-374.5402,-223.8556;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;4;-370.3233,789.6942;Float;True;Property;_boss_03Default_Normal;boss_03 - Default_Normal;2;0;Assets/Art/texturas enemigos/Boss/boss_03 - Default_Normal.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;BossShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;16;0
WireConnection;15;1;14;0
WireConnection;23;0;14;0
WireConnection;23;1;22;1
WireConnection;18;0;19;0
WireConnection;18;1;15;0
WireConnection;24;0;18;0
WireConnection;24;1;23;0
WireConnection;20;0;1;0
WireConnection;20;1;15;0
WireConnection;38;0;34;0
WireConnection;38;1;39;0
WireConnection;25;0;18;0
WireConnection;25;1;24;0
WireConnection;25;2;26;0
WireConnection;33;0;32;0
WireConnection;33;1;20;0
WireConnection;42;0;25;0
WireConnection;42;1;38;0
WireConnection;36;0;25;0
WireConnection;36;1;42;0
WireConnection;36;2;37;0
WireConnection;31;0;20;0
WireConnection;31;1;33;0
WireConnection;31;2;26;0
WireConnection;0;0;31;0
WireConnection;0;1;4;0
WireConnection;0;2;36;0
WireConnection;0;3;21;0
WireConnection;0;5;2;0
ASEEND*/
//CHKSM=B6921675F67636BCAAB0DCB8734C05D522C8A1F7