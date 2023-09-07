// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RainDrop/Internal/RainDistortion (Mobile)"
{
  Properties
  {
    _Strength ("Distortion Strength", Range(0, 1000)) = 50
    _Distortion ("Normalmap", 2D) = "bump" {}
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
      LOD 1146244943
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
        Mode  1868789861
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
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      uniform float4 _BackgroundTexture_TexelSize;
      uniform float _Strength;
      uniform sampler2D _Distortion;
      uniform sampler2D _BackgroundTexture;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
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
          float2 tmpvar_3;
          tmpvar_1 = UnityObjectToClipPos(in_v.vertex);
          float4 o_4;
          float4 tmpvar_5;
          tmpvar_5 = (tmpvar_1 * 0.5);
          o_4.xy = (tmpvar_5.xy + tmpvar_5.w);
          o_4.zw = tmpvar_1.zw;
          tmpvar_2 = o_4;
          tmpvar_3 = (_Strength * _BackgroundTexture_TexelSize.xy);
          out_v.vertex = tmpvar_1;
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          out_v.xlv_TEXCOORD1 = tmpvar_2;
          out_v.xlv_TEXCOORD2 = tmpvar_3;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1.zw = in_f.xlv_TEXCOORD1.zw;
          float3 tmpvar_2;
          tmpvar_2 = ((tex2D(_Distortion, in_f.xlv_TEXCOORD0).xyz * 2) - 1);
          tmpvar_1.xy = (in_f.xlv_TEXCOORD1.xy - (in_f.xlv_TEXCOORD2 * tmpvar_2.xy));
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_BackgroundTexture, tmpvar_1);
          out_f.color = tmpvar_3;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
