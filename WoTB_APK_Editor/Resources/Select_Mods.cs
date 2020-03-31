using Android.App;
using Android.Widget;
using Plugin.FilePicker;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WoTB_APK_Editor
{
    public partial class MainActivity : Activity
    {
        string File_Name_PickFile;
        void Zip_Help_Dialog()
        {
            AlertDialog.Builder f = new AlertDialog.Builder(this);
            f.SetTitle("ヘルプ");
            f.SetMessage(".zip形式のModを適応します。\n(拡張子が.zip形式である必要があります。)");
            f.Show();
        }
        async void Zip_Select_Path()
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
        void Select_Mod_Get()
        {
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
                            if (File_Path.Contains("/sounds.yaml"))
                            {
                                File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/sounds.yaml", true);
                            }
                            else if (File_Path.Contains("/sfx_high.yaml") || File_Path.Contains("/sfx_low.yaml"))
                            {
                                File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/Configs/Sfx/" + File_Name_Path, true);
                            }
                            else if (File_Path.Contains(".fev") || File_Path.Contains(".fsb"))
                            {
                                if (File_Path.Contains("/Mods"))
                                {
                                    Directory.CreateDirectory(Download_Path + "/WoTB_Files/assets/Data/Mods");
                                    File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/Mods/" + File_Name_Path, true);
                                }
                                else
                                {
                                    File.Copy(f, Download_Path + "/WoTB_Files/assets/Data/Sfx/" + File_Name_Path, true);
                                }
                            }
                        }
                    }
                    Kankore_Voice_Mod = true;
                    Toast.MakeText(this, "完了しました。", ToastLength.Short).Show();
                    File.Delete(Download_Path + "/Download_Mods/" + File_Name_PickFile);
                }
            }
            catch
            {
                Toast.MakeText(this, "エラーが発生しました。\n開発者にご連絡ください。", ToastLength.Short).Show();
            }
        }
    }
}