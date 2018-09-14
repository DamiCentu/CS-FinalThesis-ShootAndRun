// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/NormalEnemyShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_reintegrateValue("reintegrateValue", Range( 0 , 2)) = 0
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_enemigo1_DefaultMaterial_AlbedoTransparency("enemigo 1_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_enemigo1_DefaultMaterial_AO("enemigo 1_DefaultMaterial_AO", 2D) = "white" {}
		_enemigo1_DefaultMaterial_Emission("enemigo 1_DefaultMaterial_Emission", 2D) = "white" {}
		_enemigo1_DefaultMaterial_MetallicSmoothness("enemigo 1_DefaultMaterial_MetallicSmoothness", 2D) = "white" {}
		_enemigo1_DefaultMaterial_Normal("enemigo 1_DefaultMaterial_Normal", 2D) = "bump" {}
		_Berserker("Berserker", Range( 0 , 1)) = 0
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

		uniform sampler2D _enemigo1_DefaultMaterial_Normal;
		uniform float4 _enemigo1_DefaultMaterial_Normal_ST;
		uniform sampler2D _enemigo1_DefaultMaterial_AlbedoTransparency;
		uniform float4 _enemigo1_DefaultMaterial_AlbedoTransparency_ST;
		uniform sampler2D _enemigo1_DefaultMaterial_Emission;
		uniform float4 _enemigo1_DefaultMaterial_Emission_ST;
		uniform float _Berserker;
		uniform sampler2D _enemigo1_DefaultMaterial_MetallicSmoothness;
		uniform float4 _enemigo1_DefaultMaterial_MetallicSmoothness_ST;
		uniform sampler2D _enemigo1_DefaultMaterial_AO;
		uniform float4 _enemigo1_DefaultMaterial_AO_ST;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;
		uniform float _reintegrateValue;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_enemigo1_DefaultMaterial_Normal = i.uv_texcoord * _enemigo1_DefaultMaterial_Normal_ST.xy + _enemigo1_DefaultMaterial_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _enemigo1_DefaultMaterial_Normal, uv_enemigo1_DefaultMaterial_Normal ) );
			float2 uv_enemigo1_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _enemigo1_DefaultMaterial_AlbedoTransparency_ST.xy + _enemigo1_DefaultMaterial_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _enemigo1_DefaultMaterial_AlbedoTransparency, uv_enemigo1_DefaultMaterial_AlbedoTransparency ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNDotV18 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode18 = ( 0.0 + 3.3 * pow( 1.0 - fresnelNDotV18, 1.6 ) );
			float2 uv_enemigo1_DefaultMaterial_Emission = i.uv_texcoord * _enemigo1_DefaultMaterial_Emission_ST.xy + _enemigo1_DefaultMaterial_Emission_ST.zw;
			float4 tex2DNode13 = tex2D( _enemigo1_DefaultMaterial_Emission, uv_enemigo1_DefaultMaterial_Emission );
			float4 lerpResult23 = lerp( ( ( float4(1,0,0,0) * fresnelNode18 ) + tex2DNode13 ) , tex2DNode13 , _Berserker);
			o.Emission = lerpResult23.rgb;
			float2 uv_enemigo1_DefaultMaterial_MetallicSmoothness = i.uv_texcoord * _enemigo1_DefaultMaterial_MetallicSmoothness_ST.xy + _enemigo1_DefaultMaterial_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _enemigo1_DefaultMaterial_MetallicSmoothness, uv_enemigo1_DefaultMaterial_MetallicSmoothness ).r;
			float2 uv_enemigo1_DefaultMaterial_AO = i.uv_texcoord * _enemigo1_DefaultMaterial_AO_ST.xy + _enemigo1_DefaultMaterial_AO_ST.zw;
			o.Occlusion = tex2D( _enemigo1_DefaultMaterial_AO, uv_enemigo1_DefaultMaterial_AO ).r;
			o.Alpha = 1;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float4 lerpResult17 = lerp( float4( 0,0,0,0 ) , tex2D( _TextureSample1, uv_TextureSample1 ) , _reintegrateValue);
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
142;150;818;571;4480.544;1959.495;7.994497;True;False
Node;AmplifyShaderEditor.ColorNode;22;-1568.931,567.8758;Float;False;Constant;_Color2;Color 2;11;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FresnelNode;18;-1323.528,633.6516;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;3.3;False;3;FLOAT;1.6;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-955.5446,299.0884;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;13;-891.151,494.4899;Float;True;Property;_enemigo1_DefaultMaterial_Emission;enemigo 1_DefaultMaterial_Emission;5;0;Assets/Art/texturas enemigos/araña/enemigo 1_DefaultMaterial_Emission.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;2;-1344.141,215.1734;Float;False;Property;_reintegrateValue;reintegrateValue;1;0;0;0;2;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;16;-1310.309,-63.07546;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;24;-880.6509,403.8881;Float;False;Property;_Berserker;Berserker;8;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-553.0276,213.6663;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;11;-765.1556,-158.3182;Float;True;Property;_enemigo1_DefaultMaterial_AlbedoTransparency;enemigo 1_DefaultMaterial_AlbedoTransparency;3;0;Assets/Art/texturas enemigos/araña/enemigo 1_DefaultMaterial_AlbedoTransparency.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;17;-742.7573,65.89441;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;15;-712.047,1041.481;Float;True;Property;_enemigo1_DefaultMaterial_Normal;enemigo 1_DefaultMaterial_Normal;7;0;Assets/Art/texturas enemigos/araña/enemigo 1_DefaultMaterial_Normal.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;12;-359.6538,481.5426;Float;True;Property;_enemigo1_DefaultMaterial_AO;enemigo 1_DefaultMaterial_AO;4;0;Assets/Art/texturas enemigos/araña/enemigo 1_DefaultMaterial_AO.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;14;-862.1071,755.8836;Float;True;Property;_enemigo1_DefaultMaterial_MetallicSmoothness;enemigo 1_DefaultMaterial_MetallicSmoothness;6;0;Assets/Art/texturas enemigos/araña/enemigo 1_DefaultMaterial_MetallicSmoothness.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;23;-324.1194,183.5598;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MyShaders/NormalEnemyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;21;0;22;0
WireConnection;21;1;18;0
WireConnection;20;0;21;0
WireConnection;20;1;13;0
WireConnection;17;1;16;0
WireConnection;17;2;2;0
WireConnection;23;0;20;0
WireConnection;23;1;13;0
WireConnection;23;2;24;0
WireConnection;0;0;11;0
WireConnection;0;1;15;0
WireConnection;0;2;23;0
WireConnection;0;3;14;0
WireConnection;0;5;12;0
WireConnection;0;10;17;0
ASEEND*/
//CHKSM=73A54194B64C6E3ECD69B18297F0319625E3D271