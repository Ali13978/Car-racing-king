// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RunningWater"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
    _Shininess ("Shininess", Range(0.01, 1)) = 0.078125
    _Transparency ("Transparency", Range(-0.5, 0.5)) = 0.1
    _MaxWaterSpeed ("max water velocity", Range(-100, 100)) = 5
    _WaveSpeed ("wave velocity", Range(-10, 10)) = 1
    _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
    _Cube ("Reflection Cubemap", Cube) = "" {}
    _BumpMap ("Normalmap", 2D) = "bump" {}
    _SplashTex ("Splash Texture", 2D) = "Black" {}
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 400
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "ForwardBase"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 400
      ZClip Off
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
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
      //uniform float4 _Time;
      //uniform float4 unity_SHBr;
      //uniform float4 unity_SHBg;
      //uniform float4 unity_SHBb;
      //uniform float4 unity_SHC;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _BumpMap_ST;
      uniform float4 _SplashTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 unity_SHAr;
      //uniform float4 unity_SHAg;
      //uniform float4 unity_SHAb;
      uniform float4 _LightColor0;
      uniform float4 _SpecColor;
      uniform sampler2D _BumpMap;
      uniform sampler2D _SplashTex;
      uniform samplerCUBE _Cube;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float _MaxWaterSpeed;
      uniform float _WaveSpeed;
      uniform float _Shininess;
      uniform float _Transparency;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
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
          tmpvar_5.w = in_v.vertex.w;
          tmpvar_5.xyz = (in_v.vertex.xyz + ((in_v.normal * (sin(((_Time.x * 3.145) + (in_v.vertex.x * 50))) + sin(((_Time.x * 2.947) + (in_v.vertex.z * 50))))) * 0.004));
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = tmpvar_5.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _SplashTex);
          float3 tmpvar_7;
          tmpvar_7 = mul(unity_ObjectToWorld, tmpvar_5).xyz;
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
          float3 x1_18;
          float4 tmpvar_19;
          tmpvar_19 = (normal_17.xyzz * normal_17.yzzx);
          x1_18.x = dot(unity_SHBr, tmpvar_19);
          x1_18.y = dot(unity_SHBg, tmpvar_19);
          x1_18.z = dot(unity_SHBb, tmpvar_19);
          out_v.vertex = UnityObjectToClipPos(tmpvar_6);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_14;
          out_v.xlv_TEXCOORD2 = tmpvar_15;
          out_v.xlv_TEXCOORD3 = tmpvar_16;
          out_v.xlv_COLOR0 = in_v.color;
          out_v.xlv_TEXCOORD4 = (x1_18 + (unity_SHC.xyz * ((normal_17.x * normal_17.x) - (normal_17.y * normal_17.y))));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float3 worldN_3;
          float4 c_4;
          float3 worldViewDir_5;
          float3 lightDir_6;
          float3 tmpvar_7;
          float4 tmpvar_8;
          float3 tmpvar_9;
          float3 tmpvar_10;
          float3 tmpvar_11;
          float3 tmpvar_12;
          tmpvar_12.x = in_f.xlv_TEXCOORD1.w;
          tmpvar_12.y = in_f.xlv_TEXCOORD2.w;
          tmpvar_12.z = in_f.xlv_TEXCOORD3.w;
          float3 tmpvar_13;
          tmpvar_13 = _WorldSpaceLightPos0.xyz;
          lightDir_6 = tmpvar_13;
          float3 tmpvar_14;
          tmpvar_14 = normalize((_WorldSpaceCameraPos - tmpvar_12));
          worldViewDir_5 = tmpvar_14;
          tmpvar_7 = (-worldViewDir_5);
          tmpvar_9 = in_f.xlv_TEXCOORD1.xyz;
          tmpvar_10 = in_f.xlv_TEXCOORD2.xyz;
          tmpvar_11 = in_f.xlv_TEXCOORD3.xyz;
          tmpvar_8 = in_f.xlv_COLOR0;
          float3 tmpvar_15;
          float3 tmpvar_16;
          float tmpvar_17;
          float tmpvar_18;
          float tmpvar_19;
          float4 c_20;
          float4 fc_21;
          float3 norm2_22;
          float3 norm1_23;
          float tmpvar_24;
          tmpvar_24 = (_Time.x * _WaveSpeed);
          float2 tmpvar_25;
          tmpvar_25 = (in_f.xlv_TEXCOORD0.xy + float2(tmpvar_24, tmpvar_24));
          float2 tmpvar_26;
          tmpvar_26.x = (in_f.xlv_TEXCOORD0.x - tmpvar_24);
          tmpvar_26.y = ((in_f.xlv_TEXCOORD0.y + tmpvar_24) + 0.5);
          float3 tmpvar_27;
          tmpvar_27 = ((tex2D(_BumpMap, tmpvar_25).xyz * 2) - 1);
          norm1_23 = tmpvar_27;
          float3 tmpvar_28;
          tmpvar_28 = ((tex2D(_BumpMap, tmpvar_26).xyz * 2) - 1);
          norm2_22 = tmpvar_28;
          float3 tmpvar_29;
          tmpvar_29 = ((norm1_23 + norm2_22) * 0.5);
          float2 tmpvar_30;
          tmpvar_30.x = in_f.xlv_TEXCOORD0.z;
          float tmpvar_31;
          tmpvar_31 = (_Time.x * _MaxWaterSpeed);
          tmpvar_30.y = (in_f.xlv_TEXCOORD0.w + tmpvar_31);
          float2 tmpvar_32;
          tmpvar_32.x = (in_f.xlv_TEXCOORD0.z + 0.5);
          tmpvar_32.y = (in_f.xlv_TEXCOORD0.w + (tmpvar_31 * 0.5));
          fc_21 = ((tex2D(_SplashTex, tmpvar_30) + tex2D(_SplashTex, tmpvar_32)) * 0.5);
          float4 tmpvar_33;
          tmpvar_33 = ((_Color * (1 - tmpvar_8.x)) + (tmpvar_8.x * fc_21));
          c_20 = tmpvar_33;
          tmpvar_18 = (1 - tmpvar_8.x);
          tmpvar_17 = (_Shininess * (1 - tmpvar_8.x));
          tmpvar_15 = tmpvar_29;
          float3 tmpvar_34;
          tmpvar_34.x = dot(tmpvar_9, tmpvar_15);
          tmpvar_34.y = dot(tmpvar_10, tmpvar_15);
          tmpvar_34.z = dot(tmpvar_11, tmpvar_15);
          float3 tmpvar_35;
          tmpvar_35 = (tmpvar_7 - (2 * (dot(tmpvar_34, tmpvar_7) * tmpvar_34)));
          float4 tmpvar_36;
          float _tmp_dvx_23 = texCUBE(_Cube, tmpvar_35);
          tmpvar_36 = float4(_tmp_dvx_23, _tmp_dvx_23, _tmp_dvx_23, _tmp_dvx_23);
          tmpvar_16 = ((tmpvar_36.xyz * _ReflectColor.xyz) * (1 - tmpvar_8.x));
          tmpvar_19 = (((((tmpvar_36.w * _ReflectColor.w) + _Transparency) * (1 - tmpvar_8.x)) + (tmpvar_8.x * fc_21.w)) * tmpvar_8.w);
          float tmpvar_37;
          tmpvar_37 = dot(in_f.xlv_TEXCOORD1.xyz, tmpvar_15);
          worldN_3.x = tmpvar_37;
          float tmpvar_38;
          tmpvar_38 = dot(in_f.xlv_TEXCOORD2.xyz, tmpvar_15);
          worldN_3.y = tmpvar_38;
          float tmpvar_39;
          tmpvar_39 = dot(in_f.xlv_TEXCOORD3.xyz, tmpvar_15);
          worldN_3.z = tmpvar_39;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_6;
          float3 normalWorld_40;
          normalWorld_40 = worldN_3;
          float4 tmpvar_41;
          tmpvar_41.w = 1;
          tmpvar_41.xyz = float3(normalWorld_40);
          float3 x_42;
          x_42.x = dot(unity_SHAr, tmpvar_41);
          x_42.y = dot(unity_SHAg, tmpvar_41);
          x_42.z = dot(unity_SHAb, tmpvar_41);
          float3 tmpvar_43;
          tmpvar_43 = max(((1.055 * pow(max(float3(0, 0, 0), (in_f.xlv_TEXCOORD4 + x_42)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          float3 viewDir_44;
          viewDir_44 = worldViewDir_5;
          float4 c_45;
          float4 c_46;
          float nh_47;
          float diff_48;
          float tmpvar_49;
          tmpvar_49 = max(0, dot(worldN_3, tmpvar_2));
          diff_48 = tmpvar_49;
          float tmpvar_50;
          tmpvar_50 = max(0, dot(worldN_3, normalize((tmpvar_2 + viewDir_44))));
          nh_47 = tmpvar_50;
          float y_51;
          y_51 = (tmpvar_17 * 128);
          float tmpvar_52;
          tmpvar_52 = (pow(nh_47, y_51) * tmpvar_18);
          c_46.xyz = (((c_20.xyz * tmpvar_1) * diff_48) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_52));
          c_46.w = tmpvar_19;
          c_45.w = c_46.w;
          c_45.xyz = (c_46.xyz + (c_20.xyz * tmpvar_43));
          c_4.w = c_45.w;
          c_4.xyz = (c_45.xyz + tmpvar_16);
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
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "ForwardAdd"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 400
      ZClip Off
      ZWrite Off
      Cull Off
      Blend SrcAlpha One
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
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _Time;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _BumpMap_ST;
      uniform float4 _SplashTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform float4 _SpecColor;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform sampler2D _BumpMap;
      uniform sampler2D _SplashTex;
      uniform samplerCUBE _Cube;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float _MaxWaterSpeed;
      uniform float _WaveSpeed;
      uniform float _Shininess;
      uniform float _Transparency;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_COLOR0 :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_COLOR0 :COLOR0;
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
          tmpvar_5.w = in_v.vertex.w;
          tmpvar_5.xyz = (in_v.vertex.xyz + ((in_v.normal * (sin(((_Time.x * 3.145) + (in_v.vertex.x * 50))) + sin(((_Time.x * 2.947) + (in_v.vertex.z * 50))))) * 0.004));
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = tmpvar_5.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _SplashTex);
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
          out_v.xlv_TEXCOORD1 = tmpvar_13;
          out_v.xlv_TEXCOORD2 = tmpvar_14;
          out_v.xlv_TEXCOORD3 = tmpvar_15;
          out_v.xlv_TEXCOORD4 = mul(unity_ObjectToWorld, tmpvar_5).xyz;
          out_v.xlv_COLOR0 = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float3 worldN_3;
          float3 worldViewDir_4;
          float3 lightDir_5;
          float3 tmpvar_6;
          float4 tmpvar_7;
          float3 tmpvar_8;
          float3 tmpvar_9;
          float3 tmpvar_10;
          float3 tmpvar_11;
          tmpvar_11 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD4));
          lightDir_5 = tmpvar_11;
          float3 tmpvar_12;
          tmpvar_12 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD4));
          worldViewDir_4 = tmpvar_12;
          tmpvar_6 = (-worldViewDir_4);
          tmpvar_8 = in_f.xlv_TEXCOORD1;
          tmpvar_9 = in_f.xlv_TEXCOORD2;
          tmpvar_10 = in_f.xlv_TEXCOORD3;
          tmpvar_7 = in_f.xlv_COLOR0;
          float3 tmpvar_13;
          float tmpvar_14;
          float tmpvar_15;
          float tmpvar_16;
          float4 c_17;
          float4 fc_18;
          float3 norm2_19;
          float3 norm1_20;
          float tmpvar_21;
          tmpvar_21 = (_Time.x * _WaveSpeed);
          float2 tmpvar_22;
          tmpvar_22 = (in_f.xlv_TEXCOORD0.xy + float2(tmpvar_21, tmpvar_21));
          float2 tmpvar_23;
          tmpvar_23.x = (in_f.xlv_TEXCOORD0.x - tmpvar_21);
          tmpvar_23.y = ((in_f.xlv_TEXCOORD0.y + tmpvar_21) + 0.5);
          float3 tmpvar_24;
          tmpvar_24 = ((tex2D(_BumpMap, tmpvar_22).xyz * 2) - 1);
          norm1_20 = tmpvar_24;
          float3 tmpvar_25;
          tmpvar_25 = ((tex2D(_BumpMap, tmpvar_23).xyz * 2) - 1);
          norm2_19 = tmpvar_25;
          float3 tmpvar_26;
          tmpvar_26 = ((norm1_20 + norm2_19) * 0.5);
          float2 tmpvar_27;
          tmpvar_27.x = in_f.xlv_TEXCOORD0.z;
          float tmpvar_28;
          tmpvar_28 = (_Time.x * _MaxWaterSpeed);
          tmpvar_27.y = (in_f.xlv_TEXCOORD0.w + tmpvar_28);
          float2 tmpvar_29;
          tmpvar_29.x = (in_f.xlv_TEXCOORD0.z + 0.5);
          tmpvar_29.y = (in_f.xlv_TEXCOORD0.w + (tmpvar_28 * 0.5));
          fc_18 = ((tex2D(_SplashTex, tmpvar_27) + tex2D(_SplashTex, tmpvar_29)) * 0.5);
          float4 tmpvar_30;
          tmpvar_30 = ((_Color * (1 - tmpvar_7.x)) + (tmpvar_7.x * fc_18));
          c_17 = tmpvar_30;
          tmpvar_15 = (1 - tmpvar_7.x);
          tmpvar_14 = (_Shininess * (1 - tmpvar_7.x));
          tmpvar_13 = tmpvar_26;
          float3 tmpvar_31;
          tmpvar_31.x = dot(tmpvar_8, tmpvar_13);
          tmpvar_31.y = dot(tmpvar_9, tmpvar_13);
          tmpvar_31.z = dot(tmpvar_10, tmpvar_13);
          float3 tmpvar_32;
          tmpvar_32 = (tmpvar_6 - (2 * (dot(tmpvar_31, tmpvar_6) * tmpvar_31)));
          tmpvar_16 = (((((texCUBE(_Cube, tmpvar_32).w * _ReflectColor.w) + _Transparency) * (1 - tmpvar_7.x)) + (tmpvar_7.x * fc_18.w)) * tmpvar_7.w);
          float4 tmpvar_33;
          tmpvar_33.w = 1;
          tmpvar_33.xyz = in_f.xlv_TEXCOORD4;
          float3 tmpvar_34;
          tmpvar_34 = mul(unity_WorldToLight, tmpvar_33).xyz;
          float tmpvar_35;
          tmpvar_35 = dot(tmpvar_34, tmpvar_34);
          float tmpvar_36;
          tmpvar_36 = tex2D(_LightTexture0, float2(tmpvar_35, tmpvar_35)).w;
          worldN_3.x = dot(in_f.xlv_TEXCOORD1, tmpvar_13);
          worldN_3.y = dot(in_f.xlv_TEXCOORD2, tmpvar_13);
          worldN_3.z = dot(in_f.xlv_TEXCOORD3, tmpvar_13);
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_5;
          tmpvar_1 = (tmpvar_1 * tmpvar_36);
          float3 viewDir_37;
          viewDir_37 = worldViewDir_4;
          float4 c_38;
          float4 c_39;
          float nh_40;
          float diff_41;
          float tmpvar_42;
          tmpvar_42 = max(0, dot(worldN_3, tmpvar_2));
          diff_41 = tmpvar_42;
          float tmpvar_43;
          tmpvar_43 = max(0, dot(worldN_3, normalize((tmpvar_2 + viewDir_37))));
          nh_40 = tmpvar_43;
          float y_44;
          y_44 = (tmpvar_14 * 128);
          float tmpvar_45;
          tmpvar_45 = (pow(nh_40, y_44) * tmpvar_15);
          c_39.xyz = (((c_17.xyz * tmpvar_1) * diff_41) + ((tmpvar_1 * _SpecColor.xyz) * tmpvar_45));
          c_39.w = tmpvar_16;
          c_38.w = c_39.w;
          c_38.xyz = c_39.xyz;
          out_f.color = c_38;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: PREPASS
    {
      Name "PREPASS"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "PrePassBase"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 400
      ZClip Off
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
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
      //uniform float4 _Time;
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _BumpMap_ST;
      uniform float4 _SplashTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      uniform sampler2D _BumpMap;
      uniform sampler2D _SplashTex;
      uniform samplerCUBE _Cube;
      uniform float4 _ReflectColor;
      uniform float _MaxWaterSpeed;
      uniform float _WaveSpeed;
      uniform float _Transparency;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_COLOR0 :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_COLOR0 :COLOR0;
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
          tmpvar_5.w = in_v.vertex.w;
          tmpvar_5.xyz = (in_v.vertex.xyz + ((in_v.normal * (sin(((_Time.x * 3.145) + (in_v.vertex.x * 50))) + sin(((_Time.x * 2.947) + (in_v.vertex.z * 50))))) * 0.004));
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = tmpvar_5.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _SplashTex);
          float3 tmpvar_7;
          tmpvar_7 = mul(unity_ObjectToWorld, tmpvar_5).xyz;
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
          out_v.vertex = UnityObjectToClipPos(tmpvar_6);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_14;
          out_v.xlv_TEXCOORD2 = tmpvar_15;
          out_v.xlv_TEXCOORD3 = tmpvar_16;
          out_v.xlv_COLOR0 = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 res_1;
          float3 worldN_2;
          float3 worldViewDir_3;
          float3 tmpvar_4;
          float4 tmpvar_5;
          float3 tmpvar_6;
          float3 tmpvar_7;
          float3 tmpvar_8;
          float3 tmpvar_9;
          tmpvar_9.x = in_f.xlv_TEXCOORD1.w;
          tmpvar_9.y = in_f.xlv_TEXCOORD2.w;
          tmpvar_9.z = in_f.xlv_TEXCOORD3.w;
          float3 tmpvar_10;
          tmpvar_10 = normalize((_WorldSpaceCameraPos - tmpvar_9));
          worldViewDir_3 = tmpvar_10;
          tmpvar_4 = (-worldViewDir_3);
          tmpvar_6 = in_f.xlv_TEXCOORD1.xyz;
          tmpvar_7 = in_f.xlv_TEXCOORD2.xyz;
          tmpvar_8 = in_f.xlv_TEXCOORD3.xyz;
          tmpvar_5 = in_f.xlv_COLOR0;
          float3 tmpvar_11;
          float tmpvar_12;
          float3 norm2_13;
          float3 norm1_14;
          float tmpvar_15;
          tmpvar_15 = (_Time.x * _WaveSpeed);
          float2 tmpvar_16;
          tmpvar_16 = (in_f.xlv_TEXCOORD0.xy + float2(tmpvar_15, tmpvar_15));
          float2 tmpvar_17;
          tmpvar_17.x = (in_f.xlv_TEXCOORD0.x - tmpvar_15);
          tmpvar_17.y = ((in_f.xlv_TEXCOORD0.y + tmpvar_15) + 0.5);
          float3 tmpvar_18;
          tmpvar_18 = ((tex2D(_BumpMap, tmpvar_16).xyz * 2) - 1);
          norm1_14 = tmpvar_18;
          float3 tmpvar_19;
          tmpvar_19 = ((tex2D(_BumpMap, tmpvar_17).xyz * 2) - 1);
          norm2_13 = tmpvar_19;
          float3 tmpvar_20;
          tmpvar_20 = ((norm1_14 + norm2_13) * 0.5);
          float2 tmpvar_21;
          tmpvar_21.x = in_f.xlv_TEXCOORD0.z;
          float tmpvar_22;
          tmpvar_22 = (_Time.x * _MaxWaterSpeed);
          tmpvar_21.y = (in_f.xlv_TEXCOORD0.w + tmpvar_22);
          float2 tmpvar_23;
          tmpvar_23.x = (in_f.xlv_TEXCOORD0.z + 0.5);
          tmpvar_23.y = (in_f.xlv_TEXCOORD0.w + (tmpvar_22 * 0.5));
          tmpvar_11 = tmpvar_20;
          float3 tmpvar_24;
          tmpvar_24.x = dot(tmpvar_6, tmpvar_11);
          tmpvar_24.y = dot(tmpvar_7, tmpvar_11);
          tmpvar_24.z = dot(tmpvar_8, tmpvar_11);
          float3 tmpvar_25;
          tmpvar_25 = (tmpvar_4 - (2 * (dot(tmpvar_24, tmpvar_4) * tmpvar_24)));
          tmpvar_12 = (((((texCUBE(_Cube, tmpvar_25).w * _ReflectColor.w) + _Transparency) * (1 - tmpvar_5.x)) + (tmpvar_5.x * ((tex2D(_SplashTex, tmpvar_21) + tex2D(_SplashTex, tmpvar_23)) * 0.5).w)) * tmpvar_5.w);
          float tmpvar_26;
          tmpvar_26 = dot(in_f.xlv_TEXCOORD1.xyz, tmpvar_11);
          worldN_2.x = tmpvar_26;
          float tmpvar_27;
          tmpvar_27 = dot(in_f.xlv_TEXCOORD2.xyz, tmpvar_11);
          worldN_2.y = tmpvar_27;
          float tmpvar_28;
          tmpvar_28 = dot(in_f.xlv_TEXCOORD3.xyz, tmpvar_11);
          worldN_2.z = tmpvar_28;
          res_1.xyz = float3(((worldN_2 * 0.5) + 0.5));
          res_1.w = tmpvar_12;
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
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "PrePassFinal"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 400
      ZClip Off
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
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
      //uniform float4 _Time;
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
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _BumpMap_ST;
      uniform float4 _SplashTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      uniform float4 _SpecColor;
      uniform sampler2D _BumpMap;
      uniform sampler2D _SplashTex;
      uniform samplerCUBE _Cube;
      uniform float4 _Color;
      uniform float4 _ReflectColor;
      uniform float _MaxWaterSpeed;
      uniform float _WaveSpeed;
      uniform float _Transparency;
      uniform sampler2D _LightBuffer;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_COLOR0 :COLOR0;
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
          float4 xlv_COLOR0 :COLOR0;
          float4 xlv_TEXCOORD4 :TEXCOORD4;
          float3 xlv_TEXCOORD6 :TEXCOORD6;
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
          float3 tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7.w = in_v.vertex.w;
          tmpvar_7.xyz = (in_v.vertex.xyz + ((in_v.normal * (sin(((_Time.x * 3.145) + (in_v.vertex.x * 50))) + sin(((_Time.x * 2.947) + (in_v.vertex.z * 50))))) * 0.004));
          float4 tmpvar_8;
          float4 tmpvar_9;
          tmpvar_9.w = 1;
          tmpvar_9.xyz = tmpvar_7.xyz;
          tmpvar_8 = UnityObjectToClipPos(tmpvar_9);
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _SplashTex);
          float3 tmpvar_10;
          tmpvar_10 = mul(unity_ObjectToWorld, tmpvar_7).xyz;
          float3x3 tmpvar_11;
          tmpvar_11[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_11[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_11[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_12;
          tmpvar_12 = normalize(mul(in_v.normal, tmpvar_11));
          worldNormal_3 = tmpvar_12;
          float3x3 tmpvar_13;
          tmpvar_13[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_13[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_13[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_14;
          tmpvar_14 = normalize(mul(tmpvar_13, in_v.tangent.xyz));
          worldTangent_2 = tmpvar_14;
          float tmpvar_15;
          tmpvar_15 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_1 = tmpvar_15;
          float3 tmpvar_16;
          tmpvar_16 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
          float4 tmpvar_17;
          tmpvar_17.x = worldTangent_2.x;
          tmpvar_17.y = tmpvar_16.x;
          tmpvar_17.z = worldNormal_3.x;
          tmpvar_17.w = tmpvar_10.x;
          float4 tmpvar_18;
          tmpvar_18.x = worldTangent_2.y;
          tmpvar_18.y = tmpvar_16.y;
          tmpvar_18.z = worldNormal_3.y;
          tmpvar_18.w = tmpvar_10.y;
          float4 tmpvar_19;
          tmpvar_19.x = worldTangent_2.z;
          tmpvar_19.y = tmpvar_16.z;
          tmpvar_19.z = worldNormal_3.z;
          tmpvar_19.w = tmpvar_10.z;
          float4 o_20;
          float4 tmpvar_21;
          tmpvar_21 = (tmpvar_8 * 0.5);
          float2 tmpvar_22;
          tmpvar_22.x = tmpvar_21.x;
          tmpvar_22.y = (tmpvar_21.y * _ProjectionParams.x);
          o_20.xy = (tmpvar_22 + tmpvar_21.w);
          o_20.zw = tmpvar_8.zw;
          tmpvar_5.zw = float2(0, 0);
          tmpvar_5.xy = float2(0, 0);
          float3x3 tmpvar_23;
          tmpvar_23[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_23[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_23[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float4 tmpvar_24;
          tmpvar_24.w = 1;
          tmpvar_24.xyz = float3(normalize(mul(in_v.normal, tmpvar_23)));
          float4 normal_25;
          normal_25 = tmpvar_24;
          float3 res_26;
          float3 x_27;
          x_27.x = dot(unity_SHAr, normal_25);
          x_27.y = dot(unity_SHAg, normal_25);
          x_27.z = dot(unity_SHAb, normal_25);
          float3 x1_28;
          float4 tmpvar_29;
          tmpvar_29 = (normal_25.xyzz * normal_25.yzzx);
          x1_28.x = dot(unity_SHBr, tmpvar_29);
          x1_28.y = dot(unity_SHBg, tmpvar_29);
          x1_28.z = dot(unity_SHBb, tmpvar_29);
          res_26 = (x_27 + (x1_28 + (unity_SHC.xyz * ((normal_25.x * normal_25.x) - (normal_25.y * normal_25.y)))));
          float3 tmpvar_30;
          float _tmp_dvx_24 = max(((1.055 * pow(max(res_26, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_30 = float3(_tmp_dvx_24, _tmp_dvx_24, _tmp_dvx_24);
          res_26 = tmpvar_30;
          tmpvar_6 = tmpvar_30;
          out_v.vertex = tmpvar_8;
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_17;
          out_v.xlv_TEXCOORD2 = tmpvar_18;
          out_v.xlv_TEXCOORD3 = tmpvar_19;
          out_v.xlv_COLOR0 = in_v.color;
          out_v.xlv_TEXCOORD4 = o_20;
          out_v.xlv_TEXCOORD5 = tmpvar_5;
          out_v.xlv_TEXCOORD6 = tmpvar_6;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 c_2;
          float4 light_3;
          float3 worldViewDir_4;
          float3 tmpvar_5;
          float4 tmpvar_6;
          float3 tmpvar_7;
          float3 tmpvar_8;
          float3 tmpvar_9;
          float3 tmpvar_10;
          tmpvar_10.x = in_f.xlv_TEXCOORD1.w;
          tmpvar_10.y = in_f.xlv_TEXCOORD2.w;
          tmpvar_10.z = in_f.xlv_TEXCOORD3.w;
          float3 tmpvar_11;
          tmpvar_11 = normalize((_WorldSpaceCameraPos - tmpvar_10));
          worldViewDir_4 = tmpvar_11;
          tmpvar_5 = (-worldViewDir_4);
          tmpvar_7 = in_f.xlv_TEXCOORD1.xyz;
          tmpvar_8 = in_f.xlv_TEXCOORD2.xyz;
          tmpvar_9 = in_f.xlv_TEXCOORD3.xyz;
          tmpvar_6 = in_f.xlv_COLOR0;
          float3 tmpvar_12;
          float3 tmpvar_13;
          float tmpvar_14;
          float tmpvar_15;
          float4 c_16;
          float4 fc_17;
          float3 norm2_18;
          float3 norm1_19;
          float tmpvar_20;
          tmpvar_20 = (_Time.x * _WaveSpeed);
          float2 tmpvar_21;
          tmpvar_21 = (in_f.xlv_TEXCOORD0.xy + float2(tmpvar_20, tmpvar_20));
          float2 tmpvar_22;
          tmpvar_22.x = (in_f.xlv_TEXCOORD0.x - tmpvar_20);
          tmpvar_22.y = ((in_f.xlv_TEXCOORD0.y + tmpvar_20) + 0.5);
          float3 tmpvar_23;
          tmpvar_23 = ((tex2D(_BumpMap, tmpvar_21).xyz * 2) - 1);
          norm1_19 = tmpvar_23;
          float3 tmpvar_24;
          tmpvar_24 = ((tex2D(_BumpMap, tmpvar_22).xyz * 2) - 1);
          norm2_18 = tmpvar_24;
          float3 tmpvar_25;
          tmpvar_25 = ((norm1_19 + norm2_18) * 0.5);
          float2 tmpvar_26;
          tmpvar_26.x = in_f.xlv_TEXCOORD0.z;
          float tmpvar_27;
          tmpvar_27 = (_Time.x * _MaxWaterSpeed);
          tmpvar_26.y = (in_f.xlv_TEXCOORD0.w + tmpvar_27);
          float2 tmpvar_28;
          tmpvar_28.x = (in_f.xlv_TEXCOORD0.z + 0.5);
          tmpvar_28.y = (in_f.xlv_TEXCOORD0.w + (tmpvar_27 * 0.5));
          fc_17 = ((tex2D(_SplashTex, tmpvar_26) + tex2D(_SplashTex, tmpvar_28)) * 0.5);
          float4 tmpvar_29;
          tmpvar_29 = ((_Color * (1 - tmpvar_6.x)) + (tmpvar_6.x * fc_17));
          c_16 = tmpvar_29;
          tmpvar_14 = (1 - tmpvar_6.x);
          tmpvar_12 = tmpvar_25;
          float3 tmpvar_30;
          tmpvar_30.x = dot(tmpvar_7, tmpvar_12);
          tmpvar_30.y = dot(tmpvar_8, tmpvar_12);
          tmpvar_30.z = dot(tmpvar_9, tmpvar_12);
          float3 tmpvar_31;
          tmpvar_31 = (tmpvar_5 - (2 * (dot(tmpvar_30, tmpvar_5) * tmpvar_30)));
          float4 tmpvar_32;
          float _tmp_dvx_25 = texCUBE(_Cube, tmpvar_31);
          tmpvar_32 = float4(_tmp_dvx_25, _tmp_dvx_25, _tmp_dvx_25, _tmp_dvx_25);
          tmpvar_13 = ((tmpvar_32.xyz * _ReflectColor.xyz) * (1 - tmpvar_6.x));
          tmpvar_15 = (((((tmpvar_32.w * _ReflectColor.w) + _Transparency) * (1 - tmpvar_6.x)) + (tmpvar_6.x * fc_17.w)) * tmpvar_6.w);
          float4 tmpvar_33;
          tmpvar_33 = tex2D(_LightBuffer, in_f.xlv_TEXCOORD4);
          light_3 = tmpvar_33;
          light_3 = (-log2(max(light_3, float4(0.001, 0.001, 0.001, 0.001))));
          light_3.xyz = (light_3.xyz + in_f.xlv_TEXCOORD6);
          float4 c_34;
          float spec_35;
          float tmpvar_36;
          tmpvar_36 = (light_3.w * tmpvar_14);
          spec_35 = tmpvar_36;
          c_34.xyz = ((c_16.xyz * light_3.xyz) + ((light_3.xyz * _SpecColor.xyz) * spec_35));
          c_34.w = tmpvar_15;
          c_2 = c_34;
          c_2.xyz = (c_2.xyz + tmpvar_13);
          tmpvar_1 = c_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Custom/RunningWaterSimple"
}
