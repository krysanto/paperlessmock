namespace Paperless.OCR;

public interface IOcrClient
{
    string OcrPdf(Stream pdfStream);
}
