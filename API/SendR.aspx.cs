using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Json;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Collections;
using RestSharp;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

public partial class API_SendR : System.Web.UI.Page
{
    SgFramework.SgUtil su = new SgFramework.SgUtil();

    public String sql = "";

    protected JsonObjectCollection j = null;
    protected JsonObjectCollection getData = null;
    protected JsonTextParser parser = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(su.Encrypt_AES("I83C8ZJD0RGCF371"));
        //var body = "{\"eType\": \"7\"," + "\n" + "\"mid\": \"7866\" }";

        //JsonObjectCollection j = new JsonObjectCollection();
        //j.Add(new JsonStringValue("eType","1"));
        //j.Add(new JsonStringValue("mid", "7866"));

        //Response.Write(sghttp_json("http://221.150.2.131:5555/API/R.aspx", j.ToString()));

    }

    #region JSON 호출 함수 
    public String sghttp_json(String url, String jsondata)
    {

        String retVal = "";

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            streamWriter.Write(jsondata);
        }
        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            retVal = streamReader.ReadToEnd();
        }
        return retVal;
    }
    #endregion

    #region 이미지처리 함수 

    #region BitmapToBase64

    public String BitmapToBase64(Bitmap img)
    {
        var ms = new System.IO.MemoryStream();
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        byte[] byteImage = ms.ToArray();
        var base64String = Convert.ToBase64String(byteImage);
        return base64String;
    }

    #endregion

    #region Base64ToImage 
    public System.Drawing.Image Base64ToImage(string base64String)
    {
        // Convert Base64 String to byte[]
        byte[] imageBytes = Convert.FromBase64String(base64String); MemoryStream ms = new MemoryStream(imageBytes, 0,
          imageBytes.Length);

        // Convert byte[] to Image
        ms.Write(imageBytes, 0, imageBytes.Length);
        Bitmap image = new Bitmap(ms, true);
        image.MakeTransparent(Color.White);
        return image;
    }
    #endregion

    #region ImageToBase64

    public string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            // Convert Image to byte[]
            image.Save(ms, format);
            byte[] imageBytes = ms.ToArray();

            // Convert byte[] to base 64 string
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }


    #endregion

    #region ResizeImage
    public Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }
    #endregion

    #endregion

    #region 이미지코덱정보 가져오기
    public ImageCodecInfo GetEncoder(ImageFormat format)
    {

        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
    #endregion

    #region Encrypt_AES : Decrypt_AES : AES 256Bit 암복호화 함수
    // 용도 : 주민번호, 신용카드번호 등 복호화 해야 하는 경우 처리
    public string Encrypt_AES(string InputText)
    {
        return Encrypt_AES(InputText, "gtm56km412#$%inb5040sr!@#$%&^&(I");
    }

    public string Encrypt_AES(string Input, string key)
    {
        String Output = "";

        if (Input != "")
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            Output = Convert.ToBase64String(xBuff);
        }

        return Output;
    }

    public string Decrypt_AES(string InputText)
    {
        return Decrypt_AES(InputText, "gtm56km412#$%inb5040sr!@#$%&^&(I");
    }

    public string Decrypt_AES(string Input, string key)
    {
        String Output = "";

        if (Input != "")
        {
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }

                xBuff = ms.ToArray();
            }

            Output = Encoding.UTF8.GetString(xBuff);
        }

        return patchnull(Output);
    }

    public String AESEncrypt128(String Input, String key)
    {

        RijndaelManaged RijndaelCipher = new RijndaelManaged();
        //RijndaelCipher.Padding = PaddingMode.None;
        //RijndaelCipher.Mode = CipherMode.ECB;
        //RijndaelCipher.Padding = PaddingMode.PKCS7;

        byte[] PlainText = System.Text.Encoding.Default.GetBytes(Input);
        byte[] Salt = Encoding.Default.GetBytes(key.Length.ToString());

        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);
        ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

        cryptoStream.Write(PlainText, 0, PlainText.Length);
        cryptoStream.FlushFinalBlock();

        byte[] CipherBytes = memoryStream.ToArray();

        memoryStream.Close();
        cryptoStream.Close();

        StringBuilder sb = new StringBuilder();
        foreach (byte b in CipherBytes)
        {
            sb.Append(b.ToString("X2"));
        }

        string EncryptedData = sb.ToString().ToUpper();

        return EncryptedData;
    }

    //AE_S128 복호화
    public String AESDecrypt128(String Input, String key)
    {
        Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
        for (int i = 0; i <= 255; i++)
        {
            hexindex.Add(i.ToString("X2"), (byte)i);
        }

        List<byte> hexres = new List<byte>();
        for (int i = 0; i < Input.Length; i += 2)
            hexres.Add(hexindex[Input.Substring(i, 2)]);

        byte[] EncryptedData = hexres.ToArray();

        RijndaelManaged RijndaelCipher = new RijndaelManaged();
        RijndaelCipher.Padding = PaddingMode.None;

        byte[] Salt = Encoding.Default.GetBytes(key.Length.ToString());

        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);
        ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
        MemoryStream memoryStream = new MemoryStream(EncryptedData);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

        byte[] PlainText = new byte[EncryptedData.Length];

        int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

        memoryStream.Close();
        cryptoStream.Close();

        string DecryptedData = Encoding.ASCII.GetString(PlainText, 0, DecryptedCount);

        return DecryptedData;
    }

    #region patchnull : null 문자 패치 ( 통신데이터에서 ASCII 코드가 00 인것을 공백으로 치환 )
    public String patchnull(String tmp)
    {
        String retVal = "";

        retVal = HttpUtility.UrlEncode(tmp).Replace("%00", "");

        return retVal;
    }
    #endregion

    #endregion

}
