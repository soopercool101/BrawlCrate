using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Animations;
using OpenTK.Graphics.OpenGL;
using System;
using System.Globalization;

namespace BrawlLib.Wii.Graphics
{
    public unsafe class ShaderGenerator
    {
        public static bool UsePixelLighting
        {
            get => _pixelLighting;
            set
            {
                if (_pixelLighting == value)
                {
                    return;
                }

                _pixelLighting = value;
                _forceRecompile = true;
            }
        }

        private static bool _pixelLighting;

        public static bool _forceRecompile;

        //Determines if the final shader should be written to the console for review
#if DEBUG
        private static readonly bool AlwaysOutputShader = false;
#endif
        private static readonly bool DoTrunc = false;

        //The GLSL version to be used.
        //120 is the only supported version right now because it's the most compatible version
        private static readonly int GLSLVersion = 120;

        //Red fragment shader, just to test if it's working
        private static readonly bool TestFrag = false;

        private static string _shaderCode;
#if DEBUG
        private static bool _vertex;
#endif
        private static string[] swapModeTable;

        public static MDL0MaterialNode _material;
        public static MDL0ShaderNode _shaderNode;

        //Material colors
        //2 LightChannel material and ambient colors, 
        //used for light channel calculations with SCN0
        //3 Color Registers, these can be used and modified by the shader
        //4 Constant Color Registers, these can be used but cannot be modified by the shader
        private static readonly string[] _uaMatColorName = {"amb", "clr", "creg", "ccreg"};

        //SCN0 Lightset
        //8 possible lights, 1 possible ambient light
        //These are used for light channel color calculations,
        //which the result of can be used by the shader's RasterColor selection
        private const string _uSCNLightSetAmbLightName = "scnSetAmbLight";
        private const string _uSCNLightSetLightsName = "scnSetLights";

        //SCN0 Fog
        private const string _uSCNFogStartName = "scnFogStartZ";
        private const string _uSCNFogEndName = "scnFogEndZ";
        private const string _uSCNFogColorName = "scnFogColor";
        private const string _uSCNFogTypeName = "scnFogType";

        //Varying variables
        private const string _vNormalName = "vNormal";
        private const string _vPositionName = "vPosition";
        private static readonly string[] _vVtxColorsName = {"vtxColor0", "vtxColor1"};

        //Fragment shader variables
        private const string LightChannelName = "lightChannel";

        public static void SetUniforms(MDL0MaterialNode mat)
        {
            int pHandle = mat._programHandle;

            //Material colors
            Uniform(pHandle, _uaMatColorName[0], mat.amb1, mat.amb2);
            Uniform(pHandle, _uaMatColorName[1], mat.clr1, mat.clr2);
            Uniform(pHandle, _uaMatColorName[2], mat.c1, mat.c2, mat.c3);
            Uniform(pHandle, _uaMatColorName[3], mat.k1, mat.k2, mat.k3, mat.k4);

            //SCN0 Lightset
            Uniform(pHandle, _uSCNLightSetAmbLightName, mat._ambientLight);
            Uniform(pHandle, _uSCNLightSetLightsName, mat._lights);

            //Fog
            FogAnimationFrame fog = mat._fog;
            Uniform(pHandle, _uSCNFogStartName, fog.Start);
            Uniform(pHandle, _uSCNFogEndName, fog.End);
            Uniform(pHandle, _uSCNFogColorName, fog.Color);
            Uniform(pHandle, _uSCNFogTypeName, (int) fog.Type);
        }

        private static void WriteVertexUniforms()
        {
            if (!UsePixelLighting)
            {
                wU("vec4 {0}[2];", _uaMatColorName[0]);
                wU("vec4 {0}[2];", _uaMatColorName[1]);
                wl();
                wU("vec4 {0};", _uSCNLightSetAmbLightName);
                wU("{0} {1}[8];", LightStructName, _uSCNLightSetLightsName);
                wl();
            }
        }

        private static void WriteFragmentUniforms()
        {
            for (int i = 0; i < _material.Children.Count; i++)
            {
                wU("sampler2D texture{0};", i.ToString());
            }

            wl();

            if (UsePixelLighting)
            {
                wU("vec4 {0}[2];", _uaMatColorName[0]);
                wU("vec4 {0}[2];", _uaMatColorName[1]);
            }

            wU("vec4 {0}[3];", _uaMatColorName[2]);
            wU("vec4 {0}[4];", _uaMatColorName[3]);

            wl();

            if (UsePixelLighting)
            {
                wU("vec4 {0};", _uSCNLightSetAmbLightName);
                wU("{0} {1}[8];", LightStructName, _uSCNLightSetLightsName);
                wl();
            }

            wU("int {0};", _uSCNFogTypeName);
            wU("float {0};", _uSCNFogStartName);
            wU("float {0};", _uSCNFogEndName);
            wU("vec3 {0};", _uSCNFogColorName);

            wl();
        }

        private static void WriteVarying()
        {
            wV("vec3 {0};", _vPositionName);
            wV("vec3 {0};", _vNormalName);
            wV("vec4 {0};", _vVtxColorsName[0]);
            wV("vec4 {0};", _vVtxColorsName[1]);
            if (!UsePixelLighting)
            {
                wV("vec4 {0}0;", LightChannelName);
                wV("vec4 {0}1;", LightChannelName);
            }

            wl();
        }

        public static string GenVertexShader()
        {
#if DEBUG
            _vertex = true;
#endif
            Reset();

            Comment("MDL0 vertex shader generated by BrawlLib");
            Comment(_material.Name);
            wl();
            WriteVersion();
            if (!UsePixelLighting)
            {
                WriteLightFrameStruct();
            }

            WriteClamps();
            WriteVertexUniforms();
            WriteVarying();
            Begin();
            {
                //wl("gl_Position = ftransform();");
                wl("gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;");

                wl("{0} = vec3(gl_ModelViewMatrix * gl_Vertex);", _vPositionName);
                if (UsePixelLighting)
                {
                    wl("{0} = gl_NormalMatrix * gl_Normal;", _vNormalName);
                }
                else
                {
                    wl("{0} = normalize(gl_NormalMatrix * gl_Normal);", _vNormalName);
                }

                wl("{0} = gl_Color;", _vVtxColorsName[0]);
                wl("{0} = gl_SecondaryColor;", _vVtxColorsName[1]);

                if (!UsePixelLighting)
                {
                    WriteLightChannels();
                }

                for (int i = 0; i < _material.Children.Count; i++)
                {
                    MDL0MaterialRefNode mr = (MDL0MaterialRefNode) _material.Children[i];

                    //LightChannels are not written in the vertex shader when using pixel lighting,
                    //but color texture coordinates need it. Need to handle this in the fragment shader
                    if (mr.Coordinates == TexSourceRow.Colors && UsePixelLighting)
                    {
                        continue;
                    }

                    WriteCoordSource(mr, i);

                    wl("gl_TexCoord[{0}] = uv{0};", i);
                }
            }
            return Finish();
        }

