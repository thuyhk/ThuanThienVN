function validFileExtension(node, attrRegex, errSelector, attrErrMsg) {
    var errMsg = node.closest(errSelector).find('.file-ext-error');
    var regex = new RegExp(node.attr(attrRegex), "ig");

    if (!regex.test(node.val())) {
        errMsg.html(node.attr(attrErrMsg) + ' (' + node.val() + ')');
        errMsg.show(); return false;
    }
    else errMsg.hide();

    return true;
}


//function uploadImage(node) {
//    //check valid file extension
//    if (!validFileExtension($(node), 'datavalregex', '.form-group', 'datavalextension'))
//        ConfirmMessageBox(null, "Please insert an image file...", function () { }, false);

//    var waitImg = $('#LoadingImage');
//    if (waitImg.is(':visible'))
//        return;
//    else
//        waitImg.show();

//    $.ajaxFileUpload({
//        url: UPLOADURL,
//        dataType: "json",
//        data: { id: ID },
//        contentType: "application/json; charset=utf-8",
//        fileElementId: "ImageUpload",
//        success: function (result) {
//            if (result.Status == 1) {
//                $("#Thumbnail").val(result.FileName);
//                $("#ThumbnailPath").attr('src', result.ImagePath).show();
//            }
//            else if (result.Message != null)
//                ConfirmMessageBox(null, result.Message, function () { }, false);
//        },
//        error: function (data, status, e) {
//            //admincommon.showMessageDefault(/(StringTable.AjaxCommandErrorMessage)', 'ActionResultStatus.Fail)');
//        },
//        complete: function (data, status, e) {
//            waitImg.hide();
//        }
//    });
//}

function doUploadFile(postUrl, uploadId, waitImgId, args, onResult) {

    var waitImg = $('#' + waitImgId);
    if (waitImg.is(':visible')) return;
    else waitImg.show();

    $.ajaxFileUpload({
        url: postUrl,
        data: args,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        fileElementId: uploadId,
        success: function (result) {
            if (result.Status == 1) {
                if (onResult)
                    onResult(result);
            }
            else if (result.Message != null)
                ConfirmMessageBox(null, result.Message, function () { }, false);
        },
        error: function (data, status, e) {
        },
        complete: function (data, status, e) {
            waitImg.hide();
        }
    });
};