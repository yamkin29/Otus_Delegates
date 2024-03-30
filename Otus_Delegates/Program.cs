namespace Otus_Delegates;

class Program
{
    static void Main(string[] args)
    {
        List<string> strings = ["one", "two", "three", "four", "five"];
        string maxString = strings.GetMax(s => s.Length);
        Console.WriteLine("Max string: " + maxString);

        List<int> numbers = [5, 10, 2, 8, 15];
        int maxNumber = numbers.GetMax(n => n);
        Console.WriteLine("Max number: " + maxNumber);
        
        List<Person> people =
        [
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 },
            new Person { Name = "Charlie", Age = 40 }
        ];

        Person oldestPerson = people.GetMax(p => p.Age);
        Console.WriteLine("Oldest person: " + oldestPerson.Name + "\n");
        
        //-------------------------------------------------------------------//
        
        string directoryPath = "C:\\CCP";
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        FileSearcher searcher = new FileSearcher();
        searcher.FileFound += HandleFileFound;

        try
        {
            searcher.SearchFiles(directoryPath, cancellationTokenSource.Token);
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            cancellationTokenSource.Dispose();
        }
    }
    
    static void HandleFileFound(object sender, FileFoundEventArgs e)
    {
        Console.WriteLine($"Found file: {e.FileName}");
        
        ((FileSearcher)sender).CancelSearch();
    }
}