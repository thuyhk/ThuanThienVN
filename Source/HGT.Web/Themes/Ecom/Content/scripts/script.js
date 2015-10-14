
$(document).ready(function(){
	$(function () {
		$.scrollUp({
	        scrollName: 'scrollUp', // Element ID
	        scrollDistance: 300, // Distance from top/bottom before showing element (px)
	        scrollFrom: 'top', // 'top' or 'bottom'
	        scrollSpeed: 300, // Speed back to top (ms)
	        easingType: 'linear', // Scroll to top easing (see http://easings.net/)
	        animation: 'fade', // Fade, slide, none
	        animationSpeed: 200, // Animation in speed (ms)
	        scrollTrigger: false, // Set a custom triggering element. Can be an HTML string or jQuery object
					//scrollTarget: false, // Set a custom target element for scrolling to the top
	        scrollText: '<i class="fa fa-angle-up"></i>', // Text for element, can contain HTML
	        scrollTitle: false, // Set a custom <a> title if required.
	        scrollImg: false, // Set true to use image
	        activeOverlay: false, // Set CSS color to display scrollUp active point, e.g '#00FFFF'
	        zIndex: 2147483647 // Z-Index for the overlay
		});
	});
});

//Category
$(document).ready(function () {
    var path = window.location.pathname.toLowerCase();
    //path = path.replace(/\/$/, "");
   // path = decodeURIComponent(path);

    $(".sf-menu-phone li a, .navbar-nav li a, .sub-menu li a").each(function () {
        var href = $(this).attr('href');
        if (href)
            href = href.toLowerCase();

        if (path == href) {
            $(this).closest('li').addClass('active');
        }
    });

    jQuery("#menu-icon").on("click", function () {
        jQuery(".sf-menu-phone").slideToggle();
        jQuery(this).toggleClass("active");
    });

    jQuery(".sf-menu-phone li").each(function () {
        var jQuerysubmeun = jQuery(this).find('ul:first');
        if (jQuerysubmeun.length > 0) {
            jQuery(this).addClass("parent").append('<i class="icon-angle-down"></i>');

        }
    });

    //jQuery('.sf-menu-phone').find('li.parent').append('<i class="icon-angle-down"></i>');

    jQuery('.sf-menu-phone li.parent i').on("click", function () {
        if (jQuery(this).hasClass('icon-angle-up')) { jQuery(this).removeClass('icon-angle-up').parent('li.parent').find('> ul').slideToggle(); }
        else {
            jQuery(this).addClass('icon-angle-up').parent('li.parent').find('> ul').slideToggle();
        }
    });

    var node2 = jQuery('.sf-menu-phone').find('li.active').closest('.parent');
    if (node2.length > 0) {
        //node2.addClass("active");
        node2.children('i').addClass('icon-angle-up');
        node2.find('> ul').slideToggle();
    }
     var node1 = node2.closest('ul').closest('.parent')
    //var node1 = node2.parent();
    if (node1.length > 0) {
        //node1.parent().addClass("active");
        node1.find('i.icon-angle-down').addClass('icon-angle-up');
        node1.find('> ul').slideToggle();
    }    
});

$(window).resize(function () {
    if ($(window).width() >= 767) {
        $(".sf-menu-phone, .left-sidebar h2").removeAttr('style');
    }
    else {
        $(".left-sidebar h2").hide();
    }
});

$(document).ready(function () {
    if (history.pushState != undefined) {
        //$('.productsList .content_products').responsiveEqualHeightGrid();

        var para = getUrlParameter('p');
        if (para == 1 || para == undefined) {
            $('.pager a.first').addClass('selected');
        }
    }
});

function getUrlParameter(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) {
            return sParameterName[1];
        }
    }
}

function formatNumber(num) {
    return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
}

function quickCart(id) {
    $("#divLoading").show();
    if (id != '') {
        $.post("/ShoppingCart/AddToQuickCart", { "id": id },
        function (data) {
            window.location.href = "/gio-hang";
        });
    }
}

function addToQuickCart(id) {
    $("html, body").animate({ scrollTop: 0 }, "slow");
    $("#divLoading").show();
    $('#myModal').modal('show');
    if (id != '') {
        $.post("/ShoppingCart/AddToQuickCart", { "id": id },
        function (data) {
            var totalItemsCart = $(data).filter('#countItemsCart').val();
            $('#cart-status').text("(" + totalItemsCart + ")");
            $('#myModal').html(data);
            $("#divLoading").hide();
            return false;
        });
    }
}

function showMinicart() {
    $("#divLoading").show();
    $('#myModal').modal('show');
    $.post("/ShoppingCart/_ShowQuickCart", function (data) {
        $('#myModal').html(data);
        $("#divLoading").hide();
        return false;
    });
}