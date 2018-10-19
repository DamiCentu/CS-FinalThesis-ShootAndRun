// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShaderProta"
{
	Properties
	{
		_Power("Power", Float) = 0
		_Float2("Float 2", Float) = 0
		_Float1("Float 1", Float) = 0
		_Scale("Scale", Float) = 0
		_Bias("Bias", Float) = 0
		_Float0("Float 0", Float) = 0
		_mainbody_03___Default_AlbedoTransparency("main body_03___Default_AlbedoTransparency", 2D) = "white" {}
		_mainbody_03___Default_AO("main body_03___Default_AO", 2D) = "white" {}
		_mainbody_03___Default_MetallicSmoothness("main body_03___Default_MetallicSmoothness", 2D) = "white" {}
		_mainbody_03___Default_Normal("main body_03___Default_Normal", 2D) = "bump" {}
		_BerserkF("BerserkF", Float) = 0
		_DashF("DashF", Float) = 0
		_mask("mask", 2D) = "white" {}
		_TinColor("TinColor", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _mainbody_03___Default_Normal;
		uniform float4 _mainbody_03___Default_Normal_ST;
		uniform sampler2D _mainbody_03___Default_AlbedoTransparency;
		uniform float4 _mainbody_03___Default_AlbedoTransparency_ST;
		uniform sampler2D _mask;
		uniform float4 _mask_ST;
		uniform sampler2D _TinColor;
		uniform float _Float0;
		uniform float _Float1;
		uniform float _Float2;
		uniform float _DashF;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;
		uniform float _BerserkF;
		uniform sampler2D _mainbody_03___Default_MetallicSmoothness;
		uniform float4 _mainbody_03___Default_MetallicSmoothness_ST;
		uniform sampler2D _mainbody_03___Default_AO;
		uniform float4 _mainbody_03___Default_AO_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_mainbody_03___Default_Normal = i.uv_texcoord * _mainbody_03___Default_Normal_ST.xy + _mainbody_03___Default_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _mainbody_03___Default_Normal, uv_mainbody_03___Default_Normal ) );
			float2 uv_mainbody_03___Default_AlbedoTransparency = i.uv_texcoord * _mainbody_03___Default_AlbedoTransparency_ST.xy + _mainbody_03___Default_AlbedoTransparency_ST.zw;
			float2 uv_mask = i.uv_texcoord * _mask_ST.xy + _mask_ST.zw;
			float2 temp_cast_0 = (0.55).xx;
			float2 panner31 = ( _Time.y * temp_cast_0 + i.uv_texcoord);
			float4 tex2DNode30 = tex2D( _TinColor, panner31 );
			o.Albedo = ( tex2D( _mainbody_03___Default_AlbedoTransparency, uv_mainbody_03___Default_AlbedoTransparency ) + ( tex2D( _mask, uv_mask ) * tex2DNode30 ) ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNDotV18 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode18 = ( 0.0 + 2.0 * pow( 1.0 - fresnelNDotV18, 5.0 ) );
			float fresnelNDotV12 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode12 = ( _Float0 + _Float1 * pow( 1.0 - fresnelNDotV12, _Float2 ) );
			float4 lerpResult19 = lerp( ( tex2DNode30 * fresnelNode18 ) , ( fresnelNode12 * float4(0,0.8344827,1,0) ) , _DashF);
			float fresnelNDotV2 = dot( normalize( ase_worldNormal ), ase_worldViewDir );
			float fresnelNode2 = ( _Bias + _Scale * pow( 1.0 - fresnelNDotV2, _Power ) );
			float4 lerpResult16 = lerp( lerpResult19 , ( fresnelNode2 * float4(1,0,0,0) ) , _BerserkF);
			o.Emission = lerpResult16.rgb;
			float2 uv_mainbody_03___Default_MetallicSmoothness = i.uv_texcoord * _mainbody_03___Default_MetallicSmoothness_ST.xy + _mainbody_03___Default_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _mainbody_03___Default_MetallicSmoothness, uv_mainbody_03___Default_MetallicSmoothness ).r;
			float2 uv_mainbody_03___Default_AO = i.uv_texcoord * _mainbody_03___Default_AO_ST.xy + _mainbody_03___Default_AO_ST.zw;
			o.Occlusion = tex2D( _mainbody_03___Default_AO, uv_mainbody_03___Default_AO ).r;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows exclude_path:deferred 

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
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
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
444;122;602;571;1867.715;173.4909;1.074342;True;False
Node;AmplifyShaderEditor.RangedFloatNode;34;-1788.501,-543.8336;Float;False;Constant;_Float3;Float 3;16;0;Create;True;0;0;False;0;0.55;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;32;-1661.997,-496.041;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;15;-2267.914,364.9491;Float;False;800.5813;475.2104;dash;6;8;13;12;11;10;9;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;33;-1787.096,-670.3358;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;31;-1473.15,-629.6802;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2211.714,414.9491;Float;False;Property;_Float0;Float 0;5;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2217.914,488.5491;Float;False;Property;_Float1;Float 1;2;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-2215.214,560.3489;Float;False;Property;_Float2;Float 2;1;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;14;-2468.753,-314.9019;Float;False;820.7968;501.4906;berserk;6;3;4;5;2;7;6;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-2412.553,-264.9021;Float;False;Property;_Bias;Bias;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-1187.28,-669.5245;Float;True;Property;_TinColor;TinColor;14;0;Create;True;0;0;False;0;None;b32fadcc3d3cf9c49bfb6f125c265f2f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;18;-1592.169,-17.23621;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;2;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;12;-2012.012,425.7491;Float;True;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-2416.053,-119.502;Float;False;Property;_Power;Power;0;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-2418.753,-191.302;Float;False;Property;_Scale;Scale;3;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-2096.126,657.418;Float;False;Constant;_Color1;Color 1;1;0;Create;True;0;0;False;0;0,0.8344827,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;2;-2212.85,-254.1021;Float;True;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;29;-1256.125,-950.2562;Float;True;Property;_mask;mask;13;0;Create;True;0;0;False;0;8512b5973a488674f8e3e5e9222dd664;8512b5973a488674f8e3e5e9222dd664;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-1328.043,-68.57715;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;-2228.23,-20.41124;Float;False;Constant;_Color0;Color 0;1;0;Create;True;0;0;False;0;1,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;28;-1488.673,164.6781;Float;False;Property;_DashF;DashF;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1616.113,497.4942;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-890.6973,435.4677;Float;False;Property;_BerserkF;BerserkF;11;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1816.951,-182.3568;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;21;-907.4422,-288.7884;Float;True;Property;_mainbody_03___Default_AlbedoTransparency;main body_03___Default_AlbedoTransparency;7;0;Create;True;0;0;False;0;dc7c5acda41d9ac4eaaa6dd0fbb406a8;dc7c5acda41d9ac4eaaa6dd0fbb406a8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-873.9881,-849.0448;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;19;-1106.209,57.06666;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;24;-774.2142,908.2082;Float;True;Property;_mainbody_03___Default_Normal;main body_03___Default_Normal;10;0;Create;True;0;0;False;0;912177baac7d0aa458d4a8266d39f083;912177baac7d0aa458d4a8266d39f083;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-239.3719,-251.4387;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;22;-633.946,322.639;Float;True;Property;_mainbody_03___Default_AO;main body_03___Default_AO;8;0;Create;True;0;0;False;0;9435703d78a1c2b48b7082d55be8c8f8;9435703d78a1c2b48b7082d55be8c8f8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;17;-1024.766,308.666;Float;False;Property;_Berserk;Berserk;6;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;-687.6226,607.7565;Float;True;Property;_mainbody_03___Default_MetallicSmoothness;main body_03___Default_MetallicSmoothness;9;0;Create;True;0;0;False;0;d4d5d8f1a452da4429ae8d20b24578cc;d4d5d8f1a452da4429ae8d20b24578cc;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-545.9904,-18.01167;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;ShaderProta;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0.02;0,1,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;31;0;33;0
WireConnection;31;2;34;0
WireConnection;31;1;32;0
WireConnection;30;1;31;0
WireConnection;12;1;9;0
WireConnection;12;2;10;0
WireConnection;12;3;11;0
WireConnection;2;1;3;0
WireConnection;2;2;4;0
WireConnection;2;3;5;0
WireConnection;37;0;30;0
WireConnection;37;1;18;0
WireConnection;13;0;12;0
WireConnection;13;1;8;0
WireConnection;7;0;2;0
WireConnection;7;1;6;0
WireConnection;35;0;29;0
WireConnection;35;1;30;0
WireConnection;19;0;37;0
WireConnection;19;1;13;0
WireConnection;19;2;28;0
WireConnection;36;0;21;0
WireConnection;36;1;35;0
WireConnection;16;0;19;0
WireConnection;16;1;7;0
WireConnection;16;2;27;0
WireConnection;0;0;36;0
WireConnection;0;1;24;0
WireConnection;0;2;16;0
WireConnection;0;3;23;0
WireConnection;0;5;22;0
ASEEND*/
//CHKSM=E868D073AE02FED65A35AA82777760A6EA746D9D