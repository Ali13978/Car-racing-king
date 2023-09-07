// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GR3DCarPaint"
{
  Properties
  {
    _MainTex ("Albedo (RGB)", 2D) = "white" {}
    _Color ("Main Color", Color) = (1,1,1,1)
    _ColOut ("Outer Color", Color) = (1,1,1,1)
    _GlossOut ("Outer Gloss", Range(0, 1)) = 0.97
    _ColIn ("Inner Color", Color) = (1,1,1,1)
    _GlossIn ("Inner Gloss", Range(0, 1)) = 0.95
    _Highlight ("Highlight", Range(0, 0.5)) = 0.1
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
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      uniform float4 _MainTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      //uniform samplerCUBE unity_SpecCube0;
      //uniform float4 unity_SpecCube0_HDR;
      uniform float4 _LightColor0;
      uniform sampler2D unity_NHxRoughness;
      uniform sampler2D _MainTex;
      uniform float _Highlight;
      uniform float _GlossOut;
      uniform float _GlossIn;
      uniform float4 _Color;
      uniform float4 _ColOut;
      uniform float4 _ColIn;
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
          float4 xlv_TEXCOORD6 :TEXCOORD6;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
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
          float3 normal_7;
          normal_7 = worldNormal_1;
          float3 x1_8;
          float4 tmpvar_9;
          tmpvar_9 = (normal_7.xyzz * normal_7.yzzx);
          x1_8.x = dot(unity_SHBr, tmpvar_9);
          x1_8.y = dot(unity_SHBg, tmpvar_9);
          x1_8.z = dot(unity_SHBb, tmpvar_9);
          out_v.vertex = UnityObjectToClipPos(tmpvar_4);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD3 = (x1_8 + (unity_SHC.xyz * ((normal_7.x * normal_7.x) - (normal_7.y * normal_7.y))));
          out_v.xlv_TEXCOORD6 = tmpvar_3;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float4 impl_low_textureCubeLodEXT(samplerCUBE sampler, float3 coord, float lod)
      {
          #if defined( GL_EXT_shader_texture_lod)
          {
              return texCUBE(sampler, float4(coord, lod));
              #else
              return texCUBE(sampler, coord, lod);
              #endif
          }
      
          OUT_Data_Frag frag(v2f in_f)
          {
              float3 tmpvar_1;
              float4 tmpvar_2;
              float3 tmpvar_3;
              float3 tmpvar_4;
              float4 c_5;
              float3 tmpvar_6;
              float tmpvar_7;
              float3 worldViewDir_8;
              float3 lightDir_9;
              float3 tmpvar_10;
              float3 tmpvar_11;
              tmpvar_11 = _WorldSpaceLightPos0.xyz;
              lightDir_9 = tmpvar_11;
              float3 tmpvar_12;
              tmpvar_12 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD2));
              worldViewDir_8 = tmpvar_12;
              tmpvar_10 = worldViewDir_8;
              tmpvar_6 = in_f.xlv_TEXCOORD1;
              float3 tmpvar_13;
              float4 ssp_14;
              float fsp_15;
              float tmpvar_16;
              float tmpvar_17;
              tmpvar_17 = clamp(dot(normalize(tmpvar_10), tmpvar_6), 0, 1);
              tmpvar_16 = tmpvar_17;
              float tmpvar_18;
              tmpvar_18 = ((0.2 * pow(tmpvar_16, 4)) * _Highlight);
              fsp_15 = tmpvar_18;
              float4 tmpvar_19;
              tmpvar_19 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
              float tmpvar_20;
              tmpvar_20 = pow(tmpvar_16, 4);
              float4 tmpvar_21;
              tmpvar_21 = (((tmpvar_19 * _Color) * lerp(_ColOut, _ColIn, float4(tmpvar_20, tmpvar_20, tmpvar_20, tmpvar_20))) + fsp_15);
              ssp_14 = tmpvar_21;
              tmpvar_13 = ssp_14.xyz;
              float tmpvar_22;
              tmpvar_22 = lerp(_GlossOut, _GlossIn, pow(tmpvar_16, 4));
              c_5 = float4(0, 0, 0, 0);
              tmpvar_3 = _LightColor0.xyz;
              tmpvar_4 = lightDir_9;
              tmpvar_1 = worldViewDir_8;
              tmpvar_2 = unity_SpecCube0_HDR;
              float3 Normal_23;
              Normal_23 = tmpvar_6;
              float tmpvar_24;
              tmpvar_24 = (1 - tmpvar_22);
              float3 I_25;
              I_25 = (-tmpvar_1);
              float3 normalWorld_26;
              normalWorld_26 = tmpvar_6;
              float3 tmpvar_27;
              float3 tmpvar_28;
              float4 tmpvar_29;
              tmpvar_29.w = 1;
              tmpvar_29.xyz = float3(normalWorld_26);
              float3 x_30;
              x_30.x = dot(unity_SHAr, tmpvar_29);
              x_30.y = dot(unity_SHAg, tmpvar_29);
              x_30.z = dot(unity_SHAb, tmpvar_29);
              tmpvar_28 = max(((1.055 * pow(max(float3(0, 0, 0), (in_f.xlv_TEXCOORD3 + x_30)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
              tmpvar_27 = float3(0, 0, 0);
              float4 hdr_31;
              hdr_31 = tmpvar_2;
              float4 tmpvar_32;
              tmpvar_32.xyz = float3((I_25 - (2 * (dot(Normal_23, I_25) * Normal_23))));
              tmpvar_32.w = ((tmpvar_24 * (1.7 - (0.7 * tmpvar_24))) * 6);
              float4 tmpvar_33;
              tmpvar_33 = impl_low_textureCubeLodEXT(unity_SpecCube0, tmpvar_32.xyz, tmpvar_32.w);
              float4 tmpvar_34;
              tmpvar_34 = tmpvar_33;
              float tmpvar_35;
              if((hdr_31.w==1))
              {
                  tmpvar_35 = tmpvar_34.w;
              }
              else
              {
                  tmpvar_35 = 1;
              }
              tmpvar_27 = ((hdr_31.x * tmpvar_35) * tmpvar_34.xyz);
              float3 tmpvar_36;
              float3 viewDir_37;
              viewDir_37 = worldViewDir_8;
              float4 c_38;
              float3 tmpvar_39;
              tmpvar_39 = normalize(tmpvar_6);
              float3 tmpvar_40;
              float3 albedo_41;
              albedo_41 = tmpvar_13;
              float3 tmpvar_42;
              tmpvar_42 = lerp(float3(0.2209163, 0.2209163, 0.2209163), albedo_41, float3(tmpvar_7, tmpvar_7, tmpvar_7));
              float tmpvar_43;
              tmpvar_43 = (0.7790837 - (tmpvar_7 * 0.7790837));
              tmpvar_40 = (albedo_41 * tmpvar_43);
              tmpvar_36 = tmpvar_40;
              float3 diffColor_44;
              diffColor_44 = tmpvar_36;
              tmpvar_36 = diffColor_44;
              float3 diffColor_45;
              diffColor_45 = tmpvar_36;
              float3 normal_46;
              normal_46 = tmpvar_39;
              float3 color_47;
              float2 tmpvar_48;
              tmpvar_48.x = dot((viewDir_37 - (2 * (dot(normal_46, viewDir_37) * normal_46))), tmpvar_4);
              tmpvar_48.y = (1 - clamp(dot(normal_46, viewDir_37), 0, 1));
              float2 tmpvar_49;
              tmpvar_49 = ((tmpvar_48 * tmpvar_48) * (tmpvar_48 * tmpvar_48));
              float2 tmpvar_50;
              tmpvar_50.x = tmpvar_49.x;
              tmpvar_50.y = (1 - tmpvar_22);
              float4 tmpvar_51;
              tmpvar_51 = tex2D(unity_NHxRoughness, tmpvar_50);
              color_47 = ((diffColor_45 + ((tmpvar_51.w * 16) * tmpvar_42)) * (tmpvar_3 * clamp(dot(normal_46, tmpvar_4), 0, 1)));
              float _tmp_dvx_4 = clamp((tmpvar_22 + (1 - tmpvar_43)), 0, 1);
              color_47 = (color_47 + ((tmpvar_28 * diffColor_45) + (tmpvar_27 * lerp(tmpvar_42, float3(_tmp_dvx_4, _tmp_dvx_4, _tmp_dvx_4), tmpvar_49.yyy))));
              c_38.xyz = float3(color_47);
              c_38.w = 0;
              c_5.xyz = c_38.xyz;
              c_5.w = 1;
              out_f.color = c_5;
          }
      
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
        "SHADOWSUPPORT" = "true"
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
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D unity_NHxRoughness;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform sampler2D _MainTex;
      uniform float _Highlight;
      uniform float _GlossOut;
      uniform float _GlossIn;
      uniform float4 _Color;
      uniform float4 _ColOut;
      uniform float4 _ColIn;
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
          float tmpvar_5;
          float3 worldViewDir_6;
          float3 lightDir_7;
          float3 tmpvar_8;
          float3 tmpvar_9;
          tmpvar_9 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD2));
          lightDir_7 = tmpvar_9;
          float3 tmpvar_10;
          tmpvar_10 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD2));
          worldViewDir_6 = tmpvar_10;
          tmpvar_8 = worldViewDir_6;
          tmpvar_4 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_11;
          float4 ssp_12;
          float fsp_13;
          float tmpvar_14;
          float tmpvar_15;
          tmpvar_15 = clamp(dot(normalize(tmpvar_8), tmpvar_4), 0, 1);
          tmpvar_14 = tmpvar_15;
          float tmpvar_16;
          tmpvar_16 = ((0.2 * pow(tmpvar_14, 4)) * _Highlight);
          fsp_13 = tmpvar_16;
          float4 tmpvar_17;
          tmpvar_17 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float tmpvar_18;
          tmpvar_18 = pow(tmpvar_14, 4);
          float4 tmpvar_19;
          tmpvar_19 = (((tmpvar_17 * _Color) * lerp(_ColOut, _ColIn, float4(tmpvar_18, tmpvar_18, tmpvar_18, tmpvar_18))) + fsp_13);
          ssp_12 = tmpvar_19;
          tmpvar_11 = ssp_12.xyz;
          float4 tmpvar_20;
          tmpvar_20.w = 1;
          tmpvar_20.xyz = in_f.xlv_TEXCOORD2;
          float3 tmpvar_21;
          tmpvar_21 = mul(unity_WorldToLight, tmpvar_20).xyz;
          float tmpvar_22;
          tmpvar_22 = dot(tmpvar_21, tmpvar_21);
          float tmpvar_23;
          tmpvar_23 = tex2D(_LightTexture0, float2(tmpvar_22, tmpvar_22)).w;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_7;
          tmpvar_1 = (tmpvar_1 * tmpvar_23);
          float3 tmpvar_24;
          float3 viewDir_25;
          viewDir_25 = worldViewDir_6;
          float4 c_26;
          float3 tmpvar_27;
          tmpvar_27 = normalize(tmpvar_4);
          float3 tmpvar_28;
          float3 albedo_29;
          albedo_29 = tmpvar_11;
          tmpvar_28 = (albedo_29 * (0.7790837 - (tmpvar_5 * 0.7790837)));
          tmpvar_24 = tmpvar_28;
          float3 diffColor_30;
          diffColor_30 = tmpvar_24;
          tmpvar_24 = diffColor_30;
          float3 diffColor_31;
          diffColor_31 = tmpvar_24;
          float3 normal_32;
          normal_32 = tmpvar_27;
          float2 tmpvar_33;
          tmpvar_33.x = dot((viewDir_25 - (2 * (dot(normal_32, viewDir_25) * normal_32))), tmpvar_2);
          tmpvar_33.y = (1 - clamp(dot(normal_32, viewDir_25), 0, 1));
          float2 tmpvar_34;
          tmpvar_34.x = ((tmpvar_33 * tmpvar_33) * (tmpvar_33 * tmpvar_33)).x;
          tmpvar_34.y = (1 - lerp(_GlossOut, _GlossIn, pow(tmpvar_14, 4)));
          float4 tmpvar_35;
          tmpvar_35 = tex2D(unity_NHxRoughness, tmpvar_34);
          c_26.xyz = ((diffColor_31 + ((tmpvar_35.w * 16) * lerp(float3(0.2209163, 0.2209163, 0.2209163), albedo_29, float3(tmpvar_5, tmpvar_5, tmpvar_5)))) * (tmpvar_1 * clamp(dot(normal_32, tmpvar_2), 0, 1)));
          c_26.w = 0;
          c_3.xyz = c_26.xyz;
          c_3.w = 1;
          out_f.color = c_3;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: DEFERRED
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
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      uniform float4 _MainTex_ST;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      uniform sampler2D _MainTex;
      uniform float _Highlight;
      uniform float _GlossOut;
      uniform float _GlossIn;
      uniform float4 _Color;
      uniform float4 _ColOut;
      uniform float4 _ColIn;
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
          float3 xlv_TEXCOORD3 :TEXCOORD3;
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
          tmpvar_2 = worldNormal_1;
          float3 tmpvar_9;
          tmpvar_9 = (_WorldSpaceCameraPos - tmpvar_6);
          tmpvar_3 = tmpvar_9;
          tmpvar_4.zw = float2(0, 0);
          tmpvar_4.xy = float2(0, 0);
          float3 normal_10;
          normal_10 = worldNormal_1;
          float3 x1_11;
          float4 tmpvar_12;
          tmpvar_12 = (normal_10.xyzz * normal_10.yzzx);
          x1_11.x = dot(unity_SHBr, tmpvar_12);
          x1_11.y = dot(unity_SHBg, tmpvar_12);
          x1_11.z = dot(unity_SHBb, tmpvar_12);
          out_v.vertex = UnityObjectToClipPos(tmpvar_5);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = tmpvar_6;
          out_v.xlv_TEXCOORD3 = tmpvar_3;
          out_v.xlv_TEXCOORD4 = tmpvar_4;
          out_v.xlv_TEXCOORD5 = (x1_11 + (unity_SHC.xyz * ((normal_10.x * normal_10.x) - (normal_10.y * normal_10.y))));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 outEmission_1;
          float3 tmpvar_2;
          float tmpvar_3;
          float3 viewDir_4;
          float3 tmpvar_5;
          float3 tmpvar_6;
          tmpvar_6 = normalize(in_f.xlv_TEXCOORD3);
          viewDir_4 = tmpvar_6;
          tmpvar_5 = viewDir_4;
          tmpvar_2 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_7;
          float4 ssp_8;
          float fsp_9;
          float tmpvar_10;
          float tmpvar_11;
          tmpvar_11 = clamp(dot(normalize(tmpvar_5), tmpvar_2), 0, 1);
          tmpvar_10 = tmpvar_11;
          float tmpvar_12;
          tmpvar_12 = ((0.2 * pow(tmpvar_10, 4)) * _Highlight);
          fsp_9 = tmpvar_12;
          float4 tmpvar_13;
          tmpvar_13 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float tmpvar_14;
          tmpvar_14 = pow(tmpvar_10, 4);
          float4 tmpvar_15;
          tmpvar_15 = (((tmpvar_13 * _Color) * lerp(_ColOut, _ColIn, float4(tmpvar_14, tmpvar_14, tmpvar_14, tmpvar_14))) + fsp_9);
          ssp_8 = tmpvar_15;
          tmpvar_7 = ssp_8.xyz;
          float3 normalWorld_16;
          normalWorld_16 = tmpvar_2;
          float4 tmpvar_17;
          tmpvar_17.w = 1;
          tmpvar_17.xyz = float3(normalWorld_16);
          float3 x_18;
          x_18.x = dot(unity_SHAr, tmpvar_17);
          x_18.y = dot(unity_SHAg, tmpvar_17);
          x_18.z = dot(unity_SHAb, tmpvar_17);
          float3 tmpvar_19;
          float3 tmpvar_20;
          float3 tmpvar_21;
          float3 tmpvar_22;
          float3 albedo_23;
          albedo_23 = tmpvar_7;
          tmpvar_22 = (albedo_23 * (0.7790837 - (tmpvar_3 * 0.7790837)));
          tmpvar_19 = tmpvar_22;
          float3 diffColor_24;
          diffColor_24 = tmpvar_19;
          float3 color_25;
          color_25 = (max(((1.055 * pow(max(float3(0, 0, 0), (in_f.xlv_TEXCOORD5 + x_18)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0)) * diffColor_24);
          tmpvar_20 = tmpvar_19;
          tmpvar_21 = tmpvar_2;
          float4 tmpvar_26;
          tmpvar_26.xyz = float3(tmpvar_20);
          tmpvar_26.w = 1;
          float4 tmpvar_27;
          tmpvar_27.xyz = float3(lerp(float3(0.2209163, 0.2209163, 0.2209163), albedo_23, float3(tmpvar_3, tmpvar_3, tmpvar_3)));
          tmpvar_27.w = lerp(_GlossOut, _GlossIn, pow(tmpvar_10, 4));
          float4 tmpvar_28;
          tmpvar_28.w = 1;
          tmpvar_28.xyz = float3(((tmpvar_21 * 0.5) + 0.5));
          float4 tmpvar_29;
          tmpvar_29.w = 1;
          tmpvar_29.xyz = float3(color_25);
          outEmission_1.w = tmpvar_29.w;
          outEmission_1.xyz = float3(exp2((-color_25)));
          out_f.color = tmpvar_26;
          out_f.color1 = tmpvar_27;
          out_f.color2 = tmpvar_28;
          out_f.color3 = outEmission_1;
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
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      uniform sampler2D _MainTex;
      uniform float _Highlight;
      uniform float4 _Color;
      uniform float4 _ColOut;
      uniform float4 _ColIn;
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
          float3x3 tmpvar_7;
          tmpvar_7[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_7[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_7[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_8;
          tmpvar_8 = normalize(mul(in_v.normal, tmpvar_7));
          worldNormal_1 = tmpvar_8;
          tmpvar_2 = worldNormal_1;
          out_v.vertex = UnityObjectToClipPos(tmpvar_6);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          float3 worldViewDir_4;
          float3 tmpvar_5;
          float3 tmpvar_6;
          tmpvar_6 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD2));
          worldViewDir_4 = tmpvar_6;
          tmpvar_5 = worldViewDir_4;
          tmpvar_3 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_7;
          float4 ssp_8;
          float fsp_9;
          float tmpvar_10;
          float tmpvar_11;
          tmpvar_11 = clamp(dot(normalize(tmpvar_5), tmpvar_3), 0, 1);
          tmpvar_10 = tmpvar_11;
          float tmpvar_12;
          tmpvar_12 = ((0.2 * pow(tmpvar_10, 4)) * _Highlight);
          fsp_9 = tmpvar_12;
          float4 tmpvar_13;
          tmpvar_13 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          float tmpvar_14;
          tmpvar_14 = pow(tmpvar_10, 4);
          float4 tmpvar_15;
          tmpvar_15 = (((tmpvar_13 * _Color) * lerp(_ColOut, _ColIn, float4(tmpvar_14, tmpvar_14, tmpvar_14, tmpvar_14))) + fsp_9);
          ssp_8 = tmpvar_15;
          tmpvar_7 = ssp_8.xyz;
          tmpvar_2 = tmpvar_7;
          float4 res_16;
          res_16 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_17;
              tmpvar_17.w = 1;
              tmpvar_17.xyz = float3(tmpvar_2);
              res_16.w = tmpvar_17.w;
              float3 tmpvar_18;
              float _tmp_dvx_5 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_18 = clamp(pow(tmpvar_2, float3(_tmp_dvx_5, _tmp_dvx_5, _tmp_dvx_5)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_16.xyz = float3(tmpvar_18);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_19;
              if(int(unity_UseLinearSpace))
              {
                  emission_19 = float3(0, 0, 0);
              }
              else
              {
                  emission_19 = float3(0, 0, 0);
              }
              float4 tmpvar_20;
              float alpha_21;
              float3 tmpvar_22;
              tmpvar_22 = (emission_19 * 0.01030928);
              alpha_21 = (ceil((max(max(tmpvar_22.x, tmpvar_22.y), max(tmpvar_22.z, 0.02)) * 255)) / 255);
              float tmpvar_23;
              tmpvar_23 = max(alpha_21, 0.02);
              alpha_21 = tmpvar_23;
              float4 tmpvar_24;
              tmpvar_24.xyz = float3((tmpvar_22 / tmpvar_23));
              tmpvar_24.w = tmpvar_23;
              tmpvar_20 = tmpvar_24;
              res_16 = tmpvar_20;
          }
          tmpvar_1 = res_16;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
