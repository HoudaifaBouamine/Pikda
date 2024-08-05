namespace Pikda.OcrServiceIntegration
{
    public interface IOcrDevice
    {
        /// <summary>
        /// Show OCR Scanner reading form, extract data from scanned card following the selected model
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Ocr Object containing the scanned data</returns>
        IOcrObject ReadCard(int modelId);

        /// <summary>
        /// Show OCR Scanner creation form
        /// </summary>
        /// <returns>Id of new created OCR model, return 0 if process does not success</returns>
        int CreatModel();
    }
}
