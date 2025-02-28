//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "Custom/Merge" {
Properties {
_MainTex ("Sprite Texture", 2D) = "white" { }
_HairTex ("Hair Texture", 2D) = "white" { }
_Color ("Tint", Color) = (1,1,1,1)
_HairOffX ("HairOffsetX", Float) = 0
_HairOffY ("HairOffsetY", Float) = 0
_HairRotation ("HairRotation", Float) = 0
_SpriteRect ("Sprite Rect", Vector) = (0,0,1,1)
_HairRect ("Hair Rect", Vector) = (0,0,1,1)
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
  GpuProgramID 2426
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