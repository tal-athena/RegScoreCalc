using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegScoreCalc.Helpers
{
    public class TwoByTwoStatResult
    {
        public TwoByTwoStatResult()
        {
            val = max_val = min_val = 0;
        }

        public double val { get; set; }

        private double _max_val;
        public double max_val {
            get
            {
                return _max_val;
            }
            set
            {
                if (value > 1) _max_val = 1;
                else _max_val = value;
            }
        }

        private double _min_val;
        public double min_val
        {
            get
            {
                return _min_val;
            }
            set
            {
                if (value < 0) _min_val = 0;
                else _min_val = value;
            }
        }

        public string str_interval
        {
            get
            {
                return String.Format("({0}%, {1}%)", Math.Round(min_val * 100, 2), Math.Round(max_val * 100, 2));
            }
        }

        public string str_value
        {
            get
            {
                return Math.Round(val * 100, 2).ToString() + " %";
            }
        }

        public string str_numvalue
        {
            get
            {
                return Math.Round(val * 100, 2).ToString();
            }
        }
    }

    public class TwoByTwoStat
    {
        const double z = 1.96;

        public TwoByTwoStat(int tp, int fp, int fn, int tn, double manual_prevalence)
        {
            this.tp = tp;
            this.fp = fp;
            this.fn = fn;
            this.tn = tn;
            this.manual_prevalence = manual_prevalence;
        }

        protected double tp = 0;
        protected double fp = 0;
        protected double fn = 0;
        protected double tn = 0;
        protected double manual_prevalence = 0;


        public TwoByTwoStatResult accuracy
        {
            get
            {
                var retVal = new TwoByTwoStatResult();
                var n = tp + tn + fp + fn;
                if (0 == n) return retVal;
                var acc = retVal.val = (tp + tn) / n;
                var s_err = Math.Sqrt(acc * (1 - acc) / n);
                retVal.min_val = acc - z * s_err;
                retVal.max_val = acc + z * s_err;
                return retVal;
            }
        }

        public TwoByTwoStatResult sensitivity
        {
            get
            {
                var retVal = new TwoByTwoStatResult();
                var n = tp + fn;
                if (0 == n) return retVal;
                var sen = retVal.val = tp / n;
                var s_err = Math.Sqrt(sen * (1 - sen) / n);
                retVal.min_val = sen - z * s_err;
                retVal.max_val = sen + z * s_err;
                return retVal;
            }
        }

        public TwoByTwoStatResult specificity
        {
            get
            {
                var retVal = new TwoByTwoStatResult();
                var n = fp + tn;
                if (0 == n) return retVal;
                var spe = retVal.val = tn / n;
                var s_err = Math.Sqrt(spe * (1 - spe) / n);
                retVal.min_val = spe - z * s_err;
                retVal.max_val = spe + z * s_err;
                return retVal;
            }
        }

        public TwoByTwoStatResult positive_predictive_value
        {
            get
            {
                var retVal = new TwoByTwoStatResult();
                var n = tp + fp;
                if (0 == n) return retVal;
                var ppv = retVal.val = ((manual_prevalence > 0)? manual_ppv : (tp / n));
                var s_err = Math.Sqrt(ppv * (1 - ppv) / n);
                retVal.min_val = ppv - z * s_err;
                retVal.max_val = ppv + z * s_err;
                return retVal;
            }
        }

        protected double manual_ppv
        {
            get
            {
                var e = sensitivity.val * manual_prevalence;
                var d = e + (1 - specificity.val) * (1 - manual_prevalence);
                return e / d;
            }
        }

        protected double manual_npv
        {
            get
            {
                var e = specificity.val * (1-manual_prevalence);
                var d = e + (1 - sensitivity.val) * manual_prevalence;
                return e / d;
            }
        }

        public TwoByTwoStatResult negative_predictive_value
        {
            get
            {
                var retVal = new TwoByTwoStatResult();
                var n = fn + tn;
                if (0 == n) return retVal;
                var npv = retVal.val = ((manual_prevalence > 0)? manual_npv : (tn / n));
                var s_err = Math.Sqrt(npv * (1 - npv) / n);
                retVal.min_val = npv - z * s_err;
                retVal.max_val = npv + z * s_err;
                return retVal;
            }
        }

        public TwoByTwoStatResult likelihood_ratio_positive
        {
            get
            {
                var sen = sensitivity;
                var spe = specificity;
                var retVal = new TwoByTwoStatResult();
                if (sen.val == 0 || spe.val == 1) return retVal;
                
                var lrp = retVal.val = sen.val / (1 - spe.val);

                if (tp == 0 || fp == 0 || fp + tn == 0 || tp + fn == 0)
                    return retVal;

                // confidence interval    
                var s_err = Math.Sqrt(1.0/tp - 1.0/(tp + fn) + 1.0/fp - 1.0/(fp + tn));
                retVal.min_val = Math.Exp(Math.Log(lrp) - z * s_err);
                retVal.max_val = Math.Exp(Math.Log(lrp) + z * s_err);

                return retVal;
            }
        }

        public TwoByTwoStatResult likelihood_ratio_negative
        {
            get
            {
                var sen = sensitivity;
                var spe = specificity;
                var retVal = new TwoByTwoStatResult();
                if (spe.val == 0) return retVal;

                var lrn = retVal.val = (1 - sen.val) / spe.val;

                if (fn == 0 || tn == 0 || fp + tn == 0 || tp + fn == 0)
                    return retVal;

                // confidence interval    
                var s_err = Math.Sqrt(1.0 / fn - 1.0 / (tp + fn) + 1.0 / tn - 1.0 / (fp + tn));
                retVal.min_val = Math.Exp(Math.Log(lrn) - z * s_err);
                retVal.max_val = Math.Exp(Math.Log(lrn) + z * s_err);

                return retVal;
            }
        }

        public TwoByTwoStatResult prevalence
        {
            get
            {
                var n = tp + fp + fn + tn;
                var zsq = z * z;
                var retVal = new TwoByTwoStatResult();
                if (n == 0) return retVal;
                var p = retVal.val = ((manual_prevalence > 0)? manual_prevalence : ((tp + fn) / n));
                // Wilson confidence interval
                var ci_low =  ((2*n*p)+zsq-1-(z*Math.Sqrt(zsq-2-(1/n)+4*p*((n*(1-p))+1)))) / (2*(n+zsq));
                var ci_high = ((2*n*p)+zsq+1+(z*Math.Sqrt(zsq+2-(1/n)+4*p*((n*(1-p))-1)))) / (2*(n+zsq));
                retVal.min_val = Math.Min(ci_low, p);
                retVal.max_val = Math.Max(ci_high, p);
                return retVal;
            }
        }

        public TwoByTwoStatResult odds_ratio
        {
            get
            {
                // add standard error 0.5 to all the cells if one of them is zero
                if (tp * fp * fn * tn == 0)
                {
                    tp += 0.5;
                    fp += 0.5;
                    fn += 0.5;
                    tn += 0.5;
                }
                var retVal = new TwoByTwoStatResult();
                var odr = retVal.val = (tp * tn) / (fp * fn);
                var s_err = Math.Sqrt(1.0 / tp + 1.0 / fp + 1.0 / fn + 1.0 / tn);
                retVal.min_val = Math.Exp(Math.Log(odr) - z * s_err);
                retVal.max_val = Math.Exp(Math.Log(odr) + z * s_err);
                return retVal;
            }
        }

        public TwoByTwoStatResult relative_risk
        {
            get
            {
                // add standard error 0.5 to all the cells if one of them is zero
                if (tp * fp * fn * tn == 0)
                {
                    tp += 0.5;
                    fp += 0.5;
                    fn += 0.5;
                    tn += 0.5;
                }
                var retVal = new TwoByTwoStatResult();
                var rr = retVal.val = (tp * (fn + tn)) / (fn * (tp + fp));
                var s_err = Math.Sqrt(1.0 / tp - 1.0 / (tp + fp) + 1.0 / fn - 1.0 / (fn + tn));
                retVal.min_val = Math.Exp(Math.Log(rr) - z * s_err);
                retVal.max_val = Math.Exp(Math.Log(rr) + z * s_err);
                return retVal;
            }
        }

        public TwoByTwoStatResult kappa
        {
            get
            {
                var retVal = new TwoByTwoStatResult();

                if (tp + fn + fp + tn == 0) return retVal;

                // The observed proportionate agreement is:
                var p0 = (tp + tn) / (tp + fn + fp + tn);

                // The expected probability that both would say yes at random is:
                var p_yes1 = (tp + fn) / (tp + fn + fp + tn);
                var p_yes2 = (tp + fp) / (tp + fn + fp + tn);
                var p_yes = p_yes1 * p_yes2;

                // The expected probability that both would say no at random is:
                var p_no1 = (fp + tn) / (tp + fn + fp + tn);
                var p_no2 = (fn + tn) / (tp + fn + fp + tn);
                var p_no = p_no1 * p_no2;

                // Overall random agreement probability is the probability that they agreed on either Yes or No, i.e.:
                var pe = p_yes + p_no;

                // So now applying our formula for Cohen's Kappa we get:
                if (1-pe == 0) return retVal;
                var kappa = (p0 - pe) / (1 - pe);

                retVal.val = kappa;

                // PMC3900052
                var n = tp + fn + fp + tn;
                var s_err = Math.Sqrt((p0 * (1 - p0)) / (n * Math.Pow((1-pe), 2)));
               
                retVal.min_val = kappa - z * s_err;
                retVal.max_val = kappa + z * s_err;
                return retVal;
            }
        }
    }

    public static class Count95ConfidenceInterval
    {

        public static string CountSensitivity(double B7, double C11)
        {
            double min = (((2 * B7 * C11) + (1.96 * 1.96) - 1) - (1.96 * Math.Sqrt((1.96 * 1.96) - (2 + (1 / B7)) + (4 * C11 * ((B7 * (1 - C11)) + 1))))) / (2 * (B7 + (1.96 * 1.96)));
            double max = (((2 * B7 * C11) + (1.96 * 1.96) + 1) + 1.96 * Math.Sqrt((1.96 * 1.96) + (2 - (1 / B7)) + (4 * C11 * ((B7 * (1 - C11) - 1))))) / (2 * (B7 + (1.96 * 1.96)));

            return String.Format("({0}%, {1}%)", Math.Round(min * 100, 2), Math.Round(max * 100, 2));
        }



        public static string CountPosPredVal(double D5, double C13)
        {
            double min = (((2 * D5 * C13) + (1.96 * 1.96) - 1) - (1.96 * Math.Sqrt((1.96 * 1.96) - (2 + (1 / D5)) + (4 * C13 * ((D5 * (1 - C13)) + 1))))) / (2 * (D5 + (1.96 * 1.96)));
            double max = (((2 * D5 * C13) + (1.96 * 1.96) + 1) + 1.96 * Math.Sqrt((1.96 * 1.96) + (2 - (1 / D5)) + (4 * C13 * ((D5 * (1 - C13) - 1))))) / (2 * (D5 + (1.96 * 1.96)));

            return String.Format("({0}%, {1}%)", Math.Round(min * 100, 2), Math.Round(max * 100, 2));
        }

        public static string CountSpecificity(double C7, double C15)
        {
            double min = (((2 * C7 * C15) + (1.96 * 1.96) - 1) - (1.96 * Math.Sqrt((1.96 * 1.96) - (2 + (1 / C7)) + (4 * C15 * ((C7 * (1 - C15)) + 1))))) / (2 * (C7 + (1.96 * 1.96)));
            double max = (((2 * C7 * C15) + (1.96 * 1.96) + 1) + 1.96 * Math.Sqrt((1.96 * 1.96) + (2 - (1 / C7)) + (4 * C15 * ((C7 * (1 - C15) - 1))))) / (2 * (C7 + (1.96 * 1.96)));

            return String.Format("({0}%, {1}%)", Math.Round(min * 100, 2), Math.Round(max * 100, 2));
        }

        public static string CountNegPredVal(double D6, double C17)
        {
            double min = (((2 * D6 * C17) + (1.96 * 1.96) - 1) - (1.96 * Math.Sqrt((1.96 * 1.96) - (2 + (1 / D6)) + (4 * C17 * ((D6 * (1 - C17)) + 1))))) / (2 * (D6 + (1.96 * 1.96)));
            double max = (((2 * D6 * C17) + (1.96 * 1.96) + 1) + 1.96 * Math.Sqrt((1.96 * 1.96) + (2 - (1 / D6)) + (4 * C17 * ((D6 * (1 - C17) - 1))))) / (2 * (D6 + (1.96 * 1.96)));

            return String.Format("({0}%, {1}%)", Math.Round(min * 100, 2), Math.Round(max * 100, 2));
        }

        public static string CountPrevalence(double D7, double C19)
        {
            double min = (((2 * D7 * C19) + (1.96 * 1.96) - 1) - (1.96 * Math.Sqrt((1.96 * 1.96) - (2 + (1 / D7)) + (4 * C19 * ((D7 * (1 - C19)) + 1))))) / (2 * (D7 + (1.96 * 1.96)));
            double max = (((2 * D7 * C19) + (1.96 * 1.96) + 1) + 1.96 * Math.Sqrt((1.96 * 1.96) + (2 - (1 / D7)) + (4 * C19 * ((D7 * (1 - C19) - 1))))) / (2 * (D7 + (1.96 * 1.96)));

            return String.Format("({0}%, {1}%)", Math.Round(min * 100, 2), Math.Round(max * 100, 2));
        }
    }
}
