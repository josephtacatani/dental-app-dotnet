namespace mydental.domain.Constants
{
    public static class AuthConstants
    {
        // Success Messages
        public const string LoginSuccess = "Login successful.";
        public const string RegisterSuccess = "User registered successfully.";
        public const string TokenRefreshSuccess = "Token refreshed successfully.";
        public const string LogoutSuccess = "Logout successful.";
        public const string PasswordResetSuccess = "Password reset successful.";

        // Error Messages
        public const string LoginFailed = "Invalid email or password.";
        public const string RegisterFailed = "Registration failed.";
        public const string TokenRefreshFailed = "Invalid refresh token.";
        public const string LogoutFailed = "Logout failed.";
        public const string UserNotFound = "User not found.";
        public const string PasswordResetFailed = "Password reset failed.";
        public const string ValidationFailed = "Validation failed.";
        public const string Unauthorized = "Unauthorized";
    }
}
