// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

Shader "Decal/Cutout VertexLit"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "AlphaTest"
      "RenderType" = "TransparentCutout"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "Vertex"
        "QUEUE" = "AlphaTest"
        "RenderType" = "TransparentCutout"
      }
      AlphaToMask On
      ZClip Off
      Offset -1, -1
      Fog
      { 
        Mode  Off
      } 
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 unity_LightColor[8];
      //uniform float4 unity_LightPosition[8];
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 UNITY_MATRIX_IT_MV;
      //uniform float4 glstate_lightmodel_ambient;
      uniform float4 _Color;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float _Cutoff;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 lcolor_1;
          float3 eyeNormal_2;
          float4 color_3;
          float3x3 tmpvar_4;
          tmpvar_4[0] = conv_mxt4x4_0(UNITY_MATRIX_IT_MV).xyz;
          tmpvar_4[1] = conv_mxt4x4_1(UNITY_MATRIX_IT_MV).xyz;
          tmpvar_4[2] = conv_mxt4x4_2(UNITY_MATRIX_IT_MV).xyz;
          float3 tmpvar_5;
          tmpvar_5 = normalize(mul(tmpvar_4, in_v.normal));
          eyeNormal_2 = tmpvar_5;
          lcolor_1 = (_Color.xyz * glstate_lightmodel_ambient.xyz);
          float3 tmpvar_6;
          tmpvar_6 = unity_LightPosition[0].xyz;
          float3 dirToLight_7;
          dirToLight_7 = tmpvar_6;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_7), 0) * _Color.xyz) * unity_LightColor[0].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_8;
          tmpvar_8 = unity_LightPosition[1].xyz;
          float3 dirToLight_9;
          dirToLight_9 = tmpvar_8;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_9), 0) * _Color.xyz) * unity_LightColor[1].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_10;
          tmpvar_10 = unity_LightPosition[2].xyz;
          float3 dirToLight_11;
          dirToLight_11 = tmpvar_10;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_11), 0) * _Color.xyz) * unity_LightColor[2].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_12;
          tmpvar_12 = unity_LightPosition[3].xyz;
          float3 dirToLight_13;
          dirToLight_13 = tmpvar_12;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_13), 0) * _Color.xyz) * unity_LightColor[3].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_14;
          tmpvar_14 = unity_LightPosition[4].xyz;
          float3 dirToLight_15;
          dirToLight_15 = tmpvar_14;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_15), 0) * _Color.xyz) * unity_LightColor[4].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_16;
          tmpvar_16 = unity_LightPosition[5].xyz;
          float3 dirToLight_17;
          dirToLight_17 = tmpvar_16;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_17), 0) * _Color.xyz) * unity_LightColor[5].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_18;
          tmpvar_18 = unity_LightPosition[6].xyz;
          float3 dirToLight_19;
          dirToLight_19 = tmpvar_18;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_19), 0) * _Color.xyz) * unity_LightColor[6].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_20;
          tmpvar_20 = unity_LightPosition[7].xyz;
          float3 dirToLight_21;
          dirToLight_21 = tmpvar_20;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_21), 0) * _Color.xyz) * unity_LightColor[7].xyz) * 0.5), float3(1, 1, 1)));
          color_3.xyz = float3(lcolor_1);
          color_3.w = _Color.w;
          float4 tmpvar_22;
          float4 tmpvar_23;
          tmpvar_23 = clamp(color_3, 0, 1);
          tmpvar_22 = tmpvar_23;
          float4 tmpvar_24;
          tmpvar_24.w = 1;
          tmpvar_24.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_22;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = UnityObjectToClipPos(tmpvar_24);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          col_1.xyz = (tmpvar_2 * in_f.xlv_COLOR0).xyz;
          col_1.xyz = (col_1 * 2).xyz;
          col_1.w = (tmpvar_2.w * in_f.xlv_COLOR0.w);
          if((col_1.w<=_Cutoff))
          {
              discard;
          }
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "VertexLM"
        "QUEUE" = "AlphaTest"
        "RenderType" = "TransparentCutout"
      }
      AlphaToMask On
      ZClip Off
      Offset -1, -1
      Fog
      { 
        Mode  Off
      } 
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      // uniform float4 unity_LightmapST;
      uniform float4 _MainTex_ST;
      // uniform sampler2D unity_Lightmap;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      uniform float _Cutoff;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2 = clamp(in_v.color, 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.xlv_TEXCOORD0 = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = UnityObjectToClipPos(tmpvar_3);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          col_1 = (UNITY_SAMPLE_TEX2D(unity_Lightmap, in_f.xlv_TEXCOORD0) * _Color);
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD1);
          col_1.xyz = (tmpvar_2 * col_1).xyz;
          col_1.xyz = (col_1 * 2).xyz;
          col_1.w = (tmpvar_2.w * in_f.xlv_COLOR0.w);
          if((col_1.w<=_Cutoff))
          {
              discard;
          }
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "VertexLMRGBM"
        "QUEUE" = "AlphaTest"
        "RenderType" = "TransparentCutout"
      }
      AlphaToMask On
      ZClip Off
      Offset -1, -1
      Fog
      { 
        Mode  Off
      } 
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      // uniform float4 unity_LightmapST;
      uniform float4 unity_Lightmap_ST;
      uniform float4 _MainTex_ST;
      // uniform sampler2D unity_Lightmap;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      uniform float _Cutoff;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2 = clamp(in_v.color, 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.xlv_TEXCOORD0 = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.texcoord1.xy, unity_Lightmap);
          out_v.xlv_TEXCOORD2 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = UnityObjectToClipPos(tmpvar_3);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          float4 tmpvar_2;
          tmpvar_2 = UNITY_SAMPLE_TEX2D(unity_Lightmap, in_f.xlv_TEXCOORD0);
          col_1 = (tmpvar_2 * tmpvar_2.w);
          col_1 = (col_1 * 2);
          col_1 = (col_1 * _Color);
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_MainTex, in_f.xlv_TEXCOORD2);
          col_1.xyz = (tmpvar_3 * col_1).xyz;
          col_1.xyz = (col_1 * 4).xyz;
          col_1.w = (tmpvar_3.w * in_f.xlv_COLOR0.w);
          if((col_1.w<=_Cutoff))
          {
              discard;
          }
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 4, name: CASTER
    {
      Name "CASTER"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "SHADOWCASTER"
        "QUEUE" = "AlphaTest"
        "RenderType" = "TransparentCutout"
        "SHADOWSUPPORT" = "true"
      }
      ZClip Off
      ZTest Less
      Cull Off
      Offset 1, 1
      Fog
      { 
        Mode  Off
      } 
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile SHADOWS_DEPTH
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 unity_LightShadowBias;
      //uniform float4x4 UNITY_MATRIX_MVP;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float _Cutoff;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2.w = 1;
          tmpvar_2.xyz = in_v.vertex.xyz;
          tmpvar_1 = UnityObjectToClipPos(tmpvar_2);
          float4 clipPos_3;
          clipPos_3.xyw = tmpvar_1.xyw;
          clipPos_3.z = (tmpvar_1.z + clamp((unity_LightShadowBias.x / tmpvar_1.w), 0, 1));
          clipPos_3.z = lerp(clipPos_3.z, max(clipPos_3.z, (-tmpvar_1.w)), unity_LightShadowBias.y);
          out_v.vertex = clipPos_3;
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float x_1;
          x_1 = ((tex2D(_MainTex, in_f.xlv_TEXCOORD1).w * _Color.w) - _Cutoff);
          if((x_1<0))
          {
              discard;
          }
          out_f.color = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 5, name: SHADOWCOLLECTOR
    {
      Name "SHADOWCOLLECTOR"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "SHADOWCOLLECTOR"
        "QUEUE" = "AlphaTest"
        "RenderType" = "TransparentCutout"
      }
      ZClip Off
      ZTest Less
      Offset -1, -1
      Fog
      { 
        Mode  Off
      } 
      // m_ProgramMask = 6
      CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 because it uses wrong array syntax (type[size] name)
