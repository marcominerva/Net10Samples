#:package Microsoft.ML.Tokenizers@2.0.*
#:package Microsoft.ML.Tokenizers.Data.O200kBase@2.0.*

using System.Text;
using Microsoft.ML.Tokenizers;

var input = new StringBuilder();
while (Console.ReadLine() is string line && line != "§")
{
	input.AppendLine(line);
}

var tokenizer = TiktokenTokenizer.CreateForModel("gpt-4o");
var text = input.ToString();

Console.WriteLine(tokenizer.CountTokens(text));