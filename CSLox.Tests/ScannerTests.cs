namespace CSLox.Tests
{
    public class ScannerTests
    {
        [SetUp]
        public void SetUp()
        {
            CSLoxLanguage.HadError = false;
        }

        [TestCaseSource(nameof(ScanningData))]
        public void TestScanning(string source, Token[] expected)
        {
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            CSLoxLanguage.HadError.Should().BeFalse();
            tokens.Should().BeEquivalentTo(expected);
        }

        public static IEnumerable<TestCaseData> ScanningData => new[]
        {
            new TestCaseData(" ", new [] { EofToken(1) }),
            new TestCaseData("\t", new [] { EofToken(1) }),
            new TestCaseData(" \r\n ", new [] { EofToken(2) }),
            new TestCaseData(" \n ", new [] { EofToken(2) }),
            new TestCaseData("// Comment", new [] { EofToken(1) }),
            new TestCaseData("/", new [] { new Token(TokenType.SLASH, "/", null, 1), EofToken(1) }),
            new TestCaseData("(", new [] { new Token(TokenType.LEFT_PAREN, "(", null, 1), EofToken(1) }),
            new TestCaseData(")", new [] { new Token(TokenType.RIGHT_PAREN, ")", null, 1), EofToken(1) }),
            new TestCaseData("{", new [] { new Token(TokenType.LEFT_BRACE, "{", null, 1), EofToken(1) }),
            new TestCaseData("}", new [] { new Token(TokenType.RIGHT_BRACE, "}", null, 1), EofToken(1) }),
            new TestCaseData(",", new [] { new Token(TokenType.COMMA, ",", null, 1), EofToken(1) }),
            new TestCaseData(".", new [] { new Token(TokenType.DOT, ".", null, 1), EofToken(1) }),
            new TestCaseData("-", new [] { new Token(TokenType.MINUS, "-", null, 1), EofToken(1) }),
            new TestCaseData("+", new [] { new Token(TokenType.PLUS, "+", null, 1), EofToken(1) }),
            new TestCaseData(";", new [] { new Token(TokenType.SEMICOLON, ";", null, 1), EofToken(1) }),
            new TestCaseData("*", new [] { new Token(TokenType.STAR, "*", null, 1), EofToken(1) }),
            new TestCaseData("!=", new [] { new Token(TokenType.BANG_EQUAL, "!=", null, 1), EofToken(1) }),
            new TestCaseData("!", new [] { new Token(TokenType.BANG, "!", null, 1), EofToken(1) }),
            new TestCaseData("==", new [] { new Token(TokenType.EQUAL_EQUAL, "==", null, 1), EofToken(1) }),
            new TestCaseData("=", new [] { new Token(TokenType.EQUAL, "=", null, 1), EofToken(1) }),
            new TestCaseData("<=", new [] { new Token(TokenType.LESS_EQUAL, "<=", null, 1), EofToken(1) }),
            new TestCaseData("<", new [] { new Token(TokenType.LESS, "<", null, 1), EofToken(1) }),
            new TestCaseData(">=", new [] { new Token(TokenType.GREATER_EQUAL, ">=", null, 1), EofToken(1) }),
            new TestCaseData(">", new [] { new Token(TokenType.GREATER, ">", null, 1), EofToken(1) }),
            new TestCaseData("\"Hello\"", new [] { new Token(TokenType.STRING, "\"Hello\"", "Hello", 1), EofToken(1) }),
            new TestCaseData("49", new [] { new Token(TokenType.NUMBER, "49", 49d, 1), EofToken(1) }),
            new TestCaseData("49.75", new [] { new Token(TokenType.NUMBER, "49.75", 49.75, 1), EofToken(1) }),
            new TestCaseData("orchid", new [] { new Token(TokenType.IDENTIFIER, "orchid", null, 1), EofToken(1) }),
            new TestCaseData("andeas", new [] { new Token(TokenType.IDENTIFIER, "andeas", null, 1), EofToken(1) }),
            new TestCaseData("and", new [] { new Token(TokenType.AND, "and", null, 1), EofToken(1) }),
            new TestCaseData("class", new [] { new Token(TokenType.CLASS, "class", null, 1), EofToken(1) }),
            new TestCaseData("else", new [] { new Token(TokenType.ELSE, "else", null, 1), EofToken(1) }),
            new TestCaseData("false", new [] { new Token(TokenType.FALSE, "false", null, 1), EofToken(1) }),
            new TestCaseData("for", new [] { new Token(TokenType.FOR, "for", null, 1), EofToken(1) }),
            new TestCaseData("fun", new [] { new Token(TokenType.FUN, "fun", null, 1), EofToken(1) }),
            new TestCaseData("if", new [] { new Token(TokenType.IF, "if", null, 1), EofToken(1) }),
            new TestCaseData("nil", new [] { new Token(TokenType.NIL, "nil", null, 1), EofToken(1) }),
            new TestCaseData("or", new [] { new Token(TokenType.OR, "or", null, 1), EofToken(1) }),
            new TestCaseData("print", new [] { new Token(TokenType.PRINT, "print", null, 1), EofToken(1) }),
            new TestCaseData("return", new [] { new Token(TokenType.RETURN, "return", null, 1), EofToken(1) }),
            new TestCaseData("super", new [] { new Token(TokenType.SUPER, "super", null, 1), EofToken(1) }),
            new TestCaseData("this", new [] { new Token(TokenType.THIS, "this", null, 1), EofToken(1) }),
            new TestCaseData("true", new [] { new Token(TokenType.TRUE, "true", null, 1), EofToken(1) }),
            new TestCaseData("var", new [] { new Token(TokenType.VAR, "var", null, 1), EofToken(1) }),
            new TestCaseData("while", new [] { new Token(TokenType.WHILE, "while", null, 1), EofToken(1) }),
            new TestCaseData("print foo", new [] { new Token(TokenType.PRINT, "print", null, 1), new Token(TokenType.IDENTIFIER, "foo", null, 1), EofToken(1) }),
            new TestCaseData("print \"Hello world\"", new [] { new Token(TokenType.PRINT, "print", null, 1), new Token(TokenType.STRING, "\"Hello world\"", "Hello world", 1), EofToken(1) }),
        };

        static Token EofToken(int line) => new Token(TokenType.EOF, "", null, line);

        [Test]
        public void TestError()
        {
            var scanner = new Scanner("3 ^ 4");
            var tokens = scanner.ScanTokens();
            CSLoxLanguage.HadError.Should().BeTrue();
        }

        [Test]
        public void TestUnterminatedString()
        {
            var scanner = new Scanner("\"Hello");
            var tokens = scanner.ScanTokens();
            CSLoxLanguage.HadError.Should().BeTrue();
        }
    }
}
