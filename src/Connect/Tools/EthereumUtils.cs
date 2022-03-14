using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Util;

namespace Connect.Tools {
  public static class EthereumUtils {
    // ref: https://github.com/idena-network/idena-auth/blob/30c3f8f5406592bb297bd78a4e931ed2228d017b/core/auth.go#L109
    public static string GetAddressFromSigature(string signature, string nonce) {
      var signer = new Nethereum.Signer.EthereumMessageSigner();
      var nonceBytes = System.Text.UTF8Encoding.UTF8.GetBytes( nonce );

      // example
      // hash := rlp.Hash(value)
      // signatureBytes, err := hexutil.Decode(signature)
      // pubKey, err := crypto.Ecrecover(hash[:], signatureBytes)
      // addr, err := crypto.PubKeyBytesToAddress(pubKey)

      Nethereum.Util.Sha3Keccack keccack = new Nethereum.Util.Sha3Keccack();
      var nonceHash = keccack.CalculateHash( nonceBytes );
      nonceHash = keccack.CalculateHash( nonceHash );
      //var nonceHashString = nonceHash.ToHex( true );
      // nonceHashString should be 0x4243c0cab05a624ae36968be17e6f93112e45a445bbbd9cbeb1f8ed8aa6a3367
      var ecdsaSig = EthereumMessageSigner.ExtractEcdsaSignature( signature );
      var pubKeyHex = signer.EcRecover( nonceHash, ecdsaSig );
      return pubKeyHex;
    }

    //public static string PubKeyBytesToAddress(byte[] pubKeyBytes, bool sanitize = false) {
    //  //var bytes = Nethereum.Util.Sha3Keccack.Current.CalculateHash( publicKey );
    //  //var sig = Nethereum.Signer.ECDSASignatureFactory.ExtractECDSASignature( "signature" );
    //  //Nethereum.Util.AddressUtil.Current.ConvertToChecksumAddress
    //  // ref: L201: https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Signer/EthECKey.cs
    //  if (sanitize && pubKeyBytes.Length != 64) {
    //    //pubKey = Buffer.from((0, secp256k1_1.publicKeyConvert)(pubKey, false).slice(1));
    //  }
    //  //NBitcoin.Secp256k1.ECPubKey.TryRecover
    //  var ctxSecp256k1 = NBitcoin.Secp256k1.Context.Instance;
      
    //  // check: pubKey.length === 64
    //  var initaddr = new Nethereum.Util.Sha3Keccack().CalculateHash( pubKeyBytes );
    //  var addr = new byte[initaddr.Length - 12];
    //  Array.Copy( initaddr, 12, addr, 0, initaddr.Length - 12 );
    //  string hex = addr.ToHex();
    //  string address = Nethereum.Util.AddressUtil.Current.ConvertToChecksumAddress( hex );
    //  return address;
    //}

    //public static string PubKeyToAddress(string publicKey) {
    //  //var bytes = Nethereum.Util.Sha3Keccack.Current.CalculateHash( publicKey );
    //  //var sig = Nethereum.Signer.ECDSASignatureFactory.ExtractECDSASignature( "signature" );
    //  //Nethereum.Util.AddressUtil.Current.ConvertToChecksumAddress
    //  // ref: L201: https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Signer/EthECKey.cs
    //  var keccack = new Nethereum.Util.Sha3Keccack();
    //  var initaddr = keccack.CalculateHash( publicKey );
    //  //var addr = new byte[initaddr.Length - 12];
    //  //Array.Copy( initaddr, 12, addr, 0, initaddr.Length - 12 );
    //  //string hex = addr.ToHex();
    //  string address = Nethereum.Util.AddressUtil.Current.ConvertToChecksumAddress( initaddr );
    //  return address;
    //}

  }

}
