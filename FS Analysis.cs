
using System;
//using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using S = System.Windows.MessageBox;

using VMS.CA.Scripting;

namespace VMS.DV.PD.Scripting
{
    public class Script
    {
        public Script()
        {

        }

        public void Execute(ScriptContext context)
        {

            //System.Windows.Forms.MessageBox.Show("s");
            PDBeam beam = context.PDBeam;
            Patient Pat = context.Patient;

            //S.Show(beam.Id);

            DoseImage pdi = context.DoseImage;
            Image i = pdi.Image;

            string date = i.CreationDateTime.ToString().Substring(0, 10);
            // S.Show(date);

            var NL = Environment.NewLine;
            string result = "";
            string temp = "";
            int cent0 = 0;
            int cent90 = 0;
            int cent270 = 0;


            PDSession sess = new PDSession();
            

            //DoseImage bdi = new DoseImage();
            //Image p = bdi.Image;



       



            foreach (Course c in Pat.Courses)
            {
                foreach (PlanSetup pln in c.PlanSetups)

                {
                    

                    foreach (Beam pb in pln.Beams.Reverse())
                    {

                        foreach (Image di in pb.FieldImages)
                        {
                            

                            if (di.CreationDateTime.ToString().Substring(0, 10) == date)


                            {
                                                                                              

                                HProf H = new HProf();
                                VProf V = new VProf();

                                //Jaw Linearity
                                HProf Hb = new HProf();

                                int x;



                                if (pb.Id.Contains("BOTTOM"))

                                {
                                    H.x1 = 348;
                                    H.x2 = 548;
                                    H.y1 = 734; //CENTRE
                                    H.y2 = 734; //THROUGH OPAQUE CROSS

                                    V.x1 = 460;
                                    V.x2 = 460;
                                    V.y1 = 636;
                                    V.y2 = 836;

                                }


                                if (pb.Id.Contains("TOP"))

                                {
                                    //H.x1 = 632;
                                    //H.x2 = 832;

                                    H.x1 = 348; //732 centre
                                    H.x2 = 1116;

                                    H.y1 = 435;
                                    H.y2 = 435;

                                    //
                                    V.x1 = 720;
                                    V.x2 = 720;
                                    //Range of vertical line around y=449
                                    V.y1 = 349;
                                    V.y2 = 549;

                                }



                                if (pb.Id.Contains("10x10"))

                                {

                                    //H.x1 = 395;
                                    H.x1 = 348;
                                    H.x2 = 842;
                                    //H.x2 = 795;
                                    H.y1 = 600;
                                    H.y2 = 600;

                                    V.x1 = 600;
                                    V.x2 = 600;
                                    V.y1 = 395;
                                    V.y2 = 795;

                                }


                                if (pb.Id.Contains("4x4"))

                                {

                                    H.x1 = 495;
                                    H.x2 = 695;
                                    H.y1 = 600;
                                    H.y2 = 600;

                                    V.x1 = 600;
                                    V.x2 = 600;
                                    V.y1 = 495;
                                    V.y2 = 695;

                                }

                                if (pb.Id.Contains("20x20"))

                                {

                                    H.x1 = 195;
                                    H.x2 = 995;
                                    H.y1 = 600;
                                    H.y2 = 600;

                                    V.x1 = 600;
                                    V.x2 = 600;
                                    V.y1 = 195;
                                    V.y2 = 995;

                                }


                                if (pb.Id.Contains("8x8"))

                                {
                                    H.x1 = 395;
                                    H.x2 = 795;
                                    H.y1 = 600;
                                    H.y2 = 600;

                                    V.x1 = 600;
                                    V.x2 = 600;
                                    V.y1 = 395;
                                    V.y2 = 795;

                                }

                                //double max = 0;

                                double scale = 0;
                                scale = (double)400 / 1190;


                                Frame f = di.Frames[0];
                                var HStart = new VVector(H.x1, H.y1, 0); //start location of profile
                                var HEnd = new VVector(H.x2, H.y2, 0);//end location of profile
                                var Hline = f.GetImageProfile(HStart, HEnd, new double[Convert.ToInt64(VVector.Distance(HStart, HEnd)) + 1]);


                                // Off the opaque marker for line bottom Left image
                                var HbStart = new VVector(H.x1, 720, 0); //start location of profile
                                var HbEnd = new VVector(H.x2, 720, 0);//end location of profile
                                var Hbline = f.GetImageProfile(HbStart, HbEnd, new double[Convert.ToInt64(VVector.Distance(HbStart, HbEnd)) + 1]);


                                var VStart = new VVector(V.x1, V.y1, 0); //start location of profile
                                var VEnd = new VVector(V.x2, V.y2, 0);//end location of profile
                                var Vline = f.GetImageProfile(VStart, VEnd, new double[Convert.ToInt64(VVector.Distance(VStart, VEnd)) + 1]);


                                ushort[,] pixels = new ushort[f.XSize, f.YSize];
                                f.GetVoxels(0, pixels);


                                var HLine = Hline.ToArray();
                                var VLine = Vline.ToArray();

                                //Generate a separate horizonal line for identifying max dose and field edges
                                var HbLine = Hline.ToArray();


                                if (pb.Id.Contains("BOTTOM"))
                                {
                                    //Horizontal 
                                    HbLine = Hbline.ToArray();


                                    Hb.len = Hb.x2 - Hb.x1;
                                    //Ensure length is even number
                                    if (Hb.len % 2 != 0)
                                    {
                                        Hb.len = Hb.len + 1;
                                    }

                                }

                                H.len = H.x2 - H.x1;
                                V.len = V.y2 - V.y1;


                                if (H.len % 2 != 0)
                                {
                                    H.len = H.len + 1;
                                }

                                if (V.len % 2 != 0)
                                {
                                    V.len = V.len + 1;
                                }


                                //Calculating A

                                double thisNum;

                                //Find max vlaue along Horizontal profile

                                double? maxVal = null; //nullable so this works even if you have all super-low negatives
                                int index = -1;
                                for (int k = 1; k < H.len; k++)
                                {
                                    thisNum = HLine[k].Value;
                                    if (!maxVal.HasValue || thisNum > maxVal.Value)
                                    {
                                        maxVal = thisNum;
                                        H.max = thisNum;
                                        index = k;
                                    }
                                }

                                // Find minimum vlaue along Horizontal profile

                                double? minVal = null; //nullable so this works even if you have all super-low negatives
                                index = -1;
                                for (int l = (H.len / 2) - 10; l < (H.len / 2) + 10; l++)
                                {
                                    thisNum = HLine[l].Value;


                                    if (!minVal.HasValue || thisNum < minVal.Value)
                                    {
                                        minVal = thisNum;
                                        H.min = thisNum;
                                        index = l;
                                    }
                                }

                                

                                //Find centre for bottom image- this should actually be maximum value as along missing bit of marker
                                if (pb.Id.Contains("BOTTOM"))

                                {
                                    double? BTmaxVal = null; //nullable so this works even if you have all super-low negatives
                                    index = -1;
                                    for (int k = 1; k < H.len; k++)
                                    {
                                        thisNum = HLine[k].Value;
                                        if (!BTmaxVal.HasValue || thisNum > BTmaxVal.Value)
                                        {
                                            BTmaxVal = thisNum;

                                            //Applies to origin
                                            H.min = thisNum;
                                            index = k;
                                        }
                                    }

                                    //Redraw Horizontal line off marker

                                }



                                //Position along line where minimum occurs
                                H.origin = index;

                                // int cent = H.origin;


                                /*
                                S.Show("cent=" + cent.ToString());

                                if (pb.Id.Contains("10x10"))
                                {
                                    cent = H.origin - 143;
                                }

                                */


                                if (pb.Id.Contains("TOP"))

                                    
                                {
                                    if (pln.Id.Contains("G=0"))


                                    {
                                        
                                        cent0 = H.origin - 285;
                                       
                                    }

                                    if (pln.Id.Contains("G=90"))
                                    {
                                        cent90 = H.origin - 285;
                                    }

                                    if (pln.Id.Contains("G=270"))
                                    {
                                        cent270 = H.origin - 285;
                                    }


                                }





                                if (pb.Id.Contains("BOTTOM"))


                                {
                                    if (pln.Id.Contains("G=0"))
                                    {
                                        
                                        H.origin = cent0;
                                        
                                    }

                                    if (pln.Id.Contains("G=90"))
                                    {
                                        H.origin = cent90;
                                    }

                                    if (pln.Id.Contains("G=270"))
                                    {
                                        H.origin = cent270;
                                    }
                                }





                                // H.cent = H.origin - 16;
                                //H.max = H.cent;

                                /*
                                if (!pb.Id.Contains("BOTTOM"))
                                {
                                    cent = H.origin - 16;

                                }


                                if (pb.Id.Contains("BOTTOM"))
                                {
                                    H.origin = cent;

                                }

                                */



                                // S.Show(H.origin.ToString());




                                // Calculation along Hb profile which is offcentre
                                if (pb.Id.Contains("BOTTOM"))
                                {

                                    

                                    maxVal = null; //nullable so this works even if you have all super-low negatives
                                    index = -1;
                                    for (int k = 1; k < Hb.len; k++)
                                    {
                                        thisNum = HbLine[k].Value;
                                        if (!maxVal.HasValue || thisNum > maxVal.Value)
                                        {
                                            maxVal = thisNum;
                                            H.max = thisNum;
                                            index = k;
                                        }
                                    }
                                    

                                }



                                //50% percent
                                //H.Fifty = H.max / 2;

                                //Use centre dose as normalization
                                H.Fifty = H.min / 2;



                                //Bottom. Use H.origin rather Hb.origin (should be correct)
                                if (pb.Id.Contains("BOTTOM"))

                                {
                                    H.A1 = H.origin - 90;
                                    H.A2 = H.origin - 60;

                                    H.B1 = H.origin + 90;
                                    H.B2 = H.origin + 30;

                                }


                                if (pb.Id.Contains("TOP"))

                                {
                                    H.A1 = H.origin - 90;
                                    H.A2 = H.origin - 60;

                                    H.B1 = H.origin + 90;
                                    H.B2 = H.origin + 30;

                                }


                                if (pb.Id.Contains("10x10"))

                                {
                                    H.A1 = H.origin - 180;
                                    H.A2 = H.origin - 120;

                                    H.B1 = H.origin + 180;
                                    H.B2 = H.origin + 120;

                                }


                                if (pb.Id.Contains("4x4"))

                                {

                                    H.A1 = H.origin - 90;
                                    H.A2 = H.origin - 30;

                                    H.B1 = H.origin + 90;
                                    H.B2 = H.origin + 30;

                                }

                                if (pb.Id.Contains("20x20"))

                                {

                                    H.A1 = H.origin - 330;
                                    H.A2 = H.origin - 270;

                                    H.B1 = H.origin + 330;
                                    H.B2 = H.origin + 270;

                                }

                                if (pb.Id.Contains("8x8"))

                                {
                                    H.A1 = H.origin - 150;
                                    H.A2 = H.origin - 90;

                                    H.B1 = H.origin + 150;
                                    H.B2 = H.origin + 90;

                                }


                                for (x = H.A1; x < H.A2; x++)
                                {
                                    if (HLine[x].Value > H.Fifty)
                                    {
                                        break;
                                    }
                                }



                                H.A0 = x;

                                H.A = ((H.origin - H.A0) * scale); //+ (scale/2);


                                if (pb.Id.Contains("BOTTOM"))
                                {
                                    
                                    for (x = H.A1; x < H.A2; x++)
                                    {
                                        if (HbLine[x].Value > H.Fifty)
                                        {
                                            break;
                                        }
                                    }

                                    H.A0 = x;

                                    H.A = ((H.origin - H.A0) * scale); // + (scale / 2);

                                }


                                //Calculating B


                                for (x = H.B2; x < H.B1; x++)
                                {
                                    if (HLine[x].Value < H.Fifty)
                                    {
                                        break;
                                    }
                                }

                                H.B0 = x;


                                H.B = ((H.B0 - H.origin) * scale); // - (scale / 2);


                                if (pb.Id.Contains("BOTTOM"))
                                {
                                    

                                    for (x = H.B2; x < H.B1; x++)
                                    {

                                        //offcentre line
                                        if (HbLine[x].Value < H.Fifty)
                                        {
                                            break;
                                        }
                                    }

                                    H.B0 = x;

                                    H.B = ((H.B0 - H.origin) * scale); // - (scale / 2);

                                }


                                //Calculating T

                                maxVal = null; //nullable so this works even if you have all super-low negatives
                                index = -1;
                                for (int k = 1; k < V.len; k++)
                                {
                                    thisNum = VLine[k].Value;
                                    if (!maxVal.HasValue || thisNum > maxVal.Value)
                                    {
                                        maxVal = thisNum;
                                        V.max = thisNum;
                                        index = k;
                                    }
                                }


                                minVal = null; //nullable so this works even if you have all super-low negatives
                                index = -1;
                                for (int l = (V.len / 2) - 10; l < (V.len / 2) + 10; l++)
                                {
                                    thisNum = VLine[l].Value;


                                    if (!minVal.HasValue || thisNum < minVal.Value)
                                    {
                                        minVal = thisNum;
                                        V.min = thisNum;
                                        index = l;
                                        // S.Show(index.ToString());

                                    }
                                }


                                V.origin = index;
                                //S.Show(index.ToString());

                                //V.Fifty = V.max / 2;

                                //Normalization at centre
                                V.Fifty = V.min / 2;


                                if (pb.Id.Contains("BOTTOM"))

                                {
                                    
                                    V.T1 = V.origin + 85;
                                    V.T2 = V.origin + 45;

                                    V.G1 = V.origin - 95;
                                    V.G2 = V.origin - 65;

                                }


                                if (pb.Id.Contains("TOP"))

                                {    //T2->T1=x++
                                    V.T1 = V.origin + 100;
                                    V.T2 = V.origin + 70;

                                    //G2->G1=X--
                                    V.G1 = V.origin - 80;
                                    V.G2 = V.origin - 40;


                                }

                                if (pb.Id.Contains("10x10"))

                                {
                                    V.T1 = V.origin + 180;
                                    V.T2 = V.origin + 120;

                                    V.G1 = V.origin - 180;
                                    V.G2 = V.origin - 120;

                                }


                                if (pb.Id.Contains("4x4"))

                                {

                                    V.T1 = V.origin + 90;
                                    V.T2 = V.origin + 30;

                                    V.G1 = V.origin - 90;
                                    V.G2 = V.origin - 30;

                                }

                                if (pb.Id.Contains("20x20"))

                                {

                                    V.T1 = V.origin + 330;
                                    V.T2 = V.origin + 270;

                                    V.G1 = V.origin - 330;
                                    V.G2 = V.origin - 270;

                                }


                                if (pb.Id.Contains("8x8"))

                                {
                                    V.T1 = V.origin + 150;
                                    V.T2 = V.origin + 90;

                                    V.G1 = V.origin - 150;
                                    V.G2 = V.origin - 90;

                                }


                                for (x = V.T2; x < V.T1; x++)
                                {
                                    if (VLine[x].Value < V.Fifty)
                                    {
                                        break;
                                    }
                                }

                               

                                V.T0 = x;


                                V.T = ((V.T0 - V.origin) * scale); // - (scale / 2);



                                for (x = V.G1; x < V.G2; x++)
                                {
                                    if (VLine[x].Value > V.Fifty)


                                    {
                                        break;
                                    }
                                }



                                V.G0 = x;

                               

                                V.G = ((V.origin - V.G0) * scale); // + (scale / 2);

                                string A = H.A.ToString("0.0");
                                string B = H.B.ToString("0.0");
                                string T = V.T.ToString("0.0");
                                string G = V.G.ToString("0.0");

                                

                                if (pb.Id.Contains("C=90"))

                                {
                                    A = "A (Y2) = " + A + "mm, ";
                                    B = "B (Y1) = " + B + "mm, ";
                                    G = "G (X2) = " + G + "mm, ";
                                    T = "T (X1) = " + T + "mm";

                                }

                                else if (pb.Id.Contains("C=270"))

                                {
                                    A = "A (Y1) = " + A + "mm, ";
                                    B = "B (Y1) = " + B + "mm, ";
                                    G = "G (X1) = " + G + "mm, ";
                                    T = "T (X2) = " + T + "mm";

                                }

                                else if (pb.Id.Contains("C270"))

                                {
                                    A = "A (Y1) = " + A + "mm, ";
                                    B = "B (Y1) = " + B + "mm, ";
                                    G = "G (X1) = " + G + "mm, ";
                                    T = "T (X2) = " + T + "mm";

                                }

                                else
                                {
                                    A = "A (X1) = " + A + "mm, ";
                                    B = "B (X2) = " + B + "mm, ";
                                    G = "G (Y2) = " + G + "mm, ";
                                    T = "T (Y1) = " + T + "mm";

                                }

                                temp = pln.Id + ", " + pb.Id + ", " + di.CreationDateTime.ToString() + NL + A + B + G + T + NL;



                                result = result + temp + NL + NL;
                                // S.Show(result);

                                //}

                            }


                        }

                    }
                }
            }

            S.Show(result);
        }


    }


    public class HProf

    {
        public int x1 { get; set; }
        public int x2 { get; set; }
        public int y1 { get; set; }
        public int y2 { get; set; }
        public int origin { get; set; }

        public int cent0 { get; set; }

        public int cent90 { get; set; }

        public int cent270 { get; set; }

        public int A0 { get; set; }
        public double A { get; set; }
        public int B0 { get; set; }
        public double B { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public int len { get; set; }
        public int A1 { get; set; }
        public int A2 { get; set; }
        public int B1 { get; set; }
        public int B2 { get; set; }
        public double Fifty { get; set; }

    }

    public class VProf

    {
        public int x1 { get; set; }
        public int x2 { get; set; }
        public int y1 { get; set; }
        public int y2 { get; set; }
        public int origin { get; set; }
        public int G0 { get; set; }
        public double G { get; set; }
        public int T0 { get; set; }
        public double T { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public int len { get; set; }
        public int G1 { get; set; }
        public int G2 { get; set; }
        public int T1 { get; set; }
        public int T2 { get; set; }
        public double Fifty { get; set; }

    }


}







