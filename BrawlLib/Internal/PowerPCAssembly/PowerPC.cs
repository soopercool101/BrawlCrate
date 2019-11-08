using System;

namespace BrawlLib.Internal.PowerPCAssembly
{
    public static class PowerPC
    {
        public static unsafe PPCOpCode[] Disassemble(VoidPtr ptr, int count, bool bigEndian)
        {
            if (count < 0)
            {
                throw new ArgumentException();
            }

            PPCOpCode[] result = new PPCOpCode[count];
            if (bigEndian)
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = Disassemble(((buint*) ptr)[i]);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = Disassemble(((uint*) ptr)[i]);
                }
            }

            return result;
        }

        public static PPCOpCode Disassemble(uint value)
        {
            switch ((PPCMnemonic) (value & 0xFC000000))
            {
                case PPCMnemonic.vaddubm: return new PPCVaddubm(value);
                case PPCMnemonic.mulli:   return new PPCMulli(value);
                case PPCMnemonic.subfic:  return new PPCSubfic(value);
                case PPCMnemonic.cmpli:   return new PPCCmpli(value);
                case PPCMnemonic.cmpi:    return new PPCCmpi(value);
                case PPCMnemonic.addic:   return new PPCAddic(value);
                case PPCMnemonic.addic_D: return new PPCAddic(value);
                case PPCMnemonic.addi:    return new PPCAddi(value);
                case PPCMnemonic.addis:   return new PPCAddi(value);
                case PPCMnemonic.bc:      return new PPCBc(value);
                case PPCMnemonic.b:       return new PPCbx(value);
                case PPCMnemonic.grp4C:
                    switch ((PPCMnemonic) (value & 0xFC0007FE))
                    {
                        case PPCMnemonic.bclr:  return new PPCblr(value);
                        case PPCMnemonic.bcctr: return new PPCbctr(value);
                    }

                    break;
                case PPCMnemonic.rlwimi:  return new PPCRlwimi(value);
                case PPCMnemonic.rlwinm:  return new PPCRlwinm(value);
                case PPCMnemonic.ori:     return new PPCOri(value);
                case PPCMnemonic.oris:    return new PPCOri(value);
                case PPCMnemonic.xori:    return new PPCOri(value);
                case PPCMnemonic.xoris:   return new PPCOri(value);
                case PPCMnemonic.andi_D:  return new PPCAndi(value);
                case PPCMnemonic.andis_D: return new PPCAndi(value);
                case PPCMnemonic.rldicl:  return new PPCRldicl(value);
                case PPCMnemonic.grp7C:
                    switch ((PPCMnemonic) (value & 0xFC0007FE))
                    {
                        case PPCMnemonic.cmp:    return new PPCCmp(value);
                        case PPCMnemonic.subfc:  return new PPCSubc(value);
                        case PPCMnemonic.slw:    return new PPCSlw(value);
                        case PPCMnemonic.andi_D: return new PPCAnd(value);
                        case PPCMnemonic.cmplw:  return new PPCCmpl(value);
                        case PPCMnemonic.sub:    return new PPCSub(value);
                        case PPCMnemonic.cntlzw: return new PPCCntlzw(value);
                        case PPCMnemonic.cntlzd: return new PPCCntlzd(value);
                        case PPCMnemonic.add:    return new PPCAdd(value);
                        case PPCMnemonic.mfspr:  return new PPCMfspr(value);
                        case PPCMnemonic.xor:    return new PPCXor(value);
                        case PPCMnemonic.or:     return new PPCOr(value);
                        case PPCMnemonic.mtspr:  return new PPCMtspr(value);
                        case PPCMnemonic.extsh:  return new PPCExtsh(value);
                        case PPCMnemonic.extsb:  return new PPCExtsb(value);
                        case PPCMnemonic.mullw:  return new PPCMullw(value);
                    }

                    break;
                case PPCMnemonic.lwz:   return new PPCLwz(value);
                case PPCMnemonic.lwzu:  return new PPCLwz(value);
                case PPCMnemonic.lbz:   return new PPCLwz(value);
                case PPCMnemonic.lbzu:  return new PPCLwz(value);
                case PPCMnemonic.stw:   return new PPCStw(value);
                case PPCMnemonic.stwu:  return new PPCStw(value);
                case PPCMnemonic.stb:   return new PPCStw(value);
                case PPCMnemonic.stbu:  return new PPCStw(value);
                case PPCMnemonic.lhz:   return new PPCLwz(value);
                case PPCMnemonic.lhzu:  return new PPCLwz(value);
                case PPCMnemonic.lha:   return new PPCLwz(value);
                case PPCMnemonic.lhau:  return new PPCLwz(value);
                case PPCMnemonic.sth:   return new PPCStw(value);
                case PPCMnemonic.sthu:  return new PPCStw(value);
                case PPCMnemonic.lmw:   return new PPCLwz(value);
                case PPCMnemonic.stmw:  return new PPCStw(value);
                case PPCMnemonic.lfs:   return new PPCLfs(value);
                case PPCMnemonic.lfsu:  return new PPCLfs(value);
                case PPCMnemonic.lfd:   return new PPCLfs(value);
                case PPCMnemonic.lfdu:  return new PPCLfs(value);
                case PPCMnemonic.stfs:  return new PPCStfs(value);
                case PPCMnemonic.stfsu: return new PPCStfs(value);
                case PPCMnemonic.stfd:  return new PPCStfs(value);
                case PPCMnemonic.stfdu: return new PPCStfs(value);
                case PPCMnemonic.ld:    return new PPCLwz(value);
                case PPCMnemonic.std:   return new PPCStw(value);
            }

            return new PPCWord(value);
        }
    }
}