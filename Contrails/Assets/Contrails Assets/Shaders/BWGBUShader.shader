Shader "Ordinance/BWGBUShader"
{
    /*  
     * Copyright 2023 Barkın Zorlu 
     * All rights reserved.
     * 
     * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivatives 4.0 International License. 
     * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/4.0/ or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
     */

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Colour", Color) = (1,1,1,1)
    }
    SubShader
    {
        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        struct VertexInput
        {
            float4 position: POSITION;
            float2 uv : TEXCOORD0;
        };

        struct VertexOutput //v2f
        {
            float4 position : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        ENDHLSL

        pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            VertexOutput vert(VertexInput i)
            {
                VertexOutput o;
                o.position = TransformObjectToHClip(i.position.xyz);
                o.uv = i.uv;
                return o;
            }

            float4 frag(VertexOutput i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                //return baseTex * _BaseColor;

                float intensity = col.r * 0.299 + col.g * 0.587 + col.b * 0.114;
                return float4(intensity, intensity, intensity, col.a);

            }

            ENDHLSL
        }
    }
}
