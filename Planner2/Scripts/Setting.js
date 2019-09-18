 

var DataGrid = $("#DataGrid").dxDataGrid({
    dataSource: [],
    showRowLines: true,
    showBorders: true,
    autoExpandAll: true,
    paging: {
        enabled: true,
        pageSize: 100
    },
    filterRow: {
        visible: true,
        applyFilter: "auto"
    },
    columnChooser: {
        enabled: true
    },
    scrolling: {
        mode: "standard"
    },
    pager: {
        showPageSizeSelector: true,
        allowedPageSizes: [100, 200, 500, 1000],
        showNavigationButtons: true
    },
    allowColumnResizing: true,
    columnFixing: {
        enabled: true
    },
    hoverStateEnabled: true,
    headerFilter: {
        visible: true
    },
    wordWrapEnabled: true,
    searchPanel: {
        visible: true,
        width: 240,
        placeholder: "Search..."
    },
    columnsAutoWidth: true,
    allowColumnReordering: true,
    rowAlternationEnabled: true,
    "export": {
        enabled: true,
        fileName: "DataList",
        allowExportSelectedData: true
    },
    editing: {
        mode: "row",
        allowUpdating: true,
          useIcons: true
    },
    filterPanel: { visible: true },
    summary: {
        totalItems: [{
            column: "KeyID",
            summaryType: "count"
        }]
    },
    columns:
        [
           
            {
                dataField: "Type",
                caption: "Type",
            }, 
            {
                dataField: "KeyID",
                caption: "KeyID",
                allowEditing: false
            },
            {
                dataField: "Value",
                caption: "Value",
                editorOptions: {
                    showClearButton: true
                },
             } 
        ],

     

    //update
    onRowUpdating: function (e) {

        e.newData.KeyID = e.oldData.KeyID;
        $.ajax({
            url: '/Settings/Update_StaffOn',
            type: 'POST',
            dataType: 'json',
            async: false,
            data: e.newData,
            success: function (data) {
                if (data.result == true) {
                    DevExpress.ui.notify("Dữ liệu này đã được chỉnh sửa thành công!", "success");
                }
                else {
                    DevExpress.ui.notify("Dữ liệu này không thể được cập nhật!", "error");
                }
            },
            error: function (err) {
                DevExpress.ui.notify("Dữ liệu này không thể được cập nhật!", "error");
            }
        });
    },


       onCellPrepared: function onCellPrepared(e) {
        if (e.rowType == "header") {
            e.cellElement.css("text-align", "left");
            e.cellElement.addClass('headerTree');
            e.cellElement.css("vertical-align", "middle");
        }

        if (e.rowType == "data") {
            e.cellElement.css("text-align", "left");


            e.cellElement.css("vertical-align", "middle");
        }
    },
});

$(function () {
    LoadData_Data();
});

function LoadData_Data() {
    $("#loadPanel").dxLoadPanel({
        closeOnOutsideClick: false,
        message: "Xin chờ ...",
        visible: true,
        position: {
            of: "#DataGrid",
            at: "center",
            my: "center"
        },
        shading: true
    });

    $.ajax({
        url: '/Settings/GetData_Data',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var datagrid = $("#DataGrid").dxDataGrid("instance");
            datagrid.option("dataSource", data);

            $("#loadPanel").dxLoadPanel({
                closeOnOutsideClick: false,
                message: "Xin ch? ...",
                visible: false,
                position: {
                    of: "#DataGrid",
                    at: "center",
                    my: "center"
                },
                shading: true
            });

        },
        error: function (err) {
        }
    });
}