        private static void WriteClamps()
        {
            wl("float satlf(float f)");
            OpenBracket();
            wl("return (f < 0.0) ? 0.0 : f;");
            CloseBracket();
            wl();
            wl("vec3 satlv(vec3 v)");
            OpenBracket();
            wl("return vec3(satlf(v.x),satlf(v.y),satlf(v.z));");
            CloseBracket();
            wl();
            wl("float satf(float f)");
            OpenBracket();
            wl("return clamp(f, 0.0, 1.0);");
            CloseBracket();
            wl();
            wl("vec3 satv(vec3 v)");
            OpenBracket();
            wl("return clamp(v, vec3(0.0), vec3(1.0));");
            CloseBracket();
            wl();
        }

        private static void WriteCoordSource(MDL0MaterialRefNode mr, int i)
        {
            //Associated texture coordinates are loaded into the same active texture unit as the map's index
            string src = mr.TextureCoordId >= 0 ? "gl_MultiTexCoord" + i : _texGenSrc[(int) mr.Coordinates];

            if (mr.Coordinates == TexSourceRow.Colors)
            {
                src = string.Format(src, mr.Type == TexTexgenType.Color1 ? "1" : "0");
            }

            src = $"gl_TextureMatrix[{i}] * {src}";

            //TODO: Normalizing doesn't work right when a mesh is scaled by a bone
            if (mr.Normalize)
            {
                src = $"normalize({src})";
            }

            wl("vec4 uv{0} = {1};", i, src);

            if (mr.Projection == TexProjection.STQ)
            {
                wl("if (uv{0}.z != 0.0f) uv{0}.xy = uv{0}.xy / uv{0}.z;", i);
            }
        }

        public static string GenMaterialFragShader()
        {
#if DEBUG
            _vertex = false;
#endif
            Reset();

            Comment("MDL0 fragment shader generated by BrawlLib");
            Comment(_material.Name);
            wl();
            WriteVersion();
            if (UsePixelLighting)
            {
                WriteLightFrameStruct();
            }

            WriteClamps();
            WriteFragmentUniforms();
            WriteVarying();
            if (DoTrunc)
            {
                wl("float {0}(float c)", Trunc1Name);
                OpenBracket();
                wl("return (c == 0.0) ? 0.0 : ((fract(c) == 0.0) ? 1.0 : fract(c));");
                CloseBracket();
                wl();
                wl("vec3 {0}(vec3 c)", Trunc3Name);
                OpenBracket();
                wl("return vec3({0}(c.r), {0}(c.g), {0}(c.b));", Trunc1Name);
                CloseBracket();
                wl();
                wl("vec4 {0}(vec4 c)", Trunc4Name);
                OpenBracket();
                wl("return vec4({0}(c.r), {0}(c.g), {0}(c.b), {0}(c.a));", Trunc1Name);
                CloseBracket();
                wl();
            }

            Begin();
            {
                bool doAlphaTest = true;

                GXAlphaFunction func = _material._alphaFunc;
                AlphaOp logic = func.Logic;
                AlphaCompare func0 = func.Comp0;
                AlphaCompare func1 = func.Comp1;

                if (logic == AlphaOp.Or && (func0 == AlphaCompare.Always || func1 == AlphaCompare.Always) ||
                    logic == AlphaOp.And && func0 == AlphaCompare.Always && func1 == AlphaCompare.Always)
                {
                    //Always passes, so don't bother testing alpha
                    doAlphaTest = false;
                }
                else if (logic == AlphaOp.And && (func0 == AlphaCompare.Never || func0 == AlphaCompare.Never) ||
                         logic == AlphaOp.Or && func0 == AlphaCompare.Never && func0 == AlphaCompare.Never)
                {
                    //Never passes, so end the shader here.
                    wl("discard;");
                    return Finish();
                }

                if (TestFrag)
                {
                    wl("gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);");
                    return Finish();
                }

                if (UsePixelLighting)
                {
                    wl("vec4 {0}0 = {1};", LightChannelName, vec4Zero);
                    wl("vec4 {0}1 = {1};", LightChannelName, vec4Zero);
                    WriteLightChannels();

                    for (int i = 0; i < _material.Children.Count; i++)
                    {
                        MDL0MaterialRefNode mr = (MDL0MaterialRefNode) _material.Children[i];
                        if (mr.Coordinates == TexSourceRow.Colors)
                        {
                            WriteCoordSource(mr, i);
                        }
                        else
                        {
                            wl("vec4 uv{0} = gl_TexCoord[{0}];", i);
                        }
                    }
                }

                //Write the output color register value
                //In the event that a shader isn't injected, the frag shader will still compile
                wl("vec4 {0} = {1};", PrevRegName, _defaultRegisterValues[0]);

                //The shader will be added in here
                _shaderCode += "%" + tabCount + "%";

                if (_material.ConstantAlphaEnabled)
                {
                    //TODO: cut out alpha operations from injected shader
                    //No point doing them if they'll be thrown out later
                    //This is a low priority optimization
                    Comment("Constant Alpha");
                    wl("{0}.a = {1};", PrevRegName,
                        (_material.ConstantAlphaValue / 255.0f).ToString(CultureInfo.InvariantCulture));
                    wl();
                }

                ApplyFog();

                if (DoTrunc)
                {
                    wl("{0} = truncc4({0});", PrevRegName);
                }

                wl("gl_FragColor = {0};", PrevRegName, _uSCNFogColorName, _fogDensityName);

                if (doAlphaTest)
                {
                    wl();
                    Comment("Alpha Function");

                    string compare0 = string.Format(_alphaTestCompName[(int) func0], "gl_FragColor.a",
                        (func._ref0 / 255.0f).ToString(CultureInfo.InvariantCulture));
                    string compare1 = string.Format(_alphaTestCompName[(int) func1], "gl_FragColor.a",
                        (func._ref1 / 255.0f).ToString(CultureInfo.InvariantCulture));
                    string fullcompare = string.Format(_alphaTestCombineName[(int) logic], compare0, compare1);

                    if (logic == AlphaOp.Or)
                    {
                        if (func0 == AlphaCompare.Always)
                        {
                            fullcompare = compare1;
                        }
                        else if (func1 == AlphaCompare.Always)
                        {
                            fullcompare = compare0;
                        }
                    }
                    else if (logic == AlphaOp.And)
                    {
                        if (func0 == AlphaCompare.Never)
                        {
                            fullcompare = compare1;
                        }
                        else if (func1 == AlphaCompare.Never)
                        {
                            fullcompare = compare0;
                        }
                    }

                    wl("if (!(" + fullcompare + ")) discard;");
                }
                else if (!_material.EnableBlend)
                {
                    //If no alpha testing and no blending, alpha is always 1.0
                    wl("gl_FragColor.a = 1.0;");
                }
            }
            return Finish();
        }

