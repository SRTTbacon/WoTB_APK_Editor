using System;
using Android.App;
using Android.Media;
using Android.Widget;

namespace WoTB_APK_Editor.Resources.layout
{
    public partial class Music_Activity : Activity
    {
        MediaPlayer mediaplayer = new MediaPlayer();
        void BGM_Sample()
        {
            if (mediaplayer.IsPlaying == true)
            {
                mediaplayer.Stop();
            }
            Random r = new Random();
            int r2 = r.Next(1, 6);
            Button BGM_Select_B = FindViewById<Button>(Resource.Id.BGM_Select_B);
            if (BGM_Select_B.Text == "TWO STEPS FROM HEll")
            {
                if (r2 == 1)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Two_Steps_From_Hell_01);
                }
                else if (r2 == 2)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Two_Steps_From_Hell_02);
                }
                else if (r2 == 3)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Two_Steps_From_Hell_03);
                }
                else if (r2 == 4)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Two_Steps_From_Hell_04);
                }
                else if (r2 == 5)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Two_Steps_From_Hell_05);
                }
            }
            else if (BGM_Select_B.Text == "GUP_BGM_MOD")
            {
                if (r2 == 1)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.GuP_01);
                }
                else if (r2 == 2)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.GuP_02);
                }
                else if (r2 == 3)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.GuP_03);
                }
                else if (r2 == 4)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.GuP_04);
                }
                else if (r2 == 5)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.GuP_05);
                }
            }
            else if (BGM_Select_B.Text == "艦これBGM")
            {
                if (r2 == 1)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_BGM_01);
                }
                else if (r2 == 2)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_BGM_02);
                }
                else if (r2 == 3)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_BGM_03);
                }
                else if (r2 == 4)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_BGM_04);
                }
                else if (r2 == 5)
                {
                    mediaplayer = MediaPlayer.Create(this, Resource.Raw.Kankore_BGM_05);
                }
            }
            mediaplayer.Start();
        }
    }
}