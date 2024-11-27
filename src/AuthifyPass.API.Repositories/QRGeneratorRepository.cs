namespace AuthifyPass.API.Repositories;
internal class QRGeneratorRepository : IQRGeneratorRepository
{
    public string GenerateQRCode(QRDataDto qrData)
    {
        string data = JsonSerializer.Serialize(qrData);
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
        var svgImage = writer.Write(data);
        return svgImage.Content;
    }
}