        private static void ApplyFog()
        {
            string pixelZ = "posZ";

            //Don't apply fog if disabled
            Comment("SCN0 Fog");
            wl("if ({0} != 0)", _uSCNFogTypeName);
            OpenBracket();
            {
                wl("float {0} = 0.0;", _fogDensityName);
                wl("float {0} = length({1});", pixelZ, _vPositionName);

                //Return one color or the other if out of test bounds
                wl("if ({0} <= {1})", pixelZ, _uSCNFogStartName);
                OpenBracket();
                {
                    wl("{0} = 0.0;", _fogDensityName);
                }
                CloseBracket();
                wl("else if ({0} >= {1})", pixelZ, _uSCNFogEndName);
                OpenBracket();
                {
                    wl("{0} = 1.0;", _fogDensityName);
                }
                CloseBracket();
                //First compare is perspective, second is orthographic
                wl("else if (({0} == 2) || ({0} == 10))", _uSCNFogTypeName);
                OpenBracket();
                {
                    Comment("Linear");
                    wl("{0} = ({1} - {2}) / ({3} - {2});", _fogDensityName, pixelZ, _uSCNFogStartName, _uSCNFogEndName);
                }
                CloseBracket();
                wl("else if (({0} == 4) || ({0} == 12))", _uSCNFogTypeName);
                OpenBracket();
                {
                    Comment("Exp");
                    wl("{0} = 1.0 - exp2(-8.0 * (({1} - {2}) / ({3} - {2})));", _fogDensityName, pixelZ,
                        _uSCNFogStartName, _uSCNFogEndName);
                }
                CloseBracket();
                wl("else if (({0} == 5) || ({0} == 13))", _uSCNFogTypeName);
                OpenBracket();
                {
                    Comment("Exp^2");
                    wl("{0} = 1.0 - exp2(-8.0 * pow(({1} - {2}) / ({3} - {2}), 2.0));", _fogDensityName, pixelZ,
                        _uSCNFogStartName, _uSCNFogEndName);
                }
                CloseBracket();
                wl("else if (({0} == 6) || ({0} == 14))", _uSCNFogTypeName);
                OpenBracket();
                {
                    Comment("RevExp");
                    wl("{0} = exp2(-8.0 * (({3} - {1}) / ({3} - {2})));", _fogDensityName, pixelZ, _uSCNFogStartName,
                        _uSCNFogEndName);
                }
                CloseBracket();
                wl("else if (({0} == 7) || ({0} == 15))", _uSCNFogTypeName);
                OpenBracket();
                {
                    Comment("RevExp^2");
                    wl("{0} = exp2(-8.0 * pow(({3} - {1}) / ({3} - {2}), 2.0));", _fogDensityName, pixelZ,
                        _uSCNFogStartName, _uSCNFogEndName);
                }
                CloseBracket();
                wl("{0}.rgb = mix({0}.rgb, {1}, satf({2}));", PrevRegName, _uSCNFogColorName, _fogDensityName);
            }
            CloseBracket();
        }

        private static string WriteLightChannels()
        {
            Comment("Lighting Calculations");

            string error = WriteLightChannel(_material.LightChannel0, 0);
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }

            error = WriteLightChannel(_material.LightChannel1, 1);
            if (!string.IsNullOrEmpty(error))
            {
                return error;
            }

            return null;
        }

