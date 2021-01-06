var mainContent;

$(function () {
    mainContent = $("#MainContent"); //sayfada içeriği değiştirilecek olan div'in ID'si
});

var routingApp = $.sammy("#MainContent", function () {

    this.get("#/Home/Main", function (context) { 
        showLoader();
        $.get("/Home/Main", function (data) {
            context.$element().html(data);
            //init();
            showLoader();

        });
    });
    this.get("#/Home/Index", function (context) {
        showLoader();
        $.get("/Home/Index", function (data) {
            context.$element().html(data);
            //init();
            showLoader();

        });
    });

    //******--------CRM--------******
    this.get("#/Customer/Edit/:id", function (context) {
        showLoader();
        $.get("/CRM/Customer/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/Customer/Index", function (context) {
        showLoader();
        $.get("/CRM/Customer/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PreApplication/Edit/:id", function (context) {
        showLoader();
        $.get("/CRM/PreApplication/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PreApplication/Index", function (context) {
        showLoader();
        $.get("/CRM/PreApplication/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/Visit/Edit/:id", function (context) {
        showLoader();
        $.get("/CRM/Visit/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Visit/Index", function (context) {
        showLoader();
        $.get("/CRM/Visit/Index", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Visit/Create", function (context) {
        showLoader();
        $.get("/CRM/Visit/Create", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/Customer/Create", function (context) {
        showLoader();
        $.get("/CRM/Customer/Create", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });

    this.get("#/Location/Edit/:id", function (context) {
        showLoader();
        $.get("/CRM/Location/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Location/Index", function (context) {
        showLoader();
        $.get("/CRM/Location/Index", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Location/Create", function (context) {
        showLoader();
        $.get("/CRM/Location/Create", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/VisitLog/Edit/:id", function (context) {
        showLoader();
        $.get("/CRM/VisitLog/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/VisitLog/Index", function (context) {
        showLoader();
        $.get("/CRM/VisitLog/Index", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/VisitLog/Create", function (context) {
        showLoader();
        $.get("/CRM/VisitLog/Create", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    //******--------STK--------******
    this.get("#/Stock/Edit/:id", function (context) {
        showLoader();
        $.get("/STK/Stock/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            init();
        });
    });
    this.get("#/Stock/Index", function (context) {
        showLoader();
        $.get("/STK/Stock/Index", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/StockMovement/Index", function (context) {
        showLoader();
        $.get("/STK/StockMovement/Index", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Stock/DebitList", function (context) {
        showLoader();
        $.get("/STK/Stock/DebitList", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Stock/AddProduct", function (context) {
        showLoader();
        $.get("/STK/Stock/AddProduct", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/StockMainCategory/Edit/:id", function (context) {
        showLoader();
        $.get("/STK/StockMainCategory/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/StockMainCategory/Index", function (context) {
        showLoader();
        $.get("/STK/StockMainCategory/Index", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/StockCategory/Edit/:id", function (context) {
        showLoader();
        $.get("/STK/StockCategory/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/StockCategory/Index", function (context) {
        showLoader();
        $.get("/STK/StockCategory/Index", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    //******--------SVC--------******
    this.get("#/Service/Edit/:id", function (context) {
        showLoader();
        $.get("/SVC/Service/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Service/Index", function (context) {
        showLoader();
        $.get("/SVC/Service/Index", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Service/Create", function (context) {
        showLoader();
        $.get("/SVC/Service/Create", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });

    //******--------PRT--------******
    this.get("#/Printer/Edit/:id", function (context) {
        showLoader();
        $.get("/PRT/Printer/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Printer/Create", function (context) {
        showLoader();
        $.get("/PRT/Printer/Create/", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/Printer/Index", function (context) {
        showLoader();
        $.get("/PRT/Printer/Index" , function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    //******--------DEF--------******
    this.get("#/Department/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/Department/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/Department/Index", function (context) {
        showLoader();
        $.get("/DEF/Department/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PreApplicationType/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/PreApplicationType/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PreApplicationType/Index", function (context) {
        showLoader();
        $.get("/DEF/PreApplicationType/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PreApplicationStatus/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/PreApplicationStatus/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PreApplicationStatus/Index", function (context) {
        showLoader();
        $.get("/DEF/PreApplicationStatus/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/CustomerType/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/CustomerType/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/CustomerType/Index", function (context) {
        showLoader();
        $.get("/DEF/CustomerType/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/VisitType/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/VisitType/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/VisitType/Index", function (context) {
        showLoader();
        $.get("/DEF/VisitType/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/VisitCategory/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/VisitCategory/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });

    this.get("#/VisitCategory/Index", function (context) {
        showLoader();
        $.get("/DEF/VisitCategory/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/ServiceCategory/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/ServiceCategory/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/ServiceCategory/Index", function (context) {
        showLoader();
        $.get("/DEF/ServiceCategory/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/DeviceCategory/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/DeviceCategory/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/DeviceCategory/Index", function (context) {
        showLoader();
        $.get("/DEF/DeviceCategory/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PrinterBrand/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/PrinterBrand/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PrinterBrand/Index", function (context) {
        showLoader();
        $.get("/DEF/PrinterBrand/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PrinterServiceType/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/PrinterServiceType/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PrinterServiceType/Index", function (context) {
        showLoader();
        $.get("/DEF/PrinterServiceType/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PrinterModel/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/PrinterModel/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/PrinterModel/Index", function (context) {
        showLoader();
        $.get("/DEF/PrinterModel/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/Vehicle/Edit/:id", function (context) {
        showLoader();
        $.get("/DEF/Vehicle/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/Vehicle/Index", function (context) {
        showLoader();
        $.get("/DEF/Vehicle/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });



    //******--------SYS--------******
    this.get("#/AppUser/Index", function (context) {
        showLoader();
        $.get("/SYS/AppUser/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/AppUser/Edit/:id", function (context) {
        showLoader();
        $.get("/SYS/AppUser/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });

    this.get("#/Menu/Index", function (context) {
        showLoader();
        $.get("/SYS/Menu/Index", function (data) {
            context.$element().html(data);
            //init();
            showLoader();

        });
    });
    this.get("#/Menu/Edit/:id", function (context) {
        showLoader();
        $.get("/SYS/Menu/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            //init();
            showLoader();

        });
    });

    this.get("#/Area/Edit/:id", function (context) {
        showLoader();
        $.get("/SYS/Area/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/Area/Index", function (context) {
        showLoader();
        $.get("/SYS/Area/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/AppUser/AppUserLog", function (context) {
        showLoader();
        $.get("/SYS/AppUser/AppUserLog", function (data) {
            context.$element().html(data);
            showLoader();
        });
    });
    this.get("#/City/Edit/:id", function (context) {
        showLoader();
        $.get("/SYS/City/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/City/Index", function (context) {
        showLoader();
        $.get("/SYS/City/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });

    this.get("#/Town/Edit/:id", function (context) {
        showLoader();
        $.get("/SYS/Town/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/Town/Index", function (context) {
        showLoader();
        $.get("/SYS/Town/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });


    this.get("#/AuthCode/Edit/:id", function (context) {
        showLoader();
        $.get("/SYS/AuthCode/Edit/" + context.params.id, function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });
    this.get("#/AuthCode/Index", function (context) {
        showLoader();
        $.get("/SYS/AuthCode/Index", function (data) {
            context.$element().html(data);
            showLoader();
            //init();
        });
    });

});

$(function () {
    routingApp.run("#/Home/Main"); //ilk yüklendiğinde yönlendireceği sayfa
});


function init() {  // kullanılmıyor sanırım...
    mLayout.initAside();
    $("#sticky").empty();
    $("#sticky").hide();
}

function initQuestion() {
    mLayout.initAside();
}

function IfLinkNotExist(type, path) { //link sağlık kontrolü
    if (!(type !== null && path !== null))
        return false;

    var isExist = true;

    if (type.toLowerCase() === "get") {
        if (routingApp.routes.get !== undefined) {
            $.map(routingApp.routes.get, function (item) {
                if (item.path.toString().replace("/#", "#").replace(/\\/g, '').replace("$/", "").indexOf(path) >= 0) {
                    isExist = false;
                }
            });
        }
    } else if (type.toLowerCase() === "post") {
        if (routingApp.routes.post !== undefined) {
            $.map(routingApp.routes.post, function (item) {
                if (item.path.toString().replace("/#", "#").replace(/\\/g, '').replace("$/", "").indexOf(path) >= 0) {
                    isExist = false;
                }
            });
        }
    }
    return isExist;
}  