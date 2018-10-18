// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/ChargerShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_reintegrateValue("reintegrateValue", Range( 0 , 2)) = 0
		_rueda_DefaultMaterial_AlbedoTransparency("rueda_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_rueda_DefaultMaterial_AO("rueda_DefaultMaterial_AO", 2D) = "white" {}
		_Berserker("Berserker", Range( 0 , 1)) = 0
		_rueda_DefaultMaterial_MetallicSmoothness("rueda_DefaultMaterial_MetallicSmoothness", 2D) = "white" {}
		_noise2("noise 2", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
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

		uniform sampler2D _rueda_DefaultMaterial_AlbedoTransparency;
		uniform float4 _rueda_DefaultMaterial_AlbedoTransparency_ST;
		uniform float _Brillo;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Berserker;
		uniform sampler2D _rueda_DefaultMaterial_MetallicSmoothness;
		uniform float4 _rueda_DefaultMaterial_MetallicSmoothness_ST;
		uniform sampler2D _rueda_DefaultMaterial_AO;
		uniform float4 _rueda_DefaultMaterial_AO_ST;
		uniform sampler2D _noise2;
		uniform float4 _noise2_ST;
		uniform float _reintegrateValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_rueda_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _rueda_DefaultMaterial_AlbedoTransparency_ST.xy + _rueda_DefaultMaterial_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _rueda_DefaultMaterial_AlbedoTransparency, uv_rueda_DefaultMaterial_AlbedoTransparency ).rgb;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV22 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode22 = ( 0.0 + 3.3 * pow( 1.0 - fresnelNDotV22, 1.6 ) );
			float4 lerpResult24 = lerp( tex2D( _TextureSample0, uv_TextureSample0 ) , ( float4(1,0,0,0) * fresnelNode22 ) , _Berserker);
			o.Emission = ( _Brillo * lerpResult24 ).rgb;
			float2 uv_rueda_DefaultMaterial_MetallicSmoothness = i.uv_texcoord * _rueda_DefaultMaterial_MetallicSmoothness_ST.xy + _rueda_DefaultMaterial_MetallicSmoothness_ST.zw;
			float4 tex2DNode15 = tex2D( _rueda_DefaultMaterial_MetallicSmoothness, uv_rueda_DefaultMaterial_MetallicSmoothness );
			o.Metallic = tex2DNode15.r;
			o.Smoothness = tex2DNode15.r;
			float2 uv_rueda_DefaultMaterial_AO = i.uv_texcoord * _rueda_DefaultMaterial_AO_ST.xy + _rueda_DefaultMaterial_AO_ST.zw;
			o.Occlusion = tex2D( _rueda_DefaultMaterial_AO, uv_rueda_DefaultMaterial_AO ).r;
			o.Alpha = 1;
			float2 uv_noise2 = i.uv_texcoord * _noise2_ST.xy + _noise2_ST.zw;
			float4 lerpResult19 = lerp( float4( 0,0,0,0 ) , tex2D( _noise2, uv_noise2 ) , _reintegrateValue);
			clip( lerpResult19.r - _Cutoff );
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
501;122;602;571;445.1156;186.3013;1;False;False
Node;AmplifyShaderEditor.FresnelNode;22;-1256.484,414.292;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;3.3;False;3;FLOAT;1.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-1501.887,348.5163;Float;False;Constant;_Color0;Color 0;11;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;26;-933.4604,-48.71606;Float;True;Property;_TextureSample0;Texture Sample 0;8;0;Create;True;0;0;False;0;None;6f9933c097b7a0c4db77a503ff6a7e86;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1020.294,104.8325;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-908.5319,256.1377;Float;False;Property;_Berserker;Berserker;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-396.1156,0.6986694;Float;False;Property;_Brillo;Brillo;9;0;Create;True;0;0;False;0;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;24;-514.4066,110.2342;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1302.483,-97.90274;Float;False;Property;_reintegrateValue;reintegrateValue;1;0;Create;True;0;0;False;0;0;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;18;-1375.861,-345.1921;Float;True;Property;_noise2;noise 2;7;0;Create;True;0;0;False;0;f98b75fbffa36144b991e7ccfc3c3ef3;f98b75fbffa36144b991e7ccfc3c3ef3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-571.6742,875.0775;Float;True;Property;_rueda_DefaultMaterial_Normal;rueda_DefaultMaterial_Normal;6;0;Create;True;0;0;False;0;None;8d4b92cd6d65d394d9727c129a835606;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-205.1156,50.69867;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;13;-931.1851,-613.8802;Float;True;Property;_rueda_DefaultMaterial_AlbedoTransparency;rueda_DefaultMaterial_AlbedoTransparency;2;0;Create;True;0;0;False;0;None;a5f3f94c023e4314d999f60449fd8525;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;14;-591.3223,664.551;Float;True;Property;_rueda_DefaultMaterial_AO;rueda_DefaultMaterial_AO;3;0;Create;True;0;0;False;0;None;2281426817d781347a56f08d44d188f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-725.7922,426.2687;Float;True;Property;_rueda_DefaultMaterial_MetallicSmoothness;rueda_DefaultMaterial_MetallicSmoothness;5;0;Create;True;0;0;False;0;None;7f2bc70fe196784458a17ced7988f7ec;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;19;-736.6801,-289.0989;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MyShaders/ChargerShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;21;0
WireConnection;23;1;22;0
WireConnection;24;0;26;0
WireConnection;24;1;23;0
WireConnection;24;2;25;0
WireConnection;27;0;28;0
WireConnection;27;1;24;0
WireConnection;19;1;18;0
WireConnection;19;2;2;0
WireConnection;0;0;13;0
WireConnection;0;2;27;0
WireConnection;0;3;15;0
WireConnection;0;4;15;0
WireConnection;0;5;14;0
WireConnection;0;10;19;0
ASEEND*/
//CHKSM=CE086F3D98A91183DCECBE550EAFA35FFCD9CFC1