#pragma exclude_renderers d3d11
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4x4 unity_WorldToShadow;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 UNITY_MATRIX_MV;
      //uniform float4x4 unity_ObjectToWorld;
      uniform float4 _MainTex_ST;
      //uniform float4 _ProjectionParams;
      //uniform float4 _LightSplitsNear;
      //uniform float4 _LightSplitsFar;
      //uniform float4 _LightShadowData;
      uniform sampler2D _ShadowMapTexture;
      uniform sampler2D _MainTex;
      uniform float _Cutoff;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float3 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float2 xlv_TEXCOORD5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float2 xlv_TEXCOORD5 :TEXCOORD5;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.vertex;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = tmpvar_1.xyz;
          float4 tmpvar_4;
          tmpvar_4 = mul(unity_ObjectToWorld, in_v.vertex);
          tmpvar_2.xyz = tmpvar_4.xyz;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = tmpvar_1.xyz;
          tmpvar_2.w = (-UnityObjectToViewPos(tmpvar_5).z);
          out_v.vertex = UnityObjectToClipPos(tmpvar_3);
          out_v.xlv_TEXCOORD0 = mul(((float4[16])unity_WorldToShadow)[0], tmpvar_4).xyz;
          out_v.xlv_TEXCOORD1 = mul(((float4[16])unity_WorldToShadow)[1], tmpvar_4).xyz;
          out_v.xlv_TEXCOORD2 = mul(((float4[16])unity_WorldToShadow)[2], tmpvar_4).xyz;
          out_v.xlv_TEXCOORD3 = mul(((float4[16])unity_WorldToShadow)[3], tmpvar_4).xyz;
          out_v.xlv_TEXCOORD4 = tmpvar_2;
          out_v.xlv_TEXCOORD5 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 res_2;
          float shadow_3;
          float4 zFar_4;
          float4 zNear_5;
          float x_6;
          x_6 = ((tex2D(_MainTex, in_f.xlv_TEXCOORD5).w * _Color.w) - _Cutoff);
          if((x_6<0))
          {
              discard;
          }
          float4 tmpvar_7;
          tmpvar_7 = bool4(in_f.xlv_TEXCOORD4.wwww >= _LightSplitsNear);
          float4 tmpvar_8;
          tmpvar_8 = float4(tmpvar_7);
          zNear_5 = tmpvar_8;
          float4 tmpvar_9;
          tmpvar_9 = bool4(in_f.xlv_TEXCOORD4.wwww < _LightSplitsFar);
          float4 tmpvar_10;
          tmpvar_10 = float4(tmpvar_9);
          zFar_4 = tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11 = (zNear_5 * zFar_4);
          float tmpvar_12;
          tmpvar_12 = clamp(((in_f.xlv_TEXCOORD4.w * _LightShadowData.z) + _LightShadowData.w), 0, 1);
          float4 tmpvar_13;
          tmpvar_13.w = 1;
          tmpvar_13.xyz = ((((in_f.xlv_TEXCOORD0 * tmpvar_11.x) + (in_f.xlv_TEXCOORD1 * tmpvar_11.y)) + (in_f.xlv_TEXCOORD2 * tmpvar_11.z)) + (in_f.xlv_TEXCOORD3 * tmpvar_11.w));
          float4 tmpvar_14;
          tmpvar_14 = tex2D(_ShadowMapTexture, tmpvar_13.xy);
          float tmpvar_15;
          if((tmpvar_14.x<tmpvar_13.z))
          {
              tmpvar_15 = 0;
          }
          else
          {
              tmpvar_15 = 1;
          }
          shadow_3 = (_LightShadowData.x + (tmpvar_15 * (1 - _LightShadowData.x)));
          res_2.x = clamp((shadow_3 + tmpvar_12), 0, 1);
          res_2.y = 1;
          float2 enc_16;
          float2 tmpvar_17;
          tmpvar_17 = frac((float2(1, 255) * (1 - (in_f.xlv_TEXCOORD4.w * _ProjectionParams.w))));
          enc_16.y = tmpvar_17.y;
          enc_16.x = (tmpvar_17.x - (tmpvar_17.y * 0.003921569));
          res_2.zw = enc_16;
          tmpvar_1 = res_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "RenderType" = "TransparentCutout"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "Always"
        "RenderType" = "TransparentCutout"
      }
      AlphaToMask On
      ZClip Off
      Offset -1, -1
      Fog
      { 
        Mode  Off
      } 
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 unity_LightColor[8];
      //uniform float4 unity_LightPosition[8];
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 UNITY_MATRIX_IT_MV;
      //uniform float4 glstate_lightmodel_ambient;
      uniform float4 _Color;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float _Cutoff;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 lcolor_1;
          float3 eyeNormal_2;
          float4 color_3;
          float3x3 tmpvar_4;
          tmpvar_4[0] = conv_mxt4x4_0(UNITY_MATRIX_IT_MV).xyz;
          tmpvar_4[1] = conv_mxt4x4_1(UNITY_MATRIX_IT_MV).xyz;
          tmpvar_4[2] = conv_mxt4x4_2(UNITY_MATRIX_IT_MV).xyz;
          float3 tmpvar_5;
          tmpvar_5 = normalize(mul(tmpvar_4, in_v.normal));
          eyeNormal_2 = tmpvar_5;
          lcolor_1 = (_Color.xyz * glstate_lightmodel_ambient.xyz);
          float3 tmpvar_6;
          tmpvar_6 = unity_LightPosition[0].xyz;
          float3 dirToLight_7;
          dirToLight_7 = tmpvar_6;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_7), 0) * _Color.xyz) * unity_LightColor[0].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_8;
          tmpvar_8 = unity_LightPosition[1].xyz;
          float3 dirToLight_9;
          dirToLight_9 = tmpvar_8;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_9), 0) * _Color.xyz) * unity_LightColor[1].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_10;
          tmpvar_10 = unity_LightPosition[2].xyz;
          float3 dirToLight_11;
          dirToLight_11 = tmpvar_10;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_11), 0) * _Color.xyz) * unity_LightColor[2].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_12;
          tmpvar_12 = unity_LightPosition[3].xyz;
          float3 dirToLight_13;
          dirToLight_13 = tmpvar_12;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_13), 0) * _Color.xyz) * unity_LightColor[3].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_14;
          tmpvar_14 = unity_LightPosition[4].xyz;
          float3 dirToLight_15;
          dirToLight_15 = tmpvar_14;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_15), 0) * _Color.xyz) * unity_LightColor[4].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_16;
          tmpvar_16 = unity_LightPosition[5].xyz;
          float3 dirToLight_17;
          dirToLight_17 = tmpvar_16;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_17), 0) * _Color.xyz) * unity_LightColor[5].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_18;
          tmpvar_18 = unity_LightPosition[6].xyz;
          float3 dirToLight_19;
          dirToLight_19 = tmpvar_18;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_19), 0) * _Color.xyz) * unity_LightColor[6].xyz) * 0.5), float3(1, 1, 1)));
          float3 tmpvar_20;
          tmpvar_20 = unity_LightPosition[7].xyz;
          float3 dirToLight_21;
          dirToLight_21 = tmpvar_20;
          lcolor_1 = (lcolor_1 + min((((max(dot(eyeNormal_2, dirToLight_21), 0) * _Color.xyz) * unity_LightColor[7].xyz) * 0.5), float3(1, 1, 1)));
          color_3.xyz = float3(lcolor_1);
          color_3.w = _Color.w;
          float4 tmpvar_22;
          float4 tmpvar_23;
          tmpvar_23 = clamp(color_3, 0, 1);
          tmpvar_22 = tmpvar_23;
          float4 tmpvar_24;
          tmpvar_24.w = 1;
          tmpvar_24.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_22;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = UnityObjectToClipPos(tmpvar_24);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          col_1.xyz = (tmpvar_2 * in_f.xlv_COLOR0).xyz;
          col_1.xyz = (col_1 * 2).xyz;
          col_1.w = (tmpvar_2.w * in_f.xlv_COLOR0.w);
          if((col_1.w<=_Cutoff))
          {
              discard;
          }
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
