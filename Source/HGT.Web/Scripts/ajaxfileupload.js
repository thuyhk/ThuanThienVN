
jQuery.extend({
    createUploadIframe: function(id, uri) {
        //create frame
        var frameId = 'jUploadFrame' + id;
        var iframeHtml = '<iframe id="' + frameId + '" name="' + frameId + '" style="position:absolute; top:-9999px; left:-9999px"';
        if (window.ActiveXObject) {
            if (typeof uri == 'boolean') {
                iframeHtml += ' src="' + 'javascript:false' + '"';

            } else if (typeof uri == 'string') {
                iframeHtml += ' src="' + uri + '"';

            }
        }
        iframeHtml += ' />';
        jQuery(iframeHtml).appendTo(document.body);

        return jQuery('#' + frameId).get(0);
    },
    createUploadForm: function(id, fileElementId, data, datasubmit) {
        //create form	
        var formId = 'jUploadForm' + id;
        var fileId = 'jUploadFile' + id;
        var form = jQuery('<form  action="" method="POST" name="' + formId + '" id="' + formId + '" enctype="multipart/form-data"></form>');
        if (data) {
            for (var i in data) {
                jQuery('<input type="hidden" name="' + i + '" value="' + data[i] + '" />').appendTo(form);
            }
        }
        if (datasubmit) {
            for (var j in datasubmit) {
                jQuery('<input type="hidden" name="' + j + '" value="' + datasubmit[j] + '" />').appendTo(form);
            }
        }
        var oldElement = jQuery('#' + fileElementId);
        var newElement = jQuery(oldElement).clone();
        jQuery(oldElement).attr('id', fileId);
        jQuery(oldElement).before(newElement);
        jQuery(oldElement).appendTo(form);


        //set attributes
        jQuery(form).css('position', 'absolute');
        jQuery(form).css('top', '-1200px');
        jQuery(form).css('left', '-1200px');
        jQuery(form).appendTo('body');
        return form;
    },

    ajaxFileUpload: function(s, dataSubmit) {
        // TODO introduce global settings, allowing the client to modify them for all requests, not only timeout		
        s = jQuery.extend({}, jQuery.ajaxSettings, s);
        var id = new Date().getTime()
        var form = jQuery.createUploadForm(id, s.fileElementId, (typeof(s.data) == 'undefined' ? false : s.data), (typeof(dataSubmit) == 'undefined' ? false : dataSubmit));
        var io = jQuery.createUploadIframe(id, s.secureuri);
        var frameId = 'jUploadFrame' + id;
        var formId = 'jUploadForm' + id;
        // Watch for a new set of requests
        if (s.global && !jQuery.active++) {
            jQuery.event.trigger("ajaxStart");
        }
        var requestDone = false;
        // Create the request object
        var xml = {}
        if (s.global)
            jQuery.event.trigger("ajaxSend", [xml, s]);
        // Wait for a response to come back
        var uploadCallback = function(isTimeout) {
            var io = document.getElementById(frameId);
            try {
                if (io.contentWindow) {
                    xml.responseText = io.contentWindow.document.body ? io.contentWindow.document.body.innerHTML : null;
                    xml.responseJson = io.contentWindow.document.body ? (io.contentWindow.document.body.innerText? io.contentWindow.document.body.innerText: io.contentWindow.document.body.textContent):null;
                    xml.responseXML = io.contentWindow.document.XMLDocument ? io.contentWindow.document.XMLDocument : io.contentWindow.document;

                } else if (io.contentDocument) {
                    xml.responseText = io.contentDocument.document.body ? io.contentDocument.document.body.innerHTML : null;
                    xml.responseJson = io.contentDocument.document.body ? (io.contentDocument.document.body.innerText ? io.contentDocument.document.body.innerText : io.contentDocument.document.body.textContent) : null;
                    xml.responseXML = io.contentDocument.document.XMLDocument ? io.contentDocument.document.XMLDocument : io.contentDocument.document;
                }
            } catch(e) {
                jQuery.handleError(s, xml, null, e);
            }
            if (xml || isTimeout == "timeout") {
                requestDone = true;
                var status;
                try {
                    status = isTimeout != "timeout" ? "success" : "error";
                    // Make sure that the request was successful or notmodified
                    if (status != "error") {
                        // process the data (runs the xml through httpData regardless of callback)
                        var data = jQuery.uploadHttpData(xml, s.dataType);
                        // If a local callback was specified, fire it and pass it the data
                        if (s.success)
                            s.success(data, status);

                        // Fire the global callback
                        if (s.global)
                            jQuery.event.trigger("ajaxSuccess", [xml, s]);
                    } else
                        jQuery.handleError(s, xml, status);
                } catch(ex) {
                    status = "error";
                    //console.log(ex.toString()); 
                }

                // The request was completed
                if (s.global)
                    jQuery.event.trigger("ajaxComplete", [xml, s]);

                // Handle the global AJAX counter
                if (s.global && !--jQuery.active)
                    jQuery.event.trigger("ajaxStop");

                // Process result
                if (s.complete)
                    s.complete(xml, status);

                jQuery(io).unbind();

                setTimeout(function() {
                    try {
                        jQuery(io).remove();
                        jQuery(form).remove();

                    } catch(e) {
                        jQuery.handleError(s, xml, null, e);
                    }

                }, 100);

                xml = null;

            }
        }
        // Timeout checker
        if (s.timeout > 0) {
            setTimeout(function() {
                // Check to see if the request is still happening
                if (!requestDone) uploadCallback("timeout");
            }, s.timeout);
        }
        try {

            var form = jQuery('#' + formId);
            jQuery(form).attr('action', s.url);
            jQuery(form).attr('method', 'POST');
            jQuery(form).attr('target', frameId);
            if (form.encoding) {
                jQuery(form).attr('encoding', 'multipart/form-data');
            } else {
                jQuery(form).attr('enctype', 'multipart/form-data');
            }
            jQuery(form).submit();

        } catch(e) {
            jQuery.handleError(s, xml, null, e);
        }

        jQuery('#' + frameId).load(uploadCallback);
        return {
            abort: function() {
            }
        };

    },

    uploadHttpData: function(r, type) {
        var data = !type;
        data = type == "xml" || data ? r.responseXML : r.responseText;
        // If the type is "script", eval it in global context
        if (type == "script")
            jQuery.globalEval(data);
        // Get the JavaScript object, if JSON is used.
        if (type == "json")
            if (r.responseJson == null) {
                data = r.responseText;
                jQuery.globalEval("data = " + r.responseText);
            } else {
                eval("data = " + r.responseJson);
            }
           
        // evaluate scripts within html
        if (type == "html")
            jQuery("<div>").html(data).evalScripts();

        return data;
    }
});

