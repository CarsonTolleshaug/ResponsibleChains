
namespace ChainLinks
{
    public class FizzBuzzLink : IFizzBuzzChain
    {
        private readonly IFizzBuzzChain _next;

        public FizzBuzzLink(IFizzBuzzChain next)
        {
            _next = next;
        }

        public string Execute(int input)
        {
            if (input % 15 == 0)
            {
                return "FizzBuzz";
            }

            return _next.Execute(input);
        }
    }
}
