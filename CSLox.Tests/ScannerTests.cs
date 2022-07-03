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
            new TestCaseData(" ", new [] { new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("\t", new [] { new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData(" \r\n ", new [] { new Token(TokenType.EOF, "", null, 2) }),
            new TestCaseData(" \n ", new [] { new Token(TokenType.EOF, "", null, 2) }),
            new TestCaseData("// Comment", new [] { new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("/", new [] { new Token(TokenType.SLASH, "/", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("(", new [] { new Token(TokenType.LEFT_PAREN, "(", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData(")", new [] { new Token(TokenType.RIGHT_PAREN, ")", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("{", new [] { new Token(TokenType.LEFT_BRACE, "{", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("}", new [] { new Token(TokenType.RIGHT_BRACE, "}", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData(",", new [] { new Token(TokenType.COMMA, ",", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData(".", new [] { new Token(TokenType.DOT, ".", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("-", new [] { new Token(TokenType.MINUS, "-", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("+", new [] { new Token(TokenType.PLUS, "+", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData(";", new [] { new Token(TokenType.SEMICOLON, ";", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("*", new [] { new Token(TokenType.STAR, "*", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("!=", new [] { new Token(TokenType.BANG_EQUAL, "!=", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("!", new [] { new Token(TokenType.BANG, "!", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("==", new [] { new Token(TokenType.EQUAL_EQUAL, "==", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("=", new [] { new Token(TokenType.EQUAL, "=", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("<=", new [] { new Token(TokenType.LESS_EQUAL, "<=", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("<", new [] { new Token(TokenType.LESS, "<", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData(">=", new [] { new Token(TokenType.GREATER_EQUAL, ">=", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData(">", new [] { new Token(TokenType.GREATER, ">", null, 1), new Token(TokenType.EOF, "", null, 1) }),
            new TestCaseData("\"Hello\"", new [] { new Token(TokenType.STRING, "\"Hello\"", "Hello", 1), new Token(TokenType.EOF, "", null, 1) }),
        };

        [Test]
        public void TestError()
        {
            var scanner = new Scanner("3 ^ 4");
            var tokens = scanner.ScanTokens();
            CSLoxLanguage.HadError.Should().BeTrue();
        }
    }
}
