// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/Water"
{
	Properties
	{
		[HideInInspector] _DummyTex( "", 2D ) = "white" {}
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 29.6
		_TessPhongStrength( "Phong Tess Strength", Range( 0, 1 ) ) = 1
		_NormalMap("NormalMap", 2D) = "bump" {}
		_Attenuation("Attenuation", Float) = 0
		_SubSurfaceColor("SubSurfaceColor", Color) = (0,0,0,0)
		_tile("tile", 2D) = "white" {}
		_WaterColor("Water Color", Color) = (0,0,0,0)
		_CubeMapReflection("CubeMap Reflection", CUBE) = "white" {}
		_ReflectionIntensity("Reflection Intensity", Float) = 0
		_waterNormals2("waterNormals2", 2D) = "bump" {}
		_raw_perlin("raw_perlin", 2D) = "white" {}
		_DispIntensity("DispIntensity", Float) = 0
		_DepthDistance("Depth Distance", Float) = 0
		_FoamColor("Foam Color", Color) = (0,0,0,0)
		_FoamReach("Foam Reach", Range( 0 , 1)) = 0
		_Transparency("Transparency", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
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
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldRefl;
			INTERNAL_DATA
			float3 worldNormal;
			float4 screenPos;
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

		uniform sampler2D _NormalMap;
		uniform sampler2D _DummyTex;
		uniform sampler2D _raw_perlin;
		uniform float4 _raw_perlin_ST;
		uniform sampler2D _waterNormals2;
		uniform float4 _FoamColor;
		uniform float4 _WaterColor;
		uniform float _Attenuation;
		uniform float4 _SubSurfaceColor;
		uniform sampler2D _tile;
		uniform samplerCUBE _CubeMapReflection;
		uniform float _ReflectionIntensity;
		uniform sampler2D _CameraDepthTexture;
		uniform float _DepthDistance;
		uniform float _FoamReach;
		uniform float _Transparency;
		uniform float _DispIntensity;
		uniform float _EdgeLength;
		uniform float _TessPhongStrength;

		float4 tessFunction( appdata v0, appdata v1, appdata v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata v )
		{
			v.texcoord.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float2 panner48 = ( v.texcoord.xy + 1.0 * _Time.y * float2( 0.05,0 ));
			float4 tex2DNode34 = tex2Dlod( _tile, float4( panner48, 0, 0.0) );
			float2 panner62 = ( v.texcoord.xy + 1.0 * _Time.y * float2( 0,0.12 ));
			float temp_output_65_0 = saturate( ( ( tex2DNode34.r * 0.5 ) + ( tex2Dlod( _tile, float4( panner62, 0, 0.0) ).r * 0.5 ) ) );
			v.vertex.xyz += ( ( temp_output_65_0 * float3(0,1,0) ) * _DispIntensity );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 texCoordDummy45 = i.uv_DummyTex*float2( 1,1 ) + float2( 0,0 );
			float2 panner48 = ( texCoordDummy45 + 1.0 * _Time.y * float2( 0.05,0 ));
			float3 tex2DNode13 = UnpackNormal( tex2D( _NormalMap, panner48 ) );
			float2 uv_raw_perlin = i.uv_texcoord * _raw_perlin_ST.xy + _raw_perlin_ST.zw;
			float2 panner47 = ( texCoordDummy45 + 1.0 * _Time.y * float2( 0.09,0 ));
			o.Normal = BlendNormals( ( tex2DNode13 * tex2D( _raw_perlin, uv_raw_perlin ).r ) , UnpackNormal( tex2D( _waterNormals2, panner47 ) ) );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			float3 normalizeResult14 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float3 temp_cast_0 = (tex2DNode13.g).xxx;
			float dotResult15 = dot( normalizeResult14 , temp_cast_0 );
			float3 temp_cast_1 = (tex2DNode13.g).xxx;
			float dotResult12 = dot( ase_worldlightDir , temp_cast_1 );
			float4 tex2DNode34 = tex2D( _tile, panner48 );
			float3 ase_worldReflection = WorldReflectionVector( i, float3( 0, 0, 1 ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float fresnelNDotV41 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode41 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNDotV41, 5.0 ) );
			float4 lerpResult42 = lerp( ( _WaterColor + ( ( ( ( ( _LightColor0 * dotResult15 ) + ( _LightColor0 * pow( dotResult12 , 3.0 ) ) ) * ( pow( ( 1.0 - dotResult12 ) , 3.0 ) + _Attenuation ) ) * _SubSurfaceColor ) * tex2DNode34 ) ) , ( texCUBE( _CubeMapReflection, ase_worldReflection ) * _ReflectionIntensity ) , fresnelNode41);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth69 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth69 = abs( ( screenDepth69 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float4 lerpResult71 = lerp( _FoamColor , lerpResult42 , saturate( distanceDepth69 ));
			o.Albedo = lerpResult71.rgb;
			float2 texCoordDummy63 = i.uv_DummyTex*float2( 1,1 ) + float2( 0,0 );
			float2 panner62 = ( texCoordDummy63 + 1.0 * _Time.y * float2( 0,0.12 ));
			float temp_output_65_0 = saturate( ( ( tex2DNode34.r * 0.5 ) + ( tex2D( _tile, panner62 ).r * 0.5 ) ) );
			o.Emission = ( saturate( ( temp_output_65_0 - _FoamReach ) ) * _FoamColor ).rgb;
			o.Smoothness = 1.0;
			o.Alpha = _Transparency;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction tessphong:_TessPhongStrength 

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
				surfIN.worldRefl = -worldViewDir;
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
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
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
575;55;668;571;-3077.583;-862.1399;1.934554;False;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-1679.771,1084.321;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.PannerNode;48;-1247.533,887.4745;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;5;-434.3879,-20.64149;Float;False;World;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;10;-444.3547,169.4712;Float;False;1;0;FLOAT;0.0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;13;-957.9226,895.1647;Float;True;Property;_NormalMap;NormalMap;5;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-129.6407,46.7458;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.DotProductOpNode;12;429.3357,456.9563;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.NormalizeNode;14;114.4035,46.74584;Float;False;1;0;FLOAT3;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LightColorNode;18;671.3536,214.3443;Float;False;0;3;COLOR;FLOAT3;FLOAT
Node;AmplifyShaderEditor.PowerNode;53;746.0493,459.9114;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;3.0;False;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;724.7642,1149.451;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;15;436.2816,129.2669;Float;False;2;0;FLOAT3;0,0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;26;651.3947,655.9786;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;29;1050.919,905.3577;Float;False;Property;_Attenuation;Attenuation;6;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;1048.063,135.8798;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.PannerNode;62;1008.276,1205.355;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0.12;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.PowerNode;27;1000.537,765.6243;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;3.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;1023.546,462.0152;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;22;1404.042,318.7451;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleAddOpNode;28;1404.531,772.2488;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;61;1282.367,1232.036;Float;True;Property;_TextureSample0;Texture Sample 0;8;0;None;True;0;False;white;Auto;False;Instance;34;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;34;1243.651,990.6305;Float;True;Property;_tile;tile;8;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;32;1766.916,659.9724;Float;False;Property;_SubSurfaceColor;SubSurfaceColor;7;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;1578.336,1062.867;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;1593.757,1236.706;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;1717.246,489.3181;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.WorldReflectionVector;38;2622.317,283.969;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;64;1844.293,1064.983;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;2227.177,552.4933;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;70;2996.975,1592.6;Float;False;Property;_DepthDistance;Depth Distance;15;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;40;3048.93,500.1447;Float;False;Property;_ReflectionIntensity;Reflection Intensity;11;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;65;2079.796,1086.047;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;37;2947.161,309.443;Float;True;Property;_CubeMapReflection;CubeMap Reflection;10;0;None;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0.0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;77;2322.18,1146.394;Float;False;Property;_FoamReach;Foam Reach;17;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;2534.777,748.941;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ColorNode;36;2550.379,511.1814;Float;False;Property;_WaterColor;Water Color;9;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleSubtractOpNode;76;2684.874,1167.915;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.71;False;1;FLOAT
Node;AmplifyShaderEditor.DepthFade;69;3247.065,1596.388;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.Vector3Node;54;1948.644,1369.97;Float;False;Constant;_Vector0;Vector 0;10;0;0,1,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;3357.143,432.8588;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.PannerNode;47;-1273.957,1283.604;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.09,0;False;1;FLOAT;1.0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;35;3028.571,724.3662;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;50;-938.2508,1100.513;Float;True;Property;_raw_perlin;raw_perlin;13;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.FresnelNode;41;3035.49,963.9986;Float;False;Tangent;4;0;FLOAT3;0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;5.0;False;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;73;3540.789,1550.789;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;44;-937.2573,1319.412;Float;True;Property;_waterNormals2;waterNormals2;12;0;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SaturateNode;81;2913.789,1179.982;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;72;3075.859,1299.241;Float;False;Property;_FoamColor;Foam Color;16;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;57;2216.994,1586.1;Float;False;Property;_DispIntensity;DispIntensity;14;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;2308.925,1330.013;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT3;0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;42;3491.865,793.1435;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-600.6617,1013.33;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RangedFloatNode;43;4088.056,851.2023;Float;False;Constant;_Float0;Float 0;7;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BlendNormalsNode;49;-32.93352,1266.2;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;71;4012.687,1206.71;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;3438.526,1198.639;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;83;4339.208,1287.694;Float;False;Property;_Transparency;Transparency;18;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;2586.373,1466.173;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;4583.316,1093.537;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;MyShaders/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;True;0;False;Transparent;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;True;2;29.6;10;25;True;1;True;2;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;48;0;45;0
WireConnection;13;1;48;0
WireConnection;11;0;5;0
WireConnection;11;1;10;0
WireConnection;12;0;10;0
WireConnection;12;1;13;2
WireConnection;14;0;11;0
WireConnection;53;0;12;0
WireConnection;15;0;14;0
WireConnection;15;1;13;2
WireConnection;26;0;12;0
WireConnection;19;0;18;0
WireConnection;19;1;15;0
WireConnection;62;0;63;0
WireConnection;27;0;26;0
WireConnection;20;0;18;0
WireConnection;20;1;53;0
WireConnection;22;0;19;0
WireConnection;22;1;20;0
WireConnection;28;0;27;0
WireConnection;28;1;29;0
WireConnection;61;1;62;0
WireConnection;34;1;48;0
WireConnection;68;0;34;1
WireConnection;66;0;61;1
WireConnection;30;0;22;0
WireConnection;30;1;28;0
WireConnection;64;0;68;0
WireConnection;64;1;66;0
WireConnection;31;0;30;0
WireConnection;31;1;32;0
WireConnection;65;0;64;0
WireConnection;37;1;38;0
WireConnection;33;0;31;0
WireConnection;33;1;34;0
WireConnection;76;0;65;0
WireConnection;76;1;77;0
WireConnection;69;0;70;0
WireConnection;39;0;37;0
WireConnection;39;1;40;0
WireConnection;47;0;45;0
WireConnection;35;0;36;0
WireConnection;35;1;33;0
WireConnection;73;0;69;0
WireConnection;44;1;47;0
WireConnection;81;0;76;0
WireConnection;55;0;65;0
WireConnection;55;1;54;0
WireConnection;42;0;35;0
WireConnection;42;1;39;0
WireConnection;42;2;41;0
WireConnection;51;0;13;0
WireConnection;51;1;50;1
WireConnection;49;0;51;0
WireConnection;49;1;44;0
WireConnection;71;0;72;0
WireConnection;71;1;42;0
WireConnection;71;2;73;0
WireConnection;80;0;81;0
WireConnection;80;1;72;0
WireConnection;56;0;55;0
WireConnection;56;1;57;0
WireConnection;0;0;71;0
WireConnection;0;1;49;0
WireConnection;0;2;80;0
WireConnection;0;4;43;0
WireConnection;0;9;83;0
WireConnection;0;11;56;0
ASEEND*/
//CHKSM=427D89D6DC2D5ED3A5298F13D8910E7F115EAD70