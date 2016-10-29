using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_Homework_1
{
    public partial class Form1 : Form
    {
        private bool isRunning;//Timer(yani programın) çalışma-durma durumunu tutan değişken
        private int second;//Ekrana yazılacak saniye
        private int milisecond;//Ekrana yazılacak milisaniye(100 ms)
        private Process process;//Gerçek bir process'i simule edecek process nesnesi
        private List<Button> buttonList = new List<Button>();

        public Form1()
        {
            //Form1 nesnesi oluşturulurken, gerekli değişkenlere ilk değerleri atanır.
           
            isRunning = false;
            process = null;
            
            InitializeComponent();
        }

        //Form yüklendiğinde ayarlanması gereken değerleri ayarlar.
        private void Form1_Load(object sender, EventArgs e)
        {
            actionLabel.Text = string.Empty;

            //Renk ayarlama kısmında işimize yarayacak button listesini dolduralım.
            buttonList.Add(newButton);
            buttonList.Add(readyButton);
            buttonList.Add(workingButton);
            buttonList.Add(waitingButton);
            buttonList.Add(terminatedButton);
        }

        // Başlat/Durdur durumunu kontrol eden butonun tıklama olayı
        private void button1_Click(object sender, EventArgs e)
        {
            //Process null ise yeni process oluştur.
            if(process==null)
            {
                process = new Process();
                AdjustDisplay();
                milisecond = 0;
                second = 0;
            }
            
            if (!isRunning) //Timer çalışmıyorsa
            {
                isRunning = true;
                timer1.Start();
                button1.Text = "DURDUR";
            }
            else //Timer çalışıyorsa
            {
                isRunning = false;
                timer1.Stop();
                button1.Text = "DEVAM ET";
            }
        }

        //Timer her çalıştığında (her 100 ms'de 1) çalışacak kod
        private void timer1_Tick(object sender, EventArgs e)
        {
            milisecond++;
            if(milisecond%10==0)
            {
                milisecond = 0;
                second++;

                //Her 2 saniyede 1 random hareket edelim.
                if(second%3==0)
                {
                    process.MoveOnRandomly();
                    AdjustDisplay();
                }
            }
            PrintTime();//Zamanı yazdır.
        }

        //Zamanı yazdırmaya yarayan method.
        private void PrintTime()
        {
            if (second < 10)
            {
                label1.Text = "0" + second + ":" + milisecond;
            }
            else
            {
                label1.Text = second + ":" + milisecond;
            }
        }

        //Process hareketi ve durumuna göre ekranı ayarlayan method.
        private void AdjustDisplay()
        {
            //Process'in action'ına göre label'ı ayarlar.
            switch (process.getAction)
            {
                case Process.Action.ProcessCreated:
                    actionLabel.Text = "Process Oluşturuldu.";
                    break;
                case Process.Action.ProcessAdmitted:
                    actionLabel.Text = "Process Kabul Edildi.";
                    break;
                case Process.Action.IOEvent:
                    actionLabel.Text = "I/O İşlemi Yapılıyor.";
                    break;
                case Process.Action.IOEventCompletion:
                    actionLabel.Text = "I/O İşlemi Tamamlandı.";
                    break;
                case Process.Action.SchedulerPatcher:
                    actionLabel.Text = "Zamanlama Ayarlandı.";
                    break;
                case Process.Action.Interrupt:
                    actionLabel.Text = "İşletim Sistemi Kesme Gönderdi";
                    break;
                case Process.Action.Exit:
                    actionLabel.Text = "Process Kapatılıyor.";
                    break;
            }

            //Yazıya göre boyutu değişen label'i ortalar.
            AlignCenter(actionLabel);

            //Process'in durumuna göre button renklerini ayarlar.
            switch (process.getStatus)
            {
                case Process.Status.New:
                    SetButtonsColor(newButton);
                    break;
                case Process.Status.Ready:
                    SetButtonsColor(readyButton);
                    break;
                case Process.Status.Running:
                    SetButtonsColor(workingButton);
                    break;
                case Process.Status.Waiting:
                    SetButtonsColor(waitingButton);
                    break;
                case Process.Status.Terminated:
                    SetButtonsColor(terminatedButton);
                    TerminateProcess();
                    break;
            }

        }

        //Process'in durumuna göre button renklerini ayarlayan method.
        private void SetButtonsColor(Button selectedButton)
        {
            foreach (Button btn in buttonList)
            {
                btn.ForeColor = Color.Black;
                btn.BackColor = Color.White;
            }
            selectedButton.ForeColor = Color.White;
            selectedButton.BackColor = Color.Green;    
        }

        //Process sona erdiğinde gerekli ayarları yapar.
        private void TerminateProcess()
        {
            process = null;
            isRunning = false;
            timer1.Stop();
            button1.Text = "YENİDEN BAŞLAT";
        }

        //Parametre olarak verilen form kontrolünü form'a göre ortalar.
        private void AlignCenter(Control control)
        {
            control.Location = new Point((Size.Width - control.Size.Width) / 2,control.Location.Y);
        }

    }
}
