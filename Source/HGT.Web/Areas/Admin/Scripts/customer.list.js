
$(document).ready(function () {
    gridInit();
});

function gridInit() {
    var dataSource = new kendo.data.DataSource({
        transport: {
            prefix: "",
            read: {
                url: "/admin/Customer/List/",
                dataType: "json",
                type: "POST"
            }
        },
        pageSize: 10,
        page: 1,
        sortable: true,
        total: 0,
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        serverGrouping: true,
        serverAggregates: true,
        type: "aspnetmvc-ajax",
        filter: [],
        schema: {
            data: "Data",
            total: "Total",
            errors: "Errors",
            model:
                {
                    id: "UserId",
                    fields: {
                        "UserId": { "type": "number" },
                        "UserName": { "type": "string" },
                        "FullName": { "type": "string" },
                        "Email": { "type": "string" },
                        "Active": { "type": "boolean" },
                        "RoleName": { "type": "string" },
                        "ShowDelete": { "type": "boolean" }
                    }
                }
        }
    });
    dataSource.bind("error", dataSource_error);
    gridConfigInit(dataSource);
}

function dataSource_error(e) {
    if (e.status == "error") {
        loadError();
    }
    //console.log(e);
}

function loadError() {
    var msg = $("#error-page-load").val();
    if (msg == null || msg == "") {
        msg = "Loading Error!";
    }
    admincommon.showMessageDefault(msg, "Error");
}

function onDataBound(e) {
    $(".k-grid-Delete").addClass("delete_fr");
    $(".k-grid-Edit").addClass("edit_fr");
    $(".k-grid-Refresh").addClass("icon_menu-square_alt2");
    if (this.dataSource.data().length < 1) {
        this.pager.element.hide();
    }
    else this.pager.element.show();
    addCssForTable();
}

