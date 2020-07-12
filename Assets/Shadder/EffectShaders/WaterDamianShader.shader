// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WaterDamianMaterial"
{
	Properties
	{
		_NormalMap("NormalMap", 2D) = "bump" {}
		_NormalLerpStrength("NormalLerpStrength", Range( 0 , 1)) = 0
		_AnimationUV1xyUV2zw("AnimationUV1(xy)UV2(zw)", Vector) = (0,0,0,0)
		_TilingUV2("TilingUV2", Vector) = (1,1,0,0)
		_ScaleUV2("ScaleUV2", Vector) = (1,1,0,0)
		_TilingUV1("TilingUV1", Vector) = (1,1,0,0)
		_ScaleUV1("ScaleUV1", Vector) = (1,1,0,0)
		_Normal2Stegth("Normal2Stegth", Float) = 0
		_Normal1Stegth("Normal1Stegth", Float) = 0
		_MainColor("MainColor", Color) = (0,0,0,0)
		_FresnelPower("FresnelPower", Range( 0 , 4)) = 0
		_DepthDistance("DepthDistance", Float) = 0
		_CameraDepthFadeOffset("CameraDepthFadeOffset", Float) = 0
		_CameraDepthFadeLength("CameraDepthFadeLength", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Off
		GrabPass{ }
		CGPROGRAM
		#include "UnityPBSLighting.cginc"
		#include "UnityCG.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf StandardCustomLighting alpha:fade keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float eyeDepth;
		};

		struct SurfaceOutputCustomLightingCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _GrabTexture;
		uniform float _Normal1Stegth;
		uniform sampler2D _CameraDepthTexture;
		uniform float _DepthDistance;
		uniform sampler2D _NormalMap;
		uniform float4 _AnimationUV1xyUV2zw;
		uniform float2 _ScaleUV1;
		uniform float2 _TilingUV1;
		uniform float _Normal2Stegth;
		uniform float2 _ScaleUV2;
		uniform float2 _TilingUV2;
		uniform float _NormalLerpStrength;
		uniform float4 _MainColor;
		uniform float _FresnelPower;
		uniform float _CameraDepthFadeLength;
		uniform float _CameraDepthFadeOffset;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.eyeDepth = -UnityObjectToViewPos( v.vertex.xyz ).z;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float2 appendResult6 = (float2(ase_grabScreenPosNorm.r , ase_grabScreenPosNorm.g));
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth72 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth72 = abs( ( screenDepth72 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float DepthFade73 = saturate( distanceDepth72 );
			float3 ase_worldPos = i.worldPos;
			float2 temp_output_17_0 = (ase_worldPos).xz;
			float2 appendResult23 = (float2(( _Time.x * _AnimationUV1xyUV2zw.x ) , ( _Time.x * _AnimationUV1xyUV2zw.y )));
			float2 uv130 = ( ( ( temp_output_17_0 + appendResult23 ) * _ScaleUV1 ) / _TilingUV1 );
			float2 appendResult33 = (float2(( _AnimationUV1xyUV2zw.z * _Time.x ) , ( _AnimationUV1xyUV2zw.w * _Time.x )));
			float2 uv241 = ( ( ( temp_output_17_0 + appendResult33 ) * _ScaleUV2 ) / _TilingUV2 );
			float3 lerpResult7 = lerp( UnpackScaleNormal( tex2D( _NormalMap, uv130 ) ,( _Normal1Stegth * DepthFade73 ) ) , UnpackScaleNormal( tex2D( _NormalMap, uv241 ) ,( _Normal2Stegth * DepthFade73 ) ) , _NormalLerpStrength.x);
			float3 normalMapping9 = lerpResult7;
			float2 screenUV10 = ( appendResult6 - ( (normalMapping9).xy * 0.1 ) );
			float4 screenColor1 = tex2D( _GrabTexture, screenUV10 );
			float3 indirectNormal69 = WorldNormalVector( i , normalMapping9 );
			Unity_GlossyEnvironmentData g69 = UnityGlossyEnvironmentSetup( 1.0, data.worldViewDir, indirectNormal69, float3(0,0,0));
			float3 indirectSpecular69 = UnityGI_IndirectSpecular( data, 1.0, indirectNormal69, g69 );
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_vertexNormal = mul( unity_WorldToObject, float4( ase_worldNormal, 0 ) );
			float2 appendResult63 = (float2(ase_vertexNormal.x , ase_vertexNormal.y));
			float3 appendResult67 = (float3(( appendResult63 - (normalMapping9).xy ) , ase_vertexNormal.z));
			float dotResult55 = dot( ase_worldViewDir , appendResult67 );
			float Fresnel61 = pow( ( 1.0 - saturate( abs( dotResult55 ) ) ) , _FresnelPower );
			float cameraDepthFade78 = (( i.eyeDepth -_ProjectionParams.y - _CameraDepthFadeOffset ) / _CameraDepthFadeLength);
			float CameraDepthFade81 = saturate( cameraDepthFade78 );
			float4 lerpResult51 = lerp( screenColor1 , ( float4( indirectSpecular69 , 0.0 ) + _MainColor ) , ( Fresnel61 * DepthFade73 * CameraDepthFade81 ));
			c.rgb = lerpResult51.rgb;
			c.a = 1;
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
394;29;1519;1004;3394.63;385.9005;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;16;-2828.351,1308.306;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;17;-2604.351,1308.306;Float;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;22;-2927.76,1695.576;Float;False;Property;_AnimationUV1xyUV2zw;AnimationUV1(xy)UV2(zw);2;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;19;-2828.351,1484.307;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2494.092,1735.036;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;36;-2233.804,1477.548;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-2499.517,1843.06;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-2524.351,1484.307;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2521.776,1589.331;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-2289.804,1729.548;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WireNode;37;-2119.804,1586.548;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;23;-2364.353,1530.707;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-2380.702,-715.2341;Float;False;Property;_DepthDistance;DepthDistance;11;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;-2074,1702;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-2140.351,1420.307;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;45;-2067.952,1820.383;Float;False;Property;_ScaleUV2;ScaleUV2;4;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;43;-2108.804,1269.548;Float;False;Property;_ScaleUV1;ScaleUV1;6;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DepthFade;72;-2134.46,-715.6146;Float;False;True;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;42;-1877.804,1264.548;Float;False;Property;_TilingUV1;TilingUV1;5;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;44;-1843.307,1816.442;Float;False;Property;_TilingUV2;TilingUV2;3;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1922.292,1416.688;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SaturateNode;77;-1896.154,-708.1578;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1887.743,1678.531;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;40;-1670.077,1687.698;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;25;-1704.626,1425.855;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;73;-1713.801,-675.5179;Float;False;DepthFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-2588.964,247.6366;Float;False;Property;_Normal2Stegth;Normal2Stegth;7;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;84;-2571.63,384.0995;Float;False;73;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-2578.797,34.61452;Float;False;Property;_Normal1Stegth;Normal1Stegth;8;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-1511.592,1702.415;Float;False;uv2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-1546.141,1440.572;Float;False;uv1;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-2329.63,256.0995;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-2341.63,43.09949;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-2567.505,-55.08447;Float;False;30;0;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;-2564.988,162.5406;Float;False;41;0;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-2151.853,338.264;Float;False;Property;_NormalLerpStrength;NormalLerpStrength;1;0;Create;True;0;0;False;0;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-2151.786,-71.26723;Float;True;Property;_NormalMap;NormalMap;0;0;Create;True;0;0;False;0;None;d4ac8d42cb620df4d93d812c3377d94d;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-2160.587,135.2328;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Instance;2;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;7;-1785.722,116.9305;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-3200.377,-334.1857;Float;False;9;0;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;53;-3183.388,-528.5144;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;-1589.403,119.0037;Float;False;normalMapping;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;63;-2884.124,-504.5806;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;65;-2972.73,-343.7278;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;68;-2690.135,-410.9484;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;54;-2560.803,-601.6293;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;67;-2527.758,-373.1134;Float;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;55;-2328.741,-492.9599;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;11;-2415.807,742.9175;Float;False;9;0;1;FLOAT3;0
Node;AmplifyShaderEditor.AbsOpNode;56;-2140.81,-470.3818;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;57;-1992.063,-468.1944;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;15;-2146.44,740.7542;Float;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-2748.154,-825.1578;Float;False;Property;_CameraDepthFadeOffset;CameraDepthFadeOffset;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;4;-2144.572,549.9525;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;13;-2091.461,826.527;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2755.154,-914.1578;Float;False;Property;_CameraDepthFadeLength;CameraDepthFadeLength;13;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;78;-2408.154,-921.1578;Float;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1853.609,731.3858;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-1859.07,596.7619;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-1968.002,-367.5716;Float;False;Property;_FresnelPower;FresnelPower;10;0;Create;True;0;0;False;0;0;1;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;58;-1834.567,-472.5692;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;83;-2040.356,-897.7431;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;59;-1633.322,-472.5692;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;-1059.299,348.7958;Float;False;9;0;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;5;-1658.889,684.2554;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;81;-1748.488,-902.8244;Float;False;CameraDepthFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;75;-393.1543,540.8422;Float;False;73;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;-1490.343,698.23;Float;False;screenUV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;14;-1040.301,219.7032;Float;False;10;0;1;FLOAT2;0
Node;AmplifyShaderEditor.IndirectSpecularLight;69;-789.0557,395.4952;Float;False;Tangent;3;0;FLOAT3;0,0,1;False;1;FLOAT;1;False;2;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-1438.068,-470.6033;Float;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;52;-734.6273,532.1857;Float;False;Property;_MainColor;MainColor;9;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;62;-416.5008,440.0278;Float;False;61;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;-438.0887,623.7404;Float;False;81;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;1;-672.0412,214.0878;Float;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-404.6894,318.3567;Float;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;76;-177.1543,515.8422;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;51;-220.4874,238.8413;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;CustomLighting;WaterDamianMaterial;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;0;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;16;0
WireConnection;31;0;22;3
WireConnection;31;1;19;1
WireConnection;36;0;17;0
WireConnection;32;0;22;4
WireConnection;32;1;19;1
WireConnection;20;0;19;1
WireConnection;20;1;22;1
WireConnection;21;0;19;1
WireConnection;21;1;22;2
WireConnection;33;0;31;0
WireConnection;33;1;32;0
WireConnection;37;0;36;0
WireConnection;23;0;20;0
WireConnection;23;1;21;0
WireConnection;35;0;37;0
WireConnection;35;1;33;0
WireConnection;18;0;17;0
WireConnection;18;1;23;0
WireConnection;72;0;74;0
WireConnection;24;0;18;0
WireConnection;24;1;43;0
WireConnection;77;0;72;0
WireConnection;39;0;35;0
WireConnection;39;1;45;0
WireConnection;40;0;39;0
WireConnection;40;1;44;0
WireConnection;25;0;24;0
WireConnection;25;1;42;0
WireConnection;73;0;77;0
WireConnection;41;0;40;0
WireConnection;30;0;25;0
WireConnection;85;0;50;0
WireConnection;85;1;84;0
WireConnection;86;0;49;0
WireConnection;86;1;84;0
WireConnection;2;1;46;0
WireConnection;2;5;86;0
WireConnection;3;1;47;0
WireConnection;3;5;85;0
WireConnection;7;0;2;0
WireConnection;7;1;3;0
WireConnection;7;2;8;0
WireConnection;9;0;7;0
WireConnection;63;0;53;1
WireConnection;63;1;53;2
WireConnection;65;0;64;0
WireConnection;68;0;63;0
WireConnection;68;1;65;0
WireConnection;67;0;68;0
WireConnection;67;2;53;3
WireConnection;55;0;54;0
WireConnection;55;1;67;0
WireConnection;56;0;55;0
WireConnection;57;0;56;0
WireConnection;15;0;11;0
WireConnection;78;0;79;0
WireConnection;78;1;80;0
WireConnection;12;0;15;0
WireConnection;12;1;13;0
WireConnection;6;0;4;1
WireConnection;6;1;4;2
WireConnection;58;0;57;0
WireConnection;83;0;78;0
WireConnection;59;0;58;0
WireConnection;59;1;60;0
WireConnection;5;0;6;0
WireConnection;5;1;12;0
WireConnection;81;0;83;0
WireConnection;10;0;5;0
WireConnection;69;0;70;0
WireConnection;61;0;59;0
WireConnection;1;0;14;0
WireConnection;71;0;69;0
WireConnection;71;1;52;0
WireConnection;76;0;62;0
WireConnection;76;1;75;0
WireConnection;76;2;82;0
WireConnection;51;0;1;0
WireConnection;51;1;71;0
WireConnection;51;2;76;0
WireConnection;0;13;51;0
ASEEND*/
//CHKSM=5F84408788D50A02B547282813B186D9355807BD