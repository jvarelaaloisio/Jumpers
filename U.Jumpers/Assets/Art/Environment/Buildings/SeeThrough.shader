// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VarelaAloisio/SeeThrough"
{
	Properties
	{
		[HDR]_SeeThroughColor("SeeThroughColor", Color) = (0,0.5266704,4,0.3921569)
		_SeeThroughState("SeeThroughState", Float) = 0
		[NoScaleOffset][SingleLineTexture]_MainTex("Albedo", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_MetallicGlossMap("Metallic", 2D) = "white" {}
		[NoScaleOffset][Normal][SingleLineTexture]_BumpMap("Normal", 2D) = "white" {}
		[NoScaleOffset][SingleLineTexture]_Emission("Emission", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _BumpMap;
		uniform sampler2D _MainTex;
		uniform float _SeeThroughState;
		uniform sampler2D _Emission;
		uniform float4 _SeeThroughColor;
		uniform sampler2D _MetallicGlossMap;
		SamplerState sampler_MetallicGlossMap;
		SamplerState sampler_MainTex;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_BumpMap43 = i.uv_texcoord;
			float4 OUTPUT_Normal44 = tex2D( _BumpMap, uv_BumpMap43 );
			o.Normal = OUTPUT_Normal44.rgb;
			float2 uv_MainTex10 = i.uv_texcoord;
			float4 tex2DNode10 = tex2D( _MainTex, uv_MainTex10 );
			float4 Albedo13 = tex2DNode10;
			float SeeThroughState23 = _SeeThroughState;
			float4 lerpResult30 = lerp( Albedo13 , float4( 0,0,0,0 ) , SeeThroughState23);
			float4 OUTPUT_Albedo47 = lerpResult30;
			o.Albedo = OUTPUT_Albedo47.rgb;
			float2 uv_Emission12 = i.uv_texcoord;
			float4 Emission15 = tex2D( _Emission, uv_Emission12 );
			float4 SeeThroughColor2 = _SeeThroughColor;
			float4 lerpResult38 = lerp( Emission15 , SeeThroughColor2 , SeeThroughState23);
			float4 OUTPUT_Emission40 = lerpResult38;
			o.Emission = OUTPUT_Emission40.rgb;
			float2 uv_MetallicGlossMap11 = i.uv_texcoord;
			float4 tex2DNode11 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap11 );
			float OUTPUT_Metallic14 = tex2DNode11.r;
			o.Metallic = OUTPUT_Metallic14;
			float OUTPUT_Smoothness16 = tex2DNode11.a;
			o.Smoothness = OUTPUT_Smoothness16;
			float CONST_NUMBER_127 = 1.0;
			float SeeThroughAlpha3 = _SeeThroughColor.a;
			float lerpResult21 = lerp( CONST_NUMBER_127 , SeeThroughAlpha3 , SeeThroughState23);
			float AlbedoAlpha19 = tex2DNode10.a;
			float OUTPUT_Opacity32 = ( lerpResult21 * AlbedoAlpha19 );
			o.Alpha = OUTPUT_Opacity32;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18600
246;194;1072;752;2566.881;1344.099;2.567727;True;False
Node;AmplifyShaderEditor.CommentaryNode;35;-897,-960;Inherit;False;627.9644;155.5667;Constants;2;27;26;;0.3113208,0.3113208,0.3113208,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;4;-1736.305,-960;Inherit;False;812.8123;1126.371;Input;15;16;14;44;43;11;19;2;15;13;23;10;3;12;22;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-1536,-704;Inherit;False;Property;_SeeThroughState;SeeThroughState;1;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-768,-896;Inherit;False;Constant;_1;1;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-1536,-896;Inherit;False;Property;_SeeThroughColor;SeeThroughColor;0;1;[HDR];Create;True;0;0;False;0;False;0,0.5266704,4,0.3921569;0,0.5266704,4,0.3921569;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;34;-1735,176;Inherit;False;812.2015;316.522;Opacity;7;32;25;20;21;24;28;8;;0.495283,0.9231554,1,0.3921569;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-608,-896;Inherit;False;CONST_NUMBER_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-1568,-224;Inherit;True;Property;_Emission;Emission;5;2;[NoScaleOffset];[SingleLineTexture];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-1280,-704;Inherit;False;SeeThroughState;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;3;-1280,-800;Inherit;False;SeeThroughAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-1568,-608;Inherit;True;Property;_MainTex;Albedo;2;2;[NoScaleOffset];[SingleLineTexture];Create;False;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;8;-1712,320;Inherit;False;3;SeeThroughAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;24;-1712,400;Inherit;False;23;SeeThroughState;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;2;-1280,-896;Inherit;False;SeeThroughColor;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;15;-1280,-224;Inherit;False;Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;-1280,-512;Inherit;False;AlbedoAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;13;-1280,-608;Inherit;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;-1712,240;Inherit;False;27;CONST_NUMBER_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;42;-896,176;Inherit;False;639.3334;323.4185;Emission;5;40;38;9;37;39;;0.4009434,0.6760127,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;49;-894.1552,-108.4963;Inherit;False;631.6633;266.8586;Albedo;4;47;30;29;31;;1,0.6870223,0.427451,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;9;-880,320;Inherit;False;2;SeeThroughColor;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;20;-1456,416;Inherit;False;19;AlbedoAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;-880,416;Inherit;False;23;SeeThroughState;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;31;-881.6041,19.50377;Inherit;False;23;SeeThroughState;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;-832,224;Inherit;False;15;Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-833.6041,-60.49629;Inherit;False;13;Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;21;-1424,288;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1568,-416;Inherit;True;Property;_MetallicGlossMap;Metallic;3;2;[NoScaleOffset];[SingleLineTexture];Create;False;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;43;-1568,-32;Inherit;True;Property;_BumpMap;Normal;4;3;[NoScaleOffset];[Normal];[SingleLineTexture];Create;False;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;38;-624,304;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1248,288;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;30;-657.6041,-44.49628;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;32;-1120,288;Inherit;False;OUTPUT_Opacity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;40;-464,304;Inherit;False;OUTPUT_Emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;14;-1280,-400;Inherit;False;OUTPUT_Metallic;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;50;-891.1874,-768;Inherit;False;625.9103;600.2471;Output;7;0;48;33;36;46;41;45;;0.08490568,0.08490568,0.08490568,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-1280,-32;Inherit;False;OUTPUT_Normal;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-497.6042,-44.49628;Inherit;False;OUTPUT_Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;16;-1280,-320;Inherit;False;OUTPUT_Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;-832,-672;Inherit;False;47;OUTPUT_Albedo;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-832,-432;Inherit;False;14;OUTPUT_Metallic;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;41;-832,-512;Inherit;False;40;OUTPUT_Emission;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;33;-816,-272;Inherit;False;32;OUTPUT_Opacity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;45;-832,-592;Inherit;False;44;OUTPUT_Normal;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;36;-848,-352;Inherit;False;16;OUTPUT_Smoothness;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-512,-608;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;VarelaAloisio/SeeThrough;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;27;0;26;0
WireConnection;23;0;22;0
WireConnection;3;0;1;4
WireConnection;2;0;1;0
WireConnection;15;0;12;0
WireConnection;19;0;10;4
WireConnection;13;0;10;0
WireConnection;21;0;28;0
WireConnection;21;1;8;0
WireConnection;21;2;24;0
WireConnection;38;0;37;0
WireConnection;38;1;9;0
WireConnection;38;2;39;0
WireConnection;25;0;21;0
WireConnection;25;1;20;0
WireConnection;30;0;29;0
WireConnection;30;2;31;0
WireConnection;32;0;25;0
WireConnection;40;0;38;0
WireConnection;14;0;11;1
WireConnection;44;0;43;0
WireConnection;47;0;30;0
WireConnection;16;0;11;4
WireConnection;0;0;48;0
WireConnection;0;1;45;0
WireConnection;0;2;41;0
WireConnection;0;3;46;0
WireConnection;0;4;36;0
WireConnection;0;9;33;0
ASEEND*/
//CHKSM=45D5A258A185526EEAF769BFDC7A97EAF2AD635B