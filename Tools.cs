using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

public struct TekenObject {
    public String tekenTool { get; set; }
    public int P1X { get; set; }
	public int P1Y { get; set; }
	public int P2X { get; set; }
	public int P2Y { get; set; }

	public char c { get; set; }
	public Color Color { get; set; }

	public TekenObject(String tekenTool, Point P1, Point P2, Color Color) {
        this.tekenTool = tekenTool;
        this.P1X = P1.X;
        this.P1Y = P1.Y;
        this.P2X = P2.X;
        this.P2Y = P2.Y;
        this.c = ' ';
        this.Color = Color;
    }

	public TekenObject(String tekenTool, Point P1,  char c, Color Color) {
        this.tekenTool = tekenTool;
        this.c = c;
		this.P1X = P1.X;
		this.P1Y = P1.Y;
		this.P2X = P1.X;
		this.P2Y = P1.Y;
		this.Color = Color;
    }
}
public interface ISchetsTool
{
    public void MuisVast(SchetsControl s, Point p) { }
    public void MuisDrag(SchetsControl s, Point p) { }
	public void MuisLos(SchetsControl s, Point p) { }
	public void Letter(SchetsControl s, char c) { }
	public bool Hit(Point p2, int Margin) { return false; }
	public void Erase(Dictionary<string, Func<SchetsControl, Point, Point, Point, int, char, Color, bool>> SchetsTools, List<TekenObject> objects, Point loc, SchetsWin s) { }
}

public abstract class StartpuntTool : ISchetsTool
{
	public Point startpunt;
	public Point eindpunt;
	public Brush kwast;

    public virtual void MuisVast(SchetsControl s, Point p)
    {
        startpunt = p;
		kwast = new SolidBrush(s.PenKleur);
	}
    public virtual void MuisLos(SchetsControl s, Point p)
    {

    }
    public void Reset(Point P1, Point p2, Color c) {
        startpunt = P1;
        eindpunt = p2;
        kwast = new SolidBrush(c);
    }
    public abstract void MuisDrag(SchetsControl s, Point p);
    public abstract void Letter(SchetsControl s, char c);

    public void Erase(Dictionary<string, Func<SchetsControl, Point, Point, Point, int, char, Color, bool>> SchetsTools, List<TekenObject> objects, Point loc, SchetsWin s) { }
}

public class TekstTool : StartpuntTool
{
    SizeF sz = new SizeF(0, 0);
    public override string ToString() { return "tekst"; }

    public override void MuisDrag(SchetsControl s, Point p) { }

    public override void Letter(SchetsControl s, char c)
    {
        if (c >= 32)
        {
            Graphics gr = s.MaakBitmapGraphics();
            Font font = new Font("Tahoma", 40);
            string tekst = c.ToString();
            sz = gr.MeasureString(tekst, font, this.startpunt, StringFormat.GenericTypographic);
            gr.DrawString(tekst, font, kwast,
                                            this.startpunt, StringFormat.GenericTypographic);
            // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
            startpunt.X += (int)sz.Width;
			s.Invalidate();
        }
    }

	public static bool Hit(SchetsControl s, Point p1, Point p2, Point loc, int Margin, char ch, Color c) {
        Graphics gr = s.MaakBitmapGraphics();
		GraphicsPath path = new GraphicsPath();
		Font font = new Font("Tahoma", 40);
		string tekst = ch.ToString();
		SizeF sz = gr.MeasureString(tekst, font, p1, StringFormat.GenericTypographic);
		path.AddRectangle(new RectangleF(p1, sz));
        return path.IsVisible(loc);
	}

	public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
		Graphics gr = s.MaakBitmapGraphics();
		Font font = new Font("Tahoma", 40);
		string tekst = ch.ToString();
		gr.DrawString(tekst, font, new SolidBrush(c), p1, StringFormat.GenericTypographic);
		// gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
		s.Invalidate();
	}
}

