namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategory
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        public BudgetCategory(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public BudgetCategory CopyWith(long? id = null, string? name = null)
        {
            return new BudgetCategory(id ?? Id, name ?? Name);
        }
    }
}