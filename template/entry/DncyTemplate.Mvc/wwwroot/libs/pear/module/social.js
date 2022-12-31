layui.define(['table', 'jquery', 'element'], function (exports) {
    "use strict";

    var MOD_NAME = 'communication',
        $ = layui.jquery,
        element = layui.element;

    var pearCommunication = function (opt) {
        this.option = opt;
    };

    pearCommunication.prototype.render = function (opt) {
        //默认配置值
        var option = {
            elem: opt.elem,
            url: opt.url ? opt.url : false,
            height: opt.height,
            data: opt.data,
            click: opt.click
        }

        return new pearCommunication(option);
    }

    function createHtml(option) {

        var notice = '<li class="layui-nav-item" lay-unselect="">' +
            '<a href="#" class="notice layui-icon layui-icon-notice"><span class="layui-badge-dot"></span></a>' +
            '<div class="layui-nav-child layui-tab pear-notice" style="left: -200px;">';

        var noticeTitle = '<ul class="layui-tab-title">';

        var noticeContent = '<div class="layui-tab-content" style="height:' + option.height + ';overflow-x: hidden;">'

        var index = 0;

        // 根据 data 便利数据
        $.each(option.data, function (i, item) {
            console.log(option.data);
            if (index === 0) {

                noticeTitle += '<li class="layui-this">' + item.title + '</li>';

                noticeContent += '<div class="layui-tab-item layui-show">';

            } else {

                noticeTitle += '<li>' + item.title + '</li>';

                noticeContent += '<div class="layui-tab-item">';

            }

            $.each(item.children, function (i, note) {

                noticeContent += '<div class="pear-notice-item" notice-form="' + note.form + '" notice-context="' + note.context +
                    '" notice-title="' + note.title + '" notice-id="' + note.id + '">' +
                    '<img src="' + note.avatar + '">' +
                    '<span>' + note.title + '</span>' +
                    '<span>' + note.time + '</span>' +
                    '</div>';

            })

            noticeContent += '</div>';

            index++;
        })

        noticeTitle += '</ul>';

        noticeContent += '</div>';

        notice += noticeTitle;

        notice += noticeContent;

        notice += '</div></li>';

        return notice;
    }

    exports(MOD_NAME, new pearCommunication());
})
