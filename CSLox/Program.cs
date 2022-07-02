if (args.Length > 1)
{
    Console.WriteLine("Usage: cslox [script]");
    return 64;
}
else if (args.Length == 1)
{   
    // Run the given script
    return CSLox.RunFile(args[0]);
}
else
{
    // Run interactively
    CSLox.RunPrompt();
}
return 0;
