using JSON;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows.Forms;
using static TweepuntTool;

public class SchetsWin : Form {
	MenuStrip menuStrip;
	public SchetsControl schetscontrol;
	ISchetsTool huidigeTool;
	Panel paneel;
	bool vast;
	Point p;
	Point moveBuffer;

	public Dictionary<string,  Action<SchetsControl, Point, Point, Color, char>> SchetsTools = new() {
		{"tekst", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => TekstTool.Draw(s, p1, p2, c, ch))},
		{"kader", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => RechthoekTool.Draw(s, p1, p2, c, ch))},
		{"vlak", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => VolRechthoekTool.Draw(s, p1, p2, c, ch))},
		{"ovaal", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => OvaalTool.Draw(s, p1, p2, c, ch))},
		{"cirkel", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => VolOvaalTool.Draw(s, p1, p2, c, ch))},
		{"3hoek", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => TriangleTool.Draw(s, p1, p2, c, ch))},
		{"vol 3hoek", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => VolTriangleTool.Draw(s, p1, p2, c, ch))},
		{"lijn", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => LijnTool.Draw(s, p1, p2, c, ch))},
		{"pen", new Action<SchetsControl, Point, Point, Color, char>(static (s, p1, p2, c, ch) => PenTool.Draw(s, p1, p2, c, ch))},
	};
	public Dictionary<string, Func<SchetsControl, Point, Point, Point, int, char, Color, bool>> SchetsToolsE = new() {
		{"tekst", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => TekstTool.Hit(s, p1, p2, loc, margin, ch, c))},
		{"kader", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => RechthoekTool.Hit(s, p1, p2, loc, margin, ch, c))},
		{"vlak", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => VolRechthoekTool.Hit(s, p1, p2, loc, margin, ch, c))},
		{"ovaal", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => OvaalTool.Hit(s, p1, p2, loc, margin, ch, c))},
		{"cirkel", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => VolOvaalTool.Hit(s, p1, p2, loc, margin, ch, c))},
		{"3hoek", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => TriangleTool.Hit(s, p1, p2, loc, margin, ch, c))},
		{"vol 3hoek", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => VolTriangleTool.Hit(s, p1, p2, loc, margin, ch, c))},
		{"lijn", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => LijnTool.Hit(s, p1, p2, loc, margin, ch, c))},
		{"pen", new Func<SchetsControl, Point, Point, Point, int, char, Color, bool>(static (s, p1, p2, loc, margin, ch, c) => PenTool.Hit(s, p1, p2, loc, margin, ch, c))},
	};

	public List<TekenObject> geTekendeObjecten = [];
	private void veranderAfmeting(object o, EventArgs ea) {
		schetscontrol.Size = new Size(this.ClientSize.Width - 70
									  , this.ClientSize.Height - 50);
		paneel.Location = new Point(64, this.ClientSize.Height - 30);
	}

	private void klikToolMenu(object obj, EventArgs ea) {
		this.huidigeTool = (ISchetsTool)((ToolStripMenuItem)obj).Tag;
	}

	private void klikToolButton(object obj, EventArgs ea) {
		this.huidigeTool = (ISchetsTool)((RadioButton)obj).Tag;
	}

	private void afsluiten(object obj, EventArgs ea) {
		this.Close();
	}

	public SchetsWin() {
		ISchetsTool[] deTools = { new PenTool()
								, new LijnTool()
								, new RechthoekTool()
								, new VolRechthoekTool()
								, new OvaalTool()
								, new VolOvaalTool()
								, new TriangleTool()
								, new VolTriangleTool()
								, new TekstTool()
								, new GumTool()
								};
		String[] deKleuren = { "Black", "Red", "Green", "Blue", "Yellow", "Magenta", "Cyan" };

		this.ClientSize = new Size(700, 500);
		huidigeTool = deTools[0];

		schetscontrol = new SchetsControl();
		schetscontrol.Location = new Point(64, 10);
		schetscontrol.MouseDown += (object o, MouseEventArgs mea) => {
			vast = true;
			p = mea.Location;
			moveBuffer = mea.Location;
			huidigeTool.MuisVast(schetscontrol, mea.Location);
		};
		schetscontrol.MouseMove += (object o, MouseEventArgs mea) => {
			if (vast) {
				huidigeTool.MuisDrag(schetscontrol, mea.Location);
				if (huidigeTool.ToString() == "gum") huidigeTool.Erase(SchetsToolsE, geTekendeObjecten, mea.Location, this);
				else if (huidigeTool.ToString() == "pen") {
					geTekendeObjecten.Add(new TekenObject(huidigeTool.ToString(), moveBuffer, mea.Location, schetscontrol.PenKleur));
					moveBuffer = mea.Location;
				}
			}
		};
		schetscontrol.MouseUp += (object o, MouseEventArgs mea) => {
			if (vast) {
				huidigeTool.MuisLos(schetscontrol, mea.Location);
				if (huidigeTool.ToString() != "pen" && huidigeTool.ToString() != "gum" && huidigeTool.ToString() != "tekst") {
					geTekendeObjecten.Add(new TekenObject(huidigeTool.ToString(), p, mea.Location, schetscontrol.PenKleur));
				}
			}
			
			vast = false;
		};

		schetscontrol.KeyDown += (object o, KeyEventArgs kpea) => {
			if (kpea.KeyCode == Keys.Z && kpea.Control && geTekendeObjecten.Count != 0) {
				if (geTekendeObjecten[geTekendeObjecten.Count - 1].tekenTool.ToString() == "pen") {
					List<int> toDelete = [];
					for (int i = geTekendeObjecten.Count - 1; i >= 0; i--) {
						if (geTekendeObjecten[i].tekenTool.ToString() == "pen") toDelete.Add(i);
						else break;
					}
					foreach (int i in toDelete) geTekendeObjecten.RemoveAt(i);
				}
				else {
					geTekendeObjecten.RemoveAt(geTekendeObjecten.Count - 1);
				}
				Redraw();
			}
			else if (huidigeTool.ToString() == "tekst" && !kpea.Control) {
				KeysConverter converter = new KeysConverter();
				geTekendeObjecten.Add(new TekenObject("tekst", p, converter.ConvertToString(kpea.KeyCode)[0], schetscontrol.PenKleur));
				huidigeTool.Letter(schetscontrol, converter.ConvertToString(kpea.KeyCode)[0]);

				Graphics gr = schetscontrol.MaakBitmapGraphics();
				Font font = new Font("Tahoma", 40);
				string tekst = converter.ConvertToString(kpea.KeyCode)[0].ToString();
				SizeF sz = gr.MeasureString(tekst, font, p, StringFormat.GenericTypographic);
				p.X += (int)sz.Width;
			}
		};

		this.Controls.Add(schetscontrol);

		menuStrip = new MenuStrip();
		menuStrip.Visible = false;
		this.Controls.Add(menuStrip);
		this.maakFileMenu();
		this.maakToolMenu(deTools);
		this.maakActieMenu(deKleuren);
		this.maakToolButtons(deTools);
		this.maakActieButtons(deKleuren);
		this.maakExportButtons();
		this.Resize += this.veranderAfmeting;
		this.veranderAfmeting(null, null);
	}
	public void Redraw() {
		schetscontrol.Schets.Schoon();
		if (geTekendeObjecten.Count == 0) return;
		foreach (TekenObject t in geTekendeObjecten) {
			SchetsTools[t.tekenTool](schetscontrol, new Point(t.P1X, t.P1Y), new Point(t.P2X, t.P2Y), t.Color, t.c);
		}
		schetscontrol.Invalidate();
	}
	private void Rotate(object o, EventArgs ea) {
		Size sz = schetscontrol.Schets.sz;
		double xPercent = (double)sz.Height / (double)sz.Width;
		for (int i = 0; i < geTekendeObjecten.Count; i++) {
			TekenObject temp = geTekendeObjecten[i];
			double yPercent = (double)temp.P1Y / (double)sz.Height;
			Point temp1 = new Point((int)((1 - yPercent) * sz.Width), (int)(temp.P1X * xPercent));
			int xChange = (temp.P1Y > temp.P2Y) ? Math.Abs(temp.P1Y - temp.P2Y) : -Math.Abs(temp.P1Y - temp.P2Y);
			int yChange = (temp.P1X < temp.P2X) ? Math.Abs(temp.P1X - temp.P2X) : -Math.Abs(temp.P1X - temp.P2X);
			Point temp2 = new Point(temp1.X + (int)(xChange * (1 / xPercent)), temp1.Y + (int)(yChange * xPercent));
			temp.P1X = temp1.X;
			temp.P1Y = temp1.Y;
			temp.P2X = temp2.X;
			temp.P2Y = temp2.Y;
			geTekendeObjecten[i] = temp;
		}
		Redraw();
	}
	private void maakFileMenu() {
		ToolStripMenuItem menu = new ToolStripMenuItem("File");
		menu.MergeAction = MergeAction.MatchOnly;
		menu.DropDownItems.Add("Sluiten", null, this.afsluiten);
		menuStrip.Items.Add(menu);
	}
	private void maakToolMenu(ICollection<ISchetsTool> tools) {
		ToolStripMenuItem menu = new ToolStripMenuItem("Tool");
		foreach (ISchetsTool tool in tools) {
			ToolStripItem item = new ToolStripMenuItem();
			item.Tag = tool;
			item.Text = tool.ToString();
			item.Image = new Bitmap($"../../../Icons/{tool.ToString()}.png");
			item.Click += this.klikToolMenu;
			menu.DropDownItems.Add(item);
		}
		menuStrip.Items.Add(menu);
	}
	private void maakActieMenu(String[] kleuren) {
		ToolStripMenuItem menu = new ToolStripMenuItem("Actie");
		menu.DropDownItems.Add("Clear", null, (object o, EventArgs ea) => {
			geTekendeObjecten.Clear();
			schetscontrol.Schoon(o, ea);
		});
		menu.DropDownItems.Add("Roteer", null, Rotate);
		ToolStripMenuItem submenu = new ToolStripMenuItem("Kies kleur");
		foreach (string k in kleuren)
			submenu.DropDownItems.Add(k, null, schetscontrol.VeranderKleurViaMenu);
		menu.DropDownItems.Add(submenu);
		menuStrip.Items.Add(menu);
	}
	private void maakActieButtons(String[] kleuren) {
		paneel = new Panel(); this.Controls.Add(paneel);
		paneel.Size = new Size(600, 24);

		Button clear = new Button(); paneel.Controls.Add(clear);
		clear.Text = "Clear";
		clear.Location = new Point(0, 0);
		clear.Click += (object o, EventArgs ea) => {
			geTekendeObjecten.Clear();
			schetscontrol.Schoon(o, ea); 
		};

		Button rotate = new Button(); paneel.Controls.Add(rotate);
		rotate.Text = "Rotate";
		rotate.Location = new Point(80, 0);
		rotate.Click += Rotate;

		Label penkleur = new Label(); paneel.Controls.Add(penkleur);
		penkleur.Text = "Penkleur:";
		penkleur.Location = new Point(180, 3);
		penkleur.AutoSize = true;

		ComboBox cbb = new ComboBox(); paneel.Controls.Add(cbb);
		cbb.Location = new Point(240, 0);
		cbb.DropDownStyle = ComboBoxStyle.DropDownList;
		cbb.SelectedValueChanged += schetscontrol.VeranderKleur;
		foreach (string k in kleuren)
			cbb.Items.Add(k);
		cbb.SelectedIndex = 0;
	}
	private void maakToolButtons(ICollection<ISchetsTool> tools) {
		int t = 0;
		foreach (ISchetsTool tool in tools) {
			RadioButton b = new RadioButton();
			b.Appearance = Appearance.Button;
			b.Size = new Size(50, 62);
			b.Location = new Point(5, 10 + t * 62);
			b.Tag = tool;
			b.Text = tool.ToString();
			b.Image = new Bitmap($"../../../Icons/{tool.ToString()}.png");
			b.TextAlign = ContentAlignment.TopCenter;
			b.ImageAlign = ContentAlignment.BottomCenter;
			b.Click += this.klikToolButton;
			this.Controls.Add(b);
			if (t == 0) b.Select();
			t++;
		}
	}
	private void maakExportButtons() {
		ToolStripMenuItem menu = new ToolStripMenuItem("Exporteren/Importeren");
		menu.DropDownItems.Add("Exporteer afbeelding(PNG)", null, (object o, EventArgs ea) => {
			SaveFileDialog f = new() {
				Filter = "Png Image| *.png",
				Title = "Save Current Image"
			};
			f.ShowDialog();

			if (f.FileName != "") {
				FileStream fs = (FileStream)f.OpenFile();
				schetscontrol.Schets.bitmap.Save(fs, ImageFormat.Png);
				MessageBox.Show("File saved succesfully", "Png Image Export");
				fs.Close();
			}
		});
		menu.DropDownItems.Add("Exporteer afbeelding(JPG)", null, (object o, EventArgs ea) => {
			SaveFileDialog f = new() {
				Filter = "Jpeg Image| *.jpg",
				Title = "Save Current Image"
			};
			f.ShowDialog();

			if (f.FileName != "") {
				FileStream fs = (FileStream)f.OpenFile();
				schetscontrol.Schets.bitmap.Save(fs, ImageFormat.Jpeg);
				MessageBox.Show("File saved succesfully", "Jpeg Image Export");
				fs.Close();
			}
		});
		menu.DropDownItems.Add("Exporteer afbeelding(BMP)", null, (object o, EventArgs ea) => {
			SaveFileDialog f = new() {
				Filter = "Bitmap Image| *.bmp",
				Title = "Save Current Image"
			};
			f.ShowDialog();

			if (f.FileName != "") {
				FileStream fs = (FileStream)f.OpenFile();
				schetscontrol.Schets.bitmap.Save(fs, ImageFormat.Bmp);
				MessageBox.Show("File saved succesfully", "Bitmap Image Export");
				fs.Close();
			}
		});
		menu.DropDownItems.Add("Exporteer Schets", null, (object o, EventArgs ea) => {
			SaveFileDialog f = new() {
				Filter = "Json Image| *.json",
				Title = "Sla huidige schets op"
			};
			f.ShowDialog();

			if (f.FileName != "") {
				FileStream fs = (FileStream)f.OpenFile();
				fs.Close();
				File.AppendAllText(fs.Name, JsonWriter.ToJson(geTekendeObjecten));
				MessageBox.Show("Schets succesvol opgeslagen", "Schets oplaan");
				fs.Close();
			}
		});
		menu.DropDownItems.Add("Importeer Schets", null, (object o, EventArgs ea) => {
			OpenFileDialog f = new() {
				Filter = "Json Image| *.json",
				Title = "laad schets in"
			};
			f.ShowDialog();

			if (f.FileName != "") {
				foreach (string line in File.ReadLines(f.FileName)) {
					geTekendeObjecten = JSON.JsonParser.FromJson<List<TekenObject>>(line);
				}
				Redraw();
			}
		});
		menuStrip.Items.Add(menu);

		//Button png = new Button(); paneel.Controls.Add(png);
		//png.Text = "Export to PNG";
		//png.Location = new Point(0, 0);
	}
}