// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "MyShaders/Bounce"
{
	Properties
	{
		_HitWorldPos("HitWorldPos", Vector) = (-4.21,0.56,4.18,0)
		_Tesellation("Tesellation", Range( 0 , 10)) = 2
		_Radius("Radius", Float) = 3
		_Albedo("Albedo", 2D) = "white" {}
		_ImpactVector("ImpactVector", Vector) = (0,0,0,0)
		_Brick0207_NORM("Brick-0207_NORM", 2D) = "white" {}
		_10552normal("10552-normal", 2D) = "white" {}
		_grilla("grilla", 2D) = "white" {}
		_brillogrilla("brillo grilla", Float) = 0
		_nivel2_nivel2_MetallicSmoothness("nivel 2_nivel 2_MetallicSmoothness", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		struct appdata
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		uniform sampler2D _Brick0207_NORM;
		uniform float4 _Brick0207_NORM_ST;
		uniform float3 _HitWorldPos;
		uniform float _Radius;
		uniform sampler2D _10552normal;
		uniform float4 _10552normal_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _brillogrilla;
		uniform sampler2D _grilla;
		uniform float4 _grilla_ST;
		uniform sampler2D _nivel2_nivel2_MetallicSmoothness;
		uniform float4 _nivel2_nivel2_MetallicSmoothness_ST;
		uniform float3 _ImpactVector;
		uniform float _Tesellation;

		float4 tessFunction( appdata v0, appdata v1, appdata v2 )
		{
			float4 temp_cast_0 = (_Tesellation).xxxx;
			return temp_cast_0;
		}

		void vertexDataFunc( inout appdata v )
		{
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float temp_output_9_0 = ( 1.0 - saturate( ( distance( _HitWorldPos , ase_worldPos ) / _Radius ) ) );
			v.vertex.xyz += ( temp_output_9_0 * _ImpactVector );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Brick0207_NORM = i.uv_texcoord * _Brick0207_NORM_ST.xy + _Brick0207_NORM_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float temp_output_9_0 = ( 1.0 - saturate( ( distance( _HitWorldPos , ase_worldPos ) / _Radius ) ) );
			float2 uv_10552normal = i.uv_texcoord * _10552normal_ST.xy + _10552normal_ST.zw;
			o.Normal = BlendNormals( UnpackNormal( tex2D( _Brick0207_NORM, uv_Brick0207_NORM ) ) , UnpackScaleNormal( tex2D( _10552normal, uv_10552normal ) ,temp_output_9_0 ) );
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			o.Albedo = ( 0.55 * tex2D( _Albedo, uv_Albedo ) ).rgb;
			float2 uv_grilla = i.uv_texcoord * _grilla_ST.xy + _grilla_ST.zw;
			o.Emission = ( ( _brillogrilla * ( float4(0,0.8344827,1,0) * ( 1.0 - tex2D( _grilla, uv_grilla ) ) ) ) * temp_output_9_0 ).rgb;
			float2 uv_nivel2_nivel2_MetallicSmoothness = i.uv_texcoord * _nivel2_nivel2_MetallicSmoothness_ST.xy + _nivel2_nivel2_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _nivel2_nivel2_MetallicSmoothness, uv_nivel2_nivel2_MetallicSmoothness ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13701
541;143;689;571;208.8134;198.6309;1.509122;False;False
Node;AmplifyShaderEditor.WorldPosInputsNode;4;-1371.724,173.215;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.Vector3Node;2;-1423.991,-19.40328;Float;False;Property;_HitWorldPos;HitWorldPos;1;0;-4.21,0.56,4.18;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DistanceOpNode;3;-1100.394,100.2678;Float;False;2;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;7;-1113.758,240.3424;Float;False;Property;_Radius;Radius;2;0;3;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;20;-907.9391,-132.7035;Float;True;Property;_grilla;grilla;7;0;Assets/Art/Models/New new level/nivel 2/grilla.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;8;-873.2097,159.6743;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;21;-590.3873,-114.0625;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SaturateNode;10;-695.5516,188.3525;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;23;-855.1794,-349.3801;Float;False;Constant;_Color0;Color 0;8;0;0,0.8344827,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;9;-329.2695,192.7177;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;26;-565.7932,-423.9991;Float;False;Property;_brillogrilla;brillo grilla;8;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-359.4619,-148.1459;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.WireNode;28;-124.3307,-25.94713;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;17;58.88507,-626.0345;Float;False;Constant;_Float0;Float 0;7;0;0.55;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;14;-73.51524,3.006777;Float;True;Property;_Brick0207_NORM;Brick-0207_NORM;5;0;None;True;0;True;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-122.8115,-191.7037;Float;False;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.Vector3Node;12;-378.9391,426.6239;Float;False;Property;_ImpactVector;ImpactVector;4;0;0,0,0;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;15;-75.49018,195.4437;Float;True;Property;_10552normal;10552-normal;6;0;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;11;-108.2084,-541.2537;Float;True;Property;_Albedo;Albedo;3;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.BlendNormalsNode;16;317.3833,136.8196;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;219.0804,-114.0705;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;30;204.6861,395.9633;Float;True;Property;_nivel2_nivel2_MetallicSmoothness;nivel 2_nivel 2_MetallicSmoothness;9;0;Assets/Art/Models/New new level/nivel 2/nivel 2_nivel 2_MetallicSmoothness.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-52.05731,413.9977;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT3;0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;268.0622,-606.8369;Float;True;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;1;-98.50699,549.0986;Float;False;Property;_Tesellation;Tesellation;1;0;2;0;10;0;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;620.6793,9.88076;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;MyShaders/Bounce;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Opaque;0.5;True;True;0;False;Opaque;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;True;1;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;8;0;3;0
WireConnection;8;1;7;0
WireConnection;21;0;20;0
WireConnection;10;0;8;0
WireConnection;9;0;10;0
WireConnection;22;0;23;0
WireConnection;22;1;21;0
WireConnection;28;0;9;0
WireConnection;27;0;26;0
WireConnection;27;1;22;0
WireConnection;15;5;9;0
WireConnection;16;0;14;0
WireConnection;16;1;15;0
WireConnection;25;0;27;0
WireConnection;25;1;28;0
WireConnection;13;0;9;0
WireConnection;13;1;12;0
WireConnection;18;0;17;0
WireConnection;18;1;11;0
WireConnection;0;0;18;0
WireConnection;0;1;16;0
WireConnection;0;2;25;0
WireConnection;0;3;30;0
WireConnection;0;11;13;0
WireConnection;0;14;1;0
ASEEND*/
//CHKSM=A33F519D1CD138A7E1A0A9C3D881C8BC1418D24A