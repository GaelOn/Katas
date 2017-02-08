using System;

namespace Kata_Roman_Number_Refactoring
{
    public class RomanNumber
    {
        public static readonly HashSet<string> AuthorizedSubtraction = new HashSet<string>();

        static RomanNumber()
        {
            AuthorizedSubtraction.Add("IV");
            AuthorizedSubtraction.Add("IX");
            AuthorizedSubtraction.Add("XL");
            AuthorizedSubtraction.Add("XC");
            AuthorizedSubtraction.Add("CD");
            AuthorizedSubtraction.Add("CM");
        }

        readonly char[] _internalRomanNumber;
        int? _value;

        public int Value 
        { 
            get
            {
                if (!_value.HasValue)
                {
                    Decode();
                }
                return _value.Value;
            }
        }

        public RomanNumber(string number)
        {
            _internalRomanNumber = number.ToUpper().ToCharArray();
        }

        private void Decode()
        {
            int count = 0;
            char prec = default(char);
            int lastValue = 0;
            int recurrence = 0;
            for (var it = 0; it < _internalRomanNumber.Length; it++)
            {
                count += lastValue;
                int maybeLastValue = 0;
                char curChar = _internalRomanNumber[it];
                if (!TryGetValue(curChar, out maybeLastValue))
                { 
                    throw new ArgumentException($"{curChar} is not an allowed character.");
                }
                if (prec != default(char) &&
                    maybeLastValue > GetValue(prec))
                {
                    var combo = (new StringBuilder()).Append(prec).Append(curChar).ToString();
                    if (!AuthorizedSubtraction.Contains(combo))
                    {
                        throw new ArgumentException($"{combo} is not an allowed combinaison of element.");
                    }
                    recurrence = 1;
                    lastValue += maybeLastValue;
                    lastValue -= GetValue(prec);
                    prec = default(char);
                }
                else if (prec != default(char))
                { 
                    if (prec == curChar)
                    {
                        recurrence++;
                    }
                    else
                    {
                        recurrence = 1;
                    }
                    if (recurrence > 3)
                    {
                        throw new ArgumentException($"The character {prec} have been repeated {recurrence}. A character cannot be repeated more than three times.");
                    }
                    lastValue += GetValue(prec);
                    prec = curChar;
                }
                else
                {
                    recurrence = 1;
                    prec = curChar;
                }
            }
            if (prec != default(char))
            {
                lastValue += GetValue(prec);
            }
            _value = lastValue;
        }

        public int GetValue(char letterAsValue)
        {
            switch (letterAsValue)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    throw new ArgumentException($"{letterAsValue} is not an allowed character.");
            }
        }

        public bool TryGetValue(char letterAsValue, out int letterAsInt)
        {
            switch (letterAsValue)
            {
                case 'I':
                    letterAsInt = 1;
                    return true;
                case 'V':
                    letterAsInt = 5;
                    return true;
                case 'X':
                    letterAsInt = 10;
                    return true;
                case 'L':
                    letterAsInt = 50;
                    return true;
                case 'C':
                    letterAsInt = 100;
                    return true;
                case 'D':
                    letterAsInt = 500;
                    return true;
                case 'M':
                    letterAsInt = 1000;
                    return true;
                default:
                    letterAsInt = 0;
                    return false;
            }
        }
    }
}
