// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/NormalEnemyShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_reintegrateValue("reintegrateValue", Range( 0 , 1)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_enemigo1_DefaultMaterial_AlbedoTransparency("enemigo 1_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_enemigo1_DefaultMaterial_AO("enemigo 1_DefaultMaterial_AO", 2D) = "white" {}
		_enemigo1_DefaultMaterial_Emission("enemigo 1_DefaultMaterial_Emission", 2D) = "white" {}
		_Berserker("Berserker", Range( 0 , 1)) = 0
		_Brillo("Brillo", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _enemigo1_DefaultMaterial_AlbedoTransparency;
		uniform float4 _enemigo1_DefaultMaterial_AlbedoTransparency_ST;
		uniform float _Brillo;
		uniform sampler2D _enemigo1_DefaultMaterial_Emission;
		uniform float4 _enemigo1_DefaultMaterial_Emission_ST;
		uniform float _Berserker;
		uniform sampler2D _enemigo1_DefaultMaterial_AO;
		uniform float4 _enemigo1_DefaultMaterial_AO_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _reintegrateValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_enemigo1_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _enemigo1_DefaultMaterial_AlbedoTransparency_ST.xy + _enemigo1_DefaultMaterial_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _enemigo1_DefaultMaterial_AlbedoTransparency, uv_enemigo1_DefaultMaterial_AlbedoTransparency ).rgb;
			float2 uv_enemigo1_DefaultMaterial_Emission = i.uv_texcoord * _enemigo1_DefaultMaterial_Emission_ST.xy + _enemigo1_DefaultMaterial_Emission_ST.zw;
			float4 tex2DNode13 = tex2D( _enemigo1_DefaultMaterial_Emission, uv_enemigo1_DefaultMaterial_Emission );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV18 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode18 = ( 0.0 + 3.3 * pow( 1.0 - fresnelNDotV18, 1.6 ) );
			float4 lerpResult23 = lerp( tex2DNode13 , ( ( float4(1,0,0,0) * fresnelNode18 ) + tex2DNode13 ) , _Berserker);
			o.Emission = ( _Brillo * lerpResult23 ).rgb;
			float temp_output_25_0 = 1.0;
			o.Metallic = temp_output_25_0;
			o.Smoothness = temp_output_25_0;
			float2 uv_enemigo1_DefaultMaterial_AO = i.uv_texcoord * _enemigo1_DefaultMaterial_AO_ST.xy + _enemigo1_DefaultMaterial_AO_ST.zw;
			o.Occlusion = tex2D( _enemigo1_DefaultMaterial_AO, uv_enemigo1_DefaultMaterial_AO ).r;
			o.Alpha = 1;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 lerpResult17 = lerp( ( tex2D( _TextureSample1, uv_TextureSample1 ) * _reintegrateValue ) , float4(1,1,1,0) , _reintegrateValue);
			clip( lerpResult17.r - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
204;92;755;926;2748.209;733.5851;2.570063;False;False
Node;AmplifyShaderEditor.ColorNode;22;-1568.931,567.8758;Float;False;Constant;_Color2;Color 2;11;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;18;-1323.528,633.6516;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;3.3;False;3;FLOAT;1.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-955.5446,299.0884;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;-891.151,494.4899;Float;True;Property;_enemigo1_DefaultMaterial_Emission;enemigo 1_DefaultMaterial_Emission;5;0;Create;True;0;0;False;0;f8fe0c59129aca54199760bac498fcd5;f8fe0c59129aca54199760bac498fcd5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-1561.779,284.7591;Float;False;Property;_reintegrateValue;reintegrateValue;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;16;-1635.83,-151.9581;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;f98b75fbffa36144b991e7ccfc3c3ef3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-880.6509,403.8881;Float;False;Property;_Berserker;Berserker;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-588.092,200.5171;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1254.852,-187.0526;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;30;-1537.897,51.82512;Float;False;Constant;_Color0;Color 0;8;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;26;-440.526,-73.57184;Float;False;Property;_Brillo;Brillo;7;0;Create;True;0;0;False;0;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;-400.8008,122.1123;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-342.0237,319.1918;Float;False;Constant;_Float0;Float 0;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-352.8319,512.2411;Float;True;Property;_enemigo1_DefaultMaterial_AO;enemigo 1_DefaultMaterial_AO;4;0;Create;True;0;0;False;0;0d544fa66e16a974c9f91c821ecd7df8;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-184.526,28.42816;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;17;-967.2949,-18.79652;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;11;-765.1556,-343.3182;Float;True;Property;_enemigo1_DefaultMaterial_AlbedoTransparency;enemigo 1_DefaultMaterial_AlbedoTransparency;3;0;Create;True;0;0;False;0;092478f911ee00c45a66660f4a5ec239;092478f911ee00c45a66660f4a5ec239;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MyShaders/NormalEnemyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;22;0
WireConnection;21;1;18;0
WireConnection;20;0;21;0
WireConnection;20;1;13;0
WireConnection;31;0;16;0
WireConnection;31;1;2;0
WireConnection;23;0;13;0
WireConnection;23;1;20;0
WireConnection;23;2;24;0
WireConnection;27;0;26;0
WireConnection;27;1;23;0
WireConnection;17;0;31;0
WireConnection;17;1;30;0
WireConnection;17;2;2;0
WireConnection;0;0;11;0
WireConnection;0;2;27;0
WireConnection;0;3;25;0
WireConnection;0;4;25;0
WireConnection;0;5;12;0
WireConnection;0;10;17;0
ASEEND*/
//CHKSM=DACDFF6957A029EED176F12EBDA9E874CEFA55FB