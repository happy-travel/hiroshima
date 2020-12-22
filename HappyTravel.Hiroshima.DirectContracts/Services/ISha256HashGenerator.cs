namespace HappyTravel.Hiroshima.DirectContracts.Services
{
    public interface ISha256HashGenerator
    {
        string Generate(string source);

        string Generate(byte[] bytes);
    }
}