// Search
function validateInput(evt) {
    var e = event || evt;
    var charCode = e.which;
    if ((charCode > 32 && (charCode < 48)) || (charCode > 57 && charCode < 65))
        return false;
    return true
}

function clickSearch() {
    var txtInput = $("#keyWord").val();
    if (txtInput == 'Tìm kiếm sản phẩm' || txtInput == 'Search product...') {
        alert("Nhập từ khóa");
        $("#keyWord").focus();
    }
    else {
        //window.location.href = "/Search/SearchByKey?keyword=" + txtInput;
    }
}
   
$('#keyWord').focus(function () {
    var $this = $(this);
    var title = $this.attr('title');
    if ($this.val() == title) {
        $this.val('');
    }
}).blur(function () {
    var $this = $(this);
    var title = $this.attr('title');
    if ($this.val() == '') {
        $this.val(title);
    }
});
/*
var dropmenu=$("div.dropmenu");			
var overlay=$("div.overlay");
var menuWidth = $(".nav-menu").width()-20+'px';
var posNav = $(".nav-menu").offset();


$(".abc").hover(function () {
    removeCurrentMenu();
    $(this).addClass('active');
    pos = dropmenu.offset();
    if (pos.top <= 0) {
        showOverLay(200);
        dropmenu.css({ top: posNav.top - 21, display: "block", width: menuWidth }).delay(100).animate({ "top": posNav.top + 1 }, 800, "swing");
    } else {
        dropmenu.clearQueue();
        dropmenu.stop();
        overlay.clearQueue();
        overlay.stop();
        showOverLay(200);
        dropmenu.css({ top: posNav.top + 1, display: "block" });
    }
}, function () {
    $(this).removeClass('active');
    addCurrentMenu()
    overlay.clearQueue();
    overlay.stop();
    dropmenu.clearQueue();
    dropmenu.stop();
    dropmenu.delay(200).animate({ "top": "-329px" }, { queue: true, duration: 500 }, "swing");
    hideOverLay(100);
});

dropmenu.hover(function () {
    removeCurrentMenu();
    $('.nav-menu li.abc').addClass('active');
    pos = dropmenu.offset();
    if (pos.top > -100) {
        dropmenu.clearQueue();
        dropmenu.stop();
        overlay.clearQueue();
        overlay.stop();
        $(this).animate({ "top": posNav.top + 1 }, { queue: true, duration: 300 }, "swing");
        showOverLay(200);
    }
}, function () {
    $('.nav-menu li.abc').removeClass('active');
    addCurrentMenu();
    pos = dropmenu.offset();
    if (pos.top > 0) {
        $(this).delay(200).animate({ "top": "-329px" }, { queue: true, duration: 500 }, "swing");
        hideOverLay(1000);
    }
});

function showOverLay(second){			
	overlay.stop().delay(second).show(0);
}

function hideOverLay(second){					
	overlay.delay(second).hide(0);
}	

// MENU
jQuery(document).ready(function () {
    //addCurrentMenu();
});

function addCurrentMenu(){  
    jQuery(".nav-menu li").each(function () {
        var jQuerysubmeun = jQuery(this).find('ul:first');
        jQuery(this).hover(function () {
            jQuerysubmeun.stop().css({ overflow: "hidden", height: "auto", display: "none", paddingTop: 0 }).slideDown(300,
            function () {
                jQuery(this).css({ overflow: "visible", height: "auto" });
            });
        }, function () {
            jQuerysubmeun.stop().slideUp(300, function () {
                jQuery(this).css({ overflow: "hidden", display: "none" });
            });
        });
    });

    var pathname = window.location.pathname;
    var pathname = pathname.split('/');
    var urlcurrent = pathname[pathname.length - 1];

   
        jQuery(".nav-menu li a").each(function () {
            var test = jQuery(this).attr('href');
            if (test == urlcurrent) {
                var t = (this).parentNode;
                jQuery(t).addClass('active');
                return;
            }
       });   
}

function removeCurrentMenu(){

    var pathname = window.location.pathname;
    var pathname = pathname.split('/');
    var urlcurrent = pathname[pathname.length - 1];

        jQuery(".nav-menu li a").each(function () {
            var test = jQuery(this).attr('href');
            if (test == urlcurrent) {
                var t = (this).parentNode;
                jQuery(t).removeClass('active');
                return;
            }
        });    
}

*/