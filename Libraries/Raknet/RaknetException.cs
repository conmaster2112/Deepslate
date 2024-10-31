namespace ConMaster.Raknet
{
    public class RaknetException(string name, Exception? inner = default): Exception(name, inner);
}
