        private void textBoxWord_TextChanged(object sender, EventArgs e)
        {
            if (textBoxWord.Text.Length != 0) // Gia su tu khoa hop le
            {
                string khoa = textBoxWord.Text;
                // Neu nhu trong LISTBOX da co KHOA nay roi
                if (listBoxWord.Items.IndexOf(khoa) != -1) 
                {
                    int vitri = listBoxWord.Items.IndexOf(khoa); // Vay thi toi ngay vi tri cua KHOA ngay thoi
                    listBoxWord.TopIndex = vitri;
                    listBoxWord.SelectedIndex = vitri;
                    return; // Vay thi khoi lam gi nua, return thoi
                }
                // Con khong thi di tiep, tim tu lien quan, vi du: khoa 'hel' thi lay tu 'held'
                foreach (string s in listBoxWord.Items)
                {
                    if (s.StartsWith(khoa)) // Neu 'held' chua 'hel'
                    {
                        int vitri = listBoxWord.Items.IndexOf(s); // Lay vi tri
                        listBoxWord.TopIndex = vitri;
                        listBoxWord.SelectedIndex = vitri;
                        return; // Cha can di tiep nua
                    }
                }
                // Neu ma toi duoc buoc nay, thi chac chac la chua co du lieu trong LISTBOX
                string chucaidautien = khoa[0].ToString();
                if (char.IsLetter(khoa[0]))
                    chucaidautien = chucaidautien.ToUpper();
                else if (char.IsNumber(khoa[0]))
                    chucaidautien = "0";
                else
                    chucaidautien = "?";

                var d = from x in list[chucaidautien].Keys
                        where (!listBoxWord.Items.Contains(x) && x.Contains(khoa))
                        select x;

                if (d != null) // var d co cai gi o ben trong thi moi add vao listbox, con khong thi add vao lam chi
                {
                    IEnumerable<string> kq = d as IEnumerable<string>;
                    listBoxWord.Items.AddRange(kq.ToArray());
                }
            }
        }