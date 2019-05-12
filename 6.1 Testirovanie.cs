public static void RunTests()
{   
    Test("hello world", new[] { "hello", "world" });
    Test("hello  world", new[] { "hello", "world" });
    Test(String.Empty, new string[0]);  
	Test("'x y'",  new[]{"x y"});
	Test("1", new []{ "1" });  
	Test("a \"bcd ef\" 'x y'", new[] { "a", "bcd ef", "x y"});
	Test("a\"ab", new[] {"a","ab"});
	Test("\"a'b'c\"\"d\"", new[] {"a'b'c","d" });
	Test("a\"b\"c", new[] { "a","b","c" });
	Test(@"""\\""", new[] { @"\"});
	Test("\"a ", new[] { "a "});
	Test("", new string[0]);
	Test("'ab\\'", new[] { "ab\'" });
	Test("\"\"", new[] { "" });
	Test("a ", new [] { "a" });
	Test("\"ab\\\"\"", new[] { "ab\"" });
	Test("'\"1\"'", new[] {"\"1\""});
}