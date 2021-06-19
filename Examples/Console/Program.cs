using ResponsibleChains;
using ChainLinks;

namespace Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0 || !int.TryParse(args[0], out int input) || input < 0)
            {
                System.Console.WriteLine("Please provide a positive integer as an argument.");
                return;
            }

            IFizzBuzzChain chain = new ResponsibleChainBuilder<IFizzBuzzChain>()
                .WithLink<FizzBuzzLink>()
                .WithLink<FizzLink>()
                .WithLink<BuzzLink>()
                .WithLink<DefaultLink>()
                .Build();

            string output = chain.Execute(input);

            System.Console.WriteLine(output);
        }
    }
}
