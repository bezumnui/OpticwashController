namespace OpticwashController.Message
{
	// Token: 0x02000006 RID: 6 (Copied from original)
	public static class MessageHelper
	{
		public static byte GenerateChecksum(byte[] bytes)
		{
			int num = 0;

			foreach (byte b in bytes)
				num += b;

			return (byte)num;
		}

		public static bool ValidateChecksum(byte[] bytes, byte checksum)
		{
			return bytes != null && checksum.Equals(GenerateChecksum(bytes));
		}

		public static byte[] StringToByteArray(string hex)
		{
			return (from x in Enumerable.Range(0, hex.Length)
				where x % 2 == 0
				select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
		}
	}
}
