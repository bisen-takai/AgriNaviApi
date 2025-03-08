namespace AgriNaviApi.Common.Settings
{
    public class SecuritySetting
    {
        public int SaltSize { get; set; }
        public int Iterations { get; set; }
        public int KeySize { get; set; }
    }
}
