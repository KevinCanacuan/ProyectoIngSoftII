var intIdTabla = 0;
var strOptionsContainer;
fntGetCbxContainerUpload();
$(function () {
    var intLastsel;
    jQuery("#rowed5").jqGrid({
        datatype: "local", height: 250, colNames: ['ID', 'Nombre', 'Nombre Alterno', 'Tipo Archivo', 'Empresa', 'Contenedor', 'Aplicativo', 'Módulo', 'Fecha Carga', 'Año Referencial', 'Mes Referencial', 'Path', 'Tags', 'Descripción'],
        colModel: [
        {name: 'Id', index: 'Id', width: 30, sorttype: "int", editable: false},
        { name: 'FileName', index: 'FileName', width: 150, editable: false, editoptions: { size: "20", maxlength: "30" }},
        { name: 'AlterName', index: 'AlterName', width: 100, editable: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'Extension', index: 'Extension', width: 30, editable: false, editoptions: { size: "20", maxlength: "30" }},
        { name: 'Company', index: 'Company', width: 80, editable: false, editoptions: { size: "20", maxlength: "30" } },
        { name: 'Container', index: 'Container', width: 80, editable: true, edittype: 'select', editoptions: { value: strOptionsContainer } },
        {name: 'Application', index: 'Application', width: 70, editable: true, edittype: 'select',editoptions: { value: "1:baaniv;2:dms;3:zeuz" }},
        {name: 'Module', index: 'Module', width: 70, editable: true,editoptions: { size: "20", maxlength: "30" }},
        {name: 'DocumentUploadAt', index: 'DocumentUploadAt', width: 100, editable: false, editoptions: { size: "20", maxlength: "30" }},
        {name: 'ReferencialYear', index: 'ReferencialYear', width: 100, editable: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'ReferencialMonth', index: 'ReferencialMonth', width: 100, editable: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'Path', index: 'Path', width: 100, editable: true, hidden: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'Tags', index: 'Tags', width: 70, editable: true, editoptions: { size: "20", maxlength: "30" }},
        {name: 'Description', index: 'Description', width: 200, sortable: false, editable: true, edittype: "textarea",editoptions: { rows: "2", cols: "10" }}],
        multiselect: true,
        sortname: 'id',
        viewrecords: true,
        onSelectRow: function (id) {
            if (id && id !== intLastsel) {
                jQuery('#rowed5').jqGrid('restoreRow', intLastsel);
                jQuery('#rowed5').jqGrid('editRow', id, true);
                intLastsel = id;
            }
        },
    });
    var strFileName;
    var clsData;
    var dtmDate = new Date();
    var strDocumentUpload = dtmDate.getDate() + "/" + (dtmDate.getMonth() + 1) + "/" + dtmDate.getFullYear();
    $(function () {
        $("#fileInput").on("change", function () {
                var lstArchivos = document.getElementById("fileInput").files;
                if (lstArchivos.length>10) {
                    alert("Solo se permite subir 10 archivos");
                } else {
                    for (var i = 0; i < lstArchivos.length; i++) {
                        strFileName = lstArchivos[i].name;
                        var strPath = "c:\\ArchivosPDF\\" + lstArchivos[i].name;
                        var strTipoArchivo = fntEncontrarTipoArchivoStr(strFileName);
                        var strAltername = fntEliminarExtensionStr(strFileName);
                        intIdTabla++;
                        clsData = [{
                            Id: intIdTabla,
                            FileName: strFileName,
                            AlterName: strAltername,
                            Extension: strTipoArchivo,
                            Company: "maresa",
                            Container: $("#cbxContainerUpload").val(),
                            Application: $("#cbxAplicationUpload").val(),
                            Module: $("#txtNombreModulo").val(),
                            DocumentUploadAt: strDocumentUpload,
                            ReferencialYear: dtmDate.getFullYear(),
                            ReferencialMonth: dtmDate.getMonth() + 1,
                            Path: strPath,
                            Tags: $("#TagsFilt").val(),
                            Description: $("#DescriptionFilt").val()
                        }];
                        jQuery("#rowed5").jqGrid('addRowData', clsData.Id, clsData);
                    }

                }
                
                fntLimpiarFormularios();
        });
    });
});

function fntEncontrarTipoArchivoStr(cadena) {
    var strResult;
    for (var i = 0; i < cadena.length; i++) {
        if (cadena[i] == ".") {
            strResult = cadena.substring(i + 1);
        }
    }
    return strResult;
};

function fntLimpiarFormularios() {
    $("#cbxContainerUpload").val("Seleccione el Contenedor"),
    $("#cbxAplicationUpload").val("Seleccione aplicación"),
    $("#txtNombreModulo").val("");
    $("#TagsFilt").val("");
    $("#DescriptionFilt").val("");
    $("#AlterNameFilt").val("");
    
}

function fntEliminarExtensionStr(cadena) {
    var strResult;
    for (var i = 0; i < cadena.length; i++) {
        if (cadena[i] == ".") {
            strResult = cadena.substring(0, i);
        }
    }
    return strResult;
};


$("#txtReferencialYear").change(function () {
    var strDate = $("#txtReferencialYear").val();
    var strMonth = parseInt(strDate.substr(5, 6));
    var strYear = parseInt(strDate.substr(0, 4))
    var lstRowData = $("#rowed5").getDataIDs();
    for (var i = 0; i < lstRowData.length; i++) {
        $("#rowed5").jqGrid('setRowData', lstRowData[i], { ReferencialYear: strYear});
        $("#rowed5").jqGrid('setRowData', lstRowData[i], { ReferencialMonth: strMonth });
    }
});

