namespace Nop.Plugin.InstantSearch.KendoUI
{
    public class GridSort
    {
        public string Field { get; set; }

        public string Dir { get; set; }

        protected virtual string BuildExpression() => this.Field + " " + this.Dir;
    }
}
