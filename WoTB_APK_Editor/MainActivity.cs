using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WoTB_APK_Editor
{
    [Activity(Label = "WoTB_APK_Editor", Theme = "@style/AppTheme", MainLauncher = true)]
    public partial class MainActivity : Activity
    {
        private static string GetPathForDCIM()
        {
            return Directory.GetCurrentDirectory();
        }
        bool Tenkai_OK = false;
        string Download_Path = "/storage/emulated/0/Android/data/WoTB_APK_Editor.WoTB_APK_Editor/files";
        string WoTB_Path = "";
        int APK_Files_Max = 1;
        bool OK = true;
        bool Kankore_Voice_Mod = true;
        protected override async void OnCreate(Bundle savedInstanceState) 
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Button APK_Tenkai_B = FindViewById<Button>(Resource.Id.APK_Tenkai_B);
            Button Test_B = FindViewById<Button>(Resource.Id.Test_B);
            Button Kankore_B = FindViewById<Button>(Resource.Id.Kankore_B);
            Button Backup_B = FindViewById<Button>(Resource.Id.Backup_B);
            Button Zip_Select_B = FindViewById<Button>(Resource.Id.Zip_Select_B);
            Button Zip_Help_B = FindViewById<Button>(Resource.Id.Zip_Help_B);
            Button Sample_Voice_B = FindViewById<Button>(Resource.Id.Sample_Voice_B);
            Button BGM_B = FindViewById<Button>(Resource.Id.BGM_B);
            Button Change_Sample_Voice_B = FindViewById<Button>(Resource.Id.Change_Sample_Voice_B);
            ProgressBar Wait_P = FindViewById<ProgressBar>(Resource.Id.Wait_P);
            TextView Wait_Text = FindViewById<TextView>(Resource.Id.Wait_Text);
            ProgressBar Kankore_P = FindViewById<ProgressBar>(Resource.Id.Kankore_P);
            Kankore_P.Max = 100;
            Zip_Help_B.Visibility = Android.Views.ViewStates.Invisible;
            Zip_Select_B.Visibility = Android.Views.ViewStates.Invisible;
            Wait_P.Visibility = Android.Views.ViewStates.Invisible;
            Test_B.Visibility = Android.Views.ViewStates.Invisible;
            Wait_Text.Visibility = Android.Views.ViewStates.Invisible;
            Kankore_B.Visibility = Android.Views.ViewStates.Invisible;
            Kankore_P.Visibility = Android.Views.ViewStates.Invisible;
            Backup_B.Visibility = Android.Views.ViewStates.Invisible;
            Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
            Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
            BGM_B.Visibility = Android.Views.ViewStates.Invisible;
            var w = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);
            var h = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            APK_Tenkai_B.SetX(w / 3);
            Zip_Help_B.SetY(0 + h / 5);
            Zip_Help_B.SetX(-w - w / 2);
            Zip_Select_B.SetX(0);
            Kankore_B.SetX(0 - (w / 3));
            Kankore_B.SetY(h / 3 - (h / 8));
            Change_Sample_Voice_B.SetX(0 - (w / 3));
            Change_Sample_Voice_B.SetY(h / 3 - (h / 8));
            Sample_Voice_B.SetX(0 - w / 5);
            Sample_Voice_B.SetY(h / 3 - (h / 6));
            Kankore_P.SetX(0 - w / 3);
            Wait_P.SetX(0 - w / 8);
            Backup_B.SetX(0 - w / 3);
            Test_B.SetX(0 - w / 3);
            Wait_Text.SetX(0 - w / 3);
            BGM_B.SetX(0 - w / 3);
            //WoTBのAPK場所を取得(めっちゃ時間かかった。。。)
            Context context = this;
            Intent mainIntent = new Intent(Intent.ActionMain, null);
            mainIntent.AddCategory(Intent.CategoryLauncher);
            mainIntent.SetPackage("net.wargaming.wot.blitz");
            IList<ResolveInfo> pkgAppsList = context.PackageManager.QueryIntentActivities(mainIntent, 0);
            foreach (object ok in pkgAppsList)
            {
                ResolveInfo info = (ResolveInfo)ok;
                WoTB_Path = info.ActivityInfo.ApplicationInfo.PublicSourceDir.Replace("/base.apk", "");
                break;
            }
            try
            {
                Runtime.GetRuntime().Exec(new string[] { "su", "-c", "cp " + WoTB_Path + "/oat/arm64/base.odex " + Download_Path + "/WoTB_Exist.dat" });
            }
            catch
            {

            }
            await Task.Delay(100);
            if (File.Exists(Download_Path + "/WoTB_Exist.dat"))
            {
                File.Delete(Download_Path + "/WoTB_Exist.dat");
            }
            else
            {
                Android.App.AlertDialog.Builder f = new Android.App.AlertDialog.Builder(this);
                f.SetTitle("警告");
                f.SetMessage("Root、又はWoTBにアクセスできません。");
                f.Show();
            }
            if (File.Exists(Download_Path + "/WoTB_Files/resources.arsc"))
            {
                APK_Tenkai_B.Visibility = Android.Views.ViewStates.Invisible;
                Kankore_B.Visibility = Android.Views.ViewStates.Visible;
                Test_B.Visibility = Android.Views.ViewStates.Visible;
                Zip_Help_B.Visibility = Android.Views.ViewStates.Visible;
                Zip_Select_B.Visibility = Android.Views.ViewStates.Visible;
                Backup_B.Visibility = Android.Views.ViewStates.Visible;
                Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
                BGM_B.Visibility = Android.Views.ViewStates.Visible;
                Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
            }
            if (!File.Exists(Download_Path + "/Backup/base.apk"))
            {
                try
                {
                    Directory.CreateDirectory(Download_Path + "/Backup");
                    File.Copy(WoTB_Path + "/base.apk", Download_Path + "/Backup/base.apk", true);
                }
                catch
                {

                }
            }
            if (File.Exists(Download_Path + "/Download_Mods/Kankore_Voice_Mod/sounds.yaml"))
            {
                Kankore_Voice_Mod = true;
            }
            APK_Tenkai_B.Click += delegate
            {
                try
                {
                    Directory.CreateDirectory("/storage/emulated/0/Android/data/WoTB_APK_Editor.WoTB_APK_Editor/files");
                    Tenkai_OK = true;
                }
                catch
                {
                    Android.App.AlertDialog.Builder f = new Android.App.AlertDialog.Builder(this);
                    f.SetTitle("警告");
                    f.SetMessage("ストレージにアクセスできません。\n設定からストレージのアクセス許可を行ってください。");
                    f.Show();
                    Tenkai_OK = false;
                }
                if (Tenkai_OK == true)
                {
                    OK = false;
                    Wait_P.Visibility = Android.Views.ViewStates.Visible;
                    Wait_Text.Visibility = Android.Views.ViewStates.Visible;
                    Kankore_P.Visibility = Android.Views.ViewStates.Invisible;
                    Test_B.Visibility = Android.Views.ViewStates.Invisible;
                    Backup_B.Visibility = Android.Views.ViewStates.Invisible;
                    Zip_Help_B.Visibility = Android.Views.ViewStates.Invisible;
                    Zip_Select_B.Visibility = Android.Views.ViewStates.Invisible;
                    APK_Tenkai_B.Visibility = Android.Views.ViewStates.Invisible;
                    Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
                    BGM_B.Visibility = Android.Views.ViewStates.Invisible;
                    Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
                    APK_Tenkai();
                }
            };
            Kankore_B.Click += delegate
            {
                if (File.Exists(Download_Path + "/Download_Mods/Kankore_Voice_Mod/sounds.yaml"))
                {
                    Mods_Get();
                }
                else
                {
                    Kankore_Mod();
                }
            };
            Test_B.Click += delegate
            {
                if (Kankore_Voice_Mod == true)
                {
                    File_OK();
                }
                else
                {
                    Toast.MakeText(this, "Modが入っていません。", ToastLength.Short).Show();
                }
            };
            Backup_B.Click += delegate
            {
                try
                {
                    Task task = Task.Run(() =>
                    {
                        Runtime.GetRuntime().Exec(new string[] { "su", "-c", "cp " + Download_Path + "/Backup/base.apk /data/app/net.wargaming.wot.blitz-1/base.apk" });
                    });
                    task.Wait();
                    Toast.MakeText(this, "デフォルトのAPKを適応しました。", ToastLength.Short).Show();
                }
                catch
                {
                    Toast.MakeText(this, "Rootにアクセスできないためこの機能は使用できません。", ToastLength.Short).Show();
                }
            };
            Zip_Help_B.Click += delegate
            {
                Zip_Help_Dialog();
            };
            Zip_Select_B.Click += delegate
            {
                Zip_Select_Path();
            };
            Sample_Voice_B.Click += delegate
            {
                Sample_Voice_Click();
            };
            BGM_B.Click += delegate
            {
                var intent = new Intent(this, typeof(Resources.layout.Music_Activity));
                StartActivity(intent);
            };
            Change_Sample_Voice_B.Click += delegate
            {
                Sample_Voice_Mod_Random();
            };
        }
        async void APK_Tenkai()
        {
            Button APK_Tenkai_B = FindViewById<Button>(Resource.Id.APK_Tenkai_B);
            ProgressBar Wait_P = FindViewById<ProgressBar>(Resource.Id.Wait_P);
            Button Kankore_B = FindViewById<Button>(Resource.Id.Kankore_B);
            Button Test_B = FindViewById<Button>(Resource.Id.Test_B);
            Button Backup_B = FindViewById<Button>(Resource.Id.Backup_B);
            Button Zip_Select_B = FindViewById<Button>(Resource.Id.Zip_Select_B);
            Button Zip_Help_B = FindViewById<Button>(Resource.Id.Zip_Help_B);
            Button BGM_B = FindViewById<Button>(Resource.Id.BGM_B);
            TextView Wait_Text = FindViewById<TextView>(Resource.Id.Wait_Text);
            Button Sample_Voice_B = FindViewById<Button>(Resource.Id.Sample_Voice_B);
            Button Change_Sample_Voice_B = FindViewById<Button>(Resource.Id.Change_Sample_Voice_B);
            Directory.CreateDirectory(Download_Path + "/WoTB_Files");
            Task Copy = Task.Run(() =>
            {
                try
                {
                    File.Copy(WoTB_Path + "/base.apk", Download_Path + "/WoTB.apk", true);
                }
                catch
                {
                    Android.App.AlertDialog.Builder f = new Android.App.AlertDialog.Builder(this);
                    f.SetTitle("警告");
                    f.SetMessage("ストレージにアクセスできません。\n設定からストレージのアクセス許可を行ってください。");
                    f.Show();
                }
            });
            Copy.Wait();
            Task task = Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(Download_Path + "/WoTB.apk", Download_Path + "/WoTB_Files");
            });
            ZipArchive archive = ZipFile.Open(Download_Path + "/WoTB.apk", ZipArchiveMode.Read);
            foreach (var entry in archive.Entries)
            {
                if (!string.IsNullOrEmpty(entry.Name))
                {
                    APK_Files_Max += 1;
                }
            }
            await Task.Delay(100);
            while (true)
            {
                string[] files = Directory.GetFiles(Download_Path + "/WoTB_Files", "*", SearchOption.AllDirectories);
                int b = files.Count() + 1;
                if (b == APK_Files_Max)
                {
                    OK = true;
                    Wait_P.Visibility = Android.Views.ViewStates.Invisible;
                    Wait_Text.Visibility = Android.Views.ViewStates.Invisible;
                    APK_Tenkai_B.Visibility = Android.Views.ViewStates.Invisible;
                    Test_B.Visibility = Android.Views.ViewStates.Visible;
                    Kankore_B.Visibility = Android.Views.ViewStates.Visible;
                    Backup_B.Visibility = Android.Views.ViewStates.Visible;
                    Zip_Help_B.Visibility = Android.Views.ViewStates.Visible;
                    Zip_Select_B.Visibility = Android.Views.ViewStates.Visible;
                    Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
                    BGM_B.Visibility = Android.Views.ViewStates.Visible;
                    Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
                    Toast.MakeText(this, "展開しました。", ToastLength.Short).Show();
                    break;
                }
                else
                {
                    if (OK == false)
                    {
                        Wait_Text.Text = "ファイルを展開しています..." + b + "/" + APK_Files_Max;
                        Wait_P.Visibility = Android.Views.ViewStates.Visible;
                        Wait_Text.Visibility = Android.Views.ViewStates.Visible;
                        Test_B.Visibility = Android.Views.ViewStates.Invisible;
                        Kankore_B.Visibility = Android.Views.ViewStates.Invisible;
                        Backup_B.Visibility = Android.Views.ViewStates.Invisible;
                        Zip_Help_B.Visibility = Android.Views.ViewStates.Invisible;
                        Zip_Select_B.Visibility = Android.Views.ViewStates.Invisible;
                        APK_Tenkai_B.Visibility = Android.Views.ViewStates.Invisible;
                        Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
                        BGM_B.Visibility = Android.Views.ViewStates.Invisible;
                        Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
                    }
                }
                await Task.Delay(1000);
            }
        }
        WebClient wc;
        void Kankore_Mod()
        {
            WebRequest.DefaultWebProxy = null;
            Button Test_B = FindViewById<Button>(Resource.Id.Test_B);
            Button Backup_B = FindViewById<Button>(Resource.Id.Backup_B);
            Button Kankore_B = FindViewById<Button>(Resource.Id.Kankore_B);
            ProgressBar Kankore_P = FindViewById<ProgressBar>(Resource.Id.Kankore_P);
            Button Zip_Select_B = FindViewById<Button>(Resource.Id.Zip_Select_B);
            Button Zip_Help_B = FindViewById<Button>(Resource.Id.Zip_Help_B);
            Button BGM_B = FindViewById<Button>(Resource.Id.BGM_B);
            Button Sample_Voice_B = FindViewById<Button>(Resource.Id.Sample_Voice_B);
            Button Change_Sample_Voice_B = FindViewById<Button>(Resource.Id.Change_Sample_Voice_B);
            Test_B.Visibility = Android.Views.ViewStates.Invisible;
            Kankore_B.Visibility = Android.Views.ViewStates.Invisible;
            Backup_B.Visibility = Android.Views.ViewStates.Invisible;
            Zip_Help_B.Visibility = Android.Views.ViewStates.Invisible;
            Zip_Select_B.Visibility = Android.Views.ViewStates.Invisible;
            Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
            BGM_B.Visibility = Android.Views.ViewStates.Invisible;
            Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
            Kankore_P.Progress = 0;
            Kankore_P.Visibility = Android.Views.ViewStates.Visible;
            Directory.CreateDirectory(Download_Path + "/Download_Mods");
            int fCount = Directory.GetFiles(Download_Path + "/Download_Mods", "*", SearchOption.TopDirectoryOnly).Length;
            count = fCount + 1;
            string Uri2 = "";
            if (Kankore_B.Text == "艦これ音声Mod")
            {
                Uri2 = "https://www.dropbox.com/s/dek3vzi1krclt1f/Kankore_Voice_V2.zip?dl=1";
            }
            else if (Kankore_B.Text == "ASMR音声Mod")
            {
                Uri2 = "https://www.dropbox.com/s/ay5714v365vrhl6/Slave_Voice_Mod.zip?dl=1";
            }
            else if (Kankore_B.Text == "クリス音声Mod")
            {
                Uri2 = "https://www.dropbox.com/s/jznjn17dc93d33o/Criss_Voice_Mod.zip?dl=1";
            }
            else if (Kankore_B.Text == "ウィズ音声Mod")
            {
                Uri2 = "https://www.dropbox.com/s/qu3ox0bclvf9sit/Wiss_Voice_Mod.zip?dl=1";
            }
            else if (Kankore_B.Text == "あやせ音声Mod")
            {
                Uri2 = "https://www.dropbox.com/s/2b76zb9pghf1get/Ayase_Voice_Mod.zip?dl=1";
            }
            Uri uri = new Uri(Uri2);
            if (wc == null)
            {
                wc = new WebClient();
                wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
                wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            }
            wc.DownloadFileAsync(uri, Download_Path + "/Download_Mods/Download_Voice_Mod.dat");
            if (Directory.Exists(Download_Path + "/Download_Mods/Download_Voice_Mod"))
            {
                Directory.Delete(Download_Path + "/Download_Mods/Download_Voice_Mod", true);
            }
            Kankore_Voice_Mod = true;
        }
        private void wc_DownloadProgressChanged(System.Object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar Kankore_P = FindViewById<ProgressBar>(Resource.Id.Kankore_P);
            Kankore_P.Progress = e.ProgressPercentage;
        }

        //完了時のイベント
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
                    string filePath = Download_Path + "/Download_Mods/Download_Voice_Mod.dat";
                    string folderPath = Download_Path + "/Download_Mods/Download_Voice_Mod";
                    ICSharpCode.SharpZipLib.Zip.FastZip zip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                    zip.ExtractZip(filePath, folderPath, "");
                });
                task.Wait();
                File.Delete(Download_Path + "/Download_Mods/Download_Voice_Mod.dat");
                Mods_Get();
            }
        }
        void Mods_Get()
        {
            Button Zip_Select_B = FindViewById<Button>(Resource.Id.Zip_Select_B);
            Button Zip_Help_B = FindViewById<Button>(Resource.Id.Zip_Help_B);
            Button Kankore_B = FindViewById<Button>(Resource.Id.Kankore_B);
            Button Sample_Voice_B = FindViewById<Button>(Resource.Id.Sample_Voice_B);
            Button BGM_B = FindViewById<Button>(Resource.Id.BGM_B);
            Button Change_Sample_Voice_B = FindViewById<Button>(Resource.Id.Change_Sample_Voice_B);
            ProgressBar Kankore_P = FindViewById<ProgressBar>(Resource.Id.Kankore_P);
            try
            {
                Directory.CreateDirectory(Download_Path + "/WoTB_Files/assets/Data/Mods");
                IEnumerable<string> files = Directory.EnumerateFiles(Download_Path + "/Download_Mods/Download_Voice_Mod", "*", SearchOption.AllDirectories);
                foreach (string f in files)
                {
                    string Path_Name = Path.GetFileName(f);
                    if (f.Contains("sounds.yaml"))
                    {
                        File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/sounds.yaml", true);
                    }
                    if (f.Contains("sfx_high.yaml") || f.Contains("sfx_low.yaml"))
                    {
                        File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/Configs/Sfx/" + Path_Name, true);
                    }
                    if (f.Contains(".fev") || f.Contains(".fsb"))
                    {
                        File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/Mods/" + Path_Name, true);
                    }
                }
                Toast.MakeText(this, Kankore_B.Text + "が適応されました。\n適応ボタンを押してください。", ToastLength.Short).Show();
                Kankore_P.Visibility = Android.Views.ViewStates.Invisible;
                Button Test_B = FindViewById<Button>(Resource.Id.Test_B);
                Test_B.Visibility = Android.Views.ViewStates.Visible;
                Kankore_B.Visibility = Android.Views.ViewStates.Visible;
                Button Backup_B = FindViewById<Button>(Resource.Id.Backup_B);
                Backup_B.Visibility = Android.Views.ViewStates.Visible;
                Zip_Help_B.Visibility = Android.Views.ViewStates.Visible;
                Zip_Select_B.Visibility = Android.Views.ViewStates.Visible;
                Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
                BGM_B.Visibility = Android.Views.ViewStates.Visible;
                Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
            }
            catch
            {
                Toast.MakeText(this, "エラーが発生しました。\n開発者にご連絡ください。", ToastLength.Short).Show();
                Kankore_P.Visibility = Android.Views.ViewStates.Invisible;
                Button Test_B = FindViewById<Button>(Resource.Id.Test_B);
                Test_B.Visibility = Android.Views.ViewStates.Visible;
                Kankore_B.Visibility = Android.Views.ViewStates.Visible;
                Button Backup_B = FindViewById<Button>(Resource.Id.Backup_B);
                Backup_B.Visibility = Android.Views.ViewStates.Visible;
                Zip_Help_B.Visibility = Android.Views.ViewStates.Visible;
                Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
                Zip_Select_B.Visibility = Android.Views.ViewStates.Visible;
                BGM_B.Visibility = Android.Views.ViewStates.Visible;
                Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
            }
        }
        async void File_OK()
        {
            File_OK_02();
            Button Sample_Voice_B = FindViewById<Button>(Resource.Id.Sample_Voice_B);
            Button Zip_Select_B = FindViewById<Button>(Resource.Id.Zip_Select_B);
            Button Zip_Help_B = FindViewById<Button>(Resource.Id.Zip_Help_B);
            Button APK_Tenkai_B = FindViewById<Button>(Resource.Id.APK_Tenkai_B);
            Button Test_B = FindViewById<Button>(Resource.Id.Test_B);
            Button Kankore_B = FindViewById<Button>(Resource.Id.Kankore_B);
            Button Backup_B = FindViewById<Button>(Resource.Id.Backup_B);
            ProgressBar Wait_P = FindViewById<ProgressBar>(Resource.Id.Wait_P);
            Button BGM_B = FindViewById<Button>(Resource.Id.BGM_B);
            TextView Wait_Text = FindViewById<TextView>(Resource.Id.Wait_Text);
            Button Change_Sample_Voice_B = FindViewById<Button>(Resource.Id.Change_Sample_Voice_B);
            await Task.Delay(500);
            while (true)
            {
                if (File.Exists(Download_Path + "/Mod_APK/OK.dat"))
                {
                    Wait_P.Visibility = Android.Views.ViewStates.Invisible;
                    Wait_Text.Visibility = Android.Views.ViewStates.Invisible;
                    Kankore_B.Visibility = Android.Views.ViewStates.Visible;
                    Test_B.Visibility = Android.Views.ViewStates.Visible;
                    Backup_B.Visibility = Android.Views.ViewStates.Visible;
                    Zip_Help_B.Visibility = Android.Views.ViewStates.Visible;
                    Zip_Select_B.Visibility = Android.Views.ViewStates.Visible;
                    Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
                    BGM_B.Visibility = Android.Views.ViewStates.Visible;
                    Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Visible;
                    Toast.MakeText(this, "適応しました。", ToastLength.Short).Show();
                    break;
                }
                else
                {
                    Wait_Text.Text = "APKファイルを作成しています...";
                    Wait_P.Visibility = Android.Views.ViewStates.Visible;
                    Backup_B.Visibility = Android.Views.ViewStates.Invisible;
                    Wait_Text.Visibility = Android.Views.ViewStates.Visible;
                    APK_Tenkai_B.Visibility = Android.Views.ViewStates.Invisible;
                    Kankore_B.Visibility = Android.Views.ViewStates.Invisible;
                    Test_B.Visibility = Android.Views.ViewStates.Invisible;
                    Zip_Help_B.Visibility = Android.Views.ViewStates.Invisible;
                    Zip_Select_B.Visibility = Android.Views.ViewStates.Invisible;
                    Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
                    BGM_B.Visibility = Android.Views.ViewStates.Invisible;
                    Change_Sample_Voice_B.Visibility = Android.Views.ViewStates.Invisible;
                }
                await Task.Delay(500);
            }
        }
        long Lengh = 0;
        async void File_OK_02()
        {
            Directory.CreateDirectory(Download_Path + "/Mod_APK");
            File.Delete(Download_Path + "/Mod_APK/OK.dat");
            Task task = Task.Run(() =>
            {
                string zipFilePath = Download_Path + "/Mod_APK/base.apk";
                string folderPath = Download_Path + "/WoTB_Files";
                ICSharpCode.SharpZipLib.Zip.FastZip zip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                zip.CreateZip(zipFilePath, folderPath, true, null, null);
            });
            await Task.Delay(1000);
            while (true)
            {
                FileInfo info = new FileInfo(Download_Path + "/Mod_APK/base.apk");
                if (Lengh == info.Length)
                {
                    StreamWriter stw = File.CreateText(Download_Path + "/Mod_APK/OK.dat");
                    stw.Write("OK");
                    stw.Close();
                    try
                    {
                        Runtime.GetRuntime().Exec(new string[] { "su", "-c", "cp " + Download_Path + "/Mod_APK/base.apk " + WoTB_Path + "/base.apk" });
                    }
                    catch
                    {
                        Android.App.AlertDialog.Builder f = new Android.App.AlertDialog.Builder(this);
                        f.SetTitle("確認");
                        f.SetMessage("APKファイルは作成できましたがルート化されていないため適応できませんでした。");
                        f.Show();
                    }
                    break;
                }
                else
                {
                    Lengh = info.Length;
                }
                await Task.Delay(1000);
            }
        }
        void Sample_Voice_Mod_Random()
        {
            Button Kankore_B = FindViewById<Button>(Resource.Id.Kankore_B);
            Random r = new Random();
            int r2 = r.Next(5);
            if (r2 == 0)
            {
                if (Kankore_B.Text == "艦これ音声Mod")
                {
                    Sample_Voice_Mod_Random();
                }
                else
                {
                    Kankore_B.Text = "艦これ音声Mod";
                }
            }
            else if (r2 == 1)
            {
                if (Kankore_B.Text == "ASMR音声Mod")
                {
                    Sample_Voice_Mod_Random();
                }
                else
                {
                    Kankore_B.Text = "ASMR音声Mod";
                }
            }
            else if (r2 == 2)
            {
                if (Kankore_B.Text == "クリス音声Mod")
                {
                    Sample_Voice_Mod_Random();
                }
                else
                {
                    Kankore_B.Text = "クリス音声Mod";
                }
            }
            else if (r2 == 3)
            {
                if (Kankore_B.Text == "ウィズ音声Mod")
                {
                    Sample_Voice_Mod_Random();
                }
                else
                {
                    Kankore_B.Text = "ウィズ音声Mod";
                }
            }
            else if (r2 == 4)
            {
                if (Kankore_B.Text == "あやせ音声Mod")
                {
                    Sample_Voice_Mod_Random();
                }
                else
                {
                    Kankore_B.Text = "あやせ音声Mod";
                }
            }
        }
    }
}