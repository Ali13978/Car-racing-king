// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "RainDrop/Internal/RainNoDistortion"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Distortion ("Normalmap", 2D) = "black" {}
    _Relief ("Relief Value", Range(0, 2)) = 1.5
    _Darkness ("Darkness", Range(0, 100)) = 10
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      uniform float4 _Color;
      uniform float _Darkness;
      uniform sampler2D _MainTex;
      uniform sampler2D _Distortion;
      uniform float _Relief;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.xyz = _Color.xyz;
          float tmpvar_2;
          tmpvar_2 = clamp((1 - _Darkness), 0, 1);
          tmpvar_1.xyz = (tmpvar_1.xyz * tmpvar_2);
          tmpvar_1.w = _Color.w;
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.xlv_COLOR = tmpvar_1;
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2.w = in_f.xlv_COLOR.w;
          float2 norm_3;
          float2 tmpvar_4;
          tmpvar_4 = ((tex2D(_Distortion, in_f.xlv_TEXCOORD0).xyz * 2) - 1).xy;
          norm_3 = tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tmpvar_2.xyz = (in_f.xlv_COLOR.xyz * tmpvar_5.xyz);
          tmpvar_2.xyz = (tmpvar_2.xyz * (1 - (norm_3.x * _Relief)));
          tmpvar_2.w = ((in_f.xlv_COLOR.w * tmpvar_5.x) * (tmpvar_5.y * tmpvar_5.z));
          tmpvar_1 = tmpvar_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
