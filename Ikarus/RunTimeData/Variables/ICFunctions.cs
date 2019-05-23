using System;
using Ikarus.MovesetFile;
using Ikarus.ModelViewer;

namespace Ikarus
{
    public static unsafe partial class IC
    {
        /// <summary>
        /// Returns a constant IC from Fighter.pac
        /// </summary>
        private static VoidPtr GetConst(int id, bool global, bool sse)
        {
            bool temp = Manager.InSubspaceEmissary;
            Manager.InSubspaceEmissary = sse;
            RawParamList list = global ? Manager.GetGlobalICs() : Manager.GetICs();
            Manager.InSubspaceEmissary = temp;
            if (id >= 0 && id <= list.AttributeBuffer.Length / 4)
                return list.AttributeBuffer[id, 4];
            return 0;
        }

        //Add IC variable functions here!
        //The name of the function has to be the number of the IC
        //with some character in front of it. I prefer an underscore.
        //If the return type is int, the function is a Basic function.
        //If it's float, then it's Float. bool; Bit.
        //Make sure the function is private and static, otherwise it will not be read.
        #region Functions

        //Current Frame
        private static int _0() { return RunTime.CurrentFrame; }
        //Damage
        private static int _2() { return 0; }
        //Character X
        private static int _3()
        {
            if (Manager.Moveset != null &&
                Manager.Moveset.Data != null &&
                Manager.Moveset.Data._misc != null)
            {
                Vector3 p = Manager.Moveset.Data._misc._boneRefs[4].BoneNode._frameMatrix.GetPoint();
                return (int)(p._x < 0 ? p._x - 0.5f : p._x + 0.5f);
            }
            return 0; 
        }
        //Character Y
        private static int _4()
        {
            if (Manager.Moveset != null &&
                Manager.Moveset.Data != null &&
                Manager.Moveset.Data._misc != null)
            {
                Vector3 p = Manager.Moveset.Data._misc._boneRefs[4].BoneNode._frameMatrix.GetPoint();
                return (int)(p._y < 0 ? p._y - 0.5f : p._y + 0.5f);
            }
            return 0;
        }
        //Control Stick Forward, 0 to 1
        private static int _1011() { return 0; }
        //Control Stick Backward, 0 to 1
        private static int _1012() { return 0; }
        //Control Stick Upward, 0 to 1
        private static int _1018() { return 0; }
        //Control Stick Downward, 0 to 1
        private static int _1020() { return 0; }

        //Current Subaction
        private static int _20000() { return RunTime.CurrentSubaction != null ? RunTime.CurrentSubaction.ID : -1; }
        //Current Action
        private static int _20001() { return RunTime.CurrentAction != null ? RunTime.CurrentAction.ID : -1; }
        //Previous Action
        private static int _20003() { return RunTime.PreviousAction != null ? RunTime.PreviousAction.ID : -1; }

        #region Constants

        #region Global

        #region Basic
        private static int _22001() { return *(bint*)GetConst(4, true, false); }
        private static int _22002() { return *(bint*)GetConst(5, true, false); }
        private static int _22003() { return *(bint*)GetConst(17, true, false); }
        private static int _22004() { return *(bint*)GetConst(22, true, false); }
        private static int _22005() { return *(bint*)GetConst(46, true, false); }
        private static int _22006() { return *(bint*)GetConst(4, true, true); }
        private static int _22007() { return *(bint*)GetConst(5, true, true); }
        private static int _22008() { return *(bint*)GetConst(17, true, true); }
        private static int _22009() { return *(bint*)GetConst(22, true, true); }
        private static int _22010() { return *(bint*)GetConst(46, true, true); }
        #endregion

