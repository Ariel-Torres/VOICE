using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Speech.Recognition;
using System.Speech.Synthesis;
namespace ReconocimientoVoz
{
    public partial class Form1 : Form
    {
        string escuchado;
        SpeechRecognitionEngine reconoce = new SpeechRecognitionEngine();//reconocedor
        SpeechSynthesizer asistente = new SpeechSynthesizer();//el asistente habla
        bool habilitareconocimiento = true;
        string voz;
        public Form1()
        {
            InitializeComponent();
            asistente.Speak("iniciando");
            gramaticas();
            cargavoces();
        }
        void speakoutpout(string texto)
        {
            if (voz != null)
            {
                asistente.SelectVoice(voz);
                asistente.Speak(texto);
            }
            else
            {
                asistente.SelectVoice(voz);
            }
        }
        void gramaticas()
        {//sentencias de reconocimiento
            reconoce.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"archivostexto\comandos.txt")))));//carga los comandos
            reconoce.RequestRecognizerUpdate();//da un refresh al reconocimiento
            reconoce.SpeechRecognized += Reconoce_SpeechRecognized;//evento de reconocimiento
            asistente.SpeakStarted += Asistente_SpeakStarted;
            reconoce.AudioLevelUpdated += Reconoce_AudioLevelUpdated;
            asistente.SpeakCompleted += Asistente_SpeakCompleted;
            reconoce.SetInputToDefaultAudioDevice();//utilizaremos el audio que lea nuestro microfono
            reconoce.RecognizeAsync(RecognizeMode.Multiple);//si queremos desactivarlo utilizaremos esto
        }

        private void Reconoce_AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e)
        {
            int nivel = e.AudioLevel;
            int nivelresul = nivel * 20;
            if (nivelresul <= 100)
            {
                progressBar1.Value = nivelresul;
            }
            else
            {
                progressBar1.Value = 100;
            }


        }

        void cargavoces()
        {
            int carga = 0;
            foreach (InstalledVoice voces in asistente.GetInstalledVoices())
            {
                comboBox1.Items.Add(voces.VoiceInfo.Name);
                if (carga == 0)
                {
                    voz = voces.VoiceInfo.Name;
                }

            }

        }

        private void Asistente_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            habilitareconocimiento = true;
        }

        private void Asistente_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            habilitareconocimiento = false;
        }

        private void Reconoce_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            escuchado = e.Result.Text;// lo reconocido atravez del evento lo guarda en una variable que llamaremos escuchado
            habilitareconocimiento = false;
            switch (escuchado)
            {
                case ("buenos dias asistente"):
                    {
                        speakoutpout("buenos dias señor" + " . .");
                        label1.Text = "";
                        label1.Text = escuchado;
                    }
                    break;
                case ("abrir el google"):
                    {
                        speakoutpout("abriendo el google" + ". . ");
                        System.Diagnostics.Process.Start("https://www.google.com/");
                        label1.Text = "";
                        label1.Text = escuchado;
                    }
                    break;
                case ("adios asistente"):
                    {
                        speakoutpout("hasta luego señor" + ". . ");
                        Application.Exit();
                        //Close();
                    }
                    break;
                case ("abrir facebook"):
                    {
                        speakoutpout("abriendo facebook" + ". . ");
                        System.Diagnostics.Process.Start("https://www.facebook.com/");
                        label1.Text = "";

                        label1.Text = escuchado;
                    }
                    break;

                default:
                    break;
            }
            habilitareconocimiento = true;
        }
    }
}
