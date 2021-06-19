namespace ChainLinks
{
    public class FizzLink : IFizzBuzzChain
    {
        private readonly IFizzBuzzChain _next;

        public FizzLink(IFizzBuzzChain next)
        {
            _next = next;
        }

        public string Execute(int input)
        {
            if (input % 3 == 0)
            {
                return "Fizz";
            }

            return _next.Execute(input);
        }
    }
}
