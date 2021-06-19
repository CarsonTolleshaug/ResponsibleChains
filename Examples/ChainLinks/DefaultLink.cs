namespace ChainLinks
{
    public class DefaultLink : IFizzBuzzChain
    {
        public string Execute(int input)
        {
            return input.ToString();
        }
    }
}
