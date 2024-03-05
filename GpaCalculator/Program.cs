namespace GpaCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create Calculator Service Object
            CalculatorService calculatorService = new CalculatorService();
            // Start calculator
            calculatorService.Start();
        }
    }
}
