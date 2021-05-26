namespace ResponsibleChains.Tests.Links
{
    public class TestDependency
    {
        public string Value { get; set; }
    }

    public class TestLinkWithDependencies : ITestLink
    {
        private readonly ITestLink _nextLink;
        private readonly TestDependency _otherDependency;

        public TestLinkWithDependencies(ITestLink nextLink, TestDependency otherDependency)
        {
            _nextLink = nextLink;
            _otherDependency = otherDependency;
        }

        public TestDependency TestDependency => _otherDependency;
        public ITestLink Next => _nextLink;

        public int Result(int input)
        {
            return _nextLink.Result(input);
        }
    }
}
