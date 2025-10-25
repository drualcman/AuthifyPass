namespace AuthifyPass.API.Repositories;
internal class QRGeneratorRepository : IQRGeneratorRepository
{
    public string GenerateQRCode(QRDataDto qrData)
    {
        string otpauthUrl = GenerateOtpAuthUrl(qrData);
        BarcodeWriterSvg writer = new BarcodeWriterSvg
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = 250,
                Width = 250,
                Margin = 0
            }
        };
        var svgImage = writer.Write(otpauthUrl);
        return svgImage.Content;
    }

    private string GenerateOtpAuthUrl(QRDataDto qrData)
    {
        return $"otpauth://totp/{Uri.EscapeDataString(qrData.ClientId)}:{qrData.Name}" +
               $"?secret={qrData.SharedKey}" +
               $"&issuer={Uri.EscapeDataString(qrData.AppName)}" +
               $"&algorithm=SHA1" +
               $"&digits=6" +
               $"&period=30";
    }
}
