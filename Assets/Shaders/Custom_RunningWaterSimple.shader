// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RunningWaterSimple"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _Transparency ("Transparency", Range(-0.5, 0.5)) = 0.1
    _WaveSpeed ("wave velocity", Range(-10, 10)) = 1
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
      uniform float4 _SplashTex_ST;
      //uniform float4 _Time;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _SplashTex;
      uniform float _WaveSpeed;
      uniform float _Transparency;
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
          float _tmp_dvx_22 = max(((1.055 * pow(max(res_8, float3(0, 0, 0)), float3(0.4166667, 0.4166667, 0.4166667))) - 0.055), float3(0, 0, 0));
          tmpvar_12 = float3(_tmp_dvx_22, _tmp_dvx_22, _tmp_dvx_22);
          res_8 = tmpvar_12;
          out_v.vertex = UnityObjectToClipPos(tmpvar_3);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _SplashTex);
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
          float3 tmpvar_3;
          float3 lightDir_4;
          float3 tmpvar_5;
          tmpvar_5 = _WorldSpaceLightPos0.xyz;
          lightDir_4 = tmpvar_5;
          tmpvar_3 = in_f.xlv_TEXCOORD1;
          float tmpvar_6;
          float2 tmpvar_7;
          tmpvar_7.x = in_f.xlv_TEXCOORD0.x;
          float tmpvar_8;
          tmpvar_8 = (_Time.x * _WaveSpeed);
          tmpvar_7.y = (in_f.xlv_TEXCOORD0.y + tmpvar_8);
          float2 tmpvar_9;
          tmpvar_9.x = (in_f.xlv_TEXCOORD0.x + 0.5);
          tmpvar_9.y = (in_f.xlv_TEXCOORD0.y + (tmpvar_8 * 0.5));
          float4 tmpvar_10;
          tmpvar_10 = (tex2D(_SplashTex, tmpvar_7) + tex2D(_SplashTex, tmpvar_9));
          tmpvar_6 = (_Transparency + 0.5);
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_4;
          float4 c_11;
          float4 c_12;
          float diff_13;
          float tmpvar_14;
          tmpvar_14 = max(0, dot(tmpvar_3, tmpvar_2));
          diff_13 = tmpvar_14;
          c_12.xyz = ((tmpvar_10.xyz * tmpvar_1) * diff_13);
          c_12.w = tmpvar_6;
          c_11.w = c_12.w;
          c_11.xyz = (c_12.xyz + (tmpvar_10.xyz * in_f.xlv_TEXCOORD3));
          out_f.color = c_11;
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
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      uniform float4 _SplashTex_ST;
      //uniform float4 _Time;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _LightTexture0;
      uniform float4x4 unity_WorldToLight;
      uniform sampler2D _SplashTex;
      uniform float _WaveSpeed;
      uniform float _Transparency;
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
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _SplashTex);
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
          float3 tmpvar_3;
          float3 lightDir_4;
          float3 tmpvar_5;
          tmpvar_5 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD2));
          lightDir_4 = tmpvar_5;
          tmpvar_3 = in_f.xlv_TEXCOORD1;
          float tmpvar_6;
          float2 tmpvar_7;
          tmpvar_7.x = in_f.xlv_TEXCOORD0.x;
          float tmpvar_8;
          tmpvar_8 = (_Time.x * _WaveSpeed);
          tmpvar_7.y = (in_f.xlv_TEXCOORD0.y + tmpvar_8);
          float2 tmpvar_9;
          tmpvar_9.x = (in_f.xlv_TEXCOORD0.x + 0.5);
          tmpvar_9.y = (in_f.xlv_TEXCOORD0.y + (tmpvar_8 * 0.5));
          tmpvar_6 = (_Transparency + 0.5);
          float4 tmpvar_10;
          tmpvar_10.w = 1;
          tmpvar_10.xyz = in_f.xlv_TEXCOORD2;
          float3 tmpvar_11;
          tmpvar_11 = mul(unity_WorldToLight, tmpvar_10).xyz;
          float tmpvar_12;
          tmpvar_12 = dot(tmpvar_11, tmpvar_11);
          float tmpvar_13;
          tmpvar_13 = tex2D(_LightTexture0, float2(tmpvar_12, tmpvar_12)).w;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_4;
          tmpvar_1 = (tmpvar_1 * tmpvar_13);
          float4 c_14;
          float4 c_15;
          float diff_16;
          float tmpvar_17;
          tmpvar_17 = max(0, dot(tmpvar_3, tmpvar_2));
          diff_16 = tmpvar_17;
          c_15.xyz = (((tex2D(_SplashTex, tmpvar_7) + tex2D(_SplashTex, tmpvar_9)).xyz * tmpvar_1) * diff_16);
          c_15.w = tmpvar_6;
          c_14.w = c_15.w;
          c_14.xyz = c_15.xyz;
          out_f.color = c_14;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Bumped Specular"
}
