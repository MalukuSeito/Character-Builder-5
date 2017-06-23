namespace Character_Builder
{
    public class UsedResource
    {
        public string ResourceID;
        public int Used;
        public UsedResource()
        {
            ResourceID = "";
            Used = 0;
        }
        public UsedResource(string id, int value)
        {
            ResourceID = id;
            Used = value;
        }
    }
}
