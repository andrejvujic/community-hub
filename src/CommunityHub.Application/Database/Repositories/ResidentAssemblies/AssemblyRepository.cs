using CommunityHub.Application.Database.Mapping.EntityMappers;
using CommunityHub.Application.Domain.RepositoryInterfaces.ResidentAssemblies;
using CommunityHub.Application.Domain.ResidentAssemblies;

namespace CommunityHub.Application.Database.Repositories.ResidentAssemblies
{
    public class AssemblyRepository : IAssemblyRepository
    {
        private readonly AssemblyMapper _assemblyMapper = new();

        public Assembly Create(Assembly assembly)
        {
            using var connection = DatabaseConnection.Create();

            var query = DatabaseQuery
                .Create(
                    @"INSERT INTO assemblies
                        (building_id, starts_at, status)
                    VALUES
                        (@building_id, @starts_at, @status)
                    RETURNING id;"
                )
                .ExtendParameters(_assemblyMapper.MapToQueryParameters(assembly));

            var id = connection.ExecuteScalar(query, Convert.ToInt64);

            return new Assembly(
                id: id,
                buildingId: assembly.BuildingId,
                buildingApartmentCount: assembly.BuildingApartmentCount,
                startsAt: assembly.StartsAt,
                status: assembly.Status,
                attendances: assembly.Attendances.ToList()
            );
        }

        public void Update(Assembly assembly)
        {
            using var connection = DatabaseConnection.Create();

            connection.Execute(
                DatabaseQuery
                    .Create(
                        @"UPDATE assemblies
                        SET
                            building_id = @building_id,
                            starts_at = @starts_at,
                            status = @status
                        WHERE id = @id;"
                    )
                    .AddParameter("id", assembly.Id)
                    .ExtendParameters(_assemblyMapper.MapToQueryParameters(assembly))
            );
        }
    }
}
