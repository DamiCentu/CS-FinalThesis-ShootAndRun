// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New AmplifyShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_duration("duration", Range( -2 , 0)) = -1.221386
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			fixed filler;
		};

		uniform float _duration;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = float4(0.1995026,0.6617647,0.298331,0).rgb;
			o.Emission = ( float4(1,0,0,0) * saturate( exp( ( _Time.y / _duration ) ) ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=10001
57;171;1200;926;2072.904;-266.7987;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-1696.005,722.2988;Float;False;Property;_duration;duration;1;0;-1.221386;-2;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;11;-1656.604,641.899;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;15;-1350.904,595.7987;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ExpOpNode;6;-1140.101,579.798;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SaturateNode;9;-580.9011,565.3979;Float;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;2;-970.3995,227.6004;Float;False;Constant;_Color0;Color 0;1;0;1,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;13;-1083.298,-104.8004;Float;False;Constant;_Color1;Color 1;1;0;0.1995026,0.6617647,0.298331,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-413.8026,296.1005;Float;True;2;0;COLOR;0.0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;Standard;New AmplifyShader;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;Relative;0;;-1;-1;-1;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;13;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;11;0
WireConnection;15;1;3;0
WireConnection;6;0;15;0
WireConnection;9;0;6;0
WireConnection;4;0;2;0
WireConnection;4;1;9;0
WireConnection;0;0;13;0
WireConnection;0;2;4;0
ASEEND*/
//CHKSM=3ECD487F760C341CC9C72E5BBD78CAB44D295693