function gridConfigInit(dataSource) {
    var grid = $("#grid").data("kendoGrid");
    var temp = "<div class=\"btnEdit  txt_align_c\">"
                 + "<button class=\"btn btn-success\" onClick=\"ShowDetail(#= UserId #)\" ><i class=\"fa fa-search-plus\"></i></button>"
                 + "#if(showUpdate) {#<button class=\"btn btn-info\" onClick=\"EditUser(#= UserId #)\" ><i class=\"fa fa-edit\"></i></button>#}#"
                 + "#if(ShowDelete) {#<button class=\"btn btn-danger\" onClick=\"DeleteUser(#= UserId #)\" ><i class=\"fa fa-trash-o\"></i></button>#}#"

                 + "</div>";

    //grid.destroy();
    jQuery("#grid").kendoGrid({
        "dataBound": onDataBound,
        "columns": [
         {
             "title": "FullName", "attributes": { "id": "FullName", "name": "FullName" },
             "field": "FullName", "filterable": {}, "encoded": true, "editor": null,
             sortable: true,

         },
         {
             "title": "UserName", "attributes": { "id": "UserName", "name": "UserName" },
             "field": "UserName", "filterable": {}, "encoded": true, "editor": null,
             sortable: true,
         },
        {
            "title": "Email", "attributes": { "id": "Email", "name": "Email", "class": "hidden-610" },
            "field": "Email", "filterable": {}, "encoded": true, "editor": null,
            sortable: true,
        },
        {
            "title": "Active", "attributes": { "id": "Active", "name": "Active", "class": "hidden-410 center" },
            "template": "<input type=\"checkbox\" id=\"Selected_active\"  #= Active ? checked='checked' : '' #  disabled=\"disabled\" />",
            "field": "Active", "filterable": {}, "encoded": true, "editor": null,
            sortable: false,
        },
        {
            "title": "RoleName", "attributes": { "id": "RoleName", "name": "RoleName", "class": "hidden-480" },
            "field": "RoleName", "filterable": {}, "encoded": true, "editor": null,
            sortable: false,
        },
         {
             "template": temp,
             "field": null,
             "sortable": false,
             "filterable": false,
             "encoded": true,
             "editor": null
         }
        ],
        "pageable": {
            "pageSizes": [10, 20, 30, 50, 100], "buttonCount": 10
            , messages: {
                display: $('#showing-item-count').val(),
                itemsPerPage: $('#item-per-page-mess').val()
            }
        },
        "scrollable": false,
        sortable: true,
        //"editable": {
        //    "confirmation": "Are you sure you want to delete this record?",
        //    "mode": "popup",
        //    "template": "\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"Id\"\u003eId\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" data-val=\"true\" data-val-number=\"The field Id must be a number.\" data-val-required=\"The Id field is required.\" id=\"Id\" name=\"Id\" type=\"number\" value=\"0\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"Id\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"UserName\"\u003eUserName\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" data-val=\"true\" data-val-length=\"The UserName length must be between 3 to 50 characters.\" data-val-length-max=\"50\" data-val-length-min=\"3\" data-val-regex=\"UserName is invalid.\" data-val-regex-pattern=\"^[a-z0-9_-]{3,50}$\" data-val-required=\"The UserName field is required.\" id=\"UserName\" name=\"UserName\" type=\"text\" value=\"\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"UserName\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"FullName\"\u003eFull Name\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" data-val=\"true\" data-val-required=\"The Full Name field is required.\" id=\"FullName\" name=\"FullName\" type=\"text\" value=\"\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"FullName\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"Email\"\u003eEmail\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" data-val=\"true\" data-val-email=\"The Email field is not a valid e-mail address.\" data-val-required=\"The Email field is required.\" id=\"Email\" name=\"Email\" type=\"email\" value=\"\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"Email\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"CreatedDate\"\u003eUpdated Date\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" data-val=\"true\" data-val-date=\"The field Updated Date must be a date.\" data-val-required=\"The Updated Date field is required.\" id=\"CreatedDate\" name=\"CreatedDate\" type=\"datetime\" value=\"01.01.0001 00:00:00\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"CreatedDate\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"SiteId\"\u003eSiteId\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" data-val=\"true\" data-val-number=\"The field SiteId must be a number.\" data-val-required=\"The SiteId field is required.\" id=\"SiteId\" name=\"SiteId\" type=\"number\" value=\"0\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"SiteId\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"DisplayName\"\u003eDisplay name\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" data-val=\"true\" data-val-required=\"The Display name field is required.\" id=\"DisplayName\" name=\"DisplayName\" type=\"text\" value=\"\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"DisplayName\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"Password\"\u003ePassword\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line password\" data-val=\"true\" data-val-length=\"The Password length must be between 3 to 40 characters.\" data-val-length-max=\"40\" data-val-length-min=\"3\" id=\"Password\" name=\"Password\" type=\"password\" value=\"\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"Password\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"ConfirmPassword\"\u003eConfirm Password\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line password\" data-val=\"true\" data-val-equalto=\"Confirm Password must match!\" data-val-equalto-other=\"*.Password\" id=\"ConfirmPassword\" name=\"ConfirmPassword\" type=\"password\" value=\"\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"ConfirmPassword\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"UpdatedDate\"\u003eUpdated Date\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" data-val=\"true\" data-val-date=\"The field Updated Date must be a date.\" id=\"UpdatedDate\" name=\"UpdatedDate\" type=\"datetime\" value=\"\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"UpdatedDate\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"Active\"\u003eActive\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"check-box\" data-val=\"true\" data-val-required=\"The Active field is required.\" id=\"Active\" name=\"Active\" type=\"checkbox\" value=\"true\" /\u003e\u003cinput name=\"Active\" type=\"hidden\" value=\"false\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"Active\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e\u003cdiv class=\"editor-label\"\u003e\u003clabel for=\"AllRole\"\u003eAllRole\u003c/label\u003e\u003c/div\u003e\u003cdiv class=\"editor-field\"\u003e\u003cinput class=\"text-box single-line\" id=\"AllRole\" name=\"AllRole\" type=\"text\" value=\"\" /\u003e \u003cspan class=\"field-validation-valid\" data-valmsg-for=\"AllRole\" data-valmsg-replace=\"true\"\u003e\u003c/span\u003e\u003c/div\u003e",
        //    "window": {
        //        "title": "Edit", "modal": true,
        //        "draggable": true, "resizable": false
        //    },
        //    "create": true, "update": true, "destroy": true
        //},
        "dataSource": dataSource
    });
}

function addCssForTable() {
    $("td#Active").addClass('txt_align_c');
}

function DetailUser(id) {
    var siteUrl = "/admin/Customer/details/";
    $(location).attr('href', siteUrl + id);
}

function EditUser(id) {
    var siteUrl = "/admin/Customer/edit/";
    $(location).attr('href', siteUrl + id);
}


function DeleteUser(userId) {
    ConfirmMessageBox(null, null, function () {
        $.ajax({
            url: "/admin/Customer/delete",
            type: "POST",
            data: { id: userId },
            success: function (result) {
                if (result.Status != null && result.Status == 1) {
                    window.close()
                    $("#grid").data("kendoGrid").dataSource.read();
                }
                if (result.Message != null && result.Status == 0) {
                    MessageBox(result.Message, false);
                }
            }
        });
    }, true)

}