        private static string WriteLightChannel(LightChannel channel, int index)
        {
            if (!channel.Flags.HasFlag(LightingChannelFlags.UseChanColor) &&
                !channel.Flags.HasFlag(LightingChannelFlags.UseChanAlpha))
            {
                return null;
            }

            Comment("LightChannel" + index);

            LightChannelControl color = channel._color;
            LightChannelControl alpha = channel._alpha;

            string amb = $"{_uaMatColorName[0]}[{index}]";
            string clr = $"{_uaMatColorName[1]}[{index}]";

            string matColorName = "matColor" + index;
            string lightFuncName = "lightFunc" + index;
            string illumName = "illum" + index;

            wl("vec4 {0}, {1}, {2} = {3};", matColorName, lightFuncName, illumName, vec4One);

            if (color.Enabled || alpha.Enabled)
            {
                //Set SCN0 ambient color 
                if (color.Enabled && alpha.Enabled)
                {
                    wl("{0} = {1};", illumName, _uSCNLightSetAmbLightName);
                }
                else
                {
                    if (color.Enabled)
                    {
                        wl("{0}.rgb = {1}.rgb;", illumName, _uSCNLightSetAmbLightName);
                    }

                    if (alpha.Enabled)
                    {
                        wl("{0}.a = {1}.a;", illumName, _uSCNLightSetAmbLightName);
                    }
                }

                //Set material base color
                GXColorSrc cAmbSrc = color.AmbientSource;
                GXColorSrc aAmbSrc = alpha.AmbientSource;
                if (cAmbSrc == aAmbSrc)
                {
                    wl("{0} *= {1};", illumName,
                        cAmbSrc == GXColorSrc.Register ? amb : _vVtxColorsName[index]);
                }
                else
                {
                    wl("{0}.rgb *= {1}.rgb;", illumName,
                        cAmbSrc == GXColorSrc.Register ? amb : _vVtxColorsName[index]);
                    wl("{0}.a *= {1}.a;", illumName,
                        aAmbSrc == GXColorSrc.Register ? amb : _vVtxColorsName[index]);
                }

                //Add each light's color and attenuation to the illumination, on top of the ambient
                wl("for (int i = 0; i < 8; i++)");
                OpenBracket();
                {
                    //Get the light
                    wl("{0} {1} = {2}[i];", LightStructName, lightName, _uSCNLightSetLightsName);
                    //lightName = _uSCNLightSetLightsName + "[i]";

                    wl("if ({0}.{1} != 0)", lightName, LightEnabledName);
                    //Add this light if it is used
                    OpenBracket();
                    {
                        //Get vector from light position to vertex position in eye space
                        //lightVec = mvMtx * lightPos - vertexPos;
                        wl("vec3 {0} = normalize((gl_ModelViewMatrix * vec4({2}.{3}, 1.0)).xyz - {1});",
                            lightVecName, _vPositionName, lightName, LightPosName);

                        //Used by diffuse function and specular attenuation
                        wl("float {0} = dot(normalize({1}), {2});", NdotLName, _vNormalName, lightVecName);
                        wl("vec4 {0} = {1};", lightColorName, vec4One);
                        wl();

                        #region Diffuse Attenuation

                        //Initialize attenuation value with diffuse attenuation
                        string cAttn = $"float {attnName}{colorPassSuffix} = " + "{0};";
                        string aAttn = $"float {attnName}{alphaPassSuffix} = " + "{0};";
                        switch (color.DiffuseFunction)
                        {
                            case GXDiffuseFn.Disabled:
                                wl(cAttn, "1.0");
                                break;
                            case GXDiffuseFn.Enabled:
                                wl(cAttn, NdotLName);
                                break;
                            case GXDiffuseFn.Clamped:
                                wl(cAttn, "satlf(" + NdotLName + ")");
                                break;
                        }

                        switch (alpha.DiffuseFunction)
                        {
                            case GXDiffuseFn.Disabled:
                                wl(aAttn, "1.0");
                                break;
                            case GXDiffuseFn.Enabled:
                                wl(aAttn, NdotLName);
                                break;
                            case GXDiffuseFn.Clamped:
                                wl(aAttn, "satlf(" + NdotLName + ")");
                                break;
                        }

                        #endregion

                        wl();

                        //Create variables used by both attenuation passes only if either is used
                        if (color.Attenuation != GXAttnFn.None ||
                            alpha.Attenuation != GXAttnFn.None)
                        {
                            //Distance and angular quadratic coefficients
                            wl("float k0 = 1.0, k1 = 0.0, k2 = 0.0;");
                            wl("float a0 = 1.0, a1 = 0.0, a2 = 0.0;");

                            wl("vec3 {0} = gl_NormalMatrix * {1}.{2};", lightDirName, lightName, LightDirName);
                        }

                        if (color.Enabled)
                        {
                            wl();
                            CalcAttn(color.Attenuation, true, illumName);
                        }

                        if (alpha.Enabled)
                        {
                            wl();
                            CalcAttn(alpha.Attenuation, false, illumName);
                        }
                    } //End if light enabled
                    CloseBracket();
                } //End light loop
                CloseBracket();

                //Clamp illumination
                wl("{0} = clamp({0}, {1}, {2});", illumName, vec4Zero, vec4One);
            }

            //Set light function
            if (color.Enabled == alpha.Enabled)
            {
                wl("{0} = {1};", lightFuncName, color.Enabled ? illumName : vec4One);
            }
            else
            {
                wl("{0}.rgb = {1};", lightFuncName, color.Enabled ? illumName + ".rgb" : vec3One);
                wl("{0}.a = {1};", lightFuncName, alpha.Enabled ? illumName + ".a" : "1.0");
            }

            //Set material base color
            GXColorSrc colorSrc = color.MaterialSource;
            GXColorSrc alphaSrc = alpha.MaterialSource;
            if (colorSrc == alphaSrc)
            {
                wl("{0} = {1};", matColorName,
                    colorSrc == GXColorSrc.Register ? clr : _vVtxColorsName[index]);
            }
            else
            {
                wl("{0}.rgb = {1}.rgb;", matColorName,
                    colorSrc == GXColorSrc.Register ? clr : _vVtxColorsName[index]);
                wl("{0}.a = {1}.a;", matColorName,
                    alphaSrc == GXColorSrc.Register ? clr : _vVtxColorsName[index]);
            }

            //Multiply material base color by the light function value to get the light channel value
            wl("{0}{1} = {2} * {3};", LightChannelName, index, matColorName, lightFuncName);
            wl();
            return null;
        }

        private const string colorPassSuffix = "C";
        private const string alphaPassSuffix = "A";
        private const string attnName = "attn";
        private const string AAttName = "AAtt";
        private const string distName = "distL";
        private const string NdotLName = "NdotL";
        private const string lightDirName = "lightDir";
        private const string lightVecName = "lightVec";
        private static readonly string lightName = "lightAnimFrame";
        private const string lightColorName = "lightColor";

        private static void CalcAttn(GXAttnFn fn, bool colorPass, string illumName)
        {
            string suffix = colorPass ? colorPassSuffix : alphaPassSuffix;
            string part = colorPass ? ".rgb" : ".a";

            Comment("{0} attenuation pass", colorPass ? "Color" : "Alpha");

            if (fn == GXAttnFn.None)
            {
                Comment("No Attn");

                //Use diffuse color
                wl("{0}{3} = {1}.{2}{3};", lightColorName, lightName, LightColorName, part);
            }
            else
            {
                //If specular, don't do anything if this isn't a specular light
                if (fn == GXAttnFn.Specular)
                {
                    Comment("Spec Attn");
                    wl("if ({0}.{1} != 0)", lightName, LightSpecEnabledName);
                    OpenBracket();
                }
                else
                {
                    Comment("Spot Attn");
                }

                //Write floats for light attenuation color equation
                wl("float {0}{1} = 1.0;", AAttName, suffix); //Angular attenuation
                wl("float {0}{1} = 1.0;", distName, suffix); //Distance attenuation

                switch (fn)
                {
                    case GXAttnFn.Spotlight:

                        wl("k0 = {0}.{1}[0];", lightName, LightDistCoefsName);
                        wl("k1 = {0}.{1}[1];", lightName, LightDistCoefsName);
                        wl("k2 = {0}.{1}[2];", lightName, LightDistCoefsName);
                        wl("a0 = {0}.{1}[0];", lightName, LightSpotCoefsName);
                        wl("a1 = {0}.{1}[1];", lightName, LightSpotCoefsName);
                        wl("a2 = {0}.{1}[2];", lightName, LightSpotCoefsName);

                        //Use diffuse color
                        wl("{0}{3} = {1}.{2}{3};",
                            lightColorName, lightName, LightColorName, part);

                        //Set dist
                        //length = sqrt(dot(lightVec, lightVec))
                        wl("{0}{2} = length({1});",
                            distName, lightVecName, suffix);

                        //Set angular value
                        wl("{0}{3} = satlf(dot({1}, {2}));",
                            AAttName, lightVecName, lightDirName, suffix);

                        break;
                    case GXAttnFn.Specular:

                        wl("k0 = {0}.{1}[0];", lightName, LightDistCoefsSpecName);
                        wl("k1 = {0}.{1}[1];", lightName, LightDistCoefsSpecName);
                        wl("k2 = {0}.{1}[2];", lightName, LightDistCoefsSpecName);
                        wl("a0 = 0.0;");
                        wl("a1 = 0.0;");
                        wl("a2 = 1.0;");

                        //Use specular color
                        wl("{0}{3} = {1}.{2}{3};", lightColorName, lightName, LightSpecColorName, part);

                        string halfVecName = "halfVec";
                        wl("vec3 {0} = normalize({1} + normalize(-{2}));",
                            halfVecName, lightVecName, _vPositionName);

                        //Set dist
                        wl("{0}{4} = {1} > 0.0 ? satlf(dot(normalize({2}), {3})): 0.0;",
                            distName, NdotLName, _vNormalName, halfVecName, suffix);

                        //Set angular value, same as dist
                        wl("{0}{2} = {1}{2};",
                            AAttName, distName, suffix);

                        break;
                }

                string equation = "{2}2 * {0}{1} * {0}{1} + {2}1 * {0}{1} + {2}0";
                string numerator = string.Format(equation, AAttName, suffix, "a");
                string denominator = string.Format(equation, distName, suffix, "k");

                wl("{0}{3} *= satlf({1}) / ({2});", attnName, numerator, denominator, suffix);

                if (fn == GXAttnFn.Specular)
                {
                    CloseBracket();
                    wl("else");
                    OpenBracket();
                    wl("{0}{1} *= 0.0;", attnName, suffix);
                    CloseBracket();
                }
            }

            //Add the light color multiplied by the attenuation
            wl("{0}{4} += ({3}{4} * {1}{2});", illumName, attnName, suffix, lightColorName, part);
        }

