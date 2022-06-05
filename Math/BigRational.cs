using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
[Serializable]
public struct BigRational : IComparable, IComparable<BigRational>, IEquatable<BigRational>
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct DoubleUlong
    {
        [FieldOffset(0)] public double dbl;
        [FieldOffset(0)] public ulong uu;
    }
    /// <summary>
    ///     Change here if more then 2048 bits are specified
    /// </summary>
    private const float DecimalMaxScale = 2048f / 64f * 20f;
    private static readonly BigInteger DecimalPrecision = BigInteger.Pow(10, (int)DecimalMaxScale);
    private const int DoubleMaxScale = 308;
    public static BigRational E = GetE(MaxFactorials);
    private static readonly BigInteger DoublePrecision = BigInteger.Pow(10, DoubleMaxScale);
    private static readonly BigInteger DoubleMaxValue = (BigInteger)double.MaxValue;
    private static readonly BigInteger DoubleMinValue = (BigInteger)double.MinValue;
    private static BigRational[] Factorials;
    static BigRational()
    {
    }
    [StructLayout(LayoutKind.Explicit)]
    internal struct DecimalUInt32
    {
        [FieldOffset(0)] public decimal dec;
        [FieldOffset(0)] public int flags;
    }
    private const int DecimalScaleMask = 0x00FF0000;
    private const int DecimalSignMask = unchecked((int)0x80000000);
    private const int MaxFactorials = 100;
    private static readonly BigInteger DecimalMaxValue = (BigInteger)decimal.MaxValue;
    private static readonly BigInteger DecimalMinValue = (BigInteger)decimal.MinValue;
    private const string Solidus = @"/";
    public static BigRational Zero
    {
        get;
    } = new BigRational(BigInteger.Zero);
    public static BigRational One
    {
        get;
    } = new BigRational(BigInteger.One);
    public static BigRational MinusOne
    {
        get;
    } = new BigRational(BigInteger.MinusOne);
    public int Sign => Numerator.Sign;
    public BigInteger Numerator
    {
        get;
        private set;
    }
    public BigInteger Denominator
    {
        get;
        private set;
    }
    public BigInteger WholePart => BigInteger.Divide(Numerator, Denominator);
    public bool IsFractionalPart
    {
        get
        {
            var fp = FractionPart;
            return fp.Numerator != 0 || fp.Denominator != 1;
        }
    }
    public BigInteger GetUnscaledAsDecimal => Numerator * DecimalPrecision / Denominator;
    public BigInteger Remainder => Numerator % Denominator;
    public int DecimalPlaces
    {
        get
        {
            var a = GetUnscaledAsDecimal;
            var dPlaces = 0;
            if (a.Sign == 0)
                return 1;
            if (a.Sign < 0)
                try {
                    a = -a;
                }
                catch (Exception ex) {
                    return 0;
                }
            var biRadix = new BigInteger(10);
            while (a > 0)
                try {
                    a /= biRadix;
                    dPlaces++;
                }
                catch (Exception ex) {
                    break;
                }
            return dPlaces;
        }
    }


    public BigRational FractionPart
    {
        get
        {
            var rem = BigInteger.Remainder(Numerator, Denominator);
            return new BigRational(rem, Denominator);
        }
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (!(obj is BigRational))
            return false;
        return Equals((BigRational)obj);
    }
    public override int GetHashCode()
    {
        return (Numerator / Denominator).GetHashCode();
    }
    int IComparable.CompareTo(object obj)
    {
        if (obj == null)
            return 1;
        if (!(obj is BigRational))
            throw new ArgumentException();
        return Compare(this, (BigRational)obj);
    }
    public int CompareTo(BigRational other)
    {
        return Compare(this, other);
    }
    public bool Equals(BigRational other)
    {
        if (Denominator == other.Denominator)
            return Numerator == other.Numerator;
        return Numerator * other.Denominator == Denominator * other.Numerator;
    }
    public BigRational(BigInteger numerator)
    {
        Numerator = numerator;
        Denominator = BigInteger.One;
    }
    public BigRational(string n, string d)
    {
        Numerator = BigInteger.Parse(n);
        Denominator = BigInteger.Parse(d);
    }

    public BigRational(double value) : this((decimal)value)
    {
    }

    public BigRational(decimal value)
    {
        var bits = decimal.GetBits(value);
        if (bits == null || bits.Length != 4 ||
            (bits[3] & ~(DecimalSignMask | DecimalScaleMask)) != 0 ||
            (bits[3] & DecimalScaleMask) > 28 << 16)
            throw new ArgumentException();
        if (value == decimal.Zero) {
            this = Zero;
            return;
        }
        var ul = ((ulong)(uint)bits[2] << 32) | (uint)bits[1];
        Numerator = (new BigInteger(ul) << 32) | (uint)bits[0];
        var isNegative = (bits[3] & DecimalSignMask) != 0;
        if (isNegative)
            Numerator = BigInteger.Negate(Numerator);
        var scale = (bits[3] & DecimalScaleMask) >> 16;
        Denominator = BigInteger.Pow(10, scale);
        Simplify();
    }
    public BigRational(BigInteger numerator, BigInteger denominator)
    {
        if (denominator.Sign == 0)
            throw new DivideByZeroException();
        if (numerator.Sign == 0) {
            Numerator = BigInteger.Zero;
            Denominator = BigInteger.One;
        }
        else if (denominator.Sign < 0) {
            Numerator = BigInteger.Negate(numerator);
            Denominator = BigInteger.Negate(denominator);
        }
        else {
            Numerator = numerator;
            Denominator = denominator;
        }
        Simplify();
    }
    public BigRational(BigInteger whole, BigInteger numerator, BigInteger denominator)
    {
        if (denominator.Sign == 0)
            throw new DivideByZeroException();
        if (numerator.Sign == 0 && whole.Sign == 0) {
            Numerator = BigInteger.Zero;
            Denominator = BigInteger.One;
        }
        else if (denominator.Sign < 0) {
            Denominator = BigInteger.Negate(denominator);
            Numerator = BigInteger.Negate(whole) * Denominator + BigInteger.Negate(numerator);
        }
        else {
            Denominator = denominator;
            Numerator = whole * denominator + numerator;
        }
        Simplify();
    }
    public static BigRational Abs(BigRational r)
    {
        return r.Numerator.Sign < 0
            ? new BigRational(BigInteger.Abs(r.Numerator), r.Denominator)
            : r;
    }
    public static BigRational Negate(BigRational r)
    {
        return new BigRational(BigInteger.Negate(r.Numerator), r.Denominator);
    }
    public static BigRational Invert(BigRational r)
    {
        return new BigRational(r.Denominator, r.Numerator);
    }
    public static BigRational Add(BigRational x, BigRational y)
    {
        return x + y;
    }
    public static BigRational Subtract(BigRational x, BigRational y)
    {
        return x - y;
    }
    public static BigRational Multiply(BigRational x, BigRational y)
    {
        return x * y;
    }
    public static BigRational Divide(BigRational dividend, BigRational divisor)
    {
        return dividend / divisor;
    }
    public static BigRational DivRem(BigRational dividend,
        BigRational divisor,
        out BigRational remainder)
    {
        var ad = dividend.Numerator * divisor.Denominator;
        var bc = dividend.Denominator * divisor.Numerator;
        var bd = dividend.Denominator * divisor.Denominator;
        remainder = new BigRational(ad % bc, bd);
        return new BigRational(ad, bc);
    }
    public static BigInteger LeastCommonDenominator(BigRational x, BigRational y)
    {
        return x.Denominator * y.Denominator /
               BigInteger.GreatestCommonDivisor(x.Denominator, y.Denominator);
    }
    public static int Compare(BigRational r1, BigRational r2)
    {
        return BigInteger.Compare(r1.Numerator * r2.Denominator, r2.Numerator * r1.Denominator);
    }
    public static bool operator ==(BigRational x, BigRational y)
    {
        return Compare(x, y) == 0;
    }
    public static bool operator !=(BigRational x, BigRational y)
    {
        return Compare(x, y) != 0;
    }
    public static bool operator <(BigRational x, BigRational y)
    {
        return Compare(x, y) < 0;
    }
    public static bool operator <=(BigRational x, BigRational y)
    {
        return Compare(x, y) <= 0;
    }
    public static bool operator >(BigRational x, BigRational y)
    {
        return Compare(x, y) > 0;
    }
    public static bool operator >=(BigRational x, BigRational y)
    {
        return Compare(x, y) >= 0;
    }
    public static BigRational operator +(BigRational r)
    {
        return r;
    }
    public static BigRational operator -(BigRational r)
    {
        return new BigRational(-r.Numerator, r.Denominator);
    }
    public static BigRational operator ++(BigRational r)
    {
        return r + One;
    }
    public static BigRational operator --(BigRational r)
    {
        return r - One;
    }
    public static BigRational operator +(BigRational r1, BigRational r2)
    {
        return new BigRational(r1.Numerator * r2.Denominator + r1.Denominator * r2.Numerator,
            r1.Denominator * r2.Denominator);
    }
    public static BigRational operator -(BigRational r1, BigRational r2)
    {
        return new BigRational(r1.Numerator * r2.Denominator - r1.Denominator * r2.Numerator,
            r1.Denominator * r2.Denominator);
    }
    public static BigRational operator *(BigRational r1, BigRational r2)
    {
        return new BigRational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);
    }
    public static BigRational operator /(BigRational r1, BigRational r2)
    {
        return new BigRational(r1.Numerator * r2.Denominator, r1.Denominator * r2.Numerator);
    }
    public static BigRational operator %(BigRational r1, BigRational r2)
    {
        return new BigRational(r1.Numerator * r2.Denominator % (r1.Denominator * r2.Numerator),
            r1.Denominator * r2.Denominator);
    }
    public static explicit operator sbyte(BigRational value)
    {
        return (sbyte)BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator ushort(BigRational value)
    {
        return (ushort)BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator uint(BigRational value)
    {
        return (uint)BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator ulong(BigRational value)
    {
        return (ulong)BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator byte(BigRational value)
    {
        return (byte)BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator short(BigRational value)
    {
        return (short)BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator int(BigRational value)
    {
        return (int)BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator long(BigRational value)
    {
        return (long)BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator BigInteger(BigRational value)
    {
        return BigInteger.Divide(value.Numerator, value.Denominator);
    }
    public static explicit operator float(BigRational value)
    {
        return (float)(double)value;
    }
    public static explicit operator double(BigRational value)
    {
        if (SafeCastToDouble(value.Numerator) && SafeCastToDouble(value.Denominator))
            return (double)value.Numerator / (double)value.Denominator;
        var denormalized = value.Numerator * DoublePrecision / value.Denominator;
        if (denormalized.IsZero)
            return value.Sign < 0
                ? BitConverter.Int64BitsToDouble(unchecked((long)0x8000000000000000))
                : 0d;
        double result = 0;
        var isDouble = false;
        var scale = DoubleMaxScale;
        while (scale > 0) {
            if (!isDouble)
                if (SafeCastToDouble(denormalized)) {
                    result = (double)denormalized;
                    isDouble = true;
                }
                else {
                    denormalized = denormalized / 10;
                }
            result = result / 10;
            scale--;
        }
        if (!isDouble)
            return value.Sign < 0 ? double.NegativeInfinity : double.PositiveInfinity;
        return result;
    }

    public static explicit operator decimal(BigRational value)
    {
        if (SafeCastToDecimal(value.Numerator) && SafeCastToDecimal(value.Denominator))
            return (decimal)value.Numerator / (decimal)value.Denominator;
        var denormalized = value.Numerator * DecimalPrecision / value.Denominator;
        if (denormalized.IsZero)
            return decimal.Zero;
        for (var scale = (int)DecimalMaxScale; scale >= 0; scale--)
            if (!SafeCastToDecimal(denormalized)) {
                denormalized /= 10;
            }
            else {
                var dec = new DecimalUInt32();
                dec.dec = (decimal)denormalized;
                dec.flags = (dec.flags & ~DecimalScaleMask) | (scale << 16);
                return dec.dec;
            }
        throw new OverflowException();
    }
    public static implicit operator BigRational(sbyte value)
    {
        return new BigRational((BigInteger)value);
    }
    public static implicit operator BigRational(ushort value)
    {
        return new BigRational((BigInteger)value);
    }
    public static implicit operator BigRational(uint value)
    {
        return new BigRational((BigInteger)value);
    }
    public static implicit operator BigRational(ulong value)
    {
        return new BigRational((BigInteger)value);
    }
    public static implicit operator BigRational(byte value)
    {
        return new BigRational((BigInteger)value);
    }
    public static implicit operator BigRational(short value)
    {
        return new BigRational((BigInteger)value);
    }
    public static implicit operator BigRational(int value)
    {
        return new BigRational((BigInteger)value);
    }
    public static implicit operator BigRational(long value)
    {
        return new BigRational((BigInteger)value);
    }
    public static implicit operator BigRational(BigInteger value)
    {
        return new BigRational(value);
    }

    public static implicit operator BigRational(float value)
    {
        return new BigRational(value);
    }
    public static implicit operator BigRational(double value)
    {
        return new BigRational(value);
    }
    public static implicit operator BigRational(decimal value)
    {
        return new BigRational(value);
    }

    private void Simplify()
    {
        if (Numerator == BigInteger.Zero)
            Denominator = BigInteger.One;
        var gcd = BigInteger.GreatestCommonDivisor(Numerator, Denominator);
        if (gcd > BigInteger.One) {
            Numerator = Numerator / gcd;
            Denominator = Denominator / gcd;
        }
    }
    private static bool SafeCastToDouble(BigInteger value)
    {
        return DoubleMinValue <= value && value <= DoubleMaxValue;
    }
    private static bool SafeCastToDecimal(BigInteger value)
    {
        return DecimalMinValue <= value && value <= DecimalMaxValue;
    }
    private static void SplitDoubleIntoParts(double dbl,
        out int sign,
        out int exp,
        out ulong man,
        out bool isFinite)
    {
        DoubleUlong du;
        du.uu = 0;
        du.dbl = dbl;
        sign = 1 - ((int)(du.uu >> 62) & 2);
        man = du.uu & 0x000FFFFFFFFFFFFF;
        exp = (int)(du.uu >> 52) & 0x7FF;
        if (exp == 0) {
            isFinite = true;
            if (man != 0)
                exp = -1074;
        }
        else if (exp == 0x7FF) {
            isFinite = false;
            exp = int.MaxValue;
        }
        else {
            isFinite = true;
            man |= 0x0010000000000000;
            exp -= 1075;
        }
    }
    public static double GetDoubleFromParts(int sign, int exp, ulong man)
    {
        DoubleUlong du;
        du.dbl = 0;
        if (man == 0) {
            du.uu = 0;
        }
        else {
            var cbitShift = CbitHighZero(man) - 11;
            if (cbitShift < 0)
                man >>= -cbitShift;
            else
                man <<= cbitShift;
            exp += 1075;
            if (exp >= 0x7FF) {
                du.uu = 0x7FF0000000000000;
            }
            else if (exp <= 0) {
                exp--;
                if (exp < -52)
                    du.uu = 0;
                else
                    du.uu = man >> -exp;
            }
            else {
                du.uu = (man & 0x000FFFFFFFFFFFFF) | ((ulong)exp << 52);
            }
        }
        if (sign < 0)
            du.uu |= 0x8000000000000000;
        return du.dbl;
    }
    private static int CbitHighZero(ulong uu)
    {
        if ((uu & 0xFFFFFFFF00000000) == 0)
            return 32 + CbitHighZero((uint)uu);
        return CbitHighZero((uint)(uu >> 32));
    }
    private static int CbitHighZero(uint u)
    {
        if (u == 0)
            return 32;
        var cbit = 0;
        if ((u & 0xFFFF0000) == 0) {
            cbit += 16;
            u <<= 16;
        }
        if ((u & 0xFF000000) == 0) {
            cbit += 8;
            u <<= 8;
        }
        if ((u & 0xF0000000) == 0) {
            cbit += 4;
            u <<= 4;
        }
        if ((u & 0xC0000000) == 0) {
            cbit += 2;
            u <<= 2;
        }
        if ((u & 0x80000000) == 0)
            cbit += 1;
        return cbit;
    }
    private static (BigRational High, BigRational Low) SqrtLimits(BigInteger number)
    {
        if (number == BigInteger.Zero) return (0, 0);
        var high = number >> 1;
        var low = BigInteger.Zero;
        while (high > low + 1) {
            var n = (high + low) >> 1;
            var p = n * n;
            if (number < p)
                high = n;
            else if (number > p)
                low = n;
            else
                break;
        }
        return (high, low);
    }
    public static BigRational Sqrt(BigRational value)
    {
        if (value == 0) return 0;
        var hl = SqrtLimits(value.WholePart);
        BigRational n = 0, p = 0;
        if (hl.High == 0 && hl.Low == 0)
            return 0;
        var high = hl.High;
        var low = hl.Low;
        var d = DecimalPrecision;
        var pp = 1 / (BigRational)d;
        while (high > low + pp) {
            n = (high + low) / 2;
            p = n * n;
            if (value < p)
                high = n;
            else if (value > p)
                low = n;
            else
                break;
        }
        var r = value == p ? n : low;
        return r;
    }
    public BigRational Sqrt()
    {
        return Sqrt(this);
    }
    public static BigRational ArcTangent(BigRational v, int n)
    {
        var retVal = v;
        for (var i = 1; i < n; i++) {
            var powRat = Pow(v, 2 * i + 1);
            retVal += new BigRational(powRat.Numerator * (BigInteger)Math.Pow(-1d, i),
                (2 * i + 1) * powRat.Denominator);
        }
        return retVal;
    }
    public static BigRational Reciprocal(BigRational v)
    {
        return new BigRational(v.Denominator, v.Numerator);
    }
    public static BigRational Round(BigRational number, int decimalPlaces)
    {
        BigRational power = BigInteger.Pow(10, decimalPlaces);
        number *= power;
        return number >= 0 ? (BigInteger)(number + 0.5) / power : (BigInteger)(number - 0.5) / power;
    }
    public void Round(int decimalPlaces)
    {
        var number = this;
        BigRational power = BigInteger.Pow(10, decimalPlaces);
        number *= power;
        var n = number >= 0 ? (BigInteger)(number + 0.5) / power : (BigInteger)(number - 0.5) / power;
        Numerator = n.Numerator;
        Denominator = n.Denominator;
    }
    public static BigRational Pow(BigRational v, int e)
    {
        if (e < 1) throw new ArgumentException("Powers must be greater than or equal to one.");
        var retVal = new BigRational(v.Numerator, v.Denominator);
        for (var i = 1; i < e; i++) {
            retVal.Numerator *= v.Numerator;
            retVal.Denominator *= v.Denominator;
        }
        return retVal;
    }
    public static BigRational Min(BigRational r, BigRational l)
    {
        return l < r ? l : r;
    }
    public static BigRational Max(BigRational r, BigRational l)
    {
        return l > r ? l : r;
    }


    private static BigRational Factorial(BigRational x)
    {
        BigRational r = 1;
        BigRational c = 1;
        while (c <= x) {
            r *= c;
            c++;
        }
        return r;
    }
    public static BigRational Exp(BigRational x)
    {
        BigRational r = 0;
        BigRational r1 = 0;
        var k = 0;
        while (true) {
            r += Pow(x, k) / Factorial(k);
            if (r == r1)
                break;
            r1 = r;
            k++;
        }
        return r;
    }
    public static BigRational Sine(BigRational ar, int n)
    {
        if (Factorials == null) {
            Factorials = new BigRational[MaxFactorials];
            for (var i = 0; i < MaxFactorials; i++)
                Factorials[i] = new BigRational();
            for (var i = 1; i < MaxFactorials + 1; i++)
                Factorials[i - 1] = Factorial(i);
        }
        var sin = ar;
        for (var i = 1; i <= n; i++) {
            var trm = Pow(ar, i * 2 + 1);
            trm /= Factorials[i * 2];
            if ((i & 1) == 1)
                sin -= trm;
            else
                sin += trm;
        }
        return sin;
    }
    public static BigRational Atan(BigRational ar, int n)
    {
        var atan = ar;
        for (var i = 1; i <= n; i++) {
            var trm = Pow(ar, i * 2 + 1);
            trm /= i * 2;
            if ((i & 1) == 1)
                atan -= trm;
            else
                atan += trm;
        }
        return atan;
    }
    public static BigRational Cosine(BigRational ar, int n)
    {
        if (Factorials == null) {
            Factorials = new BigRational[MaxFactorials];
            for (var i = 0; i < MaxFactorials; i++)
                Factorials[i] = new BigRational();
            for (var i = 1; i < MaxFactorials + 1; i++)
                Factorials[i - 1] = Factorial(i);
        }
        BigRational cos = 1.0;
        for (var i = 1; i <= n; i++) {
            var trm = Pow(ar, i * 2);
            trm /= Factorials[i * 2 - 1];
            if ((i & 1) == 1)
                cos -= trm;
            else
                cos += trm;
        }
        return cos;
    }
    public static BigRational Tangent(BigRational ar, int n)
    {
        return Sine(ar, n) / Cosine(ar, n);
    }
    public static BigRational CoTangent(BigRational ar, int n)
    {
        return Cosine(ar, n) / Sine(ar, n);
    }
    public static BigRational Secant(BigRational ar, int n)
    {
        return 1.0 / Cosine(ar, n);
    }
    public static BigRational CoSecant(BigRational ar, int n)
    {
        return 1.0 / Sine(ar, n);
    }
    private static BigRational GetE(int n)
    {
        BigRational e = 1.0;
        var c = n;
        while (c > 0) {
            BigRational f = 0;
            if (c == 1) {
                f = 1;
            }
            else {
                var i = c - 1;
                f = c;
                while (i > 0) {
                    f *= i;
                    i--;
                }
            }
            c--;
            e += 1.0 / f;
        }
        return e;
    }
    public static BigRational NthRoot(BigRational value, int nth)
    {
        BigRational lx;
        var a = value;
        var n = nth;
        BigRational s = 1.0;
        do {
            var t = s;
            lx = a / Pow(s, n - 1);
            var r = (n - 1) * s;
            s = (lx + r) / n;
        } while (lx != s);
        return s;
    }
    public static BigRational LogN(BigRational value)
    {
        BigRational a;
        var p = value;
        BigRational n = 0.0;
        while (p >= E) {
            p /= E;
            n++;
        }
        n += p / E;
        p = value;
        do {
            a = n;
            var lx = p / Exp(n - 1.0);
            var r = (n - 1.0) * E;
            n = (lx + r) / E;
        } while (n != a);
        return n;
    }
    public static BigRational Log(BigRational n, int b)
    {
        return LogN(n) / LogN(b);
    }
    private static int ConversionIterations(BigRational v)
    {
        return (int)((DecimalMaxScale + 1) / (2 * Math.Log10((double)Reciprocal(v))));
    }
    public static BigRational GetPI()
    {
        var oneFifth = new BigRational(1, 5);
        var oneTwoThirtyNine = new BigRational(1, 239);
        var arcTanOneFifth = ArcTangent(oneFifth, ConversionIterations(oneFifth));
        var arcTanOneTwoThirtyNine =
            ArcTangent(oneTwoThirtyNine, ConversionIterations(oneTwoThirtyNine));
        return arcTanOneFifth * 16 - arcTanOneTwoThirtyNine * 4;
    }
    public override string ToString()
    {
        var ret = new StringBuilder();
        ret.Append(Numerator.ToString("R", CultureInfo.InvariantCulture));
        ret.Append(Solidus);
        ret.Append(Denominator.ToString("R", CultureInfo.InvariantCulture));
        return ret.ToString();
    }
}
