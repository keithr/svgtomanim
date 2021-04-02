using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Aspose.Svg;
using Aspose.Svg.Dom;
using Aspose.Svg.Dom.Traversal.Filters;
using Aspose.Svg.Paths;

namespace SVGToManim
{
    public class Style
    {
        public Color Fill { get; set; } = Color.White;
        public Color Stroke { get; set; } = Color.Empty;
        public double StrokeWidth { get; set; } = 0.0;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }

        public static Style ToStyle(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, string s)
        {
            int index = 0;
            Color fill = Color.Empty;
            Color stroke = Color.Empty;
            double strokewidth = 1.0;
            ReadFillStroke(s, ref index, ref fill, ref stroke, ref strokewidth);
            return new Style{Fill=fill, Stroke = stroke, StrokeWidth = strokewidth};
        }

        private static void SkipJunk(string s, ref int index)
        {
            // Skip junk
            while (index < s.Length && (char.IsControl(s[index]) || char.IsWhiteSpace(s[index]) || s[index] == '.'))
            {
                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public string ManimFill(double scale)
        {
            return Fill != Color.Empty ? $".set_fill({ColorToString(Fill)},opacity=1)" : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        /// <returns></returns>
        public string ManimStroke(double scale)
        {
            return Stroke != Color.Empty ? $".set_stroke(color={ColorToString(Stroke)},width={StrokeWidth * scale})" : $".set_stroke(width=0)";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        internal static string ColorToString(Color c)
        {
            return $"\"#{c.R:X2}{c.G:X2}{c.B:X2}\"";
        }

        private static string ReadName(string s, ref int index)
        {
            var name = "";
            // Read Name
            while (index < s.Length && (char.IsLetterOrDigit(s[index]) || s[index] == '_' || s[index] == '-'))
            {
                name += s[index++];
            }

            return name;
        }

        private static void ReadFillStroke(string s, ref int index, ref Color fill, ref Color Stroke, ref double strokewidth)
        {
            if (s[index] == '{') index++;

            // Skip junk
            while (index < s.Length && (char.IsControl(s[index]) || char.IsWhiteSpace(s[index]) || s[index] == '.' || s[index] == ';'))
            {
                index++;
            }

            string name = ReadName(s, ref index);
            while (!string.IsNullOrEmpty(name) && index < s.Length)
            {
                if (String.Compare(name, "fill", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (index < s.Length && s[index] == ':')
                    {
                        index++;
                        var contents = "";
                        while (index < s.Length && s[index] != ';' && s[index] != '}')
                        {
                            if (s[index] != '#')
                                contents += s[index];
                            index++;
                        }

                        try
                        {
                            var value = Convert.ToUInt32(contents, 16);
                            fill = Color.FromArgb((int) value);
                        }
                        catch
                        {

                        }
                        
                        index++;
                    }
                }
                else if (String.Compare(name, "stroke", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (index < s.Length && s[index] == ':')
                    {
                        index++;
                        var contents = "";
                        while (index < s.Length && s[index] != ';' && s[index] != '}')
                        {
                            if (s[index] != '#')
                                contents += s[index];
                            index++;
                        }

                        try
                        {
                            var value = Convert.ToUInt32(contents, 16);
                            Stroke = Color.FromArgb((int)value);
                        }
                        catch
                        {
                        }
                       
                        index++;
                    }
                }
                else if (String.Compare(name, "stroke-width", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (index < s.Length && s[index] == ':')
                    {
                        index++;
                        var contents = "";
                        while (index < s.Length && s[index] != ';' && s[index] != '}')
                        {
                            if (s[index] != '#')
                                contents += s[index];
                            index++;
                        }

                        strokewidth = Convert.ToDouble(contents);
                        index++;
                    }
                }
                else
                {
                    // Ignore
                    if (index < s.Length && s[index] == ':')
                    {
                        index++;
                        while (index < s.Length && s[index] != ';' && s[index] != '}')
                            index++;
                        index++;
                    }
                }

                name = ReadName(s, ref index);
            }

            while (index < s.Length && s[index] != '}')
                index++;
            index++;
        }

        public static Style ToStyle(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, Node item)
        {
            string s = item.TextContent;
            if (!string.IsNullOrEmpty(s))
            {
                int index = 0;
                while (index < s.Length)
                {
                    SkipJunk(s, ref index);
                    var name = ReadName(s, ref index);
                    if (string.IsNullOrEmpty(name)) break;
                    Color fill = Color.Empty, stroke = Color.Empty;
                    double strokewidth = 1.0;
                    ReadFillStroke(s, ref index, ref fill, ref stroke, ref strokewidth);
                    styledict.Add(name, new Style{Fill=fill,Stroke=stroke,StrokeWidth=strokewidth});
                }
            }

            return new Style();
        }
    }

    public interface ITransform
    {

    }

    public class RectD
    {
        /// <summary>
        /// 
        /// </summary>
        public double Height { get; set; } = 0.0;

        /// <summary>
        /// 
        /// </summary>
        public double Width { get; set; } = 0.0;

        /// <summary>
        /// 
        /// </summary>
        public double Left { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Right
        {
            get => Left + Width;
            set => Width = value - Left;
        }

        /// <summary>
        /// 
        /// </summary>
        public double Top { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Bottom
        {
            get => Top + Height;
            set => Height = value - Top;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static RectD Combine(RectD lhs, RectD rhs)
        {
            if ((lhs == null || lhs.Height <= 0.0) && (rhs == null || rhs.Width <= 0)) return new RectD();
            if (lhs == null || lhs.Height <= 0.0) return rhs;
            if (rhs == null || rhs.Height <= 0.0) return lhs;
            return new RectD
            {
                Left = Math.Min(lhs.Left, rhs.Left), Top = Math.Min(lhs.Top, rhs.Top),
                Height = Math.Max(lhs.Height, rhs.Height), Width = Math.Max(lhs.Width, rhs.Width)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        public double CenterX => Width / 2.0 + Left;

        /// <summary>
        /// 
        /// </summary>
        public double CenterY => Height / 2.0 + Top;
    }

    public interface IShape
    {
        Style Style { get; set; }
        ITransform Transform { get; set; }
        string ToManim(List<string> symbols = null, int indent = 0, double scale = double.NaN);
        RectD Bounds { get; }
    }

    public class Group : IShape, IEnumerable<IShape>
    {
        private List<IShape> _group = new List<IShape>();

        public Style Style { get; set; }
        public ITransform Transform { get; set; }

        public string ToManim(List<string> symbols, int indent = 0, double scale = double.NaN)
        {
            var sb = new StringBuilder();
            var ind = (symbols == null ? indent + 1 : indent);
            string name = SVGToManim.UniqueName("g");
            if (symbols == null && indent == 0)
            {
                sb.AppendLine($"{SVGToManim.Indent(indent)}#from manimlib.imports import *");
                sb.AppendLine($"{SVGToManim.Indent(indent)}def SVGToManinObject():");
                if (double.IsNaN(scale) && Bounds.Height > 0)
                {
                    scale = 1 / Bounds.Height;
                }
            }

            var localsymbols = new List<string>();
            
            foreach (var item in this)
            {
                sb.Append(item.ToManim(localsymbols, ind, scale));
            }

            sb.Append($"{SVGToManim.Indent(ind)}{name} = Group(");
            bool bFirst = true;
            foreach (var sym in localsymbols)
            {
                if (!bFirst) sb.Append(',');
                sb.Append(sym);
                bFirst = false;
            }
            sb.AppendLine(")");

            if (symbols == null && indent == 0)
            {
                var bounds = Bounds;
                sb.AppendLine($"{SVGToManim.Indent(ind)}{name}.center()");
                sb.AppendLine($"{SVGToManim.Indent(ind)}{name}.flip(LEFT)");
                sb.AppendLine($"{SVGToManim.Indent(ind)}{name}.scale({scale})");
                sb.AppendLine($"{SVGToManim.Indent(ind)}# Item is now scaled - height=1 and it is centered");
                sb.AppendLine($"{SVGToManim.Indent(ind)}return {name}");
            }
            else
            {
                symbols.Add(name);
            }

            return sb.ToString();
        }

        public RectD Bounds
        {
            get
            {
                var retval = new RectD();
                foreach (var item in this)
                {
                    retval = RectD.Combine(retval, item.Bounds);
                }

                return retval;
            }
        }

        public IEnumerator<IShape> GetEnumerator()
        {
            return _group.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IShape shape)
        {
            _group.Add(shape);
        }

        public static IShape ToShape(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, Node item)
        {
            Stack<Group> current = new Stack<Group>();
            var g = new Group();
            current.Push(g);
            foreach (var child in item.ChildNodes)
            {
                SVGToManim.ProcessSVGItem(current, style, styledict, child);
            }

            current.Pop();
            return g;
        }
    }


    public class Rect : IShape
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public double RY { get; set;}

        public Style Style { get; set; }
        public ITransform Transform { get; set; }


        public string ToManim(List<string> symbols, int indent, double scale)
        {
            var name = SVGToManim.UniqueName("r");
            symbols.Add(name);
            var sb = new StringBuilder();
            sb.AppendLine($"{SVGToManim.Indent(indent)}{name}=Rectangle(height={Height},width={Width}).move_to(({X + Width / 2.0},{Y + Height / 2.0},0)){Style.ManimFill(1)}{Style.ManimStroke(1)}");
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public RectD Bounds => new RectD {Left = X, Top = Y, Height = Height, Width = Width};

        /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        /// <param name="style"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IShape ToShape(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, Node item)
        {
            double height, width, x, y, ry;
            Style curstyle;
            height = item.Attributes.GetNamedItem("height") != null
                ? double.Parse(item.Attributes.GetNamedItem("height").Value)
                : 0.0;
            width = item.Attributes.GetNamedItem("width") != null
                ? double.Parse(item.Attributes.GetNamedItem("width").Value)
                : 0.0;
            x = item.Attributes.GetNamedItem("x") != null
                ? double.Parse(item.Attributes.GetNamedItem("x").Value)
                : 0.0;
            y = item.Attributes.GetNamedItem("y") != null
                ? double.Parse(item.Attributes.GetNamedItem("y").Value)
                : 0.0;
            ry = item.Attributes.GetNamedItem("ry") != null
                ? double.Parse(item.Attributes.GetNamedItem("ry").Value)
                : 0.0;

            if (item.Attributes.GetNamedItem("class") != null)
            {
                var stylename = item.Attributes.GetNamedItem("class").Value;
                if (styledict.ContainsKey(stylename))
                {
                    curstyle = styledict[stylename];
                }
                else
                {
                    curstyle = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
                }
            }
            else
            {
                curstyle = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
            }
            
            return new Rect
            {
                Height = height,
                Width = width,
                X = x,
                Y = y,
                RY = ry,
                Style = curstyle
            };
        }

        public override string ToString()
        {
            return $"rect:H={Height},W={Width},X={X},Y={Y},RY={RY},Style={Style}";
        }
    }


    public class Path : IShape, IEnumerable<PolyItem>
    {
        private List<PolyItem> _items = new List<PolyItem>();
        public Style Style { get; set; }
        public ITransform Transform { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PolyItem this[int index] => _items[index];

        /// <summary>
        /// 
        /// </summary>
        public int Count => _items.Count;

        public string ToManim(List<string> symbols, int indent =0, double scale = 1.0)
        {
            var sb = new StringBuilder();
            var name = SVGToManim.UniqueName("p");
            sb.Append($"{SVGToManim.Indent(indent)}{name}=Polygon(");
            for (int i = 0; i < _items.Count; i++)
            {
                var pt = _items[i];
                if (i % 10 == 0)
                {
                    sb.Append($"({pt.X},{pt.Y},0)");
                    if (i != _items.Count - 1)
                        sb.Append(",");
                    sb.AppendLine("");
                    sb.Append($"{SVGToManim.Indent(indent+1)}");
                }
                else
                {
                    sb.Append($"({pt.X},{pt.Y},0)");
                    if (i != _items.Count - 1)
                        sb.Append(",");
                }
            }

            sb.AppendLine($"){Style.ManimFill(1)}{Style.ManimStroke(1)}");
            symbols?.Add(name);
            return sb.ToString();
        }


        public static IShape ToShape(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, Node item)
        {
            var poly = new Path();
            var ps = item as SVGPathElement;
            double cx = 0;
            double cy = 0;
            foreach (var p in ps.PathSegList)
            {
                switch (p.PathSegTypeAsLetter)
                {
                    case "c": // Curveto
                    case "C":
                        if (p is SVGPathSegCurvetoCubicRel cr)
                        {
                            var start = new PointF { X = (float) cx, Y = (float) cy };
                            var p1 = new PointF { X = (float)cx + cr.X1, Y = (float)cy + cr.Y1 };
                            var p2 = new PointF { X = (float)cx + cr.X2, Y = (float)cy+cr.Y2 };
                            cx += cr.X;
                            cy += cr.Y;
                            var end = new PointF { X = (float)cx, Y = (float)cy };
                            List<Line> lines = new List<Line>();
                            Bezier b = new Bezier(start, p1, p2, end);
                            b.SplitBezier(lines);
                            for (int i = 0; i < lines.Count; i++)
                            {
                                var line = lines[i];
                                if (i == 0)
                                {
                                    poly.Add(new PolyItem { X = line.P2.X, Y = line.P2.Y });
                                }
                                else
                                {
                                    poly.Add(new PolyItem {X = line.P1.X, Y = line.P1.Y});
                                    poly.Add(new PolyItem {X = line.P2.X, Y = line.P2.Y});
                                }
                            }
                        }
                        else if (p is SVGPathSegCurvetoCubicAbs ca)
                        {
                            var start = new PointF { X = (float)cx, Y = (float)cy };
                            var p1 = new PointF { X = (float)ca.X1, Y = (float)ca.Y1 };
                            var p2 = new PointF { X = (float)ca.X2, Y = (float)ca.Y2 };
                            cx = ca.X;
                            cy = ca.Y;
                            var end = new PointF { X = (float)cx, Y = (float)cy };
                            List<Line> lines = new List<Line>();
                            Bezier b = new Bezier(start, p1, p2, end);
                            b.SplitBezier(lines);
                            for (int i = 0; i < lines.Count; i++)
                            {
                                var line = lines[i];
                                if (i == 0)
                                {
                                    poly.Add(new PolyItem { X = line.P2.X, Y = line.P2.Y });
                                }
                                else 
                                {
                                    poly.Add(new PolyItem { X = line.P1.X, Y = line.P1.Y });
                                    poly.Add(new PolyItem { X = line.P2.X, Y = line.P2.Y });
                                }
                            }
                        }
                        break;
                    case "m": // MoveTo
                    case "M":
                        if (p is SVGPathSegMovetoAbs mtAbs)
                        {
                            cx = mtAbs.X;
                            cy = mtAbs.Y;
                            poly.Add(new PolyItem { X = cx, Y=cy });
                        }
                        else if(p is SVGPathSegMovetoRel mtr)
                        {
                            cx += mtr.X;
                            cy += mtr.Y;
                            poly.Add(new PolyItem { X = cx, Y = cy });
                        }
                        break;
                    case "v": // Vertical Lineto
                    case "V":
                        if (p is SVGPathSegLinetoVerticalRel vr)
                        {
                            cy += vr.Y;
                            poly.Add(new PolyItem { X = cx, Y = cy });
                        }
                        else if (p is SVGPathSegLinetoVerticalAbs va)
                        {
                            cy = va.Y;
                            poly.Add(new PolyItem { X = cx, Y = cy });
                        }
                        break;
                    case "q": // quadratic Bézier curve
                    case "Q":
                        break;
                    case "l": // Line To
                    case "L":
                        if (p is SVGPathSegLinetoRel lr)
                        {
                            cx += lr.X;
                            cy += lr.Y;
                            poly.Add(new PolyItem { X = cx, Y = cy });
                        }
                        else if (p is SVGPathSegLinetoAbs la)
                        {
                            cx = la.X;
                            cy = la.Y;
                            poly.Add(new PolyItem { X = cx, Y = cy });
                        }
                        break;
                    case "z": // Close
                    case "Z":
                        break;
                    case "h": // Horizontal Lineto
                    case "H":
                        if (p is SVGPathSegLinetoHorizontalRel hr)
                        {
                            cx += hr.X;
                            poly.Add(new PolyItem { X = cx, Y = cy });
                        }
                        else if (p is SVGPathSegLinetoHorizontalAbs ha)
                        {
                            cx = ha.X;
                            poly.Add(new PolyItem { X = cx, Y = cy });
                        }
                        break;
                    default:
                        break;
                }
            }

            if (item.Attributes.GetNamedItem("class") != null)
            {
                var stylename = item.Attributes.GetNamedItem("class").Value;
                if (styledict.ContainsKey(stylename))
                {
                    poly.Style = styledict[stylename];
                }
                else
                {
                    poly.Style = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
                }
            }
            else
            {
                poly.Style = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
            }


            return poly;
        }

        public RectD Bounds { get; }


        public IEnumerator<PolyItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(PolyItem item)
        {
            _items.Add(item);
        }
    }


    public class PolyItem
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class Polygon : IShape, IEnumerable<PolyItem>
    {
        private List<PolyItem> _items = new List<PolyItem>();
        public Style Style { get; set; }
        public ITransform Transform { get; set; }

        public string ToManim(List<string> symbols, int indent = 0, double scale = 1)
        {
            var sb = new StringBuilder();
            var name = SVGToManim.UniqueName("p");
            sb.Append($"{SVGToManim.Indent(indent)}{name}=Polygon(");
            for (int i = 0; i < _items.Count; i++)
            {
                var pt = _items[i];
                if (i % 10 == 0)
                {
                    sb.Append($"({pt.X},{pt.Y},0)");
                    if (i != _items.Count - 1)
                        sb.Append(",");
                    sb.AppendLine();
                    sb.Append($"{SVGToManim.Indent(indent + 1)}");
                }
                else
                {
                    sb.Append($"({pt.X},{pt.Y},0)");
                    if (i != _items.Count - 1)
                        sb.Append(",");
                }
            }

            sb.AppendLine($"){Style.ManimFill(1)}{Style.ManimStroke(1)}");
            symbols?.Add(name);
            return sb.ToString();
        }

        public RectD Bounds { get; }

        public IEnumerator<PolyItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(PolyItem item)
        {
            _items.Add(item);
        }

        public static IShape ToShape(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, Node item)
        {
            var poly = new Polygon();
            if (item is SVGPolygonElement p)
            {
                foreach (var pt in p.AnimatedPoints)
                {
                    poly.Add(new PolyItem{X=pt.X, Y=pt.Y});
                }
            }

            if (item.Attributes.GetNamedItem("class") != null)
            {
                var stylename = item.Attributes.GetNamedItem("class").Value;
                if (styledict.ContainsKey(stylename))
                {
                    poly.Style = styledict[stylename];
                }
                else
                {
                    poly.Style = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
                }
            }
            else
            {
                poly.Style = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
            }
            

            return poly;
        }
    }

    public class Circle : IShape
    {
        public double CX { get; set;}
        public double CY { get; set;}
        public double R { get; set;}
        public Style Style { get; set; }
        public ITransform Transform { get; set; }
        public string ToManim(List<string> symbols, int indent =0, double scale = 1)
        {
            var sb = new StringBuilder();
            var name = SVGToManim.UniqueName("c");
            sb.AppendLine($"{SVGToManim.Indent(indent)}{name}=Circle(radius={R}).move_to(({CX + R / 2.0},{CY + scale / 2.0},0)){Style.ManimFill(scale)}{Style.ManimStroke(scale)}");
            symbols?.Add(name);
            return sb.ToString();
        }

        public RectD Bounds { get; }

        public static IShape ToShape(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, Node item)
        {
            double cx, cy, r;
            Style curstyle;
            cx = item.Attributes.GetNamedItem("cx") != null
                ? double.Parse(item.Attributes.GetNamedItem("cx").Value)
                : 0.0;
            cy = item.Attributes.GetNamedItem("cy") != null
                ? double.Parse(item.Attributes.GetNamedItem("cy").Value)
                : 0.0;
            r = item.Attributes.GetNamedItem("r") != null
                ? double.Parse(item.Attributes.GetNamedItem("r").Value)
                : 0.0;
            if (item.Attributes.GetNamedItem("class") != null)
            {
                var stylename = item.Attributes.GetNamedItem("class").Value;
                if (styledict.ContainsKey(stylename))
                {
                    curstyle = styledict[stylename];
                }
                else
                {
                    curstyle = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
                }
            }
            else
            {
                curstyle = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
            }
            return new Circle
            {
                CX = cx,
                CY = cy,
                R = r,
                Style = curstyle
            };
        }
    }

    public class Ellipse : IShape
    {
        public double CX { get; set; }
        public double CY { get; set; }

        public double RX { get; set; }

        public double RY { get; set; }

        public Style Style { get; set; }
        public ITransform Transform { get; set; }

        public string ToManim(List<string> symbols, int indent = 0, double scale = 1)
        {
            var sb = new StringBuilder();
            var name = SVGToManim.UniqueName("e");
            sb.AppendLine($"{SVGToManim.Indent(indent)}{name}=Ellipse(width={RX*2},height={RY*2}).move_to(({CX},{CY},0)){Style.ManimFill(scale)}{Style.ManimStroke(scale)}");
            symbols?.Add(name);
            return sb.ToString();
        }

        public RectD Bounds { get; }

        public static IShape ToShape(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, Node item)
        {
            double cx, cy, rx, ry;
            Style curstyle;
            cx = item.Attributes.GetNamedItem("cx") != null
                ? double.Parse(item.Attributes.GetNamedItem("cx").Value)
                : 0.0;
            cy = item.Attributes.GetNamedItem("cy") != null
                ? double.Parse(item.Attributes.GetNamedItem("cy").Value)
                : 0.0;
            rx = item.Attributes.GetNamedItem("rx") != null
                ? double.Parse(item.Attributes.GetNamedItem("rx").Value)
                : 0.0;
            ry = item.Attributes.GetNamedItem("ry") != null
                ? double.Parse(item.Attributes.GetNamedItem("ry").Value)
                : 0.0;
            if (item.Attributes.GetNamedItem("class") != null)
            {
                var stylename = item.Attributes.GetNamedItem("class").Value;
                if (styledict.ContainsKey(stylename))
                {
                    curstyle = styledict[stylename];
                }
                else
                {
                    curstyle = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
                }
            }
            else
            {
                curstyle = item.Attributes.GetNamedItem("style") != null ? Style.ToStyle(top, style, styledict, item.Attributes.GetNamedItem("style").Value) : style.Peek();
            }

            if (item.Attributes.GetNamedItem("transform") != null)
            {
                ;
            }
            return new Ellipse
            {
                CX = cx,
                CY = cy,
                RX = rx,
                RY = ry,
                Style = curstyle
            };
        }
    }

    public class SVGToManim
    {
        internal static Dictionary<string, int> _names = new Dictionary<string, int>();

        internal static string Indent(int indent)
        {
            string retval = "";
            for (int i = 0; i < indent; i++)
            {
                retval += "   ";
            }

            return retval;
        }

        internal static string UniqueName(string prefix)
        {
            if (!_names.ContainsKey(prefix))
            {
                _names.Add(prefix, 1);
            }

            string name = $"{prefix}{_names[prefix]}";
            _names[prefix] = _names[prefix] + 1;
            return name;
        }
        internal static void ProcessSVGItem(Stack<Group> top, Stack<Style> style, Dictionary<string, Style> styledict, Node item)
        {
            switch (item.NodeName)
            {
                case "svg":
                    break;
                case "g":
                    top.Peek().Add(Group.ToShape(top, style, styledict, item));
                    break;
                case "style":
                    style.Push(Style.ToStyle(top, style, styledict, item));
                    break;
                case "rect":
                    top.Peek().Add(Rect.ToShape(top, style, styledict, item));
                    break;
                case "circle":
                    top.Peek().Add(Circle.ToShape(top, style, styledict, item));
                    break;
                case "ellipse":
                    top.Peek().Add(Ellipse.ToShape(top, style, styledict, item));
                    break;
                case "text":
                    break;
                case "polygon":
                    top.Peek().Add(Polygon.ToShape(top, style, styledict, item));
                    break;
                case "path":
                    top.Peek().Add(Path.ToShape(top, style, styledict, item));
                    break;
                default:
                    Console.WriteLine(item);
                    break;

            }
        }

        public static IShape ReadSVG(string filename)
        {
            var root = new Group();
            var top = new Stack<Group>();
            var style = new Stack<Style>();
            var styledict = new Dictionary<string, Style>();
            top.Push(root);
            style.Push(new Style());
            using (var document = new SVGDocument(filename))
            {
                using (var iterator = document.CreateNodeIterator(document, NodeFilter.SHOW_ALL))
                {
                    var item = iterator.NextNode();
                    while (item != null)
                    {
                        ProcessSVGItem(top, style, styledict, item);
                        item = iterator.NextNode();
                    }
                }
                return root;
            }
        }
    }

}
