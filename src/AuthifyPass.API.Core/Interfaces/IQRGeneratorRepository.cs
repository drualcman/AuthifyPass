namespace AuthifyPass.API.Core.Interfaces;
public interface IQRGeneratorRepository
{
    string GenerateQRCode(QRDataDto qrData);
}
