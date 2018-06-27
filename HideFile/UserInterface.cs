using System.Windows;
using System.Windows.Controls;
using System.Security.Cryptography;

using WPF = System.Windows.Controls;

using FORMS = System.Windows.Forms;
using System.Windows.Input;
using System.IO;

namespace HideFile
{
    public class UserInterface
    {
        private Grid GrdContent;
        private byte[] Key;
        private byte[] Iv;
        private int BlockSize;
        private int KeySize;
        private CipherMode CipherMode;
        private TypeTagEnum typeAction;
        private string Source;
        private string SourceFile;
        private string DestinationName;

        #region Métodos públicos

        /// <summary>
        /// Constructor de la clase. Recibe el objeto "Grid" de la UI para trabajar con el.
        /// </summary>
        /// <param name="grdContent"></param>
        public UserInterface(Grid grdContent, byte[] key, byte[] iv, int blockSize, int keySize, CipherMode cipherMode)
        {
            GrdContent = grdContent;
            Key = key;
            Iv = iv;
            BlockSize = blockSize;
            KeySize = keySize;
            CipherMode = cipherMode;
        }

        /// <summary>
        /// Funcion que pinta la UI dependiendo de la opcion seleccionada.
        /// </summary>
        /// <param name="tag"></param>
        public void PrintUI(TypeTagEnum tag)
        {
            typeAction = tag;
            var typeUI = TypeTagEnum.Nothing;
            EmtpyGridContent();

            switch (tag)
            {
                case TypeTagEnum.EncryptFile:
                    typeUI = TypeTagEnum.EncryptFile;
                    break;

                case TypeTagEnum.DecryptFile:
                    typeUI = TypeTagEnum.DecryptFile;
                    break;

                case TypeTagEnum.EncryptPath:
                    typeUI = TypeTagEnum.EncryptPath;
                    break;

                case TypeTagEnum.DecryptPath:
                    typeUI = TypeTagEnum.DecryptPath;
                    break;

                default:
                    typeUI = TypeTagEnum.Nothing;
                    break;
            }

            if (typeUI != TypeTagEnum.Nothing)
            {
                var txtSource = ControlBuilder.GetTextBox(Constants.TXT_SOURCE,
                                             GetText(typeUI, Constants.TXT_SOURCE),
                                             Constants.WIDTH_TEXTBOX_LARGE,
                                             Constants.HEIGTH_TEXTBOX_LARGE,
                                             new Thickness() { Top = 50, Left = 10 });

                txtSource.KeyUp += TextBoxKeyDown;
                txtSource.GotFocus += TextBoxGotFocus;
                txtSource.LostFocus += TextBoxLostFocus;
                GrdContent.Children.Add(txtSource);

                var btnBrowseFile = ControlBuilder.GetButton(Constants.BTN_BROWSE_FILE,
                                                             Constants.LABEL_BROWSE,
                                                             Constants.WIDTH_BUTTON_SHORT,
                                                             Constants.HEIGTH_BUTTON_SHORT,
                                                             new Thickness() { Top = 50, Right = 10 },
                                                             Constants.TAG_BROWSE_FILE_BUTTON);

                btnBrowseFile.Click += ButtonClick;
                GrdContent.Children.Add(btnBrowseFile);

                var txtDestination = ControlBuilder.GetTextBox(Constants.TXT_DESTINATION_NAME,
                                                                  GetText(typeUI, Constants.TXT_DESTINATION_NAME),
                                                                  Constants.WIDTH_TEXTBOX_LARGE,
                                                                  Constants.HEIGTH_TEXTBOX_LARGE,
                                                                  new Thickness() { Top = 90, Left = 10 });

                txtDestination.KeyUp += TextBoxKeyDown;
                txtDestination.GotFocus += TextBoxGotFocus;
                txtDestination.LostFocus += TextBoxLostFocus;
                GrdContent.Children.Add(txtDestination);

                var btnActionFile = ControlBuilder.GetButton(Constants.BTN_ACTION,
                                                             GetText(typeUI, Constants.BTN_ACTION),
                                                             Constants.WIDTH_BUTTON_SHORT,
                                                             Constants.HEIGTH_BUTTON_SHORT,
                                                             new Thickness() { Top = 90, Right = 10 },
                                                             GetButtonTag(typeUI));

                btnActionFile.Click += ButtonClick;
                GrdContent.Children.Add(btnActionFile);

                GrdContent.Children.Add(ControlBuilder.GetTextBlock(Constants.TBK_DESTINATION_FILE,
                                                                    GetText(typeUI, Constants.LABEL_DESTINATION),
                                                                    Constants.WIDTH_TEXTBLOCK_LARGE,
                                                                    Constants.HEIGTH_TEXTBLOCK_LARGE,
                                                                    new Thickness() { Top = 130, Left = 10 }));
            }
        }

