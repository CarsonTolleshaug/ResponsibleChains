namespace ResponsibleChains.Tests.Links
{
    public class TestLinkWithNoPublicConstructor : ITestLink
    {
        private readonly ITestLink _next;

        protected TestLinkWithNoPublicConstructor(ITestLink next)
        {
            _next = next;
        }

        public int Result(int input)
        {
            throw new System.NotImplementedException();
        }
    }
}
