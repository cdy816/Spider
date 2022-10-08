using Cdy.Spider.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.MelsecDriver
{
    /// <summary>
    /// 
    /// </summary>
    public class McAddressData : DeviceAddressDataBase
    {
        /// <summary>
        /// 实例化一个默认的对象
        /// </summary>
        public McAddressData()
        {
            McDataType = MelsecMcDataType.D;
        }

        /// <summary>
        /// 三菱的数据类型及地址信息
        /// </summary>
        public MelsecMcDataType McDataType { get; set; }

        /// <summary>
        /// 从指定的地址信息解析成真正的设备地址信息，默认是三菱的地址
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="length">数据长度</param>
        public override void Parse(string address, ushort length)
        {
            McAddressData operateResult = ParseMelsecFrom(address, length);
            if (operateResult != null)
            {
                AddressStart = operateResult.AddressStart;
                Length = operateResult.Length;
                McDataType = operateResult.McDataType;
            }
        }

        public override string ToString()
        {
            return McDataType.AsciiCode.Replace("*", "") + Convert.ToString(AddressStart, McDataType.FromBase);
        }

        /// <summary>
        /// 从实际三菱的地址里面解析出我们需要的地址类型<br />
        /// Resolve the type of address we need from the actual Mitsubishi address
        /// </summary>
        /// <param name="address">三菱的地址数据信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static McAddressData ParseMelsecFrom(string address, ushort length)
        {
            McAddressData mcAddressData = new McAddressData();
            mcAddressData.Length = length;
            try
            {
                char c = address[0];
                char c2 = c;
                switch (c2)
                {
                    case 'B':
                        goto IL_39E;
                    case 'C':
                        goto IL_7CA;
                    case 'D':
                        goto IL_1CE;
                    case 'E':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'U':
                        goto IL_8C0;
                    case 'F':
                        goto IL_340;
                    case 'L':
                        goto IL_311;
                    case 'M':
                        break;
                    case 'R':
                        goto IL_3CD;
                    case 'S':
                        goto IL_3FC;
                    case 'T':
                        goto IL_6CE;
                    case 'V':
                        goto IL_36F;
                    case 'W':
                        goto IL_2E2;
                    case 'X':
                        goto IL_12A;
                    case 'Y':
                        goto IL_17B;
                    case 'Z':
                        goto IL_64E;
                    default:
                        switch (c2)
                        {
                            case 'b':
                                goto IL_39E;
                            case 'c':
                                goto IL_7CA;
                            case 'd':
                                goto IL_1CE;
                            case 'e':
                            case 'g':
                            case 'h':
                            case 'i':
                            case 'j':
                            case 'k':
                            case 'n':
                            case 'o':
                            case 'p':
                            case 'q':
                            case 'u':
                                goto IL_8C0;
                            case 'f':
                                goto IL_340;
                            case 'l':
                                goto IL_311;
                            case 'm':
                                break;
                            case 'r':
                                goto IL_3CD;
                            case 's':
                                goto IL_3FC;
                            case 't':
                                goto IL_6CE;
                            case 'v':
                                goto IL_36F;
                            case 'w':
                                goto IL_2E2;
                            case 'x':
                                goto IL_12A;
                            case 'y':
                                goto IL_17B;
                            case 'z':
                                goto IL_64E;
                            default:
                                goto IL_8C0;
                        }
                        break;
                }
                mcAddressData.McDataType = MelsecMcDataType.M;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.M.FromBase);
                goto IL_8D0;
            IL_12A:
                mcAddressData.McDataType = MelsecMcDataType.X;
                address = address.Substring(1);
                bool flag = address.StartsWith("0");
                if (flag)
                {
                    mcAddressData.AddressStart = Convert.ToInt32(address, 8);
                }
                else
                {
                    mcAddressData.AddressStart = Convert.ToInt32(address, MelsecMcDataType.X.FromBase);
                }
                goto IL_8D0;
            IL_17B:
                mcAddressData.McDataType = MelsecMcDataType.Y;
                address = address.Substring(1);
                bool flag2 = address.StartsWith("0");
                if (flag2)
                {
                    mcAddressData.AddressStart = Convert.ToInt32(address, 8);
                }
                else
                {
                    mcAddressData.AddressStart = Convert.ToInt32(address, MelsecMcDataType.Y.FromBase);
                }
                goto IL_8D0;
            IL_1CE:
                bool flag3 = address[1] == 'X' || address[1] == 'x';
                if (flag3)
                {
                    mcAddressData.McDataType = MelsecMcDataType.DX;
                    address = address.Substring(2);
                    bool flag4 = address.StartsWith("0");
                    if (flag4)
                    {
                        mcAddressData.AddressStart = Convert.ToInt32(address, 8);
                    }
                    else
                    {
                        mcAddressData.AddressStart = Convert.ToInt32(address, MelsecMcDataType.DX.FromBase);
                    }
                    goto IL_8D0;
                }
                bool flag5 = address[1] == 'Y' || address[1] == 's';
                if (flag5)
                {
                    mcAddressData.McDataType = MelsecMcDataType.DY;
                    address = address.Substring(2);
                    bool flag6 = address.StartsWith("0");
                    if (flag6)
                    {
                        mcAddressData.AddressStart = Convert.ToInt32(address, 8);
                    }
                    else
                    {
                        mcAddressData.AddressStart = Convert.ToInt32(address, MelsecMcDataType.DY.FromBase);
                    }
                    goto IL_8D0;
                }
                mcAddressData.McDataType = MelsecMcDataType.D;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.D.FromBase);
                goto IL_8D0;
            IL_2E2:
                mcAddressData.McDataType = MelsecMcDataType.W;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.W.FromBase);
                goto IL_8D0;
            IL_311:
                mcAddressData.McDataType = MelsecMcDataType.L;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.L.FromBase);
                goto IL_8D0;
            IL_340:
                mcAddressData.McDataType = MelsecMcDataType.F;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.F.FromBase);
                goto IL_8D0;
            IL_36F:
                mcAddressData.McDataType = MelsecMcDataType.V;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.V.FromBase);
                goto IL_8D0;
            IL_39E:
                mcAddressData.McDataType = MelsecMcDataType.B;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.B.FromBase);
                goto IL_8D0;
            IL_3CD:
                mcAddressData.McDataType = MelsecMcDataType.R;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.R.FromBase);
                goto IL_8D0;
            IL_3FC:
                bool flag7 = address[1] == 'N' || address[1] == 'n';
                if (flag7)
                {
                    mcAddressData.McDataType = MelsecMcDataType.SN;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.SN.FromBase);
                    goto IL_8D0;
                }
                bool flag8 = address[1] == 'S' || address[1] == 's';
                if (flag8)
                {
                    mcAddressData.McDataType = MelsecMcDataType.SS;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.SS.FromBase);
                    goto IL_8D0;
                }
                bool flag9 = address[1] == 'C' || address[1] == 'c';
                if (flag9)
                {
                    mcAddressData.McDataType = MelsecMcDataType.SC;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.SC.FromBase);
                    goto IL_8D0;
                }
                bool flag10 = address[1] == 'M' || address[1] == 'm';
                if (flag10)
                {
                    mcAddressData.McDataType = MelsecMcDataType.SM;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.SM.FromBase);
                    goto IL_8D0;
                }
                bool flag11 = address[1] == 'D' || address[1] == 'd';
                if (flag11)
                {
                    mcAddressData.McDataType = MelsecMcDataType.SD;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.SD.FromBase);
                    goto IL_8D0;
                }
                bool flag12 = address[1] == 'B' || address[1] == 'b';
                if (flag12)
                {
                    mcAddressData.McDataType = MelsecMcDataType.SB;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.SB.FromBase);
                    goto IL_8D0;
                }
                bool flag13 = address[1] == 'W' || address[1] == 'w';
                if (flag13)
                {
                    mcAddressData.McDataType = MelsecMcDataType.SW;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.SW.FromBase);
                    goto IL_8D0;
                }
                mcAddressData.McDataType = MelsecMcDataType.S;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.S.FromBase);
                goto IL_8D0;
            IL_64E:
                bool flag14 = address.StartsWith("ZR") || address.StartsWith("zr");
                if (flag14)
                {
                    mcAddressData.McDataType = MelsecMcDataType.ZR;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.ZR.FromBase);
                    goto IL_8D0;
                }
                mcAddressData.McDataType = MelsecMcDataType.Z;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Z.FromBase);
                goto IL_8D0;
            IL_6CE:
                bool flag15 = address[1] == 'N' || address[1] == 'n';
                if (flag15)
                {
                    mcAddressData.McDataType = MelsecMcDataType.TN;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.TN.FromBase);
                    goto IL_8D0;
                }
                bool flag16 = address[1] == 'S' || address[1] == 's';
                if (flag16)
                {
                    mcAddressData.McDataType = MelsecMcDataType.TS;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.TS.FromBase);
                    goto IL_8D0;
                }
                bool flag17 = address[1] == 'C' || address[1] == 'c';
                if (flag17)
                {
                    mcAddressData.McDataType = MelsecMcDataType.TC;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.TC.FromBase);
                    goto IL_8D0;
                }
                throw new Exception("NotSupportedDataType");
            IL_7CA:
                bool flag18 = address[1] == 'N' || address[1] == 'n';
                if (flag18)
                {
                    mcAddressData.McDataType = MelsecMcDataType.CN;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.CN.FromBase);
                    goto IL_8D0;
                }
                bool flag19 = address[1] == 'S' || address[1] == 's';
                if (flag19)
                {
                    mcAddressData.McDataType = MelsecMcDataType.CS;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.CS.FromBase);
                    goto IL_8D0;
                }
                bool flag20 = address[1] == 'C' || address[1] == 'c';
                if (flag20)
                {
                    mcAddressData.McDataType = MelsecMcDataType.CC;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.CC.FromBase);
                    goto IL_8D0;
                }
                throw new Exception("NotSupportedDataType");
            IL_8C0:
                throw new Exception("NotSupportedDataType");
            IL_8D0:;
            }
            catch (Exception ex)
            {
                return null;
            }
            return mcAddressData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static McAddressData ParseMelsecRFrom(string address, ushort length)
        {
            Tuple<MelsecMcDataType, int> operateResult = AnalysisAddress(address);
            if (operateResult == null)
            {
                return null;
            }
            else
            {
                return new McAddressData
                {
                    McDataType = operateResult.Item1,
                    AddressStart = operateResult.Item2,
                    Length = length
                };
            }
        }

        /// <summary>
        /// 分析三菱R系列的地址，并返回解析后的数据对象
        /// </summary>
        /// <param name="address">字符串地址</param>
        /// <returns>是否解析成功</returns>
        public static Tuple<MelsecMcDataType, int> AnalysisAddress(string address)
        {
            Tuple<MelsecMcDataType, int> result;
            try
            {
                if (address.StartsWith("LSTS"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LSTS, Convert.ToInt32(address.Substring(4), MelsecMcDataType.R_LSTS.FromBase));
                }
                else if (address.StartsWith("LSTC"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LSTC, Convert.ToInt32(address.Substring(4), MelsecMcDataType.R_LSTC.FromBase));
                }
                else if (address.StartsWith("LSTN"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LSTN, Convert.ToInt32(address.Substring(4), MelsecMcDataType.R_LSTN.FromBase));
                }
                else if (address.StartsWith("STS"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_STS, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_STS.FromBase));
                }
                else if (address.StartsWith("STC"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_STC, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_STC.FromBase));
                }
                else if (address.StartsWith("STN"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_STN, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_STN.FromBase));
                }
                else if (address.StartsWith("LTS"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LTS, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_LTS.FromBase));
                }
                else if (address.StartsWith("LTC"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LTC, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_LTC.FromBase));
                }
                else if (address.StartsWith("LTN"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LTN, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_LTN.FromBase));
                }
                else if (address.StartsWith("LCS"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LCS, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_LCS.FromBase));
                }
                else if (address.StartsWith("LCC"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LCC, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_LCC.FromBase));
                }
                else if (address.StartsWith("LCN"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_LCN, Convert.ToInt32(address.Substring(3), MelsecMcDataType.R_LCN.FromBase));
                }
                else if (address.StartsWith("TS"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_TS, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_TS.FromBase));
                }
                else if (address.StartsWith("TC"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_TC, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_TC.FromBase));
                }
                else if (address.StartsWith("TN"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_TN, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_TN.FromBase));
                }
                else if (address.StartsWith("CS"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_CS, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_CS.FromBase));
                }
                else if (address.StartsWith("CC"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_CC, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_CC.FromBase));
                }
                else if (address.StartsWith("CN"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_CN, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_CN.FromBase));
                }
                else if (address.StartsWith("SM"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_SM, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_SM.FromBase));
                }
                else if (address.StartsWith("SB"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_SB, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_SB.FromBase));
                }
                else if (address.StartsWith("DX"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_DX, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_DX.FromBase));
                }
                else if (address.StartsWith("DY"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_DY, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_DY.FromBase));
                }
                else if (address.StartsWith("SD"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_SD, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_SD.FromBase));
                }
                else if (address.StartsWith("SW"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_SW, Convert.ToInt32(address.Substring(2), MelsecMcDataType.R_SW.FromBase));
                }
                else if (address.StartsWith("X"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_X, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_X.FromBase));
                }
                else if (address.StartsWith("Y"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_Y, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_Y.FromBase));
                }
                else if (address.StartsWith("M"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_M, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_M.FromBase));
                }
                else if (address.StartsWith("L"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_L, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_L.FromBase));
                }
                else if (address.StartsWith("F"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_F, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_F.FromBase));
                }
                else if (address.StartsWith("V"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_V, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_V.FromBase));
                }
                else if (address.StartsWith("S"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_S, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_S.FromBase));
                }
                else if (address.StartsWith("B"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_B, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_B.FromBase));
                }
                else if (address.StartsWith("D"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_D, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_D.FromBase));
                }
                else if (address.StartsWith("W"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_W, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_W.FromBase));
                }
                else if (address.StartsWith("R"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_R, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_R.FromBase));
                }
                else if (address.StartsWith("Z"))
                {
                    result = new Tuple<MelsecMcDataType, int>(MelsecMcDataType.R_Z, Convert.ToInt32(address.Substring(1), MelsecMcDataType.R_Z.FromBase));
                }
                else
                {
                    result = null;
                }

            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 从实际基恩士的地址里面解析出我们需要的地址信息<br />
        /// Resolve the address information we need from the actual Keyence address
        /// </summary>
        /// <param name="address">基恩士的地址数据信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static McAddressData ParseKeyenceFrom(string address, ushort length)
        {
            McAddressData mcAddressData = new McAddressData();
            mcAddressData.Length = length;
            try
            {
                char c = address[0];
                char c2 = c;
                if (c2 <= 'Z')
                {
                    switch (c2)
                    {
                        case 'B':
                            goto IL_16E;
                        case 'C':
                            goto IL_461;
                        case 'D':
                            goto IL_278;
                        default:
                            switch (c2)
                            {
                                case 'L':
                                    goto IL_19D;
                                case 'M':
                                    break;
                                case 'N':
                                case 'O':
                                case 'P':
                                case 'Q':
                                case 'U':
                                case 'V':
                                    goto IL_557;
                                case 'R':
                                    goto IL_2A7;
                                case 'S':
                                    goto IL_1CC;
                                case 'T':
                                    goto IL_365;
                                case 'W':
                                    goto IL_336;
                                case 'X':
                                    goto IL_110;
                                case 'Y':
                                    goto IL_13F;
                                case 'Z':
                                    goto IL_2D6;
                                default:
                                    goto IL_557;
                            }
                            break;
                    }
                }
                else
                {
                    switch (c2)
                    {
                        case 'b':
                            goto IL_16E;
                        case 'c':
                            goto IL_461;
                        case 'd':
                            goto IL_278;
                        default:
                            switch (c2)
                            {
                                case 'l':
                                    goto IL_19D;
                                case 'm':
                                    break;
                                case 'n':
                                case 'o':
                                case 'p':
                                case 'q':
                                case 'u':
                                case 'v':
                                    goto IL_557;
                                case 'r':
                                    goto IL_2A7;
                                case 's':
                                    goto IL_1CC;
                                case 't':
                                    goto IL_365;
                                case 'w':
                                    goto IL_336;
                                case 'x':
                                    goto IL_110;
                                case 'y':
                                    goto IL_13F;
                                case 'z':
                                    goto IL_2D6;
                                default:
                                    goto IL_557;
                            }
                            break;
                    }
                }
                mcAddressData.McDataType = MelsecMcDataType.Keyence_M;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Keyence_M.FromBase);
                goto IL_567;
            IL_110:
                mcAddressData.McDataType = MelsecMcDataType.Keyence_X;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Keyence_X.FromBase);
                goto IL_567;
            IL_13F:
                mcAddressData.McDataType = MelsecMcDataType.Keyence_Y;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Keyence_Y.FromBase);
                goto IL_567;
            IL_16E:
                mcAddressData.McDataType = MelsecMcDataType.Keyence_B;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Keyence_B.FromBase);
                goto IL_567;
            IL_19D:
                mcAddressData.McDataType = MelsecMcDataType.Keyence_L;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Keyence_L.FromBase);
                goto IL_567;
            IL_1CC:
                bool flag = address[1] == 'M' || address[1] == 'm';
                if (flag)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_SM;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_SM.FromBase);
                    goto IL_567;
                }
                bool flag2 = address[1] == 'D' || address[1] == 'd';
                if (flag2)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_SD;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_SD.FromBase);
                    goto IL_567;
                }
                throw new Exception("NotSupportedDataType");
            IL_278:
                mcAddressData.McDataType = MelsecMcDataType.Keyence_D;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Keyence_D.FromBase);
                goto IL_567;
            IL_2A7:
                mcAddressData.McDataType = MelsecMcDataType.Keyence_R;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Keyence_R.FromBase);
                goto IL_567;
            IL_2D6:
                bool flag3 = address[1] == 'R' || address[1] == 'r';
                if (flag3)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_ZR;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_ZR.FromBase);
                    goto IL_567;
                }
                throw new Exception("NotSupportedDataType");
            IL_336:
                mcAddressData.McDataType = MelsecMcDataType.Keyence_W;
                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1), MelsecMcDataType.Keyence_W.FromBase);
                goto IL_567;
            IL_365:
                bool flag4 = address[1] == 'N' || address[1] == 'n';
                if (flag4)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_TN;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_TN.FromBase);
                    goto IL_567;
                }
                bool flag5 = address[1] == 'S' || address[1] == 's';
                if (flag5)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_TS;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_TS.FromBase);
                    goto IL_567;
                }
                bool flag6 = address[1] == 'C' || address[1] == 'c';
                if (flag6)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_TC;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_TC.FromBase);
                    goto IL_567;
                }
                throw new Exception("NotSupportedDataType");
            IL_461:
                bool flag7 = address[1] == 'N' || address[1] == 'n';
                if (flag7)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_CN;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_CN.FromBase);
                    goto IL_567;
                }
                bool flag8 = address[1] == 'S' || address[1] == 's';
                if (flag8)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_CS;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_CS.FromBase);
                    goto IL_567;
                }
                bool flag9 = address[1] == 'C' || address[1] == 'c';
                if (flag9)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Keyence_CC;
                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2), MelsecMcDataType.Keyence_CC.FromBase);
                    goto IL_567;
                }
                throw new Exception("NotSupportedDataType");
            IL_557:
                throw new Exception("NotSupportedDataType");
            IL_567:;
            }
            catch (Exception ex)
            {
                return null;
            }
            return mcAddressData;
        }

        /// <summary>
        /// 从实际松下的地址里面解析出
        /// </summary>
        /// <param name="address">松下的地址数据信息</param>
        /// <param name="length">读取的数据长度</param>
        /// <returns>是否成功的结果对象</returns>
        public static McAddressData ParsePanasonicFrom(string address, ushort length)
        {
            McAddressData mcAddressData = new McAddressData();
            mcAddressData.Length = length;
            try
            {
                //char c = address[0];
                char c2 = address[0];
                if (c2 <= 'Y')
                {
                    if (c2 <= 'D')
                    {
                        if (c2 == 'C')
                        {
                            if (address[1] == 'N' || address[1] == 'n')
                            {
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_CN;
                                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                return mcAddressData;
                            }
                            else if (address[1] == 'S' || address[1] == 's')
                            {
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_CS;
                                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                return mcAddressData;
                            }
                            else
                            {
                                return null;
                            }
                        }
                        if (c2 != 'D')
                        {
                            return null;
                        }
                        int num2 = Convert.ToInt32(address.Substring(1));
                        if (num2 < 90000)
                        {
                            mcAddressData.McDataType = MelsecMcDataType.Panasonic_DT;
                            mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1));
                        }
                        else
                        {
                            mcAddressData.McDataType = MelsecMcDataType.Panasonic_SD;
                            mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1)) - 90000;
                        }
                        return mcAddressData;
                    }
                    else
                    {
                        if (c2 == 'L')
                        {
                            if (address[1] == 'D' || address[1] == 'd')
                            {
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_LD;
                                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                return mcAddressData;
                            }
                            else
                            {
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_L;
                                mcAddressData.AddressStart = CalculateComplexAddress(address.Substring(1), 16);
                                return mcAddressData;
                            }
                        }
                        switch (c2)
                        {
                            case 'R':
                                break;
                            case 'S':
                                if (address[1] == 'D' || address[1] == 'd')
                                {
                                    mcAddressData.McDataType = MelsecMcDataType.Panasonic_SD;
                                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                    return mcAddressData;
                                }
                                else
                                {
                                    return null;
                                }
                            case 'T':
                                if (address[1] == 'N' || address[1] == 'n')
                                {
                                    mcAddressData.McDataType = MelsecMcDataType.Panasonic_TN;
                                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                    return mcAddressData;
                                }
                                else if (address[1] == 'S' || address[1] == 's')
                                {
                                    mcAddressData.McDataType = MelsecMcDataType.Panasonic_TS;
                                    mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                    return mcAddressData;
                                }
                                else
                                {
                                    return null;
                                }
                            case 'U':
                            case 'V':
                            case 'W':
                                return null;
                            case 'X':
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_X;
                                mcAddressData.AddressStart = CalculateComplexAddress(address.Substring(1), 16);
                                return mcAddressData;
                            case 'Y':
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_Y;
                                mcAddressData.AddressStart = CalculateComplexAddress(address.Substring(1), 16);
                                return mcAddressData;
                            default:
                                return null;
                        }
                    }
                }
                else if (c2 <= 'd')
                {
                    if (c2 == 'c')
                    {
                        if (address[1] == 'N' || address[1] == 'n')
                        {
                            mcAddressData.McDataType = MelsecMcDataType.Panasonic_CN;
                            mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                            return mcAddressData;
                        }
                        else if (address[1] == 'S' || address[1] == 's')
                        {
                            mcAddressData.McDataType = MelsecMcDataType.Panasonic_CS;
                            mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                            return mcAddressData;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    if (c2 != 'd')
                    {
                        return null;
                    }
                    int num2 = Convert.ToInt32(address.Substring(1));
                    if (num2 < 90000)
                    {
                        mcAddressData.McDataType = MelsecMcDataType.Panasonic_DT;
                        mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1));
                    }
                    else
                    {
                        mcAddressData.McDataType = MelsecMcDataType.Panasonic_SD;
                        mcAddressData.AddressStart = Convert.ToInt32(address.Substring(1)) - 90000;
                    }
                    return mcAddressData;
                }
                else
                {
                    if (c2 == 'l')
                    {
                        if (address[1] == 'D' || address[1] == 'd')
                        {
                            mcAddressData.McDataType = MelsecMcDataType.Panasonic_LD;
                            mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                            return mcAddressData;
                        }
                        else
                        {
                            mcAddressData.McDataType = MelsecMcDataType.Panasonic_L;
                            mcAddressData.AddressStart = CalculateComplexAddress(address.Substring(1), 16);
                            return mcAddressData;
                        }
                    }
                    switch (c2)
                    {
                        case 'r':
                            break;
                        case 's':
                            if (address[1] == 'D' || address[1] == 'd')
                            {
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_SD;
                                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                return mcAddressData;
                            }
                            else
                            {
                                return null;
                            }
                        case 't':
                            if (address[1] == 'N' || address[1] == 'n')
                            {
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_TN;
                                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                return mcAddressData;
                            }
                            else if (address[1] == 'S' || address[1] == 's')
                            {
                                mcAddressData.McDataType = MelsecMcDataType.Panasonic_TS;
                                mcAddressData.AddressStart = Convert.ToInt32(address.Substring(2));
                                return mcAddressData;
                            }
                            else
                            {
                                return null;
                            }
                        case 'u':
                        case 'v':
                        case 'w':
                            return null;
                        case 'x':
                            mcAddressData.McDataType = MelsecMcDataType.Panasonic_X;
                            mcAddressData.AddressStart = CalculateComplexAddress(address.Substring(1), 16);
                            return mcAddressData;
                        case 'y':
                            mcAddressData.McDataType = MelsecMcDataType.Panasonic_Y;
                            mcAddressData.AddressStart = CalculateComplexAddress(address.Substring(1), 16);
                            return mcAddressData;
                        default:
                            return null;
                    }
                }
                int num = CalculateComplexAddress(address.Substring(1), 16);
                if (num < 14400)
                {
                    mcAddressData.McDataType = MelsecMcDataType.Panasonic_R;
                    mcAddressData.AddressStart = num;
                }
                else
                {
                    mcAddressData.McDataType = MelsecMcDataType.Panasonic_SM;
                    mcAddressData.AddressStart = num - 14400;
                }
                return mcAddressData;
           
            }
            catch (Exception)
            {
                
            }
            return null;
        }

        /// <summary>
        /// 位地址转换方法，101等同于10.1等同于10*16+1=161<br />
        /// Bit address conversion method, 101 is equivalent to 10.1 is equivalent to 10 * 16 + 1 = 161
        /// </summary>
        /// <param name="address">地址信息</param>
        /// <param name="fromBase">倍率信息</param>
        /// <returns>实际的位地址信息</returns>
        public static int CalculateComplexAddress(string address, int fromBase = 16)
        {
            bool flag = address.IndexOf(".") < 0;
            int num;
            if (flag)
            {
                bool flag2 = address.Length == 1;
                if (flag2)
                {
                    num = Convert.ToInt32(address, fromBase);
                }
                else
                {
                    num = Convert.ToInt32(address.Substring(0, address.Length - 1)) * fromBase + Convert.ToInt32(address.Substring(address.Length - 1), fromBase);
                }
            }
            else
            {
                num = Convert.ToInt32(address.Substring(0, address.IndexOf("."))) * fromBase;
                string bit = address.Substring(address.IndexOf(".") + 1);
                num += bit.CalculateBitStartIndex();
            }
            return num;
        }
    }

    /// <summary>
    /// 三菱PLC的数据类型，此处包含了几个常用的类型<br />
    /// Data types of Mitsubishi PLC, here contains several commonly used types
    /// </summary>
    public class MelsecMcDataType
    {
        /// <summary>
        /// 实例化一个三菱数据类型对象，如果您清楚类型代号，可以根据值进行扩展<br />
        /// Instantiate a Mitsubishi data type object, if you know the type code, you can expand according to the value
        /// </summary>
        /// <param name="code">数据类型的代号</param>
        /// <param name="type">0或1，默认为0，0代表按字，1代表按位</param>
        /// <param name="asciiCode">ASCII格式的类型信息</param>
        /// <param name="fromBase">指示地址的多少进制的，10或是16</param>
        public MelsecMcDataType(ushort code, byte type, string asciiCode, int fromBase)
        {
            DataCode = code;
            AsciiCode = asciiCode;
            FromBase = fromBase;
            bool flag = type < 2;
            if (flag)
            {
                DataType = type;
            }
        }

        /// <summary>
        /// 类型的代号值
        /// </summary>
        public ushort DataCode { get; private set; } = 0;

        /// <summary>
        /// 数据的类型，0代表按字，1代表按位
        /// </summary>

        public byte DataType { get; private set; } = 0;

        /// <summary>
        /// 当以ASCII格式通讯时的类型描述
        /// </summary>

        public string AsciiCode { get; private set; }

        /// <summary>
        /// 指示地址是10进制，还是16进制的
        /// </summary>

        public int FromBase { get; private set; }

        /// <summary>
        /// X输入继电器
        /// </summary>
        public static readonly MelsecMcDataType X = new MelsecMcDataType(156, 1, "X*", 16);

        /// <summary>
        /// Y输出继电器
        /// </summary>
        public static readonly MelsecMcDataType Y = new MelsecMcDataType(157, 1, "Y*", 16);

        /// <summary>
        /// M内部继电器
        /// </summary>
        public static readonly MelsecMcDataType M = new MelsecMcDataType(144, 1, "M*", 10);

        /// <summary>
        /// SM特殊继电器
        /// </summary>
        public static readonly MelsecMcDataType SM = new MelsecMcDataType(145, 1, "SM", 10);

        /// <summary>
        /// S步进继电器
        /// </summary>
        public static readonly MelsecMcDataType S = new MelsecMcDataType(152, 1, "S*", 10);

        /// <summary>
        /// L锁存继电器
        /// </summary>
        public static readonly MelsecMcDataType L = new MelsecMcDataType(146, 1, "L*", 10);

        /// <summary>
        /// F报警器
        /// </summary>
        public static readonly MelsecMcDataType F = new MelsecMcDataType(147, 1, "F*", 10);

        /// <summary>
        /// V边沿继电器
        /// </summary>
        public static readonly MelsecMcDataType V = new MelsecMcDataType(148, 1, "V*", 10);

        /// <summary>
        /// B链接继电器
        /// </summary>
        public static readonly MelsecMcDataType B = new MelsecMcDataType(160, 1, "B*", 16);

        /// <summary>
        /// SB特殊链接继电器
        /// </summary>
        public static readonly MelsecMcDataType SB = new MelsecMcDataType(161, 1, "SB", 16);

        /// <summary>
        /// DX直接访问输入
        /// </summary>
        public static readonly MelsecMcDataType DX = new MelsecMcDataType(162, 1, "DX", 16);

        /// <summary>
        /// DY直接访问输出
        /// </summary>
        public static readonly MelsecMcDataType DY = new MelsecMcDataType(163, 1, "DY", 16);

        /// <summary>
        /// D数据寄存器
        /// </summary>
        // Token: 0x04000316 RID: 790
        public static readonly MelsecMcDataType D = new MelsecMcDataType(168, 0, "D*", 10);

        /// <summary>
        /// 特殊链接存储器
        /// </summary>
        public static readonly MelsecMcDataType SD = new MelsecMcDataType(169, 0, "SD", 10);

        /// <summary>
        /// W链接寄存器
        /// </summary>
        public static readonly MelsecMcDataType W = new MelsecMcDataType(180, 0, "W*", 16);

        /// <summary>
        /// SW特殊链接寄存器
        /// </summary>
        public static readonly MelsecMcDataType SW = new MelsecMcDataType(181, 0, "SW", 16);

        /// <summary>
        /// R文件寄存器
        /// </summary>
        public static readonly MelsecMcDataType R = new MelsecMcDataType(175, 0, "R*", 10);

        /// <summary>
        /// 变址寄存器
        /// </summary>
        public static readonly MelsecMcDataType Z = new MelsecMcDataType(204, 0, "Z*", 10);

        /// <summary>
        /// 文件寄存器ZR区
        /// </summary>
        public static readonly MelsecMcDataType ZR = new MelsecMcDataType(176, 0, "ZR", 10);

        /// <summary>
        /// 定时器的当前值
        /// </summary>
        public static readonly MelsecMcDataType TN = new MelsecMcDataType(194, 0, "TN", 10);

        /// <summary>
        /// 定时器的触点
        /// </summary>
        public static readonly MelsecMcDataType TS = new MelsecMcDataType(193, 1, "TS", 10);

        /// <summary>
        /// 定时器的线圈
        /// </summary>
        public static readonly MelsecMcDataType TC = new MelsecMcDataType(192, 1, "TC", 10);

        /// <summary>
        /// 累计定时器的触点
        /// </summary>
        public static readonly MelsecMcDataType SS = new MelsecMcDataType(199, 1, "SS", 10);

        /// <summary>
        /// 累计定时器的线圈
        /// </summary>
        public static readonly MelsecMcDataType SC = new MelsecMcDataType(198, 1, "SC", 10);

        /// <summary>
        /// 累计定时器的当前值
        /// </summary>
        public static readonly MelsecMcDataType SN = new MelsecMcDataType(200, 0, "SN", 10);

        /// <summary>
        /// 计数器的当前值
        /// </summary>
        public static readonly MelsecMcDataType CN = new MelsecMcDataType(197, 0, "CN", 10);

        /// <summary>
        /// 计数器的触点
        /// </summary>
        public static readonly MelsecMcDataType CS = new MelsecMcDataType(196, 1, "CS", 10);

        /// <summary>
        /// 计数器的线圈
        /// </summary>
        public static readonly MelsecMcDataType CC = new MelsecMcDataType(195, 1, "CC", 10);

        /// <summary>
        /// X输入继电器
        /// </summary>
        public static readonly MelsecMcDataType R_X = new MelsecMcDataType(156, 1, "X***", 16);

        /// <summary>
        /// Y输入继电器
        /// </summary>
        public static readonly MelsecMcDataType R_Y = new MelsecMcDataType(157, 1, "Y***", 16);

        /// <summary>
        /// M内部继电器
        /// </summary>
        public static readonly MelsecMcDataType R_M = new MelsecMcDataType(144, 1, "M***", 10);

        /// <summary>
        /// 特殊继电器
        /// </summary>
        public static readonly MelsecMcDataType R_SM = new MelsecMcDataType(145, 1, "SM**", 10);

        /// <summary>
        /// 锁存继电器
        /// </summary>
        public static readonly MelsecMcDataType R_L = new MelsecMcDataType(146, 1, "L***", 10);

        /// <summary>
        /// 报警器
        /// </summary>
        public static readonly MelsecMcDataType R_F = new MelsecMcDataType(147, 1, "F***", 10);

        /// <summary>
        /// 变址继电器
        /// </summary>
        public static readonly MelsecMcDataType R_V = new MelsecMcDataType(148, 1, "V***", 10);

        /// <summary>
        /// S步进继电器
        /// </summary>
        public static readonly MelsecMcDataType R_S = new MelsecMcDataType(152, 1, "S***", 10);

        /// <summary>
        /// 链接继电器
        /// </summary>
        public static readonly MelsecMcDataType R_B = new MelsecMcDataType(160, 1, "B***", 16);

        /// <summary>
        /// 特殊链接继电器
        /// </summary>
        public static readonly MelsecMcDataType R_SB = new MelsecMcDataType(161, 1, "SB**", 16);

        /// <summary>
        /// 直接访问输入继电器
        /// </summary>
        public static readonly MelsecMcDataType R_DX = new MelsecMcDataType(162, 1, "DX**", 16);

        /// <summary>
        /// 直接访问输出继电器
        /// </summary>
        public static readonly MelsecMcDataType R_DY = new MelsecMcDataType(163, 1, "DY**", 16);

        /// <summary>
        /// 数据寄存器
        /// </summary>
        public static readonly MelsecMcDataType R_D = new MelsecMcDataType(168, 0, "D***", 10);

        /// <summary>
        /// 特殊数据寄存器
        /// </summary>
        public static readonly MelsecMcDataType R_SD = new MelsecMcDataType(169, 0, "SD**", 10);

        /// <summary>
        /// 链接寄存器
        /// </summary>
        public static readonly MelsecMcDataType R_W = new MelsecMcDataType(180, 0, "W***", 16);

        /// <summary>
        /// 特殊链接寄存器
        /// </summary>
        public static readonly MelsecMcDataType R_SW = new MelsecMcDataType(181, 0, "SW**", 16);

        /// <summary>
        /// 文件寄存器
        /// </summary>
        public static readonly MelsecMcDataType R_R = new MelsecMcDataType(175, 0, "R***", 10);

        /// <summary>
        /// 变址寄存器
        /// </summary>
        public static readonly MelsecMcDataType R_Z = new MelsecMcDataType(204, 0, "Z***", 10);

        /// <summary>
        /// 长累计定时器触点
        /// </summary>
        public static readonly MelsecMcDataType R_LSTS = new MelsecMcDataType(89, 1, "LSTS", 10);

        /// <summary>
        /// 长累计定时器线圈
        /// </summary>
        public static readonly MelsecMcDataType R_LSTC = new MelsecMcDataType(88, 1, "LSTC", 10);

        /// <summary>
        /// 长累计定时器当前值
        /// </summary>
        public static readonly MelsecMcDataType R_LSTN = new MelsecMcDataType(90, 0, "LSTN", 10);

        /// <summary>
        /// 累计定时器触点
        /// </summary>
        public static readonly MelsecMcDataType R_STS = new MelsecMcDataType(199, 1, "STS*", 10);

        /// <summary>
        /// 累计定时器线圈
        /// </summary>
        public static readonly MelsecMcDataType R_STC = new MelsecMcDataType(198, 1, "STC*", 10);

        /// <summary>
        /// 累计定时器当前值
        /// </summary>
        public static readonly MelsecMcDataType R_STN = new MelsecMcDataType(200, 0, "STN*", 10);

        /// <summary>
        /// 长定时器触点
        /// </summary>
        public static readonly MelsecMcDataType R_LTS = new MelsecMcDataType(81, 1, "LTS*", 10);

        /// <summary>
        /// 长定时器线圈
        /// </summary>
        public static readonly MelsecMcDataType R_LTC = new MelsecMcDataType(80, 1, "LTC*", 10);

        /// <summary>
        /// 长定时器当前值
        /// </summary>
        public static readonly MelsecMcDataType R_LTN = new MelsecMcDataType(82, 0, "LTN*", 10);

        /// <summary>
        /// 定时器触点
        /// </summary>
        public static readonly MelsecMcDataType R_TS = new MelsecMcDataType(193, 1, "TS**", 10);

        /// <summary>
        /// 定时器线圈
        /// </summary>
        public static readonly MelsecMcDataType R_TC = new MelsecMcDataType(192, 1, "TC**", 10);

        /// <summary>
        /// 定时器当前值
        /// </summary>
        public static readonly MelsecMcDataType R_TN = new MelsecMcDataType(194, 0, "TN**", 10);

        /// <summary>
        /// 长计数器触点
        /// </summary>
        public static readonly MelsecMcDataType R_LCS = new MelsecMcDataType(85, 1, "LCS*", 10);

        /// <summary>
        /// 长计数器线圈
        /// </summary>
        public static readonly MelsecMcDataType R_LCC = new MelsecMcDataType(84, 1, "LCC*", 10);

        /// <summary>
        /// 长计数器当前值
        /// </summary>
        public static readonly MelsecMcDataType R_LCN = new MelsecMcDataType(86, 0, "LCN*", 10);

        /// <summary>
        /// 计数器触点
        /// </summary>
        public static readonly MelsecMcDataType R_CS = new MelsecMcDataType(196, 1, "CS**", 10);

        /// <summary>
        /// 计数器线圈
        /// </summary>
        public static readonly MelsecMcDataType R_CC = new MelsecMcDataType(195, 1, "CC**", 10);

        /// <summary>
        /// 计数器当前值
        /// </summary>
        public static readonly MelsecMcDataType R_CN = new MelsecMcDataType(197, 0, "CN**", 10);

        /// <summary>
        /// X输入继电器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_X = new MelsecMcDataType(156, 1, "X*", 16);

        /// <summary>
        /// Y输出继电器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_Y = new MelsecMcDataType(157, 1, "Y*", 16);

        /// <summary>
        /// 链接继电器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_B = new MelsecMcDataType(160, 1, "B*", 16);

        /// <summary>
        /// 内部辅助继电器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_M = new MelsecMcDataType(144, 1, "M*", 10);

        /// <summary>
        /// 锁存继电器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_L = new MelsecMcDataType(146, 1, "L*", 10);

        /// <summary>
        /// 控制继电器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_SM = new MelsecMcDataType(145, 1, "SM", 10);

        /// <summary>
        /// 控制存储器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_SD = new MelsecMcDataType(169, 0, "SD", 10);

        /// <summary>
        /// 数据存储器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_D = new MelsecMcDataType(168, 0, "D*", 10);

        /// <summary>
        /// 文件寄存器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_R = new MelsecMcDataType(175, 0, "R*", 10);

        /// <summary>
        /// 文件寄存器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_ZR = new MelsecMcDataType(176, 0, "ZR", 10);

        /// <summary>
        /// 链路寄存器
        /// </summary>
        public static readonly MelsecMcDataType Keyence_W = new MelsecMcDataType(180, 0, "W*", 16);

        /// <summary>
        /// 计时器（当前值）
        /// </summary>
        // Token: 0x04000355 RID: 853
        public static readonly MelsecMcDataType Keyence_TN = new MelsecMcDataType(194, 0, "TN", 10);

        /// <summary>
        /// 计时器（接点）
        /// </summary>
        public static readonly MelsecMcDataType Keyence_TS = new MelsecMcDataType(193, 1, "TS", 10);

        /// <summary>
        /// 计时器（线圈）
        /// </summary>
        public static readonly MelsecMcDataType Keyence_TC = new MelsecMcDataType(192, 1, "TC", 10);

        /// <summary>
        /// 计数器（当前值）
        /// </summary>
        public static readonly MelsecMcDataType Keyence_CN = new MelsecMcDataType(197, 0, "CN", 10);

        /// <summary>
        /// 计数器（接点）
        /// </summary>
        public static readonly MelsecMcDataType Keyence_CS = new MelsecMcDataType(196, 1, "CS", 10);

        /// <summary>
        /// 计数器（线圈）
        /// </summary>
        public static readonly MelsecMcDataType Keyence_CC = new MelsecMcDataType(195, 1, "CC", 10);

        /// <summary>
        /// 输入继电器
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_X = new MelsecMcDataType(156, 1, "X*", 10);

        /// <summary>
        /// 输出继电器
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_Y = new MelsecMcDataType(157, 1, "Y*", 10);

        /// <summary>
        /// 链接继电器
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_L = new MelsecMcDataType(160, 1, "L*", 10);

        /// <summary>
        /// 内部继电器
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_R = new MelsecMcDataType(144, 1, "R*", 10);

        /// <summary>
        /// 数据存储器
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_DT = new MelsecMcDataType(168, 0, "D*", 10);

        /// <summary>
        /// 链接存储器
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_LD = new MelsecMcDataType(180, 0, "W*", 10);

        /// <summary>
        /// 计时器（当前值）
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_TN = new MelsecMcDataType(194, 0, "TN", 10);

        /// <summary>
        /// 计时器（接点）
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_TS = new MelsecMcDataType(193, 1, "TS", 10);

        /// <summary>
        /// 计数器（当前值）
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_CN = new MelsecMcDataType(197, 0, "CN", 10);

        /// <summary>
        /// 计数器（接点）
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_CS = new MelsecMcDataType(196, 1, "CS", 10);

        /// <summary>
        /// 特殊链接继电器
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_SM = new MelsecMcDataType(145, 1, "SM", 10);

        /// <summary>
        /// 特殊链接存储器
        /// </summary>
        public static readonly MelsecMcDataType Panasonic_SD = new MelsecMcDataType(169, 0, "SD", 10);
    }
}
