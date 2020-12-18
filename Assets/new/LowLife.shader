// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/LowLife"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_intensity("intensity", Range( 0 , 1)) = 0.2235294
		_ColorDamage("Color Damage", Color) = (1,0,0,0)
		_Active("Active", Int) = 0
		_Colornormal("Color normal", Color) = (0,0,0,0)
		_Float0("Float 0", Range( 0 , 1)) = 0

	}

	SubShader
	{
		LOD 0

		
		
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
			
			uniform int _Active;
			uniform float4 _Colornormal;
			uniform float _Float0;
			uniform float _intensity;
			uniform float4 _ColorDamage;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos( v.vertex );
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
				float2 uv0_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float temp_output_27_0 = ( _Float0 * _intensity );
				float4 tex2DNode9 = tex2D( _MainTex, (uv0_MainTex*( 1.0 - temp_output_27_0 ) + ( temp_output_27_0 / 2.0 )) );
				float4 lerpResult41 = lerp( ( _Colornormal * tex2DNode9 ) , ( tex2DNode9 * _ColorDamage ) , _Float0);
				

				finalColor = lerpResult41;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18301
1759;343;1520;756;1392.735;655.364;2.172297;True;True
Node;AmplifyShaderEditor.RangedFloatNode;20;-953.8992,496.6582;Inherit;False;Property;_intensity;intensity;0;0;Create;True;0;0;False;0;False;0.2235294;0.108;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-422.4811,565.1578;Inherit;False;Property;_Float0;Float 0;5;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;2;-803.5822,-281.7015;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-692.7075,312.9938;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;23;-444.6447,229.7158;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-679.3729,-131.3647;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;28;-679.9954,173.0055;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;18;-406.088,6.434906;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0.8;False;2;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;9;-123.1756,-41.64268;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-192.3173,204.1027;Inherit;False;Property;_ColorDamage;Color Damage;2;0;Create;True;0;0;False;0;False;1,0,0,0;0.5582948,0.911768,0.9622642,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;56;-29.50789,-342.7202;Inherit;False;Property;_Colornormal;Color normal;4;0;Create;True;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;378.0508,-187.3727;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;407.8495,36.49142;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;29;-718.1794,1243.585;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleRemainderNode;39;-74.88441,858.441;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;15;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1172.873,1257.56;Inherit;False;Property;_speed;speed;1;0;Create;True;0;0;False;0;False;3.86;3.421556;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;249.742,911.0034;Inherit;False;2;2;0;INT;0;False;1;INT;0;False;1;INT;0
Node;AmplifyShaderEditor.StepOpNode;50;424.2341,812.739;Inherit;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;37;131.5995,1058.476;Inherit;False;2;0;FLOAT;0;False;1;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;35;-96.88242,1075.418;Inherit;False;Property;_Active;Active;3;0;Create;True;0;0;True;0;False;0;18;0;1;INT;0
Node;AmplifyShaderEditor.SinOpNode;36;446.2635,1183.94;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;260.4497,1190.369;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;733.3185,1147.178;Inherit;False;PulsationSound;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-1209.347,201.8697;Inherit;False;44;PulsationSound;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;567.1938,563.0671;Inherit;False;44;PulsationSound;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;41;759.832,102.5857;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.AbsOpNode;25;613.9843,1189.239;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;939.959,-48.24458;Float;False;True;-1;2;ASEMaterialInspector;0;2;Custom/LowLife;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;27;0;57;0
WireConnection;27;1;20;0
WireConnection;23;0;27;0
WireConnection;4;2;2;0
WireConnection;28;1;27;0
WireConnection;18;0;4;0
WireConnection;18;1;28;0
WireConnection;18;2;23;0
WireConnection;9;0;2;0
WireConnection;9;1;18;0
WireConnection;54;0;56;0
WireConnection;54;1;9;0
WireConnection;47;0;9;0
WireConnection;47;1;11;0
WireConnection;29;0;31;0
WireConnection;39;0;29;0
WireConnection;51;0;35;0
WireConnection;50;0;39;0
WireConnection;50;1;51;0
WireConnection;37;0;39;0
WireConnection;37;1;35;0
WireConnection;36;0;30;0
WireConnection;30;0;37;0
WireConnection;30;1;29;0
WireConnection;44;0;25;0
WireConnection;41;0;54;0
WireConnection;41;1;47;0
WireConnection;41;2;57;0
WireConnection;25;0;36;0
WireConnection;1;0;41;0
ASEEND*/
//CHKSM=0763A28A50CC1DB3B7984D4ACA292EECF3A34038