$("#btnExit").click(function () {
    window.location.href = "/Container/Container";
    $("#MyModalNewUpload").modal("hide");
});



$("#cbxContainerUpload").change(function () {
    var lstRowData = $("#rowed5").getDataIDs();
    for (var i = 0; i < lstRowData.length; i++) {
        $("#rowed5").jqGrid('setRowData', lstRowData[i], { Container: $("#cbxContainerUpload").val() });
    }
});

$("#cbxAplicationUpload").change(function () {
    var lstRowData = $("#rowed5").getDataIDs();
    for (var i = 0; i < lstRowData.length; i++) {
        $("#rowed5").jqGrid('setRowData', lstRowData[i], { Application: $("#cbxAplicationUpload").val() });
    }
});

$("#txtNombreModulo").change(function () {
    var lstRowData = $("#rowed5").getDataIDs();
    if (lstRowData!=0) {
        for (var i = 0; i < lstRowData.length; i++) {
            $("#rowed5").jqGrid('setRowData', lstRowData[i], { Module: $("#txtNombreModulo").val() });
            $("#rowed5").jqGrid('setRowData', lstRowData[i], { Id: i + 1 });
        }
    $("#txtNombreModulo").val("");
    }
});

$("#AlterNameFilt").change(function () {
    var lstRowData = $("#rowed5").getDataIDs();
    if (lstRowData != 0) {
        for (var i = 0; i < lstRowData.length; i++) {
            $("#rowed5").jqGrid('setRowData', lstRowData[i], { AlterName: $("#AlterNameFilt").val() });
        }
        $("#AlterNameFilt").val("");
    }
});

$("#TagsFilt").change(function () {
    var lstRowData = $("#rowed5").getDataIDs();
    if (lstRowData != 0) {
        for (var i = 0; i < lstRowData.length; i++) {
            $("#rowed5").jqGrid('setRowData', lstRowData[i], { Tags: $("#TagsFilt").val() });
        }
        $("#TagsFilt").val("");
    }
});

$("#DescriptionFilt").change(function () {
    var lstRowData = $("#rowed5").getDataIDs();
    if (lstRowData != 0) {
        for (var i = 0; i < lstRowData.length; i++) {
            $("#rowed5").jqGrid('setRowData', lstRowData[i], { Description: $("#DescriptionFilt").val() });
        }
        $("#DescriptionFilt").val("");
    }
});

$("#DeleteRow").click(function () {
    var intRow = jQuery("#rowed5").jqGrid('getGridParam', 'selrow');
    var intI;
    if (intRow != null) {
        jQuery("#rowed5").jqGrid('delRowData', intRow);
        var lstRowData = $("#rowed5").getDataIDs();
        for (intI = 0; intI < lstRowData.length; intI++) {
           
            $("#rowed5").jqGrid('setRowData', lstRowData[intI], { Id: intI + 1 });
        }
    }
    else {
        alert("Por favor seleccione un archivo para borrar");
    }
    intIdTabla = intI;   
});


$("#DeleteGrid").click(function () {
    $("#rowed5").jqGrid("clearGridData");
    intIdTabla = 0;
});

function fntGetCbxContainerUpload() {
    var intNumber = 1;
    var strContainers;
    $("#cbxContainerUpload option").each(function () {
        if ($(this).val() != "") {
            strContainers += intNumber + ":" + $(this).val() + ";";
            intNumber++;
        }
    });
    strOptionsContainer = strContainers.substring(9);
};

function fntValidationExtensionFilesBln() {
    var lstValidationExtensionFile = $("#rowed5").jqGrid("getCol", "Extension");
    for (var i = 0; i < lstValidationExtensionFile.length; i++) {
        if (lstValidationExtensionFile[i] == "pdf") {
            return true;
        } else {
            $("#rowed5").jqGrid("setCell", 1, "Extension", "", {background:"black"});
            return false;
        }
    }
};

function fntValidationAplicationContainerFilesBln() {
    var lstValidationAplicationFile = $("#rowed5").jqGrid("getCol", "Application");
    var lstValidationContainerFile = $("#rowed5").jqGrid("getCol", "Container");
    var lstValidationModuleFile = $("#rowed5").jqGrid("getCol", "Module");
    for (var i = 0; i < lstValidationContainerFile.length; i++) {
        if (lstValidationContainerFile[i] == "Seleccione el Contenedor" || lstValidationAplicationFile[i] == "Seleccione aplicación" || lstValidationModuleFile =="") {
            return false;
        } else {
            return true;
        }
    }
};


$("#btnUploadFiles").click(function () {
    
    var intRowKey = $("#rowed5").getGridParam("selrow");
    if (!intRowKey) {
        alert("Debe seleccionar todos los archivos");
    } else {
        if (fntValidationAplicationContainerFilesBln() != true || fntValidationExtensionFilesBln() != true) {
            alert("Debe seleccionar un campo válido");
        } else {
            var lstObjectData = jQuery('#rowed5').jqGrid('getGridParam', 'data');
            $.ajax({
                type: "POST",
                url: "/Container/fntUploadFiles",
                data: { lstDocuments: lstObjectData },
                success: function (result) {
                    alert(result);
                    window.location.href = "/Container/Container";
                    $("#MyModalUpload").modal("hide").remove();

                }
            });
            $("#wait").show();
        }
    }

    
    
});

