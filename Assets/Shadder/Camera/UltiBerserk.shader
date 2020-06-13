// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UltiBerserk"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_Vector0("Vector 0", Vector) = (1,1,0,0)
		_Speed("Speed", Float) = 0
		_Float3("Float 3", Float) = -1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}

	SubShader
	{
		
		
		ZTest Always
		Cull Off
		ZWrite Off
		

		Pass
		{ 
			CGPROGRAM 

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform sampler2D _TextureSample2;
			uniform float2 _Vector0;
			uniform sampler2D _TextureSample1;
			uniform float4 _TextureSample1_ST;
			uniform float _Float0;
			uniform float _Float3;
			uniform float _Speed;

			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos ( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 uv_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode3 = tex2D( _MainTex, uv_MainTex );
				float2 uv52 = i.uv.xy * _Vector0 + float2( 0,0 );
				float2 uv_TextureSample1 = i.uv.xy * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
				float mulTime93 = _Time.y * _Speed;
				float lerpResult88 = lerp( _Float0 , _Float3 , sin( mulTime93 ));
				

				finalColor = ( tex2DNode3 + ( tex2DNode3 * ( tex2D( _TextureSample2, uv52 ) * ( ( 1.0 - tex2D( _TextureSample1, uv_TextureSample1 ).a ) * lerpResult88 ) ) ) );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
957;92;480;926;2479.201;689.015;2.470004;True;False
Node;AmplifyShaderEditor.RangedFloatNode;60;-2170.143,231.8107;Float;False;Property;_Speed;Speed;4;0;Create;True;0;0;False;0;0;8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;93;-2010.084,238.7692;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1887.736,59.17018;Float;False;Property;_Float0;Float 0;1;0;Create;True;0;0;False;0;0;-1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-1887.073,145.042;Float;False;Property;_Float3;Float 3;5;0;Create;True;0;0;False;0;-1;-1.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-1667.205,-83.31599;Float;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;None;44f47db5199ce90438b38b64a3bcdce3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;53;-1556.824,264.5255;Float;False;Property;_Vector0;Vector 0;3;0;Create;True;0;0;False;0;1,1;6.5,5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SinOpNode;94;-1837.888,239.3053;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;88;-1579.451,115.8765;Float;False;3;0;FLOAT;-1.5;False;1;FLOAT;-1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;52;-1354.239,243.0131;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;51;-1287.128,1.395088;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;50;-1100.543,213.6632;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;None;d1de8691cc27fa94c88069b4cb522ab5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-1037.048,-4.771201;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;-1339.965,-269.4008;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-751.3901,-61.18371;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-1119.206,-274.3491;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-528.8379,-144.1371;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-304.6293,-228.9196;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;-32.48377,-231.5758;Float;False;True;2;Float;ASEMaterialInspector;0;1;UltiBerserk;c71b220b631b6344493ea3cf87110c93;0;0;SubShader 0 Pass 0;1;False;False;True;Off;False;False;True;2;True;7;False;True;0;False;0;0;0;False;False;False;False;False;False;False;False;False;True;2;0;0;0;1;0;FLOAT4;0,0,0,0;False;0
WireConnection;93;0;60;0
WireConnection;94;0;93;0
WireConnection;88;0;48;0
WireConnection;88;1;90;0
WireConnection;88;2;94;0
WireConnection;52;0;53;0
WireConnection;51;0;9;4
WireConnection;50;1;52;0
WireConnection;49;0;51;0
WireConnection;49;1;88;0
WireConnection;43;0;50;0
WireConnection;43;1;49;0
WireConnection;3;0;2;0
WireConnection;55;0;3;0
WireConnection;55;1;43;0
WireConnection;54;0;3;0
WireConnection;54;1;55;0
WireConnection;0;0;54;0
ASEEND*/
//CHKSM=641974595D05163C7F44F13486E48BA3A5F37AA1