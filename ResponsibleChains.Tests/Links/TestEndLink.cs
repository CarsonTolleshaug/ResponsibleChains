namespace ResponsibleChains.Tests.Links
{
    public class TestEndLink : ITestLink
    {
        public int RespondsWith { get; set; } = 1337;

        public int Result(int input)
        {
            return RespondsWith;
        }
    }
}
