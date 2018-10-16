// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "New AmplifyShader"
{
	Properties
	{
		_enemigo1_DefaultMaterial_Emission("enemigo 1_DefaultMaterial_Emission", 2D) = "white" {}
		_enemigo1_DefaultMaterial_MetallicSmoothness("enemigo 1_DefaultMaterial_MetallicSmoothness", 2D) = "white" {}
		_enemigo1_DefaultMaterial_AO("enemigo 1_DefaultMaterial_AO", 2D) = "white" {}
		_enemigo1_DefaultMaterial_AlbedoTransparency("enemigo 1_DefaultMaterial_AlbedoTransparency", 2D) = "white" {}
		_fuerza("fuerza", Float) = 0
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
		};

		uniform sampler2D _enemigo1_DefaultMaterial_AlbedoTransparency;
		uniform float4 _enemigo1_DefaultMaterial_AlbedoTransparency_ST;
		uniform sampler2D _enemigo1_DefaultMaterial_Emission;
		uniform float4 _enemigo1_DefaultMaterial_Emission_ST;
		uniform float _fuerza;
		uniform sampler2D _enemigo1_DefaultMaterial_MetallicSmoothness;
		uniform float4 _enemigo1_DefaultMaterial_MetallicSmoothness_ST;
		uniform sampler2D _enemigo1_DefaultMaterial_AO;
		uniform float4 _enemigo1_DefaultMaterial_AO_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_enemigo1_DefaultMaterial_AlbedoTransparency = i.uv_texcoord * _enemigo1_DefaultMaterial_AlbedoTransparency_ST.xy + _enemigo1_DefaultMaterial_AlbedoTransparency_ST.zw;
			o.Albedo = tex2D( _enemigo1_DefaultMaterial_AlbedoTransparency, uv_enemigo1_DefaultMaterial_AlbedoTransparency ).rgb;
			float2 uv_enemigo1_DefaultMaterial_Emission = i.uv_texcoord * _enemigo1_DefaultMaterial_Emission_ST.xy + _enemigo1_DefaultMaterial_Emission_ST.zw;
			o.Emission = ( tex2D( _enemigo1_DefaultMaterial_Emission, uv_enemigo1_DefaultMaterial_Emission ) * _fuerza ).rgb;
			float2 uv_enemigo1_DefaultMaterial_MetallicSmoothness = i.uv_texcoord * _enemigo1_DefaultMaterial_MetallicSmoothness_ST.xy + _enemigo1_DefaultMaterial_MetallicSmoothness_ST.zw;
			o.Metallic = tex2D( _enemigo1_DefaultMaterial_MetallicSmoothness, uv_enemigo1_DefaultMaterial_MetallicSmoothness ).r;
			float2 uv_enemigo1_DefaultMaterial_AO = i.uv_texcoord * _enemigo1_DefaultMaterial_AO_ST.xy + _enemigo1_DefaultMaterial_AO_ST.zw;
			o.Occlusion = tex2D( _enemigo1_DefaultMaterial_AO, uv_enemigo1_DefaultMaterial_AO ).r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15301
661;150;602;571;1208.102;393.6325;2.445458;True;False
Node;AmplifyShaderEditor.SamplerNode;1;-1083.444,10.96632;Float;True;Property;_enemigo1_DefaultMaterial_Emission;enemigo 1_DefaultMaterial_Emission;0;0;Create;True;0;0;False;0;f8fe0c59129aca54199760bac498fcd5;f8fe0c59129aca54199760bac498fcd5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;6;-867.7111,238.0718;Float;False;Property;_fuerza;fuerza;4;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-606.1959,-250.3163;Float;True;Property;_enemigo1_DefaultMaterial_AlbedoTransparency;enemigo 1_DefaultMaterial_AlbedoTransparency;3;0;Create;True;0;0;False;0;092478f911ee00c45a66660f4a5ec239;092478f911ee00c45a66660f4a5ec239;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-616.6382,288.8841;Float;True;Property;_enemigo1_DefaultMaterial_MetallicSmoothness;enemigo 1_DefaultMaterial_MetallicSmoothness;1;0;Create;True;0;0;False;0;c2af3352aa127584da11627efc38e89a;c2af3352aa127584da11627efc38e89a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-584.1587,73.95012;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-188.6932,549.277;Float;True;Property;_enemigo1_DefaultMaterial_AO;enemigo 1_DefaultMaterial_AO;2;0;Create;True;0;0;False;0;0d544fa66e16a974c9f91c821ecd7df8;0d544fa66e16a974c9f91c821ecd7df8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;New AmplifyShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;0;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;0;False;-1;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;1;0
WireConnection;5;1;6;0
WireConnection;0;0;4;0
WireConnection;0;2;5;0
WireConnection;0;3;2;0
WireConnection;0;5;3;0
ASEEND*/
//CHKSM=2619498D72CDFF2641E56E556D186461360B4CF0