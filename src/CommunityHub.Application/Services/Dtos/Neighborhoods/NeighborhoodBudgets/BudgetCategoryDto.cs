namespace CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetCategoryDto
    {
        public long Id {  get; set; }
        public string Name { get; set; }

        public BudgetCategoryDto(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