        public static string[] GenTEVFragShader()
        {
            BuildSwapModeTable(_shaderNode);

#if DEBUG
            _vertex = false;
#endif
            Reset();

            string[] arr = new string[_shaderNode.Children.Count + 1];

            bool usesTex = false;
            bool usesRas = false;
            bool usesConst = false;
            bool[] usesReg = new bool[3];

            int x = 1;
            foreach (MDL0TEVStageNode stage in _shaderNode.Children)
            {
                if (!usesTex)
                {
                    usesTex = stage.AnyTextureSourceUsed();
                }

                if (!usesRas)
                {
                    usesRas = stage.AnyRasterSourceUsed();
                }

                if (!usesConst)
                {
                    usesConst = stage.AnyConstantColorSourceUsed() || stage.AnyConstantAlphaSourceUsed();
                }

                if (!usesReg[0])
                {
                    usesReg[0] = stage.AnyReg0Used();
                }

                if (!usesReg[1])
                {
                    usesReg[1] = stage.AnyReg1Used();
                }

                if (!usesReg[2])
                {
                    usesReg[2] = stage.AnyReg2Used();
                }

                string error = WriteStage(stage);
                if (error != null)
                {
                    return new[] {error};
                }

                wl();

                arr[x++] = _shaderCode;
                Reset();
            }

            for (int i = 1; i < 4; i++)
            {
                if (usesReg[i - 1])
                {
                    wl("vec4 {0} = {1};", _outReg[i], _defaultRegisterValues[i]);
                }
            }

            if (usesTex)
            {
                wl("vec4 {0};", _texColorName);
            }

            if (usesRas)
            {
                wl("vec4 {0};", _rasColorName);
            }

            if (usesConst)
            {
                wl("vec4 {0};", _constColorName);
            }

            wl();

            arr[0] = _shaderCode;
            Reset();

            return arr;
        }

