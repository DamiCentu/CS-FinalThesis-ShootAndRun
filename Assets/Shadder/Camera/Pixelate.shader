// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Pixelate"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_PixelY("PixelY", Range( 0 , 5000)) = 200
		_PixelX("PixelX", Range( 0 , 5000)) = 200
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
			
			uniform float _PixelX;
			uniform float _PixelY;

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
				float2 uv5 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float pixelWidth4 =  1.0f / _PixelX;
				float pixelHeight4 = 1.0f / _PixelY;
				half2 pixelateduv4 = half2((int)(uv5.x / pixelWidth4) * pixelWidth4, (int)(uv5.y / pixelHeight4) * pixelHeight4);
				

				finalColor = tex2D( _MainTex, pixelateduv4 );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
320;107;1379;788;1048.227;403.7005;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;6;-969.8464,3.194643;Float;False;Property;_PixelX;PixelX;1;0;Create;True;0;0;False;0;200;0;0;5000;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-968.9047,83.47366;Float;False;Property;_PixelY;PixelY;0;0;Create;True;0;0;False;0;200;0;0;5000;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-962.0111,-118.3269;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;-513.7009,-147.1962;Float;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCPixelate;4;-565.3008,-81.39616;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;3;-350.9568,-109.5962;Float;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;46,-100;Float;False;True;2;Float;ASEMaterialInspector;0;1;Pixelate;c71b220b631b6344493ea3cf87110c93;0;0;SubShader 0 Pass 0;1;False;False;True;Off;False;False;True;2;True;7;False;True;0;False;0;0;0;False;False;False;False;False;False;False;False;False;True;2;0;0;0;1;0;FLOAT4;0,0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;4;1;6;0
WireConnection;4;2;7;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;0;0;3;0
ASEEND*/
//CHKSM=DD19ED39DFF1568B3F3D0FA71B0E7B01240E0A1D