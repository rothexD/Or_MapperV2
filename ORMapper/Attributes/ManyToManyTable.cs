namespace ORMapper.Attributes
{
    public class ManyToManyTableAttribute : TableAttribute
    {
        public ManyToManyTableAttribute()
        {
            isManyToManyTable = true;
        }
    }
}