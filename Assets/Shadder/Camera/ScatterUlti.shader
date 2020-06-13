// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ScatterUlti"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Vector3("Vector 3", Vector) = (0,0,0,0)
		_Vector1("Vector 1", Vector) = (0,0,0,0)
		_Float0("Float 0", Float) = 0
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Vector2("Vector 2", Vector) = (1,1,0,0)
		_Vector0("Vector 0", Vector) = (1,1,0,0)
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
			
			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;
			uniform float _Float0;
			uniform sampler2D _TextureSample1;
			uniform float2 _Vector2;
			uniform float2 _Vector1;
			uniform sampler2D _TextureSample3;
			uniform float2 _Vector0;
			uniform float2 _Vector3;

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
				float4 tex2DNode15 = tex2D( _MainTex, uv_MainTex );
				float2 uv_TextureSample0 = i.uv.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float2 uv31 = i.uv.xy * _Vector2 + ( _Time.y * _Vector1 );
				float2 uv66 = i.uv.xy * _Vector0 + ( _Time.y * _Vector3 );
				

				finalColor = ( tex2DNode15 + ( tex2DNode15 * ( ( 1.0 - pow( tex2D( _TextureSample0, uv_TextureSample0 ).a , _Float0 ) ) * ( tex2D( _TextureSample1, uv31 ).r + tex2D( _TextureSample3, uv66 ).r ) ) ) );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
715;92;813;926;1351.506;636.6199;2.366709;False;False
Node;AmplifyShaderEditor.SimpleTimeNode;25;-2609.11,303.8855;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;68;-2717.344,934.4281;Float;False;Property;_Vector3;Vector 3;1;0;Create;True;0;0;False;0;0,0;0.5,0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;24;-2619.26,471.558;Float;False;Property;_Vector1;Vector 1;2;0;Create;True;0;0;False;0;0,0;-0.5,-0.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;69;-2697.186,818.6993;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-2355.451,376.9734;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;32;-2349.321,643.2631;Float;False;Property;_Vector2;Vector 2;6;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;70;-2443.529,891.7874;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;67;-2439.398,1159.076;Float;False;Property;_Vector0;Vector 0;7;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;5;-2273.212,-107.1469;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;44f47db5199ce90438b38b64a3bcdce3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;66;-2222.257,1001.676;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;-2134.179,486.8621;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;62;-2130.746,137.4155;Float;False;Property;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;71;-1889.307,731.2192;Float;True;Property;_TextureSample3;Texture Sample 3;4;0;Create;True;0;0;False;0;None;0c4ba924a1d7e6642b8cf822a1973a6c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;64;-1833.32,326.8418;Float;True;Property;_TextureSample1;Texture Sample 1;5;0;Create;True;0;0;False;0;None;0c4ba924a1d7e6642b8cf822a1973a6c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;61;-1858.245,-28.31256;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;10;-1337.079,-43.22219;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;13;-1623.035,-275.1652;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;72;-1469.337,485.4444;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1098.011,-65.53002;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;15;-1402.276,-280.1136;Float;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;-811.9075,-149.9014;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-587.6992,-234.684;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinTimeNode;86;-1336.985,724.1828;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;80;-471.6342,282.3718;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CosTime;87;-1165.982,781.0992;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;89;-1203.13,1027.048;Float;False;Property;_Float1;Float 1;8;0;Create;True;0;0;False;0;0;13.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;84;-891.0927,222.2746;Float;False;Constant;_Color1;Color 1;8;0;Create;True;0;0;False;0;0.9044118,0.5052232,0,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;88;-838.9341,796.9195;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;79;-948.1533,504.6156;Float;False;Constant;_Color0;Color 0;8;0;Create;True;0;0;False;0;0.9322993,0.9411765,0.2975779,1;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;1;ScatterUlti;c71b220b631b6344493ea3cf87110c93;0;0;SubShader 0 Pass 0;1;False;False;True;Off;False;False;True;2;True;7;False;True;0;False;0;0;0;False;False;False;False;False;False;False;False;False;True;2;0;0;0;1;0;FLOAT4;0,0,0,0;False;0
WireConnection;26;0;25;0
WireConnection;26;1;24;0
WireConnection;70;0;69;0
WireConnection;70;1;68;0
WireConnection;66;0;67;0
WireConnection;66;1;70;0
WireConnection;31;0;32;0
WireConnection;31;1;26;0
WireConnection;71;1;66;0
WireConnection;64;1;31;0
WireConnection;61;0;5;4
WireConnection;61;1;62;0
WireConnection;10;0;61;0
WireConnection;72;0;64;1
WireConnection;72;1;71;1
WireConnection;12;0;10;0
WireConnection;12;1;72;0
WireConnection;15;0;13;0
WireConnection;16;0;15;0
WireConnection;16;1;12;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;80;0;84;0
WireConnection;80;1;79;0
WireConnection;80;2;88;0
WireConnection;88;0;87;4
WireConnection;88;1;89;0
WireConnection;0;0;17;0
ASEEND*/
//CHKSM=F1F65A5FD34D541505152A51E662025AF8CB329E