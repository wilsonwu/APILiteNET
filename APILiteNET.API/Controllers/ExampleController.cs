using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APILiteNET.API.Controllers
{
    public class ExampleController : APIControllerBase
    {
        //
        // GET: /Example/
        [APIAuthorize]
        [RequiredAuthorze]
        public ActionResult Index()
        {
            return APIResult(new
            {
                Name = "Hello",
                Value = "World",
            });
        }

        [RequiredAuthorze]
        public ActionResult GetOrder(long OrderID)
        {
            List<dynamic> datas1 = new List<dynamic>();
            dynamic data11 = new
            {
                OrderPhotoID = 1,
                OrderPhotoStatusCode = 101,
                OrderPhotoStatusName = "通过",
                OrderPhotoRejectReason = DBNull.Value,
                OrderPhotoImageURL = "http://xxxx/1.jpg",
            };
            dynamic data21 = new
            {
                OrderPhotoID = 2,
                OrderPhotoStatusCode = 102,
                OrderPhotoStatusName = "驳回",
                OrderPhotoRejectReason = "不符合要求，头太小",
                OrderPhotoImageURL = "http://xxxx/2.jpg",
            };
            datas1.Add(data11);
            datas1.Add(data21);

            List<dynamic> datas11 = new List<dynamic>();
            dynamic data111 = new
            {
                OrderPaintingID = 1,
                OrderPaintingImageURL = "http://xxxx/1.jpg",
            };
            dynamic data211 = new
            {
                OrderPaintingID = 1,
                OrderPaintingImageURL = "http://xxxx/1.jpg",
            };
            datas11.Add(data111);
            datas11.Add(data211);


            List<int> datas111 = new List<int>();

            datas111.Add(1);
            datas111.Add(2);


            dynamic result = new
            {
                OrderID = 111,
                OrderPrice = 200,
                OrderStatusCode = 101,
                OrderStatusName = "完成",
                OrderPhotos = datas1,
                OrderPaintings = datas11,
                Templates = datas111,
            };

            return APIResult(result);
        }

    }
}
