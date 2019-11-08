using System.Runtime.InteropServices;

namespace BrawlLib.Internal.Audio
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct G726State
    {
        public short sr0, sr1; /* Reconstructed signal with delays 0 and 1 */
        public short a1r, a2r; /* Triggered 2nd order predictor coeffs. */
        public short b1r;      /* Triggered 6nd order predictor coeffs */
        public short b2r;
        public short b3r;
        public short b4r;
        public short b5r;
        public short b6r;
        public short dq5; /* Quantized difference signal with delays 5 to 0 */
        public short dq4;
        public short dq3;
        public short dq2;
        public short dq1;
        public short dq0;
        public short dmsp;     /* Short term average of the F(I) sequence */
        public short dmlp;     /* Long term average of the F(I) sequence */
        public short apr;      /* Triggered unlimited speed control parameter */
        public short yup;      /* Fast quantizer scale factor */
        public short tdr;      /* Triggered tone detector */
        public short pk0, pk1; /* sign of dq+sez with delays 0 and 1 */
        public int ylp;        /* Slow quantizer scale factor */
    }

    public static unsafe class G726
    {
        internal static void G726_encode(short* inp_buf, short* out_buf, int smpno, byte* law, short rate, short r,
                                         G726State* state)
        {
            short s;
            short d, i;
            short y;
            short sigpk;
            short sr, tr;
            short yu;
            short al, fi, dl, ap, dq, ds, se, ax, td, sl, wi;
            short u1, u2, u3, u4, u5, u6;
            short a1, a2, b1, b2, b3, b4, b5, b6;
            short dqln;
            short a1p,
                a2p,
                a1t,
                a2t,
                b1p,
                b2p,
                b3p,
                b4p,
                b5p,
                b6p,
                dq6,
                pk2,
                sr2,
                wa1,
                wa2,
                wb1,
                wb2,
                wb3,
                wb4,
                wb5,
                wb6;
            short dml, dln, app, dql, dms;
            short dqs, tdp;
            short sez;
            short yut;
            int yl;

            int j;

            /* Invert even bits if A law */
            if (*law == '1')
            {
                for (j = 0; j < smpno; j++)
                {
                    inp_buf[j] ^= 85;
                }
            }

            /* Process all desired samples in inp_buf to out_buf; The comments about
             * general blocks are given as in G.726, and refer to: 4.1.1 Input PCM
             * format conversion and difference signal computation    4.1.2 Adaptive
             * quantizer 4.1.3 Inverse adaptive quantizer 4.1.4 Quantizer scale factor
             * adaptation 4.1.5 Adaptation speed control 4.1.6 Adaptive predictor and
             * reconstructed signal calculator 4.1.7 Tone and transition detector 4.1.8
             * (Only in the decoder) */
            for (j = 0; j < smpno; j++, r = 0)
            {
                s = inp_buf[j];

                /* Process `known-state' part of 4.2.6 */
                G726_delayd(&r, &state->sr1, &sr2);
                G726_delayd(&r, &state->sr0, &state->sr1);
                G726_delaya(&r, &state->a2r, &a2);
                G726_delaya(&r, &state->a1r, &a1);
                G726_fmult(&a2, &sr2, &wa2);
                G726_fmult(&a1, &state->sr1, &wa1);

                G726_delayd(&r, &state->dq5, &dq6);
                G726_delayd(&r, &state->dq4, &state->dq5);
                G726_delayd(&r, &state->dq3, &state->dq4);
                G726_delayd(&r, &state->dq2, &state->dq3);
                G726_delayd(&r, &state->dq1, &state->dq2);
                G726_delayd(&r, &state->dq0, &state->dq1);

                G726_delaya(&r, &state->b1r, &b1);
                G726_delaya(&r, &state->b2r, &b2);
                G726_delaya(&r, &state->b3r, &b3);
                G726_delaya(&r, &state->b4r, &b4);
                G726_delaya(&r, &state->b5r, &b5);
                G726_delaya(&r, &state->b6r, &b6);

                G726_fmult(&b1, &state->dq1, &wb1);
                G726_fmult(&b2, &state->dq2, &wb2);
                G726_fmult(&b3, &state->dq3, &wb3);
                G726_fmult(&b4, &state->dq4, &wb4);
                G726_fmult(&b5, &state->dq5, &wb5);
                G726_fmult(&b6, &dq6, &wb6);

                G726_accum(&wa1, &wa2, &wb1, &wb2, &wb3, &wb4, &wb5, &wb6, &se, &sez);

                /* Process 4.2.1 */
                G726_expand(&s, law, &sl);
                G726_subta(&sl, &se, &d);

                /* Process delays and `know-state' part of 4.2.5 */
                G726_delaya(&r, &state->dmsp, &dms);
                G726_delaya(&r, &state->dmlp, &dml);
                G726_delaya(&r, &state->apr, &ap);
                G726_lima(&ap, &al);

                /* Process `know-state' parts of 4.2.4 */
                G726_delayb(&r, &state->yup, &yu);
                G726_delayc(&r, &state->ylp, &yl);
                G726_mix(&al, &yu, &yl, &y);

                /* Process 4.2.2 */
                G726_log(&d, &dl, &ds);
                G726_subtb(&dl, &y, &dln);
                G726_quan(rate, &dln, &ds, &i);

                /* Save ADPCM quantized sample into output buffer */
                out_buf[j] = i;

                /* Process 4.2.3 */
                G726_reconst(rate, &i, &dqln, &dqs);
                G726_adda(&dqln, &y, &dql);
                G726_antilog(&dql, &dqs, &dq);

                /* Part of 4.2.5 */
                G726_functf(rate, &i, &fi);
                G726_filta(&fi, &dms, &state->dmsp);
                G726_filtb(&fi, &dml, &state->dmlp);

                /* Remaining part of 4.2.4 */
                G726_functw(rate, &i, &wi);
                G726_filtd(&wi, &y, &yut);
                G726_limb(&yut, &state->yup);
                G726_filte(&state->yup, &yl, &state->ylp);

                /* Process `known-state' part of 4.2.7 */
                G726_delaya(&r, &state->tdr, &td);
                G726_trans(&td, &yl, &dq, &tr);

                /* More `known-state' parts of 4.2.6: update of `pk's */
                G726_delaya(&r, &state->pk1, &pk2);
                G726_delaya(&r, &state->pk0, &state->pk1);
                G726_addc(&dq, &sez, &state->pk0, &sigpk);

                /* 4.2.6: find sr0 */
                G726_addb(&dq, &se, &sr);
                G726_floatb(&sr, &state->sr0);

                /* 4.2.6: find dq0 */
                G726_floata(&dq, &state->dq0);

                /* 4.2.6: prepar a2(r) */
                G726_upa2(&state->pk0, &state->pk1, &pk2, &a2, &a1, &sigpk, &a2t);
                G726_limc(&a2t, &a2p);
                G726_trigb(&tr, &a2p, &state->a2r);

                /* 4.2.6: prepar a1(r) */
                G726_upa1(&state->pk0, &state->pk1, &a1, &sigpk, &a1t);
                G726_limd(&a1t, &a2p, &a1p);
                G726_trigb(&tr, &a1p, &state->a1r);

                /* Remaining of 4.2.7 */
                G726_tone(&a2p, &tdp);
                G726_trigb(&tr, &tdp, &state->tdr);

                /* Remaining of 4.2.5 */
                G726_subtc(&state->dmsp, &state->dmlp, &tdp, &y, &ax);
                G726_filtc(&ax, &ap, &app);
                G726_triga(&tr, &app, &state->apr);

                /* Remaining of 4.2.6: update of all `b's */
                G726_xor(&state->dq1, &dq, &u1); /* Here, b1 */
                G726_upb(rate, &u1, &b1, &dq, &b1p);
                G726_trigb(&tr, &b1p, &state->b1r);

                G726_xor(&state->dq2, &dq, &u2); /* Here, b2 */
                G726_upb(rate, &u2, &b2, &dq, &b2p);
                G726_trigb(&tr, &b2p, &state->b2r);

                G726_xor(&state->dq3, &dq, &u3); /* Here, b3 */
                G726_upb(rate, &u3, &b3, &dq, &b3p);
                G726_trigb(&tr, &b3p, &state->b3r);

                G726_xor(&state->dq4, &dq, &u4); /* Here, b4 */
                G726_upb(rate, &u4, &b4, &dq, &b4p);
                G726_trigb(&tr, &b4p, &state->b4r);

                G726_xor(&state->dq5, &dq, &u5); /* Here, b5 */
                G726_upb(rate, &u5, &b5, &dq, &b5p);
                G726_trigb(&tr, &b5p, &state->b5r);

                G726_xor(&dq6, &dq, &u6); /* At last, b6 */
                G726_upb(rate, &u6, &b6, &dq, &b6p);
                G726_trigb(&tr, &b6p, &state->b6r);
            }
        }
        /* ........................ end of G726_encode() ....................... */


        /*
          ----------------------------------------------------------------------------

                void G726_decode (short *inp_buf, short *out_buf, long smpno,
                ~~~~~~~~~~~~~~~~  char *law, short rate, short r, G726_state *state);

                Description:
                ~~~~~~~~~~~~

                Simulation of the ITU-T G.726 ADPCM decoder. Takes the ADPCM
                input array of shorts `inp_buf' (16 bit, right- justified,
                without sign extension) of length `smpno', and saves the
                decoded samples (A or mu law) in the array of shorts
                `out_buf', with the same number of samples and
                right-justified.

                The state variables are saved in the structure `state', and the
                reset can be stablished by making r equal to 1. The law is A if
                `law'=='1', and mu law if `law'=='0'.

    
                Return value:
                ~~~~~~~~~~~~~
                None.

                Prototype:      in file g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Version 1.0 in Fortran
                                <tdsindi@venus.cpqd.ansp.br>
                05.Feb.92 v1.0c Version 1.0 in C, by translating Fortran to C (f2c)
                                <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------------
        */
        private static void G726_decode(short* inp_buf, short* out_buf, int smpno, byte* law, short rate, short r,
                                        G726State* state)
        {
            short i;
            short y;
            short sigpk;
            short sr, tr;
            short sp, dlnx, dsx, sd, slx, dlx, dx; /* these are unique to
                             * the decoder */
            int yl;
            short yu;
            short al, fi, ap, dq, se, ax, td, wi;
            short u1, u2, u3, u4, u5, u6;
            short a1, a2, b1, b2, b3, b4, b5, b6;
            short dqln;
            short a1p,
                a2p,
                a1t,
                a2t,
                b1p,
                b2p,
                b3p,
                b4p,
                b5p,
                b6p,
                dq6,
                pk2,
                sr2,
                wa1,
                wa2,
                wb1,
                wb2,
                wb3,
                wb4,
                wb5,
                wb6;
            short dml, app, dql, dms;
            short dqs, tdp;
            short sez;
            short yut;
            int j;

            /* Process all desired samples in inp_buf to out_buf; The comments about
             * general blocks are given as in G.726, and refer to: 4.1.1 Input PCM
             * format conversion and difference signal computation    4.1.2 Adaptive
             * quantizer 4.1.3 Inverse adaptive quantizer 4.1.4 Quantizer scale factor
             * adaptation 4.1.5 Adaptation speed control 4.1.6 Adaptive predictor and
             * reconstructed signal calculator 4.1.7 Tone and transition detector 4.1.8
             * Output PCM format conversion and synchronous coding adjustment */
            for (j = 0; j < smpno; j++, r = 0)
            {
                /* Process `known-state' part of 4.2.6 */
                G726_delayd(&r, &state->sr1, &sr2);
                G726_delayd(&r, &state->sr0, &state->sr1);
                G726_delaya(&r, &state->a2r, &a2);
                G726_delaya(&r, &state->a1r, &a1);
                G726_fmult(&a2, &sr2, &wa2);
                G726_fmult(&a1, &state->sr1, &wa1);

                G726_delayd(&r, &state->dq5, &dq6);
                G726_delayd(&r, &state->dq4, &state->dq5);
                G726_delayd(&r, &state->dq3, &state->dq4);
                G726_delayd(&r, &state->dq2, &state->dq3);
                G726_delayd(&r, &state->dq1, &state->dq2);
                G726_delayd(&r, &state->dq0, &state->dq1);

                G726_delaya(&r, &state->b1r, &b1);
                G726_delaya(&r, &state->b2r, &b2);
                G726_delaya(&r, &state->b3r, &b3);
                G726_delaya(&r, &state->b4r, &b4);
                G726_delaya(&r, &state->b5r, &b5);
                G726_delaya(&r, &state->b6r, &b6);

                G726_fmult(&b1, &state->dq1, &wb1);
                G726_fmult(&b2, &state->dq2, &wb2);
                G726_fmult(&b3, &state->dq3, &wb3);
                G726_fmult(&b4, &state->dq4, &wb4);
                G726_fmult(&b5, &state->dq5, &wb5);
                G726_fmult(&b6, &dq6, &wb6);

                G726_accum(&wa1, &wa2, &wb1, &wb2, &wb3, &wb4, &wb5, &wb6, &se, &sez);

                /* Process delays and `know-state' part of 4.2.5 */
                G726_delaya(&r, &state->dmsp, &dms);
                G726_delaya(&r, &state->dmlp, &dml);
                G726_delaya(&r, &state->apr, &ap);
                G726_lima(&ap, &al);

                /* Process `know-state' parts of 4.2.4 */
                G726_delayb(&r, &state->yup, &yu);
                G726_delayc(&r, &state->ylp, &yl);
                G726_mix(&al, &yu, &yl, &y);

                /* Retrieve ADPCM sample from input buffer */
                i = inp_buf[j];

                /* Process 4.2.3 */
                G726_reconst(rate, &i, &dqln, &dqs);
                G726_adda(&dqln, &y, &dql);
                G726_antilog(&dql, &dqs, &dq);

                /* Process `known-state' part of 4.2.7 */
                G726_delaya(&r, &state->tdr, &td);
                G726_trans(&td, &yl, &dq, &tr);

                /* Part of 4.2.5 */
                G726_functf(rate, &i, &fi);
                G726_filta(&fi, &dms, &state->dmsp);
                G726_filtb(&fi, &dml, &state->dmlp);

                /* Remaining part of 4.2.4 */
                G726_functw(rate, &i, &wi);
                G726_filtd(&wi, &y, &yut);
                G726_limb(&yut, &state->yup);
                G726_filte(&state->yup, &yl, &state->ylp);

                /* More `known-state' parts of 4.2.6: update of `pk's */
                G726_delaya(&r, &state->pk1, &pk2);
                G726_delaya(&r, &state->pk0, &state->pk1);
                G726_addc(&dq, &sez, &state->pk0, &sigpk);

                /* 4.2.6: find sr0 */
                G726_addb(&dq, &se, &sr);
                G726_floatb(&sr, &state->sr0);

                /* 4.2.6: find dq0 */
                G726_floata(&dq, &state->dq0);

                /* Process 4.2.8 */
                G726_compress(&sr, law, &sp);
                G726_expand(&sp, law, &slx);
                G726_subta(&slx, &se, &dx);
                G726_log(&dx, &dlx, &dsx);
                G726_subtb(&dlx, &y, &dlnx);
                G726_sync(rate, &i, &sp, &dlnx, &dsx, law, &sd);

                /* Save output PCM word in output buffer */
                out_buf[j] = sd;

                /* 4.2.6: prepar a2(r) */
                G726_upa2(&state->pk0, &state->pk1, &pk2, &a2, &a1, &sigpk, &a2t);
                G726_limc(&a2t, &a2p);
                G726_trigb(&tr, &a2p, &state->a2r);

                /* 4.2.6: prepar a1(r) */
                G726_upa1(&state->pk0, &state->pk1, &a1, &sigpk, &a1t);
                G726_limd(&a1t, &a2p, &a1p);
                G726_trigb(&tr, &a1p, &state->a1r);

                /* Remaining of 4.2.7 */
                G726_tone(&a2p, &tdp);
                G726_trigb(&tr, &tdp, &state->tdr);

                /* Remaining of 4.2.5 */
                G726_subtc(&state->dmsp, &state->dmlp, &tdp, &y, &ax);
                G726_filtc(&ax, &ap, &app);
                G726_triga(&tr, &app, &state->apr);

                /* Remaining of 4.2.6: update of all `b's */
                G726_xor(&state->dq1, &dq, &u1); /* Here, b1 */
                G726_upb(rate, &u1, &b1, &dq, &b1p);
                G726_trigb(&tr, &b1p, &state->b1r);

                G726_xor(&state->dq2, &dq, &u2); /* Here, b2 */
                G726_upb(rate, &u2, &b2, &dq, &b2p);
                G726_trigb(&tr, &b2p, &state->b2r);

                G726_xor(&state->dq3, &dq, &u3); /* Here, b3 */
                G726_upb(rate, &u3, &b3, &dq, &b3p);
                G726_trigb(&tr, &b3p, &state->b3r);

                G726_xor(&state->dq4, &dq, &u4); /* Here, b4 */
                G726_upb(rate, &u4, &b4, &dq, &b4p);
                G726_trigb(&tr, &b4p, &state->b4r);

                G726_xor(&state->dq5, &dq, &u5); /* Here, b5 */
                G726_upb(rate, &u5, &b5, &dq, &b5p);
                G726_trigb(&tr, &b5p, &state->b5r);

                G726_xor(&dq6, &dq, &u6); /* At last, b6 */
                G726_upb(rate, &u6, &b6, &dq, &b6p);
                G726_trigb(&tr, &b6p, &state->b6r);
            }

            /* Invert even bits if A law */
            if (*law == '1')
            {
                for (j = 0; j < smpno; j++)
                {
                    out_buf[j] ^= 85;
                }
            }
        }
        /* ...................... end of G726_decode() ...................... */


        /*
          ----------------------------------------------------------------------

                void G726_expand (short *s, char *law, short *sl);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Convert either a-law (law=1) or u-law (law=0)  to uniform pcm.

            Inputs: s (sp in decoder), law
            Output: sl (slx in decoder)

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_expand(short* s, byte* law, short* sl)
        {
            int mant, iexp;
            short s1, ss, sig, ssm, ssq, sss;

            s1 = *s;

            if (*law == '1')
            {
                /* Invert sign bit */
                s1 ^= 128;
                if (s1 >= 128)
                {
                    s1 += -128;
                    sig = 4096;
                }
                else
                {
                    sig = 0;
                }

                iexp = s1 / 16;

                mant = s1 - (iexp << 4);
                ss = iexp == 0
                    ? (short) ((mant << 1) + 1 + sig)
                    : (short) ((1 << (iexp - 1)) * ((mant << 1) + 33) + sig);

                sss = (short) (ss / 4096);
                ssm = (short) (ss & 4095);
                ssq = (short) (ssm << 1);
            }
            else
            {
                /* Invert sign bit */
                s1 ^= 128;
                if (s1 >= 128)
                {
                    s1 += -128;
                    s1 ^= 127;
                    sig = 8192;
                }
                else
                {
                    sig = 0;
                    s1 ^= 127;
                }

                iexp = s1 / 16;

                mant = s1 - (iexp << 4);

                ss = iexp == 0 ? (short) ((mant << 1) + sig) : (short) ((1 << iexp) * ((mant << 1) + 33) - 33 + sig);

                sss = (short) (ss / 8192);
                ssq = (short) (ss & 8191);
            }

            *sl = sss == 0 ? ssq : (short) ((16384 - ssq) & 16383);
        }
        /* ...................... end of G726_expand() ...................... */


        /*
         ----------------------------------------------------------------------

                void G726_subta (short *sl, short *se, short *d);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Compute difference signal by subtracting  signal estimate from
                input signal (or  quantized reconstructed signal in decoder).

            Inputs: sl (slx in decoder), se
            Output: d  (dx in decoder)


                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>


         ----------------------------------------------------------------------
        */
        private static void G726_subta(short* sl, short* se, short* d)
        {
            int se1;
            int sl1, sei, sli;
            short ses, sls;

            sls = (short) (*sl >> 13);

            sl1 = *sl;
            se1 = *se;

            /* Sign extension */
            sli = sls == 0 ? sl1 : sl1 + 49152;

            ses = (short) (*se >> 14);

            /* Sign extension */
            sei = ses == 0 ? se1 : se1 + 32768;

            /* 16 bit TC */
            *d = (short) ((sli + 65536 - sei) & 65535);
        }
        /* ......................... end of G726_subta() ......................... */


        /*
          --------------------------------------------------------------------------

                void G726_log (short *d, short *dl, short *ds);
                ~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Convert difference signal from linear to the logarithmic domain.

                Prototype: in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f 1st release to UGST, in Fortran.
                                <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c release of 1st version, in C.
                            <tdsimao@venus.cpqd.ansp.br>

          --------------------------------------------------------------------------
        */
        private static void G726_log(short* d, short* dl, short* ds)
        {
            int mant;
            int d1;
            int dqm, exp_;

            *ds = (short) (*d >> 15);

            d1 = *d;

            /* Convert from 2-complement to signed magnitude */
            dqm = *ds != 0 ? (65536 - d1) & 32767 : d1;

            /* Compute exponent */
            if (dqm >= 16384)
            {
                exp_ = 14;
            }
            else if (dqm >= 8192)
            {
                exp_ = 13;
            }
            else if (dqm >= 4096)
            {
                exp_ = 12;
            }
            else if (dqm >= 2048)
            {
                exp_ = 11;
            }
            else if (dqm >= 1024)
            {
                exp_ = 10;
            }
            else if (dqm >= 512)
            {
                exp_ = 9;
            }
            else if (dqm >= 256)
            {
                exp_ = 8;
            }
            else if (dqm >= 128)
            {
                exp_ = 7;
            }
            else if (dqm >= 64)
            {
                exp_ = 6;
            }
            else if (dqm >= 32)
            {
                exp_ = 5;
            }
            else if (dqm >= 16)
            {
                exp_ = 4;
            }
            else if (dqm >= 8)
            {
                exp_ = 3;
            }
            else if (dqm >= 4)
            {
                exp_ = 2;
            }
            else if (dqm >= 2)
            {
                exp_ = 1;
            }
            else
            {
                exp_ = 0;
            }

            /* Compute approximation log2(1+x) = x */
            mant = ((dqm << 7) >> exp_) & 127;

            /* Combine mantissa and exponent (7 and 4) bits into a 11-bit word */
            *dl = (short) ((exp_ << 7) + mant);
        }
        /* ........................ end of G726_log() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_quan (short rate, short *dln, short *ds, short *i);
                ~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Quantize difference signal in logarithmic
            domain.

            Inputs: dln, ds, rate
            Output: i


                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_quan(short rate, short* dln, short* ds, short* i)
        {
            if (rate == 4)
            {
                if (*dln >= 3972)
                {
                    *i = 1;
                }
                else if (*dln >= 2048)
                {
                    *i = 15;
                }
                else if (*dln >= 400)
                {
                    *i = 7;
                }
                else if (*dln >= 349)
                {
                    *i = 6;
                }
                else if (*dln >= 300)
                {
                    *i = 5;
                }
                else if (*dln >= 246)
                {
                    *i = 4;
                }
                else if (*dln >= 178)
                {
                    *i = 3;
                }
                else if (*dln >= 80)
                {
                    *i = 2;
                }
                else
                {
                    *i = 1;
                }

                /* Adjust for sign */
                if (*ds != 0)
                {
                    *i = (short) (15 - *i);
                }

                if (*i == 0)
                {
                    *i = 15;
                }
            } /* ......... end of 32 kbit part ........... */


            else if (rate == 3)
            {
                if (*dln >= 2048)
                {
                    *i = 7;
                }
                else if (*dln >= 331)
                {
                    *i = 3;
                }
                else if (*dln >= 218)
                {
                    *i = 2;
                }
                else if (*dln >= 8)
                {
                    *i = 1;
                }
                else if (*dln >= 0)
                {
                    *i = 7;
                }

                /* Adjust for sign */
                if (*ds != 0)
                {
                    *i = (short) (7 - *i);
                }

                if (*i == 0)
                {
                    *i = 7;
                }
            } /* ......... end of 24 kbit part ........... */

            else if (rate == 2)
            {
                if (*dln >= 2048)
                {
                    *i = 0;
                }
                else if (*dln >= 261)
                {
                    *i = 1;
                }
                else
                {
                    *i = 0;
                }

                /* Adjust for sign */
                if (*ds != 0)
                {
                    *i = (short) (3 - *i);
                }
            } /* ......... end of 16 kbit part ........... */

            else
            {
                if (*dln >= 4080)
                {
                    *i = 2;
                }
                else if (*dln >= 3974)
                {
                    *i = 1;
                }
                else if (*dln >= 2048)
                {
                    *i = 31;
                }
                else if (*dln >= 553)
                {
                    *i = 15;
                }
                else if (*dln >= 528)
                {
                    *i = 14;
                }
                else if (*dln >= 502)
                {
                    *i = 13;
                }
                else if (*dln >= 475)
                {
                    *i = 12;
                }
                else if (*dln >= 445)
                {
                    *i = 11;
                }
                else if (*dln >= 413)
                {
                    *i = 10;
                }
                else if (*dln >= 378)
                {
                    *i = 9;
                }
                else if (*dln >= 339)
                {
                    *i = 8;
                }
                else if (*dln >= 298)
                {
                    *i = 7;
                }
                else if (*dln >= 250)
                {
                    *i = 6;
                }
                else if (*dln >= 198)
                {
                    *i = 5;
                }
                else if (*dln >= 139)
                {
                    *i = 4;
                }
                else if (*dln >= 68)
                {
                    *i = 3;
                }
                else if (*dln >= 0)
                {
                    *i = 2;
                }

                if (*ds != 0)
                {
                    *i = (short) (31 - *i);
                }

                if (*i == 0)
                {
                    *i = 31;
                }
            } /* ......... end of 40 kbit part ........... */
        }
        /* ........................ end of G726_quan() ........................ */


        /*
         ----------------------------------------------------------------------

                void G726_subtb (short *dl, short *y, short *dln);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Scale logarithmic version of difference signal  by subtracting
                scale factor.

            Inputs:   dl (dlx no decodificador), y
            Output:   dln (dlnx no decodificador)


                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_subtb(short* dl, short* y, short* dln)
        {
            *dln = (short) ((*dl + 4096 - (*y >> 2)) & 4095);
        }
        /* ........................ end of G726_subtb() ........................ */


        /*
         ----------------------------------------------------------------------

                void G726_adda (short *dqln, short *y, short *dql);
                ~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Addition of scale factor to logarithmic  version of quantized
                difference signal.

            Inputs: dqln, y
            Output: dql


                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_adda(short* dqln, short* y, short* dql)
        {
            *dql = (short) ((*dqln + (*y >> 2)) & 4095);
        }
        /* ....................... end of G726_adda() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_antilog (short *dql, short *dqs, short *dq);
                ~~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Convert quantized difference signal from the  logarithmic to
                the linear domain.

            Inputs:   dql, dqs
            Output:   dq

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_antilog(short* dql, short* dqs, short* dq)
        {
            int dqmag;
            int ds, dmn, dex, dqt;

            /* Extract 4-bit exponent */
            ds = *dql >> 11;
            dex = (*dql >> 7) & 15;

            /* Extract 7-bit mantissa */
            dmn = *dql & 127;

            dqt = dmn + 128;

            /* Convert mantissa to linear using the approx. 2**x = 1+x */
            dqmag = ds != 0 ? 0 : (dqt << 7) >> (14 - dex);

            /* Attach sign bit to signed mag. word */
            *dq = (short) ((*dqs << 15) + dqmag);
        }
        /* ..................... end of G726_antilog() ..................... */


        /*
         ----------------------------------------------------------------------

                void G726_reconst (short rate, short *i, short *dqln, short *dqs);
                ~~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Reconstruction of quantized difference signal  in the
                logarithmic domain.

            Inputs:   i, rate
            Outputs:  dqln, dqs

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static short[] tab1 = new short[]
            {2048, 4, 135, 213, 273, 323, 373, 425, 425, 373, 323, 273, 213, 135, 4, 2048};

        private static short[] tab2 = new short[] {2048, 135, 273, 373, 373, 273, 135, 2048};
        private static short[] tab3 = new short[] {116, 365, 365, 116};

        private static short[] tab4 = new short[]
        {
            2048, 4030, 28, 104, 169, 224, 274, 318, 358, 395, 429,
            459, 488, 514, 539, 566, 566, 539, 514, 488, 459, 429, 395, 358, 318, 274, 224,
            169, 104, 28, 4030, 2048
        };

        private static void G726_reconst(short rate, short* i, short* dqln, short* dqs)
        {
            if (rate == 4)
            {
                /* Initialized data */
                //static short    tab[16] = {2048, 4, 135, 213, 273, 323, 373, 425,
                //425, 373, 323, 273, 213, 135, 4, 2048};

                /* Extract sign */
                *dqs = (short) (*i >> 3);

                /* Table look-up */
                *dqln = tab1[*i];
            } /* ............... end of 32 kbit part
                 * ................. */

            else if (rate == 3)
            {
                /* Initialized data */
                //static short    tab[8] = {2048, 135, 273, 373, 373, 273, 135, 2048};

                *dqs = (short) (*i >> 2);

                /* Table look-up */
                *dqln = tab2[*i];
            } /* ............... end of 24 kbit part
                 * ................. */


            else if (rate == 2)
            {
                /* Initialized data */
                //static short    tab[4] = {116, 365, 365, 116};

                *dqs = (short) (*i >> 1);

                /* Table look-up */
                *dqln = tab3[*i];
            } /* ............... end of 16 kbit part
                 * ................. */
            else
            {
                /* Initialized data */
                //static short    tab[32] = {2048, 4030, 28, 104, 169, 224, 274, 318, 358, 395, 429,
                //  459, 488, 514, 539, 566, 566, 539, 514, 488, 459, 429, 395, 358, 318, 274, 224,
                //169, 104, 28, 4030, 2048};

                *dqs = (short) (*i >> 4);

                /* Table look-up */
                *dqln = tab4[*i];
            } /* ................ end of 40 kbit part
                 * ................... */
        }
        /* ....................... end of G726_reconst() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_delaya (short *r, short *x, short *y);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Memory block.

            Inputs:    r, x
            Output:    y

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_delaya(short* r, short* x, short* y)
        {
            *y = *r == 0 ? *x : (short) 0;
        }
        /* ....................... end of G726_delaya() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_delayb (short *r, short *x, short *y);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Memory block.

            Inputs:    r, x
            Output:    y

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_delayb(short* r, short* x, short* y)
        {
            *y = *r == 0 ? *x : (short) 544;
        }
        /* ....................... end of G726_delayb() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_delayc (short *r, long *x, long *y);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Memory block.

            Inputs:    r, x
            Output:    y

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_delayc(short* r, int* x, int* y)
        {
            *y = *r == 0 ? *x : 34816;
        }
        /* ....................... end of G726_delayc() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_delayd (short *r, short *x, short *y);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Memory block

            Inputs:   r, x
            Output:   y

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_delayd(short* r, short* x, short* y)
        {
            *y = *r == 0 ? *x : (short) 32;
        }
        /* ....................... end of G726_delayd() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_filtd (short *wi, short *y, short *yut);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Update of fast quantizer scale factor.

            Inputs: wi, y
            Output: yut

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_filtd(short* wi, short* y, short* yut)
        {
            int difs, difsx;
            int y1;
            int wi1, dif;

            /* Compute difference */
            wi1 = *wi;
            y1 = *y;
            dif = ((wi1 << 5) + 131072 - y1) & 131071;
            difs = dif >> 16;

            /* Time constant is 1/32; sign extension */
            difsx = difs == 0 ? dif >> 5 : (dif >> 5) + 4096;

            *yut = (short) ((y1 + difsx) & 8191);
        }
        /* ....................... end of G726_filte() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_filte (short *yup, long *yl, long *ylp);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Update of slow quantizer scale factor.


            Inputs:  yup, yl
            Output:  ylp

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_filte(short* yup, int* yl, int* ylp)
        {
            int difs, difsx;
            int dif, dif1, yup1;

            /* Compute difference */
            yup1 = *yup;
            dif1 = 1048576 - *yl;
            dif = (yup1 + (dif1 >> 6)) & 16383;
            difs = dif >> 13;

            /* Sign extension */
            difsx = difs == 0 ? dif : dif + 507904;

            *ylp = (*yl + difsx) & 524287;
        }
        /* ....................... end of G726_filte() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_functw (short rate, short *i, short *wi);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Map quantizer output into logarithmic version  of scale factor
                multiplier.

            Inputs: i, rate
            Output: wi

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static short[] tab5 = new short[] {4084, 18, 41, 64, 112, 198, 355, 1122};
        private static short[] tab6 = new short[] {4092, 30, 137, 582};
        private static short[] tab7 = new short[] {4074, 439};

        private static short[] tab8 = new short[]
            {14, 14, 24, 39, 40, 41, 58, 100, 141, 179, 219, 280, 358, 440, 529, 696};

        private static void G726_functw(short rate, short* i, short* wi)
        {
            if (rate == 4)
            {
                /* Initialized data */
                //static short    tab[8] = {4084, 18, 41, 64, 112, 198, 355, 1122};
                short im, _is;

                _is = (short) (*i >> 3);

                im = _is == 0 ? (short) (*i & 7) : (short) ((15 - *i) & 7);

                /* Scale factor multiplier */
                *wi = tab5[im];
            } /* ................. end of 32 kbit part
                 * .............. */


            else if (rate == 3)
            {
                /* Initialized data */
                //static short    tab[4] = {4092, 30, 137, 582};
                short im, _is;


                _is = (short) (*i >> 2);

                im = _is == 0 ? (short) (*i & 3) : (short) ((7 - *i) & 3);

                *wi = tab6[im];
            } /* ................. end of 24 kbit part
                 * .............. */


            else if (rate == 2)
            {
                /* Initialized data */
                //static short    tab[2] = {4074, 439};
                short im, _is;


                _is = (short) (*i >> 1);

                im = _is == 0 ? (short) (*i & 1) : (short) ((3 - *i) & 1);

                *wi = tab7[im];
            } /* ................. end of 16 kbit part
                 * .............. */


            else
            {
                /* Initialized data */
                //static short    tab[16] = {14, 14, 24, 39, 40, 41, 58, 100, 141, 179, 219, 280, 358,
                //440, 529, 696};
                short im, _is;

                _is = (short) (*i >> 4);

                im = _is == 0 ? (short) (*i & 15) : (short) ((31 - *i) & 15);

                *wi = tab8[im];
            } /* ................. end of 40 kbit part
                 * .............. */
        }
        /* ....................... end of G726_functw() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_limb (short *yut, short *yup);
                ~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Limit quantizer scale factor.

            Inputs: yut
            Output: yup

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_limb(short* yut, short* yup)
        {
            short gell, geul;

            geul = (short) (((*yut + 11264) & 16383) >> 13);
            gell = (short) (((*yut + 15840) & 16383) >> 13);

            if (gell == 1)
            {
                *yup = 544; /* Lower limit is 1.06 */
            }
            else if (geul == 0)
            {
                *yup = 5120; /* Upper limit is 10.0 */
            }
            else
            {
                *yup = *yut;
            }
        }
        /* ....................... end of G726_limb() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_mix (short *al, short *yu, long *yl, short *y);
                ~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Form linear combination of fast and slow  quantizer scale
                factors.

            Inputs:    al, yu, yl
            Output:    y


                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_mix(short* al, short* yu, int* yl, short* y)
        {
            int difm, difs, prod;
            int prodm, al1;
            int yu1, dif;

            /* Preamble */
            al1 = *al;
            yu1 = *yu;

            /* Compute difference */
            dif = (yu1 + 16384 - (*yl >> 6)) & 16383;
            difs = dif >> 13;

            /* Compute magnitude of difference */
            difm = difs == 0 ? dif : (16384 - dif) & 8191;

            /* Compute magnitude of product */
            prodm = (difm * al1) >> 6;

            /* Convert magnitude to two's complement */
            prod = difs == 0 ? prodm : (16384 - prodm) & 16383;

            *y = (short) (((*yl >> 6) + prod) & 8191);
        }
        /* ....................... end of G726_mix() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_filta (short *fi, short *dms, short *dmsp);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Update of short term average of f(i).

            Inputs:   fi, dms
            Output:   dmsp

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_filta(short* fi, short* dms, short* dmsp)
        {
            short difs, difsx;
            short dif;

            /* Compute difference */
            dif = (short) (((*fi << 9) + 8192 - *dms) & 8191);
            difs = (short) (dif >> 12);

            /* Time constant is 1/32, sign extension */
            difsx = difs == 0 ? (short) (dif >> 5) : (short) ((dif >> 5) + 3840);

            *dmsp = (short) ((difsx + *dms) & 4095);
        }
        /* ....................... end of G726_filta() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_filtb (short *fi, short *dml, short *dmlp);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Update of long term average of f(i).

            Inputs:    fi, dml
            Output:    dmlp

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_filtb(short* fi, short* dml, short* dmlp)
        {
            int difs, difsx;
            int fi1;
            int dif, dml1;

            /* Preamble */
            fi1 = *fi;
            dml1 = *dml;

            /* Compute difference */
            dif = ((fi1 << 11) + 32768 - dml1) & 32767;
            difs = dif >> 14;

            /* Time constant is 1/28, sign extension */
            difsx = difs == 0 ? dif >> 7 : (dif >> 7) + 16128;

            *dmlp = (short) ((difsx + dml1) & 16383);
        }
        /* ....................... end of G726_filtb() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_filtc (short *ax, short *ap, short *app);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Low pass filter of speed control parameter.

            Inputs:   ax, ap
            Output:   app

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_filtc(short* ax, short* ap, short* app)
        {
            short difs, difsx;
            short dif;

            /* Compute difference */
            dif = (short) (((*ax << 9) + 2048 - *ap) & 2047);
            difs = (short) (dif >> 10);

            /* Time constant is 1/16, sign extension */
            difsx = difs == 0 ? (short) (dif >> 4) : (short) ((dif >> 4) + 896);

            *app = (short) ((difsx + *ap) & 1023);
        }
        /* .................... end of G726_filtc() .................... */


        /*
         ----------------------------------------------------------------------

                void G726_functf (short rate, short *i, short *fi);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Map quantizer output into the f(i) function.

            Inputs:   i, rate
            Output:   fi

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static short[] tab9 = new short[] {0, 0, 0, 1, 1, 1, 3, 7};
        private static short[] tab10 = new short[] {0, 1, 2, 7};
        private static short[] tab11 = new short[] {0, 7};
        private static short[] tab12 = new short[] {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 3, 4, 5, 6, 6};

        private static void G726_functf(short rate, short* i, short* fi)
        {
            short im, _is;

            if (rate == 4)
            {
                /* Initialized data */
                //static short    tab[8] = {0, 0, 0, 1, 1, 1, 3, 7};

                _is = (short) (*i >> 3);

                im = _is == 0 ? (short) (*i & 7) : (short) ((15 - *i) & 7);

                *fi = tab9[im];
            } /* ................ end of 32 kbit part
                 * ................. */

            else if (rate == 3)
            {
                /* Initialized data */
                //static short    tab[4] = {0, 1, 2, 7};

                _is = (short) (*i >> 2);

                im = _is == 0 ? (short) (*i & 3) : (short) ((7 - *i) & 3);

                *fi = tab10[im];
            } /* ................ end of 24 kbit part
                 * ................. */
            else if (rate == 2)
            {
                /* Initialized data */
                //static short    tab[2] = {0, 7};


                _is = (short) (*i >> 1);

                im = _is == 0 ? (short) (*i & 1) : (short) ((3 - *i) & 1);

                *fi = tab11[im];
            } /* ................ end of 16 kbit part
                 * ................. */

            else
            {
                /* Initialized data */
                //static short    tab[16] = {0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 3, 4, 5, 6, 6};


                _is = (short) (*i >> 4);

                im = _is == 0 ? (short) (*i & 15) : (short) ((31 - *i) & 15);

                *fi = tab12[im];
            } /* ................ end of 40 kbit part
                 * ................. */
        }
        /* ...................... end of G726_functf() ...................... */


        /*
         ----------------------------------------------------------------------

                void G726_lima (short *ap, short *al);
                ~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Limit speed control parameter.

            Inputs:   ap
            Output:   al

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_lima(short* ap, short* al)
        {
            *al = *ap >= 256 ? (short) 64 : (short) (*ap >> 2);
        }
        /* ....................... end of G726_lima() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_subtc (short *dmsp, short *dmlp, short *tdp,
                ~~~~~~~~~~~~~~~  short *y, short *ax);

                Description:
                ~~~~~~~~~~~~

                Functions of quantizer output sequence  and then perform
                threshold comparison for  quantizing speed control parameter.
                compute magnitude of the difference of short and  long-term

            Inputs:   dmsp, dmlp, tdp, y
            Output:   ax

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_subtc(short* dmsp, short* dmlp, short* tdp, short* y, short* ax)
        {
            int difm, difs, dthr, dmlp1, dmsp1;
            int dif;

            /* Preamble */
            dmsp1 = *dmsp;
            dmlp1 = *dmlp;

            /* Compute difference */
            dif = ((dmsp1 << 2) + 32768 - dmlp1) & 32767;
            difs = dif >> 14;

            /* Compute magnitude of difference */
            difm = difs == 0 ? dif : (32768 - dif) & 16383;

            /* Compute threshold */
            dthr = dmlp1 >> 3;

            /* Quantize speed control parameter */
            *ax = *y >= 1536 && difm < dthr && *tdp == 0 ? (short) 0 : (short) 1;
        }
        /* ....................... end of G726_subtc() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_triga (short *tr, short *app, short *apr);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Speed control trigger block.

            Inputs: tr, app
            Output: apr

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_triga(short* tr, short* app, short* apr)
        {
            *apr = *tr == 0 ? *app : (short) 256;
        }
        /* ....................... end of G726_triga() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_accum (short *wa1, short *wa2, short *wb1,
                ~~~~~~~~~~~~~~~  short *wb2, short *wb3, short *wb4,
                                short *wb5, short *wb6, short *se, short *sez);

                Description:
                ~~~~~~~~~~~~

                Addition of predictor outputs to form the partial  signal
                estimate (from the sixth order predictor)  and the signal
                estimate.

            Inputs:   wa1, wa2, wb1, wb2, wb3, wb4, wb5, wb6
            Output:   se, sez

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_accum(short* wa1, short* wa2, short* wb1, short* wb2, short* wb3, short* wb4,
                                       short* wb5, short* wb6, short* se, short* sez)
        {
            uint sezi;
            uint wa11, wa21, wb11, wb21, wb31, wb41, wb51, wb61, sei;

            /* Preamble */
            wa11 = (uint) *wa1;
            wa21 = (uint) *wa2;
            wb11 = (uint) *wb1;
            wb21 = (uint) *wb2;
            wb31 = (uint) *wb3;
            wb41 = (uint) *wb4;
            wb51 = (uint) *wb5;
            wb61 = (uint) *wb6;

            /* Sum of partial signal estimate */
            sezi = (((((((((wb11 + wb21) & 65535) + wb31) & 65535)
                        + wb41) & 65535) + wb51) & 65535) + wb61) & 65535;

            /* Complete sum for signal estimate */
            sei = (((sezi + wa21) & 65535) + wa11) & 65535;

            *sez = (short) (sezi >> 1);
            *se = (short) (sei >> 1);
        }
        /* ....................... end of G726_accum() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_addb (short *dq, short *se, short *sr);
                ~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Addition of quantized difference signal and  signal estimate
                to form reconstructed signal.

            Inputs:   dq, se
            Output:   sr

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

          ----------------------------------------------------------------------
        */
        private static void G726_addb(short* dq, short* se, short* sr)
        {
            uint dq1, se1;
            uint dqi, sei;
            short dqs, ses;

            /* Preamble */
            dq1 = (uint) *dq & 65535;
            se1 = (uint) *se;

            /* Sign */
            dqs = (short) ((*dq >> 15) & 1);

            /* Convert signed magnitude to 2's complement */
            dqi = dqs == 0 ? dq1 : (65536 - (dq1 & 32767)) & 65535;

            ses = (short) (*se >> 14);

            /* Sign extension */
            sei = ses == 0 ? se1 : (1 << 15) + se1;

            *sr = (short) ((dqi + sei) & 65535);
        }
        /* ....................... end of G726_addb() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_addc (short *dq, short *sez, short *pk0,
                ~~~~~~~~~~~~~~  short *sigpk);

                Description:
                ~~~~~~~~~~~~

                Obtain sign of addition of quantized difference  signal and
                partial signal estimate.

            Inputs:   dq, sez
            Outputs:   pk0, sigpk

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_addc(short* dq, short* sez, short* pk0, short* sigpk)
        {
            uint sezi;
            short sezs;
            uint dqsez, dq1;
            uint dqi;
            short dqs;
            uint sez1;

            /* Preamble */
            dq1 = (uint) *dq & 65535;
            sez1 = (uint) *sez;

            /* Get sign */
            dqs = (short) ((*dq >> 15) & 1);

            /* Convert signed magnitude to 2's compelemnent */
            dqi = dqs == 0 ? dq1 : (65536 - (dq1 & 32767)) & 65535;

            sezs = (short) (*sez >> 14);

            /* Sign extension */
            sezi = sezs == 0 ? sez1 : sez1 + 32768;

            dqsez = (dqi + sezi) & 65535;

            *pk0 = (short) (dqsez >> 15);
            *sigpk = dqsez == 0 ? (short) 1 : (short) 0;
        }
        /* ....................... end of G726_addc() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_floata (short *dq, short *dq0);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Convert 16-bit signed magnitude to floating  point.

            Inputs:  dq
            Output:  dq0

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_floata(short* dq, short* dq0)
        {
            int mant;
            int mag, exp_;
            int dqs;

            dqs = (*dq >> 15) & 1;

            /* Compute magnitude */
            mag = *dq & 32767;

            /* Exponent */
            if (mag >= 16384)
            {
                exp_ = 15;
            }
            else if (mag >= 8192)
            {
                exp_ = 14;
            }
            else if (mag >= 4096)
            {
                exp_ = 13;
            }
            else if (mag >= 2048)
            {
                exp_ = 12;
            }
            else if (mag >= 1024)
            {
                exp_ = 11;
            }
            else if (mag >= 512)
            {
                exp_ = 10;
            }
            else if (mag >= 256)
            {
                exp_ = 9;
            }
            else if (mag >= 128)
            {
                exp_ = 8;
            }
            else if (mag >= 64)
            {
                exp_ = 7;
            }
            else if (mag >= 32)
            {
                exp_ = 6;
            }
            else if (mag >= 16)
            {
                exp_ = 5;
            }
            else if (mag >= 8)
            {
                exp_ = 4;
            }
            else if (mag >= 4)
            {
                exp_ = 3;
            }
            else if (mag >= 2)
            {
                exp_ = 2;
            }
            else if (mag == 1)
            {
                exp_ = 1;
            }
            else
            {
                exp_ = 0;
            }

            /* Compute mantissa w/a 1 in the most sig. bit */
            mant = mag == 0 ? 1 << 5 : (mag << 6) >> exp_;
            /* Combine sign, exponent and mantissa (1,4,6) bits in a word */
            *dq0 = (short) ((dqs << 10) + (exp_ << 6) + mant);
        }
        /* ....................... end of G726_floata() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_floatb (short *sr, short *sr0);
                ~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Convert 16-bit two's complement to floating point.

            Inputs:   sr
            Output:   sr0

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_floatb(short* sr, short* sr0)
        {
            int mant;
            int mag, exp_, srr, srs;

            /* Preamble */
            srr = *sr & 65535;

            /* Sign */
            srs = srr >> 15;

            /* Compute magnitude */
            mag = srs == 0 ? srr : (65536 - srr) & 32767;

            /* Exponent */
            if (mag >= 16384)
            {
                exp_ = 15;
            }
            else if (mag >= 8192)
            {
                exp_ = 14;
            }
            else if (mag >= 4096)
            {
                exp_ = 13;
            }
            else if (mag >= 2048)
            {
                exp_ = 12;
            }
            else if (mag >= 1024)
            {
                exp_ = 11;
            }
            else if (mag >= 512)
            {
                exp_ = 10;
            }
            else if (mag >= 256)
            {
                exp_ = 9;
            }
            else if (mag >= 128)
            {
                exp_ = 8;
            }
            else if (mag >= 64)
            {
                exp_ = 7;
            }
            else if (mag >= 32)
            {
                exp_ = 6;
            }
            else if (mag >= 16)
            {
                exp_ = 5;
            }
            else if (mag >= 8)
            {
                exp_ = 4;
            }
            else if (mag >= 4)
            {
                exp_ = 3;
            }
            else if (mag >= 2)
            {
                exp_ = 2;
            }
            else if (mag == 1)
            {
                exp_ = 1;
            }
            else
            {
                exp_ = 0;
            }

            /* Compute mantissa w/a 1 in the most sig. bit */
            mant = mag == 0 ? 1 << 5 : (mag << 6) >> exp_;

            /* Combine sign, exponent and mantissa (1,4,6) bits in a word */
            *sr0 = (short) ((srs << 10) + (exp_ << 6) + mant);
        }
        /* ....................... end of G726_floatb() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_fmult (short *a, short *srn, short *wa);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Multiply predictor coefficients with corresponding  quantized
                difference signal or reconstructed  signal. multiplication is
                done in floating piont format

            Inputs:  a (or b), srn (or dqn)
            Outputs:  wa (or wb)

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_fmult(short* An, short* SRn, short* WAn)
        {
            int anmag, anexp, wanmag, anmant;
            int wanexp, srnexp, an, ans, wanmant, srnmant;
            int wan, wans, srns, srn1;

            /* Preamble */
            an = *An & 65535;
            srn1 = *SRn & 65535;

            /* Sign */
            ans = an & 32768;
            ans = ans >> 15;

            /* Convert 2's complement to signed magnitude */
            anmag = ans == 0 ? an >> 2 : (16384 - (an >> 2)) & 8191;

            /* Exponent */
            if (anmag >= 4096)
            {
                anexp = 13;
            }
            else if (anmag >= 2048)
            {
                anexp = 12;
            }
            else if (anmag >= 1024)
            {
                anexp = 11;
            }
            else if (anmag >= 512)
            {
                anexp = 10;
            }
            else if (anmag >= 256)
            {
                anexp = 9;
            }
            else if (anmag >= 128)
            {
                anexp = 8;
            }
            else if (anmag >= 64)
            {
                anexp = 7;
            }
            else if (anmag >= 32)
            {
                anexp = 6;
            }
            else if (anmag >= 16)
            {
                anexp = 5;
            }
            else if (anmag >= 8)
            {
                anexp = 4;
            }
            else if (anmag >= 4)
            {
                anexp = 3;
            }
            else if (anmag >= 2)
            {
                anexp = 2;
            }
            else if (anmag == 1)
            {
                anexp = 1;
            }
            else
            {
                anexp = 0;
            }

            /* Compute mantissa w/a 1 in the most sig. bit */
            anmant = anmag == 0 ? 1 << 5 : (anmag << 6) >> anexp;

            /* Split floating point word into sign, exponent and mantissa */
            srns = srn1 >> 10;
            srnexp = (srn1 >> 6) & 15;
            srnmant = srn1 & 63;

            /* Floating point multiplication */
            wans = srns ^ ans;
            wanexp = srnexp + anexp;
            wanmant = (srnmant * anmant + 48) >> 4;

            /* Convert floating point to magnitude */
            wanmag = wanexp <= 26 ? (wanmant << 7) >> (26 - wanexp) : (wanmant << 7 << (wanexp - 26)) & 32767;

            /* Convert mag. to 2's complement */
            wan = wans == 0 ? wanmag : (65536 - wanmag) & 65535;

            *WAn = (short) wan;
        }
        /* ....................... end of G726_fmult() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_limc (short *a2t, short *a2p);
                ~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Limits on a2 coefficient of second order  predictor.

            Inputs:   a2t
            Output:   a2p

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */

        private static void G726_limc(short* a2t, short* a2p)
        {
            int a2p1, a2t1, a2ll, a2ul;

            a2t1 = *a2t & 65535;

            a2ul = 12288; /* Upper limit of +.75 */
            a2ll = 53248; /* Lower limit of -.75 */

            if (a2t1 >= 32768 && a2t1 <= a2ll)
            {
                a2p1 = a2ll;
            }
            else if (a2t1 >= a2ul && a2t1 <= 32767)
            {
                a2p1 = a2ul;
            }
            else
            {
                a2p1 = a2t1;
            }

            *a2p = (short) a2p1;
        }
        /* ....................... end of G726_limc() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_limd (short *a1t, short *a2p, short *a1p);
                ~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Limits on a1 coefficient of second order  predictor.

            Inputs:   a1t, a2p
            Output:   a1p

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_limd(short* a1t, short* a2p, short* a1p)
        {
            int a1p1, a2p1, a1t1, ome, a1ll, a1ul;

            /* Preamble */
            a1t1 = *a1t & 65535;
            a2p1 = *a2p & 65535;

            /* (1-epsilon), where epsilon = (1/16) */
            ome = 15360;

            /* Compute upper limit */
            a1ul = (ome + 65536 - a2p1) & 65535;

            /* Compute lower limit */
            a1ll = (a2p1 + 65536 - ome) & 65535;

            if (a1t1 >= 32768 && a1t1 <= a1ll)
            {
                a1p1 = a1ll;
            }
            else if (a1t1 >= a1ul && a1t1 <= 32767)
            {
                a1p1 = a1ul;
            }
            else
            {
                a1p1 = a1t1;
            }

            *a1p = (short) a1p1;
        }
        /* ....................... end of G726_limd() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_trigb (short *tr, short *ap, short *ar);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Predictor trigger block.

            Inputs: tr, ap (ou bp ou tdp)
            Output: ar (ou br ou tdr)

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_trigb(short* tr, short* ap, short* ar)
        {
            *ar = *tr == 0 ? *ap : (short) 0;
        }
        /* ....................... end of G726_trigb() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_upa1 (short *pk0, short *pk1, short *a1,
                ~~~~~~~~~~~~~~  short *sigpk, short *a1t);

                Description:
                ~~~~~~~~~~~~

                Update a1 coefficient of second order predictor.

            Inputs:   pk0, pk1, a1, sigpk
            Output:   a1t

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_upa1(short* pk0, short* pk1, short* a1, short* sigpk, short* a1t)
        {
            int a11, a1s, ua1;
            int ash;
            short pks;
            int uga1, ula1;

            /* Preamble */
            a11 = *a1 & 65535;

            pks = (short) (*pk0 ^ *pk1);

            /* Gain is +/- (3/256) */
            uga1 = *sigpk == 1 ? 0 : pks == 0 ? 192 : 65344;

            a1s = a11 >> 15;

            /* Leak factor is (1/256) */
            ash = a11 >> 8;
            ula1 = (a1s == 0 ? 65536 - ash : 65536 - (ash + 65280)) & 65535;

            /* Compute update */
            ua1 = (uga1 + ula1) & 65535;
            *a1t = (short) ((a11 + ua1) & 65535);
        }
        /* ....................... end of G726_upa1() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_upa2 (short *pk0, short *pk1, short *pk2,
                ~~~~~~~~~~~~~~  short *a2, short *a1, short *sigpk, short *a2t);

                Description:
                ~~~~~~~~~~~~

                Update a2 coefficient of second order predictor.

            Inputs:   pk0, pk1, pk2, a2, a1, sigpk
            Output:   a2t

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_upa2(short* pk0, short* pk1, short* pk2, short* a2, short* a1, short* sigpk,
                                      short* a2t)
        {
            int uga2a, uga2b, uga2s;
            int a11, a21, fa, fa1;
            short a1s, a2s;
            int ua2;
            int uga2, ula2;
            short pks1, pks2;

            /* Preamble */
            a11 = *a1 & 65535;
            a21 = *a2 & 65535;

            /* 1 bit xors */
            pks1 = (short) (*pk0 ^ *pk1);
            pks2 = (short) (*pk0 ^ *pk2);

            uga2a = pks2 == 0 ? 16384 : 114688;

            a1s = (short) (*a1 >> 15);

            /* Implement f(a1) w/ limiting at +/-(1/2) */
            if (a1s == 0)
            {
                fa1 = a11 <= 8191 ? a11 << 2 : 8191 << 2;
            }
            else
            {
                fa1 = a11 >= 57345 ? (a11 << 2) & 131071 : 24577 << 2;
            }

            /* Attach sign to the result of f(a1) */
            fa = pks1 != 0 ? fa1 : (131072 - fa1) & 131071;

            uga2b = (uga2a + fa) & 131071;
            uga2s = uga2b >> 16;

            uga2 = *sigpk == 1 ? 0 :
                uga2s != 0 ? (uga2b >> 7) + 64512 : uga2b >> 7;

            a2s = (short) (*a2 >> 15);

            ula2 = a2s == 0 ? (65536 - (a21 >> 7)) & 65535 : (65536 - ((a21 >> 7) + 65024)) & 65535;

            /* Compute update */
            ua2 = (uga2 + ula2) & 65535;
            *a2t = (short) ((a21 + ua2) & 65535);
        }
        /* ....................... end of G726_upa2() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_upb (short rate, short *u, short *b, short *dq, short *bp);
                ~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Update for coefficients of sixth order predictor.

            Inputs:    u, b, dq, rate
            Output:    bp

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_upb(short rate, short* u, short* b, short* dq, short* bp)
        {
            short dqmag;
            int bb, bs, ub;
            int ugb, ulb;
            short param;
            short leak;

            /* Preamble */
            bb = *b & 65535;

            unchecked
            {
                dqmag = (short) (*dq & 32767);
                if (rate != 5)
                {
                    leak = 8;
                    param = (short) 65280;
                }
                else
                {
                    leak = 9;
                    param = (short) 65408;
                }
            }

            /* gain is 0 or +/- (1/128) */
            ugb = dqmag == 0 ? 0 : *u == 0 ? 128 : 65408;

            bs = bb >> 15;

            /* Leak factor is (1/256 or 1/512 for 40 kbit/s) */

            ulb = bs == 0 ? (65536 - (bb >> leak)) & 65535 : (65536 - ((bb >> leak) + param)) & 65535;

            /* Compute update */
            ub = (ugb + ulb) & 65535;
            /*  aux = bb + ub;*/

            *bp = (short) ((bb + ub) & 65535);
        }
        /* ....................... end of G726_upb() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_xor (short *dqn, short *dq, short *u);
                ~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                One bit "exclusive or" of sign of difference  signal and sign
                of delayed difference signal.

            Inputs:    dqn, dq
            Output:    u


                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_xor(short* dqn, short* dq, short* u)
        {
            short dqns;
            short dqs;

            dqs = (short) ((*dq >> 15) & 1);

            dqns = (short) (*dqn >> 10);

            *u = (short) (dqs ^ dqns);
        }
        /* ....................... end of G726_xor() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_tone (short *a2p, short *tdp);
                ~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Partial band signal detection.

            Inputs: a2p
            Output: tdp

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_tone(short* a2p, short* tdp)
        {
            int a2p1;

            a2p1 = *a2p & 65535;

            *tdp = a2p1 >= 32768 && a2p1 < 53760 ? (short) 1 : (short) 0;
        }
        /* ....................... end of G726_tone() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_trans (short *td, long *yl, short *dq, short *tr);
                ~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Transition detector.

            Inputs: td, yl, dq
            Output: tr

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_trans(short* td, int* yl, short* dq, short* tr)
        {
            short dqmag;
            int dqthr;
            short ylint;
            int dqmag1;
            short ylfrac;
            int thr1, thr2;

            dqmag = (short) (*dq & 32767);

            ylint = (short) (*yl >> 15);

            ylfrac = (short) ((*yl >> 10) & 31);

            thr1 = (ylfrac + 32) << ylint;

            thr2 = ylint > 9 ? 31744 : thr1;

            dqthr = (thr2 + (thr2 >> 1)) >> 1;

            dqmag1 = dqmag;

            *tr = dqmag1 > dqthr && *td == 1 ? (short) 1 : (short) 0;
        }
        /* ....................... end of G726_trans() ....................... */


        /*
         ----------------------------------------------------------------------

                void G726_compress (short *sr, char *law, short *sp);
                ~~~~~~~~~~~~~~~~~~

                Description:
                ~~~~~~~~~~~~

                Convert from uniform pcm  to either a-law or u-law pcm

                Inputs:   sr, law
                Output:   sp

                Return value:  none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                                <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                                <tdsimao@venus.cpqd.ansp.br>
                24.Jan.00 v2.0  Corrected im calculation that caused incorrect 
                                processing of test vector ri40fa. Corrected
                                code provided by Jayesh Patel <jayesh@dspse.com>. 
                    Verified by <simao.campos@labs.comsat.com>
         ----------------------------------------------------------------------
        */
        private static void G726_compress(short* sr, byte* law, short* sp)
        {
            short imag, iesp, ofst;
            short ofst1 = 0;
            int i;
            int im;
            short _is;
            int srr;

            _is = (short) (*sr >> 15);
            srr = *sr & 65535;

            /* Convert 2-complement to signed magnitude */
            im = _is == 0 ? srr : (65536 - srr) & 32767;

            /* Compress ... */
            if (*law == '1')
            {
                /* Next line added by J.Patel to fix a with test vector ri40fa.o */
                im = *sr == -32768 ? 2 : im; /* *** */

                imag = _is == 0 ? (short) (im >> 1) : (short) ((im + 1) >> 1);

                if (_is != 0)
                {
                    --imag;
                }

                /* Saturation */
                if (imag > 4095)
                {
                    imag = 4095;
                }

                iesp = 7;
                for (i = 1; i <= 7; ++i)
                {
                    imag += imag;
                    if (imag >= 4096)
                    {
                        break;
                    }

                    iesp = (short) (7 - i);
                }

                imag &= 4095;

                imag = (short) (imag >> 8);
                *sp = _is == 0 ? (short) (imag + (iesp << 4)) : (short) (imag + (iesp << 4) + 128);

                /* Sign bit inversion */
                *sp ^= 128;
            }
            else
            {
                imag = (short) im;

                if (imag > 8158)
                {
                    imag = 8158; /* Saturation */
                }

                ++imag;
                iesp = 0;
                ofst = 31;

                if (imag > ofst)
                {
                    for (iesp = 1; iesp <= 8; ++iesp)
                    {
                        ofst1 = ofst;
                        ofst += (short) (1 << (iesp + 5));
                        if (imag <= ofst)
                        {
                            break;
                        }
                    }

                    imag -= (short) (ofst1 + 1);
                }

                imag /= (short) (1 << (iesp + 1));

                *sp = _is == 0 ? (short) (imag + (iesp << 4)) : (short) (imag + (iesp << 4) + 128);

                /* Sign bit inversion */
                *sp ^= 128;
                *sp ^= 127;
            }
        }
        /* ....................... end of G726_compress()....................... */


        /*
         ----------------------------------------------------------------------

                void G726_sync (short rate, short *i, short *sp, short *dlnx,
                ~~~~~~~~~~~~~~  short *dsx, char *law, short *sd);

                Description:
                ~~~~~~~~~~~~

                Re-encode output pcm sample in decoder for  synchronous tandem
                coding.

            Inputs:   i, sp, dlnx, dsx, law, rate
            Output:   sd

                Return value:          none.
                ~~~~~~~~~~~~~

                Prototype:   in g726.h
                ~~~~~~~~~~

                History:
                ~~~~~~~~
                31.Jan.91 v1.0f Release of 1st Fortran version to UGST.
                        <tdsindi@venus.cpqd.ansp.br>
                13.Feb.92 v1.0c 1st version in C translated from Fortran (f2c)
                        <tdsimao@venus.cpqd.ansp.br>

         ----------------------------------------------------------------------
        */
        private static void G726_sync(short rate, short* i, short* sp, short* dlnx, short* dsx, byte* law, short* sd)
        {
            short mask, id = 0, im, _is, ss;

            if (rate == 4)
            {
                _is = (short) (*i >> 3);

                im = _is == 0 ? (short) (*i + 8) : (short) (*i & 7);

                /* Find value of `id' as in Table 17/G.726 */
                if (*dlnx >= 3972)
                {
                    id = 9;
                }
                else if (*dlnx >= 2048)
                {
                    id = 7;
                }
                else if (*dlnx >= 400)
                {
                    id = 15;
                }
                else if (*dlnx >= 349)
                {
                    id = 14;
                }
                else if (*dlnx >= 300)
                {
                    id = 13;
                }
                else if (*dlnx >= 246)
                {
                    id = 12;
                }
                else if (*dlnx >= 178)
                {
                    id = 11;
                }
                else if (*dlnx >= 80)
                {
                    id = 10;
                }
                else
                {
                    id = 9;
                }

                /* Account for the negative part of the table */
                if (*dsx != 0)
                {
                    id = (short) (15 - id);
                }

                if (id == 8)
                {
                    id = 7;
                }
            } /* ............... end of 32 kbit part
                 * ................. */
            else if (rate == 3)
            {
                _is = (short) (*i >> 2);

                im = _is == 0 ? (short) (*i + 4) : (short) (*i & 3);

                /* Find value of `id' as in the Table 18/G.726 */
                if (*dlnx >= 2048)
                {
                    id = 3;
                }
                else if (*dlnx >= 331)
                {
                    id = 7;
                }
                else if (*dlnx >= 218)
                {
                    id = 6;
                }
                else if (*dlnx >= 8)
                {
                    id = 5;
                }
                else if (*dlnx >= 0)
                {
                    id = 3;
                }


                if (*dsx != 0)
                {
                    id = (short) (7 - id);
                }

                if (id == 4)
                {
                    id = 3;
                }
            } /* ............... end of 24 kbit part
                 * ................. */
            else if (rate == 2)
            {
                _is = (short) (*i >> 1);

                im = _is == 0 ? (short) (*i + 2) : (short) (*i & 1);

                /* Find value of `id' as in the Table 19/G.726 */
                if (*dlnx >= 2048)
                {
                    id = 2;
                }
                else if (*dlnx >= 261)
                {
                    id = 3;
                }
                else if (*dlnx >= 0)
                {
                    id = 2;
                }

                if (*dsx != 0)
                {
                    id = (short) (3 - id);
                }
            } /* ............... end of 16 kbit part
                 * ................. */
            else
            {
                _is = (short) (*i >> 4);

                im = _is == 0 ? (short) (*i + 16) : (short) (*i & 15);

                /* Find value of `id' as in the Table 16/G.726 */

                if (*dlnx >= 4080)
                {
                    id = 18;
                }
                else if (*dlnx >= 3974)
                {
                    id = 17;
                }
                else if (*dlnx >= 2048)
                {
                    id = 15;
                }
                else if (*dlnx >= 553)
                {
                    id = 31;
                }
                else if (*dlnx >= 528)
                {
                    id = 30;
                }
                else if (*dlnx >= 502)
                {
                    id = 29;
                }
                else if (*dlnx >= 475)
                {
                    id = 28;
                }
                else if (*dlnx >= 445)
                {
                    id = 27;
                }
                else if (*dlnx >= 413)
                {
                    id = 26;
                }
                else if (*dlnx >= 378)
                {
                    id = 25;
                }
                else if (*dlnx >= 339)
                {
                    id = 24;
                }
                else if (*dlnx >= 298)
                {
                    id = 23;
                }
                else if (*dlnx >= 250)
                {
                    id = 22;
                }
                else if (*dlnx >= 198)
                {
                    id = 21;
                }
                else if (*dlnx >= 139)
                {
                    id = 20;
                }
                else if (*dlnx >= 68)
                {
                    id = 19;
                }
                else if (*dlnx >= 0)
                {
                    id = 18;
                }

                if (*dsx != 0)
                {
                    id = (short) (31 - id);
                }

                if (id == 16)
                {
                    id = 15;
                }
            } /* ............... end of 40 kbit part
                 * ................. */

            /* Choose sd as sp, sp+ or sp- */

            ss = (short) ((*sp & 128) >> 7);
            mask = (short) (*sp & 127);

            if (*law == '1') /* ......... A-law */
            {
                if (id > im && ss == 1 && mask == 0)
                {
                    ss = 0;
                }
                else if (id > im && ss == 1 && mask != 0)
                {
                    mask--;
                }
                else if (id > im && ss == 0 && mask != 127)
                {
                    mask++;
                }
                else if (id < im && ss == 1 && mask != 127)
                {
                    mask++;
                }
                else if (id < im && ss == 0 && mask == 0)
                {
                    ss = 1;
                }
                else if (id < im && ss == 0 && mask != 0)
                {
                    mask--;
                }
            }
            else
            {
                /* ......... u-law */
                if (id > im && ss == 1 && mask == 127)
                {
                    ss = 0;
                    mask--;
                }
                else if (id > im && ss == 1 && mask != 127)
                {
                    mask++;
                }
                else if (id > im && ss == 0 && mask != 0)
                {
                    mask--;
                }
                else if (id < im && ss == 1 && mask != 0)
                {
                    mask--;
                }
                else if (id < im && ss == 0 && mask == 127)
                {
                    ss = 1;
                }
                else if (id < im && ss == 0 && mask != 127)
                {
                    mask++;
                }
            }

            *sd = (short) (mask + (ss << 7));
        }
    }
}