        public static string WriteStage(MDL0TEVStageNode stage)
        {
            string identifier = $"{stage.Parent.Name} {stage.Name}";

            Comment(identifier);
            OpenBracket();

            string rswap = swapModeTable[(int) stage.RasterSwap];
            string tswap = swapModeTable[(int) stage.TextureSwap];

            if (stage.AnyConstantColorSourceUsed())
            {
                wl(_constColorName + ".rgb = {0};", _cConst[(int) stage.ConstantColorSelection]);
            }

            if (stage.AnyConstantAlphaSourceUsed())
            {
                wl(_constColorName + ".a = {0};", _aConst[(int) stage.ConstantAlphaSelection]);
            }

            if (stage.AnyTextureSourceUsed())
            {
                if (stage.TextureEnabled)
                {
                    int mapID = (int) stage.TextureMapID;
                    int coordID = (int) stage.TextureCoordID;

                    if (UsePixelLighting)
                    {
                        wl(_texColorName + " = texture2D(texture{0}, uv{1}.st).{2};",
                            mapID.ToString(), coordID.ToString(), tswap);
                    }
                    else
                    {
                        wl(_texColorName + " = texture2D(texture{0}, gl_TexCoord[{1}].st).{2};",
                            mapID.ToString(), coordID.ToString(), tswap);
                    }
                }
                else
                {
                    wl(_texColorName + " = {0};", vec4Zero);
                }
            }

            if (stage.AnyRasterSourceUsed())
            {
                switch (stage.RasterColor)
                {
                    case ColorSelChan.LightChannel0:
                    case ColorSelChan.LightChannel1:
                        int id = (int) stage.RasterColor - (int) ColorSelChan.LightChannel0;
                        wl(_rasColorName + " = {0}{1}.{2};", LightChannelName, id.ToString(), rswap);
                        break;
                    case ColorSelChan.BumpAlpha:
                    case ColorSelChan.NormalizedBumpAlpha:
                    //WHAT DO?
                    //break;
                    case ColorSelChan.Zero:
                    default:
                        wl(_rasColorName + " = {0};", vec4Zero);
                        break;
                }
            }

            string reg, ca, cb, cc, cd, aa, ab, ac, ad;

            wl();
            Comment("Color Operation");

            ColorEnv color = stage._colorEnv;

            reg = _outReg[(int) color.Dest] + ".rgb";
            ca = TruncCSel((int) color.SelA);
            cb = TruncCSel((int) color.SelB);
            cc = TruncCSel((int) color.SelC);
            cd = TruncCSel((int) color.SelD);

            TevColorOp cOp = color.Operation;
            int icOp = (int) cOp;
            if (icOp <= 1)
            {
                int bias = (int) color.Bias;
                int scale = (int) color.Shift;

                wl(string.Format("{0} = ({4} {5} (mix({1},{2},{3})){6}){7};",
                    reg, ca, cb, cc, cd,
                    cOp == TevColorOp.Add ? "+" : "-",
                    _tevBiasName[bias],
                    _tevScaleName[scale]));
                if (color.Clamp)
                {
                    wl("{0} = satv({0});", reg, vec3Zero, vec3One);
                }
            }
            else if (icOp >= 8 && icOp <= 15)
            {
                bool greater = icOp % 2 == 0;

                //255 divided down to 1 has an accuracy of ~0.00392 between values
                //use abs() < 0.005 to compare equality of floats that may not be accurate
                string comp = greater ? ca + ".{0} > " + cb + ".{0}" : "abs(" + ca + ".{0} - " + cb + ".{0}) < 0.005";

                string compAdd = "if ({0}) " + reg + ".{1} += " + cc + ".{1};";

                //Write d to the register
                wl("{0}.{2} = {1}.{2};", reg, cd, "rgb");

                //Compare a and b selections and add c to the register
                if (icOp > 13)
                {
                    wl(compAdd, string.Format(comp, "r"), "r");
                    wl(compAdd, string.Format(comp, "g"), "g");
                    wl(compAdd, string.Format(comp, "b"), "b");
                }
                else
                {
                    switch (icOp / 2 - 4)
                    {
                        case 0: //R
                            wl(compAdd,
                                string.Format(comp, "r"),
                                "rgb");
                            break;
                        case 1: //GR
                            wl(compAdd,
                                string.Format(comp, "r") + " && " +
                                string.Format(comp, "g"),
                                "rgb");
                            break;
                        case 2: //BGR
                            wl(compAdd,
                                string.Format(comp, "r") + " && " +
                                string.Format(comp, "g") + " && " +
                                string.Format(comp, "b"),
                                "rgb");
                            break;
                    }
                }
            }

            wl();
            Comment("Alpha Operation");

            AlphaEnv alpha = stage._alphaEnv;

            reg = _outReg[(int) alpha.Dest] + ".a";
            aa = TruncASel((int) alpha.SelA);
            ab = TruncASel((int) alpha.SelB);
            ac = TruncASel((int) alpha.SelC);
            ad = TruncASel((int) alpha.SelD);

            TevAlphaOp aOp = alpha.Operation;
            int iaOp = (int) aOp;
            if (iaOp <= 1)
            {
                int bias = (int) alpha.Bias;
                int scale = (int) alpha.Shift;

                wl(string.Format("{0} = ({4} {5} (mix({1},{2},{3})){6}){7};",
                    reg, aa, ab, ac, ad,
                    aOp == TevAlphaOp.Add ? "+" : "-",
                    _tevBiasName[bias],
                    _tevScaleName[scale]));
                if (alpha.Clamp)
                {
                    wl("{0} = satf({0});", reg);
                }
            }
            else if (iaOp >= 8 && iaOp <= 15)
            {
                bool greater = iaOp % 2 == 0;

                string compAdd = "if ({0}) " + reg + ".{1} += " + ac + ".{1};";

                //Write d to the register
                wl("{0}.{2} = {1}.{2};", reg, ad, "a");

                //Compare a and b selections and add c to the register
                if (iaOp > 13)
                {
                    string comp = greater
                        ? aa + ".{0} > " + ab + ".{0}"
                        : "abs(" + aa + ".{0} - " + ab + ".{0}) < 0.005";

                    wl(compAdd, string.Format(comp, "a"), "a");
                }
                else
                {
                    //This actually compares color values, not a typo
                    string comp = greater
                        ? ca + ".{0} > " + cb + ".{0}"
                        : "abs(" + ca + ".{0} - " + cb + ".{0}) < 0.005";

                    switch (iaOp / 2 - 4)
                    {
                        case 0: //R
                            wl(compAdd,
                                string.Format(comp, "r"),
                                "a");
                            break;
                        case 1: //GR
                            wl(compAdd,
                                string.Format(comp, "r") + " && " +
                                string.Format(comp, "g"),
                                "a");
                            break;
                        case 2: //BGR
                            wl(compAdd,
                                string.Format(comp, "r") + " && " +
                                string.Format(comp, "g") + " && " +
                                string.Format(comp, "b"),
                                "a");
                            break;
                    }
                }
            }

            CloseBracket();

            return null;
        }

