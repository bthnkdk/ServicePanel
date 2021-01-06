function documentReady(root, controller, action) {
    $(document).ajaxComplete(function () {
        $('.field-validation-error').each(function () {
            var $this = $(this);
            if (!$this.children().first().is('span')) {
                var $msg = $('<span/>').html($this.html());
                $(this).html($msg);
            }
        });
    });
}

function initForm() {
    $('a[data-action="collapse"]').unbind();
    $('a[data-action="collapse"]').on('click', function (e) {
        e.preventDefault();
        $(this).closest('.card').children('.card-content').collapse('toggle');
        $(this).closest('.card').find('[data-action="collapse"] i').toggleClass('ft-minus ft-plus');

    });

    $('a[data-action="expand"]').unbind();
    $('a[data-action="expand"]').on('click', function (e) {
        e.preventDefault();
        $(this).closest('.card').find('[data-action="expand"] i').toggleClass('ft-maximize ft-minimize');
        $(this).closest('.card').toggleClass('card-fullscreen');
    });

    $(".heading-elements-toggle").on("click", function () {
        $(this).parent().children(".heading-elements").toggleClass("visible");
    });
    $('a[data-action="reload"]').on('click', function () {
        var block_ele = $(this).closest('.card');
        block_ele.block({
            message: '<div class="ft-refresh-cw icon-spin font-medium-2"></div>',
            timeout: 1000, 
            overlayCSS: {
                backgroundColor: '#FFF',
                cursor: 'wait'
            },
            css: {
                border: 0,
                padding: 0,
                backgroundColor: 'none'
            }
        });
    });
}

function refresh(e, url) {
    var div = $("#" + e);
    div.html('İşleniyor ...');
    $.ajax({
        url: url,
        type: "POST",
        success: function (response, status, xhr) {
            div.html(response);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            div.html(textStatus);
        }
    });
}

var aniTemp = '<div class="spinner"><div class="dot1"></div><div class="dot2"></div></div>';

function showProgress() {
    mainContent.html(aniTemp);
}

function start(e) {
    $('<div id="loader" class="text-danger"><img src="/Content/loading.gif" /> işleniyor...</div>').insertAfter($("#" + e));
}

function finish(e) {
    $("#" + e).next().remove();
}

function phoneMask(id) {
    $("#" + id).inputmask("mask", { mask: "0(999) 999-9999" });
}

function timeMask(id) {
    $("#" + id).inputmask("mask", { mask: "99:99" });
}

function fckCheck() {
    try {
        for (var ck in CKEDITOR.instances) {
            CKEDITOR.instances[ck].updateElement();
        }
    } catch (e) {
        console.log(e);
    }
}

function searchGrid(e, grid) {
    if (e.keyCode == 13) {
        loadGrid(grid);
        return false;
    }
}

function loadGrid(grid) {
    $('#' + grid).data('api').load();
}

function go(url) {
    location.href = url;
}

function checkAll(isChecked, name) {
    if (isChecked) {
        $('input[name="' + name + '"]').each(function () {
            this.checked = true;
        });
    } else {
        $('input[name="' + name + '"]').each(function () {
            this.checked = false;
        });
    }
}

function checkAllByClass(className, isChecked) {
    if (isChecked) {
        $('input[class="' + className + '"]').each(function (e) {
            this.checked = true;
        });
    } else {
        $('input[class="' + className + '"]').each(function (e) {
            this.checked = false;
        });
    }
}

function showError(message) {
    awem.notif(message, 3000, 'o-err');
}
function showSuccess(message) {
    awem.notif(message, 3000, 'o-msg');
}