        #endregion Métodos públicos

        #region Dynamics listeners

        /// <summary>
        /// Funcion asociada a los listeners de los botones. Gestiona la logica para cada boton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = (WPF.Button)sender;
            var tag = button.Tag.ToString();
            var resultProcess = string.Empty;
            var countResultProcess = -1;
            var cf = new CryptoFunctions(Key, Iv, BlockSize, KeySize, CipherMode);

            switch (tag)
            {
                case Constants.TAG_BROWSE_FILE_BUTTON:
                    if (typeAction == TypeTagEnum.EncryptFile || typeAction == TypeTagEnum.DecryptFile)
                    {
                        using (var fbd = new FORMS.OpenFileDialog())
                        {
                            var result = fbd.ShowDialog();
                            var path = fbd.FileName;

                            if (result == FORMS.DialogResult.OK && !string.IsNullOrWhiteSpace(path))
                            {
                                foreach (var item in GrdContent.Children)
                                {
                                    if (item.GetType() == typeof(TextBox) && ((TextBox)item).Name.Equals(Constants.TXT_SOURCE))
                                    {
                                        ((TextBox)item).Text = path.ToUpper();
                                        Source = path.ToUpper();
                                        SourceFile = Path.GetFileName(Source).ToUpper();
                                        UpdateDestination();
                                    }
                                }
                            }
                        }
                    }
                    else if (typeAction == TypeTagEnum.EncryptPath || typeAction == TypeTagEnum.DecryptPath)
                    {
                        using (var fbd = new FORMS.FolderBrowserDialog())
                        {
                            var result = fbd.ShowDialog();
                            var path = fbd.SelectedPath;

                            if (result == FORMS.DialogResult.OK && !string.IsNullOrWhiteSpace(path))
                            {
                                foreach (var item in GrdContent.Children)
                                {
                                    if (item.GetType() == typeof(TextBox) && ((TextBox)item).Name.Equals(Constants.TXT_SOURCE))
                                    {
                                        ((TextBox)item).Text = path.ToUpper();
                                        Source = path.ToUpper();
                                        SourceFile = Path.GetFileName(Source).ToUpper();
                                        UpdateDestination();
                                    }
                                }
                            }
                        }
                    }
                    break;

                case Constants.TAG_ENCRYPT_FILE_BUTTON:
                    resultProcess = cf.EncryptFile(Source, DestinationName);
                    MessageBox.Show(string.IsNullOrWhiteSpace(resultProcess) ? Constants.RESULT_FILE_KO : string.Format(Constants.RESULT_FILE_OK, resultProcess));
                    break;

                case Constants.TAG_DECRYPT_FILE_BUTTON:
                    resultProcess = cf.DecryptFile(Source, DestinationName);
                    MessageBox.Show(string.IsNullOrWhiteSpace(resultProcess) ? Constants.RESULT_FILE_KO : string.Format(Constants.RESULT_FILE_OK, resultProcess));
                    break;

                case Constants.TAG_ENCRYPT_PATH_BUTTON:
                    countResultProcess = cf.EncryptPath(Source, DestinationName);
                    MessageBox.Show(countResultProcess == -1 ? Constants.RESULT_PATH_KO : string.Format(Constants.RESULT_PATH_OK, Source, countResultProcess.ToString()));
                    break;

                case Constants.TAG_DECRYPT_PATH_BUTTON:
                    countResultProcess = cf.DecryptPath(Source, DestinationName);
                    MessageBox.Show(countResultProcess == -1 ? Constants.RESULT_PATH_KO : string.Format(Constants.RESULT_PATH_OK, Source, countResultProcess.ToString()));
                    break;
            }
        }

