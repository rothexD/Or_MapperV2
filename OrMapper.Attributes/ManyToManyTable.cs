namespace OrMapper.Attributes
{
    public class ManyToManyTableAttribute : TableAttribute
    {
        public ManyToManyTableAttribute()
        {
            IsManyToManyTable = true;
        }
    }
}