function ajaxFormCompleted(o) {
    if (o.Error) {
        awem.notif(o.Error, 15000, 'o-err');
        return;
    }
    if (o.Url && o.Content) {
        awem.notif(o.Content, 5000, 'o-msg');
        setTimeout(function () {
            document.location.href = o.Url;
            return;
        }, 2000);
    }
    if (o.Url) {
        document.location.href = o.Url;
        return;
    }
    if (o.Content)
        awem.notif(o.Content, 5000, 'o-msg');

    if (o.ErrorList) {
        showErrorMessagesOnValidate(o.ErrorList);
    }
}
function showErrorMessagesOnValidate(errors) {
    if (!errors)
        return;
    errors.forEach(function (item) {
        if (item.message)
            awem.notif(item.message, 3000, 'o-err');
        else
            awem.notif(item, 3000, 'o-err');
    });
}
function toExcel(grid, formPostFix) {
    var $form = $('#excelForm').empty();
    if (formPostFix)
        $form = $('#excelForm' + formPostFix).empty();
    var req = $('#' + grid).data('api').getRequest();
    for (var i = 0; i < req.length; i++) {
        $form.append("<input type='hidden' name='" + req[i].name + "' value='" + req[i].value + "'/>");
    }
    $form.submit();
}
(function ($) {
    utils.logStatus = function () {
        return function (model) {
            if (model.Status == "Başarılı") {
                return "<span class='text-success'>" + model.Status + "</span>";
            } else {
                return "<span class='text-danger'>" + model.Status + "</span>";
            }
        };
    };

    utils.workerStatus = function () {
        return function (model) {
            if (model.IsSuccess) {
                return "<span class='text-success'>Başarılı</span>";
            } else {
                return "<span class='text-danger'>Hata</span>";
            }
        };
    };

    utils.planningStatus = function () {
        return function (model) {
            if (model.IsCompleted)
                return "<span class='badge badge-success' style='font-size:12px;'>Yapıldı</span>";
            else
                return "<span class='badge badge-warning' style='font-size:12px;'>İşlemde</span>";
        };
    };

    utils.sealedStatus = function () {
        return function (model) {
            if (model.IsSealed)
                return "<span class='badge badge-danger' style='font-size:12px;'>M</span>";
            else
                return "";
        };
    };

    utils.cPriceCount = function () {
        return function (model) {
            if (model.LeftPayDay == 0)
                return "";
            else
                return "<span class='badge badge-danger' style='font-size:11px;'>" + model.LeftPayDay + "</span>";
        };
    };

    utils.cPKCount = function () {
        return function (model) {
            if (model.NextPCDate == 0)
                return "";
            else {
                if (model.NextPCDate > 150)
                    return "<span class='badge badge-success' style='font-size:11px;'>" + model.NextPCDate + "</span>";
                else if (model.NextPCDate > 50)
                    return "<span class='badge badge-warning' style='font-size:11px;'>" + model.NextPCDate + "</span>";
                else if (model.NextPCDate > 10)
                    return "<span class='badge badge-danger' style='font-size:11px;'>" + model.NextPCDate + "</span>";
            }
        };
    };

    utils.cTKCount = function () {
        return function (model) {
            if (model.NextTCDate == 0)
                return "";
            else {
                if (model.NextTCDate > 150)
                    return "<span class='badge badge-success' style='font-size:11px;'>" + model.NextTCDate + "</span>";
                else if (model.NextTCDate > 50)
                    return "<span class='badge badge-warning' style='font-size:11px;'>" + model.NextTCDate + "</span>";
                else if (model.NextTCDate > 10)
                    return "<span class='badge badge-danger' style='font-size:11px;'>" + model.NextTCDate + "</span>";
            }
        };
    };

    utils.bgColor = function () {
        return function (model) {
            return "<div style='width:32px;height:32px;background-color:" + model.Color + ";'></div>";
        };
    };

    utils.downloadPdf = function () {
        return function (model) {
            if (model.Source == "Milenyum")
                return "";
            else
                return "<button type=\"button\" class=\"awe-btn awe-nonselect editbtn\" onclick=\"location.href ='/Checklist/Download/" + model.Id + "' ;\"><span class='fa fa-file-pdf text-danger'></span></button>";
        };
    };

    utils.isDownloadable = function (gridId) {
        return function (model) {
            if (model.isDownloadable) {
                return "<button type=\"button\" onclick=\"go('/WorkerLog/File/?rowId=" + model.RowId + "');\" class=\"awe-btn awe-nonselect editbtn\" style=\"width:26px;\"><span class='fa fa-arrow-alt-circle-down text-warning'></span></button>";
            }
            else {
                "";
            }
        }
    }

    utils.deleteFormat = function (popupName, gridId) {
        return function (model) {
            if (model.IsDeleted) {
                return "<button type='button' class='awe-btn delbtn' onclick=\"utils.restore('" + gridId + "'," + model.Id + ")\"><span class='fa fa-reply-all text-warning'></span></button>";
            }

            return "<button type='button' class='awe-btn delbtn' onclick=\"awe.open('" + popupName + "', { params:{ id: " + model.Id + " }}, event)\"><span class='fa fa-trash text-danger'></span></button>";
        };
    };

    utils.deleteFormat2 = function (popupName, gridId) {
        return function (model) {
            if (model.IsDeleted) {
                return "<button type='button' class='awe-btn delbtn' onclick=\"alert('Bu kayıt geri alınamaz !')\"><span class='fa fa-reply-all text-warning'></span></button>";
            }

            return "<button type='button' class='awe-btn delbtn' onclick=\"awe.open('" + popupName + "', { params:{ id: " + model.Id + " }}, event)\"><span class='fa fa-trash text-danger'></span></button>";
        };
    };


    utils.restore = function (gridId, id) {
        var $grid = $('#' + gridId);
        var api = $grid.data('api');
        var xhr = api.update(id, { restore: true });

        $.when(xhr).done(function () {
            var $row = api.select(id)[0];
            awe.flash($row);
        });
    };

    utils.lookupRestored = function (listId, key, func) {
        return function (o) {
            $('#' + listId).find('[data-val="' + o[key] + '"]').fadeOut(300, function () {
                $(this).after($.trim(o.Content)).remove();
                if (func) func();
            });
        };
    };

    utils.duration = function (hourw, hoursw, minw) {
        return function (val) {
            var mval = parseInt(val, 10);
            if (isNaN(mval)) return val;
            var hour = Math.floor(mval / 60);
            var minute = mval % 60;
            var res = "";
            if (hour > 0)
                res += hour + " " + (hour > 1 ? hoursw : hourw) + " ";
            if (minute > 0)
                res += minute + " " + minw;

            return res;
        };
    };
})(jQuery);


