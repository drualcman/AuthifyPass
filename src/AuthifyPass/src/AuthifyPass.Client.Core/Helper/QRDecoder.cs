using static ZXing.RGBLuminanceSource;

namespace AuthifyPass.Client.Core.Helper;
public class QRDecoder(string base64Image)
{
    public static string Decode(string input)
    {
        QRDecoder decoder = new(input);
        return decoder.DecodeQRCode();
    }

    internal string DecodeQRCode()
    {
        using var bitmap = Base64ToSKBitmap(base64Image);
        Console.WriteLine("------ DecodeQRCode 1 --------");
        var binaryMap = ConvertToBinaryMap(bitmap);
        Console.WriteLine("------ DecodeQRCode 3 --------");
        return DecodeWithZXing(binaryMap);
    }

    private SKBitmap Base64ToSKBitmap(string base64Image)
    {
        string[] sections = base64Image.Split(',');
        string base64 = sections.Length > 1 ? sections[1] : sections[0];
        byte[] imageBytes = Convert.FromBase64String(base64);
        using var stream = new MemoryStream(imageBytes);
        return SKBitmap.Decode(stream) ?? throw new InvalidOperationException("Failed to decode image.");
    }

    private LuminanceSource ConvertToBinaryMap(SKBitmap bitmap)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;
        if (width <= 0 || height <= 0)
        {
            throw new InvalidOperationException("The bitmap dimensions are invalid.");
        }
        byte[] rgbRawBytes = bitmap.Bytes;
        if (rgbRawBytes.Length != width * height * 4)
        {
            throw new InvalidOperationException("The raw data length does not match the expected size.");
        }
        Console.WriteLine("------ DecodeQRCode 2 --------");
        return new RGBLuminanceSource(rgbRawBytes, width, height, BitmapFormat.RGB32);
    }


    private string DecodeWithZXing(LuminanceSource luminanceSource)
    {
        var reader = new BarcodeReaderGeneric { Options = { TryHarder = true } };
        Console.WriteLine("------ DecodeQRCode 4 --------");
        var result = reader.Decode(luminanceSource);
        Console.WriteLine("------ DecodeQRCode 5 --------");
        return result?.Text ?? "No QR code detected.";
    }
}
