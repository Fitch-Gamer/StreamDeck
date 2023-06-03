using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Diagnostics;

namespace StreamDeck
{
    public partial class Deck : Form
    {
        public int borderx = 10, bordery = 10, butborderx = 20, butbordery = 20, butx = 100, buty = 100;
        public Deck()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Main(borderx,bordery,butborderx,butbordery,butx,buty,"Test.json");
        }

        private void Main(int BorderWidthX,int BorderWidthY,int ButBorderWidthX,int ButBorderWidthY, int ButSizeX, int ButSizeY, string StartLoc)
        {
            ButtonLocs Buttons = JsonConvert.DeserializeObject<ButtonLocs>(File.ReadAllText(StartLoc));
            int ButYNum = Buttons.ButtonLoc.Length;
            int ButXNum = Buttons.ButtonLoc[0].Length;
            Size = new Size((2*BorderWidthX) + (ButSizeX*ButXNum) + (ButBorderWidthX*(ButXNum-1)), (2 * BorderWidthY) + (ButSizeY * ButYNum) + (ButBorderWidthY * (ButYNum - 1)));
            int x = 0;
            int y = 0;
            foreach(ButtonProps[] BPA in Buttons.ButtonLoc)
            {
                foreach(ButtonProps BP in Buttons.ButtonLoc[y])
                {
                    Button b = new Button();
                    b.Size = new Size(ButSizeX, ButSizeY);
                    b.Location = new Point(BorderWidthX + ((ButBorderWidthX + ButSizeX) * x), BorderWidthY + ((ButBorderWidthY + ButSizeY) * y));
                    b.BackgroundImageLayout = ImageLayout.Zoom;
                    b.BackgroundImage = Image.FromFile(BP.Img);
                    b.Tag = "BP";
                    b.Enabled = true;
                    b.Visible = true;
                    b.Click += (sender,EventArgs) => { ButtonLink(BP.Type, BP.Link);};
                    Controls.Add(b);

                    x++;
                }
                x = 0;
                y++;
            }
        }

        private void ButtonLink(string Type, string Link)
        {
            switch (Type)
            {
                case "url":
                    Process.Start(Link);
                    break;

                case "urlrequest":
                    break;

                case "executable":
                    Process.Start(Link);
                    break;
                case "link":
                    List<Button> ct = Controls.OfType<Button>().ToList();
                    foreach(Button b in ct)
                    {
                        if(b.Tag.ToString() == "BP")
                        {
                            b.Dispose();
                        }
                    }
                    Main(borderx, bordery, butborderx, butbordery, butx, buty, Link);
                    break;

                default:
                    break;
            }
        }

        public class ButtonLocs
        {
           public ButtonProps[][] ButtonLoc { get; set; }
        }
        public class ButtonProps
        {
            public string Type { get; set; }
            public string Link { get; set; }
            public string Img { get; set; }
        }
    }
}