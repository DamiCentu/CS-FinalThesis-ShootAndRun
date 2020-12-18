// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Abduction"
{
	Properties
	{
		_TessValue( "Max Tessellation", Range( 1, 32 ) ) = 7.5
		_TessMin( "Tess Min Distance", Float ) = 10
		_TessMax( "Tess Max Distance", Float ) = 25
		_Opacity("Opacity", Range( 0 , 1)) = 0.5
		_ShieldPatternColor("Shield Pattern Color", Color) = (0.2470588,0.7764706,0.9098039,1)
		_ShieldPatternPower("Shield Pattern Power", Range( 0 , 100)) = 5
		_ShieldRimPower("Shield Rim Power", Range( 0 , 10)) = 7
		_ShieldAnimSpeed("Shield Anim Speed", Range( -1 , 1)) = 3
		_Vector3("Vector 3", Vector) = (1,1,0,0)
		_ShieldPatternWaves("Shield Pattern Waves", 2D) = "white" {}
		_shakeIntencity("shakeIntencity", Range( 0 , 0.3)) = 0.1473639
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "Tessellation.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform sampler2D _ShieldPatternWaves;
		uniform float _ShieldAnimSpeed;
		uniform float _shakeIntencity;
		uniform float _ShieldRimPower;
		uniform float2 _Vector3;
		uniform float4 _ShieldPatternColor;
		uniform float _ShieldPatternPower;
		uniform float _Opacity;
		uniform float _TessValue;
		uniform float _TessMin;
		uniform float _TessMax;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityDistanceBasedTess( v0.vertex, v1.vertex, v2.vertex, _TessMin, _TessMax, _TessValue );
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float3 ase_vertexNormal = v.normal.xyz;
			float4 temp_output_36_0 = ( _Time * _ShieldAnimSpeed );
			float2 uv_TexCoord41 = v.texcoord.xy + temp_output_36_0.xy;
			float4 ShieldPattern17 = tex2Dlod( _ShieldPatternWaves, float4( uv_TexCoord41, 0, 1.0) );
			v.vertex.xyz += ( ( float4( ase_vertexNormal , 0.0 ) * ShieldPattern17 ) * _shakeIntencity ).rgb;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV8 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode8 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV8, (10.0 + (_ShieldRimPower - 0.0) * (0.0 - 10.0) / (10.0 - 0.0)) ) );
			float4 temp_output_36_0 = ( _Time * _ShieldAnimSpeed );
			float2 uv_TexCoord41 = i.uv_texcoord + temp_output_36_0.xy;
			float4 ShieldPattern17 = tex2D( _ShieldPatternWaves, uv_TexCoord41 );
			float2 uv_TexCoord87 = i.uv_texcoord * _Vector3 + temp_output_36_0.xy;
			float4 waves94 = tex2D( _ShieldPatternWaves, uv_TexCoord87 );
			o.Emission = ( ( ( ( fresnelNode8 + ShieldPattern17 ) * waves94 ) * _ShieldPatternColor ) * _ShieldPatternPower ).rgb;
			o.Alpha = _Opacity;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 

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
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
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
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
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
Version=18301
1753;155;1520;756;1921.006;1408.734;1.899109;True;True
Node;AmplifyShaderEditor.TimeNode;34;-2941.424,-1239.79;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;35;-3015.016,-1046.636;Float;False;Property;_ShieldAnimSpeed;Shield Anim Speed;9;0;Create;True;0;0;False;0;False;3;-0.1;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-2698.888,-1116.376;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector2Node;97;-2212.124,-1034.182;Float;False;Property;_Vector3;Vector 3;10;0;Create;True;0;0;False;0;False;1,1;5,5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;41;-2180.397,-1393.801;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;87;-2021.422,-855.7429;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1921.001,-1412.336;Inherit;True;Property;_ShieldPattern;Shield Pattern;11;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;86;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-2281.597,-2416.874;Float;False;Property;_ShieldRimPower;Shield Rim Power;8;0;Create;True;0;0;False;0;False;7;1.6;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;86;-1776.723,-878.3155;Inherit;True;Property;_ShieldPatternWaves;Shield Pattern Waves;11;0;Create;True;0;0;False;0;False;-1;None;b7d4c32b4c281614ca4d0bd9908c26ae;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;94;-1480.617,-933.9607;Float;False;waves;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;30;-1989.888,-2285.618;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;10;False;3;FLOAT;10;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;17;-1531.8,-1409.1;Float;False;ShieldPattern;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;8;-1778.287,-2283.918;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-1644.021,-1996.199;Inherit;False;94;waves;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;125;-1662.897,-2074.487;Inherit;False;17;ShieldPattern;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.WireNode;270;-1445.601,-2008.019;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-1451.572,-2252.348;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;301;-1156.458,-690.2922;Inherit;False;17;ShieldPattern;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalVertexDataNode;296;-1094.869,-1047.03;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1206.437,-2207.714;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;3;-1367.112,-1927.641;Float;False;Property;_ShieldPatternColor;Shield Pattern Color;6;0;Create;True;0;0;False;0;False;0.2470588,0.7764706,0.9098039,1;0.172549,0.668071,0.772549,1;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;297;-875.8674,-958.6545;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;300;-794.8504,-542.1216;Inherit;False;Property;_shakeIntencity;shakeIntencity;12;0;Create;True;0;0;False;0;False;0.1473639;0.08683019;0;0.3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-994.0899,-2053.895;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1079.71,-1707.878;Float;False;Property;_ShieldPatternPower;Shield Pattern Power;7;0;Create;True;0;0;False;0;False;5;5.4;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;299;-560.9449,-973.5712;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;-663.4163,-2090.332;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-893.5145,-1351.604;Float;False;Property;_Opacity;Opacity;5;0;Create;True;0;0;False;0;False;0.5;0.245;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-129.3663,-1507.179;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;Custom/Abduction;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;1;False;-1;7;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;0;7.5;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;1;False;-1;1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;36;0;34;0
WireConnection;36;1;35;0
WireConnection;41;1;36;0
WireConnection;87;0;97;0
WireConnection;87;1;36;0
WireConnection;1;1;41;0
WireConnection;86;1;87;0
WireConnection;94;0;86;0
WireConnection;30;0;31;0
WireConnection;17;0;1;0
WireConnection;8;3;30;0
WireConnection;270;0;20;0
WireConnection;53;0;8;0
WireConnection;53;1;125;0
WireConnection;5;0;53;0
WireConnection;5;1;270;0
WireConnection;297;0;296;0
WireConnection;297;1;301;0
WireConnection;95;0;5;0
WireConnection;95;1;3;0
WireConnection;299;0;297;0
WireConnection;299;1;300;0
WireConnection;126;0;95;0
WireConnection;126;1;6;0
WireConnection;0;2;126;0
WireConnection;0;9;28;0
WireConnection;0;11;299;0
ASEEND*/
//CHKSM=E27A6400EF23BE11B268A3E29295BB4CA79C6330