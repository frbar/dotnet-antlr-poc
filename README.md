# Get started with ANTLR4 / C#

Strongly inspired by: https://tomassetti.me/getting-started-with-antlr-in-csharp/

- In a new directory, let's call it `src`

```powershell
dotnet new sln

dotnet new console -o AntlrCSharp.PoC
dotnet new classlib -o AntlrCSharp.Lib
dotnet new mstest -o AntlrCSharp.Tests

dotnet sln add .\AntlrCSharp.PoC\AntlrCSharp.PoC.csproj
dotnet sln add .\AntlrCSharp.Lib\AntlrCSharp.Lib.csproj
dotnet sln add .\AntlrCSharp.Tests\AntlrCSharp.Tests.csproj

dotnet add AntlrCSharp.PoC reference AntlrCSharp.Lib
dotnet add AntlrCSharp.Tests reference AntlrCSharp.Lib

dotnet add AntlrCSharp.Lib package Antlr4.Runtime.Standard --version 4.9
dotnet add AntlrCSharp.Lib package Antlr4BuildTasks
```

- Add `Speak.g4` from `resources` to `AntlrCSharp.Lib` project.
- Edit `AntlrCSharp.Lib.csproj` to configure ANTLR4

```powershell
<ItemGroup>
    <Antlr4 Include="Speak.g4">
        <Listener>false</Listener>
        <Visitor>true</Visitor>
        <GAtn>true</GAtn> 
        <Package>foo</Package>
        <Error>true</Error>
    </Antlr4>
</ItemGroup>
```

- Build `AntlrCSharp.Lib` (this will trigger ANTLR4): `dotnet build .\AntlrCSharp.Lib\`
- Check content of `AntlrCSharp.Lib\obj\Debug\net7.0\`: `SpeakParser.cs`, `SpeakVisitor.cs`, etc.

- Create a new `.cs` file in the console app project (`AntlrCSharp.PoC`) with the following content (our implementation of the `SpeakVisitor` and the `SpeakLine` class from `G. Tomassetti` tutorial).

```csharp
using Antlr.CSharp.Lib;

namespace AntlrCSharp.PoC
{
    public class MySpeakVisitor : SpeakBaseVisitor<object>
    {
        public List<SpeakLine> Lines = new();
        public override object VisitLine(SpeakParser.LineContext context)
        {
            var name = context.name();
            var opinion = context.opinion();
            var line = new SpeakLine() { Person = name.GetText(), Text = opinion.GetText().Trim('"') };
            Lines.Add(line);
            return line;
        }
    }

    public class SpeakLine
    {
        public required string Person { get; set; }
        public required string Text { get; set; }
    }
}
```

- Update `Program.cs`:

```csharp
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
```

- Run the console app: `dotnet run --project AntlrCSharp.PoC`

```powershell
PS C:\POCs\dotnet-antlr-poc\src> dotnet run --project AntlrCSharp.PoC
Input the chat, 2 lines, then CTRL+D and <enter> to type the EOF character and end the input.
francois says "hi"
michel says "bonjour"
^D
francois has said hi
michel has said bonjour
PS C:\POCs\dotnet-antlr-poc\src>
```

# Want to see more?
- Download https://graphviz.org/download/ and explore `.dot` files in `AntlrCSharp.Lib\obj\Debug\net7.0\`
- Use VS Code and the ANTLR extension
- https://tomassetti.me/antlr-mega-tutorial/
- https://www.youtube.com/watch?v=lc9JlXyBG4E
- https://www.youtube.com/watch?v=bfiAvWZWnDA
