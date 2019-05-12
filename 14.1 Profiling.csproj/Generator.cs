using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiling
{
    class Generator
    {
        public static string GenerateDeclarations()
        {
            string result = "";
            foreach (var number in Constants.FieldCounts)
            {
                result += "\nstruct S" + number + "\n{\n";
                for (int i = 0; i < number; i++)
                {
                    result += "byte Value" + i + "; ";
                }
                result += "\n}\n";
                result += "\nclass C" + number + "\n{\n";
                for (int i = 0; i < number; i++)
                {
                    result += "byte Value" + i + "; ";
                }
                result += "\n}\n";
            }
            return result;
        }


        public static string GenerateArrayRunner()
        {
            string result = "";
            result += "public class ArrayRunner : IRunner\n{\n";

            foreach (var number in Constants.FieldCounts)
            {
                result = GetC(result, number);
            }
            result += "\npublic void Call(bool isClass, int size, int count)\n{\n";
            foreach (var number in Constants.FieldCounts)
            {
                result = GetClassStruct(result, number);
            }
            result += "\nthrow new ArgumentException();\n}";
            result += "\n}";
            return result;
        }

        private static string GetClassStruct(string result, int number)
        {
            {
                result += "if (isClass && size == " + number + ")\n{\n" +
                "for (int i = 0; i < count; i++) PC" + number + "();\n" +
                "return;\n}\n" + "if (!isClass && size == " + number + ")\n{\n" +
                "for (int i = 0; i < count; i++) PS" + number + "();\n" +
                "return;\n}\n";
            }

            return result;
        }

        private static string GetC(string result, int number)
        {
            {
                result += "void PC" + number + "()\n{\n" +
                    "var array = new C" + number + "[Constants.ArraySize];\n" +
                    "for (int i = 0; i < Constants.ArraySize; i++) array[i] = new C" +
                    number + "();\n}\n" +
                    "void PS" + number + "()\n{\nvar array = new S" +
                    number + "[Constants.ArraySize];\n}\n";
            }

            return result;
        }

        public static string GenerateCallRunner()
        {
            string result = "";
            result = "public class CallRunner : IRunner\n{\n";
            foreach (var number in Constants.FieldCounts)
            {
                result += "void PC" + number + "(C" + number + " o) " +
                    "{}\nvoid PS" + number + "(S" + number + " o) {}\n";
            }
            result += "public void Call(bool isClass, int size, int count)\n{\n";
            foreach (var number in Constants.FieldCounts)
            {
                result += "if (isClass && size == " + number + ")\n{\nvar o = new C" + number +
            "(); for (int i = 0; i < count; i++) PC" + number + "(o);" +
            "\nreturn;\n}" + "\nif (!isClass && size == " + number + ")\n{\nvar o = new S" + number +
            "(); for (int i = 0; i < count; i++) PS" + number + "(o);" +
            "\nreturn;\n}";
            }
            result += "\nthrow new ArgumentException();\n}}";
            return result;
        }
    }
}