public abstract class TweepuntTool : StartpuntTool {
    public static Rectangle Punten2Rechthoek(Point p1, Point p2) {
        return new Rectangle(new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y))
                            , new Size(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y))
                            );
    }
    public static Pen MaakPen(Brush b, int dikte) {
        Pen pen = new Pen(b, dikte);
        pen.StartCap = LineCap.Round;
        pen.EndCap = LineCap.Round;
        return pen;
    }
    public override void MuisVast(SchetsControl s, Point p) {
        base.MuisVast(s, p);
    }
    public override void MuisDrag(SchetsControl s, Point p) {
        s.Refresh();
        this.Bezig(s.CreateGraphics(), this.startpunt, p);
    }
    public override void MuisLos(SchetsControl s, Point p) {
        eindpunt = p;
        base.MuisLos(s, p);
        this.Compleet(s.MaakBitmapGraphics(), this.startpunt, p);
        s.Invalidate();
    }
    public override void Letter(SchetsControl s, char c) {
    }
    public abstract void Bezig(Graphics g, Point p1, Point p2);

    public virtual void Compleet(Graphics g, Point p1, Point p2) {
        this.Bezig(g, p1, p2);
    }
}

public class RechthoekTool : TweepuntTool {
    public override string ToString() { return "kader"; }

    public override void Bezig(Graphics g, Point p1, Point p2) {
        g.DrawRectangle(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
    }

	public static bool Hit(SchetsControl s, Point p1, Point p2, Point loc, int Margin, char ch, Color c) {
		GraphicsPath path = new GraphicsPath();
        path.AddRectangle(TweepuntTool.Punten2Rechthoek(p1, p2));
        return path.IsOutlineVisible(loc, MaakPen(new SolidBrush(c), 3 + 2 * Margin));
    }
	public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
		s.MaakBitmapGraphics().DrawRectangle(MaakPen(new SolidBrush(c), 3), TweepuntTool.Punten2Rechthoek(p1, p2));
		s.Invalidate();
	}
}

public class VolRechthoekTool : RechthoekTool {
	public override string ToString() { return "vlak"; }

	public override void Compleet(Graphics g, Point p1, Point p2) {
		g.FillRectangle(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
	}

	public static bool Hit(SchetsControl s, Point p1, Point p2, Point loc, int Margin, char ch, Color c) {
		GraphicsPath path = new GraphicsPath();
		path.AddRectangle(TweepuntTool.Punten2Rechthoek(p1, p2));
		return path.IsVisible(loc);
	}
	new public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
		s.MaakBitmapGraphics().FillRectangle(new SolidBrush(c), TweepuntTool.Punten2Rechthoek(p1, p2));
		s.Invalidate();
	}
}

public class OvaalTool : TweepuntTool {
	public override string ToString()  { return "ovaal"; }

	public override void Bezig(Graphics g, Point p1, Point p2) {
        g.DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1,p2));
	}
	public static bool Hit(SchetsControl s, Point p1, Point p2, Point loc, int Margin, char ch, Color c) {
		GraphicsPath path = new GraphicsPath();
		path.AddEllipse(TweepuntTool.Punten2Rechthoek(p1, p2));
		return path.IsOutlineVisible(loc, MaakPen(new SolidBrush(c), 3 + 2 * Margin));
	}
	public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
		s.MaakBitmapGraphics().DrawEllipse(MaakPen(new SolidBrush(c), 3), TweepuntTool.Punten2Rechthoek(p1, p2));
		s.Invalidate();
	}
}

public class VolOvaalTool : TweepuntTool {
	public override string ToString() { return "cirkel"; }

	public override void Bezig(Graphics g, Point p1, Point p2) {
		g.FillEllipse(kwast, TweepuntTool.Punten2Rechthoek(p1, p2));
	}

	public static bool Hit(SchetsControl s, Point p1, Point p2, Point loc, int Margin, char ch, Color c) {
		GraphicsPath path = new GraphicsPath();
		path.AddEllipse(TweepuntTool.Punten2Rechthoek(p1, p2));
		return path.IsVisible(loc);
	}
	public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
		s.MaakBitmapGraphics().FillEllipse(new SolidBrush(c), TweepuntTool.Punten2Rechthoek(p1, p2));
		s.Invalidate();
	}

}

public class TriangleTool : TweepuntTool {
	public override string ToString() { return "3hoek"; }

