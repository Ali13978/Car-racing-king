// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Legacy Shaders/Bumped Diffuse"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _BumpMap ("Normalmap", 2D) = "bump" {}
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
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform float4 _Color;
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
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
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
          float3 normal_16;
          normal_16 = worldNormal_3;
          float4 tmpvar_17;
          tmpvar_17.w = 1;
          tmpvar_17.xyz = float3(normal_16);
          float3 res_18;
          float3 x_19;
          x_19.x = dot(unity_SHAr, tmpvar_17);
          x_19.y = dot(unity_SHAg, tmpvar_17);
          x_19.z = dot(unity_SHAb, tmpvar_17);
          float3 x1_20;
          float4 tmpvar_21;
          tmpvar_21 = (normal_16.xyzz * normal_16.yzzx);
          x1_20.x = dot(unity_SHBr, tmpvar_21);
          x1_20.y = dot(unity_SHBg, tmpvar_21);
          x1_20.z = dot(unity_SHBb, tmpvar_21);
          res_18 = (x_19 + (x1_20 + (unity_SHC.xyz * ((normal_16.x * normal_16.x) - (normal_16.y * normal_16.y)))));
          float3 tmpvar_22;
          float _tmp_dvx_0 = max(((1.055 * pow(max(res_18, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_22 = float3(_tmp_dvx_0, _tmp_dvx_0, _tmp_dvx_0);
          res_18 = tmpvar_22;
          out_v.vertex = UnityObjectToClipPos(tmpvar_5);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_13;
          out_v.xlv_TEXCOORD2 = tmpvar_14;
          out_v.xlv_TEXCOORD3 = tmpvar_15;
          out_v.xlv_TEXCOORD4 = max(float3(0, 0, 0), tmpvar_22);
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
          float3 lightDir_5;
          float3 tmpvar_6;
          tmpvar_6 = _WorldSpaceLightPos0.xyz;
          lightDir_5 = tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy) * _Color);
          float3 tmpvar_8;
          tmpvar_8 = ((tex2D(_BumpMap, in_f.xlv_TEXCOORD0.zw).xyz * 2) - 1);
          float tmpvar_9;
          tmpvar_9 = dot(in_f.xlv_TEXCOORD1.xyz, tmpvar_8);
          worldN_3.x = tmpvar_9;
          float tmpvar_10;
          tmpvar_10 = dot(in_f.xlv_TEXCOORD2.xyz, tmpvar_8);
          worldN_3.y = tmpvar_10;
          float tmpvar_11;
          tmpvar_11 = dot(in_f.xlv_TEXCOORD3.xyz, tmpvar_8);
          worldN_3.z = tmpvar_11;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_5;
          float4 c_12;
          float4 c_13;
          float diff_14;
          float tmpvar_15;
          tmpvar_15 = max(0, dot(worldN_3, tmpvar_2));
          diff_14 = tmpvar_15;
          c_13.xyz = ((tmpvar_7.xyz * tmpvar_1) * diff_14);
          c_13.w = tmpvar_7.w;
          c_12.w = c_13.w;
          c_12.xyz = (c_13.xyz + (tmpvar_7.xyz * in_f.xlv_TEXCOORD4));
          c_4.xyz = c_12.xyz;
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
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _MainTex_ST;
      uniform float4 _BumpMap_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform float4 _Color;
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
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
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
          float tangentSign_1;
          float3 worldTangent_2;
          float3 worldNormal_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = in_v.vertex.xyz;
          tmpvar_4.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          tmpvar_4.zw = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          float3x3 tmpvar_6;
          tmpvar_6[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_6[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_6[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_7;
          tmpvar_7 = normalize(mul(in_v.normal, tmpvar_6));
          worldNormal_3 = tmpvar_7;
          float3x3 tmpvar_8;
          tmpvar_8[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_8[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_8[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_9;
          tmpvar_9 = normalize(mul(tmpvar_8, in_v.tangent.xyz));
          worldTangent_2 = tmpvar_9;
          float tmpvar_10;
          tmpvar_10 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_1 = tmpvar_10;
          float3 tmpvar_11;
          tmpvar_11 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
          float3 tmpvar_12;
          tmpvar_12.x = worldTangent_2.x;
          tmpvar_12.y = tmpvar_11.x;
          tmpvar_12.z = worldNormal_3.x;
          float3 tmpvar_13;
          tmpvar_13.x = worldTangent_2.y;
          tmpvar_13.y = tmpvar_11.y;
          tmpvar_13.z = worldNormal_3.y;
          float3 tmpvar_14;
          tmpvar_14.x = worldTangent_2.z;
          tmpvar_14.y = tmpvar_11.z;
          tmpvar_14.z = worldNormal_3.z;
          out_v.vertex = UnityObjectToClipPos(tmpvar_5);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_12;
          out_v.xlv_TEXCOORD2 = tmpvar_13;
          out_v.xlv_TEXCOORD3 = tmpvar_14;
          out_v.xlv_TEXCOORD4 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
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
          float3 lightDir_5;
          float3 tmpvar_6;
          tmpvar_6 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD4));
          lightDir_5 = tmpvar_6;
          float4 tmpvar_7;
          tmpvar_7 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy) * _Color);
          float3 tmpvar_8;
          tmpvar_8 = ((tex2D(_BumpMap, in_f.xlv_TEXCOORD0.zw).xyz * 2) - 1);
          float4 tmpvar_9;
          tmpvar_9.w = 1;
          tmpvar_9.xyz = in_f.xlv_TEXCOORD4;
          float3 tmpvar_10;
          tmpvar_10 = mul(unity_WorldToLight, tmpvar_9).xyz;
          float tmpvar_11;
          tmpvar_11 = dot(tmpvar_10, tmpvar_10);
          float tmpvar_12;
          tmpvar_12 = tex2D(_LightTexture0, float2(tmpvar_11, tmpvar_11)).w;
          worldN_3.x = dot(in_f.xlv_TEXCOORD1, tmpvar_8);
          worldN_3.y = dot(in_f.xlv_TEXCOORD2, tmpvar_8);
          worldN_3.z = dot(in_f.xlv_TEXCOORD3, tmpvar_8);
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_5;
          tmpvar_1 = (tmpvar_1 * tmpvar_12);
          float4 c_13;
          float4 c_14;
          float diff_15;
          float tmpvar_16;
          tmpvar_16 = max(0, dot(worldN_3, tmpvar_2));
          diff_15 = tmpvar_16;
          c_14.xyz = ((tmpvar_7.xyz * tmpvar_1) * diff_15);
          c_14.w = tmpvar_7.w;
          c_13.w = c_14.w;
          c_13.xyz = c_14.xyz;
          c_4.xyz = c_13.xyz;
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
      //uniform float4 unity_WorldTransformParams;
      uniform float4 _BumpMap_ST;
      uniform sampler2D _BumpMap;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
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
          worldNormal_3 = tmpvar_7;
          float3x3 tmpvar_8;
          tmpvar_8[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_8[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_8[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_9;
          tmpvar_9 = normalize(mul(tmpvar_8, in_v.tangent.xyz));
          worldTangent_2 = tmpvar_9;
          float tmpvar_10;
          tmpvar_10 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_1 = tmpvar_10;
          float3 tmpvar_11;
          tmpvar_11 = (((worldNormal_3.yzx * worldTangent_2.zxy) - (worldNormal_3.zxy * worldTangent_2.yzx)) * tangentSign_1);
          float4 tmpvar_12;
          tmpvar_12.x = worldTangent_2.x;
          tmpvar_12.y = tmpvar_11.x;
          tmpvar_12.z = worldNormal_3.x;
          tmpvar_12.w = tmpvar_5.x;
          float4 tmpvar_13;
          tmpvar_13.x = worldTangent_2.y;
          tmpvar_13.y = tmpvar_11.y;
          tmpvar_13.z = worldNormal_3.y;
          tmpvar_13.w = tmpvar_5.y;
          float4 tmpvar_14;
          tmpvar_14.x = worldTangent_2.z;
          tmpvar_14.y = tmpvar_11.z;
          tmpvar_14.z = worldNormal_3.z;
          tmpvar_14.w = tmpvar_5.z;
          out_v.vertex = UnityObjectToClipPos(tmpvar_4);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _BumpMap);
          out_v.xlv_TEXCOORD1 = tmpvar_12;
          out_v.xlv_TEXCOORD2 = tmpvar_13;
          out_v.xlv_TEXCOORD3 = tmpvar_14;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 res_1;
          float3 worldN_2;
          float3 tmpvar_3;
          tmpvar_3 = ((tex2D(_BumpMap, in_f.xlv_TEXCOORD0).xyz * 2) - 1);
          float tmpvar_4;
          tmpvar_4 = dot(in_f.xlv_TEXCOORD1.xyz, tmpvar_3);
          worldN_2.x = tmpvar_4;
          float tmpvar_5;
          tmpvar_5 = dot(in_f.xlv_TEXCOORD2.xyz, tmpvar_3);
          worldN_2.y = tmpvar_5;
          float tmpvar_6;
          tmpvar_6 = dot(in_f.xlv_TEXCOORD3.xyz, tmpvar_3);
          worldN_2.z = tmpvar_6;
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
          float _tmp_dvx_1 = max(((1.055 * pow(max(res_11, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_15 = float3(_tmp_dvx_1, _tmp_dvx_1, _tmp_dvx_1);
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
          float4 tmpvar_4;
          tmpvar_4 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_LightBuffer, in_f.xlv_TEXCOORD2);
          light_3 = tmpvar_5;
          light_3 = (-log2(max(light_3, float4(0.001, 0.001, 0.001, 0.001))));
          light_3.xyz = (light_3.xyz + in_f.xlv_TEXCOORD4);
          float4 c_6;
          c_6.xyz = (tmpvar_4.xyz * light_3.xyz);
          c_6.w = tmpvar_4.w;
          c_2.xyz = c_6.xyz;
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
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform float4 _Color;
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
          tmpvar_5.zw = float2(0, 0);
          tmpvar_5.xy = float2(0, 0);
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
          float _tmp_dvx_2 = max(((1.055 * pow(max(res_19, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_23 = float3(_tmp_dvx_2, _tmp_dvx_2, _tmp_dvx_2);
          res_19 = tmpvar_23;
          out_v.vertex = UnityObjectToClipPos(tmpvar_6);
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_14;
          out_v.xlv_TEXCOORD2 = tmpvar_15;
          out_v.xlv_TEXCOORD3 = tmpvar_16;
          out_v.xlv_TEXCOORD4 = tmpvar_5;
          out_v.xlv_TEXCOORD5 = max(float3(0, 0, 0), tmpvar_23);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 outEmission_1;
          float3 worldN_2;
          float3 tmpvar_3;
          float4 tmpvar_4;
          tmpvar_4 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy) * _Color);
          tmpvar_3 = tmpvar_4.xyz;
          float3 tmpvar_5;
          tmpvar_5 = ((tex2D(_BumpMap, in_f.xlv_TEXCOORD0.zw).xyz * 2) - 1);
          float tmpvar_6;
          tmpvar_6 = dot(in_f.xlv_TEXCOORD1.xyz, tmpvar_5);
          worldN_2.x = tmpvar_6;
          float tmpvar_7;
          tmpvar_7 = dot(in_f.xlv_TEXCOORD2.xyz, tmpvar_5);
          worldN_2.y = tmpvar_7;
          float tmpvar_8;
          tmpvar_8 = dot(in_f.xlv_TEXCOORD3.xyz, tmpvar_5);
          worldN_2.z = tmpvar_8;
          float4 emission_9;
          float3 tmpvar_10;
          float3 tmpvar_11;
          tmpvar_10 = tmpvar_3;
          tmpvar_11 = worldN_2;
          float4 tmpvar_12;
          tmpvar_12.xyz = float3(tmpvar_10);
          tmpvar_12.w = 1;
          float4 tmpvar_13;
          tmpvar_13.xyz = float3(0, 0, 0);
          tmpvar_13.w = 0;
          float4 tmpvar_14;
          tmpvar_14.w = 1;
          tmpvar_14.xyz = float3(((tmpvar_11 * 0.5) + 0.5));
          float4 tmpvar_15;
          tmpvar_15.w = 1;
          tmpvar_15.xyz = float3(0, 0, 0);
          emission_9 = tmpvar_15;
          emission_9.xyz = (emission_9.xyz + (tmpvar_4.xyz * in_f.xlv_TEXCOORD5));
          outEmission_1.w = emission_9.w;
          outEmission_1.xyz = exp2((-emission_9.xyz));
          out_f.color = tmpvar_12;
          out_f.color1 = tmpvar_13;
          out_f.color2 = tmpvar_14;
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
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
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
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
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
          float tangentSign_1;
          float3 worldTangent_2;
          float3 worldNormal_3;
          float4 vertex_4;
          vertex_4 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_4.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_5;
              if((in_v.vertex.z>0))
              {
                  tmpvar_5 = 0.0001;
              }
              else
              {
                  tmpvar_5 = 0;
              }
              vertex_4.z = tmpvar_5;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_4.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_6;
              if((vertex_4.z>0))
              {
                  tmpvar_6 = 0.0001;
              }
              else
              {
                  tmpvar_6 = 0;
              }
              vertex_4.z = tmpvar_6;
          }
          float4 tmpvar_7;
          tmpvar_7.w = 1;
          tmpvar_7.xyz = vertex_4.xyz;
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
          out_v.vertex = UnityObjectToClipPos(tmpvar_7);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_15;
          out_v.xlv_TEXCOORD2 = tmpvar_16;
          out_v.xlv_TEXCOORD3 = tmpvar_17;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          tmpvar_3 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color).xyz;
          tmpvar_2 = tmpvar_3;
          float4 res_4;
          res_4 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_5;
              tmpvar_5.w = 1;
              tmpvar_5.xyz = float3(tmpvar_2);
              res_4.w = tmpvar_5.w;
              float3 tmpvar_6;
              float _tmp_dvx_3 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_6 = clamp(pow(tmpvar_2, float3(_tmp_dvx_3, _tmp_dvx_3, _tmp_dvx_3)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_4.xyz = float3(tmpvar_6);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_7;
              if(int(unity_UseLinearSpace))
              {
                  emission_7 = float3(0, 0, 0);
              }
              else
              {
                  emission_7 = float3(0, 0, 0);
              }
              float4 tmpvar_8;
              float alpha_9;
              float3 tmpvar_10;
              tmpvar_10 = (emission_7 * 0.01030928);
              alpha_9 = (ceil((max(max(tmpvar_10.x, tmpvar_10.y), max(tmpvar_10.z, 0.02)) * 255)) / 255);
              float tmpvar_11;
              tmpvar_11 = max(alpha_9, 0.02);
              alpha_9 = tmpvar_11;
              float4 tmpvar_12;
              tmpvar_12.xyz = float3((tmpvar_10 / tmpvar_11));
              tmpvar_12.w = tmpvar_11;
              tmpvar_8 = tmpvar_12;
              res_4 = tmpvar_8;
          }
          tmpvar_1 = res_4;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Legacy Shaders/Diffuse"
}
