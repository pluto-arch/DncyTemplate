
let layer;

layui.use(["layer"],
    function() {
        layer = layui.layer;
    });


function showMsg(msg) {
    layer.msg(msg);
}