/*
    Alert confirm Yes|No or OK
    titleMgs        : title of message box
    contentMsg      : content of alert
    fuctionAction   : action of function
    typeShow        : true: yes|No , false : ok

*/
function ConfirmMessageBox(titleMgs, contentMsg, fuctionAction, typeShow) {

    var dialog = new CRMDialog("modal-mask");
    var kendoWindow = dialog.showDialog(titleMgs == null ? $("#confirmDialogTitleDefault").val() : titleMgs, $("#confirmationPublic").html());
    if (typeShow)
        $('#modal .box-Confirm').show();
    else
        $('#modal .box-Success').show();

    $('#modal h4 span').append(contentMsg == null ? $("#confirmDialogContentDefault").val() : contentMsg);
    kendoWindow
        .find(".btnConfirm, .btnCancel, .btnSuccess, .btnCancelError")
            .click(function () {
                if ($(this).hasClass("btnConfirm")) {
                    fuctionAction();
                }
                else if ($(this).hasClass("btnSuccess")) {
                    kendoWindow.data("kendoWindow").close();
                    fuctionAction();
                }
                else if ($(this).hasClass("btnCancel") || $(this).hasClass("btnCancelError")) {
                    kendoWindow.data("kendoWindow").close();
                }
            })
        .end();
}
/* show message error*/
/*
    bool: true : delete success, false : false
*/
function MessageBox(msg, bool) {
    $('#modal h4 span').remove();
    if (bool)
        $('#modal h4').append("<span>" + msg + "</span>");
    else
        $('#modal h4').append("<span class=\"red\">" + msg + "</span>");

    $(".box-Error").show();
    $(".box-Confirm").hide();
    return false;
}

/*set active all left menu*/
$(document).ready(function () {
    $('ul#sidebar-menu li a').each(function () {
        $(this).removeClass("active");

        var lst = $(this).attr('href').toLowerCase().split("/");

        if (lst != null) {
            var a = lst[1] + '/' + lst[2] + lst[3];
            if (document.URL.toLowerCase().indexOf(a) != -1) {
                $(this).addClass('active');// son
                $(this).parent().parent().addClass("collapse in"); //parent ul
                $(this).parent().parent().parent().addClass('active'); // parent ul li
            }
        }
    });
});

function checkImageUpload(input) {
    var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png"];
    for (var j = 0; j < _validFileExtensions.length; j++) {
        var sCurExtension = _validFileExtensions[j];
        if (input.extension.toLowerCase() == sCurExtension.toLowerCase()) {
            return true;
        }
    }
    return false;
}


// Alias helper
function getObjectById(id) {
    if (document.getElementById)
        var returnVar = document.getElementById(id);
    else if (document.all)
        var returnVar = document.all[id];
    else if (document.layers)
        var returnVar = document.layers[id];
    return returnVar;
}

//This method check the inputText is meet the alias requirement or not
function IsAliasFormat(inputText) {
    return inputText.match(/^[a-zA-Z0-9][a-zA-Z0-9_-]+$/);
}

//This method generate an alias from an input text, return empty string if no valid character presents.
function GenerateAlias(inputText) {
    return GenerateAlias(inputText, 250);
}

