// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/LaserShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_noiseSpeed("noiseSpeed", Vector) = (0,10,0,0)
		_Vector6("Vector 6", Vector) = (0,10,0,0)
		_Vector3("Vector 3", Vector) = (0,10,0,0)
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		[HDR]_COlor("COlor", Color) = (0,0,0,0)
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_noiseStrength("noiseStrength", Float) = 4
		_Vector5("Vector 5", Vector) = (2,2,0,0)
		_EdgesSmoothStrength("EdgesSmoothStrength", Float) = 0
		_Brigthness("Brigthness", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Brigthness;
		uniform sampler2D _TextureSample1;
		uniform float2 _Vector3;
		uniform sampler2D _TextureSample3;
		uniform float2 _noiseSpeed;
		uniform float2 _Vector5;
		uniform float _noiseStrength;
		uniform float4 _COlor;
		uniform sampler2D _TextureSample4;
		uniform float2 _Vector6;
		uniform float _EdgesSmoothStrength;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 panner37 = ( 1.0 * _Time.y * _Vector3 + i.uv_texcoord);
			float2 uv_TexCoord46 = i.uv_texcoord * _Vector5;
			float2 panner45 = ( 1.0 * _Time.y * _noiseSpeed + uv_TexCoord46);
			float4 tex2DNode36 = tex2D( _TextureSample1, ( panner37 + pow( tex2D( _TextureSample3, panner45 ).b , _noiseStrength ) ) );
			float cos62 = cos( radians( 90.0 ) );
			float sin62 = sin( radians( 90.0 ) );
			float2 rotator62 = mul( i.uv_texcoord - _Vector6 , float2x2( cos62 , -sin62 , sin62 , cos62 )) + _Vector6;
			float temp_output_64_0 = pow( tex2D( _TextureSample4, rotator62 ).a , _EdgesSmoothStrength );
			o.Emission = ( _Brigthness * ( ( tex2DNode36 * _COlor ) * temp_output_64_0 ) ).rgb;
			float temp_output_68_0 = ( temp_output_64_0 * tex2DNode36.b );
			o.Alpha = temp_output_68_0;
			clip( temp_output_68_0 - _Cutoff );
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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
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
Version=15301
0;92;1203;926;2192.485;422.358;2.025574;True;False
Node;AmplifyShaderEditor.Vector2Node;50;-2536.761,350.8822;Float;False;Property;_Vector5;Vector 5;9;0;Create;True;0;0;False;0;2,2;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;46;-2333.568,334.2312;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;47;-2263.306,457.7261;Float;False;Property;_noiseSpeed;noiseSpeed;1;0;Create;True;0;0;False;0;0,10;8,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;45;-2083.286,375.3634;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1771.368,569.9867;Float;False;Property;_noiseStrength;noiseStrength;8;0;Create;True;0;0;False;0;4;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;44;-1895.681,347.6774;Float;True;Property;_TextureSample3;Texture Sample 3;7;0;Create;True;0;0;False;0;None;7f1cd443fabe3b34ab368f436b25ebf4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;38;-1933.358,70.68311;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;60;-1895.027,-58.74007;Float;False;Constant;_Float3;Float 3;20;0;Create;True;0;0;False;0;90;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;39;-1862.657,198.644;Float;False;Property;_Vector3;Vector 3;3;0;Create;True;0;0;False;0;0,10;8,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;59;-1770.531,-330.1324;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;58;-1773.807,-206.5176;Float;False;Property;_Vector6;Vector 6;2;0;Create;True;0;0;False;0;0,10;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PowerNode;48;-1541.378,313.4586;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RadiansOpNode;61;-1684.063,-61.58825;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;37;-1642.387,142.2127;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;53;-1359.526,135.3065;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RotatorNode;62;-1502.266,-216.3274;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;40;-1163.896,347.4749;Float;False;Property;_COlor;COlor;5;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-1195.538,102.7355;Float;True;Property;_TextureSample1;Texture Sample 1;4;0;Create;True;0;0;False;0;None;09f070c55755b0b48bf8cda8ae67a7fe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;63;-1233.186,-284.9044;Float;True;Property;_TextureSample4;Texture Sample 4;6;0;Create;True;0;0;False;0;None;172ccf4c916e2dc4581a34c70875fed7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;52;-1196.135,-53.38277;Float;False;Property;_EdgesSmoothStrength;EdgesSmoothStrength;10;0;Create;True;0;0;False;0;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-792.0406,159.392;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;64;-874.8865,-192.0971;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-482.0957,10.75922;Float;False;Property;_Brigthness;Brigthness;11;0;Create;True;0;0;False;0;0;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-553.5536,124.6962;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-303.0957,29.75922;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-547.9244,339.5237;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;MyShaders/LaserShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;0;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;46;0;50;0
WireConnection;45;0;46;0
WireConnection;45;2;47;0
WireConnection;44;1;45;0
WireConnection;48;0;44;3
WireConnection;48;1;49;0
WireConnection;61;0;60;0
WireConnection;37;0;38;0
WireConnection;37;2;39;0
WireConnection;53;0;37;0
WireConnection;53;1;48;0
WireConnection;62;0;59;0
WireConnection;62;1;58;0
WireConnection;62;2;61;0
WireConnection;36;1;53;0
WireConnection;63;1;62;0
WireConnection;41;0;36;0
WireConnection;41;1;40;0
WireConnection;64;0;63;4
WireConnection;64;1;52;0
WireConnection;43;0;41;0
WireConnection;43;1;64;0
WireConnection;69;0;70;0
WireConnection;69;1;43;0
WireConnection;68;0;64;0
WireConnection;68;1;36;3
WireConnection;0;2;69;0
WireConnection;0;9;68;0
WireConnection;0;10;68;0
ASEEND*/
//CHKSM=A9FC1B302B1CD8A03E00D03CA0B192BB697DDE6A