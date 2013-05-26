static void Main(string[] args) {
            string x;
            using (StreamReader reader = new StreamReader(@"C:\dictd_anh-viet.dict"))
            {
                x = reader.ReadToEnd();
            }
            
            Dictionary<string, string> DICT = new Dictionary<string, string>(); // TU DIEN
            string[] noi_dung = x.Split(new char[] { '@' }); // Moi phan tu la mot tu
            int dau_xuong_hang = -1; // Dung bien tinh trong lop de han che khai bao lien tuc
            
            foreach (string s in noi_dung)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    dau_xuong_hang = s.IndexOf("\n");
                    if (dau_xuong_hang != -1) // Chuoi s co dau xuong hang
                    {
                        if (s.IndexOf(" =") != -1 && s.IndexOf(" =") < dau_xuong_hang) // Co Tu Dong Nghia
                        {
                            int dau_dong_nghia = s.IndexOf(" =");
                            x = s.Substring(0, dau_dong_nghia).Trim(); // Tu khoa
                            string nd = s.Substring(dau_dong_nghia + 1);
                            ThemTu(DICT, x, nd);
                        }
                        else // Khong co tu Dong Nghia
                        {
                            if (s.IndexOf(" /") != -1 && s.IndexOf(" /") < dau_xuong_hang) // CO Phat am
                            {
                                int dau_phat_am = s.IndexOf(" /");
                                x = s.Substring(0, dau_phat_am).Trim();
                                string nd = s.Substring(dau_phat_am + 1);

                                ThemTu(DICT, x, nd);
                            }
                            else // Khong co phat am
                            {

                                x = s.Substring(0, dau_xuong_hang).Trim();
                                string nd = s.Substring(dau_xuong_hang + 1);

                                ThemTu(DICT, x, nd);
                            }
                        }
                    }
                    else //Truong hop tu khong co ki tu xuong hang \n (mot so tu dong nghia co th nay)
                    {
                        if (s.IndexOf(" =") != -1)
                        {
                            x = s.Substring(0, s.IndexOf(" =")).Trim();
                            string nd = s.Substring(s.IndexOf(" =") + 1);
                            ThemTu(DICT, x, nd);
                        }
                        
                    }
                }
            }
}

public static void ThemTu(Dictionary<string, string> DICT, string tu, string noidung) {
            if (DICT.ContainsKey(tu))
            {
                DICT[tu] += noidung;
                Program.duplicate++;
            }
            else
                DICT.Add(tu, noidung);
        }
