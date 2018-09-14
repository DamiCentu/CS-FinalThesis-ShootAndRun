// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/TurretShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_reintegrateValue("reintegrateValue", Range( 0 , 2)) = 0
		_Berserker("Berserker", Range( 0 , 1)) = 0
		_noise2("noise 2", 2D) = "white" {}
		_torreta_DefaultMaterial_AlbedoTransparency("torreta_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_torreta_DefaultMaterial_Emission("torreta_DefaultMaterial_Emission", 2D) = "white" {}
		_torreta_DefaultMaterial_MetallicSmoothness("torreta_DefaultMaterial_MetallicSmoothness", 2D) = "white" {}
		_torreta_DefaultMaterial_Normal("torreta_DefaultMaterial_Normal", 2D) = "white" {}
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

		uniform sampler2D _torreta_DefaultMaterial_Normal;
		uniform float4 _torreta_DefaultMaterial_Normal_ST;
		uniform sampler2D _torreta_DefaultMaterial_AlbedoTransparency;
		uniform float4 _torreta_DefaultMaterial_AlbedoTransparency_ST;
		uniform sampler2D _torreta_DefaultMaterial_Emission;
		uniform float4 _torreta_DefaultMaterial_Emission_ST;
		uniform float _Berserker;
		uniform sampler2D _torreta_DefaultMaterial_MetallicSmoothness;
		uniform float4 _torreta_DefaultMaterial_MetallicSmoothness_ST;
		uniform sampler2D _noise2;
		uniform float4 _noise2_ST;
		uniform float _reintegrateValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_torreta_DefaultMaterial_Normal = i.uv_texcoord * _torreta_DefaultMaterial_Normal_ST.xy + _torreta_DefaultMaterial_Normal_ST.zw;
			o.Normal = tex2D( _torreta_DefaultMaterial_Normal, uv_torreta_DefaultMaterial_Normal ).rgb;
			float2 uv_torreta_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _torreta_DefaultMaterial_AlbedoTransparency_ST.xy + _torreta_DefaultMaterial_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _torreta_DefaultMaterial_AlbedoTransparency, uv_torreta_DefaultMaterial_AlbedoTransparency ).rgb;
			float2 uv_torreta_DefaultMaterial_Emission = i.uv_texcoord * _torreta_DefaultMaterial_Emission_ST.xy + _torreta_DefaultMaterial_Emission_ST.zw;
			float4 tex2DNode14 = tex2D( _torreta_DefaultMaterial_Emission, uv_torreta_DefaultMaterial_Emission );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNDotV19 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode19 = ( 0.0 + 3.3 * pow( 1.0 - fresnelNDotV19, 1.6 ) );
			float4 lerpResult22 = lerp( tex2DNode14 , ( ( float4(1,0,0,0) * fresnelNode19 ) + tex2DNode14 ) , _Berserker);
			o.Emission = lerpResult22.rgb;
			float2 uv_torreta_DefaultMaterial_MetallicSmoothness = i.uv_texcoord * _torreta_DefaultMaterial_MetallicSmoothness_ST.xy + _torreta_DefaultMaterial_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _torreta_DefaultMaterial_MetallicSmoothness, uv_torreta_DefaultMaterial_MetallicSmoothness ).r;
			o.Alpha = 1;
			float2 uv_noise2 = i.uv_texcoord * _noise2_ST.xy + _noise2_ST.zw;
			float4 lerpResult17 = lerp( float4( 0.0,0,0,0 ) , tex2D( _noise2, uv_noise2 ) , _reintegrateValue);
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
189;154;818;570;1258.13;307.1508;1.92925;False;False
Node;AmplifyShaderEditor.FresnelNode;19;-1424.801,-93.82311;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;3.3;False;3;FLOAT;1.6;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;18;-1670.203,-159.5987;Float;False;Constant;_Color0;Color 0;11;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-1114.695,-162.1496;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;14;-1324.535,218.4209;Float;True;Property;_torreta_DefaultMaterial_Emission;torreta_DefaultMaterial_Emission;7;0;Assets/Art/texturas enemigos/torreta/torreta_DefaultMaterial_Emission.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;23;-897.3597,57.47747;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0.0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;1;-900.7154,-377.8216;Float;False;Property;_reintegrateValue;reintegrateValue;1;0;0;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;12;-906.419,-585.0645;Float;True;Property;_noise2;noise 2;5;0;Assets/Shadder/EnemiesShaders/Textures/noise 2.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;20;-822.8386,357.8983;Float;False;Property;_Berserker;Berserker;4;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-553.4448,441.913;Float;True;Property;_torreta_DefaultMaterial_MetallicSmoothness;torreta_DefaultMaterial_MetallicSmoothness;8;0;Assets/Art/texturas enemigos/torreta/torreta_DefaultMaterial_MetallicSmoothness.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;22;-489.8001,-54.47434;Float;True;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;13;-665.4112,-798.9055;Float;True;Property;_torreta_DefaultMaterial_AlbedoTransparency;torreta_DefaultMaterial_AlbedoTransparency;6;0;Assets/Art/texturas enemigos/torreta/torreta_DefaultMaterial_AlbedoTransparency.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;16;-505.8249,610.8674;Float;True;Property;_torreta_DefaultMaterial_Normal;torreta_DefaultMaterial_Normal;9;0;Assets/Art/texturas enemigos/torreta/torreta_DefaultMaterial_Normal.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;17;-398.7181,-468.5755;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MyShaders/TurretShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;18;0
WireConnection;21;1;19;0
WireConnection;23;0;21;0
WireConnection;23;1;14;0
WireConnection;22;0;14;0
WireConnection;22;1;23;0
WireConnection;22;2;20;0
WireConnection;17;1;12;0
WireConnection;17;2;1;0
WireConnection;0;0;13;0
WireConnection;0;1;16;0
WireConnection;0;2;22;0
WireConnection;0;3;15;0
WireConnection;0;10;17;0
ASEEND*/
//CHKSM=8CD20A0569C881C6D92342BAF5C12C1D65BDAAF4