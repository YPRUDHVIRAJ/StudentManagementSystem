namespace StudentManagementSystem.Services
{
    public interface IFingerprintProSdk
    {
        byte[] CaptureFingerprint(); // Example method to capture a fingerprint
        bool ValidateFingerprint(byte[] fingerprintData); // Example method to validate a fingerprint
    }
}