function GenerateAlias(inputText, maxLength) {
    //lower chars
    inputText = inputText.replace(/æ|å|ä|à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    inputText = inputText.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    inputText = inputText.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    inputText = inputText.replace(/ø|ö|ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    inputText = inputText.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    inputText = inputText.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    inputText = inputText.replace(/đ|ð/g, "d");
    inputText = inputText.replace(/ñ/g, "n");

    //upper chars
    inputText = inputText.replace(/Æ|Å|Ä|À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
    inputText = inputText.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
    inputText = inputText.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
    inputText = inputText.replace(/Ø|Ö|Ó|Ò|Ỏ|Õ|Ọ|Ô|Ố|Ồ|Ổ|Ỗ|Ộ|Ơ|Ớ|Ờ|Ở|Ỡ|Ợ/g, "O");
    inputText = inputText.replace(/Ú|Ù|Ủ|Ũ|Ụ|Ư|Ứ|Ừ|Ử|Ữ|Ự/g, "U");
    inputText = inputText.replace(/Ý|Ỳ|Ỷ|Ỹ|Ỵ/g, "Y");
    inputText = inputText.replace(/Đ|Ð/g, "D");
    inputText = inputText.replace(/Ñ/g, "N");

    inputText = inputText.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'| |\"|\&|\#|\[|\]|~|$|_/g, " ");

    // Remove invalid character
    var result = inputText.replace(/(^[^a-zA-Z0-9]+)|([^a-zA-Z0-9\-_\s])/g, "");
    //Replace all space with _
    result = result.replace(/\s+[\-_]*/g, "-");
    result = result.replace(/-+-/g, "-"); //thay thế 2- thành 1-
    result = result.replace(/^\-+|\-+$/g, "");
    // trim space, - and _ of strim
    result = result.replace(/^[\s\-_]+|[\s\-_]+$/g, "");

    return result.substring(0, maxLength).toLowerCase();
}

function AutoGenerateAlias(inputText, targetObjectId, isOverwrite) {
    var aliasTextbox = getObjectById(targetObjectId);

    if (aliasTextbox == null || aliasTextbox == undefined)
        return;

    // the target text does not allow to change value
    if (aliasTextbox.disabled == true || aliasTextbox.attributes["readonly"] != undefined)
        return;

    var currentAliasText = aliasTextbox.value.replace(/^[\s\-_]+|[\s\-_]+$/g, "");

    if (currentAliasText.length > 0 && !isOverwrite)
        return; //Do not allow overwrite on current alias

    currentAliasText = GenerateAlias(inputText);

    if (currentAliasText.length > 0)
        aliasTextbox.value = currentAliasText;
}


function getInputVal(node) {
    if (isNodeCKEditor(node)) {
        var ckNode = CKEDITOR.instances[node.attr('id')];
        if (ckNode) {
            return $.trim(ckNode.getData());
        }
    }

    if (node.attr('type') == 'radio') {
        var radio = $("input:radio[name='" + node.attr('name') + "']:checked");
        //console.log('get:' + node.attr('id') + ':' + node.attr('name') + ':' + radio.val());
        return $.trim(radio.val());
    }

    return $.trim(node.val());
}

function translateAlias(srcNode, dstId, override) {
    var dstNode = $('#' + dstId);

    if (srcNode.value.length > 0)
        dstNode.removeAttr("readonly");

    var dstOVal = getInputVal(dstNode);
    var dstBKVal = dstNode.attr('bak-value');

    if (!override || !dstBKVal || dstBKVal == dstOVal || dstOVal == '') {
        AutoGenerateAlias($(srcNode).val(), dstId, override);
        dstNode.attr('bak-value', dstNode.val());

        dstNode.change();
        dstNode.valid();
    }
}

function isNodeCKEditor(node) {
    return window.CKEDITOR && CKEDITOR.instances &&
    (
        node.hasClass('ckeditor_standard') || node.hasClass('ckeditor_simple')
    );
}

function initImagePreview() {
    var xOffset = -20, yOffset = 40, wSpace = 30, hSpace = 40;

    $("a.preview").hover(function (e) {
        this.t = this.title;
        this.title = "";
        var c = (this.t != "") ? "<br/>" + this.t : "";
        $("body").append("<p id='imgPreview'><img src='" + this.href + "' alt='Image preview' />" + c + "</p>");
        var topP = e.pageY - xOffset, leftP = e.pageX + yOffset;

        var previewB = $("#imgPreview");
        var bottomP = topP + previewB.height() - $(window).scrollTop() + wSpace;
        if (window.innerHeight >= previewB.height() + wSpace) {
            var remainHeight = bottomP - window.innerHeight;
            if (remainHeight > 0) topP -= remainHeight;
        }

        var rightP = leftP + previewB.width() - $(window).scrollLeft() + hSpace;
        if (window.innerWidth >= previewB.width() + hSpace) {
            var remain = rightP - window.innerWidth;
            if (remain > 0) leftP = e.pageX - yOffset - previewB.width();
        }

        $("#imgPreview").css("top", topP + "px").css("left", leftP + "px").fadeIn("slow");
    },
        //handlerOut (leave)
        function () {
            this.title = this.t;
            $("#imgPreview").remove();
        }
    );

    $("a.preview").mousemove(function (e) {
        var topP = e.pageY - xOffset, leftP = e.pageX + yOffset;
        var previewB = $("#imgPreview");
        var bottomP = topP + previewB.height() - $(window).scrollTop() + wSpace;
        if (window.innerHeight >= previewB.height() + wSpace) {
            var remainHeight = bottomP - window.innerHeight;
            if (remainHeight > 0) topP -= remainHeight;
        }

        var rightP = leftP + previewB.width() - $(window).scrollLeft() + hSpace;
        if (window.innerWidth >= previewB.width() + hSpace) {
            var remain = rightP - window.innerWidth;
            if (remain > 0) leftP = e.pageX - yOffset - previewB.width();
        }

        $("#imgPreview").css("top", topP + "px").css("left", leftP + "px");
    });

    //$(".preview").fancybox({ width: 800, height: 600, autoScale: false, fitToView: true });
};