using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
namespace ThuNghiem
{
    class Program
    {
        static void Main(string[] args)
        {
            string x;
            using (StreamReader reader = new StreamReader("dictd_anh-viet.dict"))
            {
                x = reader.ReadToEnd();
            }
            
            Dictionary<string, string[]> DICT = new Dictionary<string, string[]>();  // TU DIEN
            string[] content = x.Split(new char[] { '@' }); // NOI DUNG

            foreach (string s in content)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    int dau_xuong_hang = s.IndexOf('\n');
                    
                    if (dau_xuong_hang > 0)
                    {
                        // Co phat am hay khong?
                        if (s[dau_xuong_hang - 1].Equals('/'))
                        {
                            int khoang_trang = s.IndexOf(' ', 0, dau_xuong_hang); // Khoang trang truoc phan phat am /... /

                            x = s.Substring(0, khoang_trang); // WORD
                            string phatam = s.Substring(khoang_trang + 1, dau_xuong_hang - khoang_trang - 1); // Phat Am
                            //Console.WriteLine(phatam);
                            // Them vao tu dien
                            if (!DICT.ContainsKey(x))
                            {
                                DICT.Add(x, new string[] { 
                                phatam, 
                                s.Substring(dau_xuong_hang + 1) });
                            }
                            else
                            {
                                DICT[x][1] += s.Substring(dau_xuong_hang + 1);
                            }
                        }
                        else // Khong co phat am!
                        {
                            x = s.Substring(0, dau_xuong_hang);

                            // Them vao tu dien
                            // va neu khong trung
                            if (!DICT.ContainsKey(x))
                            {
                                DICT.Add(x, new string[] {
                                "",
                                s.Substring(dau_xuong_hang + 1) });
                            }
                            else
                            {
                                DICT[x][1] += s.Substring(dau_xuong_hang + 1);
                            }

                        }

                    }
                    else
                    {
                        DICT.Add(s, new string[] { "" });
                    }
                }
            }
            //string tu = "";
            //while (!tu.Equals("exit"))
            //{
            //    tu = Console.ReadLine();
            //    if (DICT.ContainsKey(tu.ToLower()))
            //    {
            //        Console.WriteLine("****");
            //        Console.Write(DICT[tu][0] + '\n' + DICT[tu][1]);
            //    }
                    
            //}
            ///
            Console.ReadKey();
        }
    }
}
    	
