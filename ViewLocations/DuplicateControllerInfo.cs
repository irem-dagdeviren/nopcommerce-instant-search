namespace Nop.Plugin.InstantSearch.ViewLocations
{
    public class DuplicateControllerInfo : IEquatable<DuplicateControllerInfo>
    {
        public string DuplicateControllerName { get; set; }

        public string DuplicateOfControllerName { get; set; }

        public bool Equals(DuplicateControllerInfo other)
        {
            if ((object) other == null)
            return false;
            return (object) this == (object) other || object.Equals((object) other.DuplicateControllerName, (object) this.DuplicateControllerName);
        }

        public static bool operator ==(DuplicateControllerInfo left, DuplicateControllerInfo right) => object.Equals((object) left, (object) right);

        public static bool operator !=(DuplicateControllerInfo left, DuplicateControllerInfo right) => !object.Equals((object) left, (object) right);
        }
}
