namespace Services;

public static class ExceptionHandlingService
{
    public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
            Console.WriteLine($"Something went wrong. \n {exception}");
        else
            Console.WriteLine($"Something went wrong. Non-Exception object: \n{e.ExceptionObject}");
        
        Environment.Exit(1);
    }
}