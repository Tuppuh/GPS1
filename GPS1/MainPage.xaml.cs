using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GPS1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            progress1.IsActive = true;

            try
            {
                var position = await LocationManager.GetPosition();
                LonBox.Text = "Lat: " + position.Coordinate.Longitude.ToString() +
                "  Lon: " + position.Coordinate.Latitude.ToString();
                //LatBox.Text = position.Coordinate.Latitude.ToString();

                string asemacode = await LiikennePaikka.etsiPaikka(
                    position.Coordinate.Latitude, position.Coordinate.Longitude);
                string asema = await LiikennePaikka.asemaNimi(asemacode);
                Asemabox.Text = asema;
                progress1.IsActive = false;

                List<JunaTiedot> tietolista = await AsemaTiedot.haeTiedot(asemacode);
                //junaLista.ItemsSource = tietolista;

                string destCode;
                string destSt;
                foreach (JunaTiedot juna in tietolista)
                {
                    destCode = juna.destination;
                    destSt = await LiikennePaikka.asemaNimi(destCode);
                    juna.destination = destSt;
                }
                tietolista.Sort((x, y) => DateTime.Compare(x.timeStamp, y.timeStamp));

                junaLista.ItemsSource = tietolista;
                Debug.WriteLine("----MOIKKA----");
            }
            catch (Exception)
            {
                LonBox.Text = "NO GPS!";
                progress1.IsActive = false;
                return;
            }
            
        }

        /*
        private async void DemoNappi_Click(object sender, RoutedEventArgs e)
        {
            progress1.IsActive = true;

            LonBox.Text = "Lat: " + "Demo" +
                "        Lon: " + "Demo";
            //LatBox.Text = position.Coordinate.Latitude.ToString();

            string asemacode = "HKI";
            string asema = await LiikennePaikka.asemaNimi(asemacode);
            Asemabox.Text = asema;
            progress1.IsActive = false;

            List<JunaTiedot> tietolista = await AsemaTiedot.haeTiedot(asemacode);
            //junaLista.ItemsSource = tietolista;

            string destCode;
            string destSt;
            foreach (JunaTiedot juna in tietolista)
            {
                destCode = juna.destination;
                destSt = await LiikennePaikka.asemaNimi(destCode);
                juna.destination = destSt;
            }
            tietolista.Sort((x, y) => DateTime.Compare(x.timeStamp, y.timeStamp));

            junaLista.ItemsSource = tietolista;
            Debug.WriteLine("----MOIKKA----");
        }
        */
    }
}
