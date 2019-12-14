using System;
using System.Windows.Forms;
using NAudio.Wave;

namespace LoudSoundDetection
{
    public partial class LoudSoundDetection : Form
    {
        public LoudSoundDetection()
        {
            InitializeComponent();
        }

        WaveIn waveIn;
        float soundLevel;

        float continuousBig;

        private void Form1_Load(object sender, EventArgs e)
        {
            waveIn = new WaveIn();

            continuousBig = 0;

            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(waveIn_DataAvailable);
            waveIn.WaveFormat = new WaveFormat(8000, 2); //(sampleRate, channel)
            waveIn.StartRecording();
        }

        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                soundLevel = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]) / 32768f;
                /*
                if (soundLevel > 0.999)
                {
                    MessageBox.Show("It was loud.");
                    break;
                }
                */
                if (continuousBig < soundLevel)
                {
                    continuousBig = soundLevel;
                    if (continuousBig > 0.999)
                    {
                        MessageBox.Show("It was loud.");
                        continuousBig = 0;
                        break;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textSoundLevel.Text = soundLevel.ToString();
        }
    }
}