namespace CSLox;

public sealed class Scanner
{
    readonly string _source;
    readonly List<Token> _tokens = new List<Token>();
    int _start = 0;
    int _current = 0;
    int _line = 1;

    private static readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
    {
        { "and",    TokenType.AND },
        { "class",  TokenType.CLASS },
        { "else",   TokenType.ELSE },
        { "false",  TokenType.FALSE },
        { "for",    TokenType.FOR },
        { "fun",    TokenType.FUN },
        { "if",     TokenType.IF },
        { "nil",    TokenType.NIL },
        { "or",     TokenType.OR },
        { "print",  TokenType.PRINT },
        { "return", TokenType.RETURN },
        { "super",  TokenType.SUPER },
        { "this",   TokenType.THIS },
        { "true",   TokenType.TRUE },
        { "var",    TokenType.VAR },
        { "while",  TokenType.WHILE },
    };

    public Scanner(string source)
    {
        _source = source;
    }

    public List<Token> ScanTokens()
    {
        while (!IsAtEnd)
        {
            _start = _current;
            ScanToken();
        }
        _tokens.Add(new Token(TokenType.EOF, "", null, _line));
        return _tokens;
    }

    void ScanToken()
    {
        char c = Advance();
        switch (c)
        {
            case ' ':
            case '\t':
            case '\r':
                // Ignore whitespace
                break;
            case '\n':
                _line++;
                break;
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                if (Match('/'))
                {
                    // Comments go to the end of the line
                    while (Peek() != '\n' && !IsAtEnd) Advance();
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            case '"':
                ScanString();
                break;
            default:
                if (IsDigit(c))
                {
                    ScanNumber();
                }
                else if (IsAlphaNumeric(c))
                {
                    ScanIdentifier();
                }
                else
                {
                    CSLoxLanguage.Error(_line, $"Unexpected character {c}");
                }
                break;
        }
    }

    char Advance() => _source[_current++];

    bool Match(char expected)
    {
        if (IsAtEnd) return false;
        if (_source[_current] == expected)
        {
            _current++;
            return true;
        }
        return false;
    }

    char Peek() => IsAtEnd ? '\0' : _source[_current];

    char PeekNext() => _current + 1 >= _source.Length ? '\0' : _source[_current + 1];

    void ScanString()
    {
        while (Peek() != '"' && !IsAtEnd)
        {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd)
        {
            CSLoxLanguage.Error(_line, "Untermimated string");
            return;
        }

        // Consume the final "
        Advance();

        string value = _source.Substring(_start + 1, _current - _start - 2);
        AddToken(TokenType.STRING, value);
    }

    void ScanNumber()
    {
        // Pull off the whole numbers
        while (IsDigit(Peek()))
            Advance();

        // Decimal place?
        if(Peek() == '.' && Char.IsDigit(PeekNext()))
        {
            Advance();

            while (IsDigit(Peek()))
                Advance();
        }

        AddToken(TokenType.NUMBER, Double.Parse(_source.Substring(_start, _current - _start)));
    }

    void ScanIdentifier()
    {
        while (IsAlphaNumeric(Peek()))
            Advance();

        string value = _source.Substring(_start, _current - _start);
        if(_keywords.TryGetValue(value, out TokenType tokenType))
        {
            AddToken(tokenType);
            return;
        }

        AddToken(TokenType.IDENTIFIER);
    }

    static bool IsDigit(char c) => 
        c >= '0' && c <= '9';

    static bool IsAlpha(char c) => 
        (c >= 'a' && c <= 'z') ||
        (c >= 'A' && c <= 'Z') ||
         c == '_';

    static bool IsAlphaNumeric(char c) =>
        IsAlpha(c) || IsDigit(c);

    void AddToken(TokenType type) =>
        AddToken(type, null);

    void AddToken(TokenType type, object? literal)
    {
        string text = _source.Substring(_start, _current - _start);
        _tokens.Add(new Token(type, text, literal, _line));
    }

    bool IsAtEnd => _current >= _source.Length;
}