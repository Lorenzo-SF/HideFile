namespace HideFile
{
    public static class Constants
    {
        // Nombres de controles

        public const string TXT_SOURCE = "txtSourceFile";
        public const string TXT_DESTINATION_NAME = "txtDestinationName";
        public const string TBK_DESTINATION_FILE = "tbkDestination";
        public const string BTN_BROWSE_FILE = "btnBrowseFile";
        public const string BTN_ACTION = "btnAction";
        public const string BTN_ENCRYPT_FILE = "Encrypt file";
        public const string BTN_DECRYPT_FILE = "Decrypt file";
        public const string BTN_ENCRYPT_PATH = "Encrypt path";
        public const string BTN_DECRYPT_PATH = "Decrypt path";

        // Etiquetas

        public const string LABEL_UNENCRYPTED_SOURCE = "Unencrypted source";
        public const string LABEL_ENCRYPTED_SOURCE = "Encrypted source";

        public const string LABEL_NEW_NAME_ENCRYPTED_DESTINATION = "New Name for encrypited destination";
        public const string LABEL_NEW_NAME_DECRYPTED_DESTINATION = "New Name for decrypited destination";

        public const string LABEL_ENCRYPTED_DESTINATION = "Encrypted destination";
        public const string LABEL_DECRYPTED_DESTINATION = "Decrypted destination";

        public const string LABEL_DESTINATION = "Destination";
        public const string LABEL_BROWSE = "Browse...";

        // Tamaños para TextBox

        public const int WIDTH_TEXTBOX_LARGE = 387;
        public const int HEIGTH_TEXTBOX_LARGE = 20;
        public const int WIDTH_TEXTBOX_SHORT = 0;
        public const int HEIGTH_TEXTBOX_SHORT = 0;

        // Tamaños para TextBlock

        public const int WIDTH_TEXTBLOCK_LARGE = 497;
        public const int HEIGTH_TEXTBLOCK_LARGE = 20;
        public const int WIDTH_TEXTBLOCK_SHORT = 0;
        public const int HEIGTH_TEXTBLOCK_SHORT = 0;

        // Tamaños para Button

        public const int WIDTH_BUTTON_LARGE = 0;
        public const int HEIGTH_BUTTON_LARGE = 0;
        public const int WIDTH_BUTTON_SHORT = 85;
        public const int HEIGTH_BUTTON_SHORT = 20;

        // Tags

        public const string TAG_BROWSE_FILE_BUTTON = "BROWSE";
        public const string TAG_ENCRYPT_FILE_BUTTON = "ENCRYPT_FILE";
        public const string TAG_DECRYPT_FILE_BUTTON = "DECRYPT_FILE";
        public const string TAG_ENCRYPT_PATH_BUTTON = "ENCRYPT_PATH";
        public const string TAG_DECRYPT_PATH_BUTTON = "DECRYPT_PATH";

        // Mensajes resultado procesado

        public const string RESULT_FILE_OK = "Se ha procesado con existo el archivo: {0}";
        public const string RESULT_FILE_KO = "Ha habido un error al procesar el archivo.";

        public const string RESULT_PATH_OK = "Se ha procesado con existo el directorio: {0}. Se han procesado {1} archivo(s).";
        public const string RESULT_PATH_KO = "Ha habido un error al procesar el directorio.";
    }
}