        /// <summary>
        /// Funcion para gestionar el evento "KeyUp" de los TextBoxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case Constants.TXT_SOURCE:
                    Source = ((TextBox)sender).Text.ToUpper();
                    SourceFile = Path.GetFileName(Source).ToUpper();
                    break;

                case Constants.TXT_DESTINATION_NAME:
                    if (!((TextBox)sender).Text.Equals(Constants.LABEL_NEW_NAME_ENCRYPTED_DESTINATION) ||
                    !((TextBox)sender).Text.Equals(Constants.LABEL_NEW_NAME_DECRYPTED_DESTINATION))
                    {
                        DestinationName = ((TextBox)sender).Text.ToUpper();
                    }
                    break;
            }

            UpdateDestination();
        }

        /// <summary>
        /// Funcion para gestionar el evento "GotFocus" de los Textboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = string.Empty;
        }

        /// <summary>
        /// Funcion para gestionar el evento "LostFocus" de los Textboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                if (((TextBox)sender).Name.Equals(Constants.TXT_SOURCE))
                {
                    ((TextBox)sender).Text = string.IsNullOrWhiteSpace(Source) ? GetText(typeAction, ((TextBox)sender).Name) : Source;
                }

                if (((TextBox)sender).Name.Equals(Constants.TXT_DESTINATION_NAME))
                {
                    ((TextBox)sender).Text = string.IsNullOrWhiteSpace(DestinationName) ? GetText(typeAction, ((TextBox)sender).Name) : DestinationName;
                }
            }
        }

        #endregion Dynamics listeners

        #region Otras funciones sin ubicar

        /// <summary>
        /// Funcion que vacia el grid de la UI. Usado normalmente antes de rellenar el grid con un nuevo UI.
        /// </summary>
        private void EmtpyGridContent()
        {
            GrdContent.Children.Clear();
        }

        /// <summary>
        /// Funcion que, dependiendo del control y el tipo de UI a pintar, devuelve el texto que deben tener.
        /// </summary>
        /// <param name="typeUI"></param>
        /// <param name="nameControl"></param>
        /// <returns></returns>
        private string GetText(TypeTagEnum typeUI, string nameControl)
        {
            switch (nameControl)
            {
                case Constants.TXT_SOURCE:
                    switch (typeUI)
                    {
                        case TypeTagEnum.EncryptFile:
                        case TypeTagEnum.EncryptPath:
                            return Constants.LABEL_UNENCRYPTED_SOURCE;

                        case TypeTagEnum.DecryptFile:
                        case TypeTagEnum.DecryptPath:
                            return Constants.LABEL_ENCRYPTED_SOURCE;

                        default:
                            return string.Empty;
                    }

                case Constants.TXT_DESTINATION_NAME:
                    switch (typeUI)
                    {
                        case TypeTagEnum.EncryptFile:
                        case TypeTagEnum.EncryptPath:
                            return Constants.LABEL_NEW_NAME_ENCRYPTED_DESTINATION;

                        case TypeTagEnum.DecryptFile:
                        case TypeTagEnum.DecryptPath:
                            return Constants.LABEL_NEW_NAME_DECRYPTED_DESTINATION;

                        default:
                            return string.Empty;
                    }
                case Constants.BTN_BROWSE_FILE:
                    return Constants.LABEL_BROWSE;

                case Constants.BTN_ACTION:
                    switch (typeUI)
                    {
                        case TypeTagEnum.EncryptFile:
                            return Constants.BTN_ENCRYPT_FILE;

                        case TypeTagEnum.EncryptPath:
                            return Constants.BTN_ENCRYPT_PATH;

                        case TypeTagEnum.DecryptFile:
                            return Constants.BTN_DECRYPT_FILE;

                        case TypeTagEnum.DecryptPath:
                            return Constants.BTN_DECRYPT_PATH;

                        default:
                            return string.Empty;
                    }
                case Constants.LABEL_DESTINATION:
                    switch (typeUI)
                    {
                        case TypeTagEnum.EncryptFile:
                        case TypeTagEnum.EncryptPath:
                            return Constants.LABEL_ENCRYPTED_DESTINATION;

                        case TypeTagEnum.DecryptFile:
                        case TypeTagEnum.DecryptPath:
                            return Constants.LABEL_DECRYPTED_DESTINATION;

                        default:
                            return string.Empty;
                    }
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Funcion que actializa el texto del Textblock para que esté al dia.
        /// </summary>
        private void UpdateDestination()
        {
            foreach (var item in GrdContent.Children)
            {
                if (item.GetType() == typeof(TextBlock) && ((TextBlock)item).Name.Equals(Constants.TBK_DESTINATION_FILE))
                {
                    var tbk = (TextBlock)item;

                    tbk.Text = string.IsNullOrWhiteSpace(SourceFile) ? string.Empty : Source.Replace(string.IsNullOrWhiteSpace(SourceFile) ? string.Empty : SourceFile, string.IsNullOrWhiteSpace(DestinationName) ? string.Empty : DestinationName);

                    switch (typeAction)
                    {
                        case TypeTagEnum.EncryptFile:
                            if (string.IsNullOrWhiteSpace(DestinationName))
                            {
                                tbk.Text = Source.ToUpper() + ".RIJENC";
                            }
                            else
                            {
                                tbk.Text = Source.ToUpper().Replace(Path.GetFileNameWithoutExtension(SourceFile), DestinationName) + ".RIJENC";
                            }

                            DestinationName = tbk.Text;
                            break;

                        case TypeTagEnum.DecryptFile:
                            if (string.IsNullOrWhiteSpace(DestinationName))
                            {
                                tbk.Text = Source.ToUpper().Replace(".RIJENC", string.Empty);
                            }
                            else
                            {
                                tbk.Text = Source.ToUpper().Replace(".RIJENC", string.Empty).Replace(Path.GetFileNameWithoutExtension(SourceFile.Replace(".RIJENC", string.Empty)), DestinationName);
                            }

                            DestinationName = tbk.Text;
                            break;

                        case TypeTagEnum.EncryptPath:
                            if (string.IsNullOrWhiteSpace(DestinationName))
                            {
                                tbk.Text = Source.ToUpper() + "_RIJENC";
                            }
                            else
                            {
                                tbk.Text = Source.ToUpper().Replace(Path.GetFileNameWithoutExtension(SourceFile), DestinationName) + "_RIJENC";
                            }

                            DestinationName = tbk.Text;
                            break;

                        case TypeTagEnum.DecryptPath:
                            if (string.IsNullOrWhiteSpace(DestinationName))
                            {
                                tbk.Text = Source.ToUpper().Replace("_RIJENC", string.Empty);
                            }
                            else
                            {
                                tbk.Text = Source.ToUpper().Replace("_RIJENC", string.Empty).Replace(Path.GetFileNameWithoutExtension(SourceFile.Replace(".RIJENC", string.Empty)), DestinationName);
                            }

                            DestinationName = tbk.Text;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Funcion que devuelve el tag correspondiente a la accion indicada
        /// </summary>
        /// <param name="typeUI"></param>
        /// <returns></returns>
        private string GetButtonTag(TypeTagEnum typeUI)
        {
            switch (typeUI)
            {
                case TypeTagEnum.EncryptFile:
                    return Constants.TAG_ENCRYPT_FILE_BUTTON;

                case TypeTagEnum.DecryptFile:
                    return Constants.TAG_DECRYPT_FILE_BUTTON;

                case TypeTagEnum.EncryptPath:
                    return Constants.TAG_ENCRYPT_PATH_BUTTON;

                case TypeTagEnum.DecryptPath:
                    return Constants.TAG_DECRYPT_PATH_BUTTON;

                default:
                    return string.Empty;
            }
        }

        #endregion Otras funciones sin ubicar
    }
}