namespace Application.UseCases.ViewPermissions.Results {
    public class PermissionsResult {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