var uploadProcessing = false;
function InitUploadControl(fileInputId, hiddenFieldName, formId) {
    var fileInput = "";
    try {
        fileInput = $('#' + fileInputId).attr('name');
    } catch(ex) {
        fileInput = "";
    }
   
    $("#" + fileInputId).kendoUpload({
        multiple: false,
        async: {
            saveUrl: "../modules/contacts/FileUpload/Index/",
            removeUrl: "../modules/contacts/FileUpload/remove",
            autoUpload: true,
            
        },
        progress: function () {
           
        },
        remove:function() {
            $("input[name='" + hiddenFieldName + "']", formId).val("");
        },
        localization: { select: 'Select File' },
        error: onError,
        success: onSuccess,
        select: onSelect,
        complete: onComplete
    });
    //$("#" + fileInputId).parent().css("style","width:100%");
    function onComplete(e) {
        
        e.preventDefault();
    }
    function onError(e) {
        if (e.files!= undefined && e.files.length >= 1) {
            if ($(".k-button k-upload-button").find("li").length >= 2) {
                $(".k-upload-files.k-reset").find("li").last().remove();
            }
        }
        e.preventDefault();
    }
    function onSelect(e) {
        if (e.files.length > 1) {
            alert("Please select max 1 file.");
            e.preventDefault();
        }
    }
    function onSuccess(e) {
        if ((e.response.Status == 1 || e.response.Status == 2) && e.response.Message != null) {
            if (e.files != undefined && e.files.length >= 1) {
                $(".k-upload-files.k-reset").find("li").last().remove();
            }
            alert(e.response.Message);
        }
        if (e.response.Status == 0) {
            if (e.files != undefined && e.files.length >= 1) {
                if ($(".k-button k-upload-button").find("li").length >= 2) {
                    $(".k-upload-files.k-reset").find("li").first().remove();
                }
                $("#" + fileInputId).parent().find("input[name='" + hiddenFieldName + "']").first().remove();
            }
            $("input[name='" + hiddenFieldName + "']", "#"+formId).val(typeof(e.response.FileName) == 'undefined' ? "" : e.response.FileName);
        }
        e.preventDefault();
    }
}
function UploadFile(hiddenFieldName, formId, desturl) {
    
    if (uploadProcessing == true) {
        return;
    }
    if (formId.indexOf("#") == -1) {
        formId = "#" + formId;
    }
    uploadProcessing = true;
    var UPLOADURL = desturl;
  
    var completeImage = $('#LoadingCompleteImage' + formId.replace("#", ""), formId);
    var deleteImage = $('#DeleteImage' + formId.replace("#", ""), formId);
    $('#LoadingImage' + formId.replace("#", ""), formId).css('display', 'block');
    
    if (deleteImage != undefined) {
        $('#DeleteImage' + formId.replace("#", ""), formId).click(function() {
            $("input[name='" + hiddenFieldName + "']", formId).val("");
            if ($(completeImage) != null) {
                $(completeImage).css('display', '');
            }
            if ($(deleteImage) != null) {
                $(deleteImage).css('display', '');
            }
        });
    }
   
    var datasubmit = {id:0};
    $.ajaxFileUpload({
        url: "../"+ UPLOADURL,
        contentType: "application/json; charset=utf-8",
        fileElementId: hiddenFieldName.replace("$", "").replace("$", ""),
        dataType: "json",
        success: function (data) {
            var result = data;
            uploadProcessing = false;
            $('#LoadingImage' + formId.replace("#", ""), formId).css('display', 'none');
            
            if ((result.Status == 1 || result.Status == 2) && result.Message != null) {
                alert(result.Message);
            }
            if (result.Status == 0) {
                $("input[name='" + hiddenFieldName + "']", formId).val(typeof (result.FileName) == 'undefined' ? "" : result.FileName);
                if ($(completeImage) != null) {
                    $(completeImage).css('display', '');
                }
                if ($(deleteImage) != null) {
                    $(deleteImage).css('display', '');
                }
            }
        },
        error: function (data, status, e) {
            alert(e);
        }
    }, datasubmit);
};

