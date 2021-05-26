namespace ResponsibleChains.Tests.Links
{
    public class TestLinkWithPrivateConstructor : ITestLink
    {
        private ITestLink _next;
        private int _value;

        public TestLinkWithPrivateConstructor(ITestLink next) : this(next, 42) { }

        private TestLinkWithPrivateConstructor(ITestLink next, int value)
        {
            _next = next;
            _value = value;
        }

        public int Result(int input)
        {
            return _value;
        }
    }
}
