namespace CommunityHub.Application.Domain
{
    public sealed class ServiceName
    {
        public static class Predefined
        {
            public const string LawnMowing = "Košenje trave";
            public const string ElevatorMaintenance = "Održavanje lifta";
            public const string PlaygroundRepair = "Popravke igrališta";
            
            public static IReadOnlyList<string> All { get; } = new[]
            {
                LawnMowing,
                ElevatorMaintenance,
                PlaygroundRepair,
            };
        }

        public string? PredefinedName { get; }

        public string? CustomName { get; }

        public string Value => PredefinedName ?? CustomName!;

        public bool IsCustom => PredefinedName is null;

        private ServiceName(string? predefinedName, string? customName)
        {
            PredefinedName = predefinedName;
            CustomName = customName;
        }

        public static ServiceName FromPredefined(string name)
        {
            if (!Predefined.All.Contains(name))
                throw new ArgumentException(
                    $"'{name}' is not a recognised predefined service name. " +
                    $"Use {nameof(FromCustom)} for provider-supplied names.",
                    nameof(name));

            return new ServiceName(predefinedName: name, customName: null);
        }

        public static ServiceName FromCustom(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Custom service name must not be empty.", nameof(name));

            if (Predefined.All.Contains(name))
                throw new ArgumentException(
                    $"'{name}' is already in the predefined catalogue. " +
                    $"Use {nameof(FromPredefined)} instead.",
                    nameof(name));

            return new ServiceName(predefinedName: null, customName: name);
        }

        public static ServiceName FromValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Service name value must not be empty.", nameof(value));

            return Predefined.All.Contains(value)
                ? new ServiceName(predefinedName: value, customName: null)
                : new ServiceName(predefinedName: null, customName: value);
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj) =>
            obj is ServiceName other &&
            string.Equals(Value, other.Value, StringComparison.Ordinal);

        public override int GetHashCode() =>
            StringComparer.Ordinal.GetHashCode(Value);
    }
}