// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "edge"
{
	Properties
	{
		_Float1("Float 1", Float) = 0.73
		_fuerza("fuerza", Float) = 0
		_Float0("Float 0", Float) = 4.63
		_Color0("Color 0", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform float _fuerza;
		uniform float _Float0;
		uniform float _Float1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color0.rgb;
			float lerpResult47 = lerp( ( pow( ( 1.0 - sin( ( i.uv_texcoord.x * 3.2 ) ) ) , _Float0 ) - _Float1 ) , ( pow( ( 1.0 - sin( ( i.uv_texcoord.y * 3.2 ) ) ) , _Float0 ) - _Float1 ) , 0.5);
			float edge58 = lerpResult47;
			float temp_output_66_0 = saturate( ( _fuerza * edge58 ) );
			o.Metallic = temp_output_66_0;
			o.Smoothness = temp_output_66_0;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
610;135;602;571;417.1333;183.3005;1;False;False
Node;AmplifyShaderEditor.CommentaryNode;54;-2876.978,-380.9049;Float;False;1822.308;549.4905;Comment;14;23;24;25;51;26;38;36;43;46;37;42;45;44;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-2826.978,-202.3393;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-2516.435,-315.956;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-2500.299,-69.71039;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;3.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;26;-2298.081,-317.7291;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;51;-2296.63,-49.69042;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;43;-2128.561,-25.50939;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;36;-2086.291,-318.4641;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-2190.687,-179.4387;Float;False;Property;_Float0;Float 0;2;0;Create;True;0;0;False;0;4.63;22.17;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;37;-1830.271,-290.2491;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;42;-1839.573,12.37071;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1788.642,-120.8029;Float;False;Property;_Float1;Float 1;0;0;Create;True;0;0;False;0;0.73;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;45;-1568.464,35.58556;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;44;-1569.709,-330.9049;Float;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;47;-1319.667,-196.968;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;-824.9528,140.9221;Float;False;58;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-805.8035,3.87433;Float;False;Property;_fuerza;fuerza;1;0;Create;True;0;0;False;0;0;5.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;-958.0657,-107.7551;Float;False;edge;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-616.5688,8.439805;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-395.124,-205.8231;Float;False;Property;_Color0;Color 0;4;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;66;-440.9775,31.84733;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;edge;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;2;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;24;0;23;1
WireConnection;25;0;23;2
WireConnection;26;0;24;0
WireConnection;51;0;25;0
WireConnection;43;0;51;0
WireConnection;36;0;26;0
WireConnection;37;0;36;0
WireConnection;37;1;38;0
WireConnection;42;0;43;0
WireConnection;42;1;38;0
WireConnection;45;0;42;0
WireConnection;45;1;46;0
WireConnection;44;0;37;0
WireConnection;44;1;46;0
WireConnection;47;0;44;0
WireConnection;47;1;45;0
WireConnection;58;0;47;0
WireConnection;52;0;53;0
WireConnection;52;1;65;0
WireConnection;66;0;52;0
WireConnection;0;0;5;0
WireConnection;0;3;66;0
WireConnection;0;4;66;0
ASEEND*/
//CHKSM=8779F460B3DFE1A16568019E0138956BEF8D5101