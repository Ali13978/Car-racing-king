// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RainDrop/Internal/RainDistortion (Forward)"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _Strength ("Distortion Strength", Range(0, 550)) = 50
    _Relief ("Relief Value", Range(0, 2)) = 1.5
    _Distortion ("Normalmap", 2D) = "black" {}
    _ReliefTex ("Relief", 2D) = "black" {}
    _Blur ("Blur", Range(0, 50)) = 3
    _Darkness ("Darkness", Range(0, 100)) = 10
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
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZTest Always
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile BLUR
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      uniform float4 _GrabTexture_TexelSize;
      uniform float4 _Color;
      uniform float _Strength;
      uniform float _Darkness;
      uniform sampler2D _Distortion;
      uniform sampler2D _GrabTexture;
      uniform sampler2D _ReliefTex;
      uniform float _Relief;
      uniform float _Blur;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float xlv_TEXCOORD2 :TEXCOORD2;
          float xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float xlv_TEXCOORD2 :TEXCOORD2;
          float xlv_TEXCOORD3 :TEXCOORD3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          tmpvar_1 = in_v.texcoord.xy;
          float4 tmpvar_2;
          float2 tmpvar_3;
          float2 tmpvar_4;
          float4 tmpvar_5;
          tmpvar_2 = UnityObjectToClipPos(in_v.vertex);
          float2 tmpvar_6;
          tmpvar_6 = in_v.color.xy;
          tmpvar_3 = tmpvar_6;
          tmpvar_4 = tmpvar_1;
          float4 o_7;
          float4 tmpvar_8;
          tmpvar_8 = (tmpvar_2 * 0.5);
          o_7.xy = (tmpvar_8.xy + tmpvar_8.w);
          o_7.zw = tmpvar_2.zw;
          tmpvar_5 = o_7;
          out_v.vertex = tmpvar_2;
          out_v.xlv_COLOR = tmpvar_3;
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_5;
          out_v.xlv_TEXCOORD2 = (_Strength * _GrabTexture_TexelSize.x);
          out_v.xlv_TEXCOORD3 = (_Darkness * _Color.w);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1.zw = in_f.xlv_TEXCOORD1.zw;
          float4 col_2;
          float4 relf_3;
          float3 tmpvar_4;
          tmpvar_4 = ((tex2D(_Distortion, in_f.xlv_TEXCOORD0).xyz * 2) - 1);
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_ReliefTex, in_f.xlv_TEXCOORD0);
          relf_3.xyz = tmpvar_5.xyz;
          relf_3.w = clamp(((tmpvar_5.x * tmpvar_5.y) * tmpvar_5.z), 0, 1);
          tmpvar_1.xy = (in_f.xlv_TEXCOORD1.xy - (in_f.xlv_TEXCOORD2 * tmpvar_4.xy));
          float2 tmpvar_6;
          tmpvar_6 = tmpvar_1.xy;
          float3 tmpvar_7;
          float2 texcoord_8;
          texcoord_8 = tmpvar_6;
          float3 blured_9;
          float tmpvar_10;
          tmpvar_10 = ((tmpvar_4.x * _Blur) * 0.390625);
          float2 tmpvar_11;
          tmpvar_11 = (texcoord_8 - (float2(4, 4) * float2(tmpvar_10, tmpvar_10)));
          float3 tmpvar_12;
          tmpvar_12 = (tex2D(_GrabTexture, tmpvar_11) * 0.016).xyz;
          blured_9 = tmpvar_12;
          float2 tmpvar_13;
          tmpvar_13 = (texcoord_8 - (float2(3, 3) * float2(tmpvar_10, tmpvar_10)));
          float3 tmpvar_14;
          tmpvar_14 = (tex2D(_GrabTexture, tmpvar_13) * 0.054).xyz;
          blured_9 = (blured_9 + tmpvar_14);
          float2 tmpvar_15;
          tmpvar_15 = (texcoord_8 - (float2(2, 2) * float2(tmpvar_10, tmpvar_10)));
          float3 tmpvar_16;
          tmpvar_16 = (tex2D(_GrabTexture, tmpvar_15) * 0.122).xyz;
          blured_9 = (blured_9 + tmpvar_16);
          float2 tmpvar_17;
          tmpvar_17 = (texcoord_8 - float2(tmpvar_10, tmpvar_10));
          float3 tmpvar_18;
          tmpvar_18 = (tex2D(_GrabTexture, tmpvar_17) * 0.195).xyz;
          blured_9 = (blured_9 + tmpvar_18);
          float3 tmpvar_19;
          tmpvar_19 = (tex2D(_GrabTexture, texcoord_8) * 0.227).xyz;
          blured_9 = (blured_9 + tmpvar_19);
          float2 tmpvar_20;
          tmpvar_20 = (texcoord_8 + float2(tmpvar_10, tmpvar_10));
          float3 tmpvar_21;
          tmpvar_21 = (tex2D(_GrabTexture, tmpvar_20) * 0.195).xyz;
          blured_9 = (blured_9 + tmpvar_21);
          float2 tmpvar_22;
          tmpvar_22 = (texcoord_8 + (float2(2, 2) * float2(tmpvar_10, tmpvar_10)));
          float3 tmpvar_23;
          tmpvar_23 = (tex2D(_GrabTexture, tmpvar_22) * 0.122).xyz;
          blured_9 = (blured_9 + tmpvar_23);
          float2 tmpvar_24;
          tmpvar_24 = (texcoord_8 + (float2(3, 3) * float2(tmpvar_10, tmpvar_10)));
          float3 tmpvar_25;
          tmpvar_25 = (tex2D(_GrabTexture, tmpvar_24) * 0.054).xyz;
          blured_9 = (blured_9 + tmpvar_25);
          float2 tmpvar_26;
          tmpvar_26 = (texcoord_8 + (float2(4, 4) * float2(tmpvar_10, tmpvar_10)));
          float3 tmpvar_27;
          tmpvar_27 = (tex2D(_GrabTexture, tmpvar_26) * 0.016).xyz;
          blured_9 = (blured_9 + tmpvar_27);
          tmpvar_7 = blured_9;
          float4 tmpvar_28;
          tmpvar_28.w = 1;
          tmpvar_28.xyz = float3(tmpvar_7);
          col_2.w = tmpvar_28.w;
          col_2.xyz = (tmpvar_7 + ((tmpvar_5.xyz * _Color.xyz) * _Color.w));
          float tmpvar_29;
          tmpvar_29 = clamp((1 - (in_f.xlv_TEXCOORD3 * relf_3.w)), 0, 1);
          col_2.xyz = (col_2.xyz * tmpvar_29);
          col_2.xyz = (col_2.xyz * (1 - (tmpvar_4.x * _Relief)));
          out_f.color = col_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
