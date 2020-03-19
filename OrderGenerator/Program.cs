using System;
using System.Security.Cryptography;
using System.Text;

namespace OrderGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            ASCIIEncoding ByteConverter = new ASCIIEncoding();
            Console.WriteLine("Id*Start*End*StartHour*EndHour*Force*Interval*Url");
            Console.WriteLine("123*2020-01-01*2022-12-12*9*10*80*60*http://localhost:81/index.html");
            var data = Console.ReadLine() +"*"+Guid.NewGuid();
            byte[] originalData = ByteConverter.GetBytes(data);
            byte[] signedData;

            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
            //System.IO.File.WriteAllText("private.txt", RSAalg.ToXmlString(true));
            //System.IO.File.WriteAllText("public.txt", RSAalg.ToXmlString(false));
            RSAalg.FromXmlString(System.IO.File.ReadAllText("private.txt"));
            RSAParameters Key = RSAalg.ExportParameters(true);

            signedData = HashAndSignBytes(originalData, Key);
            data ="START "+ BitConverter.ToString(signedData).Replace("-", "") + "|" + data+"ENDCCC";
            System.IO.File.WriteAllText("Data.txt",data );
            Console.WriteLine(data.Length);
            Console.WriteLine(data);
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.  
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }
    }
}
