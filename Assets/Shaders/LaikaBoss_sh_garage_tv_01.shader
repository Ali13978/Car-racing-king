// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "LaikaBoss/sh_garage_tv_01"
{
  Properties
  {
    _MainTex ("MainTex", 2D) = "white" {}
    _numU ("numU", float) = 4
    _numV ("numV", float) = 8
    _Speed ("Speed", float) = 0.3
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "Opaque"
    }
    Pass // ind: 1, name: FORWARDBASE
    {
      Name "FORWARDBASE"
      Tags
      { 
        "LIGHTMODE" = "ForwardBase"
        "RenderType" = "Opaque"
        "SHADOWSUPPORT" = "true"
      }
      ZClip Off
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 UNITY_MATRIX_MVP;
      //uniform float4 _Time;
      uniform float4 _TimeEditor;
      uniform sampler2D _MainTex;
      uniform float4 _MainTex_ST;
      uniform float _numU;
      uniform float _numV;
      uniform float _Speed;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
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
          out_v.vertex = UnityObjectToClipPos(in_v.vertex);
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 _MainTex_var_2;
          float tmpvar_3;
          tmpvar_3 = frac(((_Time + _TimeEditor).y * _Speed));
          float2 tmpvar_4;
          float tmpvar_5;
          tmpvar_5 = (1 / _numU);
          tmpvar_4.x = tmpvar_5;
          tmpvar_4.y = (1 / _numV);
          int curentCol_6;
          int tmpvar_7;
          tmpvar_7 = int(floor((tmpvar_3 / (1 / (_numU * _numV)))));
          curentCol_6 = tmpvar_7;
          if((float(tmpvar_7)>=_numU))
          {
              curentCol_6 = int((float(tmpvar_7) - (floor((float(tmpvar_7) / _numU)) * _numU)));
          }
          float tmpvar_8;
          tmpvar_8 = (1 / _numV);
          float2 tmpvar_9;
          tmpvar_9.x = (tmpvar_5 * float(curentCol_6));
          tmpvar_9.y = ((1 - (tmpvar_8 * float(int((tmpvar_3 / tmpvar_8))))) - _numV);
          float4 tmpvar_10;
          float2 P_11;
          P_11 = ((((tmpvar_4 * in_f.xlv_TEXCOORD0) + tmpvar_9) * _MainTex_ST.xy) + _MainTex_ST.zw);
          tmpvar_10 = tex2D(_MainTex, P_11);
          _MainTex_var_2 = tmpvar_10;
          float4 tmpvar_12;
          tmpvar_12.w = 1;
          tmpvar_12.xyz = _MainTex_var_2.xyz;
          tmpvar_1 = tmpvar_12;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
