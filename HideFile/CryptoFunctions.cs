using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace HideFile
{
    public class CryptoFunctions
    {
        // El resultado de "GetBytes" debe devolver un byte[32]
        // Si se usan los metodos "GenerateKey" o "GenerateIV" del objeto "RijndaelManaged"
        // genera strings de 44 de largo para keysize = 256, pero son mas letras y numeros que otra cosa.
        // Creo que al convertir con "Encoding.UTF8.GetBytes" genera morralla o quiza es por tener mas simbolos
        // Se ponene estos valores por si no se recibe nada

        private byte[] key = Encoding.UTF8.GetBytes(@"8|4]1ldU]wB|^61dA>9>VY=gJrlbp<=S");
        private byte[] iv = Encoding.UTF8.GetBytes(@"t\8c1n5t|+fsY(u\oVLiLefd{|*u4=8J");
        private int? blockSize = 256;
        private int? keySize = 256;
        private CipherMode cipherMode = CipherMode.CBC;

        #region Constructores

        /// <summary>
        /// Constructor vacio. Para usar los datos genericos.
        /// </summary>
        public CryptoFunctions()
        {
        }

        /// <summary>
        /// Constructor con los parametros a usar al enciptar y desencriptar.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <param name="blockSize"></param>
        /// <param name="keySize"></param>
        /// <param name="cipherMode"></param>
        public CryptoFunctions(byte[] key, byte[] iv, int blockSize, int keySize, CipherMode cipherMode)
        {
            this.key = key;
            this.iv = iv;
            this.blockSize = blockSize;
            this.keySize = keySize;
            this.cipherMode = cipherMode;
        }

        #endregion Constructores

        #region Encriptado de strings (si no es string, se transforma)

        ///<summary>
        /// Encripta un texto usando el algoritmo Rijndael(AES). Devuelve un string "NULL" si ha fallado algo o el string resultante de la encriptacion.
        ///</summary>
        ///<param name="inputText"></param>
        public string EncryptText(string inputText)
        {
            try
            {
                var inputBytes = Encoding.UTF8.GetBytes(inputText);

                var cripto = new RijndaelManaged()
                {
                    BlockSize = Convert.ToInt32(blockSize),
                    KeySize = Convert.ToInt32(keySize),
                    Key = key,
                    IV = iv,
                    Mode = cipherMode
                };

                using (var ms = new MemoryStream(inputBytes.Length))
                using (var objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch (ArgumentNullException ane)
            {
                MessageBox.Show("Error al encriptar el texto recibido. Es posible que no sea un texto valido.");
                return null;
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                MessageBox.Show("Error al encriptar el texto recibido. La longitud del texto es negativa ¿?");
                return null;
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show("Error al encriptar el texto recibido. No se puede escribir el stream.");
                return null;
            }
        }

        ///<summary>
        /// Desencripta un texto usando el algoritmo Rijndael(AES). Devuelve un string "NULL" si ha fallado algo o el string resultante de la desencriptacion.
        ///</summary>
        ///<param name="inputText"></param>
        public string DecryptText(string inputText)
        {
            try
            {
                var inputBytes = Convert.FromBase64String(inputText);

                var cripto = new RijndaelManaged()
                {
                    BlockSize = Convert.ToInt32(blockSize),
                    KeySize = Convert.ToInt32(keySize),
                    Key = key,
                    IV = iv,
                    Mode = cipherMode
                };

                using (var ms = new MemoryStream(inputBytes))
                using (var objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(), CryptoStreamMode.Read))
                using (var sr = new StreamReader(objCryptoStream, true))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (ArgumentNullException ane)
            {
                MessageBox.Show("Error al encriptar el texto recibido. Es posible que no sea un texto valido.");
                return null;
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                MessageBox.Show("Error al encriptar el texto recibido. La longitud del texto es negativa ¿?");
                return null;
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show("Error al encriptar el texto recibido. No se puede leer el stream.");
                return null;
            }
        }

        #endregion Encriptado de strings (si no es string, se transforma)

        #region Encriptado y desencriptado de archivos (binario)

        ///<summary>
        /// Encripta un archivo usando el algoritmo Rijndael(AES). Devuelve un string "NULL" si ha fallado algo o el nombre del archivo procesado.
        ///</summary>
        ///<param name="inputText"></param>
        public string EncryptFile(string source, string destination)
        {
            var fileDestino = destination;

            try
            {
                //PONE EL ARCHIVO A CONVERTIR EN BYTES
                var ARCHIVO = File.ReadAllBytes(source);
                var size = ARCHIVO.Length;

                //DEFINE EL TIPO DE ENCRIPTADO
                var cripto = new RijndaelManaged()
                {
                    BlockSize = Convert.ToInt32(blockSize),
                    KeySize = Convert.ToInt32(keySize),
                    Key = key,
                    IV = iv,
                    Mode = cipherMode
                };

                //CREA E INICIALIZA EL STREAM PARA EL DESTINO
                //CREA E INICIALIZA EL STREAM PARA LA ENCRIPTACION
                using (var DESTINO = new FileStream(fileDestino, FileMode.OpenOrCreate, FileAccess.Write))
                using (var ENCRIPTADO = new CryptoStream(DESTINO, cripto.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    //ENCRIPTA
                    ENCRIPTADO.Write(ARCHIVO, 0, size);
                }
            }
            catch (ArgumentNullException ane)
            {
                MessageBox.Show("Error al encriptar el texto recibido. Es posible que no sea un texto valido.");
                fileDestino = null;
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                MessageBox.Show("Error al encriptar el texto recibido. La longitud del texto es negativa ¿?");
                fileDestino = null;
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show("Error al encriptar el texto recibido. No se puede escribir el stream.");
                fileDestino = null;
            }

            return fileDestino;
        }

        ///<summary>
        /// Desencripta un archivo usando el algoritmo Rijndael(AES). Devuelve un string "NULL" si ha fallado algo o el nombre del archivo procesado.
        ///</summary>
        ///<param name="inputText"></param>
        public string DecryptFile(string source, string destination)
        {
            var fileDestino = destination;

            try
            {
                //PONE EL ARCHIVO A CONVERTIR EN BYTES
                var ARCHIVO = File.ReadAllBytes(source);
                var size = ARCHIVO.Length;

                //DEFINE EL TIPO DE ENCRIPTADO
                var cripto = new RijndaelManaged()
                {
                    BlockSize = Convert.ToInt32(blockSize),
                    KeySize = Convert.ToInt32(keySize),
                    Key = key,
                    IV = iv,
                    Mode = cipherMode
                };

                //CREA E INICIALIZA EL STREAM PARA EL DESTINO
                //CREA E INICIALIZA EL STREAM PARA LA DESENCRIPTACION
                using (var DESTINO = new FileStream(fileDestino, FileMode.OpenOrCreate, FileAccess.Write))
                using (var ENCRIPTADO = new CryptoStream(DESTINO, cripto.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    //ENCRIPTA
                    ENCRIPTADO.Write(ARCHIVO, 0, size);
                }
            }
            catch (ArgumentNullException ane)
            {
                MessageBox.Show("Error al encriptar el texto recibido. Es posible que no sea un texto valido.");
                fileDestino = null;
            }
            catch (ArgumentOutOfRangeException aoore)
            {
                MessageBox.Show("Error al encriptar el texto recibido. La longitud del texto es negativa ¿?");
                fileDestino = null;
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show("Error al encriptar el texto recibido. No se puede escribir el stream.");
                fileDestino = null;
            }

            return fileDestino;
        }

        #endregion Encriptado y desencriptado de archivos (binario)

        #region Encriptado y desencriptado de directorios

        /// <summary>
        /// Encripta todo el contenido de un directorio usando el algoritmo Rijndael(AES). Devuelve un string "NULL" si ha fallado algo o el nombre del directorio procesado.
        /// </summary>
        /// <param name="path"></param>
        public int EncryptPath(string source, string destination)
        {
            string file;
            var count = 0;
            if (source.Equals(destination) || Directory.Exists(destination)) return -1;

            GenerateDestinationStructure(source, destination);

            var fileList = Getfiles(source);

            foreach (var originfile in fileList)
            {
                var destinationFile = originfile.Replace(source, destination);
                destinationFile += ".RIJENC";

                if (File.Exists(destinationFile))
                {
                    File.Delete(destinationFile);
                }

                file = EncryptFile(originfile, destinationFile);

                if (file == null) return -1;
                count++;
            }

            return count;
        }

        /// <summary>
        /// Desencripta todo el contenido de un directorio usando el algoritmo Rijndael(AES). Devuelve un string "NULL" si ha fallado algo o el nombre del directorio procesado.
        /// </summary>
        /// <param name="path"></param>
        public int DecryptPath(string source, string destination)
        {
            string file;
            var count = 0;
            if (source.Equals(destination) || Directory.Exists(destination)) return -1;

            GenerateDestinationStructure(source, destination);

            var fileList = Getfiles(source);

            foreach (var originfile in fileList)
            {
                var destinationFile = originfile.Replace(source, destination);
                destinationFile = destinationFile.Replace(".RIJENC", string.Empty);

                if (File.Exists(destinationFile))
                {
                    File.Delete(destinationFile);
                }

                file = DecryptFile(originfile, destinationFile);

                if (file == null) return -1;
                count++;
            }

            return count;
        }

        #endregion Encriptado y desencriptado de directorios

        #region Otras funciones

        /// <summary>
        /// Funcion que devuelve una lista de directorios.
        /// </summary>
        /// <param name="Extensions"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        private static List<string> GetDirectories(string source, bool WithRoot)
        {
            var directoryList = Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories).ToList();

            if (WithRoot) { directoryList.Add(source); }

            return directoryList;
        }

        /// <summary>
        /// Funcion para generar la estructura de carpetas en el destino. Machaca el destino si la carpeta (o subcarpetas) existe.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private void GenerateDestinationStructure(string source, string destination)
        {
            var directoryList = GetDirectories(source, false);

            foreach (string originPath in directoryList)
            {
                var destinationPath = originPath.Replace(source, destination);

                if (Directory.Exists(destinationPath))
                {
                    Directory.Delete(destinationPath, true);
                }

                Directory.CreateDirectory(destinationPath);
            }
        }

        /// <summary>
        /// Funcion para listar todos los archivos de una ruta indicada. Lista los archivos de la carpeta y de todas las subcarpetas.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private List<string> Getfiles(string source)
        {
            return Directory.EnumerateFiles(source, "*.*", SearchOption.AllDirectories).ToList();
        }

        #endregion Otras funciones
    }
}