utils.servicePriority = function () {
    return function (model) {
        if (model.Priority == 1)
            return "Düşük";
        if (model.Priority == 2)
            return "Normal";
        if (model.Priority == 3)
            return "Yüksek";
    };
};

utils.serviceStatus = function () {
    return function (model) {
        if (model.Status == 1)
            return "Yeni";
        if (model.Status == 2)
            return "İşlemde";
        if (model.Status == 3)
            return "Atandı";
        if (model.Status == 4)
            return "Ü.O Bekliyor";
        if (model.Status == 5)
            return "Kapatıldı";
        if (model.Status == 6)
            return "Tamamlandı";
    };
};

utils.visitStatus = function () {
    return function (model) {
        if (model.Status == 1)
            return "Yeni";
        if (model.Status == 2)
            return "İşlemde";
        if (model.Status == 3)
            return "Bekliyor";
        if (model.Status == 4)
            return "Tamamlandı";
        if (model.Status == 5)
            return "İptal";
    };
};

utils.stockMovementAction = function () {
    return function (model) {
        if (model.Action == 1)
            return "GİRİŞ";
        if (model.Action == 2)
            return "ÇIKIŞ";
        if (model.Action == 3)
            return "ZİMMET";
        if (model.Action == 4)
            return "İADE";
    };
};


Handlebars.registerHelper('ServicePrinterStatus', function (status) {
    if (status == 2) {
        return "İşlemde"
    }
    if (status == 6) {
        return "Tamamlandı"
    }
    return 
})

Handlebars.registerHelper('ServicePrinterStatusClass', function (status) {
    if (status == 2) {
        return "bg-info"
    }
    if (status == 6) {
        return "bg-success"
    }
    return
})

Handlebars.registerHelper('ServicePrinterMaintenance', function (maintenance) {
    if (maintenance == true) {
        return "Yapıldı"
    }
    else
        return "Yapılmadı"
})