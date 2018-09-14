Shader "Hidden/TriplanarNode"
{
	Properties
	{
		_A ("_TopTex", 2D) = "white" {}
		_B ("_MidTex", 2D) = "white" {}
		_C ("_BotTex", 2D) = "white" {}
		_D ("_Tilling", 2D) = "white" {}
		_E ("_Falloff", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityStandardUtils.cginc"

			sampler2D _A;		
			sampler2D _B;
			sampler2D _C;
			sampler2D _D;
			sampler2D _E;
			float _IsNormal;
			float _IsSpherical;

			inline float3 TriplanarNormal( sampler2D topBumpMap, sampler2D midBumpMap, sampler2D botBumpMap, float3 worldPos, float3 worldNormal, float falloff, float tilling )
			{
				float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
				projNormal /= projNormal.x + projNormal.y + projNormal.z;
				float3 nsign = sign(worldNormal);
				float negProjNormalY = max( 0, projNormal.y * -nsign.y );
				projNormal.y = max( 0, projNormal.y * nsign.y );
				half3 xNorm; half3 yNorm; half3 yNormN; half3 zNorm;
				xNorm = UnpackNormal( tex2D( midBumpMap, tilling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
				yNorm = UnpackNormal( tex2D( topBumpMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
				yNormN = UnpackNormal( tex2D( botBumpMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
				zNorm = UnpackNormal( tex2D( midBumpMap, tilling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
				xNorm = half3( xNorm.xy * float2( nsign.x, 1.0 ) + worldNormal.zy, worldNormal.x );
				yNorm = half3( yNorm.xy * float2( nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y );
				yNormN = half3( yNormN.xy * float2( nsign.y, 1.0 ) + worldNormal.xz, worldNormal.y );
				zNorm = half3( zNorm.xy * float2( -nsign.z, 1.0 ) + worldNormal.xy, worldNormal.z );
				xNorm = xNorm.zyx;
				yNorm = yNorm.xzy;
				yNormN = yNormN.xzy;
				zNorm = zNorm.xyz;
				return normalize( xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z );
			}

			inline float4 TriplanarSampling( sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float tilling )
			{
				float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
				projNormal /= projNormal.x + projNormal.y + projNormal.z;
				float3 nsign = sign( worldNormal );
				float negProjNormalY = max( 0, projNormal.y * -nsign.y );
				projNormal.y = max( 0, projNormal.y * nsign.y );
				half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
				xNorm = ( tex2D( midTexMap, tilling * worldPos.zy * float2( nsign.x, 1.0 ) ) );
				yNorm = ( tex2D( topTexMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
				yNormN = ( tex2D( botTexMap, tilling * worldPos.xz * float2( nsign.y, 1.0 ) ) );
				zNorm = ( tex2D( midTexMap, tilling * worldPos.xy * float2( -nsign.z, 1.0 ) ) );
				return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
			}

			float4 frag( v2f_img i ) : SV_Target
			{
				float2 p = 2 * i.uv - 1;
				float r = sqrt( dot(p,p) );
				r = saturate( r );
				float2 uvs;
				float f = ( 1 - sqrt( 1 - r ) ) / r;
				uvs.x = p.x;
				uvs.y = p.y;
				float3 vertexPos = float3( uvs, ( f - 1 ) * 2 );
				float3 worldPos = mul(unity_ObjectToWorld, vertexPos).xyz;
				float3 normal = normalize(worldPos);
				float3 worldNormal = UnityObjectToWorldNormal(normal);

				float falloff = tex2D( _E, uvs ).r;
				float tilling = tex2D( _D, uvs ).r * 0.625;
				float4 triplanar = 1;

				if ( _IsNormal == 1 ) {
					float3 tangent = normalize(float3( (1-f)*2, p.y*0.01, p.x ));
					float3 worldTangent = UnityObjectToWorldDir(tangent);
					float tangentSign = -1;
					float3 worldBinormal = normalize( cross(worldNormal, worldTangent) * tangentSign);
					float3x3 worldToTangent = float3x3( worldTangent, worldBinormal, worldNormal );
					if ( _IsSpherical == 1 )
						triplanar.xyz = TriplanarNormal( _A, _A, _A, worldPos, worldNormal, falloff, tilling );
					else
						triplanar.xyz = TriplanarNormal( _A, _B, _C, worldPos, worldNormal, falloff, tilling );

					triplanar.xyz = mul( worldToTangent, triplanar.xyz );
				}
				else 
				{
					if ( _IsSpherical == 1 )
						triplanar = TriplanarSampling( _A, _A, _A, worldPos, worldNormal, falloff, tilling );
					else
						triplanar = TriplanarSampling( _A, _B, _C, worldPos, worldNormal, falloff, tilling );
				}

				return triplanar;
			}
			ENDCG
		}
	}
}
