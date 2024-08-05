namespace Pikda.OcrServiceIntegration
{
    public class OcrDevice : IOcrDevice
    {
        public int CreatModel()
        {
            var scanner = new OcrScannerForm();
            scanner.ShowDialog();
            scanner.Dispose();

            return 1;
        }

        public IOcrObject ReadCard(int modelId)
        {
            var scanner = new OcrScannerClientForm(modelId);
            scanner.ShowDialog();

            var ocrObject = scanner.OcrObject;

            scanner.Dispose();
            
            return ocrObject;

        }
    }
}
