using ZXing;
using ZXing.Common;

namespace AuthifyPass.Views.Services;

public static class QrCodeSvgRenderer
{
    // Renders an SVG QR for the given content. Returns an empty string on any failure so callers can
    // simply hide the QR (no broken image, no error) instead of crashing. ZXing's SVG renderer is pure
    // managed code (no System.Drawing), so it is safe under Blazor WebAssembly.
    public static string RenderSvg(string content)
    {
        string result = string.Empty;

        if (!string.IsNullOrWhiteSpace(content))
        {
            try
            {
                BarcodeWriterSvg writer = new BarcodeWriterSvg
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Width = 260,
                        Height = 260,
                        Margin = 1
                    }
                };

                string svg = writer.Write(content).Content;

                // ZXing emits an XML prolog ("<?xml ...?>"); inline SVG in HTML must start at the
                // <svg> element, so drop anything before it.
                int svgStart = svg.IndexOf("<svg", StringComparison.Ordinal);
                if (svgStart >= 0)
                {
                    result = svg.Substring(svgStart);
                }
            }
            catch (Exception)
            {
                result = string.Empty;
            }
        }

        return result;
    }
}
