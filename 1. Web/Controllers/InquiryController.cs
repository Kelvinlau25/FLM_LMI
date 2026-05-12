using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using PAB.Repository;

namespace PAB.Controllers
{
    public class InquiryController : Controller
    {
        public CommonRepo CommonRepo = new CommonRepo();
        public InqRepo InqRepo = new InqRepo(); 
        
    }
}
