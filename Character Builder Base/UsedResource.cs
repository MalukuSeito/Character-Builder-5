namespace Character_Builder
{
    public class UsedResource
    {
        public string ResourceID { get; set; }
        public int Used { get; set; }
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
