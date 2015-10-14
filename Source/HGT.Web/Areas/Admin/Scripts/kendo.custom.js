
$(document).ready(function () {

});

function CRMPopupModal(maskSelector, cssClass) {
    var modal = null;
    var mask = null;
    var maskElement = maskSelector;
    var destroy = false;
    if ($(maskSelector).length > 0) {
        mask = $(maskSelector);
        if (!mask.hasClass(cssClass)) {
            mask.addClass(cssClass);
        }
    }
    if (mask == null) {
        //clog("null mask");
        createMask();
    }
    showMask();

    function createMask() {
        mask = $("div#claw-modal-mask-window");
        if (mask.length == 0) {
            var newdiv = document.createElement("div");
            newdiv.setAttribute("id", "claw-modal-mask-window");
            $("body").append(newdiv);
            mask = $(newdiv);
            mask.attr("style", "position: fixed; top: 0; left: 0; height: 100%; width: 100%; display: none;z-index: 9990;background-color:#390F3D;opacity:0.6;");
        }
        if (cssClass != null) {
            mask.addClass(cssClass);
        }
    }
    function showMask() {
        //var mask = $("#mask-loading");
        if (mask != null) {
            mask.removeAttr("style");
            mask.attr("style", "position: fixed; top: 0; left: 0; height: 100%; width: 100%; display: none;z-index: 9990;background-color:#390F3D;opacity:0.6;");
            mask.show();
        };
    }
    function hideMask() {
        if (mask != null) {
            mask.hide();
            //mask.removeAttr("class");
        };
    }
    function modalMask() {
        //var mask = $("#mask-loading");
        if (mask != null) {
            //clog("modalMask");
            mask.css("background-image", "none");
            if (mask.is(":visible") == false) {
                mask.show();
            }
        };
    }

    CRMPopupModal.prototype.showModal = function (windowElementSelector, title, content, width, height, callbackFunc) {
        destroy = false;
        if (windowElementSelector == null || $(windowElementSelector).length == 0) {
            windowElementSelector = "<div />";
        }
        if (width == null) {
            width = 710;
        }
        if (height == null) {
            height = 510;
        }
        if (title == null) {
            title = "";
        }
        modal = $(windowElementSelector).kendoWindow({
            width: width,
            height: height,
            modal: true,
            title: title,
            //actions: ["Minimize", "Maximize", "Close"],
            actions: ["Pin", "Close"],
            visible: false,
            activate: onActivate,
            open: onOpen,
            //close: onClose,
            deactivate: onDeactivate,
            refresh: onRefresh,
            animation: { open: { effects: getEffects(), duration: 500 }, close: { effects: getEffects(), reverse: true } },
            //resizable: false,
            resize: onResize,
            dragstart: onDragStart,
            dragend: onDragEnd
        });

        modal.data("kendoWindow").center().open().content(content);
        modal.data("kendoWindow").pin();
        if (callbackFunc != null) {
            callbackFunc(window);
        }
        return modal;
    }

    CRMPopupModal.prototype.showIframeModal = function (windowElementSelector, title, contentUrl, width, height, callbackFunc) {
        destroy = false;
        if (windowElementSelector == null || $(windowElementSelector).length == 0) {
            windowElementSelector = "<div />";
        }
        if (width == null) {
            width = 710;
        }
        if (height == null) {
            height = 510;
        }
        if (title == null) {
            title = "";
        }
        modal = $(windowElementSelector).kendoWindow({
            width: width,
            height: height,
            modal: true,
            title: title,
            iframe: true,
            content: contentUrl,
            //actions: ["Minimize", "Maximize", "Close"],
            actions: ["Pin", "Close"],
            visible: false,
            activate: onActivate,
            open: onOpen,
            //close: onClose,
            deactivate: onDeactivate,
            refresh: onRefresh,
            animation: { open: { effects: getEffects(), duration: 500 }, close: { effects: getEffects(), reverse: true } },
            //resizable: false,
            resize: onResize,
            dragstart: onDragStart,
            dragend: onDragEnd
        });
        modal.data("kendoWindow").refresh();
        modal.data("kendoWindow").center().open();//.content(contentUrl);
        modal.data("kendoWindow").pin();
        if (callbackFunc != null) {
            callbackFunc(window);
        }
        return modal;
    }

    CRMPopupModal.prototype.showDialog = function (title, content, width, height, callbackFunc) {
        destroy = false;
        if (width == null) {
            width = 350;
        }
        if (height == null) {
            height = 150;
        }
        if (title == null) {
            title = "Message";
        }
        var window = $("<div />").kendoWindow({
            width: width,
            height: height,
            modal: true,
            title: title,
            actions: ["Minimize", "Close"],
            visible: false,
            activate: onActivate,
            //open: onOpen,
            //close: onClose,
            deactivate: onDeactivate,
            //refresh: onRefresh,
            //animation: { open: { effects: getEffects(), duration: 500 }, close: { effects: getEffects(), reverse: true } },
            resize: false,
            //dragstart: onDragStart,
            //dragend: onDragEnd
        });

        window.data("kendoWindow").center().open().content(content);
        window.data("kendoWindow").pin();

        if (callbackFunc != null) {
            callbackFunc(window);
        }
        return window;
    }
    CRMPopupModal.prototype.showIframeDialog = function (title, contentUrl, width, height, callbackFunc) {
        destroy = false;
        if (width == null) {
            width = 350;
        }
        if (height == null) {
            height = 150;
        }
        if (title == null) {
            title = "Message";
        }
        var window = $("<div />").kendoWindow({
            width: width,
            height: height,
            modal: true,
            //iframe: true,
            title: title,
            content: contentUrl,
            actions: ["Minimize", "Close"],
            visible: false,
            activate: onActivate,
            //open: onOpen,
            //close: onClose,
            deactivate: onDeactivate,
            //refresh: onRefresh,
            //animation: { open: { effects: getEffects(), duration: 500 }, close: { effects: getEffects(), reverse: true } },
            resize: false,
            //dragstart: onDragStart,
            //dragend: onDragEnd
        });

        window.data("kendoWindow").center().open();//.content(content);
        window.data("kendoWindow").pin();

        if (callbackFunc != null) {
            callbackFunc(window);
        }
        return window;
    }
    CRMPopupModal.prototype.close = function (isDestroy) {
        if (modal != null && modal.data("kendoWindow") != null) {
            destroy = isDestroy;
            modal.data("kendoWindow").close();
        }
    }
    CRMPopupModal.prototype.closeIfError = function () {
        this.close();
        hideMask();
    }
    CRMPopupModal.prototype.pin = function (window) {
        window.data("kendoWindow").pin();
    }
    CRMPopupModal.prototype.unpin = function (window) {
        window.data("kendoWindow").unpin();
    }
    CRMPopupModal.prototype.setOptions = function (options) {
        if (modal != null) {
            modal.data("kendoWindow").setOptions(options);
        }
    }
    CRMPopupModal.prototype.getModal = function () {
        return modal;
    }
    CRMPopupModal.prototype.getDataWindow = function () {
        if (modal != null && modal.length > 0) {
            return modal.data("kendoWindow");
        }
        return null;
    }
    CRMPopupModal.prototype.setEventCallback = function (eventName, callback) {
        //clog(eventName + " name");
        var dataWindow = this.getDataWindow();
        if (dataWindow != null) {
            var originalEvent = null;
            switch (eventName) {
                case "open":
                    originalEvent = dataWindow.open;
                    dataWindow.open = function () {
                        originalEvent.call(dataWindow);
                        callback(this);
                    };
                    break;
                case "close":
                    originalEvent = dataWindow.close;
                    dataWindow.close = function () {
                        originalEvent.call(dataWindow);
                        callback(this);
                    };
                    break
                case "activate":
                    originalEvent = dataWindow.open;
                    dataWindow.activate = function () {
                        //clog("exe");
                        originalEvent.call(dataWindow);
                        callback(this);

                    };
                    //clog("get in");
                    break;
                case "deactivate":
                    originalEvent = dataWindow.deactivate;
                    dataWindow.deactivate = function () {
                        originalEvent.call(dataWindow);
                        callback(this);
                    };
                    break;
                case "refresh":
                    originalEvent = dataWindow.refresh;
                    dataWindow.refresh = function () {
                        originalEvent.call(dataWindow);
                        callback(this);
                    };
                    break;
                case "dragstart":
                    originalEvent = dataWindow.dragstart;
                    dataWindow.dragstart = function () {
                        originalEvent.call(dataWindow);
                        callback(this);
                    };
                    break;
                case "dragend":
                    originalEvent = dataWindow.dragend;
                    dataWindow.dragend = function () {
                        originalEvent.call(dataWindow);
                        callback(this);
                    };
                    break;
            }
        }
    }

    function getEffects() {
        var animateOpacity = "fadeIn";
        //1: scale problem
        //return "expand:vertical " + animateOpacity;

        //2
        return "slideIn:down " + animateOpacity;

        //3: 1: scale problem
        return "toggle" + animateOpacity;

        //0
        //return "zoom " + animateOpacity;
        //return "";
    }

    /*--- function event of kendo window ---*/
    function onOpen(e) {
        //clog("event :: open");
    }

    function onClose(e) {
        //clog("event :: close");
        //hideMask();
    }

    function onActivate(e) {
        //clog("event :: activate");
        modalMask();
    }

    function onDeactivate(e) {
        //clog("event :: deactivate");
        hideMask();
        this.destroy();
        //if (destroy == true) {
        //    clog("destroy: true");
        //    this.destroy();
        //    destroy = false;
        //}
    }

    function onRefresh(e) {
        //clog("event :: refresh");
    }

    function onResize(e) {
        //clog("event :: resize");
    }

    function onDragStart(e) {
        //clog("event :: dragstart");
    }

    function onDragEnd(e) {
        //clog("event :: dragend");
    }

}

