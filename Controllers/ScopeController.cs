using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using cancel.Models;

namespace cancel.Controllers
{
    public class ScopeController : Controller
    {
        private readonly ITransisionService _transisionService1;
        private readonly ITransisionService _transisionService2;
        private readonly IScopedService _scopedService1;
        private readonly IScopedService _scopedService2;
        private readonly ISingletonService _singletonService1;
        private readonly ISingletonService _singletonService2;

        public ScopeController(ITransisionService tran1, ITransisionService tran2,
        IScopedService scoped1, IScopedService scoped2, ISingletonService single1,
        ISingletonService single2)
        {
            _transisionService1 = tran1;
            _transisionService2 = tran2;
            _scopedService1 = scoped1;
            _scopedService2 = scoped2;
            _singletonService1 = single1;
            _singletonService2 = single2;
        }
        public IActionResult Index()
        {
            ViewBag.Transistion1 = $"First transision instance {_transisionService1.GetGuid().ToString()}";
            ViewBag.Transistion2 = $"Second transision instance {_transisionService2.GetGuid().ToString()}";
            ViewBag.Scope1 = $"First Scoped instance {_scopedService1.GetGuid().ToString()}";
            ViewBag.Scope2 = $"Second transision instance {_scopedService2.GetGuid().ToString()}";
            ViewBag.Singleton1 = $"First Singleton instance {_singletonService1.GetGuid().ToString()}";
            ViewBag.Singleton2 = $"Second Singleton instance {_singletonService2.GetGuid().ToString()}";
            return View();
        }
    }
}