using System.Text;

var message = new StringBuilder();
while (Console.ReadLine() is string line && line.Length > 0)
{
	message.Append(line);
}

Console.WriteLine($"Hello {message}!");