using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security;
using Test101.Models;

namespace Test101.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Upload(IFormFile uploadFile)
        {


            // アップロードファイル名の取得
            string up_filename = uploadFile.FileName;
            string up_file_ext = Path.GetExtension(up_filename);
            // 拡張子のチェック
            string[] accept_ext = new string[]{"jpg","png" }; // 許可する拡張子のリスト
            bool is_accept_ext = false;                         // 許可する拡張子かどうかのフラグ
            foreach(string extensions in accept_ext)
            {
                // 許可された拡張子であればフラグをtrue
                if(extensions == up_file_ext)
                {
                    is_accept_ext = true;
                }
            }

            // 許可されてない拡張子の場合の処理
            if (is_accept_ext)
            {
                ViewData["up_file_name"] = null;
                return View("Index","Home");
            }

            // アップロードファイルの保存、事前に/wwwroot/img/productフォルダを作成しておく。
            // なおproductフォルダに何か入ってないとプロジェクトで認識されないので、
            // 適当なテキストファイル「none.txt」など入れて置く。

            // 保存するファイルの名前の生成、ファイル名の数字をDBの主キーにするなど被らないようにしてください。
            Random r = new Random();
            string save_filename =  r.Next(1000)+ up_file_ext; // 保存するファイル名

            using (var stream = System.IO.File.Create("./wwwroot/image/product/" + save_filename))
            {
                ViewData["up_file_name"] = save_filename;
                // 保存
                uploadFile.CopyTo(stream);
            }

            return View("Index", "Home");

        }
    }
}