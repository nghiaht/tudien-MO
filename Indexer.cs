using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.IO.Compression;
using System.Text;

public class Indexer
{

    string INDEX_FILEPATH;
    string DICT_FILEPATH;
    Dictionary<string, Dictionary<string, string>> DICT = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                {"A", new Dictionary<string,string>{}},
                {"B", new Dictionary<string,string>{}},
                {"C", new Dictionary<string,string>{}},
                {"D", new Dictionary<string,string>{}},
                {"E", new Dictionary<string,string>{}},
                {"F", new Dictionary<string,string>{}},
                {"G", new Dictionary<string,string>{}},
                {"H", new Dictionary<string,string>{}},
                {"I", new Dictionary<string,string>{}},
                {"J", new Dictionary<string,string>{}},
                {"K", new Dictionary<string,string>{}},
                {"L", new Dictionary<string,string>{}},
                {"M", new Dictionary<string,string>{}},
                {"N", new Dictionary<string,string>{}},
                {"O", new Dictionary<string,string>{}},
                {"P", new Dictionary<string,string>{}},
                {"Q", new Dictionary<string,string>{}},
                {"R", new Dictionary<string,string>{}},
                {"S", new Dictionary<string,string>{}},
                {"T", new Dictionary<string,string>{}},
                {"U", new Dictionary<string,string>{}},
                {"V", new Dictionary<string,string>{}},
                {"W", new Dictionary<string,string>{}},
                {"X", new Dictionary<string,string>{}},
                {"Y", new Dictionary<string,string>{}},
                {"Z", new Dictionary<string,string>{}},
                {"0", new Dictionary<string,string>{}},
                {"?", new Dictionary<string,string>{}}
            };

    public Indexer(string DICT_FILEPATH, string INDEX_FILEPATH)
    {
        //NapListBox(INDEX_FILEPATH, listBox);
        this.DICT_FILEPATH = DICT_FILEPATH;
        this.INDEX_FILEPATH = INDEX_FILEPATH;
    }

    public Dictionary<string, Dictionary<string, string>> TUDIEN { get { return this.DICT; } set { this.DICT = value; } }
    //
    // Giai nen file TUDIEN duoc nen o dang .gz
    //
    public void GiaiNen(string DAUVAO, string DAURA)
    {
        using (FileStream streamMoFileGiaiNen = new FileStream(DAUVAO, FileMode.Open))
        {
            using (FileStream streamSauKhiGiaiNen = File.Create(DAURA))
            {
                using (GZipStream gzreader = new GZipStream(streamMoFileGiaiNen, CompressionMode.Decompress))
                {
                    gzreader.CopyTo(streamSauKhiGiaiNen);
                }
            }
        }
    }
   

    #region Nap TU va LISTBOX
    public void NapListBox(ListBox listBox1)
    {
        Dictionary<string, string> DANHSACHTU = new Dictionary<string, string>() { };
        string tu;

        using (StreamReader reader = new StreamReader(INDEX_FILEPATH))
        {
            tu = reader.ReadToEnd();
        }


        string[] TU = tu.Split(new char[] { '\n' }); // Tach tu ra thanh tung phan tu trong mang

        foreach (string s in TU)
        {
            if (!string.IsNullOrEmpty(s))
            {
                int vitri_tab = s.IndexOf('\t');
                tu = s.Substring(0, vitri_tab);
                if (!DANHSACHTU.ContainsKey(tu))
                    DANHSACHTU.Add(tu, "");
            }
        }

        listBox1.DataSource = DANHSACHTU.Keys.ToList();

    }

    public Dictionary<string, Dictionary<string, string>> NapTu()
    {
        string tu; // TU khoa
        string tu_offset_length; //  Thong tin offset, length cua TU khoa trong file DULIEU .DICT nham tang toc do truy cap

        using (StreamReader reader = new StreamReader(INDEX_FILEPATH))
        {
            tu = reader.ReadToEnd();
        }

        string[] TU = tu.Split(new char[] { '\n' }); // Tach tu ra thanh tung phan tu trong mang


        foreach (string s in TU)
        {

            if (!string.IsNullOrEmpty(s))
            {
                int vitri_tab = s.IndexOf('\t');
                tu = s.Substring(0, vitri_tab);
                tu_offset_length = s.Substring(vitri_tab + 1);

                string kitu_dautien = tu[0].ToString().ToUpper(); // lay ki tu dau tien cua TU
                if (char.IsLetter(tu[0])) // neu ki tu dau tien la chu cai
                {
                    if (!DICT[kitu_dautien].ContainsKey(tu))
                        DICT[kitu_dautien].Add(tu, tu_offset_length);
                }
                else if (char.IsNumber(tu[0])) // neu la so
                {
                    //if (!DICT["0"].ContainsKey(tu))
                    DICT["0"].Add(tu, tu_offset_length);
                }
                else // chac la nhung ky tu dac biet
                {
                    DICT["?"].Add(tu, tu_offset_length);
                }
            }
        }
        return DICT;
    } 
    #endregion

    #region Co so 64 va Tra tu
    private static string INDEX_TABLE = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/"; // Bang index luu vi tri so cua cac ki tu trong base64

    public static int LayInt(string s)
    {
        int ket_qua = 0;
        int length = s.Length;
        for (int i = 0; i < length; i++)
        {
            int vitri = INDEX_TABLE.IndexOf(s[i]);
            ket_qua += (int)(vitri * Math.Pow(64, length - i - 1)); // Chuyen doi tu co so 64 sang co so 10
        }
        return ket_qua;
    }


    //
    // Lay ve ca tu lan nghia (doc truc tiep tu file)
    //

    public string TraTu(string khoa) // dung ham nay de tra tu
    {
        int[] offset_length = Lay_OffsetLength(khoa);
        string tu = DocTu(offset_length[0], offset_length[1]);

        return tu;
    }

    public int[] Lay_OffsetLength(string khoa)
    {
        string offlen = DICT[khoa[0].ToString().ToUpper()][khoa];
        string[] offset_length = offlen.Split('\t');
        int offset = LayInt(offset_length[0]);
        int length = LayInt(offset_length[1]);
        return new int[] { offset, length };
    }
    public string DocTu(int offset, int length)
    {
        byte[] bytes = new byte[length];
        using (FileStream fs = new FileStream(DICT_FILEPATH, FileMode.Open, FileAccess.Read))
        {
            fs.Seek(offset, SeekOrigin.Begin);
            fs.Read(bytes, 0, length);
        }
        return Encoding.UTF8.GetString(bytes);
    }
    #endregion


}