using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Plugin.FilePicker;

namespace WoTB_APK_Editor.Resources.layout
{
    [Activity(Label = "戦闘BGM")]
    public partial class Music_Activity : Activity
    {
        string File_Name_PickFile;
        string Download_Path = "/storage/emulated/0/Android/data/WoTB_APK_Editor.WoTB_APK_Editor/files";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            var w = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            var h = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Music_Layout);
            Button BGM_Exit_B = FindViewById<Button>(Resource.Id.BGM_Exit_B);
            Button BGM_Select_B = FindViewById<Button>(Resource.Id.BGM_Select_B);
            Button Sample_BGM_B = FindViewById<Button>(Resource.Id.Sample_BGM_B);
            Button Select_BGM_File_B = FindViewById<Button>(Resource.Id.Select_BGM_File_B);
            Button Change_Sample_BGM_B = FindViewById<Button>(Resource.Id.Change_Sample_BGM_B);
            ProgressBar Download_Progress = FindViewById<ProgressBar>(Resource.Id.Download_Progress);
            Download_Progress.Visibility = ViewStates.Invisible;
            BGM_Exit_B.Click += async delegate
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                if (mediaplayer.IsPlaying == true)
                {
                    float a = 1.0f;
                    while (true)
                    {
                        if (a <= 0f)
                        {
                            mediaplayer.Stop();
                            break;
                        }
                        a = a - 0.01f;
                        mediaplayer.SetVolume(a, a);
                        await Task.Delay(10);
                    }
                }
            };
            BGM_Select_B.SetX(0 - w / 10);
            Sample_BGM_B.SetX(0 - w / 10);
            Select_BGM_File_B.SetX(0 - w / 10);
            Change_Sample_BGM_B.SetX(0 - w / 10);
            Download_Progress.SetX(0 - w / 10);
            BGM_Select_B.Click += delegate
            {
                BGM_Select();
            };
            Sample_BGM_B.Click += delegate
            {
                BGM_Sample();
            };
            Change_Sample_BGM_B.Click += delegate
            {
                Sample_BGM_Change();
            };
            Select_BGM_File_B.Click += delegate
            {
                Select_BGM_File();
            };
            Sample_BGM_Change();
        }
        WebClient wc;
        void BGM_Select()
        {
            Button BGM_Exit_B = FindViewById<Button>(Resource.Id.BGM_Exit_B);
            Button BGM_Select_B = FindViewById<Button>(Resource.Id.BGM_Select_B);
            Button Select_BGM_File_B = FindViewById<Button>(Resource.Id.Select_BGM_File_B);
            Button Change_Sample_BGM_B = FindViewById<Button>(Resource.Id.Change_Sample_BGM_B);
            ProgressBar Download_Progress = FindViewById<ProgressBar>(Resource.Id.Download_Progress);
            Download_Progress.Visibility = ViewStates.Visible;
            BGM_Exit_B.Visibility = ViewStates.Invisible;
            Change_Sample_BGM_B.Visibility = ViewStates.Invisible;
            Select_BGM_File_B.Visibility = ViewStates.Invisible;
            BGM_Select_B.Visibility = ViewStates.Invisible;
            Directory.CreateDirectory(Download_Path + "/Download_Mods");
            int fCount = Directory.GetFiles(Download_Path + "/Download_Mods", "*", SearchOption.TopDirectoryOnly).Length;
            count = fCount + 1;
            string Uri2 = "";
            if (BGM_Select_B.Text == "TWO STEPS FROM HEll")
            {
                Uri2 = "https://www.dropbox.com/s/eb7cdzt92eke0uc/Two_Steps_From_Hell.zip?dl=1";
            }
            else if (BGM_Select_B.Text == "GUP_BGM_MOD")
            {
                Uri2 = "https://www.dropbox.com/s/hokil3o63ikud48/GuP_BGM.zip?dl=1";
            }
            else if (BGM_Select_B.Text == "艦これBGM")
            {
                Uri2 = "https://www.dropbox.com/s/6x7paczm5iqjgcm/Kankore_BGM.zip?dl=1";
            }
            Uri uri = new Uri(Uri2);
            if (wc == null)
            {
                wc = new WebClient();
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            }
            wc.DownloadFileAsync(uri, Download_Path + "/Download_Mods/Download_Music_Mod.dat");
            if (Directory.Exists(Download_Path + "/Download_Mods/Download_Music_Mod"))
            {
                Directory.Delete(Download_Path + "/Download_Mods/Download_Music_Mod", true);
            }
        }
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar Download_Progress = FindViewById<ProgressBar>(Resource.Id.Download_Progress);
            Download_Progress.Progress = e.ProgressPercentage;
        }
        double count = 0;
        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if ((e.Error != null) && (!e.Cancelled))
            {
                Toast.MakeText(this, "ダウンロード中にエラーが発生しました。\n内容:" + e.Error.Message, ToastLength.Long).Show();
            }
            else if (e.Cancelled)
            {
                Toast.MakeText(this, "ダウンロードがキャンセルされました。", ToastLength.Short).Show();
            }
            else
            {
                Task.Delay(500);
                Task task = Task.Run(() =>
                {
                    string filePath = Download_Path + "/Download_Mods/Download_Music_Mod.dat";
                    string folderPath = Download_Path + "/Download_Mods/Download_Music_Mod";
                    ICSharpCode.SharpZipLib.Zip.FastZip zip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                    zip.ExtractZip(filePath, folderPath, "");
                });
                task.Wait();
                File.Delete(Download_Path + "/Download_Mods/Download_Music_Mod.dat");
                Mods_Get();
            }
        }
        void Mods_Get()
        {
            Button BGM_Exit_B = FindViewById<Button>(Resource.Id.BGM_Exit_B);
            Button BGM_Select_B = FindViewById<Button>(Resource.Id.BGM_Select_B);
            Button Select_BGM_File_B = FindViewById<Button>(Resource.Id.Select_BGM_File_B);
            Button Change_Sample_BGM_B = FindViewById<Button>(Resource.Id.Change_Sample_BGM_B);
            ProgressBar Download_Progress = FindViewById<ProgressBar>(Resource.Id.Download_Progress);
            try
            {
                Directory.CreateDirectory(Download_Path + "/WoTB_Files/assets/Data/Mods");
                IEnumerable<string> files = Directory.EnumerateFiles(Download_Path + "/Download_Mods/Download_Music_Mod", "*", SearchOption.AllDirectories);
                foreach (string f in files)
                {
                    string Path_Name = Path.GetFileName(f);
                    if (f.Contains(".fev") || f.Contains(".fsb"))
                    {
                        File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/Mods/" + Path_Name, true);
                    }
                }
                Change_Sounds();
                Toast.MakeText(this, BGM_Select_B.Text + "が適応されました。", ToastLength.Short).Show();
                Download_Progress.Visibility = ViewStates.Invisible;
                BGM_Exit_B.Visibility = ViewStates.Visible;
                Select_BGM_File_B.Visibility = ViewStates.Visible;
                Change_Sample_BGM_B.Visibility = ViewStates.Visible;
                BGM_Select_B.Visibility = ViewStates.Visible;
            }
            catch
            {
                Toast.MakeText(this, "エラーが発生しました。\n開発者にご連絡ください。", ToastLength.Short).Show();
                Download_Progress.Visibility = ViewStates.Invisible;
                BGM_Exit_B.Visibility = ViewStates.Visible;
                BGM_Select_B.Visibility = ViewStates.Visible;
                Select_BGM_File_B.Visibility = ViewStates.Visible;
                Change_Sample_BGM_B.Visibility = ViewStates.Visible;
            }
        }
        void Sample_BGM_Change()
        {
            Button BGM_Select_B = FindViewById<Button>(Resource.Id.BGM_Select_B);
            Random r = new Random();
            int r2 = r.Next(1, 4);
            if (r2 == 1)
            {
                if (BGM_Select_B.Text == "TWO STEPS FROM HEll")
                {
                    Sample_BGM_Change();
                }
                else
                {
                    BGM_Select_B.Text = "TWO STEPS FROM HEll";
                }
            }
            else if (r2 == 2)
            {
                if (BGM_Select_B.Text == "GUP_BGM_MOD")
                {
                    Sample_BGM_Change();
                }
                else
                {
                    BGM_Select_B.Text = "GUP_BGM_MOD";
                }
            }
            else if (r2 == 3)
            {
                if (BGM_Select_B.Text == "艦これBGM")
                {
                    Sample_BGM_Change();
                }
                else
                {
                    BGM_Select_B.Text = "艦これBGM";
                }
            }
        }
        async void Select_BGM_File()
        {
            try
            {
                Directory.CreateDirectory(Download_Path + "/Download_Mods");
                var file = await CrossFilePicker.Current.PickFile();
                File_Name_PickFile = file.FileName;
                if (Directory.Exists(Download_Path + "/Download_Mods/Select_Mod"))
                {
                    Directory.Delete(Download_Path + "/Download_Mods/Select_Mod", true);
                }
                await Task.Delay(100);
                try
                {
                    FileStream fs = new FileStream(Download_Path + "/Download_Mods/" + file.FileName, FileMode.CreateNew);
                    byte[] data = file.DataArray;
                    fs.Write(data, 0, data.Length);
                    fs.Close();
                }
                catch
                {
                    Toast.MakeText(this, "エラーが発生しました。", ToastLength.Short).Show();
                }
                try
                {
                    Task task_02 = Task.Run(() =>
                    {
                        System.IO.Compression.ZipFile.ExtractToDirectory(Download_Path + "/Download_Mods/" + file.FileName, Download_Path + "/Download_Mods/Select_Mod");
                    });
                    task_02.Wait();
                }
                catch
                {
                    Toast.MakeText(this, "異なったファイルが選択されました。", ToastLength.Short).Show();
                    File.Delete(Download_Path + "/Download_Mods/" + file.FileName);
                }
                Select_Mod_Get();
                file.Dispose();
            }
            catch
            {

            }
        }
        async void Select_Mod_Get()
        {
            bool Music_OK = false;
            try
            {
                if (Directory.Exists(Download_Path + "/Download_Mods/Select_Mod"))
                {
                    IEnumerable<string> files = Directory.EnumerateFiles(Download_Path + "/Download_Mods/Select_Mod", "*", SearchOption.AllDirectories);
                    foreach (string f in files)
                    {
                        string File_Path = f.Replace(Download_Path + "/Download_Mods/Select_Mod", "");
                        string File_Name_Path = Path.GetFileName(f);
                        if (File_Path.Contains(".dvpl"))
                        {
                            AlertDialog.Builder g = new AlertDialog.Builder(this);
                            g.SetTitle("警告");
                            g.SetMessage(".dvpl形式のModは導入できません。");
                            g.Show();
                            break;
                        }
                        else
                        {
                            if (File_Path.Contains("/Music.fev") || File_Path.Contains("/Music.fsb"))
                            {
                                Music_OK = true;
                                File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/Mods/" + File_Name_Path, true);
                            }
                        }
                    }
                    await Task.Delay(100);
                    if (Music_OK == true)
                    {
                        Change_Sounds();
                        Toast.MakeText(this, "完了しました。", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Music.fev又はMusic.fsbが見つかりませんでした。", ToastLength.Short).Show();
                    }
                    File.Delete(Download_Path + "/Download_Mods/" + File_Name_PickFile);
                }
            }
            catch
            {
                Toast.MakeText(this, "エラーが発生しました。\n開発者にご連絡ください。", ToastLength.Short).Show();
            }
        }
        void Change_Sounds()
        {
            StreamReader str = new StreamReader(Download_Path + "/WoTB_Files/assets/Data/Configs/Sfx/sfx_high.yaml");
            string str2 = str.ReadToEnd();
            str.Close();
            StreamReader str3 = new StreamReader(Download_Path + "/WoTB_Files/assets/Data/Configs/Sfx/sfx_low.yaml");
            string str4 = str3.ReadToEnd();
            str3.Close();
            if (!str2.Contains("/Music.fev"))
            {
                StreamWriter stw = new StreamWriter(Download_Path + "/WoTB_Files/assets/Data/Configs/Sfx/sfx_high.yaml");
                stw.Write(str2 + "\n -\n  \"~res:/Mods/Music.fev\"");
                stw.Close();
                StreamWriter stw2 = new StreamWriter(Download_Path + "/WoTB_Files/assets/Data/Configs/Sfx/sfx_low.yaml");
                stw2.Write(str4 + "\n -\n  \"~res:/Mods/Music.fev\"");
                stw2.Close();
            }
            StreamReader sound = new StreamReader(Download_Path + "/WoTB_Files/assets/Data/sounds.yaml");
            string sound2 = sound.ReadToEnd();
            sound.Close();
            if (!sound2.Contains("VOICE_START_BATTLE: \"Music/Music/Music\""))
            {
                StreamWriter sound3 = File.CreateText(Download_Path + "/sounds.yaml");
                StreamReader sr = new StreamReader(Download_Path + "/WoTB_Files/assets/Data/sounds.yaml", Encoding.GetEncoding("UTF-8"));
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    if (line.Contains("VOICE_START_BATTLE"))
                    {
                        sound3.WriteLine("    VOICE_START_BATTLE: \"Music/Music/Music\"");
                    }
                    else
                    {
                        sound3.WriteLine(line);
                    }
                }
                sr.Close();
                sound3.Close();
                File.Copy(Download_Path + "/sounds.yaml", Download_Path + "/WoTB_Files/assets/Data/sounds.yaml", true);
                File.Delete(Download_Path + "/sounds.yaml");
            }
        }
    }
}