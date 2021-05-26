namespace ResponsibleChains.Tests.Links
{
    public class TestLink : ITestLink
    {
        private readonly ITestLink _nextLink;

        public TestLink(ITestLink nextLink)
        {
            _nextLink = nextLink;
        }

        public ITestLink Next => _nextLink;
        public int ListensFor { get; set; }
        public int RespondsWith { get; set; }

        public int Result(int input)
        {
            if (input == ListensFor)
            {
                return RespondsWith;
            }

            return _nextLink.Result(input);
        }
    }
}