	public override void Bezig(Graphics g, Point p1, Point p2) {
        g.DrawPolygon(MaakPen(kwast, 3), new Point[] { p2, new Point(p1.X, p2.Y), new Point(p1.X + (p2.X - p1.X) / 2, p1.Y) });
	}
	public static bool Hit(SchetsControl s, Point p1, Point p2, Point loc, int Margin, char ch, Color c) {
		GraphicsPath path = new GraphicsPath();
        path.AddPolygon(new Point[] { p2, new Point(p1.X, p2.Y), new Point(p1.X + (p2.X - p1.X) / 2, p1.Y) });
		return path.IsOutlineVisible(loc, MaakPen(new SolidBrush(c), 3 + 2 * Margin));
	}

	public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
		s.MaakBitmapGraphics().DrawPolygon(MaakPen(new SolidBrush(c), 3), new Point[] { p2, new Point(p1.X, p2.Y), new Point(p1.X + (p2.X - p1.X) / 2, p1.Y) });
		s.Invalidate();
	}
}

public class VolTriangleTool : TweepuntTool {
	public override string ToString() { return "vol 3hoek"; }

	public override void Bezig(Graphics g, Point p1, Point p2) {
		g.FillPolygon(kwast, new Point[] { p2, new Point(p1.X, p2.Y), new Point(p1.X + (p2.X - p1.X) / 2, p1.Y) });
	}
	public static bool Hit(SchetsControl s, Point p1, Point p2, Point loc, int Margin, char ch, Color c) {
		GraphicsPath path = new GraphicsPath();
		path.AddPolygon(new Point[] { p2, new Point(p1.X, p2.Y), new Point(p1.X + (p2.X - p1.X) / 2, p1.Y) });
		return path.IsVisible(loc);
	}

    public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
        s.MaakBitmapGraphics().FillPolygon(new SolidBrush(c), new Point[] { p2, new Point(p1.X, p2.Y), new Point(p1.X + (p2.X - p1.X) / 2, p1.Y) });
        s.Invalidate();
    }
}

public class LijnTool : TweepuntTool
{
    public override string ToString() { return "lijn"; }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        g.DrawLine(MaakPen(this.kwast, 3), p1, p2);
    }

	public static bool Hit(SchetsControl s, Point p1, Point p2, Point loc, int Margin, char ch, Color c) {
		GraphicsPath path = new GraphicsPath();
        path.AddLine(p1, p2);
		return path.IsOutlineVisible(loc, MaakPen(new SolidBrush(c), 3 + 2*Margin));
	}
    public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
		s.MaakBitmapGraphics().DrawLine(MaakPen(new SolidBrush(c), 3), p1, p2);
		s.Invalidate();
	}
}

public class PenTool : LijnTool
{
    public override string ToString() { return "pen"; }

    public override void MuisDrag(SchetsControl s, Point p) {
        this.Compleet(s.MaakBitmapGraphics(), this.startpunt, p);
        s.Invalidate();
        this.startpunt = p;
    }
	new public static void Draw(SchetsControl s, Point p1, Point p2, Color c, char ch) {
		s.MaakBitmapGraphics().DrawLine(MaakPen(new SolidBrush(c), 3), p1, p2);
		s.Invalidate();
	}
}

public class GumTool : ISchetsTool {
    public override string ToString() { return "gum"; }
    public void MuisVast(SchetsControl s, Point p) { }
    public void MuisDrag(SchetsControl s, Point p) { }
    public void MuisLos(SchetsControl s, Point p) { }
    public void Letter(SchetsControl s, char c) { }
    public void Reset(Point P1, Point p2, Color c) { }

    public bool Hit(Point p2, int Margin) { return false; }
    public void Erase(Dictionary<string, Func<SchetsControl, Point, Point, Point, int, char, Color, bool>> SchetsTools, List<TekenObject> objects, Point loc, SchetsWin s) {
        List<TekenObject> toDelete = [];
        foreach (TekenObject obj in objects) {
            if (SchetsTools[obj.tekenTool](s.schetscontrol, new Point(obj.P1X, obj.P1Y), new Point(obj.P2X, obj.P2Y), loc, 2, obj.c, obj.Color)) toDelete.Add(obj);
        }
        foreach (TekenObject obj in toDelete)
            objects.Remove(obj);
        s.Redraw();
    }
}
