// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/CubeEnemyShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_reintegrateValue("reintegrateValue", Range( 0 , 1)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Albedo("Albedo", 2D) = "white" {}
		_AO("AO", 2D) = "white" {}
		_Emission("Emission", 2D) = "white" {}
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

		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _Brillo;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float _Berserker;
		uniform sampler2D _AO;
		uniform float4 _AO_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _reintegrateValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = tex2D( _Albedo, uv_Albedo ).rgb;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float4 tex2DNode13 = tex2D( _Emission, uv_Emission );
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
			float2 uv_AO = i.uv_texcoord * _AO_ST.xy + _AO_ST.zw;
			o.Occlusion = tex2D( _AO, uv_AO ).r;
			o.Alpha = 1;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 lerpResult17 = lerp( tex2D( _TextureSample1, uv_TextureSample1 ) , float4(1,1,1,0) , _reintegrateValue);
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
204;92;755;926;1687.474;422.0846;1.640607;False;False
Node;AmplifyShaderEditor.FresnelNode;18;-1323.528,633.6516;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;3.3;False;3;FLOAT;1.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;22;-1568.931,567.8758;Float;False;Constant;_Color2;Color 2;11;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-1055.277,562.192;Float;True;Property;_Emission;Emission;5;0;Create;True;0;0;False;0;f8fe0c59129aca54199760bac498fcd5;f5302e8b07d9eaa4bbf106b85280bf70;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-955.5446,299.0884;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-583.8012,211.6147;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1044.777,471.5903;Float;False;Property;_Berserker;Berserker;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;28;-1341.306,135.7218;Float;False;Constant;_Color0;Color 0;8;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-1390.078,326.7347;Float;False;Property;_reintegrateValue;reintegrateValue;1;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-329.4026,60.2049;Float;False;Property;_Brillo;Brillo;7;0;Create;True;0;0;False;0;0;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;23;-397.4868,186.1785;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;16;-1310.309,-63.07546;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;f98b75fbffa36144b991e7ccfc3c3ef3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-112.4026,125.2049;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;12;-292.7032,379.715;Float;True;Property;_AO;AO;4;0;Create;True;0;0;False;0;0d544fa66e16a974c9f91c821ecd7df8;89953113bbe7dd34fb2b5f7de51d6c7a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-765.1556,-157.3182;Float;True;Property;_Albedo;Albedo;3;0;Create;True;0;0;False;0;092478f911ee00c45a66660f4a5ec239;0ab9a41e470aeab40a55bae74fc2fb8d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;17;-742.7573,65.89441;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;69.59744,159.2049;Float;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;332.734,8.206337;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MyShaders/CubeEnemyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;22;0
WireConnection;21;1;18;0
WireConnection;20;0;21;0
WireConnection;20;1;13;0
WireConnection;23;0;13;0
WireConnection;23;1;20;0
WireConnection;23;2;24;0
WireConnection;26;0;27;0
WireConnection;26;1;23;0
WireConnection;17;0;16;0
WireConnection;17;1;28;0
WireConnection;17;2;2;0
WireConnection;0;0;11;0
WireConnection;0;2;26;0
WireConnection;0;3;25;0
WireConnection;0;4;25;0
WireConnection;0;5;12;0
WireConnection;0;10;17;0
ASEEND*/
//CHKSM=90E7B04F26B4B003BA00E8FDB0D603DA0EB75D3C