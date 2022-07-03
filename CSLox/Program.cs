using CSLox;

if (args.Length > 1)
{
    Console.WriteLine("Usage: cslox [script]");
    return 64;
}
else if (args.Length == 1)
{   
    // Run the given script
    return CSLoxLanguage.RunFile(args[0]);
}
else
{
    // Run interactively
    CSLoxLanguage.RunPrompt();
}
return 0;
