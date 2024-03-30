namespace Otus_Delegates;

public class FileSearcher
{
    public event EventHandler<FileFoundEventArgs>? FileFound;

    private CancellationTokenSource? _cancellationTokenSource;
    
    private bool _searchCancelled = false;

    public void SearchFiles(string directory, CancellationToken cancellationToken)
    {
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            SearchDirectory(directory, cancellationToken);
            Console.WriteLine("Search completed successfully.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Search cancelled.");
        }
        finally
        {
            _cancellationTokenSource.Dispose();
        }
    }

    private void SearchDirectory(string directory, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException(cancellationToken);
        }

        if (_searchCancelled)
        {
            return;
        }

        string[] files = Directory.GetFiles(directory);
        foreach (string file in files)
        {
            OnFileFound(Path.GetFileName(file));

            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            if (_searchCancelled)
            {
                return;
            }
        }

        string[] subdirectories = Directory.GetDirectories(directory);
        foreach (string subdirectory in subdirectories)
        {
            SearchDirectory(subdirectory, cancellationToken);
            
            if (_searchCancelled)
            {
                return;
            }
        }
    }

    protected virtual void OnFileFound(string fileName)
    {
        FileFound?.Invoke(this, new FileFoundEventArgs(fileName));
        _searchCancelled = true;
    }
    
    public void CancelSearch()
    {
        _cancellationTokenSource?.Cancel();
    }
}

public class FileFoundEventArgs : EventArgs
{
    public string FileName { get; }

    public FileFoundEventArgs(string fileName)
    {
        FileName = fileName;
    }
}