        public static void BuildSwapModeTable(MDL0ShaderNode node)
        {
            string swapColors = "rgba";
            swapModeTable = new string[4];

            //Iterate through the swaps
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        swapModeTable[i] = new string(new char[]
                        {
                            swapColors[(int) node.Swap0Red],
                            swapColors[(int) node.Swap0Green],
                            swapColors[(int) node.Swap0Blue],
                            swapColors[(int) node.Swap0Alpha]
                        });
                        break;
                    case 1:
                        swapModeTable[i] = new string(new char[]
                        {
                            swapColors[(int) node.Swap1Red],
                            swapColors[(int) node.Swap1Green],
                            swapColors[(int) node.Swap1Blue],
                            swapColors[(int) node.Swap1Alpha]
                        });
                        break;
                    case 2:
                        swapModeTable[i] = new string(new char[]
                        {
                            swapColors[(int) node.Swap2Red],
                            swapColors[(int) node.Swap2Green],
                            swapColors[(int) node.Swap2Blue],
                            swapColors[(int) node.Swap2Alpha]
                        });
                        break;
                    case 3:
                        swapModeTable[i] = new string(new char[]
                        {
                            swapColors[(int) node.Swap3Red],
                            swapColors[(int) node.Swap3Green],
                            swapColors[(int) node.Swap3Blue],
                            swapColors[(int) node.Swap3Alpha]
                        });
                        break;
                }
            }
        }

        #region String Helpers

        private static string Tabs
        {
            get
            {
                string t = "";
                for (int i = 0; i < tabCount; i++)
                {
                    t += "\t";
                }

                return t;
            }
        }

        private static int tabCount;
        public const string NewLine = "\n";

        public static void Reset()
        {
            _shaderCode = "";
            tabCount = 0;
        }

        public static void WriteVersion()
        {
            wl("#version {0}", GLSLVersion.ToString());
            wl();
        }

        public static void Begin()
        {
            wl("void main()");
            OpenBracket();
        }

        public static string Finish()
        {
            CloseBracket();
            return _shaderCode;
        }

        private static void Comment(string comment, params object[] args)
        {
            wl("//" + comment, args);
        }

        private static void wU(string uniform, params object[] args)
        {
            wl("uniform " + uniform, args);
        }

        private static void wV(string varying, params object[] args)
        {
            wl("varying " + varying, args);
        }

        /// <summary>
        /// Writes the current line and increments to the next line.
        /// Do not use arguments if you need to include brackets in the string.
        /// </summary>
        private static void wl(string str = "", params object[] args)
        {
            str += NewLine;

            //Decrease tabs for every close bracket
            if (args.Length == 0)
            {
                tabCount -= Helpers.FindCount(str, 0, '}');
            }

            bool s = false;
            int r = str.LastIndexOf(NewLine);
            if (r == str.Length - NewLine.Length)
            {
                str = str.Substring(0, str.Length - NewLine.Length);
                s = true;
            }

            str = str.Replace(NewLine, NewLine + Tabs);
            if (s)
            {
                str += NewLine;
            }

            _shaderCode += Tabs + (args != null && args.Length > 0 ? string.Format(str, args) : str);

            //Increase tabs for every open bracket
            if (args.Length == 0)
            {
                tabCount += Helpers.FindCount(str, 0, '{');
            }
        }

        private static void OpenBracket()
        {
            wl("{");
        }

        private static void CloseBracket()
        {
            wl("}");
        }

        #endregion

        #region Uniform Types

        private static void Uniform(int pHandle, string name, params Vector4[] p)
        {
            int u = GL.GetUniformLocation(pHandle, name);
            if (u > -1)
            {
                float[] values = new float[p.Length * 4];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = ((float*) p[i >> 2].Address)[i & 3];
                }

                GL.Uniform4(u, p.Length, values);
            }
        }

        private static void Uniform(int pHandle, string name, params Vector3[] p)
        {
            int u = GL.GetUniformLocation(pHandle, name);
            if (u > -1)
            {
                float[] values = new float[p.Length * 3];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = ((float*) p[i / 3].Address)[i % 3];
                }

                GL.Uniform3(u, p.Length, values);
            }
        }

        private static void Uniform(int pHandle, string name, params float[] p)
        {
            int u = GL.GetUniformLocation(pHandle, name);
            if (u > -1)
            {
                GL.Uniform1(u, p.Length, p);
            }
        }

        private static void Uniform(int pHandle, string name, params int[] p)
        {
            int u = GL.GetUniformLocation(pHandle, name);
            if (u > -1)
            {
                GL.Uniform1(u, p.Length, p);
            }
        }

        private static void Uniform(int pHandle, string name, GLSLLightFrame[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                GLSLLightFrame frame = p[i];
                string x = $"{name}[{i}].";
                Uniform(pHandle, x + LightEnabledName, frame.Enabled);
                Uniform(pHandle, x + LightSpecEnabledName, frame.SpecEnabled);
                Uniform(pHandle, x + LightPosName, frame.Position);
                Uniform(pHandle, x + LightDirName, frame.Direction);
                Uniform(pHandle, x + LightColorName, frame.DiffColor);
                Uniform(pHandle, x + LightSpecColorName, frame.SpecColor);
                Uniform(pHandle, x + LightDistCoefsName, frame.DiffK);
                Uniform(pHandle, x + LightSpotCoefsName, frame.DiffA);
                Uniform(pHandle, x + LightDistCoefsSpecName, frame.SpecK);
            }
        }

        #endregion

        #region Light

        private const string LightStructName = "LightFrame";
        private const string LightEnabledName = "enabled";
        private const string LightSpecEnabledName = "hasSpecular";
        private const string LightPosName = "pos";
        private const string LightDirName = "dir";
        private const string LightColorName = "color";
        private const string LightSpecColorName = "specColor";
        private const string LightDistCoefsName = "distCoefs";
        private const string LightSpotCoefsName = "spotCoefs";
        private const string LightDistCoefsSpecName = "distCoefsSpec";

        private static void WriteLightFrameStruct()
        {
            wl("struct {0}", LightStructName);
            wl("{");
            wl("int {0};", LightEnabledName);
            wl("vec3 {0};", LightPosName);
            wl("vec3 {0};", LightDirName);
            wl();
            Comment("Diffuse light");
            wl("vec4 {0};", LightColorName);
            wl("vec3 {0};", LightDistCoefsName); //K
            wl("vec3 {0};", LightSpotCoefsName); //A
            wl();
            Comment("Specular light");
            wl("int {0};", LightSpecEnabledName);
            wl("vec4 {0};", LightSpecColorName);
            wl("vec3 {0};", LightDistCoefsSpecName); //K
            //0.0,0.0,1.0 = A
            wl("};");
            wl();
        }

        #endregion

        #region Variable Names

        private const string Trunc1Name = "truncc1";
        private const string Trunc3Name = "truncc3";
        private const string Trunc4Name = "truncc4";
        private const string vec4Zero = "vec4(0.0)";
        private const string vec4Half = "vec4(0.5)";
        private const string vec4One = "vec4(1.0)";
        private const string vec3Zero = "vec3(0.0)";
        private const string vec3Half = "vec3(0.5)";
        private const string vec3One = "vec3(1.0)";
        private const string _fogDensityName = "fogDensity";
        private const string _texColorName = "texColor";
        private const string _rasColorName = "rasColor";
        private const string _constColorName = "constColor";
        private static readonly string[] _outReg = {"rPrev", "r0", "r1", "r2"};

        private static string PrevRegName => _outReg[0];

        private static readonly string[] _texGenSrc =
        {
            "gl_Vertex",
            "vec4(gl_Normal, 1.0)",
            LightChannelName + "{0}",
            "BinormalsT", //Unsupported
            "BinormalsB", //Unsupported
            "gl_MultiTexCoord0",
            "gl_MultiTexCoord1",
            "gl_MultiTexCoord2",
            "gl_MultiTexCoord3",
            "gl_MultiTexCoord4",
            "gl_MultiTexCoord5",
            "gl_MultiTexCoord6",
            "gl_MultiTexCoord7"
        };

        private static readonly string[] _defaultRegisterValues =
        {
            vec4One,
            _uaMatColorName[2] + "[0]",
            _uaMatColorName[2] + "[1]",
            _uaMatColorName[2] + "[2]"
        };

        private static string TruncCSel(int i)
        {
            return DoTrunc && i < 8 ? $"{Trunc3Name}({_cSel[i]})" : _cSel[i];
        }

        private static string TruncASel(int i)
        {
            return DoTrunc && i < 4 ? $"{Trunc1Name}({_aSel[i]})" : _aSel[i];
        }

        private static readonly string[] _cSel =
        {
            _outReg[0] + ".rgb", _outReg[0] + ".aaa",
            _outReg[1] + ".rgb", _outReg[1] + ".aaa",
            _outReg[2] + ".rgb", _outReg[2] + ".aaa",
            _outReg[3] + ".rgb", _outReg[3] + ".aaa",
            _texColorName + ".rgb", _texColorName + ".aaa",
            _rasColorName + ".rgb", _rasColorName + ".aaa",
            vec3One,
            vec3Half,
            _constColorName + ".rgb",
            vec3Zero
        };

        private static readonly string[] _aSel =
        {
            _outReg[0] + ".a",
            _outReg[1] + ".a",
            _outReg[2] + ".a",
            _outReg[3] + ".a",
            _texColorName + ".a",
            _rasColorName + ".a",
            _constColorName + ".a",
            "0.0"
        };

        private static readonly string[] _cConst =
        {
            //Constants
            vec3One,
            "vec3(0.875,0.875,0.875)",
            "vec3(0.75,0.75,0.75)",
            "vec3(0.625,0.625,0.625)",
            vec3Half,
            "vec3(0.375,0.375,0.375)",
            "vec3(0.25,0.25,0.25)",
            "vec3(0.125,0.125,0.125)",
            //8 - 11 not used, skip
            "", "", "", "",
            //Constant color selections
            _uaMatColorName[3] + "[0].rgb", _uaMatColorName[3] + "[1].rgb", _uaMatColorName[3] + "[2].rgb",
            _uaMatColorName[3] + "[3].rgb",
            _uaMatColorName[3] + "[0].rrr", _uaMatColorName[3] + "[1].rrr", _uaMatColorName[3] + "[2].rrr",
            _uaMatColorName[3] + "[3].rrr",
            _uaMatColorName[3] + "[0].ggg", _uaMatColorName[3] + "[1].ggg", _uaMatColorName[3] + "[2].ggg",
            _uaMatColorName[3] + "[3].ggg",
            _uaMatColorName[3] + "[0].bbb", _uaMatColorName[3] + "[1].bbb", _uaMatColorName[3] + "[2].bbb",
            _uaMatColorName[3] + "[3].bbb",
            _uaMatColorName[3] + "[0].aaa", _uaMatColorName[3] + "[1].aaa", _uaMatColorName[3] + "[2].aaa",
            _uaMatColorName[3] + "[3].aaa"
        };

        private static readonly string[] _aConst =
        {
            //Constants
            "1.0", "0.875", "0.75", "0.625", "0.5", "0.375", "0.25", "0.125",
            //8 - 15 not used, skip
            "", "", "", "", "", "", "", "",
            //Constant alpha selections
            _uaMatColorName[3] + "[0].r", _uaMatColorName[3] + "[1].r", _uaMatColorName[3] + "[2].r",
            _uaMatColorName[3] + "[3].r",
            _uaMatColorName[3] + "[0].g", _uaMatColorName[3] + "[1].g", _uaMatColorName[3] + "[2].g",
            _uaMatColorName[3] + "[3].g",
            _uaMatColorName[3] + "[0].b", _uaMatColorName[3] + "[1].b", _uaMatColorName[3] + "[2].b",
            _uaMatColorName[3] + "[3].b",
            _uaMatColorName[3] + "[0].a", _uaMatColorName[3] + "[1].a", _uaMatColorName[3] + "[2].a",
            _uaMatColorName[3] + "[3].a"
        };

        private static readonly string[] _tevBiasName = {"", " + 0.5", " - 0.5"};
        private static readonly string[] _tevScaleName = {"", " * 2.0", " * 4.0", " * 0.5"};

        private static readonly string[] _alphaTestCompName =
        {
            "{0} != {0}",
            "{0} < {1}",
            "{0} == {1}",
            "{0} <= {1}",
            "{0} > {1}",
            "{0} != {1}",
            "{0} >= {1}",
            "{0} == {0}"
        };

        private static readonly string[] _alphaTestCombineName =
        {
            "({0}) && ({1})",
            "({0}) || ({1})",
            "(({0}) && (!({1}))) || ((!({0})) && ({1}))",
            "(({0}) && ({1})) || ((!({0})) && (!({1})))"
        };

        #endregion

        #region Other Functions

        private static string HandleProblem(string message)
        {
#if DEBUG
            System.Windows.Forms.MessageBox.Show(_material.RootNode._mainForm, message,
                $"Handled error compiling {(_vertex ? "vertex" : "fragment")} shader",
                System.Windows.Forms.MessageBoxButtons.OK);
#endif
            return "Error";
        }

        internal static void TryCompile(int programHandle, int shaderHandle, string source)
        {
            GL.ShaderSource(shaderHandle, source);
            GL.CompileShader(shaderHandle);

#if DEBUG
            GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out int status);
            if (status == 0 || AlwaysOutputShader ||
                System.Windows.Forms.Control.ModifierKeys == System.Windows.Forms.Keys.Control)
            {
                GL.GetShaderInfoLog(shaderHandle, out string info);
                Console.WriteLine(info + "\n\n");

                //Split the source by new lines
                string[] s = source.Split(new[] {NewLine}, StringSplitOptions.None);

                //Add the line number to the source so we can go right to errors on specific lines
                int lineNumber = 1;
                foreach (string line in s)
                {
                    Console.WriteLine($"{(lineNumber++).ToString().PadLeft(s.Length.ToString().Length, '0')}: {line}");
                }

                Console.WriteLine("\n\n");
            }
