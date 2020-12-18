// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Disolve"
{
	Properties
	{
		_hitPosition("hitPosition", Vector) = (0,1,0,0)
		_NoiseStrength("Noise Strength", Range( 0 , 1.3)) = 0.5
		_Anchodelborde("Ancho del borde", Range( 0 , 0.2)) = 0
		_TexturaPrincipal("TexturaPrincipal", 2D) = "white" {}
		_ColorBorder("Color Border", Color) = (0,0.2269145,0.9811321,0)
		_ColorDamage("Color Damage", Color) = (0.5566038,0.1654058,0.1654058,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "bossShader"="bossShader" "Enemie"="Enemie" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform float3 _hitPosition;
		uniform float _NoiseStrength;
		uniform float _Anchodelborde;
		uniform float4 _ColorBorder;
		uniform float4 _ColorDamage;
		uniform sampler2D _TexturaPrincipal;
		uniform float4 _TexturaPrincipal_ST;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float temp_output_9_0 = ( distance( ase_vertex3Pos , _hitPosition ) * ( 1.0 / (0.1 + (abs( _CosTime.w ) - 0.0) * (1.0 - 0.1) / (1.0 - 0.0)) ) );
			float simplePerlin2D3 = snoise( i.uv_texcoord*20.0 );
			simplePerlin2D3 = simplePerlin2D3*0.5 + 0.5;
			float temp_output_21_0 = ( simplePerlin2D3 * _NoiseStrength );
			float MascaraTexturaDanio56 = step( temp_output_9_0 , temp_output_21_0 );
			float MascaraTexturaPrincipal55 = step( temp_output_21_0 , ( temp_output_9_0 + ( _Anchodelborde * -1.0 ) ) );
			float2 uv_TexturaPrincipal = i.uv_texcoord * _TexturaPrincipal_ST.xy + _TexturaPrincipal_ST.zw;
			o.Albedo = ( ( ( 1.0 - ( MascaraTexturaDanio56 + MascaraTexturaPrincipal55 ) ) * _ColorBorder ) + ( _ColorDamage * MascaraTexturaDanio56 ) + ( tex2D( _TexturaPrincipal, uv_TexturaPrincipal ) * MascaraTexturaPrincipal55 ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
1753;155;1520;756;2958.19;-755.939;3.851726;True;True
Node;AmplifyShaderEditor.CosTime;66;-1177.153,2126.005;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;67;-973.0419,2118.627;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;23;-680.6511,1979.996;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;6;-1090.797,1628.368;Inherit;False;Property;_hitPosition;hitPosition;0;0;Create;True;0;0;False;0;False;0,1,0;0,2.56,0.28;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;12;-1098.694,1472.103;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-726.6061,2310.742;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-469.5433,2001.736;Inherit;False;Property;_Anchodelborde;Ancho del borde;3;0;Create;True;0;0;False;0;False;0;0.1229;0;0.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;7;-765.0781,1563.176;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;14;-403.4508,1783.853;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-592.6216,2557.992;Inherit;False;Property;_NoiseStrength;Noise Strength;2;0;Create;True;0;0;False;0;False;0.5;0.547;0;1.3;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;3;-413.7965,2204.223;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;20;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;-159.8513,1991.482;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-246.705,1732.532;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0.13;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;18.21803,2279.759;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-6.229753,1906.059;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;26;342.3276,2048.353;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;18;322.8081,1749.324;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;56;618.4706,1747.958;Inherit;True;MascaraTexturaDanio;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;593.7226,2058.656;Inherit;True;MascaraTexturaPrincipal;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;806.5528,1052.612;Inherit;False;56;MascaraTexturaDanio;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;784.5528,1211.612;Inherit;False;55;MascaraTexturaPrincipal;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;57;1124.437,1092.046;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;51;1361.38,1376.021;Inherit;False;Property;_ColorBorder;Color Border;5;0;Create;True;0;0;False;0;False;0,0.2269145,0.9811321,0;1,0.4515924,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;60;1445.577,1010.79;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;64;1336.373,1649.703;Inherit;False;Property;_ColorDamage;Color Damage;6;0;Create;True;0;0;False;0;False;0.5566038,0.1654058,0.1654058,0;0.735849,0.0798327,0.09747409,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;61;1343.281,2083.653;Inherit;True;Property;_TexturaPrincipal;TexturaPrincipal;4;0;Create;True;0;0;False;0;False;-1;None;5d09bc2fef76d6a409f69241b2c5f1d4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;62;1378.157,1846.861;Inherit;True;56;MascaraTexturaDanio;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;1368.926,2333.289;Inherit;True;55;MascaraTexturaPrincipal;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;1694.289,2218.71;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;1713.211,1772.579;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;1857.749,1263.019;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;2089.695,1606.096;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1056.622,2000.417;Inherit;False;Property;_Impacto;Impacto;1;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2604.383,1659.135;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Custom/Disolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;2;bossShader=bossShader;Enemie=Enemie;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;67;0;66;4
WireConnection;23;0;67;0
WireConnection;7;0;12;0
WireConnection;7;1;6;0
WireConnection;14;1;23;0
WireConnection;3;0;4;0
WireConnection;65;0;19;0
WireConnection;9;0;7;0
WireConnection;9;1;14;0
WireConnection;21;0;3;0
WireConnection;21;1;16;0
WireConnection;25;0;9;0
WireConnection;25;1;65;0
WireConnection;26;0;21;0
WireConnection;26;1;25;0
WireConnection;18;0;9;0
WireConnection;18;1;21;0
WireConnection;56;0;18;0
WireConnection;55;0;26;0
WireConnection;57;0;58;0
WireConnection;57;1;59;0
WireConnection;60;0;57;0
WireConnection;27;0;61;0
WireConnection;27;1;54;0
WireConnection;46;0;64;0
WireConnection;46;1;62;0
WireConnection;52;0;60;0
WireConnection;52;1;51;0
WireConnection;48;0;52;0
WireConnection;48;1;46;0
WireConnection;48;2;27;0
WireConnection;0;0;48;0
ASEEND*/
//CHKSM=12CD49BF26500D4D1D49DA5730E31D65A2D4305D