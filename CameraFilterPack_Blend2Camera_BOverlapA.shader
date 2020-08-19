// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/////////////////////////////////////////////////////////
// CameraFilterPack(Revision) - by SmalBox.top 2020 /////
/////////////////////////////////////////////////////////
Shader "CameraFilterPack/Blend2Camera_BOverlapA" { 
Properties 
{
_MainTex ("Base (RGB)", 2D) = "white" {}
_MainTex2 ("Base (RGB)", 2D) = "white" {}
_TimeX ("Time", Range(0.0, 1.0)) = 1.0
_ScreenResolution ("_ScreenResolution", Vector) = (0.,0.,0.,0.)
}
SubShader
{
Pass
{
Cull Off ZWrite Off ZTest Always
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#pragma target 3.0
#pragma glsl
#include "UnityCG.cginc"
uniform sampler2D _MainTex;
uniform sampler2D _MainTex2;
uniform float _TimeX;
uniform float _Value;
uniform float2 _MainTex_TexelSize;
uniform float4 _ScreenResolution;
struct appdata_t
{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};
struct v2f
{
float2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
float4 color    : COLOR;
};
v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}

half4 _MainTex_ST;
float4 frag(v2f i) : COLOR
{
float2 uvst = UnityStereoScreenSpaceUVAdjust(i.texcoord, _MainTex_ST);
float2 uv=uvst.xy;

float4 tex = tex2D(_MainTex,uv);    
#if UNITY_UV_STARTS_AT_TOP
if (_MainTex_TexelSize.y < 0)
uv.y = 1-uv.y;
#endif
float4 tex2 = tex2D(_MainTex2,uv);

// tex2 Overlapped tex (Transparence Overlap)
float r = (1 - tex2.a) * tex.r + tex2.a * tex2.r * _Value; 
float g = (1 - tex2.a) * tex.g + tex2.a * tex2.g * _Value; 
float b = (1 - tex2.a) * tex.b + tex2.a * tex2.b * _Value; 

float3 finalColor = float3(r, g, b);
float4 finalTex = float4(finalColor, 1.0);

return finalTex;
}
ENDCG
}
}
}
