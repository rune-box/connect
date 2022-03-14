namespace Connect.Tools {
  public static class StringUtils {
    public static byte[] DecodeHexToBytes(string hex) {
      if (hex == null)
        throw new ArgumentNullException( "hex" );
      hex = hex.Replace( ",", "" );
      hex = hex.Replace( "\n", "" );
      hex = hex.Replace( "\\", "" );
      hex = hex.Replace( " ", "" );
      if (hex.Length % 2 != 0) {
        hex += "20";//空格
        throw new ArgumentException( "hex is not a valid number!", "hex" );
      }
      // 需要将 hex 转换成 byte 数组。
      byte[] bytes = new byte[hex.Length / 2];
      for (int i = 0; i < bytes.Length; i++) {
        try {
          // 每两个字符是一个 byte。
          bytes[i] = byte.Parse( hex.Substring( i * 2, 2 ),
          System.Globalization.NumberStyles.HexNumber );
        }
        catch {
          // Rethrow an exception with custom message.
          throw new ArgumentException( "hex is not a valid hex number!", "hex" );
        }
      }
      return bytes;
    }

    public static string DecodeHexToString(string hex, System.Text.Encoding encoding) {
      byte[] bytes = DecodeHexToBytes( hex );
      return encoding.GetString( bytes );
    }

  }

}
