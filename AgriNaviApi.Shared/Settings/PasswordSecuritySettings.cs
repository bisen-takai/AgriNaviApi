namespace AgriNaviApi.Shared.Settings
{
    public record PasswordSecuritySettings
    {
        public int Iterations { get; init; }
        public int KeySize { get; init; }
    }
}
