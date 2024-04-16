Shader "Unlit/OutlinerUIShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("_MainTex", 2D) = "white" {}
        _Outline("Outline", Range(0, 5)) = 0.5
        _Color("Color", Color) = (1, 0.9964251, 0, 0)
        [HideInInspector]_BUILTIN_QueueOffset("Float", Float) = 0
        [HideInInspector]_BUILTIN_QueueControl("Float", Float) = -1
    }
        SubShader
        {
            Tags
            {
                // RenderPipeline: <None>
                "RenderType" = "Transparent"
                "BuiltInMaterialType" = "Unlit"
                "Queue" = "Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader" = "true"
            "ShaderGraphTargetId" = "BuiltInUnlitSubTarget"
        }
        Pass
        {
            Name "Pass"

            // Render State
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            ZTest LEqual
            ZWrite Off
            ColorMask RGB

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 3.0
            #pragma multi_compile_instancing
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #pragma vertex vert
            #pragma fragment frag

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_UNLIT
            #define BUILTIN_TARGET_API 1
            #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
            #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
            #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
            #endif
            #ifdef _BUILTIN_ALPHATEST_ON
            #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
            #endif
            #ifdef _BUILTIN_AlphaClip
            #define _AlphaClip _BUILTIN_AlphaClip
            #endif
            #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
            #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
            #endif


            // custom interpolator pre-include
            /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */

            // Includes
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            // custom interpolators pre packing
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

            struct Attributes
            {
                 float3 positionOS : POSITION;
                 float3 normalOS : NORMAL;
                 float4 tangentOS : TANGENT;
                 float4 uv0 : TEXCOORD0;
                #if UNITY_ANY_INSTANCING_ENABLED
                 uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
            struct Varyings
            {
                 float4 positionCS : SV_POSITION;
                 float4 texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                 uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            struct SurfaceDescriptionInputs
            {
                 float4 uv0;
            };
            struct VertexDescriptionInputs
            {
                 float3 ObjectSpaceNormal;
                 float3 ObjectSpaceTangent;
                 float3 ObjectSpacePosition;
            };
            struct PackedVaryings
            {
                 float4 positionCS : SV_POSITION;
                 float4 texCoord0 : INTERP0;
                #if UNITY_ANY_INSTANCING_ENABLED
                 uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                 uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                 uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                 FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };

            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                ZERO_INITIALIZE(PackedVaryings, output);
                output.positionCS = input.positionCS;
                output.texCoord0.xyzw = input.texCoord0;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }

            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.texCoord0.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                return output;
            }


            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_TexelSize;
            float _Outline;
            float4 _Color;
            CBUFFER_END


                // Object and Global properties
                SAMPLER(SamplerState_Linear_Repeat);
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);

                // -- Property used by ScenePickingPass
                #ifdef SCENEPICKINGPASS
                float4 _SelectionID;
                #endif

                // -- Properties used by SceneSelectionPass
                #ifdef SCENESELECTIONPASS
                int _ObjectId;
                int _PassValue;
                #endif

                // Graph Includes
                // GraphIncludes: <None>

                // Graph Functions

                void Unity_Multiply_float_float(float A, float B, out float Out)
                {
                Out = A * B;
                }

                void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
                {
                    Out = UV * Tiling + Offset;
                }

                void Unity_Add_float(float A, float B, out float Out)
                {
                    Out = A + B;
                }

                void Unity_Negate_float(float In, out float Out)
                {
                    Out = -1 * In;
                }

                void Unity_Clamp_float(float In, float Min, float Max, out float Out)
                {
                    Out = clamp(In, Min, Max);
                }

                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }

                void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
                {
                Out = A * B;
                }

                void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A + B;
                }

                struct Bindings_var2DOutliner_9b5ffa47cf87ad848bdea1194699384a_float
                {
                half4 uv0;
                };

                void SG_var2DOutliner_9b5ffa47cf87ad848bdea1194699384a_float(UnityTexture2D _MainTex, float4 _Color, float _Outline, Bindings_var2DOutliner_9b5ffa47cf87ad848bdea1194699384a_float IN, out float4 OutVector4_1, out float OutVector1_2)
                {
                UnityTexture2D _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D = _MainTex;
                float4 _UV_da9d4e2b15b147619c12358fa31ee9b0_Out_0_Vector4 = IN.uv0;
                float4 _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.tex, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.samplerstate, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.GetTransformedUV((_UV_da9d4e2b15b147619c12358fa31ee9b0_Out_0_Vector4.xy)));
                float _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_R_4_Float = _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_RGBA_0_Vector4.r;
                float _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_G_5_Float = _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_RGBA_0_Vector4.g;
                float _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_B_6_Float = _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_RGBA_0_Vector4.b;
                float _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_A_7_Float = _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_RGBA_0_Vector4.a;
                float4 _Property_aa95a7a83fdb45e3998bf4be6fcfff3c_Out_0_Vector4 = _Color;
                float _Property_0a379c625b51426089239d25fa214d81_Out_0_Float = _Outline;
                float _Multiply_da0786839ad04d0d9578f95b151bcc7e_Out_2_Float;
                Unity_Multiply_float_float(_Property_0a379c625b51426089239d25fa214d81_Out_0_Float, 0.0015, _Multiply_da0786839ad04d0d9578f95b151bcc7e_Out_2_Float);
                float2 _Vector2_8d9cc426f5f54591b0a68d1eb3745d92_Out_0_Vector2 = float2(_Multiply_da0786839ad04d0d9578f95b151bcc7e_Out_2_Float, 0);
                float2 _TilingAndOffset_2664b84b858f44df9173981a8326d8b4_Out_3_Vector2;
                Unity_TilingAndOffset_float((_UV_da9d4e2b15b147619c12358fa31ee9b0_Out_0_Vector4.xy), float2 (1, 1), _Vector2_8d9cc426f5f54591b0a68d1eb3745d92_Out_0_Vector2, _TilingAndOffset_2664b84b858f44df9173981a8326d8b4_Out_3_Vector2);
                float4 _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.tex, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.samplerstate, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_2664b84b858f44df9173981a8326d8b4_Out_3_Vector2));
                float _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_R_4_Float = _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_RGBA_0_Vector4.r;
                float _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_G_5_Float = _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_RGBA_0_Vector4.g;
                float _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_B_6_Float = _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_RGBA_0_Vector4.b;
                float _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_A_7_Float = _SampleTexture2D_aab4cb0923804839b6246bd3f3658374_RGBA_0_Vector4.a;
                float2 _Vector2_c4e2ac45e9644feeb03c7ee4cc8c4599_Out_0_Vector2 = float2(0, _Multiply_da0786839ad04d0d9578f95b151bcc7e_Out_2_Float);
                float2 _TilingAndOffset_b12a78c1608a42a28bb86b74f70f1a1c_Out_3_Vector2;
                Unity_TilingAndOffset_float((_UV_da9d4e2b15b147619c12358fa31ee9b0_Out_0_Vector4.xy), float2 (1, 1), _Vector2_c4e2ac45e9644feeb03c7ee4cc8c4599_Out_0_Vector2, _TilingAndOffset_b12a78c1608a42a28bb86b74f70f1a1c_Out_3_Vector2);
                float4 _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.tex, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.samplerstate, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_b12a78c1608a42a28bb86b74f70f1a1c_Out_3_Vector2));
                float _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_R_4_Float = _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_RGBA_0_Vector4.r;
                float _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_G_5_Float = _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_RGBA_0_Vector4.g;
                float _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_B_6_Float = _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_RGBA_0_Vector4.b;
                float _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_A_7_Float = _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_RGBA_0_Vector4.a;
                float _Add_ee166105d3dd43bfaa93ec114576599a_Out_2_Float;
                Unity_Add_float(_SampleTexture2D_aab4cb0923804839b6246bd3f3658374_A_7_Float, _SampleTexture2D_6f3ff5d20a50404f912df91a3fa5744b_A_7_Float, _Add_ee166105d3dd43bfaa93ec114576599a_Out_2_Float);
                float _Negate_870b9152499c47a7967376f015f796d7_Out_1_Float;
                Unity_Negate_float(_Multiply_da0786839ad04d0d9578f95b151bcc7e_Out_2_Float, _Negate_870b9152499c47a7967376f015f796d7_Out_1_Float);
                float2 _Vector2_14eca97e0f254206a4feb48e77892ec3_Out_0_Vector2 = float2(_Negate_870b9152499c47a7967376f015f796d7_Out_1_Float, 0);
                float2 _TilingAndOffset_47a8a640a0844c5f8379075ff4405026_Out_3_Vector2;
                Unity_TilingAndOffset_float((_UV_da9d4e2b15b147619c12358fa31ee9b0_Out_0_Vector4.xy), float2 (1, 1), _Vector2_14eca97e0f254206a4feb48e77892ec3_Out_0_Vector2, _TilingAndOffset_47a8a640a0844c5f8379075ff4405026_Out_3_Vector2);
                float4 _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.tex, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.samplerstate, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_47a8a640a0844c5f8379075ff4405026_Out_3_Vector2));
                float _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_R_4_Float = _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_RGBA_0_Vector4.r;
                float _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_G_5_Float = _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_RGBA_0_Vector4.g;
                float _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_B_6_Float = _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_RGBA_0_Vector4.b;
                float _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_A_7_Float = _SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_RGBA_0_Vector4.a;
                float _Negate_ab0d5166d4e0440abcedee63f1f93efa_Out_1_Float;
                Unity_Negate_float(_Multiply_da0786839ad04d0d9578f95b151bcc7e_Out_2_Float, _Negate_ab0d5166d4e0440abcedee63f1f93efa_Out_1_Float);
                float2 _Vector2_430c22ef89f14bddae482886401cc3c5_Out_0_Vector2 = float2(0, _Negate_ab0d5166d4e0440abcedee63f1f93efa_Out_1_Float);
                float2 _TilingAndOffset_11c0b2ed9ff74daf99a078325f8f0d62_Out_3_Vector2;
                Unity_TilingAndOffset_float((_UV_da9d4e2b15b147619c12358fa31ee9b0_Out_0_Vector4.xy), float2 (1, 1), _Vector2_430c22ef89f14bddae482886401cc3c5_Out_0_Vector2, _TilingAndOffset_11c0b2ed9ff74daf99a078325f8f0d62_Out_3_Vector2);
                float4 _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.tex, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.samplerstate, _Property_54b69d8086204181a40eb0cb0a8dec8e_Out_0_Texture2D.GetTransformedUV(_TilingAndOffset_11c0b2ed9ff74daf99a078325f8f0d62_Out_3_Vector2));
                float _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_R_4_Float = _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_RGBA_0_Vector4.r;
                float _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_G_5_Float = _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_RGBA_0_Vector4.g;
                float _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_B_6_Float = _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_RGBA_0_Vector4.b;
                float _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_A_7_Float = _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_RGBA_0_Vector4.a;
                float _Add_814a5b7c449540fb8566fccc8922b347_Out_2_Float;
                Unity_Add_float(_SampleTexture2D_bfe809bc4d0a411bbcf0030cef31dd4c_A_7_Float, _SampleTexture2D_5e45283c346749f28bec4dea21a8fd69_A_7_Float, _Add_814a5b7c449540fb8566fccc8922b347_Out_2_Float);
                float _Add_de734c4c240846bab6a2654bd3ce79a6_Out_2_Float;
                Unity_Add_float(_Add_ee166105d3dd43bfaa93ec114576599a_Out_2_Float, _Add_814a5b7c449540fb8566fccc8922b347_Out_2_Float, _Add_de734c4c240846bab6a2654bd3ce79a6_Out_2_Float);
                float _Clamp_bb4d132a6da648acbeffafcb1e1e63d5_Out_3_Float;
                Unity_Clamp_float(_Add_de734c4c240846bab6a2654bd3ce79a6_Out_2_Float, 0, 1, _Clamp_bb4d132a6da648acbeffafcb1e1e63d5_Out_3_Float);
                float _Subtract_1b034b4f758e4ad99a085fc8450efe78_Out_2_Float;
                Unity_Subtract_float(_Clamp_bb4d132a6da648acbeffafcb1e1e63d5_Out_3_Float, _SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_A_7_Float, _Subtract_1b034b4f758e4ad99a085fc8450efe78_Out_2_Float);
                float4 _Multiply_58d7723f3a584e50b68350b7affab6e2_Out_2_Vector4;
                Unity_Multiply_float4_float4(_Property_aa95a7a83fdb45e3998bf4be6fcfff3c_Out_0_Vector4, (_Subtract_1b034b4f758e4ad99a085fc8450efe78_Out_2_Float.xxxx), _Multiply_58d7723f3a584e50b68350b7affab6e2_Out_2_Vector4);
                float4 _Add_a6f7924bf4b04e62939e110089018983_Out_2_Vector4;
                Unity_Add_float4(_SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_RGBA_0_Vector4, _Multiply_58d7723f3a584e50b68350b7affab6e2_Out_2_Vector4, _Add_a6f7924bf4b04e62939e110089018983_Out_2_Vector4);
                float _Add_06948c8e25fd4bdcbaeab1adf83bb65b_Out_2_Float;
                Unity_Add_float(_SampleTexture2D_b8f03a64a720416cbbb3b77c5c62cba0_A_7_Float, _Subtract_1b034b4f758e4ad99a085fc8450efe78_Out_2_Float, _Add_06948c8e25fd4bdcbaeab1adf83bb65b_Out_2_Float);
                OutVector4_1 = _Add_a6f7924bf4b04e62939e110089018983_Out_2_Vector4;
                OutVector1_2 = _Add_06948c8e25fd4bdcbaeab1adf83bb65b_Out_2_Float;
                }

                // Custom interpolators pre vertex
                /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };

                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }

                // Custom interpolators, pre surface
                #ifdef FEATURES_GRAPH_VERTEX
                Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
                {
                return output;
                }
                #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
                #endif

                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float Alpha;
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    UnityTexture2D _Property_faed061ef9224c9a80601daa2b657de7_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
                    float4 _Property_63242347e1f34cd68588ecc4a23c5820_Out_0_Vector4 = _Color;
                    float _Property_141087439202455d9df73407dcf331fb_Out_0_Float = _Outline;
                    Bindings_var2DOutliner_9b5ffa47cf87ad848bdea1194699384a_float _var2DOutliner_16efc99500914ca4b74429974e29bb92;
                    _var2DOutliner_16efc99500914ca4b74429974e29bb92.uv0 = IN.uv0;
                    float4 _var2DOutliner_16efc99500914ca4b74429974e29bb92_OutVector4_1_Vector4;
                    float _var2DOutliner_16efc99500914ca4b74429974e29bb92_OutVector1_2_Float;
                    SG_var2DOutliner_9b5ffa47cf87ad848bdea1194699384a_float(_Property_faed061ef9224c9a80601daa2b657de7_Out_0_Texture2D, _Property_63242347e1f34cd68588ecc4a23c5820_Out_0_Vector4, _Property_141087439202455d9df73407dcf331fb_Out_0_Float, _var2DOutliner_16efc99500914ca4b74429974e29bb92, _var2DOutliner_16efc99500914ca4b74429974e29bb92_OutVector4_1_Vector4, _var2DOutliner_16efc99500914ca4b74429974e29bb92_OutVector1_2_Float);
                    surface.BaseColor = (_var2DOutliner_16efc99500914ca4b74429974e29bb92_OutVector4_1_Vector4.xyz);
                    surface.Alpha = _var2DOutliner_16efc99500914ca4b74429974e29bb92_OutVector1_2_Float;
                    return surface;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);

                    output.ObjectSpaceNormal = input.normalOS;
                    output.ObjectSpaceTangent = input.tangentOS.xyz;
                    output.ObjectSpacePosition = input.positionOS;

                    return output;
                }
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);








                    #if UNITY_UV_STARTS_AT_TOP
                    #else
                    #endif


                    output.uv0 = input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

                        return output;
                }

                void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
                {
                    result.vertex = float4(attributes.positionOS, 1);
                    result.tangent = attributes.tangentOS;
                    result.normal = attributes.normalOS;
                    result.texcoord = attributes.uv0;
                    result.vertex = float4(vertexDescription.Position, 1);
                    result.normal = vertexDescription.Normal;
                    result.tangent = float4(vertexDescription.Tangent, 0);
                    #if UNITY_ANY_INSTANCING_ENABLED
                    #endif
                }

                void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
                {
                    result.pos = varyings.positionCS;
                    // World Tangent isn't an available input on v2f_surf


                    #if UNITY_ANY_INSTANCING_ENABLED
                    #endif
                    #if UNITY_SHOULD_SAMPLE_SH
                    #if !defined(LIGHTMAP_ON)
                    #endif
                    #endif
                    #if defined(LIGHTMAP_ON)
                    #endif
                    #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                        result.fogCoord = varyings.fogFactorAndVertexLight.x;
                        COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
                    #endif

                    DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
                }

                void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
                {
                    result.positionCS = surfVertex.pos;
                    // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
                    // World Tangent isn't an available input on v2f_surf

                    #if UNITY_ANY_INSTANCING_ENABLED
                    #endif
                    #if UNITY_SHOULD_SAMPLE_SH
                    #if !defined(LIGHTMAP_ON)
                    #endif
                    #endif
                    #if defined(LIGHTMAP_ON)
                    #endif
                    #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                        result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                        COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
                    #endif

                    DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
                }

                // --------------------------------------------------
                // Main

                #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

                ENDHLSL
                }
        }
            CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
                                            CustomEditorForRenderPipeline "UnityEditor.Rendering.BuiltIn.ShaderGraph.BuiltInUnlitGUI" ""
                                            FallBack "Hidden/Shader Graph/FallbackError"
}