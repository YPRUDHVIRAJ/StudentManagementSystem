namespace StudentManagementSystem.Services
{
    public class FingerprintService
    {
        private readonly IFingerprintProSdk _fingerprintProSdk;

        public FingerprintService(IFingerprintProSdk fingerprintProSdk)
        {
            _fingerprintProSdk = fingerprintProSdk;
        }

        public byte[] CaptureFingerprint()
        {
            return _fingerprintProSdk.CaptureFingerprint();
        }

        public bool ValidateFingerprint(byte[] fingerprintData)
        {
            return _fingerprintProSdk.ValidateFingerprint(fingerprintData);
        }
    }
}
