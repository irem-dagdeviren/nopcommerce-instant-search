using System.Text;

namespace Nop.Plugin.InstantSearch.KendoUI
{
  public class GridFilters
  {
    protected static readonly IDictionary<string, string> operators = (IDictionary<string, string>) new Dictionary<string, string>()
    {
      {
        "eq",
        "="
      },
      {
        "neq",
        "!="
      },
      {
        "lt",
        "<"
      },
      {
        "lte",
        "<="
      },
      {
        "gt",
        ">"
      },
      {
        "gte",
        ">="
      },
      {
        "startswith",
        "StartsWith"
      },
      {
        "endswith",
        "EndsWith"
      },
      {
        "contains",
        "Contains"
      },
      {
        "doesnotcontain",
        "DoesNotContain"
      }
    };

    public IList<GridFilter> Filters { get; set; }

    public string Logic { get; set; }

    public string ToExpression(IList<GridFilter> filters, out IList<object> values)
    {
      values = (IList<object>) new List<object>();
      return filters != null && filters.Count > 0 ? this.BuildExpression(filters, values) : string.Empty;
    }

    protected virtual string BuildExpression(
      IList<GridFilter> filters,
      IList<object> values,
      int index = 0)
    {
      StringBuilder stringBuilder1 = new StringBuilder();
      stringBuilder1.Append("(");
      int num1 = index;
      for (int index1 = index; index1 < filters.Count + index; ++index1)
      {
        GridFilter filter = filters[index1 - index];
        string str = GridFilters.operators[filter.Operator];
        if (filter.Field.StartsWith("Id"))
        {
          int result;
          if (int.TryParse(filter.Value, out result))
          {
            stringBuilder1.AppendFormat("{0} {1} @{2}", (object) filter.Field, (object) str, (object) num1++);
            values.Add((object) result);
          }
        }
        else
        {
          switch (str)
          {
            case "=":
              stringBuilder1.AppendFormat("{0}.ToLower().Equals(@{1}.ToLower())", (object) filter.Field, (object) num1++);
              values.Add((object) filter.Value);
              break;
            case "!=":  //
                int currentNum2 = num1++;
                int nextNum2 = num1++;
                stringBuilder1.AppendFormat("!({0}.Equals(@{1}, @{2}))", filter.Field, currentNum2, nextNum2);
                values.Add(filter.Value);
                values.Add(StringComparison.InvariantCultureIgnoreCase);
                break;
            case "Contains":
              stringBuilder1.AppendFormat("{0}.Contains(@{1})", (object) filter.Field, (object) num1++);
              values.Add((object) filter.Value);
              break;
            case "DoesNotContain":
              stringBuilder1.AppendFormat("!({0}.Contains(@{1}))", (object) filter.Field, (object) num1++);
              values.Add((object) filter.Value);
              break;
            case "StartsWith":
            case "EndsWith":
              stringBuilder1.AppendFormat("{0}.{1}(@{2})", (object) filter.Field, (object) str, (object) num1++);
              values.Add((object) filter.Value);
              break;
            default:
              int result1;
              if (int.TryParse(filter.Value, out result1))
              {
                stringBuilder1.AppendFormat("{0} {1} @{2}", (object) filter.Field, (object) str, (object) num1++);
                values.Add((object) result1);
                break;
              }
              stringBuilder1.AppendFormat("{0} {1} @{2}", (object) filter.Field, (object) str, (object) num1++);
              values.Add((object) filter.Value);
              break;
          }
        }
        if (index1 < filters.Count - 1)
          stringBuilder1.AppendFormat(" {0} ", (object) this.Logic);
      }
      stringBuilder1.Append(")");
      return stringBuilder1.ToString();
    }
  }
}
