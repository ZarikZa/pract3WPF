using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Material_design
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<FileInfo> musikForList = new List<FileInfo>();
        private static int tekushiyTrek;
        Logic logic = new Logic();
        List<FileInfo> suffledForList = new List<FileInfo>();
        private static List<FileInfo> Lisend = new List<FileInfo>();
        private static string repeatmod = "нет";
        public MainWindow()
        {
            InitializeComponent();
            Thread potok = new Thread(_ =>
            {
                while (true)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (MediaEl.HasAudio)
                        {
                            AudioSlid.Value = MediaEl.Position.Duration().Ticks;
                            CurrentTimeTbl.Text = $"{MediaEl.Position.Hours}:{MediaEl.Position.Minutes}:{MediaEl.Position.Seconds}";
                            if (MediaEl.NaturalDuration.HasTimeSpan)
                            {
                                if (MediaEl.Position.Duration() == MediaEl.NaturalDuration.TimeSpan)
                                {
                                    if(repeatmod == "да")
                                    {
                                        MediaEl.Source = new Uri(MusikList.SelectedItem.ToString());
                                    }
                                    else
                                    {
                                        AudioSlid.Value = 0;
                                        if(tekushiyTrek == musikForList.Count - 1)
                                        {
                                            MusikList.SelectedIndex = 0;
                                        }
                                        else
                                        {
                                            MusikList.SelectedIndex += 1;
                                        }
                                    }
                                }
                            }
                        }
                    }));
                    Thread.Sleep(1000);
                }
            });
            potok.Start();
            ZvukSlid.Value = MediaEl.Volume; 
        }

        private void RepeatButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (RepeatIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.Repeat)
            {
                repeatmod = "нет";
                RepeatIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.RepeatOff;
            }
            else
            {
                repeatmod = "да";
                RepeatIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Repeat;
            }
        }

        private void PlayBtm_Click(object sender, RoutedEventArgs e)
        {
            if (PlayIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.Play)
            {
                MediaEl.Play();
                PlayIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
            }
            else
            {
                MediaEl.Pause();
                PlayIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            }
        }

        private void ShuffleBtm_Click(object sender, RoutedEventArgs e)
        {
            if (ShuffleIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.Shuffle)
            {
                musikForList = suffledForList;
                MusikList.ItemsSource = musikForList;
                ShuffleIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.ShuffleDisabled;
            }
            else
            {
                musikForList = logic.SuffleForList(musikForList);
                MusikList.ItemsSource = musikForList;
                ShuffleIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Shuffle;
            }
        }

        private void ZvukSlid_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaEl.Volume = ZvukSlid.Value;
        }

        private void LoadMusikBtm_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                musikForList = Directory.GetFiles(dialog.FileName).Select(item => new FileInfo(item)).ToList();
                for (int i = 0; i <= musikForList.Count; i++)
                {
                    if (musikForList[i].ToString().Contains(".mp3") == false)
                    {
                        musikForList.Remove(musikForList[i]);
                    }

                }
                MusikList.ItemsSource = musikForList;
                suffledForList = musikForList;
            }
            MusikList.SelectedIndex = 0;
            MediaEl.Source = new Uri(MusikList.SelectedItem.ToString());
            MediaEl.Play();
        }

        private void MediaEl_MediaOpened(object sender, RoutedEventArgs e)
        {
            AudioSlid.Maximum = MediaEl.NaturalDuration.TimeSpan.Ticks;
            MaxTimeTbl.Text = MediaEl.NaturalDuration.TimeSpan.ToString();
            Lisend.Add((FileInfo)MusikList.SelectedItem);
        }

        private void MusikList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tekushiyTrek = MusikList.SelectedIndex;
            MediaEl.Source = new Uri(MusikList.SelectedItem.ToString());
        }

        private void AudioSlid_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaEl.Position = new TimeSpan(Convert.ToInt64(AudioSlid.Value));
        }

        private void BackBtm_Click(object sender, RoutedEventArgs e)
        {
            if(tekushiyTrek > 0)
            {
                MusikList.SelectedIndex -= 1;
            }
            else
            {
                MusikList.SelectedItem = musikForList.Last();
            }
                
            MediaEl.Source = new Uri(MusikList.SelectedItem.ToString());
        }

        private void NextBtm_Click(object sender, RoutedEventArgs e)
        {
            if(tekushiyTrek <= musikForList.Count - 2)
            {
                MusikList.SelectedIndex += 1;
            }
            else
            {
                MusikList.SelectedItem = musikForList.First();
            }
            MediaEl.Source = new Uri(MusikList.SelectedItem.ToString());
        }

        private void HystoryBtm_Click(object sender, RoutedEventArgs e)
        {
            History hystory = new History(Lisend);
            hystory.ShowDialog();
            MusikList.SelectedItem = hystory.result;
        }
    }
}