using Connect.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Connect.Controllers {
  public class HomeController : BaseController {

    public HomeController(ILogger<HomeController> logger): base(logger) {
    }

    public IActionResult Index() {
      // test
      //string address = "0xc5f49cc53478f025d9b2a5d6875e19d9508cc43f";
      //string token = "428489af-3ca1-4861-b1c7-5f634f6466e2";
      //string guid = "0652c409-17ef-4ad6-b580-3faaefcc204d"; // test
      //string nonce = string.Format( "signin-{0}", guid );
      //string signature = "0x048cbb12117a1bc66fe8af99cfb79539895c0349713f38c77af3289c77b00bd015ff577bdbc6e71725825793b931aa4625ccaa24bf5e341002d2f51d4526f3de01";
      //var addr = Tools.EthereumUtils.GetAddressFromSigature( signature, nonce );

      return View();
    }

    public IActionResult Privacy() {
      return View();
    }

  }
}