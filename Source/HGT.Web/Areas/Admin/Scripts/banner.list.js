$(document).ready(function () {
    gridInit();
});

function gridInit() {
    var dataSource = new kendo.data.DataSource({
        transport: {
            prefix: "",
            read: {
                url: "/admin/banner/list/",
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
                    id: "Id",
                    fields: {
                        "Id": { "type": "number" },
                        "Title": { "type": "string" },
                        "Link": { "type": "string" },
                        "ImagePath": { "type": "string" },
                        "DisplayOrder": { "type": "int" },
                        "IsActive": { "type": "boolean" }
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
    if (this.dataSource.data().length < 1) {
        this.pager.element.hide();
        this.thead.hide();
        $(".t-no-data").show();
    }
    else {
        this.thead.show();
        $(".t-no-data").hide();
        gridCustom();
        this.pager.element.show();
    }
}

function gridConfigInit(dataSource) {
    var grid = $("#grid").data("kendoGrid");
    var temp = "<div class=\"txt-center\">"
                 + "<button class=\"btn btn-info\" onClick=\"edit(#= Id #)\" ><i class=\"fa fa-edit\"></i></button>"
                 + "<button class=\"btn btn-danger\" onClick=\"confirmDelete(#= Id #)\" ><i class=\"fa fa-trash-o\"></i></button>"
                 + "</div>";

    //grid.destroy();
    jQuery("#grid").kendoGrid({
        "dataBound": onDataBound,
        "columns": [
         {
             "title": "Title", "attributes": { "id": "Title", "name": "Title" },
             "field": "Title", "filterable": {}, "encoded": true, "editor": null,
             sortable: true,

         },
         {
             "title": "ImagePath", "attributes": { "id": "ImagePath", "name": "ImagePath", "class": "txt-center" },
             "template": "<a href=\"#= ImagePath# \" title=\"#= Title#\"  class=\"preview\"><img id='thumb_#:Id#' class=\"img_thumb\" src=\"#= ImagePath# \" /></a>",
             "field": "ImagePath", "filterable": {}, "encoded": true, "editor": null,
             sortable: false,
         },
         {
             "title": "Link", "attributes": { "id": "Link", "name": "Link" },
             "template": "<a  href=\"#= Link#\" target=\"_blank\">#= Link #</a>",
             "field": "Link", "filterable": {}, "encoded": true, "editor": null,
             sortable: false,
         },
         {
             "title": "DisplayOrder", "attributes": { "id": "DisplayOrder", "name": "DisplayOrder", "class": "txt-center" },
             "field": "DisplayOrder", "filterable": {}, "encoded": true, "editor": null,
             sortable: true,
         },
        {
            "title": "IsActive", "attributes": { "id": "IsActive", "name": "IsActive", "class": "txt-center" },
            "template": "<input type=\"checkbox\" class=\"multi\" id=\"Selected_active\"  #= IsActive ? checked='checked' : '' #  disabled=\"disabled\" /><span class=\"lbl\" disabled=\"disabled\"></span>",
            "field": "IsActive", "filterable": {}, "encoded": true, "editor": null,
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
        "dataSource": dataSource
    });
}

function edit(id) {
    var siteUrl = "/admin/banner/input/";
    $(location).attr('href', siteUrl + id);
}

function confirmDelete(Id) {
    ConfirmMessageBox(null, null, function () {
        $.ajax({
            url: "/admin/banner/delete",
            type: "POST",
            data: { id: Id },
            success: function (result) {
                if (result.Status != null && result.Status == 0) {
                    window.close()
                }
                if (result.Message != null) {
                    MessageBox(result.Message, false);
                    $("#grid").data("kendoGrid").dataSource.read();
                }
            }
        });
    }, true)
}

function gridCustom() {
    $('#grid').find(".preview[href='']").hide();
    initImagePreview();
}
