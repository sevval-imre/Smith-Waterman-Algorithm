using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Biyoinformatik_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int match, mis, gap;
        string[] Seq1;
        string[] Seq2;
        int Seq1_len, Seq2_len;
        int counter = 0;
        int score = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = "1";
            textBox4.Text = "-1";
            textBox5.Text = "-2";
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.ColumnHeadersHeight = 10;
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void button2_Click(object sender, EventArgs e)
        {           
            string s1 = Seq1[1].ToUpper(); //büyük harfe çevir
            string s2 = Seq2[1].ToUpper(); //büyük harfe çevir

            match = int.Parse(textBox3.Text);
            mis = int.Parse(textBox4.Text);
            gap = int.Parse(textBox5.Text);

            char[] dizi1 = new char[s1.Length]; 
            char[] dizi2 = new char[s2.Length]; 

            dizi1 = s1.ToCharArray(); 
            dizi2 = s2.ToCharArray(); 

            dataGridView1.ColumnCount = dizi1.Length + 1; //gridview yanlara yazma
            dataGridView1.RowCount = dizi2.Length + 1;
            dataGridView1.Columns[0].Name = "i";  
            dataGridView1.Rows[0].HeaderCell.Value = "j";
            //Gridview Satır ve Sütunları Doldurma
            for (int i = 0; i < dizi1.Length; i++)
            {
                //i ye aktar
                dataGridView1.Columns[i + 1].Name = dizi1[i].ToString(); //yazılan i+1 den i ye yazdır
            }
            for (int j = 0; j < dizi2.Length; j++)
            {   
                //j ye aktar
                dataGridView1.Rows[j + 1].HeaderCell.Value = dizi2[j].ToString(); //yazılan j+1 den j ye yazdır
            }

            int Tmax;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                //i ye değerleri aktar
                dataGridView1[i, 0].Value = 0; //ilk sütun 0 
            }
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                //j ye değerleri aktar
                dataGridView1[0, j].Value = 0; //ilk satır 0
            }
            for (int i = 1; i < dataGridView1.Columns.Count; i++) 
            {
                for (int j = 1; j < dataGridView1.Rows.Count; j++)
                {
                    if (dataGridView1.Columns[i].Name.ToString() == dataGridView1.Rows[j].HeaderCell.Value.ToString())
                    {
                        //Tmax Hesaplama
                        Tmax = Math.Max(int.Parse(dataGridView1[i - 1, j].Value.ToString()) + gap, //T(i-1,j)+gap hesaplanması
                        Math.Max(int.Parse(dataGridView1[i, j - 1].Value.ToString()) + gap, int.Parse(dataGridView1[i - 1, j - 1].Value.ToString()) + match));
                        //T(i,j-1)+gap hesaplanması                                         T(i-1,j-1)+match hesaplanması      
                        //sıfır ile doldurma
                        if (Tmax > 0) //sıfırdan büyükse
                        {
                            dataGridView1[i, j].Value = Tmax; //i ve j değerini Tmax a aktar
                        }
                        else //Tmax negatifse
                        {
                            dataGridView1[i, j].Value = 0; //değilse 0 
                        }
                    }
                    else
                    {
                        Tmax = Math.Max(int.Parse(dataGridView1[i - 1, j].Value.ToString()) + gap,Math.Max(int.Parse(dataGridView1[i, j - 1].Value.ToString()) 
                            + gap, int.Parse(dataGridView1[i - 1, j - 1].Value.ToString()) + mis));
                        //0 doldurma
                        if (Tmax > 0)
                        {
                            dataGridView1[i, j].Value = Tmax; //i ve j yi Tmax yap
                        }
                        else //Tmax negatifse
                        {
                            dataGridView1[i, j].Value = 0;//değiilse 0 
                        }

                    }
                }
            }
            Hizala(s1, s2);
            timer1.Stop();
        }

        public void Hizala(string s1, string s2)
        {
            int max = int.Parse(dataGridView1[0, 0].Value.ToString()); //datagridview [0, 0] değerine ata
            StringBuilder seq1_hiza = new StringBuilder(); 
            StringBuilder seq2_hiza = new StringBuilder(); 
            List<string> seq1_list = new List<string>(); 
            List<string> seq2_list = new List<string>(); 
            List<int> score_list = new List<int>(); 
            s1 = "-" + s1; 
            s2 = "-" + s2; 
           
            for (int i = 0; i < dataGridView1.Columns.Count; i++) 
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    if (int.Parse(dataGridView1[i, j].Value.ToString()) > max) 
                    {
                        max = int.Parse(dataGridView1[i, j].Value.ToString()); //max gridviewdeki değere aktar
                    }
                }
            }
            for (int i = 0; i < dataGridView1.Columns.Count; i++) 
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++) 
                {
                    if (int.Parse(dataGridView1[i, j].Value.ToString()) == max)//maxs eşitse
                    {
                        int x = i; 
                        int y = j; 
                        while (true)
                        {
                            int tmp_score = 0; 
                            //Geri İzleme Adımı
                            while (int.Parse(dataGridView1[x, y].Value.ToString()) != 0)
                            {
                                if (dataGridView1.Columns[x].Name.ToString() == dataGridView1.Rows[y].HeaderCell.Value.ToString())
                                {
                                    //Çapraz
                                    seq1_hiza.Insert(0, s1[x]);
                                    seq2_hiza.Insert(0, s2[y]);
                                    dataGridView1[x, y].Style.BackColor = Color.Gold; 
                                    tmp_score += match;
                                    x--;
                                    y--;
                                }
                                else
                                {
                                    //Çapraz Max 
                                    if (int.Parse(dataGridView1[x, y].Value.ToString()) == (int.Parse(dataGridView1[x, y - 1].Value.ToString()) + gap))
                                    {
                                        seq1_hiza.Insert(0, "-");
                                        seq2_hiza.Insert(0, s2[y]);
                                        dataGridView1[x, y].Style.BackColor = Color.Gold;
                                        tmp_score += gap;
                                        y--;
                                    }
                                    //Sol Max
                                    else if (int.Parse(dataGridView1[x, y].Value.ToString()) == (int.Parse(dataGridView1[x - 1, y - 1].Value.ToString()) + mis))
                                    {
                                        seq1_hiza.Insert(0, s1[x]);
                                        seq2_hiza.Insert(0, s2[y]);
                                        dataGridView1[x, y].Style.BackColor = Color.Gold;
                                        tmp_score += mis;
                                        x--;
                                        y--;
                                    }
                                    //Yukarı Max
                                    else if (int.Parse(dataGridView1[x, y].Value.ToString()) == (int.Parse(dataGridView1[x - 1, y].Value.ToString()) + gap)) 
                                    {
                                        seq1_hiza.Insert(0, s1[x]);
                                        seq2_hiza.Insert(0, "-");
                                        dataGridView1[x, y].Style.BackColor = Color.Gold; 
                                        tmp_score += gap;
                                        x--;
                                    }
                                }
                            }
                            score_list.Add(tmp_score); //skoru ekle
                            seq1_list.Add(seq1_hiza.ToString()); //hizalamak için listeye ekle
                            seq2_list.Add(seq2_hiza.ToString());
                            if (tmp_score >= score) //tmp skor büyük eşit skordan ise
                            {
                                score = tmp_score; //skoru tmp a aktar
                            }
                            else
                            {
                                continue; //değilse devam et
                            }
                            if (int.Parse(dataGridView1[x, y].Value.ToString()) == 0)
                            {
                                dataGridView1[x, y].Style.BackColor = Color.Gold; 
                                break;
                            }
                        }
                    }
                    else continue;
                }
            }
            for (int i = 0; i < score_list.Count; i++)
            {
                if (score_list[i] == score)
                {
                   textBox6.Text += seq1_list[i].ToString(); //yazdırma
                   textBox7.Text += seq2_list[i].ToString(); 
                }
            }
            textBox8.Text = score.ToString();
        }
     
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++;
            label7.Text = "Çalışma Süresi: " + counter.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
            try
            {
                if (File.Exists(@"C:\Users\Sevval\OneDrive\Masaüstü\2022-2023 BM DERSLER\BAHAR\Biyoinformatik\190508003-Şevval İMRE-Biyoinformatik Proje_2\BiyoInformatik_2\seq1.txt") && File.Exists(@"C:\Users\Sevval\OneDrive\Masaüstü\2022-2023 BM DERSLER\BAHAR\Biyoinformatik\190508003-Şevval İMRE-Biyoinformatik Proje_2\BiyoInformatik_2\seq2.txt"))
                {
                    Seq1 = File.ReadAllLines(@"C:\Users\Sevval\OneDrive\Masaüstü\2022-2023 BM DERSLER\BAHAR\Biyoinformatik\190508003-Şevval İMRE-Biyoinformatik Proje_2\BiyoInformatik_2\seq1.txt");//dosya okuma
                    textBox1.Text = Seq1[1];//dosyadaki veriyi textboxa yazdırma
                    Seq1_len = Convert.ToInt32(Seq1[0]);//dosyadaki verinin uzunluğu

                    Seq2 = File.ReadAllLines(@"C:\Users\Sevval\OneDrive\Masaüstü\2022-2023 BM DERSLER\BAHAR\Biyoinformatik\190508003-Şevval İMRE-Biyoinformatik Proje_2\BiyoInformatik_2\seq2.txt");
                    textBox2.Text = Seq2[1];
                    Seq2_len = Convert.ToInt32(Seq1[0]);
                }
                else

                    MessageBox.Show("Dosya Bulunamadı...", "Error");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata :" + ex.ToString(), "Error");
            }
        }
    }
}
