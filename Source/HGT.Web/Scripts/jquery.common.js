$(document).ready(function () {
    //new WOW().init();

    var path = window.location.pathname.toLowerCase();

    $(".navbar-nav li a, .left-sidebar li a").each(function (i, e) {
        var href = $(e).attr('href');
        if (href)
            href = href.toLowerCase();
        if (path == href) {
            $(e).closest('li').addClass('active');
        }
    });

    $("#icon-left-slidebar").on("click", function () {
        jQuery(".left-sidebar").slideToggle();
        jQuery(this).toggleClass("active");
    });

    //$(".left-sidebar li").each(function () {
    //    var jQuerysubmeun = jQuery(this).find('ul:first');
    //    if (jQuerysubmeun.length > 0) {
    //        $(this).addClass("parent").append('<i class="icon-angle-down"></i>');

    //    }
    //});

    $('.left-sidebar').find('li.parent').append('<i class="icon-angle-down"></i>');
    $('.left-sidebar li.parent i').on("click", function () {
        if (jQuery(this).hasClass('icon-angle-up')) { 
            $(this).removeClass('icon-angle-up').parent('li.parent').find('> ul').slideToggle(); 
        }
        else {
            jQuery(this).addClass('icon-angle-up').parent('li.parent').find('> ul').slideToggle();
        }
    });

    var node2 = jQuery('.left-sidebar').find('li.active').closest('.parent');
    if (node2.length > 0) {
        node2.children('i').addClass('icon-angle-up');
        node2.find('> ul').slideToggle();
    }
    //$('#services .single-items p').responsiveEqualHeightGrid();

    var para = getUrlParameter('p');
    if (para == 1 || para == undefined) {
        $('.pager a.first').addClass('selected');
    }
    
});

$(window).resize(function () {
    if ($(window).width() <= 767) {
       //if($('.icon'))
        //$( ".flex-container" ).appendTo( $( ".banner" ) );
    }
    if ($(window).width() > 767) {
        $('#icon-left-slidebar').removeClass('active');
        $('.left-sidebar').removeAttr('style');
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