namespace CSLox.Tests
{
    public class TokenTests
    {
        [Test]
        public void TestLexemeToString()
        {
            var token = new Token(TokenType.BANG_EQUAL, "!=", null, 1);
            token.ToString().Should().Be("BANG_EQUAL != ");
        }

        [Test]
        public void TestLiteralToString()
        {
            var token = new Token(TokenType.STRING, "\"Hello\"", "Hello", 1);
            token.ToString().Should().Be("STRING \"Hello\" Hello");
        }
    }
}