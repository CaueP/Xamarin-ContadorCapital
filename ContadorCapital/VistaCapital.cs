
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
using static ContadorCapital.MainActivity;

namespace ContadorCapital
{
	[Activity(Label = "VistaCapital")]
	public class VistaCapital : Activity
	{
		double defaultValue;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.VistaCapital);

			//find views
			EditText txtCapitalM = FindViewById<EditText>(Resource.Id.txtcapmexico);
			EditText txtCapitalC = FindViewById<EditText>(Resource.Id.txtcapcolombia);
			ImageView imageMex = FindViewById<ImageView>(Resource.Id.imageMexico);
			ImageView imageCol = FindViewById<ImageView>(Resource.Id.imageColombia);
			Button btnSair = FindViewById<Button>(Resource.Id.btnsair);


			try
			{
				txtCapitalM.Text = Intent.GetDoubleExtra("CapitalM", defaultValue).ToString();
				txtCapitalC.Text = Intent.GetDoubleExtra("CapitalC", defaultValue).ToString();
				imageMex.SetImageResource(Resource.Drawable.mexico);
				imageCol.SetImageResource(Resource.Drawable.colombia);

                // using SQLite
                // setting path to save the DB
                var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                // setting db file
                path = Path.Combine(path, "Base.db3");
                // connecting to DB
                var conn = new SQLiteConnection(path);

                var elements = from s in conn.Table<Information>()
                               select s;
                foreach(var queue in elements)
                {
                    Toast.MakeText(
                        this, queue.IngresosMexico.ToString(), ToastLength.Short).Show();
                    Toast.MakeText(
                        this, queue.IngresosColombia.ToString(), ToastLength.Short).Show();
                    Toast.MakeText(
                        this, queue.EgresosMexico.ToString(), ToastLength.Short).Show();
                    Toast.MakeText(
                        this, queue.EgresosColombia.ToString(), ToastLength.Short).Show();
                }
            }
			catch (System.Exception ex) {
				Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
			}

			btnSair.Click += delegate {
				Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
			};


		}
	}
}
