// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Racing Game Kit/Car Paint"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
    _Shininess ("Shininess", Range(0.01, 1)) = 0.078125
    _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
    _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
    _Cube ("Reflection Cubemap", Cube) = "_Skybox" {}
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "Opaque"
    }
    LOD 300
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "ForwardBase"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 300
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
      //uniform float3 _WorldSpaceCameraPos;
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
      uniform float4 _SpecColor;
      uniform sampler2D _MainTex;
      uniform samplerCUBE _Cube;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float _Shininess;
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
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
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
          float3 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = in_v.vertex.xyz;
          float3 tmpvar_5;
          tmpvar_5 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_6;
          tmpvar_6[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_6[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_6[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_7;
          tmpvar_7 = normalize(mul(in_v.normal, tmpvar_6));
          worldNormal_1 = tmpvar_7;
          tmpvar_3 = worldNormal_1;
          float3 tmpvar_8;
          float3 I_9;
          I_9 = (tmpvar_5 - _WorldSpaceCameraPos);
          tmpvar_8 = (I_9 - (2 * (dot(worldNormal_1, I_9) * worldNormal_1)));
          tmpvar_2 = tmpvar_8;
          float3 normal_10;
          normal_10 = worldNormal_1;
          float4 tmpvar_11;
          tmpvar_11.w = 1;
          tmpvar_11.xyz = float3(normal_10);
          float3 res_12;
          float3 x_13;
          x_13.x = dot(unity_SHAr, tmpvar_11);
          x_13.y = dot(unity_SHAg, tmpvar_11);
          x_13.z = dot(unity_SHAb, tmpvar_11);
          float3 x1_14;
          float4 tmpvar_15;
          tmpvar_15 = (normal_10.xyzz * normal_10.yzzx);
          x1_14.x = dot(unity_SHBr, tmpvar_15);
          x1_14.y = dot(unity_SHBg, tmpvar_15);
          x1_14.z = dot(unity_SHBb, tmpvar_15);
          res_12 = (x_13 + (x1_14 + (unity_SHC.xyz * ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y)))));
          float3 tmpvar_16;
          float _tmp_dvx_10 = max(((1.055 * pow(max(res_12, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_16 = float3(_tmp_dvx_10, _tmp_dvx_10, _tmp_dvx_10);
          res_12 = tmpvar_16;
          out_v.vertex = UnityObjectToClipPos(tmpvar_4);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = tmpvar_3;
          out_v.xlv_TEXCOORD3 = tmpvar_5;
          out_v.xlv_TEXCOORD4 = max(float3(0, 0, 0), tmpvar_16);
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
          float3 worldViewDir_5;
          float3 lightDir_6;
          float3 tmpvar_7;
          float3 tmpvar_8;
          tmpvar_8 = _WorldSpaceLightPos0.xyz;
          lightDir_6 = tmpvar_8;
          float3 tmpvar_9;
          tmpvar_9 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD3));
          worldViewDir_5 = tmpvar_9;
          tmpvar_7 = in_f.xlv_TEXCOORD1;
          tmpvar_4 = in_f.xlv_TEXCOORD2;
          float3 tmpvar_10;
          float3 tmpvar_11;
          float tmpvar_12;
          float tmpvar_13;
          float4 reflcol_14;
          float4 c_15;
          float4 tex_16;
          float4 tmpvar_17;
          tmpvar_17 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_16 = tmpvar_17;
          float4 tmpvar_18;
          tmpvar_18 = (tex_16 * _Color);
          c_15 = tmpvar_18;
          tmpvar_10 = c_15.xyz;
          tmpvar_13 = tex_16.w;
          tmpvar_12 = _Shininess;
          float4 tmpvar_19;
          float _tmp_dvx_11 = texCUBE(_Cube, tmpvar_7);
          tmpvar_19 = float4(_tmp_dvx_11, _tmp_dvx_11, _tmp_dvx_11, _tmp_dvx_11);
          reflcol_14 = tmpvar_19;
          reflcol_14 = (reflcol_14 * tex_16.w);
          tmpvar_11 = (reflcol_14.xyz * _ReflectColor.xyz);
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_6;
          float3 viewDir_20;
          viewDir_20 = worldViewDir_5;
          float4 c_21;
          float4 c_22;
          float nh_23;
          float diff_24;
          float tmpvar_25;
          tmpvar_25 = max(0, dot(tmpvar_4, tmpvar_2));
          diff_24 = tmpvar_25;
          float tmpvar_26;
          tmpvar_26 = max(0, dot(tmpvar_4, normalize((tmpvar_2 + viewDir_20))));
          nh_23 = tmpvar_26;
          float y_27;
          y_27 = (tmpvar_12 * 128);
          float tmpvar_28;
          tmpvar_28 = (pow(nh_23, y_27) * tmpvar_13);
          c_22.xyz = (((tmpvar_10 * tmpvar_1) * diff_24) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_28));
          c_22.w = 0;
          c_21.w = c_22.w;
          c_21.xyz = (c_22.xyz + (tmpvar_10 * in_f.xlv_TEXCOORD4));
          c_3.xyz = (c_21.xyz + tmpvar_11);
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
      LOD 300
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
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _SpecColor;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      uniform float _Shininess;
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
          float3 worldViewDir_5;
          float3 lightDir_6;
          float3 tmpvar_7;
          tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD2));
          lightDir_6 = tmpvar_7;
          float3 tmpvar_8;
          tmpvar_8 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD2));
          worldViewDir_5 = tmpvar_8;
          tmpvar_4 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_9;
          float tmpvar_10;
          float tmpvar_11;
          float4 c_12;
          float4 tex_13;
          float4 tmpvar_14;
          tmpvar_14 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_13 = tmpvar_14;
          float4 tmpvar_15;
          tmpvar_15 = (tex_13 * _Color);
          c_12 = tmpvar_15;
          tmpvar_9 = c_12.xyz;
          tmpvar_11 = tex_13.w;
          tmpvar_10 = _Shininess;
          float4 tmpvar_16;
          tmpvar_16.w = 1;
          tmpvar_16.xyz = in_f.xlv_TEXCOORD2;
          float3 tmpvar_17;
          tmpvar_17 = mul(unity_WorldToLight, tmpvar_16).xyz;
          float tmpvar_18;
          tmpvar_18 = dot(tmpvar_17, tmpvar_17);
          float tmpvar_19;
          tmpvar_19 = tex2D(_LightTexture0, float2(tmpvar_18, tmpvar_18)).w;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_6;
          tmpvar_1 = (tmpvar_1 * tmpvar_19);
          float3 viewDir_20;
          viewDir_20 = worldViewDir_5;
          float4 c_21;
          float4 c_22;
          float nh_23;
          float diff_24;
          float tmpvar_25;
          tmpvar_25 = max(0, dot(tmpvar_4, tmpvar_2));
          diff_24 = tmpvar_25;
          float tmpvar_26;
          tmpvar_26 = max(0, dot(tmpvar_4, normalize((tmpvar_2 + viewDir_20))));
          nh_23 = tmpvar_26;
          float y_27;
          y_27 = (tmpvar_10 * 128);
          float tmpvar_28;
          tmpvar_28 = (pow(nh_23, y_27) * tmpvar_11);
          c_22.xyz = (((tmpvar_9 * tmpvar_1) * diff_24) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_28));
          c_22.w = 0;
          c_21.w = c_22.w;
          c_21.xyz = c_22.xyz;
          c_3.xyz = c_21.xyz;
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
      LOD 300
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
      uniform float _Shininess;
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
          float tmpvar_3;
          tmpvar_3 = _Shininess;
          res_1.xyz = float3(((tmpvar_2 * 0.5) + 0.5));
          res_1.w = tmpvar_3;
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
      LOD 300
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
      //uniform float3 _WorldSpaceCameraPos;
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
      uniform float4 _SpecColor;
      uniform sampler2D _MainTex;
      uniform samplerCUBE _Cube;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
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
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
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
          float3 tmpvar_4;
          float4 tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = in_v.vertex.xyz;
          tmpvar_5 = UnityObjectToClipPos(tmpvar_6);
          float3 tmpvar_7;
          tmpvar_7 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_8;
          tmpvar_8[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_8[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_8[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_9;
          tmpvar_9 = normalize(mul(in_v.normal, tmpvar_8));
          worldNormal_1 = tmpvar_9;
          float3 tmpvar_10;
          float3 I_11;
          I_11 = (tmpvar_7 - _WorldSpaceCameraPos);
          tmpvar_10 = (I_11 - (2 * (dot(worldNormal_1, I_11) * worldNormal_1)));
          tmpvar_2 = tmpvar_10;
          float4 o_12;
          float4 tmpvar_13;
          tmpvar_13 = (tmpvar_5 * 0.5);
          float2 tmpvar_14;
          tmpvar_14.x = tmpvar_13.x;
          tmpvar_14.y = (tmpvar_13.y * _ProjectionParams.x);
          o_12.xy = (tmpvar_14 + tmpvar_13.w);
          o_12.zw = tmpvar_5.zw;
          tmpvar_3.zw = float2(0, 0);
          tmpvar_3.xy = float2(0, 0);
          float3x3 tmpvar_15;
          tmpvar_15[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_15[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_15[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float4 tmpvar_16;
          tmpvar_16.w = 1;
          tmpvar_16.xyz = float3(normalize(mul(in_v.normal, tmpvar_15)));
          float4 normal_17;
          normal_17 = tmpvar_16;
          float3 res_18;
          float3 x_19;
          x_19.x = dot(unity_SHAr, normal_17);
          x_19.y = dot(unity_SHAg, normal_17);
          x_19.z = dot(unity_SHAb, normal_17);
          float3 x1_20;
          float4 tmpvar_21;
          tmpvar_21 = (normal_17.xyzz * normal_17.yzzx);
          x1_20.x = dot(unity_SHBr, tmpvar_21);
          x1_20.y = dot(unity_SHBg, tmpvar_21);
          x1_20.z = dot(unity_SHBb, tmpvar_21);
          res_18 = (x_19 + (x1_20 + (unity_SHC.xyz * ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y)))));
          float3 tmpvar_22;
          float _tmp_dvx_12 = max(((1.055 * pow(max(res_18, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_22 = float3(_tmp_dvx_12, _tmp_dvx_12, _tmp_dvx_12);
          res_18 = tmpvar_22;
          tmpvar_4 = tmpvar_22;
          out_v.vertex = tmpvar_5;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = tmpvar_7;
          out_v.xlv_TEXCOORD3 = o_12;
          out_v.xlv_TEXCOORD4 = tmpvar_3;
          out_v.xlv_TEXCOORD5 = tmpvar_4;
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
          tmpvar_4 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_5;
          float3 tmpvar_6;
          float tmpvar_7;
          float4 reflcol_8;
          float4 c_9;
          float4 tex_10;
          float4 tmpvar_11;
          tmpvar_11 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_10 = tmpvar_11;
          float4 tmpvar_12;
          tmpvar_12 = (tex_10 * _Color);
          c_9 = tmpvar_12;
          tmpvar_5 = c_9.xyz;
          tmpvar_7 = tex_10.w;
          float4 tmpvar_13;
          float _tmp_dvx_13 = texCUBE(_Cube, tmpvar_4);
          tmpvar_13 = float4(_tmp_dvx_13, _tmp_dvx_13, _tmp_dvx_13, _tmp_dvx_13);
          reflcol_8 = tmpvar_13;
          reflcol_8 = (reflcol_8 * tex_10.w);
          tmpvar_6 = (reflcol_8.xyz * _ReflectColor.xyz);
          float4 tmpvar_14;
          tmpvar_14 = tex2D(_LightBuffer, in_f.xlv_TEXCOORD3);
          light_3 = tmpvar_14;
          light_3 = (-log2(max(light_3, float4(0.001, 0.001, 0.001, 0.001))));
          light_3.xyz = (light_3.xyz + in_f.xlv_TEXCOORD5);
          float4 c_15;
          float spec_16;
          float tmpvar_17;
          tmpvar_17 = (light_3.w * tmpvar_7);
          spec_16 = tmpvar_17;
          c_15.xyz = ((tmpvar_5 * light_3.xyz) + ((light_3.xyz * _SpecColor.xyz) * spec_16));
          c_15.w = 0;
          c_2 = c_15;
          c_2.xyz = (c_2.xyz + tmpvar_6);
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
      LOD 300
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
      //uniform float3 _WorldSpaceCameraPos;
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
      uniform float4 _SpecColor;
      uniform sampler2D _MainTex;
      uniform samplerCUBE _Cube;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float _Shininess;
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
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
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
          float3 tmpvar_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = in_v.vertex.xyz;
          float3 tmpvar_6;
          tmpvar_6 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_7;
          tmpvar_7[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_7[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_7[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_8;
          tmpvar_8 = normalize(mul(in_v.normal, tmpvar_7));
          worldNormal_1 = tmpvar_8;
          tmpvar_3 = worldNormal_1;
          float3 tmpvar_9;
          float3 I_10;
          I_10 = (tmpvar_6 - _WorldSpaceCameraPos);
          tmpvar_9 = (I_10 - (2 * (dot(worldNormal_1, I_10) * worldNormal_1)));
          tmpvar_2 = tmpvar_9;
          tmpvar_4.zw = float2(0, 0);
          tmpvar_4.xy = float2(0, 0);
          float3 normal_11;
          normal_11 = worldNormal_1;
          float4 tmpvar_12;
          tmpvar_12.w = 1;
          tmpvar_12.xyz = float3(normal_11);
          float3 res_13;
          float3 x_14;
          x_14.x = dot(unity_SHAr, tmpvar_12);
          x_14.y = dot(unity_SHAg, tmpvar_12);
          x_14.z = dot(unity_SHAb, tmpvar_12);
          float3 x1_15;
          float4 tmpvar_16;
          tmpvar_16 = (normal_11.xyzz * normal_11.yzzx);
          x1_15.x = dot(unity_SHBr, tmpvar_16);
          x1_15.y = dot(unity_SHBg, tmpvar_16);
          x1_15.z = dot(unity_SHBb, tmpvar_16);
          res_13 = (x_14 + (x1_15 + (unity_SHC.xyz * ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y)))));
          float3 tmpvar_17;
          float _tmp_dvx_14 = max(((1.055 * pow(max(res_13, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_17 = float3(_tmp_dvx_14, _tmp_dvx_14, _tmp_dvx_14);
          res_13 = tmpvar_17;
          out_v.vertex = UnityObjectToClipPos(tmpvar_5);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = tmpvar_3;
          out_v.xlv_TEXCOORD3 = tmpvar_6;
          out_v.xlv_TEXCOORD4 = tmpvar_4;
          out_v.xlv_TEXCOORD5 = max(float3(0, 0, 0), tmpvar_17);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 outEmission_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          tmpvar_3 = in_f.xlv_TEXCOORD1;
          tmpvar_2 = in_f.xlv_TEXCOORD2;
          float3 tmpvar_4;
          float3 tmpvar_5;
          float tmpvar_6;
          float tmpvar_7;
          float4 reflcol_8;
          float4 c_9;
          float4 tex_10;
          float4 tmpvar_11;
          tmpvar_11 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_10 = tmpvar_11;
          float4 tmpvar_12;
          tmpvar_12 = (tex_10 * _Color);
          c_9 = tmpvar_12;
          tmpvar_4 = c_9.xyz;
          tmpvar_7 = tex_10.w;
          tmpvar_6 = _Shininess;
          float4 tmpvar_13;
          float _tmp_dvx_15 = texCUBE(_Cube, tmpvar_3);
          tmpvar_13 = float4(_tmp_dvx_15, _tmp_dvx_15, _tmp_dvx_15, _tmp_dvx_15);
          reflcol_8 = tmpvar_13;
          reflcol_8 = (reflcol_8 * tex_10.w);
          tmpvar_5 = (reflcol_8.xyz * _ReflectColor.xyz);
          float4 emission_14;
          float3 tmpvar_15;
          float3 tmpvar_16;
          float3 tmpvar_17;
          tmpvar_15 = tmpvar_4;
          tmpvar_16 = ((_SpecColor.xyz * tmpvar_7) * 0.3183099);
          tmpvar_17 = tmpvar_2;
          float4 tmpvar_18;
          tmpvar_18.xyz = float3(tmpvar_15);
          tmpvar_18.w = 1;
          float4 tmpvar_19;
          tmpvar_19.xyz = float3(tmpvar_16);
          tmpvar_19.w = tmpvar_6;
          float4 tmpvar_20;
          tmpvar_20.w = 1;
          tmpvar_20.xyz = float3(((tmpvar_17 * 0.5) + 0.5));
          float4 tmpvar_21;
          tmpvar_21.w = 1;
          tmpvar_21.xyz = float3(tmpvar_5);
          emission_14 = tmpvar_21;
          emission_14.xyz = (emission_14.xyz + (tmpvar_4 * in_f.xlv_TEXCOORD5));
          outEmission_1.w = emission_14.w;
          outEmission_1.xyz = exp2((-emission_14.xyz));
          out_f.color = tmpvar_18;
          out_f.color1 = tmpvar_19;
          out_f.color2 = tmpvar_20;
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
      LOD 300
      ZClip Off
      Cull Off
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
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform samplerCUBE _Cube;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
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
          float4 vertex_3;
          vertex_3 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_3.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_4;
              if((in_v.vertex.z>0))
              {
                  tmpvar_4 = 0.0001;
              }
              else
              {
                  tmpvar_4 = 0;
              }
              vertex_3.z = tmpvar_4;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_3.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_5;
              if((vertex_3.z>0))
              {
                  tmpvar_5 = 0.0001;
              }
              else
              {
                  tmpvar_5 = 0;
              }
              vertex_3.z = tmpvar_5;
          }
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = vertex_3.xyz;
          float3 tmpvar_7;
          tmpvar_7 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_8;
          tmpvar_8[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_8[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_8[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_9;
          tmpvar_9 = normalize(mul(in_v.normal, tmpvar_8));
          worldNormal_1 = tmpvar_9;
          float3 tmpvar_10;
          float3 I_11;
          I_11 = (tmpvar_7 - _WorldSpaceCameraPos);
          tmpvar_10 = (I_11 - (2 * (dot(worldNormal_1, I_11) * worldNormal_1)));
          tmpvar_2 = tmpvar_10;
          out_v.vertex = UnityObjectToClipPos(tmpvar_6);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = tmpvar_7;
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
          tmpvar_4 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_5;
          float3 tmpvar_6;
          float4 reflcol_7;
          float4 c_8;
          float4 tex_9;
          float4 tmpvar_10;
          tmpvar_10 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_9 = tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11 = (tex_9 * _Color);
          c_8 = tmpvar_11;
          tmpvar_5 = c_8.xyz;
          float4 tmpvar_12;
          float _tmp_dvx_16 = texCUBE(_Cube, tmpvar_4);
          tmpvar_12 = float4(_tmp_dvx_16, _tmp_dvx_16, _tmp_dvx_16, _tmp_dvx_16);
          reflcol_7 = tmpvar_12;
          reflcol_7 = (reflcol_7 * tex_9.w);
          tmpvar_6 = (reflcol_7.xyz * _ReflectColor.xyz);
          tmpvar_2 = tmpvar_5;
          tmpvar_3 = tmpvar_6;
          float4 res_13;
          res_13 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_14;
              tmpvar_14.w = 1;
              tmpvar_14.xyz = float3(tmpvar_2);
              res_13.w = tmpvar_14.w;
              float3 tmpvar_15;
              float _tmp_dvx_17 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_15 = clamp(pow(tmpvar_2, float3(_tmp_dvx_17, _tmp_dvx_17, _tmp_dvx_17)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_13.xyz = float3(tmpvar_15);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_16;
              if(int(unity_UseLinearSpace))
              {
                  emission_16 = tmpvar_3;
              }
              else
              {
                  emission_16 = (tmpvar_3 * ((tmpvar_3 * ((tmpvar_3 * 0.305306) + 0.6821711)) + 0.01252288));
              }
              float4 tmpvar_17;
              float alpha_18;
              float3 tmpvar_19;
              tmpvar_19 = (emission_16 * 0.01030928);
              alpha_18 = (ceil((max(max(tmpvar_19.x, tmpvar_19.y), max(tmpvar_19.z, 0.02)) * 255)) / 255);
              float tmpvar_20;
              tmpvar_20 = max(alpha_18, 0.02);
              alpha_18 = tmpvar_20;
              float4 tmpvar_21;
              tmpvar_21.xyz = float3((tmpvar_19 / tmpvar_20));
              tmpvar_21.w = tmpvar_20;
              tmpvar_17 = tmpvar_21;
              res_13 = tmpvar_17;
          }
          tmpvar_1 = res_13;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Reflective/VertexLit"
}
