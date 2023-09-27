using Antlr.CSharp.Lib;
using Antlr4.Runtime;
using AntlrCSharp.PoC;
using System.Text;

try
{
    string? input = "";
    var text = new StringBuilder();
    Console.WriteLine("Input the chat, 2 lines, then CTRL+D and <enter> to type the EOF character and end the input.");

    while ((input = Console.ReadLine()) != "\u0004")
    {
        text.AppendLine(input);
    }

    var inputStream = new AntlrInputStream(text.ToString());
    var speakLexer = new SpeakLexer(inputStream);
    var commonTokenStream = new CommonTokenStream(speakLexer);
    var speakParser = new SpeakParser(commonTokenStream);
    var chatContext = speakParser.chat();
    var visitor = new MySpeakVisitor();
    visitor.Visit(chatContext);
    foreach (var line in visitor.Lines)
    {
        Console.WriteLine("{0} has said {1}", line.Person, line.Text);
    }
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex);
}