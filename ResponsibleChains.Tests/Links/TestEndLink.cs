namespace ResponsibleChains.Tests.Links
{
    public class TestEndLink : ITestLink
    {
        public int RespondsWith { get; set; }

        public int Result(int input)
        {
            return RespondsWith;
        }
    }
}
