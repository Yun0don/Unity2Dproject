//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "Custom/AthleteShader" {
Properties {
_MainTex ("Sprite Texture", 2D) = "white" { }
_HairTex ("Hair Texture", 2D) = "white" { }
_HairOffX ("Hair OffsetX", Float) = 0
_HairOffY ("Hair OffsetY", Float) = 0
_Rotation ("Rotation", Float) = 0
_HairRect ("Hair Rect", Vector) = (0,0,1,1)
_HairNormal ("Hair Normal", Color) = (1,1,1,1)
_HairDark ("Hair Dark", Color) = (1,1,1,1)
_HairHighlight ("Hair Highlight", Color) = (1,1,1,1)
_TatooTex ("Tatoo Texture", 2D) = "white" { }
_TatooOffX ("Tatoo OffsetX", Float) = 0
_TatooOffY ("Tatoo OffsetY", Float) = 0
_TatooRect ("Tatoo Rect", Vector) = (0,0,1,1)
_NecklaceTex ("Necklace Texture", 2D) = "white" { }
_NecklaceOffX ("Tatoo OffsetX", Float) = 0
_NecklaceOffY ("Tatoo OffsetY", Float) = 0
_NecklaceRect ("Necklace Rect", Vector) = (0,0,1,1)
_GlassesTex ("Glasses Texture", 2D) = "white" { }
_GlassesOffX ("Tatoo OffsetX", Float) = 0
_GlassesOffY ("Tatoo OffsetY", Float) = 0
_GlassesRect ("Glasses Rect", Vector) = (0,0,1,1)
_Color ("Tint", Color) = (1,1,1,1)
_TopLightColor ("TopLightColor", Color) = (1,1,1,1)
_TopDarkColor ("TopDarkColor", Color) = (1,1,1,1)
_PantsLightColor ("PantsLightColor", Color) = (1,1,1,1)
_PantsDarkColor ("PantsDarkColor", Color) = (1,1,1,1)
_BootsLightColor ("BootsLightColor", Color) = (1,1,1,1)
_BootsDarkColor ("BootsDarkColor", Color) = (1,1,1,1)
_SpriteRect ("Sprite Rect", Vector) = (0,0,1,1)
_FlashColor ("Flash Color", Color) = (1,1,1,1)
_FlashAmount ("Flash Amount", Range(0, 1)) = 0
[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
}
SubShader {
 Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
 Pass {
  Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
  Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
  ZWrite Off
  Cull Off
  GpuProgramID 16073
Program "vp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "PIXELSNAP_ON" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 " {
Keywords { "PIXELSNAP_ON" }
"// shader disassembly not supported on DXBC"
}
}
}
}
}