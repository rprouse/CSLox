public static class CSLox
{
    public static bool HadError { get; set; }

    public static int RunFile(string path)
    {
        string source = File.ReadAllText(path);
        Run(source);
        return HadError ? 65 : 0;
    }

    public static void RunPrompt()
    {
        while (true)
        {
            Console.WriteLine("> ");
            string? line = Console.ReadLine();
            if (line == null) break;
            Run(line);
            HadError = false;
        }
    }

    static void Run(string source)
    {
        var scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    public static void Report(int line, string where, string message)
    {
        Console.WriteLine($"[line {line}] Error{where}: {message}");
        HadError = true;
    }
}