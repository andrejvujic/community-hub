using CommunityHub.Application.Domain.Neighborhoods.NeighborhoodBudgets;
using CommunityHub.Application.MappersDto.Users;
using CommunityHub.Application.Services.Dtos.Neighborhoods.NeighborhoodBudgets;

namespace CommunityHub.Application.MappersDto.Neighborhoods.NeighborhoodBudgets
{
    public class BudgetDonationMapper : IMapper<BudgetDonation, BudgetDonationDto>
    {
        private readonly NeighborhoodMapper _neighborhoodMapper = new NeighborhoodMapper();
        private readonly BudgetCategoryMapper _budgetCategoryMapper = new BudgetCategoryMapper();
        private readonly CitizenMapper _citizenMapper = new CitizenMapper();

        public BudgetDonationDto MapToDto(BudgetDonation budgetDonation)
        {
            if (budgetDonation == null) throw new ArgumentNullException(nameof(budgetDonation));

            return new BudgetDonationDto(
                budgetDonation.Id,
                _neighborhoodMapper.MapToDto(budgetDonation.Neighborhood),
                _budgetCategoryMapper.MapToDto(budgetDonation.BudgetCategory),
                _citizenMapper.MapToDto(budgetDonation.Citizen),
                budgetDonation.Amount,
                budgetDonation.DonationDate
            );
        }

        public BudgetDonation MapToEntity(BudgetDonationDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new BudgetDonation(
                _neighborhoodMapper.MapToEntity(dto.Neighborhood),
                _budgetCategoryMapper.MapToEntity(dto.BudgetCategory),
                _citizenMapper.MapToEntity(dto.Citizen),
                dto.Amount,
                dto.DonationDate,
                dto.Id
            );
        }
    }
}
