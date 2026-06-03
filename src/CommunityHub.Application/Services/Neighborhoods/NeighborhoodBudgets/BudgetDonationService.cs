using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods;
using CommunityHub.Application.Domain.RepositoryInterfaces.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Domain.Users;
using CommunityHub.Application.Domain.Neighborhoods;
using CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.Services.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetDonationService : BaseService<BudgetDonation, BudgetDonationDto>
    {
        private readonly IBudgetDonationRepository _donationRepository = Injector.CreateInstance<IBudgetDonationRepository>();
        private readonly IBudgetCategoryBalanceRepository _balanceRepository = Injector.CreateInstance<IBudgetCategoryBalanceRepository>();
        private readonly INeighborhoodRepository _neighborhoodRepository = Injector.CreateInstance<INeighborhoodRepository>();
        private readonly BudgetDonationMapper _donationMapper = new BudgetDonationMapper();

        public BudgetDonationService() : base(new BudgetDonationMapper()) { }

        public List<BudgetDonationDto> GetDonationsForNeighborhood(long neighborhoodId)
            => _donationRepository.GetByNeighborhoodId(neighborhoodId).Select(d => _donationMapper.MapToDto(d)).ToList();

        public List<BudgetDonationDto> GetDonationsForCategory(long neighborhoodId, long categoryId)
            => _donationRepository.GetByNeighborhoodIdAndCategoryId(neighborhoodId, categoryId).Select(d => _donationMapper.MapToDto(d)).ToList();

        public BudgetDonationDto CreateDonation(
            Citizen citizen,
            Neighborhood neighborhood,
            BudgetCategory category,
            decimal amount)
        {
            var donation = new BudgetDonation(neighborhood, category, citizen, amount, DateOnly.FromDateTime(DateTime.Today));
            var createdDonation = _donationRepository.Create(donation);
            UpdateCategoryBalance(category, neighborhood.Id, amount);
            UpdateNeighborhoodBudget(neighborhood, amount);
            return _donationMapper.MapToDto(createdDonation);
        }

        private void UpdateCategoryBalance(BudgetCategory category, long neighborhoodId, decimal amount)
        {
            var balances = _balanceRepository.GetByNeighborhoodId(neighborhoodId);
            var balance = balances.FirstOrDefault(b => b.Category.Id == category.Id);

            if (balance != null)
            {
                var newCategoryAmount = balance.TotalAmount + amount;
                _balanceRepository.UpdateTotalAmount(balance.Id, newCategoryAmount);
            }
        }

        private void UpdateNeighborhoodBudget(Neighborhood neighborhood, decimal amount)
        {
            var newNeighborhoodBudget = (int)(neighborhood.Budget + amount);
            _neighborhoodRepository.UpdateBudget(neighborhood.Id, newNeighborhoodBudget);
        }
    }
}
