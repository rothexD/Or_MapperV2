namespace OrMapper.ExternalModels
{
    public static class Extentions
    {
        public static SecureParameter MakeSecure(this object obj)
        {
            return SecureParameter.Create(obj);
        }
        public static CaseInsensitive MakeCaseIns(this object obj)
        {
           return CaseInsensitive.Create(obj);
        }
    }
}