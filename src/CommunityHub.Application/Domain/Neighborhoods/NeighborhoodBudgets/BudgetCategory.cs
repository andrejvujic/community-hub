namespace CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategory
    {
        public long Id { get; private set; }
        public string Name { get; private set; }

        public BudgetCategory(string name, long id = 0)
        {
            Id = id;
            Name = name;
        }

        public BudgetCategory CopyWith(long? id = null, string? name = null)
        {
            return new(name ?? Name, id ?? Id);
        }
    }
}