        #region Float
        private static float _2002() { return *(bfloat*)GetConst(0, true, false); }
        private static float _2003() { return *(bfloat*)GetConst(1, true, false); }
        private static float _2004() { return *(bfloat*)GetConst(2, true, false); }
        private static float _2005() { return *(bfloat*)GetConst(3, true, false); }
        private static float _2006() { return *(bfloat*)GetConst(6, true, false); }
        private static float _2007() { return *(bfloat*)GetConst(7, true, false); }
        private static float _2008() { return *(bfloat*)GetConst(8, true, false); }
        private static float _2009() { return *(bfloat*)GetConst(9, true, false); }
        private static float _2010() { return *(bfloat*)GetConst(10, true, false); }
        private static float _2011() { return *(bfloat*)GetConst(11, true, false); }
        private static float _2012() { return *(bfloat*)GetConst(12, true, false); }
        private static float _2013() { return *(bfloat*)GetConst(13, true, false); }
        private static float _2014() { return *(bfloat*)GetConst(14, true, false); }
        private static float _2015() { return *(bfloat*)GetConst(15, true, false); }
        private static float _2016() { return *(bfloat*)GetConst(16, true, false); }
        private static float _2017() { return *(bfloat*)GetConst(18, true, false); }
        private static float _2018() { return *(bfloat*)GetConst(19, true, false); }
        private static float _2019() { return *(bfloat*)GetConst(20, true, false); }
        private static float _2020() { return *(bfloat*)GetConst(21, true, false); }
        private static float _2021() { return *(bfloat*)GetConst(23, true, false); }
        private static float _2022() { return *(bfloat*)GetConst(24, true, false); }
        private static float _2023() { return *(bfloat*)GetConst(27, true, false); }
        private static float _2024() { return *(bfloat*)GetConst(28, true, false); }
        private static float _2025() { return *(bfloat*)GetConst(29, true, false); }
        private static float _2026() { return *(bfloat*)GetConst(30, true, false); }
        private static float _2027() { return *(bfloat*)GetConst(31, true, false); }
        private static float _2028() { return *(bfloat*)GetConst(32, true, false); }
        private static float _2029() { return *(bfloat*)GetConst(33, true, false); }
        private static float _2030() { return *(bfloat*)GetConst(34, true, false); }
        private static float _2031() { return *(bfloat*)GetConst(35, true, false); }
        private static float _2032() { return *(bfloat*)GetConst(36, true, false); }
        private static float _2033() { return *(bfloat*)GetConst(37, true, false); }
        private static float _2034() { return *(bfloat*)GetConst(38, true, false); }
        private static float _2035() { return *(bfloat*)GetConst(39, true, false); }
        private static float _2036() { return *(bfloat*)GetConst(40, true, false); }
        private static float _2037() { return *(bfloat*)GetConst(41, true, false); }
        private static float _2038() { return *(bfloat*)GetConst(42, true, false); }
        private static float _2039() { return *(bfloat*)GetConst(43, true, false); }
        private static float _2040() { return *(bfloat*)GetConst(44, true, false); }
        private static float _2041() { return *(bfloat*)GetConst(45, true, false); }
        private static float _2042() { return *(bfloat*)GetConst(0, true, true); }
        private static float _2043() { return *(bfloat*)GetConst(1, true, true); }
        private static float _2044() { return *(bfloat*)GetConst(2, true, true); }
        private static float _2045() { return *(bfloat*)GetConst(3, true, true); }
        private static float _2046() { return *(bfloat*)GetConst(6, true, true); }
        private static float _2047() { return *(bfloat*)GetConst(7, true, true); }
        private static float _2048() { return *(bfloat*)GetConst(8, true, true); }
        private static float _2049() { return *(bfloat*)GetConst(9, true, true); }
        private static float _2050() { return *(bfloat*)GetConst(10, true, true); }
        private static float _2051() { return *(bfloat*)GetConst(11, true, true); }
        private static float _2052() { return *(bfloat*)GetConst(12, true, true); }
        private static float _2053() { return *(bfloat*)GetConst(13, true, true); }
        private static float _2054() { return *(bfloat*)GetConst(14, true, true); }
        private static float _2055() { return *(bfloat*)GetConst(15, true, true); }
        private static float _2056() { return *(bfloat*)GetConst(16, true, true); }
        private static float _2057() { return *(bfloat*)GetConst(18, true, true); }
        private static float _2058() { return *(bfloat*)GetConst(19, true, true); }
        private static float _2059() { return *(bfloat*)GetConst(20, true, true); }
        private static float _2060() { return *(bfloat*)GetConst(21, true, true); }
        private static float _2061() { return *(bfloat*)GetConst(23, true, true); }
        private static float _2062() { return *(bfloat*)GetConst(24, true, true); }
        private static float _2063() { return *(bfloat*)GetConst(27, true, true); }
        private static float _2064() { return *(bfloat*)GetConst(28, true, true); }
        private static float _2065() { return *(bfloat*)GetConst(29, true, true); }
        private static float _2066() { return *(bfloat*)GetConst(30, true, true); }
        private static float _2067() { return *(bfloat*)GetConst(31, true, true); }
        private static float _2068() { return *(bfloat*)GetConst(32, true, true); }
        private static float _2069() { return *(bfloat*)GetConst(33, true, true); }
        private static float _2070() { return *(bfloat*)GetConst(34, true, true); }
        private static float _2071() { return *(bfloat*)GetConst(35, true, true); }
        private static float _2072() { return *(bfloat*)GetConst(36, true, true); }
        private static float _2073() { return *(bfloat*)GetConst(37, true, true); }
        private static float _2074() { return *(bfloat*)GetConst(38, true, true); }
        private static float _2075() { return *(bfloat*)GetConst(39, true, true); }
        private static float _2076() { return *(bfloat*)GetConst(40, true, true); }
        private static float _2077() { return *(bfloat*)GetConst(41, true, true); }
        private static float _2078() { return *(bfloat*)GetConst(42, true, true); }
        private static float _2079() { return *(bfloat*)GetConst(43, true, true); }
        private static float _2080() { return *(bfloat*)GetConst(44, true, true); }
        private static float _2081() { return *(bfloat*)GetConst(45, true, true); }
        #endregion

        #endregion

        #region Regular

