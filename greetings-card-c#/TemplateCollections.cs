using System.Drawing;

namespace greetings_card;

public class Template
{
    public string message { get; set; }
    public int[] point { get; set; }
    public string pathToIMG { get; set; }
    public string colorForeground { get; set; }
    public int fontSize { get; set; }
    public StringAlignment alignment { get; set; }
    public string fontFamily { get; set; }
}