#endif
        }

        public static void SetTarget(MDL0MaterialNode mat)
        {
            _material = mat;
            _shaderNode = mat.ShaderNode;
        }

        public static void ClearTarget()
        {
            _material = null;
            _shaderNode = null;
        }

        #endregion

        public static string CombineFragShader(string matSource, string[] shaderStages, int stageCount)
        {
            string[] fragSplit = matSource.Split('%');

            //Inject the tev operations into the material shader
            string combineFrag = fragSplit[0];
            if (fragSplit.Length == 3)
            {
                if (shaderStages != null)
                {
                    //Get base tabs
                    string tabs = "";
                    if (fragSplit.Length == 3)
                    {
                        int tabCount = int.Parse(fragSplit[1]);
                        for (int i = 0; i < tabCount; i++)
                        {
                            tabs += "\t";
                        }
                    }

                    for (int i = 0, stageIndex = 0; i < shaderStages.Length; i++)
                    {
                        if (i > 0)
                        {
                            //Don't write stages that aren't active
                            if (stageIndex >= stageCount)
                            {
                                break;
                            }

                            stageIndex++;
                        }

                        string[] shadSplit = shaderStages[i].Split(
                            new[] {NewLine},
                            StringSplitOptions.None);

                        foreach (string line in shadSplit)
                        {
                            combineFrag += tabs + line + NewLine;
                        }
                    }
                }

                combineFrag += fragSplit[2];
            }

            return combineFrag;
        }
    }
}