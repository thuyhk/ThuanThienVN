
$(document).ready(function () {
    gridInit();
});

function gridInit() {
    var dataSource = new kendo.data.DataSource({
        transport: {
            prefix: "",
            read: {
                url: "/admin/content/ListTemplate/",
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
                    id: "EmailTemplateId",
                    fields: {
                        "EmailTemplateId": { "type": "number" },
                        "Name": { "type": "string" },
                        "Subject": { "type": "string" }
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
}

function gridConfigInit(dataSource) {
    var grid = $("#grid").data("kendoGrid");
    var temp = "<div class=\"btnEdit txt_align_c\">"
                 + "<button class=\"btn btn-info\" onClick=\"EditUser(#= EmailTemplateId #)\" ><i class=\"fa fa-edit\"></i></button>"
                 + "</div>";

    //grid.destroy();
    jQuery("#grid").kendoGrid({
        "dataBound": onDataBound,
        "columns": [
         {
             "title": "Name", "attributes": { "id": "Name", "name": "Name" },
             "field": "Name", "filterable": {}, "encoded": true, "editor": null,
             sortable: true,

         },
         {
             "title": "Subject", "attributes": { "id": "Subject", "name": "Subject" },
             "field": "Subject", "filterable": {}, "encoded": true, "editor": null,
             sortable: true,
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
function EditUser(id) {
    var siteUrl = "/admin/content/editEmail/";
    $(location).attr('href', siteUrl + id);
}