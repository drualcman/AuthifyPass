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
        var binaryMap = ConvertToBinaryMap(bitmap);
        return DecodeWithZXing(binaryMap);
    }

    private SKBitmap Base64ToSKBitmap(string base64Image)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Image.Split(',')[1]);
        using var stream = new MemoryStream(imageBytes);
        return SKBitmap.Decode(stream) ?? throw new InvalidOperationException("Failed to decode image.");
    }

    private LuminanceSource ConvertToBinaryMap(SKBitmap bitmap)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;
        byte[] luminanceArray = new byte[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                SKColor pixel = bitmap.GetPixel(x, y);
                int gray = (int)(pixel.Red * 0.299 + pixel.Green * 0.587 + pixel.Blue * 0.114);
                luminanceArray[y * width + x] = (byte)gray;
            }
        }

        return new RGBLuminanceSource(luminanceArray, width, height);
    }

    private string DecodeWithZXing(LuminanceSource luminanceSource)
    {
        var reader = new BarcodeReaderGeneric { Options = { TryHarder = true } };
        var result = reader.Decode(luminanceSource);

        return result?.Text ?? "No QR code detected.";
    }
}
