// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterShader"
{
	Properties
	{
		[HideInInspector] _DummyTex( "", 2D ) = "white" {}
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 8.3
		_TextureSample1("Texture Sample 1", 2D) = "bump" {}
		_TextureSample0("Texture Sample 0", 2D) = "bump" {}
		_Smoothness("Smoothness", Float) = 2.8
		_Color0("Color 0", Color) = (0.04860511,0.1314707,0.2279412,0)
		_Color1("Color 1", Color) = (0.04860511,0.1314707,0.2279412,0)
		_depth("depth", Float) = 0
		_NoiseMaskCutout("NoiseMaskCutout", 2D) = "white" {}
		_download2("download (2)", 2D) = "white" {}
		_Waves("Waves", Float) = 0
		_powerfresnel("power fresnel", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
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
			float2 uv_DummyTex;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 screenPos;
			float2 uv_texcoord;
		};

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		uniform sampler2D _TextureSample0;
		uniform sampler2D _DummyTex;
		uniform sampler2D _TextureSample1;
		uniform float4 _Color1;
		uniform float4 _Color0;
		uniform float _powerfresnel;
		uniform sampler2D _CameraDepthTexture;
		uniform float _depth;
		uniform sampler2D _download2;
		uniform float4 _download2_ST;
		uniform float _Smoothness;
		uniform float _Waves;
		uniform sampler2D _NoiseMaskCutout;
		uniform float _EdgeLength;

		float4 tessFunction( appdata v0, appdata v1, appdata v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata v )
		{
			float3 ase_vertex3Pos = v.vertex.xyz;
			float4 transform74 = mul(unity_WorldToObject,float4( ase_vertex3Pos , 0.0 ));
			v.texcoord.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float2 panner85 = ( v.texcoord.xy + 1.0 * _Time.y * float2( 0.01,0 ));
			float3 appendResult69 = (float3(transform74.x , abs( ( cos( ( ( 1.0 - ( _Waves * tex2Dlod( _NoiseMaskCutout, float4( panner85, 0, 0.0) ) ) ) + _Time.y ) ) / 4.3 ) ).r , transform74.z));
			v.vertex.xyz += appendResult69;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 texCoordDummy19 = i.uv_DummyTex*float2( 8,8 ) + float2( 0,0 );
			float2 panner20 = ( texCoordDummy19 + 1.0 * _Time.y * float2( 0.17,0 ));
			float2 texCoordDummy22 = i.uv_DummyTex*float2( 6,6 ) + float2( 0,0 );
			float2 panner24 = ( texCoordDummy22 + 1.0 * _Time.y * float2( -0.09,0 ));
			o.Normal = BlendNormals( UnpackNormal( tex2D( _TextureSample0, panner20 ) ) , UnpackNormal( tex2D( _TextureSample1, panner24 ) ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNDotV29 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode29 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNDotV29, _powerfresnel ) );
			float4 lerpResult31 = lerp( _Color1 , _Color0 , fresnelNode29);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth33 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth33 = abs( ( screenDepth33 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _depth ) );
			float2 uv_download2 = i.uv_texcoord * _download2_ST.xy + _download2_ST.zw;
			float4 clampResult41 = clamp( ( ( 1.0 - distanceDepth33 ) * ( float4(0.9338235,0.9288987,0.7552984,0) * tex2D( _download2, uv_download2 ) ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			o.Albedo = ( lerpResult31 + clampResult41 ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred vertex:vertexDataFunc tessellate:tessFunction 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
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
				float4 screenPos : TEXCOORD7;
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
				Input customInputData;
				vertexDataFunc( v );
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
				o.screenPos = ComputeScreenPos( o.pos );
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
				surfIN.screenPos = IN.screenPos;
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
688;142;668;571;1453.557;-676.2112;1;False;False
Node;AmplifyShaderEditor.CommentaryNode;86;-2850.816,824.4156;Float;False;2215.62;566.5564;waves;16;83;85;73;84;53;54;56;55;57;58;80;82;78;74;69;87;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;83;-2800.816,985.0638;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector2Node;84;-2699.628,1116.987;Float;False;Constant;_Vector1;Vector 1;8;0;0.01,0;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;85;-2516.056,975.9823;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;73;-2334.09,961.4705;Float;True;Property;_NoiseMaskCutout;NoiseMaskCutout;12;0;Assets/KriptoFX/Realistic Effects Pack v4/Textures/NoiseMaskCutout.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;53;-2224.853,874.4156;Float;False;Property;_Waves;Waves;14;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-1998.757,894.6357;Float;True;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.CommentaryNode;47;-2177.113,-749.8107;Float;False;1006.293;547.7134;espuma;8;34;33;38;44;43;45;41;36;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;56;-1990.204,1128.152;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;55;-1790.61,875.0237;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;34;-2127.113,-696.9838;Float;False;Property;_depth;depth;9;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;48;-2469.428,-106.028;Float;False;1275.654;727.7643;normals;9;19;23;21;22;24;20;25;26;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;-1683.516,1040.983;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.DepthFade;33;-1939.398,-699.8107;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;46;-1852.422,-1442.947;Float;False;1047.945;590.8534;color agua;5;30;28;15;29;31;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;23;-2315.202,460.7363;Float;False;Constant;_Vector2;Vector 2;8;0;-0.09,0;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector2Node;21;-2411.653,174.759;Float;False;Constant;_Vector0;Vector 0;8;0;0.17,0;0;3;FLOAT2;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;44;-1990.038,-608.4288;Float;False;Constant;_Color2;Color 2;9;0;0.9338235,0.9288987,0.7552984,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;43;-2018.926,-432.0973;Float;True;Property;_download2;download (2);13;0;Assets/Shadder/download (2).jpg;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-2419.428,50.19283;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;8,8;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2334.563,323.071;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;6,6;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CosOpNode;58;-1538.72,1045.822;Float;True;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.PannerNode;24;-2049.803,313.9894;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.OneMinusNode;38;-1707.001,-699.3359;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;80;-1491.969,1275.972;Float;False;Constant;_Float3;Float 3;14;0;4.3;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;30;-1802.422,-994.9939;Float;False;Property;_powerfresnel;power fresnel;15;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1721.939,-525.2114;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.PannerNode;20;-2126.731,113.4928;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;25;-1809.277,191.5833;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;None;True;0;False;bump;Auto;True;Instance;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FresnelNode;29;-1578.425,-1051.272;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.PosVertexDataNode;87;-1400.557,883.2112;Float;False;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;28;-1417.9,-1392.947;Float;False;Property;_Color1;Color 1;8;0;0.04860511,0.1314707,0.2279412,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;82;-1338.866,1068.972;Float;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;15;-1393.431,-1205.993;Float;False;Property;_Color0;Color 0;7;0;0.04860511,0.1314707,0.2279412,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-1513.744,-615.4517;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;1;-1814.181,-56.02795;Float;True;Property;_TextureSample0;Texture Sample 0;5;0;None;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;31;-1069.478,-1105.093;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.BlendNormalsNode;26;-1425.776,143.4831;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.ClampOpNode;41;-1339.82,-626.3852;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR
Node;AmplifyShaderEditor.WorldToObjectTransfNode;74;-1164.29,886.9317;Float;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.AbsOpNode;78;-1207.985,1066.031;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;2;-209.5117,95.30159;Float;False;Property;_Smoothness;Smoothness;6;0;2.8;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-525.5575,-606.5576;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.DynamicAppendNode;69;-870.196,907.2415;Float;True;FLOAT3;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.WireNode;27;-516.7847,143.1255;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;True;2;8.3;10;25;False;0.5;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;85;0;83;0
WireConnection;85;2;84;0
WireConnection;73;1;85;0
WireConnection;54;0;53;0
WireConnection;54;1;73;0
WireConnection;55;0;54;0
WireConnection;57;0;55;0
WireConnection;57;1;56;0
WireConnection;33;0;34;0
WireConnection;58;0;57;0
WireConnection;24;0;22;0
WireConnection;24;2;23;0
WireConnection;38;0;33;0
WireConnection;45;0;44;0
WireConnection;45;1;43;0
WireConnection;20;0;19;0
WireConnection;20;2;21;0
WireConnection;25;1;24;0
WireConnection;29;3;30;0
WireConnection;82;0;58;0
WireConnection;82;1;80;0
WireConnection;36;0;38;0
WireConnection;36;1;45;0
WireConnection;1;1;20;0
WireConnection;31;0;28;0
WireConnection;31;1;15;0
WireConnection;31;2;29;0
WireConnection;26;0;1;0
WireConnection;26;1;25;0
WireConnection;41;0;36;0
WireConnection;74;0;87;0
WireConnection;78;0;82;0
WireConnection;42;0;31;0
WireConnection;42;1;41;0
WireConnection;69;0;74;1
WireConnection;69;1;78;0
WireConnection;69;2;74;3
WireConnection;27;0;26;0
WireConnection;0;0;42;0
WireConnection;0;1;27;0
WireConnection;0;4;2;0
WireConnection;0;11;69;0
ASEEND*/
//CHKSM=4F6C868DB97AB858C10221828608FAC95DC6B7AD