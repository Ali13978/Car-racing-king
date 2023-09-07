// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Enviro/BumpedDiffuseOverlaySM2"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _Opacity ("Color over opacity", Range(0, 1)) = 1
    _MainTex ("Color over (RGBA)", 2D) = "white" {}
    _BumpMap ("Normalmap over", 2D) = "bump" {}
    _MainTex2 ("Color under (RGBA)", 2D) = "white" {}
    _BumpMap2 ("Normalmap under", 2D) = "bump" {}
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "Opaque"
    }
    LOD 400
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "ForwardBase"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 400
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
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _MainTex_ST;
      uniform float4 _BumpMap_ST;
      uniform float4 _MainTex2_ST;
      uniform float4 _BumpMap2_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex2;
      uniform sampler2D _BumpMap2;
      uniform float4 _Color;
      uniform float _Opacity;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float tangentSign_1;
          float3 worldTangent_2;
          float3 worldNormal_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = in_v.vertex.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          tmpvar_5.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex2);
          tmpvar_5.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap2);
          float3 tmpvar_7;
          tmpvar_7 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_8;
          tmpvar_8[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_8[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_8[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_9;
          tmpvar_9 = normalize(mul(in_v.normal, tmpvar_8));
          worldNormal_3 = tmpvar_9;
          float3x3 tmpvar_10;
          tmpvar_10[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_10[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_10[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_11;
          tmpvar_11 = normalize(mul(tmpvar_10, in_v.tangent.xyz));
          worldTangent_2 = tmpvar_11;
          float tmpvar_12;
          tmpvar_12 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_1 = tmpvar_12;
          float3 tmpvar_13;
          tmpvar_13 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
          float4 tmpvar_14;
          tmpvar_14.x = worldTangent_2.x;
          tmpvar_14.y = tmpvar_13.x;
          tmpvar_14.z = worldNormal_3.x;
          tmpvar_14.w = tmpvar_7.x;
          float4 tmpvar_15;
          tmpvar_15.x = worldTangent_2.y;
          tmpvar_15.y = tmpvar_13.y;
          tmpvar_15.z = worldNormal_3.y;
          tmpvar_15.w = tmpvar_7.y;
          float4 tmpvar_16;
          tmpvar_16.x = worldTangent_2.z;
          tmpvar_16.y = tmpvar_13.z;
          tmpvar_16.z = worldNormal_3.z;
          tmpvar_16.w = tmpvar_7.z;
          float3 normal_17;
          normal_17 = worldNormal_3;
          float4 tmpvar_18;
          tmpvar_18.w = 1;
          tmpvar_18.xyz = float3(normal_17);
          float3 res_19;
          float3 x_20;
          x_20.x = dot(unity_SHAr, tmpvar_18);
          x_20.y = dot(unity_SHAg, tmpvar_18);
          x_20.z = dot(unity_SHAb, tmpvar_18);
          float3 x1_21;
          float4 tmpvar_22;
          tmpvar_22 = (normal_17.xyzz * normal_17.yzzx);
          x1_21.x = dot(unity_SHBr, tmpvar_22);
          x1_21.y = dot(unity_SHBg, tmpvar_22);
          x1_21.z = dot(unity_SHBb, tmpvar_22);
          res_19 = (x_20 + (x1_21 + (unity_SHC.xyz * ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y)))));
          float3 tmpvar_23;
          float _tmp_dvx_26 = max(((1.055 * pow(max(res_19, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_23 = float3(_tmp_dvx_26, _tmp_dvx_26, _tmp_dvx_26);
          res_19 = tmpvar_23;
          out_v.vertex = UnityObjectToClipPos(tmpvar_6);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_5;
          out_v.xlv_TEXCOORD2 = tmpvar_14;
          out_v.xlv_TEXCOORD3 = tmpvar_15;
          out_v.xlv_TEXCOORD4 = tmpvar_16;
          out_v.xlv_TEXCOORD5 = max(float3(0, 0, 0), tmpvar_23);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float xlat_mutable_Opacity;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float3 worldN_3;
          float4 c_4;
          float3 tmpvar_5;
          float tmpvar_6;
          float3 lightDir_7;
          float3 tmpvar_8;
          tmpvar_8 = _WorldSpaceLightPos0.xyz;
          lightDir_7 = tmpvar_8;
          tmpvar_5 = float3(0, 0, 0);
          tmpvar_6 = 0;
          float3 tmpvar_9;
          float tmpvar_10;
          tmpvar_9 = tmpvar_5;
          tmpvar_10 = tmpvar_6;
          float4 norm2_11;
          float4 norm_12;
          float4 dest_13;
          float4 tex2_14;
          float4 tex_15;
          float4 tmpvar_16;
          tmpvar_16 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          tex_15 = tmpvar_16;
          float4 tmpvar_17;
          tmpvar_17 = tex2D(_MainTex2, in_f.xlv_TEXCOORD1.xy);
          tex2_14 = tmpvar_17;
          xlat_mutable_Opacity = (_Opacity * tex_15.w);
          float3 tmpvar_18;
          tmpvar_18 = bool3(tex2_14.xyz <= float3(0.5, 0.5, 0.5));
          float3 b_19;
          b_19 = ((2 * tex_15.xyz) * tex2_14.xyz);
          float3 c_20;
          c_20 = (1 - ((2 * (1 - tex_15.xyz)) * (1 - tex2_14.xyz)));
          float tmpvar_21;
          if(tmpvar_18.x)
          {
              tmpvar_21 = b_19.x;
          }
          else
          {
              tmpvar_21 = c_20.x;
          }
          float tmpvar_22;
          if(tmpvar_18.y)
          {
              tmpvar_22 = b_19.y;
          }
          else
          {
              tmpvar_22 = c_20.y;
          }
          float tmpvar_23;
          if(tmpvar_18.z)
          {
              tmpvar_23 = b_19.z;
          }
          else
          {
              tmpvar_23 = c_20.z;
          }
          float3 tmpvar_24;
          tmpvar_24.x = tmpvar_21;
          tmpvar_24.y = tmpvar_22;
          tmpvar_24.z = tmpvar_23;
          dest_13.xyz = lerp(tex2_14.xyz, tmpvar_24, float3(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_13.xyz = (dest_13.xyz * _Color.xyz);
          tmpvar_9 = dest_13.xyz;
          tmpvar_10 = (tex2_14.w * _Color.w);
          float4 tmpvar_25;
          tmpvar_25 = tex2D(_BumpMap, in_f.xlv_TEXCOORD0.zw);
          norm_12 = tmpvar_25;
          float4 tmpvar_26;
          tmpvar_26 = tex2D(_BumpMap2, in_f.xlv_TEXCOORD1.zw);
          norm2_11 = tmpvar_26;
          float4 tmpvar_27;
          tmpvar_27 = bool4(norm2_11 <= float4(0.5, 0.5, 0.5, 0.5));
          float4 b_28;
          float _tmp_dvx_27 = ((2 * norm_12) * norm2_11);
          b_28 = float4(_tmp_dvx_27, _tmp_dvx_27, _tmp_dvx_27, _tmp_dvx_27);
          float4 c_29;
          float _tmp_dvx_28 = (1 - ((2 * (1 - norm_12)) * (1 - norm2_11)));
          c_29 = float4(_tmp_dvx_28, _tmp_dvx_28, _tmp_dvx_28, _tmp_dvx_28);
          float tmpvar_30;
          if(tmpvar_27.x)
          {
              tmpvar_30 = b_28.x;
          }
          else
          {
              tmpvar_30 = c_29.x;
          }
          float tmpvar_31;
          if(tmpvar_27.y)
          {
              tmpvar_31 = b_28.y;
          }
          else
          {
              tmpvar_31 = c_29.y;
          }
          float tmpvar_32;
          if(tmpvar_27.z)
          {
              tmpvar_32 = b_28.z;
          }
          else
          {
              tmpvar_32 = c_29.z;
          }
          float tmpvar_33;
          if(tmpvar_27.w)
          {
              tmpvar_33 = b_28.w;
          }
          else
          {
              tmpvar_33 = c_29.w;
          }
          float4 tmpvar_34;
          tmpvar_34.x = tmpvar_30;
          tmpvar_34.y = tmpvar_31;
          tmpvar_34.z = tmpvar_32;
          tmpvar_34.w = tmpvar_33;
          float4 tmpvar_35;
          tmpvar_35 = lerp(norm2_11, tmpvar_34, float4(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_13 = tmpvar_35;
          float3 tmpvar_36;
          float4 packednormal_37;
          packednormal_37 = tmpvar_35;
          tmpvar_36 = ((packednormal_37.xyz * 2) - 1);
          tmpvar_5 = tmpvar_9;
          tmpvar_6 = tmpvar_10;
          float tmpvar_38;
          tmpvar_38 = dot(in_f.xlv_TEXCOORD2.xyz, tmpvar_36);
          worldN_3.x = tmpvar_38;
          float tmpvar_39;
          tmpvar_39 = dot(in_f.xlv_TEXCOORD3.xyz, tmpvar_36);
          worldN_3.y = tmpvar_39;
          float tmpvar_40;
          tmpvar_40 = dot(in_f.xlv_TEXCOORD4.xyz, tmpvar_36);
          worldN_3.z = tmpvar_40;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_7;
          float4 c_41;
          float4 c_42;
          float diff_43;
          float tmpvar_44;
          tmpvar_44 = max(0, dot(worldN_3, tmpvar_2));
          diff_43 = tmpvar_44;
          c_42.xyz = float3(((tmpvar_9 * tmpvar_1) * diff_43));
          c_42.w = tmpvar_10;
          c_41.w = c_42.w;
          c_41.xyz = (c_42.xyz + (tmpvar_9 * in_f.xlv_TEXCOORD5));
          c_4.xyz = c_41.xyz;
          c_4.w = 1;
          out_f.color = c_4;
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
      LOD 400
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
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _MainTex_ST;
      uniform float4 _BumpMap_ST;
      uniform float4 _MainTex2_ST;
      uniform float4 _BumpMap2_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex2;
      uniform sampler2D _BumpMap2;
      uniform float4 _Color;
      uniform float _Opacity;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float tangentSign_1;
          float3 worldTangent_2;
          float3 worldNormal_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = in_v.vertex.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          tmpvar_5.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex2);
          tmpvar_5.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap2);
          float3x3 tmpvar_7;
          tmpvar_7[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_7[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_7[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_8;
          tmpvar_8 = normalize(mul(in_v.normal, tmpvar_7));
          worldNormal_3 = tmpvar_8;
          float3x3 tmpvar_9;
          tmpvar_9[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_9[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_9[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_10;
          tmpvar_10 = normalize(mul(tmpvar_9, in_v.tangent.xyz));
          worldTangent_2 = tmpvar_10;
          float tmpvar_11;
          tmpvar_11 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_1 = tmpvar_11;
          float3 tmpvar_12;
          tmpvar_12 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
          float3 tmpvar_13;
          tmpvar_13.x = worldTangent_2.x;
          tmpvar_13.y = tmpvar_12.x;
          tmpvar_13.z = worldNormal_3.x;
          float3 tmpvar_14;
          tmpvar_14.x = worldTangent_2.y;
          tmpvar_14.y = tmpvar_12.y;
          tmpvar_14.z = worldNormal_3.y;
          float3 tmpvar_15;
          tmpvar_15.x = worldTangent_2.z;
          tmpvar_15.y = tmpvar_12.z;
          tmpvar_15.z = worldNormal_3.z;
          out_v.vertex = UnityObjectToClipPos(tmpvar_6);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_5;
          out_v.xlv_TEXCOORD2 = tmpvar_13;
          out_v.xlv_TEXCOORD3 = tmpvar_14;
          out_v.xlv_TEXCOORD4 = tmpvar_15;
          out_v.xlv_TEXCOORD5 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float xlat_mutable_Opacity;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float3 worldN_3;
          float4 c_4;
          float3 tmpvar_5;
          float tmpvar_6;
          float3 lightDir_7;
          float3 tmpvar_8;
          tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD5));
          lightDir_7 = tmpvar_8;
          tmpvar_5 = float3(0, 0, 0);
          tmpvar_6 = 0;
          float3 tmpvar_9;
          float tmpvar_10;
          tmpvar_9 = tmpvar_5;
          tmpvar_10 = tmpvar_6;
          float4 norm2_11;
          float4 norm_12;
          float4 dest_13;
          float4 tex2_14;
          float4 tex_15;
          float4 tmpvar_16;
          tmpvar_16 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          tex_15 = tmpvar_16;
          float4 tmpvar_17;
          tmpvar_17 = tex2D(_MainTex2, in_f.xlv_TEXCOORD1.xy);
          tex2_14 = tmpvar_17;
          xlat_mutable_Opacity = (_Opacity * tex_15.w);
          float3 tmpvar_18;
          tmpvar_18 = bool3(tex2_14.xyz <= float3(0.5, 0.5, 0.5));
          float3 b_19;
          b_19 = ((2 * tex_15.xyz) * tex2_14.xyz);
          float3 c_20;
          c_20 = (1 - ((2 * (1 - tex_15.xyz)) * (1 - tex2_14.xyz)));
          float tmpvar_21;
          if(tmpvar_18.x)
          {
              tmpvar_21 = b_19.x;
          }
          else
          {
              tmpvar_21 = c_20.x;
          }
          float tmpvar_22;
          if(tmpvar_18.y)
          {
              tmpvar_22 = b_19.y;
          }
          else
          {
              tmpvar_22 = c_20.y;
          }
          float tmpvar_23;
          if(tmpvar_18.z)
          {
              tmpvar_23 = b_19.z;
          }
          else
          {
              tmpvar_23 = c_20.z;
          }
          float3 tmpvar_24;
          tmpvar_24.x = tmpvar_21;
          tmpvar_24.y = tmpvar_22;
          tmpvar_24.z = tmpvar_23;
          dest_13.xyz = lerp(tex2_14.xyz, tmpvar_24, float3(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_13.xyz = (dest_13.xyz * _Color.xyz);
          tmpvar_9 = dest_13.xyz;
          tmpvar_10 = (tex2_14.w * _Color.w);
          float4 tmpvar_25;
          tmpvar_25 = tex2D(_BumpMap, in_f.xlv_TEXCOORD0.zw);
          norm_12 = tmpvar_25;
          float4 tmpvar_26;
          tmpvar_26 = tex2D(_BumpMap2, in_f.xlv_TEXCOORD1.zw);
          norm2_11 = tmpvar_26;
          float4 tmpvar_27;
          tmpvar_27 = bool4(norm2_11 <= float4(0.5, 0.5, 0.5, 0.5));
          float4 b_28;
          float _tmp_dvx_29 = ((2 * norm_12) * norm2_11);
          b_28 = float4(_tmp_dvx_29, _tmp_dvx_29, _tmp_dvx_29, _tmp_dvx_29);
          float4 c_29;
          float _tmp_dvx_30 = (1 - ((2 * (1 - norm_12)) * (1 - norm2_11)));
          c_29 = float4(_tmp_dvx_30, _tmp_dvx_30, _tmp_dvx_30, _tmp_dvx_30);
          float tmpvar_30;
          if(tmpvar_27.x)
          {
              tmpvar_30 = b_28.x;
          }
          else
          {
              tmpvar_30 = c_29.x;
          }
          float tmpvar_31;
          if(tmpvar_27.y)
          {
              tmpvar_31 = b_28.y;
          }
          else
          {
              tmpvar_31 = c_29.y;
          }
          float tmpvar_32;
          if(tmpvar_27.z)
          {
              tmpvar_32 = b_28.z;
          }
          else
          {
              tmpvar_32 = c_29.z;
          }
          float tmpvar_33;
          if(tmpvar_27.w)
          {
              tmpvar_33 = b_28.w;
          }
          else
          {
              tmpvar_33 = c_29.w;
          }
          float4 tmpvar_34;
          tmpvar_34.x = tmpvar_30;
          tmpvar_34.y = tmpvar_31;
          tmpvar_34.z = tmpvar_32;
          tmpvar_34.w = tmpvar_33;
          float4 tmpvar_35;
          tmpvar_35 = lerp(norm2_11, tmpvar_34, float4(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_13 = tmpvar_35;
          float3 tmpvar_36;
          float4 packednormal_37;
          packednormal_37 = tmpvar_35;
          tmpvar_36 = ((packednormal_37.xyz * 2) - 1);
          tmpvar_5 = tmpvar_9;
          tmpvar_6 = tmpvar_10;
          float4 tmpvar_38;
          tmpvar_38.w = 1;
          tmpvar_38.xyz = in_f.xlv_TEXCOORD5;
          float3 tmpvar_39;
          tmpvar_39 = mul(unity_WorldToLight, tmpvar_38).xyz;
          float tmpvar_40;
          tmpvar_40 = dot(tmpvar_39, tmpvar_39);
          float tmpvar_41;
          tmpvar_41 = tex2D(_LightTexture0, float2(tmpvar_40, tmpvar_40)).w;
          worldN_3.x = dot(in_f.xlv_TEXCOORD2, tmpvar_36);
          worldN_3.y = dot(in_f.xlv_TEXCOORD3, tmpvar_36);
          worldN_3.z = dot(in_f.xlv_TEXCOORD4, tmpvar_36);
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_7;
          tmpvar_1 = (tmpvar_1 * tmpvar_41);
          float4 c_42;
          float4 c_43;
          float diff_44;
          float tmpvar_45;
          tmpvar_45 = max(0, dot(worldN_3, tmpvar_2));
          diff_44 = tmpvar_45;
          c_43.xyz = float3(((tmpvar_9 * tmpvar_1) * diff_44));
          c_43.w = tmpvar_10;
          c_42.w = c_43.w;
          c_42.xyz = c_43.xyz;
          c_4.xyz = c_42.xyz;
          c_4.w = 1;
          out_f.color = c_4;
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
      LOD 400
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
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _MainTex_ST;
      uniform float4 _BumpMap_ST;
      uniform float4 _BumpMap2_ST;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex2;
      uniform sampler2D _BumpMap2;
      uniform float4 _Color;
      uniform float _Opacity;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float tangentSign_1;
          float3 worldTangent_2;
          float3 worldNormal_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = in_v.vertex.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          float3 tmpvar_6;
          tmpvar_6 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_7;
          tmpvar_7[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_7[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_7[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_8;
          tmpvar_8 = normalize(mul(in_v.normal, tmpvar_7));
          worldNormal_3 = tmpvar_8;
          float3x3 tmpvar_9;
          tmpvar_9[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_9[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_9[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_10;
          tmpvar_10 = normalize(mul(tmpvar_9, in_v.tangent.xyz));
          worldTangent_2 = tmpvar_10;
          float tmpvar_11;
          tmpvar_11 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_1 = tmpvar_11;
          float3 tmpvar_12;
          tmpvar_12 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
          float4 tmpvar_13;
          tmpvar_13.x = worldTangent_2.x;
          tmpvar_13.y = tmpvar_12.x;
          tmpvar_13.z = worldNormal_3.x;
          tmpvar_13.w = tmpvar_6.x;
          float4 tmpvar_14;
          tmpvar_14.x = worldTangent_2.y;
          tmpvar_14.y = tmpvar_12.y;
          tmpvar_14.z = worldNormal_3.y;
          tmpvar_14.w = tmpvar_6.y;
          float4 tmpvar_15;
          tmpvar_15.x = worldTangent_2.z;
          tmpvar_15.y = tmpvar_12.z;
          tmpvar_15.z = worldNormal_3.z;
          tmpvar_15.w = tmpvar_6.z;
          out_v.vertex = UnityObjectToClipPos(tmpvar_5);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap2);
          out_v.xlv_TEXCOORD2 = tmpvar_13;
          out_v.xlv_TEXCOORD3 = tmpvar_14;
          out_v.xlv_TEXCOORD4 = tmpvar_15;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float xlat_mutable_Opacity;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 res_1;
          float3 worldN_2;
          float3 tmpvar_3;
          float tmpvar_4;
          float2 tmpvar_5;
          tmpvar_5.x = 1;
          tmpvar_3 = float3(0, 0, 0);
          tmpvar_4 = 0;
          float3 tmpvar_6;
          float tmpvar_7;
          tmpvar_6 = tmpvar_3;
          tmpvar_7 = tmpvar_4;
          float4 norm2_8;
          float4 norm_9;
          float4 dest_10;
          float4 tex2_11;
          float4 tex_12;
          float4 tmpvar_13;
          tmpvar_13 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          tex_12 = tmpvar_13;
          float4 tmpvar_14;
          tmpvar_14 = tex2D(_MainTex2, tmpvar_5);
          tex2_11 = tmpvar_14;
          xlat_mutable_Opacity = (_Opacity * tex_12.w);
          float3 tmpvar_15;
          tmpvar_15 = bool3(tex2_11.xyz <= float3(0.5, 0.5, 0.5));
          float3 b_16;
          b_16 = ((2 * tex_12.xyz) * tex2_11.xyz);
          float3 c_17;
          c_17 = (1 - ((2 * (1 - tex_12.xyz)) * (1 - tex2_11.xyz)));
          float tmpvar_18;
          if(tmpvar_15.x)
          {
              tmpvar_18 = b_16.x;
          }
          else
          {
              tmpvar_18 = c_17.x;
          }
          float tmpvar_19;
          if(tmpvar_15.y)
          {
              tmpvar_19 = b_16.y;
          }
          else
          {
              tmpvar_19 = c_17.y;
          }
          float tmpvar_20;
          if(tmpvar_15.z)
          {
              tmpvar_20 = b_16.z;
          }
          else
          {
              tmpvar_20 = c_17.z;
          }
          float3 tmpvar_21;
          tmpvar_21.x = tmpvar_18;
          tmpvar_21.y = tmpvar_19;
          tmpvar_21.z = tmpvar_20;
          dest_10.xyz = lerp(tex2_11.xyz, tmpvar_21, float3(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_10.xyz = (dest_10.xyz * _Color.xyz);
          tmpvar_6 = dest_10.xyz;
          tmpvar_7 = (tex2_11.w * _Color.w);
          float4 tmpvar_22;
          tmpvar_22 = tex2D(_BumpMap, in_f.xlv_TEXCOORD0.zw);
          norm_9 = tmpvar_22;
          float4 tmpvar_23;
          tmpvar_23 = tex2D(_BumpMap2, in_f.xlv_TEXCOORD1);
          norm2_8 = tmpvar_23;
          float4 tmpvar_24;
          tmpvar_24 = bool4(norm2_8 <= float4(0.5, 0.5, 0.5, 0.5));
          float4 b_25;
          float _tmp_dvx_31 = ((2 * norm_9) * norm2_8);
          b_25 = float4(_tmp_dvx_31, _tmp_dvx_31, _tmp_dvx_31, _tmp_dvx_31);
          float4 c_26;
          float _tmp_dvx_32 = (1 - ((2 * (1 - norm_9)) * (1 - norm2_8)));
          c_26 = float4(_tmp_dvx_32, _tmp_dvx_32, _tmp_dvx_32, _tmp_dvx_32);
          float tmpvar_27;
          if(tmpvar_24.x)
          {
              tmpvar_27 = b_25.x;
          }
          else
          {
              tmpvar_27 = c_26.x;
          }
          float tmpvar_28;
          if(tmpvar_24.y)
          {
              tmpvar_28 = b_25.y;
          }
          else
          {
              tmpvar_28 = c_26.y;
          }
          float tmpvar_29;
          if(tmpvar_24.z)
          {
              tmpvar_29 = b_25.z;
          }
          else
          {
              tmpvar_29 = c_26.z;
          }
          float tmpvar_30;
          if(tmpvar_24.w)
          {
              tmpvar_30 = b_25.w;
          }
          else
          {
              tmpvar_30 = c_26.w;
          }
          float4 tmpvar_31;
          tmpvar_31.x = tmpvar_27;
          tmpvar_31.y = tmpvar_28;
          tmpvar_31.z = tmpvar_29;
          tmpvar_31.w = tmpvar_30;
          float4 tmpvar_32;
          tmpvar_32 = lerp(norm2_8, tmpvar_31, float4(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_10 = tmpvar_32;
          float3 tmpvar_33;
          float4 packednormal_34;
          packednormal_34 = tmpvar_32;
          tmpvar_33 = ((packednormal_34.xyz * 2) - 1);
          tmpvar_3 = tmpvar_6;
          tmpvar_4 = tmpvar_7;
          float tmpvar_35;
          tmpvar_35 = dot(in_f.xlv_TEXCOORD2.xyz, tmpvar_33);
          worldN_2.x = tmpvar_35;
          float tmpvar_36;
          tmpvar_36 = dot(in_f.xlv_TEXCOORD3.xyz, tmpvar_33);
          worldN_2.y = tmpvar_36;
          float tmpvar_37;
          tmpvar_37 = dot(in_f.xlv_TEXCOORD4.xyz, tmpvar_33);
          worldN_2.z = tmpvar_37;
          res_1.xyz = float3(((worldN_2 * 0.5) + 0.5));
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
      LOD 400
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
      uniform float4 _MainTex2_ST;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex2;
      uniform sampler2D _BumpMap2;
      uniform float4 _Color;
      uniform float _Opacity;
      uniform sampler2D _LightBuffer;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
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
          float4 tmpvar_2;
          float3 tmpvar_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = in_v.vertex.xyz;
          tmpvar_4 = UnityObjectToClipPos(tmpvar_5);
          tmpvar_1.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          tmpvar_1.zw = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex2);
          float4 o_6;
          float4 tmpvar_7;
          tmpvar_7 = (tmpvar_4 * 0.5);
          float2 tmpvar_8;
          tmpvar_8.x = tmpvar_7.x;
          tmpvar_8.y = (tmpvar_7.y * _ProjectionParams.x);
          o_6.xy = (tmpvar_8 + tmpvar_7.w);
          o_6.zw = tmpvar_4.zw;
          tmpvar_2.zw = float2(0, 0);
          tmpvar_2.xy = float2(0, 0);
          float3x3 tmpvar_9;
          tmpvar_9[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_9[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_9[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float4 tmpvar_10;
          tmpvar_10.w = 1;
          tmpvar_10.xyz = float3(normalize(mul(in_v.normal, tmpvar_9)));
          float4 normal_11;
          normal_11 = tmpvar_10;
          float3 res_12;
          float3 x_13;
          x_13.x = dot(unity_SHAr, normal_11);
          x_13.y = dot(unity_SHAg, normal_11);
          x_13.z = dot(unity_SHAb, normal_11);
          float3 x1_14;
          float4 tmpvar_15;
          tmpvar_15 = (normal_11.xyzz * normal_11.yzzx);
          x1_14.x = dot(unity_SHBr, tmpvar_15);
          x1_14.y = dot(unity_SHBg, tmpvar_15);
          x1_14.z = dot(unity_SHBb, tmpvar_15);
          res_12 = (x_13 + (x1_14 + (unity_SHC.xyz * ((normal_11.x * normal_11.x) - (normal_11.y * normal_11.y)))));
          float3 tmpvar_16;
          float _tmp_dvx_33 = max(((1.055 * pow(max(res_12, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_16 = float3(_tmp_dvx_33, _tmp_dvx_33, _tmp_dvx_33);
          res_12 = tmpvar_16;
          tmpvar_3 = tmpvar_16;
          out_v.vertex = tmpvar_4;
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_TEXCOORD2 = o_6;
          out_v.xlv_TEXCOORD3 = tmpvar_2;
          out_v.xlv_TEXCOORD4 = tmpvar_3;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float xlat_mutable_Opacity;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 c_2;
          float4 light_3;
          float3 tmpvar_4;
          float tmpvar_5;
          float2 tmpvar_6;
          float2 tmpvar_7;
          tmpvar_6.x = 1;
          tmpvar_7.x = 1;
          tmpvar_4 = float3(0, 0, 0);
          tmpvar_5 = 0;
          float3 tmpvar_8;
          float tmpvar_9;
          tmpvar_8 = tmpvar_4;
          tmpvar_9 = tmpvar_5;
          float4 norm2_10;
          float4 norm_11;
          float4 dest_12;
          float4 tex2_13;
          float4 tex_14;
          float4 tmpvar_15;
          tmpvar_15 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          tex_14 = tmpvar_15;
          float4 tmpvar_16;
          tmpvar_16 = tex2D(_MainTex2, in_f.xlv_TEXCOORD0.zw);
          tex2_13 = tmpvar_16;
          xlat_mutable_Opacity = (_Opacity * tex_14.w);
          float3 tmpvar_17;
          tmpvar_17 = bool3(tex2_13.xyz <= float3(0.5, 0.5, 0.5));
          float3 b_18;
          b_18 = ((2 * tex_14.xyz) * tex2_13.xyz);
          float3 c_19;
          c_19 = (1 - ((2 * (1 - tex_14.xyz)) * (1 - tex2_13.xyz)));
          float tmpvar_20;
          if(tmpvar_17.x)
          {
              tmpvar_20 = b_18.x;
          }
          else
          {
              tmpvar_20 = c_19.x;
          }
          float tmpvar_21;
          if(tmpvar_17.y)
          {
              tmpvar_21 = b_18.y;
          }
          else
          {
              tmpvar_21 = c_19.y;
          }
          float tmpvar_22;
          if(tmpvar_17.z)
          {
              tmpvar_22 = b_18.z;
          }
          else
          {
              tmpvar_22 = c_19.z;
          }
          float3 tmpvar_23;
          tmpvar_23.x = tmpvar_20;
          tmpvar_23.y = tmpvar_21;
          tmpvar_23.z = tmpvar_22;
          dest_12.xyz = lerp(tex2_13.xyz, tmpvar_23, float3(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_12.xyz = (dest_12.xyz * _Color.xyz);
          tmpvar_8 = dest_12.xyz;
          tmpvar_9 = (tex2_13.w * _Color.w);
          float4 tmpvar_24;
          tmpvar_24 = tex2D(_BumpMap, tmpvar_6);
          norm_11 = tmpvar_24;
          float4 tmpvar_25;
          tmpvar_25 = tex2D(_BumpMap2, tmpvar_7);
          norm2_10 = tmpvar_25;
          float4 tmpvar_26;
          tmpvar_26 = bool4(norm2_10 <= float4(0.5, 0.5, 0.5, 0.5));
          float4 b_27;
          float _tmp_dvx_34 = ((2 * norm_11) * norm2_10);
          b_27 = float4(_tmp_dvx_34, _tmp_dvx_34, _tmp_dvx_34, _tmp_dvx_34);
          float4 c_28;
          float _tmp_dvx_35 = (1 - ((2 * (1 - norm_11)) * (1 - norm2_10)));
          c_28 = float4(_tmp_dvx_35, _tmp_dvx_35, _tmp_dvx_35, _tmp_dvx_35);
          float tmpvar_29;
          if(tmpvar_26.x)
          {
              tmpvar_29 = b_27.x;
          }
          else
          {
              tmpvar_29 = c_28.x;
          }
          float tmpvar_30;
          if(tmpvar_26.y)
          {
              tmpvar_30 = b_27.y;
          }
          else
          {
              tmpvar_30 = c_28.y;
          }
          float tmpvar_31;
          if(tmpvar_26.z)
          {
              tmpvar_31 = b_27.z;
          }
          else
          {
              tmpvar_31 = c_28.z;
          }
          float tmpvar_32;
          if(tmpvar_26.w)
          {
              tmpvar_32 = b_27.w;
          }
          else
          {
              tmpvar_32 = c_28.w;
          }
          float4 tmpvar_33;
          tmpvar_33.x = tmpvar_29;
          tmpvar_33.y = tmpvar_30;
          tmpvar_33.z = tmpvar_31;
          tmpvar_33.w = tmpvar_32;
          dest_12 = lerp(norm2_10, tmpvar_33, float4(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          tmpvar_4 = tmpvar_8;
          tmpvar_5 = tmpvar_9;
          float4 tmpvar_34;
          tmpvar_34 = tex2D(_LightBuffer, in_f.xlv_TEXCOORD2);
          light_3 = tmpvar_34;
          light_3 = (-log2(max(light_3, float4(0.001, 0.001, 0.001, 0.001))));
          light_3.xyz = (light_3.xyz + in_f.xlv_TEXCOORD4);
          float4 c_35;
          c_35.xyz = (tmpvar_8 * light_3.xyz);
          c_35.w = tmpvar_9;
          c_2.xyz = c_35.xyz;
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
      LOD 400
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
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _MainTex_ST;
      uniform float4 _BumpMap_ST;
      uniform float4 _MainTex2_ST;
      uniform float4 _BumpMap2_ST;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex2;
      uniform sampler2D _BumpMap2;
      uniform float4 _Color;
      uniform float _Opacity;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_TEXCOORD5 :TEXCOORD5;
          float3 xlv_TEXCOORD6 :TEXCOORD6;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD6 :TEXCOORD6;
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
          float tangentSign_1;
          float3 worldTangent_2;
          float3 worldNormal_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          float4 tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7.w = 1;
          tmpvar_7.xyz = in_v.vertex.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          tmpvar_5.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex2);
          tmpvar_5.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap2);
          float3 tmpvar_8;
          tmpvar_8 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_9;
          tmpvar_9[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_9[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_9[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_10;
          tmpvar_10 = normalize(mul(in_v.normal, tmpvar_9));
          worldNormal_3 = tmpvar_10;
          float3x3 tmpvar_11;
          tmpvar_11[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_11[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_11[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_12;
          tmpvar_12 = normalize(mul(tmpvar_11, in_v.tangent.xyz));
          worldTangent_2 = tmpvar_12;
          float tmpvar_13;
          tmpvar_13 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_1 = tmpvar_13;
          float3 tmpvar_14;
          tmpvar_14 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
          float4 tmpvar_15;
          tmpvar_15.x = worldTangent_2.x;
          tmpvar_15.y = tmpvar_14.x;
          tmpvar_15.z = worldNormal_3.x;
          tmpvar_15.w = tmpvar_8.x;
          float4 tmpvar_16;
          tmpvar_16.x = worldTangent_2.y;
          tmpvar_16.y = tmpvar_14.y;
          tmpvar_16.z = worldNormal_3.y;
          tmpvar_16.w = tmpvar_8.y;
          float4 tmpvar_17;
          tmpvar_17.x = worldTangent_2.z;
          tmpvar_17.y = tmpvar_14.z;
          tmpvar_17.z = worldNormal_3.z;
          tmpvar_17.w = tmpvar_8.z;
          tmpvar_6.zw = float2(0, 0);
          tmpvar_6.xy = float2(0, 0);
          float3 normal_18;
          normal_18 = worldNormal_3;
          float4 tmpvar_19;
          tmpvar_19.w = 1;
          tmpvar_19.xyz = float3(normal_18);
          float3 res_20;
          float3 x_21;
          x_21.x = dot(unity_SHAr, tmpvar_19);
          x_21.y = dot(unity_SHAg, tmpvar_19);
          x_21.z = dot(unity_SHAb, tmpvar_19);
          float3 x1_22;
          float4 tmpvar_23;
          tmpvar_23 = (normal_18.xyzz * normal_18.yzzx);
          x1_22.x = dot(unity_SHBr, tmpvar_23);
          x1_22.y = dot(unity_SHBg, tmpvar_23);
          x1_22.z = dot(unity_SHBb, tmpvar_23);
          res_20 = (x_21 + (x1_22 + (unity_SHC.xyz * ((normal_18.x * normal_18.x) - (normal_18.y * normal_18.y)))));
          float3 tmpvar_24;
          float _tmp_dvx_36 = max(((1.055 * pow(max(res_20, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_24 = float3(_tmp_dvx_36, _tmp_dvx_36, _tmp_dvx_36);
          res_20 = tmpvar_24;
          out_v.vertex = UnityObjectToClipPos(tmpvar_7);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_5;
          out_v.xlv_TEXCOORD2 = tmpvar_15;
          out_v.xlv_TEXCOORD3 = tmpvar_16;
          out_v.xlv_TEXCOORD4 = tmpvar_17;
          out_v.xlv_TEXCOORD5 = tmpvar_6;
          out_v.xlv_TEXCOORD6 = max(float3(0, 0, 0), tmpvar_24);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float xlat_mutable_Opacity;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 outEmission_1;
          float3 worldN_2;
          float3 tmpvar_3;
          float tmpvar_4;
          tmpvar_3 = float3(0, 0, 0);
          tmpvar_4 = 0;
          float3 tmpvar_5;
          float tmpvar_6;
          tmpvar_5 = tmpvar_3;
          tmpvar_6 = tmpvar_4;
          float4 norm2_7;
          float4 norm_8;
          float4 dest_9;
          float4 tex2_10;
          float4 tex_11;
          float4 tmpvar_12;
          tmpvar_12 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          tex_11 = tmpvar_12;
          float4 tmpvar_13;
          tmpvar_13 = tex2D(_MainTex2, in_f.xlv_TEXCOORD1.xy);
          tex2_10 = tmpvar_13;
          xlat_mutable_Opacity = (_Opacity * tex_11.w);
          float3 tmpvar_14;
          tmpvar_14 = bool3(tex2_10.xyz <= float3(0.5, 0.5, 0.5));
          float3 b_15;
          b_15 = ((2 * tex_11.xyz) * tex2_10.xyz);
          float3 c_16;
          c_16 = (1 - ((2 * (1 - tex_11.xyz)) * (1 - tex2_10.xyz)));
          float tmpvar_17;
          if(tmpvar_14.x)
          {
              tmpvar_17 = b_15.x;
          }
          else
          {
              tmpvar_17 = c_16.x;
          }
          float tmpvar_18;
          if(tmpvar_14.y)
          {
              tmpvar_18 = b_15.y;
          }
          else
          {
              tmpvar_18 = c_16.y;
          }
          float tmpvar_19;
          if(tmpvar_14.z)
          {
              tmpvar_19 = b_15.z;
          }
          else
          {
              tmpvar_19 = c_16.z;
          }
          float3 tmpvar_20;
          tmpvar_20.x = tmpvar_17;
          tmpvar_20.y = tmpvar_18;
          tmpvar_20.z = tmpvar_19;
          dest_9.xyz = lerp(tex2_10.xyz, tmpvar_20, float3(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_9.xyz = (dest_9.xyz * _Color.xyz);
          tmpvar_5 = dest_9.xyz;
          tmpvar_6 = (tex2_10.w * _Color.w);
          float4 tmpvar_21;
          tmpvar_21 = tex2D(_BumpMap, in_f.xlv_TEXCOORD0.zw);
          norm_8 = tmpvar_21;
          float4 tmpvar_22;
          tmpvar_22 = tex2D(_BumpMap2, in_f.xlv_TEXCOORD1.zw);
          norm2_7 = tmpvar_22;
          float4 tmpvar_23;
          tmpvar_23 = bool4(norm2_7 <= float4(0.5, 0.5, 0.5, 0.5));
          float4 b_24;
          float _tmp_dvx_37 = ((2 * norm_8) * norm2_7);
          b_24 = float4(_tmp_dvx_37, _tmp_dvx_37, _tmp_dvx_37, _tmp_dvx_37);
          float4 c_25;
          float _tmp_dvx_38 = (1 - ((2 * (1 - norm_8)) * (1 - norm2_7)));
          c_25 = float4(_tmp_dvx_38, _tmp_dvx_38, _tmp_dvx_38, _tmp_dvx_38);
          float tmpvar_26;
          if(tmpvar_23.x)
          {
              tmpvar_26 = b_24.x;
          }
          else
          {
              tmpvar_26 = c_25.x;
          }
          float tmpvar_27;
          if(tmpvar_23.y)
          {
              tmpvar_27 = b_24.y;
          }
          else
          {
              tmpvar_27 = c_25.y;
          }
          float tmpvar_28;
          if(tmpvar_23.z)
          {
              tmpvar_28 = b_24.z;
          }
          else
          {
              tmpvar_28 = c_25.z;
          }
          float tmpvar_29;
          if(tmpvar_23.w)
          {
              tmpvar_29 = b_24.w;
          }
          else
          {
              tmpvar_29 = c_25.w;
          }
          float4 tmpvar_30;
          tmpvar_30.x = tmpvar_26;
          tmpvar_30.y = tmpvar_27;
          tmpvar_30.z = tmpvar_28;
          tmpvar_30.w = tmpvar_29;
          float4 tmpvar_31;
          tmpvar_31 = lerp(norm2_7, tmpvar_30, float4(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_9 = tmpvar_31;
          float3 tmpvar_32;
          float4 packednormal_33;
          packednormal_33 = tmpvar_31;
          tmpvar_32 = ((packednormal_33.xyz * 2) - 1);
          tmpvar_3 = tmpvar_5;
          tmpvar_4 = tmpvar_6;
          float tmpvar_34;
          tmpvar_34 = dot(in_f.xlv_TEXCOORD2.xyz, tmpvar_32);
          worldN_2.x = tmpvar_34;
          float tmpvar_35;
          tmpvar_35 = dot(in_f.xlv_TEXCOORD3.xyz, tmpvar_32);
          worldN_2.y = tmpvar_35;
          float tmpvar_36;
          tmpvar_36 = dot(in_f.xlv_TEXCOORD4.xyz, tmpvar_32);
          worldN_2.z = tmpvar_36;
          float4 emission_37;
          float3 tmpvar_38;
          float3 tmpvar_39;
          tmpvar_38 = tmpvar_5;
          tmpvar_39 = worldN_2;
          float4 tmpvar_40;
          tmpvar_40.xyz = float3(tmpvar_38);
          tmpvar_40.w = 1;
          float4 tmpvar_41;
          tmpvar_41.xyz = float3(0, 0, 0);
          tmpvar_41.w = 0;
          float4 tmpvar_42;
          tmpvar_42.w = 1;
          tmpvar_42.xyz = float3(((tmpvar_39 * 0.5) + 0.5));
          float4 tmpvar_43;
          tmpvar_43.w = 1;
          tmpvar_43.xyz = float3(0, 0, 0);
          emission_37 = tmpvar_43;
          emission_37.xyz = (emission_37.xyz + (tmpvar_5 * in_f.xlv_TEXCOORD6));
          outEmission_1.w = emission_37.w;
          outEmission_1.xyz = exp2((-emission_37.xyz));
          out_f.color = tmpvar_40;
          out_f.color1 = tmpvar_41;
          out_f.color2 = tmpvar_42;
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
      LOD 400
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
      //uniform float4 unity_WorldTransformParams;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      uniform float4 _MainTex2_ST;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _MainTex2;
      uniform sampler2D _BumpMap2;
      uniform float4 _Color;
      uniform float _Opacity;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float tangentSign_1;
          float3 worldTangent_2;
          float3 worldNormal_3;
          float4 tmpvar_4;
          float4 vertex_5;
          vertex_5 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_5.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_6;
              if((in_v.vertex.z>0))
              {
                  tmpvar_6 = 0.0001;
              }
              else
              {
                  tmpvar_6 = 0;
              }
              vertex_5.z = tmpvar_6;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_5.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_7;
              if((vertex_5.z>0))
              {
                  tmpvar_7 = 0.0001;
              }
              else
              {
                  tmpvar_7 = 0;
              }
              vertex_5.z = tmpvar_7;
          }
          float4 tmpvar_8;
          tmpvar_8.w = 1;
          tmpvar_8.xyz = vertex_5.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex2);
          float3 tmpvar_9;
          tmpvar_9 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_10;
          tmpvar_10[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_10[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_10[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_11;
          tmpvar_11 = normalize(mul(in_v.normal, tmpvar_10));
          worldNormal_3 = tmpvar_11;
          float3x3 tmpvar_12;
          tmpvar_12[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_12[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_12[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_13;
          tmpvar_13 = normalize(mul(tmpvar_12, in_v.tangent.xyz));
          worldTangent_2 = tmpvar_13;
          float tmpvar_14;
          tmpvar_14 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_1 = tmpvar_14;
          float3 tmpvar_15;
          tmpvar_15 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
          float4 tmpvar_16;
          tmpvar_16.x = worldTangent_2.x;
          tmpvar_16.y = tmpvar_15.x;
          tmpvar_16.z = worldNormal_3.x;
          tmpvar_16.w = tmpvar_9.x;
          float4 tmpvar_17;
          tmpvar_17.x = worldTangent_2.y;
          tmpvar_17.y = tmpvar_15.y;
          tmpvar_17.z = worldNormal_3.y;
          tmpvar_17.w = tmpvar_9.y;
          float4 tmpvar_18;
          tmpvar_18.x = worldTangent_2.z;
          tmpvar_18.y = tmpvar_15.z;
          tmpvar_18.z = worldNormal_3.z;
          tmpvar_18.w = tmpvar_9.z;
          out_v.vertex = UnityObjectToClipPos(tmpvar_8);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_16;
          out_v.xlv_TEXCOORD2 = tmpvar_17;
          out_v.xlv_TEXCOORD3 = tmpvar_18;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      float xlat_mutable_Opacity;
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          float tmpvar_4;
          float2 tmpvar_5;
          float2 tmpvar_6;
          tmpvar_5.x = 1;
          tmpvar_6.x = 1;
          tmpvar_3 = float3(0, 0, 0);
          tmpvar_4 = 0;
          float3 tmpvar_7;
          float tmpvar_8;
          tmpvar_7 = tmpvar_3;
          tmpvar_8 = tmpvar_4;
          float4 norm2_9;
          float4 norm_10;
          float4 dest_11;
          float4 tex2_12;
          float4 tex_13;
          float4 tmpvar_14;
          tmpvar_14 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          tex_13 = tmpvar_14;
          float4 tmpvar_15;
          tmpvar_15 = tex2D(_MainTex2, in_f.xlv_TEXCOORD0.zw);
          tex2_12 = tmpvar_15;
          xlat_mutable_Opacity = (_Opacity * tex_13.w);
          float3 tmpvar_16;
          tmpvar_16 = bool3(tex2_12.xyz <= float3(0.5, 0.5, 0.5));
          float3 b_17;
          b_17 = ((2 * tex_13.xyz) * tex2_12.xyz);
          float3 c_18;
          c_18 = (1 - ((2 * (1 - tex_13.xyz)) * (1 - tex2_12.xyz)));
          float tmpvar_19;
          if(tmpvar_16.x)
          {
              tmpvar_19 = b_17.x;
          }
          else
          {
              tmpvar_19 = c_18.x;
          }
          float tmpvar_20;
          if(tmpvar_16.y)
          {
              tmpvar_20 = b_17.y;
          }
          else
          {
              tmpvar_20 = c_18.y;
          }
          float tmpvar_21;
          if(tmpvar_16.z)
          {
              tmpvar_21 = b_17.z;
          }
          else
          {
              tmpvar_21 = c_18.z;
          }
          float3 tmpvar_22;
          tmpvar_22.x = tmpvar_19;
          tmpvar_22.y = tmpvar_20;
          tmpvar_22.z = tmpvar_21;
          dest_11.xyz = lerp(tex2_12.xyz, tmpvar_22, float3(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          dest_11.xyz = (dest_11.xyz * _Color.xyz);
          tmpvar_7 = dest_11.xyz;
          tmpvar_8 = (tex2_12.w * _Color.w);
          float4 tmpvar_23;
          tmpvar_23 = tex2D(_BumpMap, tmpvar_5);
          norm_10 = tmpvar_23;
          float4 tmpvar_24;
          tmpvar_24 = tex2D(_BumpMap2, tmpvar_6);
          norm2_9 = tmpvar_24;
          float4 tmpvar_25;
          tmpvar_25 = bool4(norm2_9 <= float4(0.5, 0.5, 0.5, 0.5));
          float4 b_26;
          float _tmp_dvx_39 = ((2 * norm_10) * norm2_9);
          b_26 = float4(_tmp_dvx_39, _tmp_dvx_39, _tmp_dvx_39, _tmp_dvx_39);
          float4 c_27;
          float _tmp_dvx_40 = (1 - ((2 * (1 - norm_10)) * (1 - norm2_9)));
          c_27 = float4(_tmp_dvx_40, _tmp_dvx_40, _tmp_dvx_40, _tmp_dvx_40);
          float tmpvar_28;
          if(tmpvar_25.x)
          {
              tmpvar_28 = b_26.x;
          }
          else
          {
              tmpvar_28 = c_27.x;
          }
          float tmpvar_29;
          if(tmpvar_25.y)
          {
              tmpvar_29 = b_26.y;
          }
          else
          {
              tmpvar_29 = c_27.y;
          }
          float tmpvar_30;
          if(tmpvar_25.z)
          {
              tmpvar_30 = b_26.z;
          }
          else
          {
              tmpvar_30 = c_27.z;
          }
          float tmpvar_31;
          if(tmpvar_25.w)
          {
              tmpvar_31 = b_26.w;
          }
          else
          {
              tmpvar_31 = c_27.w;
          }
          float4 tmpvar_32;
          tmpvar_32.x = tmpvar_28;
          tmpvar_32.y = tmpvar_29;
          tmpvar_32.z = tmpvar_30;
          tmpvar_32.w = tmpvar_31;
          dest_11 = lerp(norm2_9, tmpvar_32, float4(xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity, xlat_mutable_Opacity));
          tmpvar_3 = tmpvar_7;
          tmpvar_4 = tmpvar_8;
          tmpvar_2 = tmpvar_7;
          float4 res_33;
          res_33 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_34;
              tmpvar_34.w = 1;
              tmpvar_34.xyz = float3(tmpvar_2);
              res_33.w = tmpvar_34.w;
              float3 tmpvar_35;
              float _tmp_dvx_41 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_35 = clamp(pow(tmpvar_2, float3(_tmp_dvx_41, _tmp_dvx_41, _tmp_dvx_41)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_33.xyz = float3(tmpvar_35);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_36;
              if(int(unity_UseLinearSpace))
              {
                  emission_36 = float3(0, 0, 0);
              }
              else
              {
                  emission_36 = float3(0, 0, 0);
              }
              float4 tmpvar_37;
              float alpha_38;
              float3 tmpvar_39;
              tmpvar_39 = (emission_36 * 0.01030928);
              alpha_38 = (ceil((max(max(tmpvar_39.x, tmpvar_39.y), max(tmpvar_39.z, 0.02)) * 255)) / 255);
              float tmpvar_40;
              tmpvar_40 = max(alpha_38, 0.02);
              alpha_38 = tmpvar_40;
              float4 tmpvar_41;
              tmpvar_41.xyz = float3((tmpvar_39 / tmpvar_40));
              tmpvar_41.w = tmpvar_40;
              tmpvar_37 = tmpvar_41;
              res_33 = tmpvar_37;
          }
          tmpvar_1 = res_33;
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
      "RenderType" = "Opaque"
    }
    LOD 400
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "ForwardBase"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      LOD 400
      ZClip Off
      // m_ProgramMask = 6
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "ForwardAdd"
        "RenderType" = "Opaque"
      }
      LOD 400
      ZClip Off
      ZWrite Off
      Blend One One
      // m_ProgramMask = 6
      
    } // end phase
    Pass // ind: 3, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "LIGHTMODE" = "PrePassBase"
        "RenderType" = "Opaque"
      }
      LOD 400
      ZClip Off
      // m_ProgramMask = 6
      
    } // end phase
    Pass // ind: 4, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "LIGHTMODE" = "PrePassFinal"
        "RenderType" = "Opaque"
      }
      LOD 400
      ZClip Off
      ZWrite Off
      // m_ProgramMask = 6
      
    } // end phase
    Pass // ind: 5, name: DEFERRED
    {
      Name "DEFERRED"
      Tags
      { 
        "LIGHTMODE" = "Deferred"
        "RenderType" = "Opaque"
      }
      LOD 400
      ZClip Off
      // m_ProgramMask = 6
      
    } // end phase
    Pass // ind: 6, name: META
    {
      Name "META"
      Tags
      { 
        "LIGHTMODE" = "Meta"
        "RenderType" = "Opaque"
      }
      LOD 400
      ZClip Off
      Cull Off
      // m_ProgramMask = 6
      
    } // end phase
  }
  FallBack "Bumped Diffuse"
}
