using Android.App;
using Android.Media;
using Android.Widget;
using System;

namespace WoTB_APK_Editor
{
    public partial class MainActivity : Activity
    {
        MediaPlayer mediaplayer = new MediaPlayer();
        void Sample_Voice_Click()
        {
            if (mediaplayer.IsPlaying == true)
            {
                mediaplayer.Stop();
            }
            Random r = new Random();
            int r2 = r.Next(1, 6);
            Button Kankore_B = FindViewById<Button>(Resource.Id.Kankore_B);
            if (Kankore_B.Text == "艦これ音声Mod")
            {
                if (r2 == 1)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_01);
                }
                else if (r2 == 2)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_02);
                }
                else if (r2 == 3)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_03);
                }
                else if (r2 == 4)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_04);
                }
                else if (r2 == 5)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_05);
                }
            }
            else if (Kankore_B.Text == "ASMR音声Mod")
            {
                if (r2 == 1)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.ASMR_01);
                }
                else if (r2 == 2)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.ASMR_02);
                }
                else if (r2 == 3)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.ASMR_03);
                }
                else if (r2 == 4)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.ASMR_04);
                }
                else if (r2 == 5)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.ASMR_05);
                }
            }
            else if (Kankore_B.Text == "クリス音声Mod")
            {
                if (r2 == 1)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Criss_01);
                }
                else if (r2 == 2)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Criss_02);
                }
                else if (r2 == 3)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Criss_03);
                }
                else if (r2 == 4)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Criss_04);
                }
                else if (r2 == 5)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Criss_05);
                }
            }
            else if (Kankore_B.Text == "ウィズ音声Mod")
            {
                if (r2 == 1)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Wiss_01);
                }
                else if (r2 == 2)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Wiss_02);
                }
                else if (r2 == 3)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Wiss_03);
                }
                else if (r2 == 4)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Wiss_04);
                }
                else if (r2 == 5)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Wiss_05);
                }
            }
            else if (Kankore_B.Text == "あやせ音声Mod")
            {
                if (r2 == 1)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Ayase_01);
                }
                else if (r2 == 2)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Ayase_02);
                }
                else if (r2 == 3)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Ayase_03);
                }
                else if (r2 == 4)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Ayase_04);
                }
                else if (r2 == 5)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Ayase_05);
                }
            }
            mediaplayer.Start();
        }
    }
}