using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms.BarCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string strBarCode = "www.cnblogs.com";
            pictureBox1.Image = GetCode39(strBarCode);
        }

        /// <summary>
        /// Gets the code39.
        /// </summary>
        /// <param name="sourceCode">The source code.</param>
        /// <returns>Bitmap.</returns>
        public Bitmap GetCode39(string sourceCode)
        {
            int leftMargin = 5;
            int topMargin = 0;
            int thickLength = 2;
            int narrowLength = 1;
            int barCodeHeight = 35;
            int intSourceLength = sourceCode.Length;
            string strEncode = "010010100"; //添加起始码“*”.
            var font = new System.Drawing.Font("Segoe UI", 5);

            string AlphaBet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

            string[] Code39 =
            {
                /* 0 */ "000110100",  
                /* 1 */ "100100001",  
                /* 2 */ "001100001",  
                /* 3 */ "101100000",
                /* 4 */ "000110001",  
                /* 5 */ "100110000",  
                /* 6 */ "001110000",  
                /* 7 */ "000100101",
                /* 8 */ "100100100",  
                /* 9 */ "001100100",  
                /* A */ "100001001",  
                /* B */ "001001001",
                /* C */ "101001000",  
                /* D */ "000011001",  
                /* E */ "100011000",  
                /* F */ "001011000",
                /* G */ "000001101",  
                /* H */ "100001100",  
                /* I */ "001001100",  
                /* J */ "000011100",
                /* K */ "100000011",  
                /* L */ "001000011",  
                /* M */ "101000010",  
                /* N */ "000010011",
                /* O */ "100010010",  
                /* P */ "001010010",  
                /* Q */ "000000111",  
                /* R */ "100000110",
                /* S */ "001000110",  
                /* T */ "000010110",  
                /* U */ "110000001",  
                /* V */ "011000001",
                /* W */ "111000000",  
                /* X */ "010010001",  
                /* Y */ "110010000",  
                /* Z */ "011010000",
                /* - */ "010000101",  
                /* . */ "110000100",  
                /*' '*/ "011000100",
                /* $ */ "010101000",
                /* / */ "010100010",  
                /* + */ "010001010",  
                /* % */ "000101010",  
                /* * */ "010010100"
            };
            sourceCode = sourceCode.ToUpper();

            Bitmap objBitmap = new Bitmap(
              ((thickLength * 3 + narrowLength * 7) * (intSourceLength + 2)) + (leftMargin * 2),
              barCodeHeight + (topMargin * 2));
            Graphics objGraphics = Graphics.FromImage(objBitmap);

            objGraphics.FillRectangle(Brushes.White, 0, 0, objBitmap.Width, objBitmap.Height);

            for (int i = 0; i < intSourceLength; i++)
            {
                //非法字符校验
                if (AlphaBet.IndexOf(sourceCode[i]) == -1 || sourceCode[i] == '*')
                {
                    objGraphics.DrawString("Invalid Bar Code",
                      SystemFonts.DefaultFont, Brushes.Red, leftMargin, topMargin);
                    return objBitmap;
                }
                //编码
                strEncode = string.Format("{0}0{1}", strEncode,
                 Code39[AlphaBet.IndexOf(sourceCode[i])]);
            }

            strEncode = string.Format("{0}0010010100", strEncode); //添加结束码“*”

            int intEncodeLength = strEncode.Length;
            int intBarWidth;

            for (int i = 0; i < intEncodeLength; i++) //绘制 Code39 barcode
            {
                intBarWidth = strEncode[i] == '1' ? thickLength : narrowLength;
                objGraphics.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White,
                 leftMargin, topMargin, intBarWidth, barCodeHeight);
                leftMargin += intBarWidth;
            }

            //绘制 明码
            SizeF sizeF = objGraphics.MeasureString(sourceCode, font);
            float x = (objBitmap.Width - sizeF.Width) / 2;
            float y = objBitmap.Height - sizeF.Height;
            objGraphics.FillRectangle(Brushes.White, x, y, sizeF.Width, sizeF.Height);
            objGraphics.DrawString(sourceCode, font, Brushes.Black, x, y);

            return objBitmap;
        }
    }
}
