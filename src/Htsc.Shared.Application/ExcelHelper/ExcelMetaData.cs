using System.Drawing;

namespace Htsc.Shared.ExcelHelper
{
    public class ExcelMetaData
    {
        public int Row
        {
            get;
            set;
        }

        public Color? BackGroundColor
        {
            get;
            set;
        }

        public Color? FontColor
        {
            get;
            set;
        }

        public int IndentLevel
        {
            get;
            set;
        }

        public bool WrapText
        {
            get;
            set;
        } = false;


        public bool ShrinkToFit
        {
            get;
            set;
        }

        public ExcelMetaData()
        {
            BackGroundColor = Color.White;
            FontColor = Color.Black;
        }
    }
}
