using UnityEngine;

public struct Fraction
{
    private readonly int num;
    private readonly int den;
    public Fraction(int numerator)
    {
        num = numerator;
        den = 1;
    }

    public Fraction(int numerator, int denominator)
    {
        if (denominator == 0)
        {
            Debug.Log("DivideByZeroException - spawn");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        num = numerator;
        den = denominator;
    }

    public float Float()
    {
        return num / (float)den;
    }

    public static Fraction operator +(Fraction a) => a;
    public static Fraction operator -(Fraction a) => new Fraction(-a.num, a.den);

    public static Fraction operator +(Fraction a, Fraction b)
        => new Fraction(a.num * b.den + b.num * a.den, a.den * b.den);

    public static Fraction operator -(Fraction a, Fraction b)
        => a + (-b);

    public static Fraction operator *(Fraction a, Fraction b)
        => new Fraction(a.num * b.num, a.den * b.den);

    public static Fraction operator *(int a, Fraction b)
        => new Fraction(a * b.num, b.den);

    public static Fraction operator *(Fraction a, int b)
        => new Fraction(b * a.num, a.den);

    public static Fraction operator /(Fraction a, Fraction b)
    {
        if (b.num == 0)
        {
            Debug.Log("DivideByZeroException - divide");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        return new Fraction(a.num * b.den, a.den * b.num);
    }

    public static bool operator <(Fraction a, Fraction b)
    {
        return (a.num * b.den < a.den * b.num);
    }

    public static bool operator >(Fraction a, Fraction b)
    {
        return (a.num * b.den > a.den * b.num);
    }

    public static bool operator <=(Fraction a, Fraction b)
    {
        return (a.num * b.den <= a.den * b.num);
    }

    public static bool operator >=(Fraction a, Fraction b)
    {
        return (a.num * b.den >= a.den * b.num);
    }

    public static bool operator <=(Fraction a, int b)
    {
        return (a.num <= a.den * b);
    }

    public static bool operator >=(Fraction a, int b)
    {
        return (a.num >= a.den * b);
    }

    public static bool operator <=(int a, Fraction b)
    {
        return (a * b.den <= b.num);
    }

    public static bool operator >=(int a, Fraction b)
    {
        return (a * b.den >= b.num);
    }

    public override string ToString() => $"{num} / {den}";
}
