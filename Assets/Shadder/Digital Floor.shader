// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Digital Floor"
{
	Properties
	{
		_HitWorldPos("HitWorldPos", Vector) = (-4.21,0.56,4.18,0)
		_Radio("Radio", Float) = 3
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_Tint("Tint", Color) = (0,0.8344827,1,0)
		_Brillo("Brillo", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform float4 _TextureSample0_ST;
		uniform float _Brillo;
		uniform float4 _Tint;
		uniform sampler2D _TextureSample0;
		uniform float3 _HitWorldPos;
		uniform float _Radio;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Albedo = ( float4(0.074,0.041,0.028,0) * tex2D( _TextureSample0, uv_TextureSample0 ) ).rgb;
			float3 ase_worldPos = i.worldPos;
			o.Emission = ( _Brillo * saturate( ( ( ( _Tint * tex2D( _TextureSample0, uv_TextureSample0 ).r ) * ( 1.0 - saturate( ( distance( _HitWorldPos , ase_worldPos ) / _Radio ) ) ) ) * 0.81 ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
766;112;602;556;753.575;335.0511;1.302418;False;False
Node;AmplifyShaderEditor.WorldPosInputsNode;14;-2050.006,385.6203;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;15;-2102.273,193.0019;Float;False;Property;_HitWorldPos;HitWorldPos;0;0;Create;True;0;0;False;0;-4.21,0.56,4.18;-110.0956,0,-80.14438;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;2;-1754.262,425.5458;Float;False;Property;_Radio;Radio;1;0;Create;True;0;0;False;0;3;4.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;1;-1759.098,255.5713;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;4;-1503.647,368.2381;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;7;-1513.883,-194.0767;Float;False;Property;_Tint;Tint;3;0;Create;True;0;0;False;0;0,0.8344827,1,0;0,1,0.659,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-1601.834,-4.771602;Float;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;d688558bccf5b944e870e01aa8429c49;d762d244a33ebda42b33faca35ac8c2b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;6;-1143.105,234.1699;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;8;-886.3087,179.8819;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-1018.166,7.157506;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-540.6042,-10.63445;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleNode;19;-322.1141,-15.99203;Float;False;0.81;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;16;-623.613,-561.6134;Float;False;Constant;_Color1;Color 1;4;0;Create;True;0;0;False;0;0.074,0.041,0.028,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-902.3849,-420.4947;Float;True;Property;_TextureSample1;Texture Sample 1;2;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Instance;3;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;20;-154.1288,-21.96509;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-266.4386,-140.1405;Float;False;Property;_Brillo;Brillo;4;0;Create;True;0;0;False;0;0;2.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-3.95796,-136.9303;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-232.0133,-351.8777;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;186.4395,-192.3819;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Digital Floor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;15;0
WireConnection;1;1;14;0
WireConnection;4;0;1;0
WireConnection;4;1;2;0
WireConnection;6;0;4;0
WireConnection;8;0;6;0
WireConnection;10;0;7;0
WireConnection;10;1;3;1
WireConnection;13;0;10;0
WireConnection;13;1;8;0
WireConnection;19;0;13;0
WireConnection;20;0;19;0
WireConnection;12;0;9;0
WireConnection;12;1;20;0
WireConnection;18;0;16;0
WireConnection;18;1;17;0
WireConnection;0;0;18;0
WireConnection;0;2;12;0
ASEEND*/
//CHKSM=80922879C3D8F7F7FC0B21BD4D98FC45C49FB09F