using System.Collections.Generic;

namespace Models
{
    public enum ResourceType
    {
        Picture = 0,
        Table = 1
    }

    public abstract class ResourceBase
    {
        public ResourceType ResourceType { get; set; }
        public int Index { get; set; }
    }

    public class ImageResource : ResourceBase
    {
        public string Description { get; set; }
        public string Uri { get; set; }
    }
    public class TableResourceItem
    {
        public string Column1 { get; set; }
        public string Column2 { get; set; }
    }
    public class TableResource : ResourceBase
    {
        public IEnumerable<TableResourceItem> Rows { get; set; }
    }
}
