using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace HideFile
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte[] key = Encoding.UTF8.GetBytes(@"8|4]1ldU]wB|^61dA>9>VY=gJrlbp<=S");
        private byte[] iv = Encoding.UTF8.GetBytes(@"t\8c1n5t|+fsY(u\oVLiLefd{|*u4=8J");
        private int blockSize = 256;
        private int keySize = 256;
        private CipherMode cipherMode = CipherMode.CBC;

        public MainWindow()
        {
            InitializeComponent();

            //var text = "Esto es un texto de prueba para encriptar";
            //var textEncripted = CryptographicFunctions.EncryptText(text);
            //var textDecripted = CryptographicFunctions.DecryptText(textEncripted);

            //var enc_file = CryptographicFunctions.EncryptFile(@"C:\Users\SOGETI\Desktop\alf_base.jpg");
            //var dec_file = CryptographicFunctions.DecryptFile(enc_file);
        }

        #region Listeners

        /// <summary>
        /// Funcion usada en los MenuItem. Dependiendo de la opcion seleccionada, se pinta una interfaz u otra.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectMenuOption(object sender, RoutedEventArgs e)
        {
            // Todo el trabajo recae sobre la clase "UserInterface" porque la UI se pinta en tiempo de ejecucion.
            // La creacíon, el parseado de los datos introducidos, los listeners y el procesado lo maneja esta clase

            var ui = new UserInterface(grdContent, key, iv, blockSize, keySize, cipherMode);
            ui.PrintUI((TypeTagEnum)((MenuItem)sender).Tag);
        }

        #endregion Listeners
    }
}