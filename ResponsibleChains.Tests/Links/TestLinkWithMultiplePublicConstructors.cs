using System;

namespace ResponsibleChains.Tests.Links
{
    public class TestLinkWithMultiplePublicConstructors : ITestLink
    {
        private readonly ITestLink _next;

        public TestLinkWithMultiplePublicConstructors() : this(new TestEndLink()) { }

        public TestLinkWithMultiplePublicConstructors(ITestLink next)
        {
            _next = next;
        }

        public int Result(int input)
        {
            throw new NotImplementedException();
        }
    }
}
