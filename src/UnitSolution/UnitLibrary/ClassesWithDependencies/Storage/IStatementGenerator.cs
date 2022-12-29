
namespace UnitLibrary.ClassesWithDependencies.Storage
{
    public interface IStatementGenerator
    {
        string SaveStatement(int housekeeperOid, string housekeeperName, DateTime statementDate);
    }
}