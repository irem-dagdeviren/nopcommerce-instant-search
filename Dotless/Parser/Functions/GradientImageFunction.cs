// Decompiled with JetBrains decompiler
// Type:
//
// .Parser.Functions.GradientImageFunction
// Assembly: Nop.Plugin.InstantSearch.Dotless, Version=1.6.7.0, Culture=neutral, PublicKeyToken=96b446c9e63eae34
// MVID: 55C6A6E3-E2E0-4EF5-90A0-9EEBE61A5F60
// Assembly location: C:\Users\gunes\OneDrive\Masaüstü\SevenSpikes-InstantSearch\SevenSpikes.Core\Nop.Plugin.InstantSearch.Dotless.dll

using Nop.Plugin.InstantSearch.Dotless.Exceptions;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure;
using Nop.Plugin.InstantSearch.Dotless.Parser.Infrastructure.Nodes;
using Nop.Plugin.InstantSearch.Dotless.Parser.Tree;
using Nop.Plugin.InstantSearch.Dotless.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;

namespace Nop.Plugin.InstantSearch.Dotless.Parser.Functions
{
  public class GradientImageFunction : Function
  {
    public const int DEFAULT_COLOR_OFFSET = 50;
    private const int CACHE_LIMIT = 50;
    private static readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
    private static readonly List<GradientImageFunction.CacheItem> _cache = new List<GradientImageFunction.CacheItem>();

    protected override Node Evaluate(Env env)
    {
      GradientImageFunction.ColorPoint[] colorPoints = this.GetColorPoints();
      this.WarnNotSupportedByLessJS("gradientImage(color, color[, position])");
      string colorDefs = GradientImageFunction.ColorPoint.Stringify((IEnumerable<GradientImageFunction.ColorPoint>) colorPoints);
      string str = GradientImageFunction.GetFromCache(colorDefs);
      if (str == null)
      {
        str = "data:image/png;base64," + Convert.ToBase64String(this.GetImageData(colorPoints));
        GradientImageFunction.AddToCache(colorDefs, str);
      }
      return (Node) new Url((Node) new TextNode(str));
    }

    private byte[] GetImageData(GradientImageFunction.ColorPoint[] points)
    {
      GradientImageFunction.ColorPoint colorPoint = ((IEnumerable<GradientImageFunction.ColorPoint>) points).Last<GradientImageFunction.ColorPoint>();
      int height = colorPoint.Position + 1;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (Bitmap bitmap = new Bitmap(1, height, PixelFormat.Format32bppArgb))
        {
          using (Graphics graphics = Graphics.FromImage((Image) bitmap))
          {
            for (int index = 1; index < points.Length; ++index)
            {
              Rectangle rectangle = new Rectangle(0, points[index - 1].Position, 1, points[index].Position);
              LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, points[index - 1].Color, points[index].Color, LinearGradientMode.Vertical);
              graphics.FillRectangle((Brush) linearGradientBrush, rectangle);
            }
            bitmap.SetPixel(0, colorPoint.Position, colorPoint.Color);
            bitmap.Save((Stream) memoryStream, ImageFormat.Png);
          }
        }
        return memoryStream.ToArray();
      }
    }

    private GradientImageFunction.ColorPoint[] GetColorPoints()
    {
      int count = this.Arguments.Count;
      Guard.ExpectMinArguments(2, count, (object) this, this.Location);
      Guard.ExpectAllNodes<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color>(this.Arguments.Take<Node>(2), (object) this, this.Location);
      List<GradientImageFunction.ColorPoint> colorPointList = new List<GradientImageFunction.ColorPoint>()
      {
        new GradientImageFunction.ColorPoint((System.Drawing.Color) (Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color) this.Arguments[0], 0)
      };
      int num1 = 0;
      int num2 = 50;
      for (int index = 1; index < count; ++index)
      {
        Node actual = this.Arguments[index];
        Guard.ExpectNode<Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color>(actual, (object) this, this.Location);
        Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color color = actual as Nop.Plugin.InstantSearch.Dotless.Parser.Tree.Color;
        int position = num1 + num2;
        if (index < count - 1)
        {
          Number number = this.Arguments[index + 1] as Number;
          if ((bool) (Node) number)
          {
            position = Convert.ToInt32(number.Value);
            if (position <= num1)
              throw new ParsingException(string.Format("Incrementing color point position expected, at least {0}, found {1}", (object) (num1 + 1), (object) number.Value), this.Location);
            num2 = position - num1;
            ++index;
          }
        }
        colorPointList.Add(new GradientImageFunction.ColorPoint((System.Drawing.Color) color, position));
        num1 = position;
      }
      return colorPointList.ToArray();
    }

    private static string GetFromCache(string colorDefs)
    {
      GradientImageFunction._cacheLock.EnterReadLock();
      try
      {
        return GradientImageFunction._cache.FirstOrDefault<GradientImageFunction.CacheItem>((Func<GradientImageFunction.CacheItem, bool>) (item => item._def == colorDefs))?._url;
      }
      finally
      {
        GradientImageFunction._cacheLock.ExitReadLock();
      }
    }

    private static void AddToCache(string colorDefs, string imageUrl)
    {
      GradientImageFunction._cacheLock.EnterUpgradeableReadLock();
      try
      {
        if (!GradientImageFunction._cache.All<GradientImageFunction.CacheItem>((Func<GradientImageFunction.CacheItem, bool>) (item => item._def != colorDefs)))
          return;
        GradientImageFunction._cacheLock.EnterWriteLock();
        try
        {
          if (GradientImageFunction._cache.Count >= 50)
            GradientImageFunction._cache.RemoveRange(0, 25);
          GradientImageFunction.CacheItem cacheItem = new GradientImageFunction.CacheItem(colorDefs, imageUrl);
          GradientImageFunction._cache.Add(cacheItem);
        }
        finally
        {
          GradientImageFunction._cacheLock.ExitWriteLock();
        }
      }
      finally
      {
        GradientImageFunction._cacheLock.ExitUpgradeableReadLock();
      }
    }

    private class ColorPoint
    {
      public ColorPoint(System.Drawing.Color color, int position)
      {
        this.Color = color;
        this.Position = position;
      }

      public static string Stringify(
        IEnumerable<GradientImageFunction.ColorPoint> points)
      {
        return points.Aggregate<GradientImageFunction.ColorPoint, string>("", (Func<string, GradientImageFunction.ColorPoint, string>) ((s, point) =>
        {
          object[] objArray = new object[7];
          objArray[0] = (object) s;
          objArray[1] = s == "" ? (object) "" : (object) ",";
          System.Drawing.Color color = point.Color;
          objArray[2] = (object) color.A;
          color = point.Color;
          objArray[3] = (object) color.R;
          color = point.Color;
          objArray[4] = (object) color.G;
          color = point.Color;
          objArray[5] = (object) color.B;
          objArray[6] = (object) point.Position;
          return string.Format("{0}{1}#{2:X2}{3:X2}{4:X2}{5:X2},{6}", objArray);
        }));
      }

      public System.Drawing.Color Color { get; private set; }

      public int Position { get; private set; }
    }

    private class CacheItem
    {
      public readonly string _def;
      public readonly string _url;

      public CacheItem(string def, string url)
      {
        this._def = def;
        this._url = url;
      }
    }
  }
}
