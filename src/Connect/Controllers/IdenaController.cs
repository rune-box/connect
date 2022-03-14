using Connect.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Connect.Controllers {
  // ref: https://docs.idena.io/docs/developer/desktop/sign-in
  public class IdenaController : BaseController {
    const string Suffix_Address = "___address";
    const string Suffix_Nounce = "___nounce";

    const string Key_Address = "address";
    const string Key_Token = "token";
    const string Key_Signature = "signature";

    class IdenaAuthParam {
      public string Address { get; set; }
      public string Token { get; set; }

      public IdenaAuthParam(string address, string token) {
        Address = address;
        Token = token;
      }

      public bool IsNullOrEmpty() {
        return string.IsNullOrEmpty( Address ) || string.IsNullOrEmpty( Token );
      }

    }

    public IdenaController(ILogger<HomeController> logger) : base( logger ) {
      //
    }



    [Route("idena")]
    public ActionResult Index() {
      ViewBag.Token_Idena_Desktop = Guid.NewGuid().ToString();
      ViewBag.Token_Idena_Web = Guid.NewGuid().ToString();
      return View();
    }

    async void readTokenAddress(IdenaAuthParam param) {
      if(param == null) {
        return;
      }
      if (string.IsNullOrEmpty( param.Token ) || string.IsNullOrEmpty( param.Address )) {
        if (this.Request.Body != null) {
          using (StreamReader stream = new StreamReader( this.Request.Body )) {
            string bodyContent = await stream.ReadToEndAsync();
            var reqToken = JToken.Parse( bodyContent );
            if (reqToken != null) {
              param.Token = (string)reqToken["token"];
              param.Address = (string)reqToken["address"];
            }
          }
        }
      }
    }

    async Task<Dictionary<string, string>> readParams(string key1, string key1Value, string key2, string key2Value) {
      Dictionary<string, string> dict = new Dictionary<string, string>();
      dict[key1] = key1Value;
      dict[key2] = key2Value;

      if (string.IsNullOrEmpty( key1Value ) || string.IsNullOrEmpty( key2Value )) {
        if (this.Request.Body != null) {
          using (StreamReader stream = new StreamReader( this.Request.Body )) {
            string bodyContent = await stream.ReadToEndAsync();
            var reqToken = JToken.Parse( bodyContent );
            if (reqToken != null) {
              dict[key1] = (string)reqToken[key1];
              dict[key2] = (string)reqToken[key2];
            }
          }
        }
      }
      return dict;
    }

    [HttpPost]
    [Route( "idena/start-session" )]
    public async Task<ActionResult> StartSession(string token, string address) {
      try {
        string guid = Guid.NewGuid().ToString();
        string nonce = string.Format( "signin-{0}", guid );
        var param = await readParams( Key_Token, token, Key_Address, address );
        // check again
        if (string.IsNullOrEmpty( param[Key_Token] ) || string.IsNullOrEmpty( param[Key_Address] )) {
          return new JsonResult( new {
            success = false,
            error = "token/address is null"
          } );
        }

        token = param[Key_Token];
        address = param[Key_Address];

        HttpContext.Session.SetString( token + Suffix_Address, address );
        HttpContext.Session.SetString( token + Suffix_Nounce, nonce );
        await HttpContext.Session.CommitAsync();

        return new JsonResult( new {
          success = true,
          data = new {
            nonce = nonce
          }
        } );
      }
      catch (Exception ex) {
        return new JsonResult( new {
          success = false,
          error = ex.Message
        } );
      }
    }

    [HttpPost]
    [Route( "idena/authenticate" )]
    public async Task<ActionResult> Authenticate(string token, string signature) {
      var param = await readParams( Key_Token, token, Key_Signature, signature );
      // check again
      if (string.IsNullOrEmpty( param[Key_Token] ) || string.IsNullOrEmpty( param[Key_Signature] )) {
        return new JsonResult( new {
          success = false,
          error = "token/signature is null"
        } );
      }

      token = param[Key_Token];
      signature = param[Key_Signature];

      try {
        await HttpContext.Session.LoadAsync();
        var savedAddress = HttpContext.Session.GetString( token + Suffix_Address );
        var savedNounce = HttpContext.Session.GetString( token + Suffix_Nounce );
        if (savedAddress == null || savedNounce == null) {
          return new JsonResult( new {
            success = false,
            error = "Session expired?"
          } );
        }

        string address = Tools.EthereumUtils.GetAddressFromSigature( signature, savedNounce );
        bool isEqual = string.Equals( savedAddress, address, StringComparison.CurrentCultureIgnoreCase );

        return new JsonResult( new {
          success = true,
          data = new {
            authenticated = isEqual
          }
        } );
      }
      catch (Exception ex) {
        return new JsonResult( new {
          success = false,
          error = ex.Message
        } );
      }
    }

  }

}
