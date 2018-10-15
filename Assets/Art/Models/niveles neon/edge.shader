// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "edge"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		_Float1("Float 1", Float) = 0.73
		_fuerza("fuerza", Float) = 0
		_Float0("Float 0", Float) = 4.63
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _fuerza;
		uniform float _Float0;
		uniform float _Float1;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float lerpResult47 = lerp( ( pow( ( 1.0 - sin( ( i.uv_texcoord.x * 3.2 ) ) ) , _Float0 ) - _Float1 ) , ( pow( ( 1.0 - sin( ( i.uv_texcoord.y * 3.2 ) ) ) , _Float0 ) - _Float1 ) , 0.5);
			float edge58 = lerpResult47;
			float temp_output_52_0 = ( _fuerza * edge58 );
			o.Metallic = temp_output_52_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
698;304;602;571;698.186;337.6666;2.100755;False;False
Node;AmplifyShaderEditor.CommentaryNode;54;-3811.234,-1111.825;Float;False;1822.308;549.4905;Comment;14;23;24;25;51;26;38;36;43;46;37;42;45;44;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-3761.234,-933.2592;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-3434.555,-800.6302;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-3450.691,-1046.876;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;26;-3232.337,-1048.649;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;51;-3230.886,-780.6102;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;43;-3062.817,-756.4292;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-3124.943,-910.3585;Float;False;Property;_Float0;Float 0;7;0;Create;True;0;0;False;0;4.63;7.84;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;36;-3020.547,-1049.384;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2722.9,-851.7228;Float;False;Property;_Float1;Float 1;5;0;Create;True;0;0;False;0;0.73;-0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;37;-2764.529,-1021.169;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;42;-2773.831,-718.5491;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;44;-2503.968,-1061.825;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;45;-2502.723,-695.3342;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;-2253.926,-927.8878;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;-1892.325,-838.6749;Float;False;edge;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;-737.1799,529.7106;Float;False;58;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-718.0305,392.663;Float;False;Property;_fuerza;fuerza;6;0;Create;True;0;0;False;0;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-525.6565,397.2284;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;4;-807.8123,-47.21712;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1429.543,-71.84646;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;3,3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-392.5542,177.738;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;40;-705.3605,195.3965;Float;False;Constant;_Color1;Color 1;4;0;Create;True;0;0;False;0;0,1,1,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1125.117,-64.08854;Float;True;Property;_grilla;grilla;9;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;5;-1010.12,-287.431;Float;False;Property;_Color0;Color 0;10;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;edge;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;8;-1;-1;0;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;23;2
WireConnection;24;0;23;1
WireConnection;26;0;24;0
WireConnection;51;0;25;0
WireConnection;43;0;51;0
WireConnection;36;0;26;0
WireConnection;37;0;36;0
WireConnection;37;1;38;0
WireConnection;42;0;43;0
WireConnection;42;1;38;0
WireConnection;44;0;37;0
WireConnection;44;1;46;0
WireConnection;45;0;42;0
WireConnection;45;1;46;0
WireConnection;47;0;44;0
WireConnection;47;1;45;0
WireConnection;58;0;47;0
WireConnection;52;0;53;0
WireConnection;52;1;65;0
WireConnection;4;0;1;1
WireConnection;39;0;40;0
WireConnection;39;1;52;0
WireConnection;1;1;3;0
WireConnection;0;3;52;0
ASEEND*/
//CHKSM=A4E483C4CCD544EC2015C828CFB2D988EA7A3426