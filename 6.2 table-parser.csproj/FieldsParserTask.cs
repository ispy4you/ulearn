using System.Collections.Generic;
using System.Text;

namespace TableParser
{
    public class FieldsParserTask
    {
        public static List<string> ParseLine(string line)
        {
            List<string> result = new List<string>();
            var i = 0;
            while (i < line.Length)
            {
                var field = Field(line, i);
                if (field.Value == null && field.Length < 2)
                {
                    i++;
                    continue;
                }
                i += field.Length;
                result.Add(field.Value);
            }
            return result;
        }

        public static Token CleanPunctation(string line, int startPos)
        {
            var plainField = new StringBuilder();
            while (startPos < line.Length)
            {
                if (line[startPos] == '\'' || line[startPos] == ' ' || line[startPos] == '"')
                    break;

                plainField.Append(line[startPos]);
                startPos++;
            }
            return new Token(plainField.ToString(), startPos, plainField.Length);
        }

        public static Token FieldInQuoted(string line, int startPos)
        {
            var skip = 0;
            var pos = startPos + 1;
            var fieldInQuoted = new StringBuilder();
            for (; pos < line.Length; pos++)
            {
                if (line[pos] == '\\')
                {
                    skip++;
                    pos++;
                    switch (line[pos])
                    {
                        case '\\':
                        case '\'':
                        case '\"':
                            fieldInQuoted.Append(line[pos]);
                            continue;
                    }
                }
                if (line[pos] == line[startPos])
                    break;
                fieldInQuoted.Append(line[pos]);
            }
            return new Token(fieldInQuoted.ToString(), startPos, fieldInQuoted.Length + 2 + skip);
        }

        public static Token Field(string line, int startPos)
        {
            switch (line[startPos])
            {
                case '"':
                    return FieldInQuoted(line, startPos);
                case '\'':
                    return FieldInQuoted(line, startPos);
                case ' ':
                    return new Token(null, startPos, 1);
            }
            return CleanPunctation(line, startPos);
        }
    }
}