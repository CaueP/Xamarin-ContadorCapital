using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.IO;
using SQLite;

namespace ContadorCapital
{
	[Activity(Label = "ContadorCapital", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		double ingressosC, ingressosM, egressosC, egressosM, capitalC, capitalM;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

            // using SQLite
            // setting path to save the DB
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            // setting db file
            path = Path.Combine(path, "Base.db3");
            // connecting to DB
            var conn = new SQLiteConnection(path);

            // creating a table of the class Information
            conn.CreateTable<Information>();
            

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.btncalcular);

			TextView txtIngressoColombia = FindViewById<TextView>(Resource.Id.txtincolombia);
			TextView txtEgressoColombia = FindViewById<TextView>(Resource.Id.txtegcolombia);
			TextView txtIngressoMexico = FindViewById<TextView>(Resource.Id.txtingmexico);
			TextView txtEgressoMexico = FindViewById<TextView>(Resource.Id.txtegmexico);

			button.Click += delegate {
				try {
					ingressosC = double.Parse(txtIngressoColombia.Text);
					egressosC = double.Parse(txtEgressoColombia.Text);
					ingressosM = double.Parse(txtIngressoMexico.Text);
					egressosM = double.Parse(txtEgressoMexico.Text);

					capitalC = ingressosC - egressosC;
					capitalM = ingressosM - egressosM;

                    // creating Information object that will be saved on DB
                    var Insert = new Information();
                    // setting the data
                    Insert.IngresosMexico = ingressosM;
                    Insert.IngresosColombia = ingressosC;
                    Insert.EgresosMexico = egressosM;
                    Insert.EgresosColombia = egressosC;

                    // saving data on DB
                    conn.Insert(Insert);
                    Toast.MakeText
                        (this, "Saved on SQLite", ToastLength.Long).Show();

                    // Calling new activity
					Carregar();
				}
				catch (System.Exception ex){
					Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
				}
			};
		}

		public void Carregar()
		{
			//instatiating an Intent object to start a VistaCapital activity
			Intent objIntent = new Intent(this,
										  typeof(VistaCapital));

			//setting the content that is going to be sent to the next activity
			objIntent.PutExtra("CapitalC", capitalC);
			objIntent.PutExtra("CapitalM", capitalM);

			//starting the VistaCapital activity
			StartActivity(objIntent);
		}

        // Information class to be saved on the SQLite Database
        public class Information
        {
            public double IngresosMexico { get; set; }

            public double EgresosMexico { get; set; }

            public double IngresosColombia { get; set; }

            public double EgresosColombia { get; set; }
        }
	}
}

