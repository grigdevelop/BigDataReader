using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class SorterProgram
    {
        public static void Run()
        {
            new SorterProgram().Start();
        }



        public void Start()
        {
            List<Line> lines = new List<Line>();

            using (StreamReader sr = new StreamReader(WriterProgram.FPath))
            {
                while (!sr.EndOfStream)
                {
                    lines.Add(new Line(sr.ReadLine()));
                }
            }

            lines.Sort();
            Console.WriteLine(string.Join("\n", lines));
        }


        class Line : IComparable<Line>
        {
            static long _curIndex = 1;

            private string _arrayString;
            private string _numberString;
            private readonly long _index;

            public Line(string lineString)
            {
                _index = _curIndex++;
                var temp = lineString.Split('|');
                _numberString = temp[0];
                _arrayString = temp[1];
            }

            public override string ToString()
            {
                return string.Format("{0} {1}", _arrayString, _numberString);
            }

            public int CompareTo(Line otherLine)
            {
                var dif = otherLine._arrayString.Length - _arrayString.Length;
                if (dif > 0)
                {
                    _arrayString = _arrayString + new string(' ', dif);
                }
                else
                {
                    otherLine._arrayString = otherLine._arrayString + new string(' ', -dif);
                }

                var comp = _arrayString.CompareTo(otherLine._arrayString);

                if (comp != 0) return comp;

                return _numberString.CompareTo(otherLine._numberString);
            }
        }

    }
}
