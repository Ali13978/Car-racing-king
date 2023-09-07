// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RainDrop/Internal/RainDistortion (Deffered)"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _Strength ("Distortion Strength", Range(0, 150)) = 50
    _Relief ("Relief Value", Range(0, 2)) = 1.5
    _Distortion ("Normalmap", 2D) = "bump" {}
    _ReliefTex ("Relief", 2D) = "black" {}
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      LOD 52262048
      ZClip Off
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Fog
      { 
        Mode  Off
      } 
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "ForwardBase"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
        "SHADOWSUPPORT" = "true"
      }
      LOD 100
      ZClip Off
      ZTest Always
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _ProjectionParams;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_WorldToObject;
      uniform float4 _Distortion_ST;
      uniform float4 _ReliefTex_ST;
      uniform float _Strength;
      uniform float _Relief;
      uniform float4 _Color;
      uniform float4 _GrabTexture_TexelSize;
      uniform sampler2D _Distortion;
      uniform sampler2D _ReliefTex;
      uniform sampler2D _GrabTexture;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 texcoord :TEXCOORD0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float4 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float3 texcoord3 :TEXCOORD3;
          float3 texcoord4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float4 texcoord2 :TEXCOORD2;
          float3 texcoord3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float4 u_xlat16_0;
      float4 u_xlat1;
      float4 u_xlat2;
      float3 u_xlat16_3;
      float3 u_xlat16_4;
      float u_xlat16;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          u_xlat0 = UnityObjectToClipPos(float4(in_v.vertex.xyz,1.0));
          out_v.vertex = u_xlat0;
          out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _Distortion);
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _ReliefTex);
          u_xlat1.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat1.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat1.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat1.xyz = normalize(u_xlat1.xyz);
          out_v.texcoord1.xyz = u_xlat1.xyz;
          u_xlat0.y = (u_xlat0.y * _ProjectionParams.x);
          u_xlat2.xzw = (u_xlat0.xwy * float3(0.5, 0.5, 0.5));
          out_v.texcoord2.zw = u_xlat0.zw;
          out_v.texcoord2.xy = (u_xlat2.zz + u_xlat2.xw);
          out_v.texcoord3.xyz = in_v.color.xyz;
          u_xlat16_3.x = (u_xlat1.y * u_xlat1.y);
          u_xlat16_3.x = ((u_xlat1.x * u_xlat1.x) + (-u_xlat16_3.x));
          u_xlat16_0 = (u_xlat1.yzzx * u_xlat1.xyzz);
          u_xlat16_4.x = dot(unity_SHBr, u_xlat16_0);
          u_xlat16_4.y = dot(unity_SHBg, u_xlat16_0);
          u_xlat16_4.z = dot(unity_SHBb, u_xlat16_0);
          u_xlat16_3.xyz = ((unity_SHC.xyz * u_xlat16_3.xxx) + u_xlat16_4.xyz);
          u_xlat1.w = 1;
          u_xlat16_4.x = dot(unity_SHAr, u_xlat1);
          u_xlat16_4.y = dot(unity_SHAg, u_xlat1);
          u_xlat16_4.z = dot(unity_SHAb, u_xlat1);
          u_xlat16_3.xyz = (u_xlat16_3.xyz + u_xlat16_4.xyz);
          u_xlat16_3.xyz = max(u_xlat16_3.xyz, float3(0, 0, 0));
          u_xlat1.xyz = log2(u_xlat16_3.xyz);
          u_xlat1.xyz = (u_xlat1.xyz * float3(0.416666657, 0.416666657, 0.416666657));
          u_xlat1.xyz = exp2(u_xlat1.xyz);
          u_xlat1.xyz = ((u_xlat1.xyz * float3(1.05499995, 1.05499995, 1.05499995)) + float3(-0.0549999997, (-0.0549999997), (-0.0549999997)));
          u_xlat1.xyz = max(u_xlat1.xyz, float3(0, 0, 0));
          out_v.texcoord4.xyz = u_xlat1.xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat10_0;
      float2 u_xlat1_d;
      float3 u_xlat10_1;
      float u_xlat10_2;
      float u_xlat9;
      float u_xlat10_9;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_ReliefTex, in_f.texcoord.zw);
          u_xlat1_d.xy = ((u_xlat10_0.xy * (-float2(_Strength, _Strength))) + in_f.texcoord3.xy);
          u_xlat0_d = (u_xlat10_0 * _Color);
          u_xlat1_d.xy = ((u_xlat1_d.xy * _GrabTexture_TexelSize.xy) + in_f.texcoord2.xy);
          u_xlat1_d.xy = (u_xlat1_d.xy / in_f.texcoord2.ww);
          u_xlat10_1.xyz = tex2D(_GrabTexture, u_xlat1_d.xy).xyz;
          u_xlat0_d.xyz = ((u_xlat0_d.www * u_xlat0_d.xyz) + u_xlat10_1.xyz);
          u_xlat10_9 = tex2D(_Distortion, in_f.texcoord.xy).x;
          u_xlat10_2 = ((u_xlat10_9 * 2) + (-1));
          u_xlat9 = (((-u_xlat10_2) * _Relief) + 1);
          u_xlat0_d.xyz = (float3(u_xlat9, u_xlat9, u_xlat9) * u_xlat0_d.xyz);
          out_f.color.xyz = u_xlat0_d.xyz;
          out_f.color.w = 1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "ForwardAdd"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZTest Always
      ZWrite Off
      Blend One One
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float3 normal :NORMAL0;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float3 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 texcoord :TEXCOORD0;
          float3 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 vertex :Position;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      float u_xlat3;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          out_v.vertex = UnityObjectToClipPos(float4(in_v.vertex.xyz,1.0));
          u_xlat0.x = dot(in_v.normal.xyz, conv_mxt4x4_0(unity_WorldToObject).xyz);
          u_xlat0.y = dot(in_v.normal.xyz, conv_mxt4x4_1(unity_WorldToObject).xyz);
          u_xlat0.z = dot(in_v.normal.xyz, conv_mxt4x4_2(unity_WorldToObject).xyz);
          u_xlat0.xyz = normalize(u_xlat0.xyz);
          out_v.texcoord.xyz = u_xlat0.xyz;
          u_xlat0.xyz = (in_v.vertex.yyy * conv_mxt4x4_1(unity_ObjectToWorld).xyz);
          u_xlat0.xyz = ((conv_mxt4x4_0(unity_ObjectToWorld).xyz * in_v.vertex.xxx) + u_xlat0.xyz);
          u_xlat0.xyz = ((conv_mxt4x4_2(unity_ObjectToWorld).xyz * in_v.vertex.zzz) + u_xlat0.xyz);
          out_v.texcoord1.xyz = ((conv_mxt4x4_3(unity_ObjectToWorld).xyz * in_v.vertex.www) + u_xlat0.xyz);
          out_v.texcoord2.xyz = in_v.color.xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          out_f.color = float4(0, 0, 0, 1);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 4, name: META
    {
      Name "META"
      Tags
      { 
        "LIGHTMODE" = "Meta"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZTest Always
      Cull Off
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _ProjectionParams;
      //uniform float4x4 UNITY_MATRIX_MVP;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _Distortion_ST;
      uniform float4 _ReliefTex_ST;
      uniform float _Strength;
      uniform float _Relief;
      uniform float4 _Color;
      uniform float4 _GrabTexture_TexelSize;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      uniform sampler2D _Distortion;
      uniform sampler2D _ReliefTex;
      uniform sampler2D _GrabTexture;
      struct appdata_t
      {
          float4 vertex :POSITION0;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
          float4 color :COLOR0;
      };
      
      struct OUT_Data_Vert
      {
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float3 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float4 u_xlat0;
      int u_xlatb0;
      float4 u_xlat1;
      int u_xlatb6;
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          #ifdef UNITY_ADRENO_ES3
          u_xlatb0 = (0<in_v.vertex.z);
          #else
          u_xlatb0 = (0<in_v.vertex.z);
          #endif
          u_xlat0.z = (u_xlatb0)?(9.99999975E-05):(float(0));
          u_xlat0.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
          u_xlat0.xyz = (unity_MetaVertexControl.x)?(u_xlat0.xyz):(in_v.vertex.xyz);
          #ifdef UNITY_ADRENO_ES3
          u_xlatb6 = (0<u_xlat0.z);
          #else
          u_xlatb6 = (0<u_xlat0.z);
          #endif
          u_xlat1.z = (u_xlatb6)?(9.99999975E-05):(float(0));
          u_xlat1.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
          u_xlat0.xyz = (unity_MetaVertexControl.y)?(u_xlat1.xyz):(u_xlat0.xyz);
          u_xlat0 = UnityObjectToClipPos(float4(u_xlat0.xyz,1.0));
          out_v.vertex = u_xlat0;
          out_v.texcoord.xy = TRANSFORM_TEX(in_v.texcoord.xy, _Distortion);
          out_v.texcoord.zw = TRANSFORM_TEX(in_v.texcoord.xy, _ReliefTex);
          u_xlat0.y = (u_xlat0.y * _ProjectionParams.x);
          u_xlat1.xzw = (u_xlat0.xwy * float3(0.5, 0.5, 0.5));
          out_v.texcoord1.zw = u_xlat0.zw;
          out_v.texcoord1.xy = (u_xlat1.zz + u_xlat1.xw);
          out_v.texcoord2.xyz = in_v.color.xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 u_xlat0_d;
      float4 u_xlat16_0;
      float4 u_xlat10_0;
      float4 u_xlat1_d;
      float u_xlat16_1;
      float3 u_xlat10_1;
      float u_xlat10_2;
      float3 u_xlat16_3;
      float u_xlat12;
      float u_xlat16_12;
      float u_xlat10_12;
      int u_xlatb12;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          u_xlat10_0 = tex2D(_ReliefTex, in_f.texcoord.zw);
          u_xlat1_d.xy = ((u_xlat10_0.xy * (-float2(_Strength, _Strength))) + in_f.texcoord2.xy);
          u_xlat0_d = (u_xlat10_0 * _Color);
          u_xlat1_d.xy = ((u_xlat1_d.xy * _GrabTexture_TexelSize.xy) + in_f.texcoord1.xy);
          u_xlat1_d.xy = (u_xlat1_d.xy / in_f.texcoord1.ww);
          u_xlat10_1.xyz = tex2D(_GrabTexture, u_xlat1_d.xy).xyz;
          u_xlat0_d.xyz = ((u_xlat0_d.www * u_xlat0_d.xyz) + u_xlat10_1.xyz);
          u_xlat10_12 = tex2D(_Distortion, in_f.texcoord.xy).x;
          u_xlat10_2 = ((u_xlat10_12 * 2) + (-1));
          u_xlat12 = (((-u_xlat10_2) * _Relief) + 1);
          u_xlat0_d.xyz = (float3(u_xlat12, u_xlat12, u_xlat12) * u_xlat0_d.xyz);
          u_xlat1_d.xyz = ((u_xlat0_d.xyz * float3(0.305306017, 0.305306017, 0.305306017)) + float3(0.682171106, 0.682171106, 0.682171106));
          u_xlat1_d.xyz = ((u_xlat0_d.xyz * u_xlat1_d.xyz) + float3(0.0125228781, 0.0125228781, 0.0125228781));
          u_xlat1_d.xyz = (u_xlat0_d.xyz * u_xlat1_d.xyz);
          #ifdef UNITY_ADRENO_ES3
          u_xlatb12 = (float4(0, 0, 0, 0).x != float4(unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace).x && float4(0, 0, 0, 0).y != float4(unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace).y && float4(0, 0, 0, 0).z != float4(unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace).z && float4(0, 0, 0, 0).w != float4(unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace).w);
          #else
          u_xlatb12 = (float4(0, 0, 0, 0).x != float4(unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace).x && float4(0, 0, 0, 0).y != float4(unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace).y && float4(0, 0, 0, 0).z != float4(unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace).z && float4(0, 0, 0, 0).w != float4(unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace, unity_UseLinearSpace).w);
          #endif
          u_xlat16_3.xyz = (int(u_xlatb12))?(u_xlat0_d.xyz):(u_xlat1_d.xyz);
          u_xlat16_0.xyz = (u_xlat16_3.xyz * float3(0.010309278, 0.010309278, 0.010309278));
          u_xlat16_12 = max(u_xlat16_0.y, u_xlat16_0.x);
          u_xlat16_1 = max(u_xlat16_0.z, 0.0199999996);
          u_xlat16_12 = max(u_xlat16_12, u_xlat16_1);
          u_xlat12 = (u_xlat16_12 * 255);
          u_xlat12 = ceil(u_xlat12);
          u_xlat1_d.w = (u_xlat12 * 0.00392156886);
          u_xlat1_d.xyz = (u_xlat16_0.xyz / u_xlat1_d.www);
          u_xlat0_d.x = min(unity_MaxOutputValue, 0);
          u_xlat16_0.xyz = (unity_MetaFragmentControl.x)?(u_xlat0_d.xxx):(float3(0, 0, 0));
          u_xlat16_0.w = (unity_MetaFragmentControl.x)?(1):(0);
          u_xlat16_0 = (unity_MetaFragmentControl.y)?(u_xlat1_d):(u_xlat16_0);
          out_f.color = u_xlat16_0;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Transparent/Diffuse"
}
