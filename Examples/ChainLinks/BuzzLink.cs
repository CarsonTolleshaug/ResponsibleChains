namespace ChainLinks
{
    public class BuzzLink : IFizzBuzzChain
    {
        private readonly IFizzBuzzChain _next;

        public BuzzLink(IFizzBuzzChain next)
        {
            _next = next;
        }

        public string Execute(int input)
        {
            if (input % 5 == 0)
            {
                return "Buzz";
            }

            return _next.Execute(input);
        }
    }
}