        #region Basic
        private static int _23030() { return *(bint*)GetConst(0, false, false); }
        private static int _23031() { return *(bint*)GetConst(7, false, false); }
        private static int _23032() { return *(bint*)GetConst(10, false, false); }
        private static int _23033() { return *(bint*)GetConst(11, false, false); }
        private static int _23034() { return *(bint*)GetConst(17, false, false); }
        private static int _23035() { return *(bint*)GetConst(27, false, false); }
        private static int _23036() { return *(bint*)GetConst(34, false, false); }
        private static int _23037() { return *(bint*)GetConst(39, false, false); }
        private static int _23038() { return *(bint*)GetConst(41, false, false); }
        private static int _23039() { return *(bint*)GetConst(60, false, false); }
        private static int _23040() { return *(bint*)GetConst(62, false, false); }
        private static int _23041() { return *(bint*)GetConst(68, false, false); }
        private static int _23042() { return *(bint*)GetConst(74, false, false); }
        private static int _23043() { return *(bint*)GetConst(80, false, false); }
        private static int _23044() { return *(bint*)GetConst(81, false, false); }
        private static int _23045() { return *(bint*)GetConst(84, false, false); }
        private static int _23046() { return *(bint*)GetConst(85, false, false); }
        private static int _23047() { return *(bint*)GetConst(87, false, false); }
        private static int _23048() { return *(bint*)GetConst(93, false, false); }
        private static int _23049() { return *(bint*)GetConst(95, false, false); }
        private static int _23050() { return *(bint*)GetConst(96, false, false); }
        private static int _23051() { return *(bint*)GetConst(99, false, false); }
        private static int _23052() { return *(bint*)GetConst(100, false, false); }
        private static int _23053() { return *(bint*)GetConst(103, false, false); }
        private static int _23054() { return *(bint*)GetConst(104, false, false); }
        private static int _23055() { return *(bint*)GetConst(105, false, false); }
        private static int _23056() { return *(bint*)GetConst(108, false, false); }
        private static int _23057() { return *(bint*)GetConst(109, false, false); }
        private static int _23058() { return *(bint*)GetConst(118, false, false); }
        private static int _23059() { return *(bint*)GetConst(121, false, false); }
        private static int _23060() { return *(bint*)GetConst(123, false, false); }
        private static int _23061() { return *(bint*)GetConst(124, false, false); }
        private static int _23062() { return *(bint*)GetConst(125, false, false); }
        private static int _23063() { return *(bint*)GetConst(127, false, false); }
        private static int _23064() { return *(bint*)GetConst(130, false, false); }
        private static int _23065() { return *(bint*)GetConst(131, false, false); }
        private static int _23066() { return *(bint*)GetConst(133, false, false); }
        private static int _23067() { return *(bint*)GetConst(134, false, false); }
        private static int _23068() { return *(bint*)GetConst(135, false, false); }
        private static int _23069() { return *(bint*)GetConst(136, false, false); }
        private static int _23070() { return *(bint*)GetConst(137, false, false); }
        private static int _23071() { return *(bint*)GetConst(141, false, false); }
        private static int _23072() { return *(bint*)GetConst(145, false, false); }
        private static int _23073() { return *(bint*)GetConst(147, false, false); }
        private static int _23074() { return *(bint*)GetConst(148, false, false); }
        private static int _23075() { return *(bint*)GetConst(150, false, false); }
        private static int _23076() { return *(bint*)GetConst(154, false, false); }
        private static int _23077() { return *(bint*)GetConst(157, false, false); }
        private static int _23078() { return *(bint*)GetConst(160, false, false); }
        private static int _23079() { return *(bint*)GetConst(164, false, false); }
        private static int _23080() { return *(bint*)GetConst(167, false, false); }
        private static int _23081() { return *(bint*)GetConst(172, false, false); }
        private static int _23082() { return *(bint*)GetConst(182, false, false); }
        private static int _23083() { return *(bint*)GetConst(183, false, false); }
        private static int _23084() { return *(bint*)GetConst(191, false, false); }
        private static int _23085() { return *(bint*)GetConst(192, false, false); }
        private static int _23086() { return *(bint*)GetConst(194, false, false); }
        private static int _23087() { return *(bint*)GetConst(200, false, false); }
        private static int _23088() { return *(bint*)GetConst(201, false, false); }
        private static int _23089() { return *(bint*)GetConst(204, false, false); }
        private static int _23090() { return *(bint*)GetConst(211, false, false); }
        private static int _23091() { return *(bint*)GetConst(215, false, false); }
        private static int _23092() { return *(bint*)GetConst(216, false, false); }
        private static int _23093() { return *(bint*)GetConst(217, false, false); }
        private static int _23094() { return *(bint*)GetConst(224, false, false); }
        private static int _23095() { return *(bint*)GetConst(225, false, false); }
        private static int _23096() { return *(bint*)GetConst(230, false, false); }
        private static int _23097() { return *(bint*)GetConst(233, false, false); }
        private static int _23098() { return *(bint*)GetConst(238, false, false); }
        private static int _23099() { return *(bint*)GetConst(248, false, false); }
        private static int _23100() { return *(bint*)GetConst(249, false, false); }
        private static int _23101() { return *(bint*)GetConst(250, false, false); }
        private static int _23102() { return *(bint*)GetConst(251, false, false); }
        private static int _23103() { return *(bint*)GetConst(255, false, false); }
        private static int _23104() { return *(bint*)GetConst(256, false, false); }
        private static int _23105() { return *(bint*)GetConst(257, false, false); }
        private static int _23106() { return *(bint*)GetConst(261, false, false); }
        private static int _23107() { return *(bint*)GetConst(262, false, false); }
        private static int _23108() { return *(bint*)GetConst(263, false, false); }
        private static int _23109() { return *(bint*)GetConst(264, false, false); }
        private static int _23110() { return *(bint*)GetConst(265, false, false); }
        private static int _23111() { return *(bint*)GetConst(266, false, false); }
        private static int _23112() { return *(bint*)GetConst(267, false, false); }
        private static int _23113() { return *(bint*)GetConst(270, false, false); }
        private static int _23114() { return *(bint*)GetConst(278, false, false); }
        private static int _23115() { return *(bint*)GetConst(279, false, false); }
        private static int _23116() { return *(bint*)GetConst(280, false, false); }
        private static int _23117() { return *(bint*)GetConst(281, false, false); }
        private static int _23118() { return *(bint*)GetConst(303, false, false); }
        private static int _23119() { return *(bint*)GetConst(306, false, false); }
        private static int _23120() { return *(bint*)GetConst(316, false, false); }
        private static int _23121() { return *(bint*)GetConst(331, false, false); }
        private static int _23122() { return *(bint*)GetConst(332, false, false); }
        private static int _23123() { return *(bint*)GetConst(336, false, false); }
        private static int _23124() { return *(bint*)GetConst(340, false, false); }
        private static int _23125() { return *(bint*)GetConst(343, false, false); }
        private static int _23126() { return *(bint*)GetConst(344, false, false); }
        private static int _23127() { return *(bint*)GetConst(345, false, false); }
        private static int _23128() { return *(bint*)GetConst(348, false, false); }
        private static int _23129() { return *(bint*)GetConst(349, false, false); }
        private static int _23130() { return *(bint*)GetConst(351, false, false); }
        private static int _23131() { return *(bint*)GetConst(353, false, false); }
        private static int _23132() { return *(bint*)GetConst(354, false, false); }
        private static int _23133() { return *(bint*)GetConst(355, false, false); }
        private static int _23134() { return *(bint*)GetConst(356, false, false); }
        private static int _23135() { return *(bint*)GetConst(360, false, false); }
        private static int _23136() { return *(bint*)GetConst(361, false, false); }
        private static int _23137() { return *(bint*)GetConst(363, false, false); }
        private static int _23138() { return *(bint*)GetConst(364, false, false); }
        private static int _23139() { return *(bint*)GetConst(366, false, false); }
        private static int _23140() { return *(bint*)GetConst(369, false, false); }
        private static int _23141() { return *(bint*)GetConst(378, false, false); }
        private static int _23142() { return *(bint*)GetConst(394, false, false); }
        private static int _23143() { return *(bint*)GetConst(408, false, false); }
        private static int _23144() { return *(bint*)GetConst(437, false, false); }
        private static int _23145() { return *(bint*)GetConst(438, false, false); }
        private static int _23146() { return *(bint*)GetConst(442, false, false); }
        private static int _23147() { return *(bint*)GetConst(443, false, false); }
        private static int _23148() { return *(bint*)GetConst(444, false, false); }
        private static int _23149() { return *(bint*)GetConst(445, false, false); }
        private static int _23150() { return *(bint*)GetConst(446, false, false); }
        private static int _23151() { return *(bint*)GetConst(447, false, false); }
        private static int _23152() { return *(bint*)GetConst(448, false, false); }
        private static int _23153() { return *(bint*)GetConst(449, false, false); }
        private static int _23154() { return *(bint*)GetConst(450, false, false); }
        private static int _23155() { return *(bint*)GetConst(453, false, false); }
        private static int _23156() { return *(bint*)GetConst(455, false, false); }
        private static int _23157() { return *(bint*)GetConst(459, false, false); }
        private static int _23158() { return *(bint*)GetConst(460, false, false); }
        private static int _23159() { return *(bint*)GetConst(461, false, false); }
        private static int _23160() { return *(bint*)GetConst(462, false, false); }
        private static int _23161() { return *(bint*)GetConst(463, false, false); }
        private static int _23162() { return *(bint*)GetConst(464, false, false); }
        private static int _23163() { return *(bint*)GetConst(465, false, false); }
        private static int _23164() { return *(bint*)GetConst(466, false, false); }
        private static int _23165() { return *(bint*)GetConst(473, false, false); }
        private static int _23166() { return *(bint*)GetConst(474, false, false); }
        private static int _23167() { return *(bint*)GetConst(475, false, false); }
        private static int _23168() { return *(bint*)GetConst(477, false, false); }
        private static int _23169() { return *(bint*)GetConst(478, false, false); }
        private static int _23170() { return *(bint*)GetConst(481, false, false); }
        private static int _23171() { return *(bint*)GetConst(482, false, false); }
        private static int _23172() { return *(bint*)GetConst(483, false, false); }
        private static int _23173() { return *(bint*)GetConst(484, false, false); }
        private static int _23174() { return *(bint*)GetConst(493, false, false); }
        private static int _23175() { return *(bint*)GetConst(495, false, false); }
        private static int _23176() { return *(bint*)GetConst(500, false, false); }
        private static int _23177() { return *(bint*)GetConst(501, false, false); }
        private static int _23178() { return *(bint*)GetConst(502, false, false); }
        private static int _23179() { return *(bint*)GetConst(503, false, false); }
        private static int _23180() { return *(bint*)GetConst(504, false, false); }
        private static int _23181() { return *(bint*)GetConst(505, false, false); }
        private static int _23182() { return *(bint*)GetConst(506, false, false); }
        private static int _23183() { return *(bint*)GetConst(507, false, false); }
        private static int _23184() { return *(bint*)GetConst(508, false, false); }
        private static int _23185() { return *(bint*)GetConst(509, false, false); }
        private static int _23186() { return *(bint*)GetConst(510, false, false); }
        private static int _23187() { return *(bint*)GetConst(511, false, false); }
        private static int _23188() { return *(bint*)GetConst(512, false, false); }
        private static int _23189() { return *(bint*)GetConst(520, false, false); }
        private static int _23190() { return *(bint*)GetConst(522, false, false); }
        private static int _23191() { return *(bint*)GetConst(529, false, false); }
        private static int _23192() { return *(bint*)GetConst(530, false, false); }
        private static int _23193() { return *(bint*)GetConst(531, false, false); }
        private static int _23194() { return *(bint*)GetConst(537, false, false); }
        private static int _23195() { return *(bint*)GetConst(538, false, false); }
        private static int _23196() { return *(bint*)GetConst(539, false, false); }
        private static int _23197() { return *(bint*)GetConst(540, false, false); }
        private static int _23198() { return *(bint*)GetConst(548, false, false); }
        private static int _23199() { return *(bint*)GetConst(549, false, false); }
        #endregion

