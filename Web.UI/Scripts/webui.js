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

function showLoader() {
    var loader = $("#loader");
    if (loader.is(":visible")) { //gizliyse göster & gizli değilse gizle
        loader.hide();
    }
    else {
        loader.show();
    }
}

$(".menu-item").click(function () {
    var menu = $("#menuButton");
    if (menu.is(':visible'))
        $("#menuButton").click();
});