/*--- Dialog (Message box, confirm box) ---*/
function CRMDialog(maskCssClass) {
    var dialog = null;
    var mask = null;
    var destroy = true;
    if (mask == null) {
        createMask();
    }
    showMask();

    function createMask() {
        mask = $("div#claw-dialog-mask-window");
        if (mask.length == 0) {
            var newdiv = document.createElement("div");
            newdiv.setAttribute("id", "claw-dialog-mask-window");
            $("body").append(newdiv);
            mask = $(newdiv);
            mask.attr("style", "position: fixed; top: 0; left: 0; height: 100%; width: 100%; display: none;z-index: 9999;background-color:#390F3D;opacity:0.6;");
        }
        if (mask.length > 0 && maskCssClass != null) {
            mask.addClass(maskCssClass);
        }
        //clog("create mask");
        //var newdiv = document.createElement("div");
        //mask = $(newdiv);
        //mask.attr("style", "position: fixed; top: 0; left: 0; height: 100%; width: 100%; background: rgba(255,255,255,.7); display: none;z-index: 100000;background-color:#390F3D;opacity:0.6;");
        //if (maskCssClass != null) {
        //    mask.addClass(maskCssClass);
        //}
        //$("body").append(newdiv);
    }
    function showMask() {
        //var mask = $("#mask-loading");
        if (mask != null) {
            mask.removeAttr("style");
            mask.attr("style", "position: fixed; top: 0; left: 0; height: 100%; width: 100%; display: none;z-index: 9999;background-color:#390F3D;opacity:0.6;");
            mask.show();
        };
    }
    function hideMask() {
        if (mask != null) {
            mask.hide();
            mask.removeAttr("class");
            //mask.removeAttr("class");
        };
    }
    function modalMask() {
        //var mask = $("#mask-loading");
        if (mask != null) {
            //clog("modalMask");
            mask.css("background-image", "none");
            if (mask.is(":visible") == false) {
                mask.show();
            }
        };
    }

    CRMDialog.prototype.showDialog = function (title, content, width, height, onChangeClose, callbackFunc) {
        destroy = true;
        var windowElementSelector = "<div />";
        if (width == null) {
            width = 350;
        }
        if (height == null) {
            height = 150;
        }
        if (title == null) {
            title = "Message";
        }
        dialog = $(windowElementSelector).kendoWindow({
            width: width,
            height: height,
            modal: true,
            title: title,
            actions: ["Minimize", "Close"],
            visible: false,
            activate: onActivate,
            //open: onOpen,
            close: onChangeClose,
            deactivate: onDeactivate,
            //refresh: onRefresh,
            //animation: { open: { effects: getEffects(), duration: 500 }, close: { effects: getEffects(), reverse: true } },
            resizable: false,
            //resize: onResize,
            //dragstart: onDragStart,
            //dragend: onDragEnd
        });
        dialog.data("kendoWindow").center().open().content(content);
        dialog.data("kendoWindow").pin();

        if (callbackFunc != null) {
            callbackFunc(window);
        }
        return dialog;
    }

    CRMDialog.prototype.close = function (isDestroy) {
        if (dialog != null && dialog.data("kendoWindow") != null) {
            destroy = isDestroy;
            dialog.data("kendoWindow").close();
        }
    }
    CRMDialog.prototype.pin = function (window) {
        window.data("kendoWindow").pin();
    }
    CRMDialog.prototype.unpin = function (window) {
        window.data("kendoWindow").unpin();
    }

    CRMDialog.prototype.setOptions = function (options) {
        if (dialog != null) {
            dialog.data("kendoWindow").setOptions(options);
        }
    }

    function getEffects() {
        var animateOpacity = "fadeIn";
        //1: scale problem
        //return "expand:vertical " + animateOpacity;

        //2
        return "slideIn:down " + animateOpacity;

        //3: 1: scale problem
        return "toggle" + animateOpacity;

        //0
        //return "zoom " + animateOpacity;
        //return "";
    }

    /*--- function event of kendo window ---*/
    function onOpen(e) {
        //clog("event :: open");
    }

    function onClose(e) {
        //clog("event :: close");
        //hideMask();
    }

    function onActivate(e) {
        //clog("event :: activate");
        modalMask();
    }

    function onDeactivate(e) {
        //clog("event :: deactivate");
        hideMask();
        this.destroy();
        //if (destroy == true) {
        //    clog("destroy: true");
        //    this.destroy();
        //}
    }

    function onRefresh(e) {
        //clog("event :: refresh");
    }

    function onResize(e) {
        //clog("event :: resize");
    }

    function onDragStart(e) {
        //clog("event :: dragstart");
    }

    function onDragEnd(e) {
        //clog("event :: dragend");
    }

}