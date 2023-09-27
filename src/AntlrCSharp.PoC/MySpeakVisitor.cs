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
