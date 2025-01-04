function GetHash(data) {
    $.ajax({
        method: "GET",
        url: "/home/GetHash",
        data: { text: data }
    })
    .done(function (result) {
        $("#hash").val(result);
    });
}


function MineBlock(idForm) {
    var blockIndex = $('#BlockIndex').val();
    var nonce = $('#Nonce').val();
    var data = $('#Data').val();
    $.ajax({
        method: "GET",
        url: "/home/Mine",
        data: { BlockIndex: blockIndex, Nonce: nonce, Data: data }
    })
    .done(function (result) {
        $("#Hash").val(result.hash);
        $("#Nonce").val(result.nonce);
        ValidateHash(idForm);
    });
}


function GetBlockchain() {
    $.ajax({
        method: "GET",
        url: "/home/GetBlockchain",
        data: {}
    })
    .done(function (result) {
        AssembleBlockchain(result);
    });
}


function AddBlock() {
    $.ajax({
        method: "GET",
        url: "/home/AddBlock",
        data: { }
    })
    .done(function (result) {
        AssembleBlockchain(result);
    });
}


function InvalidateBlockchain(idForm) {
    var form = $(idForm);
    var formId = form.attr("id");
    var id = formId.replace("form", "");
    var blockIndex, nonce, data;
    form.each(function () {
        data = $("#Data"+id).val();
        blockIndex = $("#BlockIndex"+id).val();
        nonce = $("#Nonce"+id).val();
    });
    $.ajax({
        method: "GET",
        url: "/home/InvalidateBlockchain",
        data: { BlockIndex: blockIndex, Nonce: nonce, Data: data, idList: id }
    })
    .done(function (result) {
        AssembleBlockchain(result);
    });
}


function RemovePreviousBlockChain() {
    $.ajax({
        method: "GET",
        url: "/home/RemovePreviousBlockChain",
        data: { }
    })
    .done(function (result) {
        AssembleBlockchain(result);
    });
}


function MineBlockchain(idForm) {
    var form = $(idForm);
    var formId = form.attr("id");
    var id = formId.replace("form", "");
    var blockIndex, nonce, data;
    form.each(function () {
        data = $("#Data" + id).val();
        blockIndex = $("#BlockIndex" + id).val();
        nonce = $("#Nonce" + id).val();
    });
    $.ajax({
        method: "GET",
        url: "/home/MineBlockchain",
        data: { BlockIndex: blockIndex, Nonce: nonce, Data: data, idList: id }
    })
    .done(function (result) {
        AssembleBlockchain(result);
    });
}

function InvalidateHash(object) {
    $(object).css("background-color", "#ECD0AF");
}


function ValidateHash(object) {
    $(object).css("background-color", "#D1F3B0");    
}

function AssembleBlockchain(data) {
    $("#Blockchain").html("");
    var formatHtml = "";
    var template = "";
    var ind = 0;
    while (ind < data.length) {
        template = '<div class="row" style="padding:10px;">';
        template += TemplateBlockchain(ind, data[ind]);
        if ((ind + 1) < data.length) {
            ind = ind + 1;
            template += TemplateBlockchain(ind, data[ind]);
        }
        ind += 1;
        template += '</div>';
        formatHtml += template;
    }
    $("#Blockchain").html(formatHtml);
}


function TemplateBlockchain(index, data) {
    var template = "";
    var form = "form" + index;
    var formId = "'#form" + index + "'";
    var style = "";
    if (data.isValid) 
        style = "background-color:#D1F3B0;padding:10px;border-radius:5px;";
    else
        style = "background-color:#ECD0AF;padding:10px;border-radius:5px;";
    template += '<div class="col-md-6">';
    template += '  <form class="form-horizontal" id="' + form + '" style="' + style + '">';
    template += '    <div class="form-group">';
    template += '      <label class="control-label"><b>Block</b></label>';
    template += '      <div class="input-group">';
    template += '        <div class="input-group-prepend">';
    template += '          <span class="input-group-text">#</span>';
    template += '        </div>';
    template += '        <input value="' + data.blockIndex + '" class="form-control" id="BlockIndex' + index + '" type="number" onchange="InvalidateBlockchain(' + formId + ')" />';
    template += '      </div>';
    template += '	 </div>';
    template += '    <div class="form-group">';
    template += '      <label class="control-label"><b>Nonce</b></label>';
    template += '      <input value="' + data.nonce + '" class="form-control" id="Nonce' + index + '" type="number" onchange="InvalidateBlockchain(' + formId + ')" />';
    template += '	 </div >';
    template += '    <div class="form-group">';
    template += '      <label class="control-label"><b>Data</b></label>';
    template += '      <textarea class="form-control" id="Data' + index + '" rows = "3" onchange="InvalidateBlockchain(' + formId + ')">' + data.data + '</textarea >';
    template += '	 </div>';
    template += '    <div class="form-group">';
    template += '      <label class="control-label" for="PreviousHash"><b>Previous</b></label>';
    template += '        <input value="' + data.previousHash + '" class="form-control" id="PreviousHash' + index + '" type="text" disabled />';
    template += '	 </div>';
    template += '    <div class="form-group">';
    template += '      <label class="control-label"><b>Hash</b></label>';
    template += '      <input value="' + data.hash + '" class="form-control" id="Hash' + index + '" type="text" disabled />';
    template += '	 </div>';
    template += '    <div class="form-group">';
    template += '      <button type="button" class="btn btn-info" onclick="MineBlockchain(' + formId + ')">Mine</button>';
    template += '	 </div>';
    template += '  </form>';
    template += '</div>';
    return template;
}
          
