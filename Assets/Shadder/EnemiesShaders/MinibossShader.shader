// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "myShaders/MinibossShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_reintegrateValue("reintegrateValue", Range( 0 , 2)) = 0
		_noise2("noise 2", 2D) = "white" {}
		_enemigo2_DefaultMaterial_AlbedoTransparency("enemigo 2_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_enemigo2_DefaultMaterial_AO("enemigo 2_DefaultMaterial_AO", 2D) = "white" {}
		_Berserker("Berserker", Range( 0 , 1)) = 0
		_enemigo2_DefaultMaterial_Normal("enemigo 2_DefaultMaterial_Normal", 2D) = "white" {}
		_enemigo2_DefaultMaterial_MetallicSmoothness("enemigo 2_DefaultMaterial_MetallicSmoothness", 2D) = "white" {}
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

		uniform sampler2D _enemigo2_DefaultMaterial_Normal;
		uniform float4 _enemigo2_DefaultMaterial_Normal_ST;
		uniform sampler2D _enemigo2_DefaultMaterial_AlbedoTransparency;
		uniform float4 _enemigo2_DefaultMaterial_AlbedoTransparency_ST;
		uniform float _Berserker;
		uniform sampler2D _enemigo2_DefaultMaterial_MetallicSmoothness;
		uniform float4 _enemigo2_DefaultMaterial_MetallicSmoothness_ST;
		uniform sampler2D _enemigo2_DefaultMaterial_AO;
		uniform float4 _enemigo2_DefaultMaterial_AO_ST;
		uniform sampler2D _noise2;
		uniform float4 _noise2_ST;
		uniform float _reintegrateValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_enemigo2_DefaultMaterial_Normal = i.uv_texcoord * _enemigo2_DefaultMaterial_Normal_ST.xy + _enemigo2_DefaultMaterial_Normal_ST.zw;
			o.Normal = tex2D( _enemigo2_DefaultMaterial_Normal, uv_enemigo2_DefaultMaterial_Normal ).rgb;
			float2 uv_enemigo2_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _enemigo2_DefaultMaterial_AlbedoTransparency_ST.xy + _enemigo2_DefaultMaterial_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _enemigo2_DefaultMaterial_AlbedoTransparency, uv_enemigo2_DefaultMaterial_AlbedoTransparency ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNDotV18 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode18 = ( 0.0 + 3.3 * pow( 1.0 - fresnelNDotV18, 1.6 ) );
			float4 lerpResult21 = lerp( float4( 0.0,0,0,0 ) , ( float4(1,0,0,0) * fresnelNode18 ) , _Berserker);
			o.Emission = lerpResult21.rgb;
			float2 uv_enemigo2_DefaultMaterial_MetallicSmoothness = i.uv_texcoord * _enemigo2_DefaultMaterial_MetallicSmoothness_ST.xy + _enemigo2_DefaultMaterial_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _enemigo2_DefaultMaterial_MetallicSmoothness, uv_enemigo2_DefaultMaterial_MetallicSmoothness ).r;
			float2 uv_enemigo2_DefaultMaterial_AO = i.uv_texcoord * _enemigo2_DefaultMaterial_AO_ST.xy + _enemigo2_DefaultMaterial_AO_ST.zw;
			o.Occlusion = tex2D( _enemigo2_DefaultMaterial_AO, uv_enemigo2_DefaultMaterial_AO ).r;
			o.Alpha = 1;
			float2 uv_noise2 = i.uv_texcoord * _noise2_ST.xy + _noise2_ST.zw;
			float4 lerpResult12 = lerp( float4( 0.0,0,0,0 ) , tex2D( _noise2, uv_noise2 ) , _reintegrateValue);
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
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
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
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
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
Version=13701
490;133;818;570;2129.682;47.3811;2.300877;False;False
Node;AmplifyShaderEditor.FresnelNode;18;-2306.812,840.0562;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;3.3;False;3;FLOAT;1.6;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;17;-2552.215,774.2805;Float;False;Constant;_Color0;Color 0;11;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;1;-2548.23,412.4308;Float;False;Property;_reintegrateValue;reintegrateValue;1;0;0;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;19;-1933.755,672.4882;Float;False;Property;_Berserker;Berserker;4;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;11;-2457.743,85.19255;Float;True;Property;_noise2;noise 2;2;0;Assets/Shadder/EnemiesShaders/Textures/noise 2.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-1938.828,505.4932;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;21;-1564.734,535.9984;Float;True;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;12;-1950.867,194.4834;Float;True;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;15;-1349.422,1333.654;Float;True;Property;_enemigo2_DefaultMaterial_Normal;enemigo 2_DefaultMaterial_Normal;5;0;Assets/Art/texturas enemigos/mini boss/enemigo 2_DefaultMaterial_Normal.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;16;-1266.524,1606.909;Float;True;Property;_enemigo2_DefaultMaterial_MetallicSmoothness;enemigo 2_DefaultMaterial_MetallicSmoothness;6;0;Assets/Art/texturas enemigos/mini boss/enemigo 2_DefaultMaterial_MetallicSmoothness.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;13;-1554.923,-149.8958;Float;True;Property;_enemigo2_DefaultMaterial_AlbedoTransparency;enemigo 2_DefaultMaterial_AlbedoTransparency;3;0;Assets/Art/texturas enemigos/mini boss/enemigo 2_DefaultMaterial_AlbedoTransparency.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;14;-1396.793,1043.847;Float;True;Property;_enemigo2_DefaultMaterial_AO;enemigo 2_DefaultMaterial_AO;4;0;Assets/Art/texturas enemigos/mini boss/enemigo 2_DefaultMaterial_AO.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-839.8232,341.8377;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;myShaders/MinibossShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;0;17;0
WireConnection;20;1;18;0
WireConnection;21;1;20;0
WireConnection;21;2;19;0
WireConnection;12;1;11;0
WireConnection;12;2;1;0
WireConnection;0;0;13;0
WireConnection;0;1;15;0
WireConnection;0;2;21;0
WireConnection;0;3;16;0
WireConnection;0;5;14;0
WireConnection;0;10;12;0
ASEEND*/
//CHKSM=D6AA567AC9BB5DF6266D82081D9B35426AFD2C2F