        #region Float
        private static float _3118() { return *(bfloat*)GetConst(1, false, false); }
        private static float _3119() { return *(bfloat*)GetConst(2, false, false); }
        private static float _3120() { return *(bfloat*)GetConst(3, false, false); }
        private static float _3121() { return *(bfloat*)GetConst(4, false, false); }
        private static float _3122() { return *(bfloat*)GetConst(5, false, false); }
        private static float _3123() { return *(bfloat*)GetConst(6, false, false); }
        private static float _3124() { return *(bfloat*)GetConst(8, false, false); }
        private static float _3125() { return *(bfloat*)GetConst(9, false, false); }
        private static float _3126() { return *(bfloat*)GetConst(12, false, false); }
        private static float _3127() { return *(bfloat*)GetConst(13, false, false); }
        private static float _3128() { return *(bfloat*)GetConst(14, false, false); }
        private static float _3129() { return *(bfloat*)GetConst(15, false, false); }
        private static float _3130() { return *(bfloat*)GetConst(16, false, false); }
        private static float _3131() { return *(bfloat*)GetConst(18, false, false); }
        private static float _3132() { return *(bfloat*)GetConst(19, false, false); }
        private static float _3133() { return *(bfloat*)GetConst(20, false, false); }
        private static float _3134() { return *(bfloat*)GetConst(21, false, false); }
        private static float _3135() { return *(bfloat*)GetConst(22, false, false); }
        private static float _3136() { return *(bfloat*)GetConst(23, false, false); }
        private static float _3137() { return *(bfloat*)GetConst(24, false, false); }
        private static float _3138() { return *(bfloat*)GetConst(25, false, false); }
        private static float _3139() { return *(bfloat*)GetConst(26, false, false); }
        private static float _3140() { return *(bfloat*)GetConst(28, false, false); }
        private static float _3141() { return *(bfloat*)GetConst(29, false, false); }
        private static float _3142() { return *(bfloat*)GetConst(30, false, false); }
        private static float _3143() { return *(bfloat*)GetConst(31, false, false); }
        private static float _3144() { return *(bfloat*)GetConst(32, false, false); }
        private static float _3145() { return *(bfloat*)GetConst(33, false, false); }
        private static float _3146() { return *(bfloat*)GetConst(35, false, false); }
        private static float _3147() { return *(bfloat*)GetConst(36, false, false); }
        private static float _3148() { return *(bfloat*)GetConst(37, false, false); }
        private static float _3149() { return *(bfloat*)GetConst(38, false, false); }
        private static float _3150() { return *(bfloat*)GetConst(40, false, false); }
        private static float _3151() { return *(bfloat*)GetConst(42, false, false); }
        private static float _3152() { return *(bfloat*)GetConst(43, false, false); }
        private static float _3153() { return *(bfloat*)GetConst(44, false, false); }
        private static float _3154() { return *(bfloat*)GetConst(45, false, false); }
        private static float _3155() { return *(bfloat*)GetConst(46, false, false); }
        private static float _3156() { return *(bfloat*)GetConst(47, false, false); }
        private static float _3157() { return *(bfloat*)GetConst(48, false, false); }
        private static float _3158() { return *(bfloat*)GetConst(49, false, false); }
        private static float _3159() { return *(bfloat*)GetConst(50, false, false); }
        private static float _3160() { return *(bfloat*)GetConst(51, false, false); }
        private static float _3161() { return *(bfloat*)GetConst(52, false, false); }
        private static float _3162() { return *(bfloat*)GetConst(53, false, false); }
        private static float _3163() { return *(bfloat*)GetConst(54, false, false); }
        private static float _3164() { return *(bfloat*)GetConst(55, false, false); }
        private static float _3165() { return *(bfloat*)GetConst(56, false, false); }
        private static float _3166() { return *(bfloat*)GetConst(57, false, false); }
        private static float _3167() { return *(bfloat*)GetConst(58, false, false); }
        private static float _3168() { return *(bfloat*)GetConst(59, false, false); }
        private static float _3169() { return *(bfloat*)GetConst(61, false, false); }
        private static float _3170() { return *(bfloat*)GetConst(63, false, false); }
        private static float _3171() { return *(bfloat*)GetConst(64, false, false); }
        private static float _3172() { return *(bfloat*)GetConst(65, false, false); }
        private static float _3173() { return *(bfloat*)GetConst(66, false, false); }
        private static float _3174() { return *(bfloat*)GetConst(67, false, false); }
        private static float _3175() { return *(bfloat*)GetConst(69, false, false); }
        private static float _3176() { return *(bfloat*)GetConst(70, false, false); }
        private static float _3177() { return *(bfloat*)GetConst(71, false, false); }
        private static float _3178() { return *(bfloat*)GetConst(72, false, false); }
        private static float _3179() { return *(bfloat*)GetConst(73, false, false); }
        private static float _3180() { return *(bfloat*)GetConst(75, false, false); }
        private static float _3181() { return *(bfloat*)GetConst(76, false, false); }
        private static float _3182() { return *(bfloat*)GetConst(77, false, false); }
        private static float _3183() { return *(bfloat*)GetConst(78, false, false); }
        private static float _3184() { return *(bfloat*)GetConst(79, false, false); }
        private static float _3185() { return *(bfloat*)GetConst(82, false, false); }
        private static float _3186() { return *(bfloat*)GetConst(83, false, false); }
        private static float _3187() { return *(bfloat*)GetConst(86, false, false); }
        private static float _3188() { return *(bfloat*)GetConst(88, false, false); }
        private static float _3189() { return *(bfloat*)GetConst(89, false, false); }
        private static float _3190() { return *(bfloat*)GetConst(90, false, false); }
        private static float _3191() { return *(bfloat*)GetConst(91, false, false); }
        private static float _3192() { return *(bfloat*)GetConst(92, false, false); }
        private static float _3193() { return *(bfloat*)GetConst(94, false, false); }
        private static float _3194() { return *(bfloat*)GetConst(97, false, false); }
        private static float _3195() { return *(bfloat*)GetConst(98, false, false); }
        private static float _3196() { return *(bfloat*)GetConst(101, false, false); }
        private static float _3197() { return *(bfloat*)GetConst(102, false, false); }
        private static float _3198() { return *(bfloat*)GetConst(106, false, false); }
        private static float _3199() { return *(bfloat*)GetConst(107, false, false); }
        private static float _3200() { return *(bfloat*)GetConst(110, false, false); }
        private static float _3201() { return *(bfloat*)GetConst(111, false, false); }
        private static float _3202() { return *(bfloat*)GetConst(112, false, false); }
        private static float _3203() { return *(bfloat*)GetConst(113, false, false); }
        private static float _3204() { return *(bfloat*)GetConst(114, false, false); }
        private static float _3205() { return *(bfloat*)GetConst(115, false, false); }
        private static float _3206() { return *(bfloat*)GetConst(116, false, false); }
        private static float _3207() { return *(bfloat*)GetConst(117, false, false); }
        private static float _3208() { return *(bfloat*)GetConst(119, false, false); }
        private static float _3209() { return *(bfloat*)GetConst(120, false, false); }
        private static float _3210() { return *(bfloat*)GetConst(122, false, false); }
        private static float _3211() { return *(bfloat*)GetConst(126, false, false); }
        private static float _3212() { return *(bfloat*)GetConst(128, false, false); }
        private static float _3213() { return *(bfloat*)GetConst(129, false, false); }
        private static float _3214() { return *(bfloat*)GetConst(132, false, false); }
        private static float _3215() { return *(bfloat*)GetConst(138, false, false); }
        private static float _3216() { return *(bfloat*)GetConst(139, false, false); }
        private static float _3217() { return *(bfloat*)GetConst(140, false, false); }
        private static float _3218() { return *(bfloat*)GetConst(142, false, false); }
        private static float _3219() { return *(bfloat*)GetConst(143, false, false); }
        private static float _3220() { return *(bfloat*)GetConst(144, false, false); }
        private static float _3221() { return *(bfloat*)GetConst(146, false, false); }
        private static float _3222() { return *(bfloat*)GetConst(149, false, false); }
        private static float _3223() { return *(bfloat*)GetConst(151, false, false); }
        private static float _3224() { return *(bfloat*)GetConst(152, false, false); }
        private static float _3225() { return *(bfloat*)GetConst(153, false, false); }
        private static float _3226() { return *(bfloat*)GetConst(155, false, false); }
        private static float _3227() { return *(bfloat*)GetConst(156, false, false); }
        private static float _3228() { return *(bfloat*)GetConst(158, false, false); }
        private static float _3229() { return *(bfloat*)GetConst(159, false, false); }
        private static float _3230() { return *(bfloat*)GetConst(161, false, false); }
        private static float _3231() { return *(bfloat*)GetConst(162, false, false); }
        private static float _3232() { return *(bfloat*)GetConst(163, false, false); }
        private static float _3233() { return *(bfloat*)GetConst(165, false, false); }
        private static float _3234() { return *(bfloat*)GetConst(166, false, false); }
        private static float _3235() { return *(bfloat*)GetConst(168, false, false); }
        private static float _3236() { return *(bfloat*)GetConst(169, false, false); }
        private static float _3237() { return *(bfloat*)GetConst(170, false, false); }
        private static float _3238() { return *(bfloat*)GetConst(171, false, false); }
        private static float _3239() { return *(bfloat*)GetConst(173, false, false); }
        private static float _3240() { return *(bfloat*)GetConst(174, false, false); }
        private static float _3241() { return *(bfloat*)GetConst(175, false, false); }
        private static float _3242() { return *(bfloat*)GetConst(176, false, false); }
        private static float _3243() { return *(bfloat*)GetConst(177, false, false); }
        private static float _3244() { return *(bfloat*)GetConst(178, false, false); }
        private static float _3245() { return *(bfloat*)GetConst(179, false, false); }
        private static float _3246() { return *(bfloat*)GetConst(180, false, false); }
        private static float _3247() { return *(bfloat*)GetConst(181, false, false); }
        private static float _3248() { return *(bfloat*)GetConst(184, false, false); }
        private static float _3249() { return *(bfloat*)GetConst(185, false, false); }
        private static float _3250() { return *(bfloat*)GetConst(186, false, false); }
        private static float _3251() { return *(bfloat*)GetConst(187, false, false); }
        private static float _3252() { return *(bfloat*)GetConst(188, false, false); }
        private static float _3253() { return *(bfloat*)GetConst(189, false, false); }
        private static float _3254() { return *(bfloat*)GetConst(190, false, false); }
        private static float _3255() { return *(bfloat*)GetConst(193, false, false); }
        private static float _3256() { return *(bfloat*)GetConst(195, false, false); }
        private static float _3257() { return *(bfloat*)GetConst(196, false, false); }
        private static float _3258() { return *(bfloat*)GetConst(197, false, false); }
        private static float _3259() { return *(bfloat*)GetConst(198, false, false); }
        private static float _3260() { return *(bfloat*)GetConst(199, false, false); }
        private static float _3261() { return *(bfloat*)GetConst(202, false, false); }
        private static float _3262() { return *(bfloat*)GetConst(203, false, false); }
        private static float _3263() { return *(bfloat*)GetConst(205, false, false); }
        private static float _3264() { return *(bfloat*)GetConst(206, false, false); }
        private static float _3265() { return *(bfloat*)GetConst(207, false, false); }
        private static float _3266() { return *(bfloat*)GetConst(208, false, false); }
        private static float _3267() { return *(bfloat*)GetConst(209, false, false); }
        private static float _3268() { return *(bfloat*)GetConst(210, false, false); }
        private static float _3269() { return *(bfloat*)GetConst(212, false, false); }
        private static float _3270() { return *(bfloat*)GetConst(213, false, false); }
        private static float _3271() { return *(bfloat*)GetConst(214, false, false); }
        private static float _3272() { return *(bfloat*)GetConst(218, false, false); }
        private static float _3273() { return *(bfloat*)GetConst(219, false, false); }
        private static float _3274() { return *(bfloat*)GetConst(220, false, false); }
        private static float _3275() { return *(bfloat*)GetConst(221, false, false); }
        private static float _3276() { return *(bfloat*)GetConst(222, false, false); }
        private static float _3277() { return *(bfloat*)GetConst(223, false, false); }
        private static float _3278() { return *(bfloat*)GetConst(226, false, false); }
        private static float _3279() { return *(bfloat*)GetConst(227, false, false); }
        private static float _3280() { return *(bfloat*)GetConst(228, false, false); }
        private static float _3281() { return *(bfloat*)GetConst(229, false, false); }
        private static float _3282() { return *(bfloat*)GetConst(231, false, false); }
        private static float _3283() { return *(bfloat*)GetConst(232, false, false); }
        private static float _3284() { return *(bfloat*)GetConst(234, false, false); }
        private static float _3285() { return *(bfloat*)GetConst(235, false, false); }
        private static float _3286() { return *(bfloat*)GetConst(236, false, false); }
        private static float _3287() { return *(bfloat*)GetConst(237, false, false); }
        private static float _3288() { return *(bfloat*)GetConst(239, false, false); }
        private static float _3289() { return *(bfloat*)GetConst(240, false, false); }
        private static float _3290() { return *(bfloat*)GetConst(241, false, false); }
        private static float _3291() { return *(bfloat*)GetConst(242, false, false); }
        private static float _3292() { return *(bfloat*)GetConst(243, false, false); }
        private static float _3293() { return *(bfloat*)GetConst(244, false, false); }
        private static float _3294() { return *(bfloat*)GetConst(245, false, false); }
        private static float _3295() { return *(bfloat*)GetConst(246, false, false); }
        private static float _3296() { return *(bfloat*)GetConst(247, false, false); }
        private static float _3297() { return *(bfloat*)GetConst(252, false, false); }
        private static float _3298() { return *(bfloat*)GetConst(253, false, false); }
        private static float _3299() { return *(bfloat*)GetConst(254, false, false); }
        private static float _3300() { return *(bfloat*)GetConst(258, false, false); }
        private static float _3301() { return *(bfloat*)GetConst(259, false, false); }
        private static float _3302() { return *(bfloat*)GetConst(260, false, false); }
        private static float _3303() { return *(bfloat*)GetConst(268, false, false); }
        private static float _3304() { return *(bfloat*)GetConst(269, false, false); }
        private static float _3305() { return *(bfloat*)GetConst(271, false, false); }
        private static float _3306() { return *(bfloat*)GetConst(272, false, false); }
        private static float _3307() { return *(bfloat*)GetConst(273, false, false); }
        private static float _3308() { return *(bfloat*)GetConst(274, false, false); }
        private static float _3309() { return *(bfloat*)GetConst(275, false, false); }
        private static float _3310() { return *(bfloat*)GetConst(276, false, false); }
        private static float _3311() { return *(bfloat*)GetConst(277, false, false); }
        private static float _3312() { return *(bfloat*)GetConst(282, false, false); }
        private static float _3313() { return *(bfloat*)GetConst(283, false, false); }
        private static float _3314() { return *(bfloat*)GetConst(284, false, false); }
        private static float _3315() { return *(bfloat*)GetConst(285, false, false); }
        private static float _3316() { return *(bfloat*)GetConst(286, false, false); }
        private static float _3317() { return *(bfloat*)GetConst(287, false, false); }
        private static float _3318() { return *(bfloat*)GetConst(288, false, false); }
        private static float _3319() { return *(bfloat*)GetConst(289, false, false); }
        private static float _3320() { return *(bfloat*)GetConst(290, false, false); }
        private static float _3321() { return *(bfloat*)GetConst(291, false, false); }
        private static float _3322() { return *(bfloat*)GetConst(292, false, false); }
        private static float _3323() { return *(bfloat*)GetConst(293, false, false); }
        private static float _3324() { return *(bfloat*)GetConst(294, false, false); }
        private static float _3325() { return *(bfloat*)GetConst(295, false, false); }
        private static float _3326() { return *(bfloat*)GetConst(296, false, false); }
        private static float _3327() { return *(bfloat*)GetConst(297, false, false); }
        private static float _3328() { return *(bfloat*)GetConst(298, false, false); }
        private static float _3329() { return *(bfloat*)GetConst(299, false, false); }
        private static float _3330() { return *(bfloat*)GetConst(300, false, false); }
        private static float _3331() { return *(bfloat*)GetConst(301, false, false); }
        private static float _3332() { return *(bfloat*)GetConst(302, false, false); }
        private static float _3333() { return *(bfloat*)GetConst(304, false, false); }
        private static float _3334() { return *(bfloat*)GetConst(305, false, false); }
        private static float _3335() { return *(bfloat*)GetConst(307, false, false); }
        private static float _3336() { return *(bfloat*)GetConst(308, false, false); }
        private static float _3337() { return *(bfloat*)GetConst(309, false, false); }
        private static float _3338() { return *(bfloat*)GetConst(310, false, false); }
        private static float _3339() { return *(bfloat*)GetConst(311, false, false); }
        private static float _3340() { return *(bfloat*)GetConst(312, false, false); }
        private static float _3341() { return *(bfloat*)GetConst(313, false, false); }
        private static float _3342() { return *(bfloat*)GetConst(314, false, false); }
        private static float _3343() { return *(bfloat*)GetConst(315, false, false); }
        private static float _3344() { return *(bfloat*)GetConst(317, false, false); }
        private static float _3345() { return *(bfloat*)GetConst(318, false, false); }
        private static float _3346() { return *(bfloat*)GetConst(319, false, false); }
        private static float _3347() { return *(bfloat*)GetConst(320, false, false); }
        private static float _3348() { return *(bfloat*)GetConst(321, false, false); }
        private static float _3349() { return *(bfloat*)GetConst(322, false, false); }
        private static float _3350() { return *(bfloat*)GetConst(323, false, false); }
        private static float _3351() { return *(bfloat*)GetConst(324, false, false); }
        private static float _3352() { return *(bfloat*)GetConst(325, false, false); }
        private static float _3353() { return *(bfloat*)GetConst(326, false, false); }
        private static float _3354() { return *(bfloat*)GetConst(327, false, false); }
        private static float _3355() { return *(bfloat*)GetConst(328, false, false); }
        private static float _3356() { return *(bfloat*)GetConst(329, false, false); }
        private static float _3357() { return *(bfloat*)GetConst(330, false, false); }
        private static float _3358() { return *(bfloat*)GetConst(333, false, false); }
        private static float _3359() { return *(bfloat*)GetConst(334, false, false); }
        private static float _3360() { return *(bfloat*)GetConst(335, false, false); }
        private static float _3361() { return *(bfloat*)GetConst(337, false, false); }
        private static float _3362() { return *(bfloat*)GetConst(338, false, false); }
        private static float _3363() { return *(bfloat*)GetConst(339, false, false); }
        private static float _3364() { return *(bfloat*)GetConst(341, false, false); }
        private static float _3365() { return *(bfloat*)GetConst(342, false, false); }
        private static float _3366() { return *(bfloat*)GetConst(346, false, false); }
        private static float _3367() { return *(bfloat*)GetConst(347, false, false); }
        private static float _3368() { return *(bfloat*)GetConst(350, false, false); }
        private static float _3369() { return *(bfloat*)GetConst(352, false, false); }
        private static float _3370() { return *(bfloat*)GetConst(357, false, false); }
        private static float _3371() { return *(bfloat*)GetConst(358, false, false); }
        private static float _3372() { return *(bfloat*)GetConst(359, false, false); }
        private static float _3373() { return *(bfloat*)GetConst(362, false, false); }
        private static float _3374() { return *(bfloat*)GetConst(365, false, false); }
        private static float _3375() { return *(bfloat*)GetConst(367, false, false); }
        private static float _3376() { return *(bfloat*)GetConst(368, false, false); }
        private static float _3377() { return *(bfloat*)GetConst(370, false, false); }
        private static float _3378() { return *(bfloat*)GetConst(371, false, false); }
        private static float _3379() { return *(bfloat*)GetConst(372, false, false); }
        private static float _3380() { return *(bfloat*)GetConst(373, false, false); }
        private static float _3381() { return *(bfloat*)GetConst(374, false, false); }
        private static float _3382() { return *(bfloat*)GetConst(375, false, false); }
        private static float _3383() { return *(bfloat*)GetConst(376, false, false); }
        private static float _3384() { return *(bfloat*)GetConst(377, false, false); }
        private static float _3385() { return *(bfloat*)GetConst(379, false, false); }
        private static float _3386() { return *(bfloat*)GetConst(380, false, false); }
        private static float _3387() { return *(bfloat*)GetConst(381, false, false); }
        private static float _3388() { return *(bfloat*)GetConst(382, false, false); }
        private static float _3389() { return *(bfloat*)GetConst(383, false, false); }
        private static float _3390() { return *(bfloat*)GetConst(384, false, false); }
        private static float _3391() { return *(bfloat*)GetConst(385, false, false); }
        private static float _3392() { return *(bfloat*)GetConst(386, false, false); }
        private static float _3393() { return *(bfloat*)GetConst(387, false, false); }
        private static float _3394() { return *(bfloat*)GetConst(388, false, false); }
        private static float _3395() { return *(bfloat*)GetConst(389, false, false); }
        private static float _3396() { return *(bfloat*)GetConst(390, false, false); }
        private static float _3397() { return *(bfloat*)GetConst(391, false, false); }
        private static float _3398() { return *(bfloat*)GetConst(392, false, false); }
        private static float _3399() { return *(bfloat*)GetConst(393, false, false); }
        private static float _3400() { return *(bfloat*)GetConst(395, false, false); }
        private static float _3401() { return *(bfloat*)GetConst(396, false, false); }
        private static float _3402() { return *(bfloat*)GetConst(397, false, false); }
        private static float _3403() { return *(bfloat*)GetConst(398, false, false); }
        private static float _3404() { return *(bfloat*)GetConst(399, false, false); }
        private static float _3405() { return *(bfloat*)GetConst(400, false, false); }
        private static float _3406() { return *(bfloat*)GetConst(401, false, false); }
        private static float _3407() { return *(bfloat*)GetConst(402, false, false); }
        private static float _3408() { return *(bfloat*)GetConst(403, false, false); }
        private static float _3409() { return *(bfloat*)GetConst(404, false, false); }
        private static float _3410() { return *(bfloat*)GetConst(405, false, false); }
        private static float _3411() { return *(bfloat*)GetConst(406, false, false); }
        private static float _3412() { return *(bfloat*)GetConst(407, false, false); }
        private static float _3413() { return *(bfloat*)GetConst(409, false, false); }
        private static float _3414() { return *(bfloat*)GetConst(410, false, false); }
        private static float _3415() { return *(bfloat*)GetConst(411, false, false); }
        private static float _3416() { return *(bfloat*)GetConst(412, false, false); }
        private static float _3417() { return *(bfloat*)GetConst(413, false, false); }
        private static float _3418() { return *(bfloat*)GetConst(414, false, false); }
        private static float _3419() { return *(bfloat*)GetConst(415, false, false); }
        private static float _3420() { return *(bfloat*)GetConst(416, false, false); }
        private static float _3421() { return *(bfloat*)GetConst(417, false, false); }
        private static float _3422() { return *(bfloat*)GetConst(418, false, false); }
        private static float _3423() { return *(bfloat*)GetConst(419, false, false); }
        private static float _3424() { return *(bfloat*)GetConst(420, false, false); }
        private static float _3425() { return *(bfloat*)GetConst(421, false, false); }
        private static float _3426() { return *(bfloat*)GetConst(422, false, false); }
        private static float _3427() { return *(bfloat*)GetConst(423, false, false); }
        private static float _3428() { return *(bfloat*)GetConst(424, false, false); }
        private static float _3429() { return *(bfloat*)GetConst(425, false, false); }
        private static float _3430() { return *(bfloat*)GetConst(426, false, false); }
        private static float _3431() { return *(bfloat*)GetConst(427, false, false); }
        private static float _3432() { return *(bfloat*)GetConst(428, false, false); }
        private static float _3433() { return *(bfloat*)GetConst(429, false, false); }
        private static float _3434() { return *(bfloat*)GetConst(430, false, false); }
        private static float _3435() { return *(bfloat*)GetConst(431, false, false); }
        private static float _3436() { return *(bfloat*)GetConst(432, false, false); }
        private static float _3437() { return *(bfloat*)GetConst(433, false, false); }
        private static float _3438() { return *(bfloat*)GetConst(434, false, false); }
        private static float _3439() { return *(bfloat*)GetConst(435, false, false); }
        private static float _3440() { return *(bfloat*)GetConst(436, false, false); }
        private static float _3441() { return *(bfloat*)GetConst(439, false, false); }
        private static float _3442() { return *(bfloat*)GetConst(440, false, false); }
        private static float _3443() { return *(bfloat*)GetConst(441, false, false); }
        private static float _3444() { return *(bfloat*)GetConst(451, false, false); }
        private static float _3445() { return *(bfloat*)GetConst(452, false, false); }
        private static float _3446() { return *(bfloat*)GetConst(454, false, false); }
        private static float _3447() { return *(bfloat*)GetConst(456, false, false); }
        private static float _3448() { return *(bfloat*)GetConst(457, false, false); }
        private static float _3449() { return *(bfloat*)GetConst(458, false, false); }
        private static float _3450() { return *(bfloat*)GetConst(467, false, false); }
        private static float _3451() { return *(bfloat*)GetConst(468, false, false); }
        private static float _3452() { return *(bfloat*)GetConst(469, false, false); }
        private static float _3453() { return *(bfloat*)GetConst(470, false, false); }
        private static float _3454() { return *(bfloat*)GetConst(471, false, false); }
        private static float _3455() { return *(bfloat*)GetConst(472, false, false); }
        private static float _3456() { return *(bfloat*)GetConst(476, false, false); }
        private static float _3457() { return *(bfloat*)GetConst(479, false, false); }
        private static float _3458() { return *(bfloat*)GetConst(480, false, false); }
        private static float _3459() { return *(bfloat*)GetConst(485, false, false); }
        private static float _3460() { return *(bfloat*)GetConst(486, false, false); }
        private static float _3461() { return *(bfloat*)GetConst(487, false, false); }
        private static float _3462() { return *(bfloat*)GetConst(488, false, false); }
        private static float _3463() { return *(bfloat*)GetConst(489, false, false); }
        private static float _3464() { return *(bfloat*)GetConst(490, false, false); }
        private static float _3465() { return *(bfloat*)GetConst(491, false, false); }
        private static float _3466() { return *(bfloat*)GetConst(492, false, false); }
        private static float _3467() { return *(bfloat*)GetConst(494, false, false); }
        private static float _3468() { return *(bfloat*)GetConst(496, false, false); }
        private static float _3469() { return *(bfloat*)GetConst(497, false, false); }
        private static float _3470() { return *(bfloat*)GetConst(498, false, false); }
        private static float _3471() { return *(bfloat*)GetConst(499, false, false); }
        private static float _3472() { return *(bfloat*)GetConst(513, false, false); }
        private static float _3473() { return *(bfloat*)GetConst(514, false, false); }
        private static float _3474() { return *(bfloat*)GetConst(515, false, false); }
        private static float _3475() { return *(bfloat*)GetConst(516, false, false); }
        private static float _3476() { return *(bfloat*)GetConst(517, false, false); }
        private static float _3477() { return *(bfloat*)GetConst(518, false, false); }
        private static float _3478() { return *(bfloat*)GetConst(519, false, false); }
        private static float _3479() { return *(bfloat*)GetConst(521, false, false); }
        private static float _3480() { return *(bfloat*)GetConst(523, false, false); }
        private static float _3481() { return *(bfloat*)GetConst(524, false, false); }
        private static float _3482() { return *(bfloat*)GetConst(525, false, false); }
        private static float _3483() { return *(bfloat*)GetConst(526, false, false); }
        private static float _3484() { return *(bfloat*)GetConst(527, false, false); }
        private static float _3485() { return *(bfloat*)GetConst(528, false, false); }
        private static float _3486() { return *(bfloat*)GetConst(532, false, false); }
        private static float _3487() { return *(bfloat*)GetConst(533, false, false); }
        private static float _3488() { return *(bfloat*)GetConst(534, false, false); }
        private static float _3489() { return *(bfloat*)GetConst(535, false, false); }
        private static float _3490() { return *(bfloat*)GetConst(536, false, false); }
        private static float _3491() { return *(bfloat*)GetConst(541, false, false); }
        private static float _3492() { return *(bfloat*)GetConst(542, false, false); }
        private static float _3493() { return *(bfloat*)GetConst(543, false, false); }
        private static float _3494() { return *(bfloat*)GetConst(544, false, false); }
        private static float _3495() { return *(bfloat*)GetConst(545, false, false); }
        private static float _3496() { return *(bfloat*)GetConst(546, false, false); }
        private static float _3497() { return *(bfloat*)GetConst(547, false, false); }
        private static float _3498() { return *(bfloat*)GetConst(550, false, false); }
        #endregion

        #endregion

        #endregion

        #endregion
    }
}