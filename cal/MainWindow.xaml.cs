using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Runtime.InteropServices;

namespace cal
{
    public partial class MainWindow : Window
    {
        String leftOp = "";
        String rightOp = "";
        String Operation = "";
        public static class StarScreenColorsHelper
        {
            [DllImport("uxtheme.dll", EntryPoint = "#98", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
            public static extern UInt32 GetImmersiveUserColorSetPreference(Boolean forceCheckRegistry, Boolean skipCheckOnFail);

            [DllImport("uxtheme.dll", EntryPoint = "#94", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
            public static extern UInt32 GetImmersiveColorSetCount();

            [DllImport("uxtheme.dll", EntryPoint = "#95", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
            public static extern UInt32 GetImmersiveColorFromColorSetEx(UInt32 immersiveColorSet, UInt32 immersiveColorType,
                Boolean ignoreHighContrast, UInt32 highContrastCacheMode);

            [DllImport("uxtheme.dll", EntryPoint = "#96", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
            public static extern UInt32 GetImmersiveColorTypeFromName(IntPtr name);

            [DllImport("uxtheme.dll", EntryPoint = "#100", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
            public static extern IntPtr GetImmersiveColorNamedTypeByIndex(UInt32 index);
        }

        public MainWindow()
        {

            InitializeComponent();
            IntPtr pElementName = Marshal.StringToHGlobalUni(ImmersiveColors.ImmersiveStartBackground.ToString());
            Color color1 = GetColor(pElementName);
            IntPtr pElementName1 = Marshal.StringToHGlobalUni(ImmersiveColors.ImmersiveStartSelectionBackground.ToString());
            Color color2 = GetColor(pElementName1);

            foreach (UIElement c in firstGrid.Children)
            {
                if (((Button)c).Name.ToString().Contains("d"))
                {
                    ((Button)c).Background = new SolidColorBrush(color1);
                }
                else if (((Button)c).Name.ToString().Contains("eq"))
                    ((Button)c).Background = new SolidColorBrush(Colors.Blue);
                else
                    ((Button)c).Background = new SolidColorBrush(color2);
                ((Button)c).Click += Button_Click;
            }
        }

        private static Color GetColor(IntPtr pElementName)
        {
            var colourset = StarScreenColorsHelper.GetImmersiveUserColorSetPreference(false, false);
            uint type = StarScreenColorsHelper.GetImmersiveColorTypeFromName(pElementName);
            Marshal.FreeCoTaskMem(pElementName);
            uint colourdword = StarScreenColorsHelper.GetImmersiveColorFromColorSetEx((uint)colourset, type, false, 0);
            byte[] colourbytes = new byte[4];
            colourbytes[0] = (byte)((0xFF000000 & colourdword) >> 24); // A
            colourbytes[1] = (byte)((0x00FF0000 & colourdword) >> 16); // B
            colourbytes[2] = (byte)((0x0000FF00 & colourdword) >> 8); // G
            colourbytes[3] = (byte)(0x000000FF & colourdword); // R
            Color color = Color.FromArgb(colourbytes[0], colourbytes[3], colourbytes[2], colourbytes[1]);
            return color;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String s = ((Button)e.OriginalSource).Content.ToString();
            if (s != "=" && s != "BackSpace" && s != Operation)
            {
                if (Operation != "" && Operation != s && s != "." && rightOp == "")
                {
                    int num1;
                    bool numOp1 = Int32.TryParse(s, out num1);
                    if (!numOp1)
                    {
                        String tmp1;
                        tmp1 = MyTextBox.Text;
                        tmp1 = tmp1.Substring(0, tmp1.Length - 1);
                        tmp1 += s;
                        Operation = "";
                        Operation = s;
                        MyTextBox.Text = tmp1;
                    }
                    else if (rightOp == "")
                        MyTextBox.Text += s;
                }
                else
                {
                    int num1;
                    bool numOp1 = Int32.TryParse(s, out num1);
                    if (!numOp1 && leftOp == "" || !numOp1 && rightOp !="")
                    {

                    }
                    else if (s != ".")
                        MyTextBox.Text += s;
                }
            } 
           // MessageBox.Show(s);
            //int j;
            //bool result = Int32.TryParse("-105", out j);
            //if (true == result)
            //    Console.WriteLine(j);
            //else
            //    Console.WriteLine("String could not be parsed.");
            int num;
            bool numOp = Int32.TryParse(s, out num);
            

            
            /////////////////////////
          
            if (numOp)
            {
            
                if (Operation == "")
                {
                        leftOp += s;
                }
                else
                {
                     rightOp += s;
                }

            }
         
            else
            {
               
                
                if (s == "=" && rightOp != "")
                {
                    Update_LeftOp();
                    MyTextBox.Text = "";
                    MyTextBox.Text += leftOp;
                    rightOp = "";
                    Operation = "";
                    
                }
                else if (s == ".")
                {
                    if (Operation == "")
                    {
                        if (leftOp != "" && !leftOp.Contains("."))
                        {
                            leftOp += s;
                            MyTextBox.Text += s;
                        }
                       
                    }
                    else
                    {
                        if (rightOp != "" && !rightOp.Contains("."))
                        {
                            rightOp += s;
                            MyTextBox.Text += s;
                        }
                    }
                }
               
                else if (s == "C")
                {
                    leftOp = "";
                    rightOp = "";
                    Operation = "";
                    MyTextBox.Text = "";
                }
                else if (s == "BackSpace")
                {
                    if (leftOp != "")
                    {
                        String tmp = MyTextBox.Text;
                        tmp = tmp.Substring(0, tmp.Length - 1);
                        MyTextBox.Text = tmp;
                        if (rightOp != "")
                        {
                            rightOp = rightOp.Substring(0, rightOp.Length - 1);
                        }
                        else if (Operation != "")
                        {
                            Operation = "";
                        }
                        else if (leftOp != "")
                        {
                            leftOp = leftOp.Substring(0, leftOp.Length - 1);
                        }
                    }
                }
               
                else
                {

                    if (leftOp != "" && s != "=" && s!="." && Operation =="")
                    {

                       
                        Operation = s;
                    }
                }
            }
           
        }


        private void Update_LeftOp()
        {
            float Left = Convert.ToSingle(leftOp);
            float Right = Convert.ToSingle(rightOp);
            switch (Operation)
            {
                case "+":
                    leftOp = (Left + Right).ToString();
                    break;
                case "-":
                    leftOp = (Left - Right).ToString();
                    break;
                case "*":
                    leftOp = (Left * Right).ToString();
                    break;
                case "/":
                    leftOp = (Left / Right).ToString();
                    break;
            }
        }
    }
}
