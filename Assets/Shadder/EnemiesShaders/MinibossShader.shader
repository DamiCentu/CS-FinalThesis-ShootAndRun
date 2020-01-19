// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "myShaders/MinibossShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_reintegrateValue("reintegrateValue", Range( 0 , 1)) = 0
		_noise2("noise 2", 2D) = "white" {}
		_Berserker("Berserker", Range( 0 , 1)) = 0
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
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

		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform float _Brillo;
		uniform float _Berserker;
		uniform sampler2D _TextureSample2;
		uniform float4 _TextureSample2_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform sampler2D _noise2;
		uniform float4 _noise2_ST;
		uniform float _reintegrateValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Albedo = tex2D( _TextureSample0, uv_TextureSample0 ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNDotV18 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode18 = ( 0.0 + 3.3 * pow( 1.0 - fresnelNDotV18, 1.6 ) );
			float4 lerpResult21 = lerp( float4( 0,0,0,0 ) , ( float4(1,0,0,0) * fresnelNode18 ) , _Berserker);
			float2 uv_TextureSample2 = i.uv_texcoord * _TextureSample2_ST.xy + _TextureSample2_ST.zw;
			o.Emission = ( _Brillo * ( lerpResult21 + tex2D( _TextureSample2, uv_TextureSample2 ) ) ).rgb;
			float temp_output_29_0 = 1.0;
			o.Metallic = temp_output_29_0;
			o.Smoothness = temp_output_29_0;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			o.Occlusion = tex2D( _TextureSample1, uv_TextureSample1 ).r;
			o.Alpha = 1;
			float2 uv_noise2 = i.uv_texcoord * _noise2_ST.xy + _noise2_ST.zw;
			float4 lerpResult12 = lerp( tex2D( _noise2, uv_noise2 ) , float4(1,1,1,0) , _reintegrateValue);
			clip( lerpResult12.r - _Cutoff );
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
204;92;755;926;2834.833;214.2711;1.658988;False;False
Node;AmplifyShaderEditor.ColorNode;17;-2598.918,545.1768;Float;False;Constant;_Color0;Color 0;11;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;18;-2353.515,610.9525;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;3.3;False;3;FLOAT;1.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-2014.779,619.4033;Float;False;Property;_Berserker;Berserker;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1938.828,505.4932;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1;-2453.669,453.9058;Float;False;Property;_reintegrateValue;reintegrateValue;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;-2474.828,278.448;Float;False;Constant;_Color1;Color 1;8;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-1686.508,636.3811;Float;True;Property;_TextureSample2;Texture Sample 2;4;0;Create;True;0;0;False;0;None;1eb0354696b94f44d877d6bb08ab4256;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-2457.743,85.19255;Float;True;Property;_noise2;noise 2;2;0;Create;True;0;0;False;0;f98b75fbffa36144b991e7ccfc3c3ef3;f98b75fbffa36144b991e7ccfc3c3ef3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;21;-1648.552,457.7679;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;24;-1328.397,417.3657;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;12;-1950.867,194.4834;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1256.294,299.0392;Float;False;Property;_Brillo;Brillo;7;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1117.763,491.8137;Float;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;25;-1532.093,11.29141;Float;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;False;0;None;c486a00f8e936814489f2be8bad12921;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;28;-1390.931,317.0873;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;26;-1631.646,855.9097;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;None;b02a047fbf09b6c4a83dca0a79ae6111;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1070.703,363.0126;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-839.8232,341.8377;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;myShaders/MinibossShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;17;0
WireConnection;20;1;18;0
WireConnection;21;1;20;0
WireConnection;21;2;19;0
WireConnection;24;0;21;0
WireConnection;24;1;27;0
WireConnection;12;0;11;0
WireConnection;12;1;32;0
WireConnection;12;2;1;0
WireConnection;28;0;12;0
WireConnection;30;0;31;0
WireConnection;30;1;24;0
WireConnection;0;0;25;0
WireConnection;0;2;30;0
WireConnection;0;3;29;0
WireConnection;0;4;29;0
WireConnection;0;5;26;0
WireConnection;0;10;28;0
ASEEND*/
//CHKSM=4A82ECCB5C000F65AF4913673DBA9BE8A8B42B41