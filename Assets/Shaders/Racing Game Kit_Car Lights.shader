// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Racing Game Kit/Car Lights"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
    _Intensity ("Brightness", Range(0, 4)) = 0
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "Opaque"
    }
    LOD 200
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "ForwardBase"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 200
      ZClip Off
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
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      uniform float _Intensity;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 worldNormal_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_4;
          tmpvar_4[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_4[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_4[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_5;
          tmpvar_5 = normalize(mul(in_v.normal, tmpvar_4));
          worldNormal_1 = tmpvar_5;
          tmpvar_2 = worldNormal_1;
          float3 normal_6;
          normal_6 = worldNormal_1;
          float4 tmpvar_7;
          tmpvar_7.w = 1;
          tmpvar_7.xyz = float3(normal_6);
          float3 res_8;
          float3 x_9;
          x_9.x = dot(unity_SHAr, tmpvar_7);
          x_9.y = dot(unity_SHAg, tmpvar_7);
          x_9.z = dot(unity_SHAb, tmpvar_7);
          float3 x1_10;
          float4 tmpvar_11;
          tmpvar_11 = (normal_6.xyzz * normal_6.yzzx);
          x1_10.x = dot(unity_SHBr, tmpvar_11);
          x1_10.y = dot(unity_SHBg, tmpvar_11);
          x1_10.z = dot(unity_SHBb, tmpvar_11);
          res_8 = (x_9 + (x1_10 + (unity_SHC.xyz * ((normal_6.x * normal_6.x) - (normal_6.y * normal_6.y)))));
          float3 tmpvar_12;
          float _tmp_dvx_6 = max(((1.055 * pow(max(res_8, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_12 = float3(_tmp_dvx_6, _tmp_dvx_6, _tmp_dvx_6);
          res_8 = tmpvar_12;
          out_v.vertex = UnityObjectToClipPos(tmpvar_3);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD3 = max(float3(0, 0, 0), tmpvar_12);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float4 c_3;
          float3 tmpvar_4;
          float3 lightDir_5;
          float3 tmpvar_6;
          tmpvar_6 = _WorldSpaceLightPos0.xyz;
          lightDir_5 = tmpvar_6;
          tmpvar_4 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_7;
          float4 tmpvar_8;
          tmpvar_8 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          tmpvar_7 = (tmpvar_8.xyz * _Intensity);
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_5;
          float4 c_9;
          float4 c_10;
          float diff_11;
          float tmpvar_12;
          tmpvar_12 = max(0, dot(tmpvar_4, tmpvar_2));
          diff_11 = tmpvar_12;
          c_10.xyz = ((tmpvar_8.xyz * tmpvar_1) * diff_11);
          c_10.w = tmpvar_8.w;
          c_9.w = c_10.w;
          c_9.xyz = (c_10.xyz + (tmpvar_8.xyz * in_f.xlv_TEXCOORD3));
          c_3.xyz = (c_9.xyz + tmpvar_7);
          c_3.w = 1;
          out_f.color = c_3;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "ForwardAdd"
        "RenderType" = "Opaque"
      }
      LOD 200
      ZClip Off
      ZWrite Off
      Blend One One
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
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 worldNormal_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_4;
          tmpvar_4[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_4[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_4[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_5;
          tmpvar_5 = normalize(mul(in_v.normal, tmpvar_4));
          worldNormal_1 = tmpvar_5;
          tmpvar_2 = worldNormal_1;
          out_v.vertex = UnityObjectToClipPos(tmpvar_3);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float4 c_3;
          float3 tmpvar_4;
          float3 lightDir_5;
          float3 tmpvar_6;
          tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD2));
          lightDir_5 = tmpvar_6;
          tmpvar_4 = in_f.xlv_TEXCOORD1;
          float4 tmpvar_7;
          tmpvar_7 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          float4 tmpvar_8;
          tmpvar_8.w = 1;
          tmpvar_8.xyz = in_f.xlv_TEXCOORD2;
          float3 tmpvar_9;
          tmpvar_9 = mul(unity_WorldToLight, tmpvar_8).xyz;
          float tmpvar_10;
          tmpvar_10 = dot(tmpvar_9, tmpvar_9);
          float tmpvar_11;
          tmpvar_11 = tex2D(_LightTexture0, float2(tmpvar_10, tmpvar_10)).w;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_5;
          tmpvar_1 = (tmpvar_1 * tmpvar_11);
          float4 c_12;
          float4 c_13;
          float diff_14;
          float tmpvar_15;
          tmpvar_15 = max(0, dot(tmpvar_4, tmpvar_2));
          diff_14 = tmpvar_15;
          c_13.xyz = ((tmpvar_7.xyz * tmpvar_1) * diff_14);
          c_13.w = tmpvar_7.w;
          c_12.w = c_13.w;
          c_12.xyz = c_13.xyz;
          c_3.xyz = c_12.xyz;
          c_3.w = 1;
          out_f.color = c_3;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "LIGHTMODE" = "PrePassBase"
        "RenderType" = "Opaque"
      }
      LOD 200
      ZClip Off
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
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
      };
      
      struct OUT_Data_Vert
      {
          float3 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 worldNormal_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_4;
          tmpvar_4[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_4[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_4[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_5;
          tmpvar_5 = normalize(mul(in_v.normal, tmpvar_4));
          worldNormal_1 = tmpvar_5;
          tmpvar_2 = worldNormal_1;
          out_v.vertex = UnityObjectToClipPos(tmpvar_3);
          out_v.xlv_TEXCOORD0 = tmpvar_2;
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 res_1;
          float3 tmpvar_2;
          tmpvar_2 = in_f.xlv_TEXCOORD0;
          res_1.xyz = float3(((tmpvar_2 * 0.5) + 0.5));
          res_1.w = 0;
          out_f.color = res_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 4, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "LIGHTMODE" = "PrePassFinal"
        "RenderType" = "Opaque"
      }
      LOD 200
      ZClip Off
      ZWrite Off
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
      //uniform float4 _ProjectionParams;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      uniform float _Intensity;
      uniform sampler2D _LightBuffer;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = in_v.vertex.xyz;
          tmpvar_3 = UnityObjectToClipPos(tmpvar_4);
          float4 o_5;
          float4 tmpvar_6;
          tmpvar_6 = (tmpvar_3 * 0.5);
          float2 tmpvar_7;
          tmpvar_7.x = tmpvar_6.x;
          tmpvar_7.y = (tmpvar_6.y * _ProjectionParams.x);
          o_5.xy = (tmpvar_7 + tmpvar_6.w);
          o_5.zw = tmpvar_3.zw;
          tmpvar_1.zw = float2(0, 0);
          tmpvar_1.xy = float2(0, 0);
          float3x3 tmpvar_8;
          tmpvar_8[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_8[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_8[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float4 tmpvar_9;
          tmpvar_9.w = 1;
          tmpvar_9.xyz = float3(normalize(mul(in_v.normal, tmpvar_8)));
          float4 normal_10;
          normal_10 = tmpvar_9;
          float3 res_11;
          float3 x_12;
          x_12.x = dot(unity_SHAr, normal_10);
          x_12.y = dot(unity_SHAg, normal_10);
          x_12.z = dot(unity_SHAb, normal_10);
          float3 x1_13;
          float4 tmpvar_14;
          tmpvar_14 = (normal_10.xyzz * normal_10.yzzx);
          x1_13.x = dot(unity_SHBr, tmpvar_14);
          x1_13.y = dot(unity_SHBg, tmpvar_14);
          x1_13.z = dot(unity_SHBb, tmpvar_14);
          res_11 = (x_12 + (x1_13 + (unity_SHC.xyz * ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y)))));
          float3 tmpvar_15;
          float _tmp_dvx_7 = max(((1.055 * pow(max(res_11, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_15 = float3(_tmp_dvx_7, _tmp_dvx_7, _tmp_dvx_7);
          res_11 = tmpvar_15;
          tmpvar_2 = tmpvar_15;
          out_v.vertex = tmpvar_3;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD2 = o_5;
          out_v.xlv_TEXCOORD3 = tmpvar_1;
          out_v.xlv_TEXCOORD4 = tmpvar_2;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 c_2;
          float4 light_3;
          float3 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          tmpvar_4 = (tmpvar_5.xyz * _Intensity);
          float4 tmpvar_6;
          tmpvar_6 = tex2D(_LightBuffer, in_f.xlv_TEXCOORD2);
          light_3 = tmpvar_6;
          light_3 = (-log2(max(light_3, float4(0.001, 0.001, 0.001, 0.001))));
          light_3.xyz = (light_3.xyz + in_f.xlv_TEXCOORD4);
          float4 c_7;
          c_7.xyz = (tmpvar_5.xyz * light_3.xyz);
          c_7.w = tmpvar_5.w;
          c_2 = c_7;
          c_2.xyz = (c_2.xyz + tmpvar_4);
          c_2.w = 1;
          tmpvar_1 = c_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 5, name: DEFERRED
    {
      Name "DEFERRED"
      Tags
      { 
        "LIGHTMODE" = "Deferred"
        "RenderType" = "Opaque"
      }
      LOD 200
      ZClip Off
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
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      uniform float _Intensity;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
          float4 color1 :SV_Target1;
          float4 color2 :SV_Target2;
          float4 color3 :SV_Target3;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 worldNormal_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_5;
          tmpvar_5[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_5[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_5[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_6;
          tmpvar_6 = normalize(mul(in_v.normal, tmpvar_5));
          worldNormal_1 = tmpvar_6;
          tmpvar_2 = worldNormal_1;
          tmpvar_3.zw = float2(0, 0);
          tmpvar_3.xy = float2(0, 0);
          float3 normal_7;
          normal_7 = worldNormal_1;
          float4 tmpvar_8;
          tmpvar_8.w = 1;
          tmpvar_8.xyz = float3(normal_7);
          float3 res_9;
          float3 x_10;
          x_10.x = dot(unity_SHAr, tmpvar_8);
          x_10.y = dot(unity_SHAg, tmpvar_8);
          x_10.z = dot(unity_SHAb, tmpvar_8);
          float3 x1_11;
          float4 tmpvar_12;
          tmpvar_12 = (normal_7.xyzz * normal_7.yzzx);
          x1_11.x = dot(unity_SHBr, tmpvar_12);
          x1_11.y = dot(unity_SHBg, tmpvar_12);
          x1_11.z = dot(unity_SHBb, tmpvar_12);
          res_9 = (x_10 + (x1_11 + (unity_SHC.xyz * ((normal_7.x * normal_7.x) - (normal_7.y * normal_7.y)))));
          float3 tmpvar_13;
          float _tmp_dvx_8 = max(((1.055 * pow(max(res_9, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_13 = float3(_tmp_dvx_8, _tmp_dvx_8, _tmp_dvx_8);
          res_9 = tmpvar_13;
          out_v.vertex = UnityObjectToClipPos(tmpvar_4);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD3 = tmpvar_3;
          out_v.xlv_TEXCOORD4 = max(float3(0, 0, 0), tmpvar_13);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 outEmission_1;
          float3 tmpvar_2;
          tmpvar_2 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_3;
          float3 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          tmpvar_3 = tmpvar_5.xyz;
          tmpvar_4 = (tmpvar_5.xyz * _Intensity);
          float4 emission_6;
          float3 tmpvar_7;
          float3 tmpvar_8;
          tmpvar_7 = tmpvar_3;
          tmpvar_8 = tmpvar_2;
          float4 tmpvar_9;
          tmpvar_9.xyz = float3(tmpvar_7);
          tmpvar_9.w = 1;
          float4 tmpvar_10;
          tmpvar_10.xyz = float3(0, 0, 0);
          tmpvar_10.w = 0;
          float4 tmpvar_11;
          tmpvar_11.w = 1;
          tmpvar_11.xyz = float3(((tmpvar_8 * 0.5) + 0.5));
          float4 tmpvar_12;
          tmpvar_12.w = 1;
          tmpvar_12.xyz = float3(tmpvar_4);
          emission_6 = tmpvar_12;
          emission_6.xyz = (emission_6.xyz + (tmpvar_5.xyz * in_f.xlv_TEXCOORD4));
          outEmission_1.w = emission_6.w;
          outEmission_1.xyz = exp2((-emission_6.xyz));
          out_f.color = tmpvar_9;
          out_f.color1 = tmpvar_10;
          out_f.color2 = tmpvar_11;
          out_f.color3 = outEmission_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 6, name: META
    {
      Name "META"
      Tags
      { 
        "LIGHTMODE" = "Meta"
        "RenderType" = "Opaque"
      }
      LOD 200
      ZClip Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      uniform float _Intensity;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 vertex_1;
          vertex_1 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_1.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_2;
              if((in_v.vertex.z>0))
              {
                  tmpvar_2 = 0.0001;
              }
              else
              {
                  tmpvar_2 = 0;
              }
              vertex_1.z = tmpvar_2;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_1.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_3;
              if((vertex_1.z>0))
              {
                  tmpvar_3 = 0.0001;
              }
              else
              {
                  tmpvar_3 = 0;
              }
              vertex_1.z = tmpvar_3;
          }
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = vertex_1.xyz;
          out_v.vertex = UnityObjectToClipPos(tmpvar_4);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          float3 tmpvar_4;
          float3 tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          tmpvar_4 = tmpvar_6.xyz;
          tmpvar_5 = (tmpvar_6.xyz * _Intensity);
          tmpvar_2 = tmpvar_4;
          tmpvar_3 = tmpvar_5;
          float4 res_7;
          res_7 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_8;
              tmpvar_8.w = 1;
              tmpvar_8.xyz = float3(tmpvar_2);
              res_7.w = tmpvar_8.w;
              float3 tmpvar_9;
              float _tmp_dvx_9 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_9 = clamp(pow(tmpvar_2, float3(_tmp_dvx_9, _tmp_dvx_9, _tmp_dvx_9)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_7.xyz = float3(tmpvar_9);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_10;
              if(int(unity_UseLinearSpace))
              {
                  emission_10 = tmpvar_3;
              }
              else
              {
                  emission_10 = (tmpvar_3 * ((tmpvar_3 * ((tmpvar_3 * 0.305306) + 0.6821711)) + 0.01252288));
              }
              float4 tmpvar_11;
              float alpha_12;
              float3 tmpvar_13;
              tmpvar_13 = (emission_10 * 0.01030928);
              alpha_12 = (ceil((max(max(tmpvar_13.x, tmpvar_13.y), max(tmpvar_13.z, 0.02)) * 255)) / 255);
              float tmpvar_14;
              tmpvar_14 = max(alpha_12, 0.02);
              alpha_12 = tmpvar_14;
              float4 tmpvar_15;
              tmpvar_15.xyz = float3((tmpvar_13 / tmpvar_14));
              tmpvar_15.w = tmpvar_14;
              tmpvar_11 = tmpvar_15;
              res_7 = tmpvar_11;
          }
          tmpvar_1 = res_7;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Mobile/Emmisive"
}
