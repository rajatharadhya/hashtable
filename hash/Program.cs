using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace hash
{
    class Program
    {
        public static short link;
        
        static BinaryWriter md;
        static BinaryWriter mdc;
        static FileStream output;
        static FileStream outputcol;
        static BinaryReader b;
        static BinaryReader c;
        static BinaryReader dd;
        public static StreamWriter AssignmentLogFile;
        public  static short loopcount;
        static void Main(string[] args)
        {
             loopcount = new short();
             loopcount = 1;
            File.Delete(".//..//..//..//" + "MainData.bin");
            File.Delete(".//..//..//..//" + "MainDataCollisions.bin");
            output = new FileStream(".//..//..//..//" + "MainData.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            md = new BinaryWriter(output);
            b = new BinaryReader(output);
            outputcol = new FileStream(".//..//..//..//" + "MainDataCollisions.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            mdc = new BinaryWriter(outputcol);
            c = new BinaryReader(outputcol);
            AssignmentLogFile = new StreamWriter(".//..//..//..//" + "AssignmentLogFile.txt");
            int collrrn = 1;
            string dir;
            string currentDir = Directory.GetCurrentDirectory();
            currentDir = Directory.GetParent(currentDir).ToString();
            currentDir = Directory.GetParent(currentDir).ToString();
            currentDir = Directory.GetParent(currentDir).ToString();
            currentDir += @"\hash\RawDataA2.csv";
            StreamReader read = new StreamReader(currentDir);
            short HeadPtr;
            while (read.Peek() > -1)
            {
                dir = read.ReadLine();
                string[] arr = dir.Split(',');
                for (int i = 0; i < arr.Length; i++)
                    if (arr[i] == "NULL")
                        arr[i] = "00";
                char[] readcode = new Char[3];
                int RRN = hashfunction(arr[1]);
                int offset = ((offsetcalc(0)) * RRN);
              try
                {
                     b.BaseStream.Seek((offset), SeekOrigin.Begin);
                    readcode = b.ReadChars(3);
                }
              catch
              { }
              string readcoding = new string(readcode);
              if (readcoding == "" || readcoding == "\0\0\0")
                    {
                        HeadPtr = -1;
                        write1rec(RRN, arr, HeadPtr);
                    }
                    else
                    {
                        b.BaseStream.Seek(offset + offsetcalc(1), SeekOrigin.Begin);
                        HeadPtr = b.ReadInt16();
                        if (HeadPtr == 0)
                            HeadPtr = -1;
                        write1reccol(collrrn, arr, HeadPtr);
                        //if (RRN == 11 || RRN == 18)
                        //    b.BaseStream.Seek(offset + offsetcalc(1) - 2, SeekOrigin.Current);
                        //else
                      b.BaseStream.Seek((offset + offsetcalc(1)), SeekOrigin.Begin);
                        md.Write(collrrn);
                        collrrn++;
                    }
                             }
            transactionhandler();
            fullmain();
            fullmaincoll();
                        b.Close();
                        md.Close();
            output.Close();
            mdc.Close();
            outputcol.Close();
            c.Close();
            AssignmentLogFile.Close();
        }
        public static int hashfunction(string ccode)
        {
            int max_homelocation = 20;
            string countryCode;
            countryCode = ccode.ToUpper();
            byte[] asciibytes = Encoding.ASCII.GetBytes(countryCode);
            int mul = asciibytes[0] * asciibytes[1] * asciibytes[2];
            int RRN = (mul % max_homelocation) + 1;
            return RRN;
        }
        public static void write1rec(int RRN, string[] rec,short HeadPtr)
        {
            Char[] countrycode = new char[3];
            countrycode = rec[1].ToCharArray(); ;
            Char[] name = new char[17];
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            rec[2] = rec[2].Length <= 17 ? rec[2] : rec[2].Substring(0, 17);
            sb.Append(rec[2]);
            for (int i = rec[2].Length; i < 17; i++)
                sb.Append(" ");
            string temp = sb.ToString();
            name = temp.ToCharArray();
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            Char[] continent = new char[12];
            rec[3] = rec[3].Length <= 12 ? rec[3] : rec[3].Substring(0, 12);
            sb2.Append(rec[3]);
            for (int i = rec[3].Length; i < 12; i++)
                sb2.Append(" ");
            temp = sb2.ToString();
            continent = temp.ToCharArray();
            Int32 surfaceArea = Convert.ToInt32(rec[5]);
            short yearOfIndependence = Convert.ToInt16(rec[6]);
            long population = Convert.ToInt64(rec[7]);
            float lifeExp = Convert.ToSingle(rec[8]);
            Int32 gnp = Convert.ToInt32(rec[9]);
            int offset = offsetcalc(0) * RRN;
            md.Seek(offset, SeekOrigin.Begin);
            md.Write(countrycode);
            md.Write(name);
            md.Write(continent);
            md.Write(surfaceArea);
            md.Write(yearOfIndependence);
            md.Write(population);
            md.Write(lifeExp);
            md.Write(gnp);
            offset = (offsetcalc(0) * RRN) + offsetcalc(1);
            //if (RRN == 11 || RRN == 18)
            //    b.BaseStream.Seek(offset - 2, SeekOrigin.Begin);
            //else
            b.BaseStream.Seek(offset, SeekOrigin.Begin);
            md.Write(HeadPtr);
            int gimick = 3 * sizeof(char) + 17 * sizeof(char) + 12 * sizeof(char) + System.Runtime.InteropServices.Marshal.SizeOf(surfaceArea) + System.Runtime.InteropServices.Marshal.SizeOf(yearOfIndependence) + System.Runtime.InteropServices.Marshal.SizeOf(population) + System.Runtime.InteropServices.Marshal.SizeOf(lifeExp) + System.Runtime.InteropServices.Marshal.SizeOf(gnp) + System.Runtime.InteropServices.Marshal.SizeOf(HeadPtr);
                   }
        public static int offsetcalc(int vv)
    {
        int sizeoflogicalDataRec = 3 * sizeof(char) + 17 * sizeof(char) + 12 * sizeof(char) + sizeof(int) + sizeof(short) + sizeof(long) + sizeof(float) + sizeof(int);
        int sizeOfPhysicalDataRec = sizeoflogicalDataRec + sizeof(short);
        if (vv == 0)
            return sizeOfPhysicalDataRec;
        else
            return sizeoflogicalDataRec;
    }
        public static void write1reccol(int rrn, string[] rec, short HeadPtr)
        {
            Char[] countrycode = new char[3];
            countrycode = rec[1].ToCharArray(); ;
            Char[] name = new char[17];
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            rec[2] = rec[2].Length <= 17 ? rec[2] : rec[2].Substring(0, 17);
            sb.Append(rec[2]);
            for (int i = rec[2].Length; i < 17; i++)
                sb.Append(" ");
            string temp = sb.ToString();
            name = temp.ToCharArray();
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            Char[] continent = new char[12];
            rec[3] = rec[3].Length <= 12 ? rec[3] : rec[3].Substring(0, 12);
            sb2.Append(rec[3]);
            for (int i = rec[3].Length; i < 12; i++)
                sb2.Append(" ");
            temp = sb2.ToString();
            continent = temp.ToCharArray();
            Int32 surfaceArea = Convert.ToInt32(rec[5]);
            short yearOfIndependence = Convert.ToInt16(rec[6]);
            long population = Convert.ToInt64(rec[7]);
            float lifeExp = Convert.ToSingle(rec[8]);
            Int32 gnp = Convert.ToInt32(rec[9]);
            int offset = rrn * offsetcalc(0);
            mdc.Seek(offset, SeekOrigin.Begin);
            mdc.Write(countrycode);
            mdc.Write(name);
            mdc.Write(continent);
            mdc.Write(surfaceArea);
            mdc.Write(yearOfIndependence);
            mdc.Write(population);
            mdc.Write(lifeExp);
            mdc.Write(gnp);
            mdc.Write(HeadPtr);
            int sizeofdod = System.Runtime.InteropServices.Marshal.SizeOf(yearOfIndependence);
            int sizeofth = System.Runtime.InteropServices.Marshal.SizeOf(surfaceArea);
            int dsdhf = System.Runtime.InteropServices.Marshal.SizeOf(population);
            int sdfsdf = System.Runtime.InteropServices.Marshal.SizeOf(lifeExp);
            int sdffssdf = System.Runtime.InteropServices.Marshal.SizeOf(gnp);
            int oo = System.Runtime.InteropServices.Marshal.SizeOf(HeadPtr);
            int gimick = 3 * sizeof(char) + 17 * sizeof(char) + 12 * sizeof(char) + sizeofdod + sizeofth + dsdhf + sdfsdf + sdffssdf + oo;// +System.Runtime.InteropServices.Marshal.SizeOf(link);
        
        
         }
        public static int hashsearch(string code)
        {
            short rrn = Convert.ToInt16(hashfunction(code));
            int offset = offsetcalc(0) ;
            b.BaseStream.Seek((offset* rrn), SeekOrigin.Begin);
            Char[] initcode = new char[3];
            initcode = b.ReadChars(3);
            string initccode = new string(initcode);
             short correctlink=0;
             if (initccode == "" || initccode == "\0\0\0")
                 return -6;
            else if (code == initccode)
            {
                link = Convert.ToInt16(rrn);
                return -2;                      // found in maindata
            }
            else
            {
                correctlink = link;
                b.BaseStream.Seek((offsetcalc(0) * rrn) + offsetcalc(1), SeekOrigin.Begin);       // headpointer from maindata to link to maindaacollision 
                for (link = b.ReadInt16();code != initccode; dummyread())
                {

                    if (link == -1)
                        break;
                    loopcount++;
                    dd = new BinaryReader(outputcol);
                    dd.BaseStream.Seek(offset * link, SeekOrigin.Begin);
                    initcode = dd.ReadChars(3);
                    string temp = new string(initcode);
                    initccode = temp;
                    int gg;
                        gg=((offsetcalc(0) * link) + (offsetcalc(1)));
                    dd.BaseStream.Seek(gg, SeekOrigin.Begin);
                    correctlink = link;
                }
            }
            if (code == initccode)
            {
                link = correctlink;
                return -4;  
            }
            else
                return -6;  
        }
        public static void dummyread()
        {

            int offset = offsetcalc(0) * link;
            c.BaseStream.Seek((offset), SeekOrigin.Begin);
            Char[] code = new char[3];
            Char[] name = new char[17];
            Char[] continent = new char[12];
            code = c.ReadChars(3);
            string ccode = new string(code);
            name = c.ReadChars(17);
            string cname = new string(name);
            continent = c.ReadChars(12);
            string ccontinent = new string(continent);
            Int32 surfaceArea = c.ReadInt32();
            short yearOfIndependence = c.ReadInt16();
            long population = c.ReadInt64();
            float lifeExp = c.ReadSingle();
            Int32 gnp = c.ReadInt32();
             link = c.ReadInt16();
        }
        public static void transactionhandler()
        {
            int indicator;
            StreamReader tr = new StreamReader(@".//..//..//..//" + "TransDataA2.txt");
            AssignmentLogFile.WriteLine("------------TRANSACTION HANDLER----------");
            
             while (tr.Peek() >= 0)
            {
                string line = tr.ReadLine();
                string[] transarr = line.Split(' ');
                string action = transarr[0];
                AssignmentLogFile.WriteLine(action+transarr[1]);
                loopcount = 1;
                if (action == "QC")
                {
                     indicator = hashsearch(transarr[1]);
                    if (indicator == -2)
                    {

                        readmaindata(link);
                        AssignmentLogFile.WriteLine("[" + loopcount + "]");
                    }
                    else if (indicator == -4)
                    {
                        readmaindatacollision(link);
                        AssignmentLogFile.WriteLine("[" + loopcount + "]");
                    }
                    else
                    {
                        AssignmentLogFile.WriteLine("**ERROR: no country with code"+transarr[1]);
                        AssignmentLogFile.WriteLine("[" + loopcount + "]");
                    }
                }
                else if (action == "DC")
                {
                    indicator = hashsearch(transarr[1]);
                    if (indicator == -2)
                    {
                        md.BaseStream.Seek((offsetcalc(0) * link), SeekOrigin.Begin);
                        Char[] countrycode = new char[3];
                        countrycode = "   ".ToCharArray(); ;
                        Char[] name = new char[17];
                        name = "                 ".ToCharArray();
                        Char[] continent = new char[12];
                        continent = "            ".ToCharArray();
                        Int32 surfaceArea = 00000000;
                        short yearOfIndependence = 00000000;
                        long population =0000000;
                        float lifeExp = 000;
                        Int32 gnp = 000000;
                        md.Write(countrycode);
                        md.Write(name);
                        md.Write(continent);
                        md.Write(surfaceArea);
                        md.Write(yearOfIndependence);
                        md.Write(population);
                        md.Write(lifeExp);
                        md.Write(gnp);
                        AssignmentLogFile.WriteLine("OK," + name + "         deleted");
                        AssignmentLogFile.WriteLine("[" + loopcount + "]");
                    }
                    else if (indicator == -4)
                    {
                        mdc.BaseStream.Seek((offsetcalc(0) * link), SeekOrigin.Begin);
                        Char[] countrycode = new char[3];
                        countrycode = "   ".ToCharArray(); ;
                        Char[] name = new char[17];
                        name = "                 ".ToCharArray();
                        Char[] continent = new char[12];
                        continent = "            ".ToCharArray();
                        Int32 surfaceArea = 00000000;
                        short yearOfIndependence = 00000000;
                        long population = 0000000;
                        float lifeExp = 000;
                        Int32 gnp = 000000;
                        mdc.Write(countrycode);
                        mdc.Write(name);
                        mdc.Write(continent);
                        mdc.Write(surfaceArea);
                        mdc.Write(yearOfIndependence);
                        mdc.Write(population);
                        mdc.Write(lifeExp);
                        mdc.Write(gnp);
                        AssignmentLogFile.WriteLine("OK," + name + "         deleted");
                        AssignmentLogFile.WriteLine("[" + loopcount + "]");
                    }
                    else
                    {
                        AssignmentLogFile.WriteLine("**ERROR: no country with code"+transarr[1]);
                        AssignmentLogFile.WriteLine("[" + loopcount + "]");
                    }

                }
                else
                {
                    AssignmentLogFile.WriteLine("**SORRY: insert function not yet operational");
                }

            }
        }
        public static void fullmain()
        {
            AssignmentLogFile.WriteLine();
            AssignmentLogFile.WriteLine();
            AssignmentLogFile.WriteLine("----------------MainData after Transactions--------------");
            for (short RRN = 1; RRN < 20; RRN++)
            {
                readmaindata(RRN);
            }
           
        }

        public static void readmaindata(int RRN)
        {
                int offset = offsetcalc(0)*RRN;
                b.BaseStream.Seek((offset), SeekOrigin.Begin);
                Char[] code = new char[3];
                Char[] name = new char[17];
                Char[] continent = new char[12];
                code = b.ReadChars(3);
                string ccode = new string(code);
                name = b.ReadChars(17);
                string cname = new string(name);
                continent = b.ReadChars(12);
                string ccontinent = new string(continent);
                Int32 surfaceArea = b.ReadInt32();
                short yearOfIndependence = b.ReadInt16();
                long population = b.ReadInt64();
                float lifeExp = b.ReadSingle();
                Int32 gnp = b.ReadInt32();
                offset = (offsetcalc(0) * RRN) + offsetcalc(1);
                b.BaseStream.Seek(offset, SeekOrigin.Begin);
                short linku = b.ReadInt16();
                 AssignmentLogFile.WriteLine(ccode + " " + cname + " " + ccontinent + " " + surfaceArea + " " + yearOfIndependence + " " + population + " " + lifeExp + " " + gnp + " " + linku);
        }
        public static void fullmaincoll()
        {
            AssignmentLogFile.WriteLine();
            AssignmentLogFile.WriteLine();
            AssignmentLogFile.WriteLine("----------------MainDataCollision after Transactions--------------");
            for (int RRN = 1; RRN < 16; RRN++)
            {
                readmaindatacollision(RRN);
            }
          
        }

        public static void readmaindatacollision(int RRN)
        {
            int offset = offsetcalc(0)*RRN;
                c.BaseStream.Seek((offset), SeekOrigin.Begin);
                Char[] code = new char[3];
                Char[] name = new char[17];
                Char[] continent = new char[12];
                code = c.ReadChars(3);
                string ccode = new string(code);
                name = c.ReadChars(17);
                string cname = new string(name);
                continent = c.ReadChars(12);
                string ccontinent = new string(continent);
                Int32 surfaceArea = c.ReadInt32();
                short yearOfIndependence = c.ReadInt16();
                long population = c.ReadInt64();
                float lifeExp = c.ReadSingle();
                Int32 gnp = c.ReadInt32();
                short link = c.ReadInt16();
               AssignmentLogFile.WriteLine(ccode + " " + cname + " " + ccontinent + " " + surfaceArea + " " + yearOfIndependence + " " + population + " " + lifeExp + " " + gnp + " " + link);
        }
    }
}
